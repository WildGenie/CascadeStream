// Decompiled with JetBrains decompiler
// Type: CSVideoServer.VideoServer
// Assembly: CSVideoServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: 28813BD9-90C6-4DB5-ADB3-76EB1EEB4BDD
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\video\CSVideoServer.exe

using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using BasicComponents;
using CS.DAL;
using CS.VideoSources.BaseSources;
using CS.VideoSources.Basler;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSVideoServer
{
  internal class VideoServer
  {
    private readonly IDictionary<Guid, CS.VideoSources.Core.IVideoSource> _videoSources = (IDictionary<Guid, CS.VideoSources.Core.IVideoSource>) new ConcurrentDictionary<Guid, CS.VideoSources.Core.IVideoSource>();
    private readonly ILog _performanceLogger = LogManager.GetLogger("PerfLog");
    private readonly ILog _logger = LogManager.GetLogger(typeof (VideoServer));
    private const int CheckDevicesTimeout = 4000;
    private Thread _checkDevicesThread;
    private bool _isLoaded;
    private volatile bool _signalToStop;

    public ILog Logger
    {
      get
      {
        return this._logger;
      }
    }

    public IDictionary<Guid, CS.VideoSources.Core.IVideoSource> VideoSources
    {
      get
      {
        return this._videoSources;
      }
    }

    public bool ServerState { get; set; }

    public string ImageFolder { get; set; }

    public string ApplicationFolder { get; set; }

    public Guid Id { get; private set; }

    public bool IsLoaded
    {
      get
      {
        return this._isLoaded;
      }
    }

    public CommonContracts.VideoFrame GetLastFrameFromDevice(Guid devId, bool deleteFrame)
    {
      try
      {
        this._performanceLogger.Info((object) string.Format("Request DevId = {0}", (object) devId));
        if (!this.VideoSources.ContainsKey(devId))
          return (CommonContracts.VideoFrame) null;
        CS.VideoSources.Core.IVideoSource videoSource = this.VideoSources[devId];
        if (!videoSource.IsRunning)
          return (CommonContracts.VideoFrame) null;
        using (CS.VideoSources.Core.VideoFrame currentFrame = videoSource.GetCurrentFrame(deleteFrame))
        {
          if (currentFrame == null)
            return (CommonContracts.VideoFrame) null;
          this._performanceLogger.Info((object) string.Format("Response DevId = {0} FrameId = {1}", (object) devId, (object) currentFrame.Index));
          byte[] frameBytes = VideoServer.GetFrameBytes(currentFrame);
          return new CommonContracts.VideoFrame()
          {
            Frame = frameBytes,
            FrameIndex = currentFrame.Index,
            Date = DateTimeOffset.Now
          };
        }
      }
      catch (Exception ex)
      {
        this._logger.Error((object) ex);
      }
      return (CommonContracts.VideoFrame) null;
    }

    private static byte[] GetFrameBytes(CS.VideoSources.Core.VideoFrame frame)
    {
      Bitmap image = frame.Image;
      if (image == null)
        return (byte[]) null;
      byte[] numArray;
      if (image.PixelFormat == PixelFormat.Format8bppIndexed)
      {
        numArray = JpegCompression.ConvertToJpeg(image, 70);
      }
      else
      {
        using (Bitmap bmp = Grayscale.CommonAlgorithms.Y.Apply(image))
          numArray = JpegCompression.ConvertToJpeg(bmp, 70);
      }
      return numArray;
    }

    public void Load(Guid serverId)
    {
      this._logger.Info((object) "Starting VideoServer");
      if (BcVideoServer.LoadById(serverId) == null)
      {
        this._logger.ErrorFormat("Server with id {0} not found. Service will be stopped", (object) serverId);
        throw new ArgumentException();
      }
      this.Id = serverId;
      Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
      try
      {
        this._logger.Info((object) "Loading devices from database");
        this.LoadDevices();
        this.StartProcessing();
      }
      catch (Exception ex)
      {
        this._logger.Error((object) "Error loading server", ex);
      }
    }

    public void StopServer()
    {
      this._logger.Info((object) "Stopping server");
      List<Task> list = new List<Task>();
      foreach (CS.VideoSources.Core.IVideoSource videoSource in (IEnumerable<CS.VideoSources.Core.IVideoSource>) this.VideoSources.Values)
      {
        CS.VideoSources.Core.IVideoSource source = videoSource;
        list.Add(Task.Factory.StartNew((Action) (() => this.StopAndDisposeSource(source))));
      }
      list.Add(Task.Factory.StartNew((Action) (() =>
      {
        this._logger.Info((object) "Stopping devices check thread");
        this._signalToStop = true;
        if (this._checkDevicesThread != null && this._checkDevicesThread.IsAlive && !this._checkDevicesThread.Join(4500))
          this._checkDevicesThread.Abort();
        this._logger.Info((object) "Devices check thread stopped successfully");
      })));
      Task.WaitAll(list.ToArray());
      this.VideoSources.Clear();
      this._logger.Info((object) "Server stopped");
    }

    public void TryAddOrUpdateVideoSource(BcDevices device)
    {
      this.TryToRemoveVideoSource(device.Id);
      string connectionString = VideoServer.GetDeviceConnectionString(device);
      IReadOnlyDictionary<string, string> deviceOptions = VideoServer.GetDeviceOptions(device);
      CS.VideoSources.Core.IVideoSource videoSource = VideoServer.CreateVideoSource(device.Type, connectionString, device.Login, device.Password, deviceOptions);
      if (videoSource != null)
      {
        this.VideoSources.Add(device.Id, videoSource);
        videoSource.VideoSourceError += new CS.VideoSources.Core.VideoSourceErrorEventHandler(this.HandleVideoSourceError);
      }
      if (videoSource != null)
        return;
      this._logger.WarnFormat("Unable to create Video source of type {0} and id {1}", (object) device.Type, (object) device.Id);
    }

    public void TryToRemoveVideoSource(Guid idToDelete)
    {
      CS.VideoSources.Core.IVideoSource source;
      if (!this.VideoSources.TryGetValue(idToDelete, out source))
        return;
      this.StopAndDisposeSource(source);
      this.VideoSources.Remove(idToDelete);
    }

    private void LoadDevices()
    {
      foreach (BcDevices device in BcDevicesStorageExtensions.LoadByVsid(this.Id))
        this.TryAddOrUpdateVideoSource(device);
    }

    private void StopAndDisposeSource(CS.VideoSources.Core.IVideoSource source)
    {
      this.StopSource(source);
      source.Dispose();
    }

    private void StartProcessing()
    {
      if (this._checkDevicesThread != null)
        this._checkDevicesThread.Abort();
      this._checkDevicesThread = new Thread(new ThreadStart(this.CheckDevices))
      {
        IsBackground = true
      };
      this._checkDevicesThread.Start();
      this._isLoaded = true;
    }

    private void HandleVideoSourceError(object sender, Exception exception, string additionalErrorMessage)
    {
      CS.VideoSources.Core.IVideoSource videoSource = (CS.VideoSources.Core.IVideoSource) sender;
      this._logger.ErrorFormat("Error in videosource. Sender: {0}. Error: {1}, stack: {2}", sender, (object) exception, (object) exception.StackTrace);
      try
      {
        if (!videoSource.IsRunning)
          return;
        this._logger.InfoFormat("Try to restart source {0}", (object) videoSource.ConnectionString);
        videoSource.Start();
      }
      catch (Exception ex)
      {
        this._logger.Error((object) "Error restarting source", ex);
      }
    }

    private static string GetDeviceConnectionString(BcDevices device)
    {
      switch (device.Type)
      {
        case "Basler Aviator Series":
          return device.ConnectionString;
        case "Axis":
          return "http://" + device.ConnectionString + "/axis-cgi/mjpg/video.cgi";
        case "Vocord":
          return "http://" + device.ConnectionString + ":82/video/data.mjpg";
        case "Basler":
          return "http://" + device.ConnectionString + "/cgi-bin/mjpeg";
        case "Arecont":
          return "http://" + device.ConnectionString + "/mjpeg?";
        case "Panasonic":
          return "http://" + device.ConnectionString + "/cgi-bin/mjpeg?";
        case "HTTP MJPEG":
          return device.ConnectionString;
        case "Image":
        case "File":
          return device.ConnectionString;
        case "Web":
          return device.ConnectionString;
        default:
          return (string) null;
      }
    }

    private static IReadOnlyDictionary<string, string> GetDeviceOptions(BcDevices device)
    {
      switch (device.Type)
      {
        case "Web":
          string capability = device.Capability;
          if (string.IsNullOrEmpty(capability))
            return (IReadOnlyDictionary<string, string>) null;
          int num1 = capability.IndexOf(' ');
          string str1 = capability.Substring(0, num1);
          int num2 = capability.IndexOf("fps");
          string str2 = capability.Substring(num1, num2 - num1);
          return (IReadOnlyDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "FrameSize",
              str1
            },
            {
              "FPS",
              str2
            }
          };
        default:
          return (IReadOnlyDictionary<string, string>) null;
      }
    }

    private static CS.VideoSources.Core.IVideoSource CreateVideoSource(string type, string connectionString, string login, string password, IReadOnlyDictionary<string, string> options)
    {
      CS.VideoSources.Core.IVideoSource videoSource = (CS.VideoSources.Core.IVideoSource) null;
      switch (type)
      {
        case "Basler Aviator Series":
          videoSource = (CS.VideoSources.Core.IVideoSource) new BaslerVideoSource();
          break;
        case "Axis":
        case "Vocord":
        case "Basler":
        case "Arecont":
        case "Panasonic":
        case "HTTP MJPEG":
          videoSource = (CS.VideoSources.Core.IVideoSource) new AForgeVideoSource((Func<AForge.Video.IVideoSource>) (() => (AForge.Video.IVideoSource) new MJPEGStream()
          {
            Login = login,
            Password = password
          }));
          break;
        case "Image":
          videoSource = (CS.VideoSources.Core.IVideoSource) new ImageSource();
          break;
        case "File":
          videoSource = (CS.VideoSources.Core.IVideoSource) new VideoFileSource();
          break;
        case "Web":
          videoSource = (CS.VideoSources.Core.IVideoSource) new AForgeVideoSource((Func<AForge.Video.IVideoSource>) (() => (AForge.Video.IVideoSource) new VideoCaptureDevice()));
          break;
      }
      if (videoSource != null)
      {
        videoSource.ConnectionString = connectionString;
        videoSource.Options = options;
      }
      return videoSource;
    }

    private void CheckDevices()
    {
      while (!this._signalToStop)
      {
        try
        {
          List<BcDevices> list = BcDevicesStorageExtensions.LoadByVsid(this.Id);
          this.AddOrUpdateVideoSources((IEnumerable<BcDevices>) list);
          this.RemoveVideoSources((IEnumerable<BcDevices>) list);
        }
        catch (Exception ex)
        {
          this._logger.Error((object) ex);
        }
        Thread.Sleep(4000);
      }
    }

    private void RemoveVideoSources(IEnumerable<BcDevices> checkDevs)
    {
      foreach (Guid idToDelete in Enumerable.Except<Guid>(Enumerable.Select<KeyValuePair<Guid, CS.VideoSources.Core.IVideoSource>, Guid>((IEnumerable<KeyValuePair<Guid, CS.VideoSources.Core.IVideoSource>>) this.VideoSources, (Func<KeyValuePair<Guid, CS.VideoSources.Core.IVideoSource>, Guid>) (kvp => kvp.Key)), Enumerable.Select<BcDevices, Guid>(checkDevs, (Func<BcDevices, Guid>) (d => d.Id))))
        this.TryToRemoveVideoSource(idToDelete);
    }

    private void AddOrUpdateVideoSources(IEnumerable<BcDevices> checkDevs)
    {
      foreach (BcDevices device in checkDevs)
      {
        bool flag = false;
        string connectionString = VideoServer.GetDeviceConnectionString(device);
        foreach (KeyValuePair<Guid, CS.VideoSources.Core.IVideoSource> keyValuePair in (IEnumerable<KeyValuePair<Guid, CS.VideoSources.Core.IVideoSource>>) this.VideoSources)
        {
          CS.VideoSources.Core.IVideoSource videoSource = keyValuePair.Value;
          if (!(keyValuePair.Key != device.Id))
          {
            if (!string.Equals(videoSource.ConnectionString, connectionString, StringComparison.InvariantCultureIgnoreCase))
              this.TryAddOrUpdateVideoSource(device);
            flag = true;
            break;
          }
        }
        if (!flag)
          this.TryAddOrUpdateVideoSource(device);
        CS.VideoSources.Core.IVideoSource processedSource = this.VideoSources[device.Id];
        if (device.IsActive && !processedSource.IsRunning)
        {
          try
          {
            this._logger.InfoFormat("Starting source {0} with connection string {1}", (object) processedSource.GetType(), (object) processedSource.ConnectionString);
            processedSource.Start();
          }
          catch (Exception ex)
          {
            this._logger.Error((object) "Error starting source", ex);
            this.TryToRemoveVideoSource(device.Id);
          }
        }
        else if (!device.IsActive && processedSource.IsRunning)
          this.StopSource(processedSource);
      }
    }

    private void StopSource(CS.VideoSources.Core.IVideoSource processedSource)
    {
      this._logger.InfoFormat("Stopping source {0} with connection string {1}", (object) processedSource.GetType(), (object) processedSource.ConnectionString);
      processedSource.SignalToStop();
      processedSource.WaitToStop();
      processedSource.VideoSourceError -= new CS.VideoSources.Core.VideoSourceErrorEventHandler(this.HandleVideoSourceError);
      this._logger.InfoFormat("Source {0} with connection string {1} stopped successfully", (object) processedSource.GetType(), (object) processedSource.ConnectionString);
    }
  }
}
