// Decompiled with JetBrains decompiler
// Type: FaceExtractorServer.ExtractorServer
// Assembly: CSExtractorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 8644959D-DFA5-425A-8F71-823BB535F3D1
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\extractor\CSExtractorServer.exe

using BasicComponents;
using BasicComponents.DetectorServer;
using BasicComponents.IdentificationServer;
using CommonContracts;
using CS.DAL;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding;
using TS.Sdk.StaticFace.NetBinding.Utils;

namespace FaceExtractorServer
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = true)]
  public class ExtractorServer : IExtractorServer
  {
    private static readonly ILog _logger = LogManager.GetLogger("PerfLog");
    private static readonly LimitedConcurrentQueue<FaceFrame> _faceframes = new LimitedConcurrentQueue<FaceFrame>(5);
    public static ILog Logger = LogManager.GetLogger(typeof (ExtractorServer));
    public static string ApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Technoserv\\ExtractorServers";
    public static BcExtractorServer MainServer = new BcExtractorServer();
    public static bool StopFlag = false;
    public static List<BcDevices> AllDevices = new List<BcDevices>();
    public static List<ExtractorServer.KeyExtractor> Extractors = new List<ExtractorServer.KeyExtractor>();
    public static PerformanceCounter CpuCounter = new PerformanceCounter()
    {
      CategoryName = "Processor",
      CounterName = "% Processor Time",
      InstanceName = "_Total"
    };
    public static bool IsLoaded;
    public static Thread RefreshTread;

    public static string StartPath
    {
      get
      {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
      }
    }

    public static void VideoThread(object device)
    {
      BcDevices dev = (BcDevices) device;
      while (!ExtractorServer.StopFlag)
      {
        try
        {
          if (!dev.IsActive)
          {
            Thread.Sleep(1000);
            continue;
          }
          if (dev.Dsid != Guid.Empty)
          {
            ExtractorServer.DetectorClientCallback detectorClientCallback = new ExtractorServer.DetectorClientCallback();
            BcDetectorServer ds = BcDetectorServer.LoadById(dev.Dsid);
            WcfExtensions.Using<DetectorServerClient>(new DetectorServerClient(new InstanceContext((object) detectorClientCallback)), (Action<DetectorServerClient>) (server =>
            {
              server.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) ds.Ip + ":" + (string) (object) ds.Port + "/CSDetectorServer/DetectorServer");
              server.Open();
              while (dev.IsActive && !ExtractorServer.StopFlag)
              {
                ExtractorServer._logger.Debug((object) string.Format("Request to Detector DevId={0}", (object) dev.Id));
                FaceFrame lastFrame = server.GetLastFrame(dev.Id);
                if (lastFrame == null)
                {
                  Thread.Sleep(20);
                }
                else
                {
                  ExtractorServer._logger.Debug((object) string.Format("Response from Detector DevId={0} FrameId={1} FrameIndex={2}", (object) dev.Id, (object) lastFrame.FrameId, (object) lastFrame.FrameIndex));
                  ExtractorServer._faceframes.Enqueue(lastFrame);
                }
              }
            }));
          }
        }
        catch (Exception ex)
        {
          ExtractorServer.Logger.Error((object) "Video Thread error ", ex);
        }
        Thread.Sleep(1000);
      }
    }

    public static BcDevices GetDeviceById(Guid id)
    {
      return Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, bool>) (dev => dev.Id == id)) ?? new BcDevices();
    }

    public static void WorkerThread(object extractor)
    {
      ExtractorServer.KeyExtractor keyExtractor = (ExtractorServer.KeyExtractor) extractor;
      while (!keyExtractor.StopFlag)
      {
        while (keyExtractor.BreakFlag)
        {
          keyExtractor.Breaked = true;
          if (!keyExtractor.StopFlag)
            Thread.Sleep(30);
          else
            break;
        }
        keyExtractor.Breaked = false;
        BcDevices bcDevices = keyExtractor.Device;
        try
        {
          Engine.Initialize(0U);
          using (Engine engine = new Engine())
          {
            while (!keyExtractor.BreakFlag && !keyExtractor.StopFlag)
            {
              FaceFrame orDefault = LimitedConcurrentQueueExtensions.GetOrDefault<FaceFrame>(ExtractorServer._faceframes);
              if (orDefault == null)
              {
                Thread.Sleep(10);
              }
              else
              {
                byte[] template = ExtractorServer.ExtractTemplate(orDefault, (IEngine) engine);
                bcDevices.AddKeyFrame(new KeyFrame()
                {
                  Frame = orDefault,
                  Key = template
                });
                ++bcDevices.ExtractCount;
              }
            }
          }
        }
        catch (Exception ex)
        {
          ExtractorServer.Logger.Error((object) string.Concat(new object[4]
          {
            (object) "Extractor Thread Error ",
            (object) ex.Message,
            (object) " Extractor Index ",
            (object) ExtractorServer.Extractors.IndexOf(keyExtractor)
          }));
        }
      }
      keyExtractor.Stopped = true;
    }

    private static byte[] ExtractTemplate(FaceFrame faceFrame, IEngine engine)
    {
      using (Bitmap imageFromArray = BasicComponents.ImageConverter.GetImageFromArray(faceFrame.Frame))
      {
        FaceData face1 = faceFrame.Face;
        FaceInfo face2 = new FaceInfo()
        {
          DetectionProbability = (double) faceFrame.Face.DetectionProb,
          YawAngle = (double) face1.YawAngle,
          PitchAngle = (double) face1.InplaneAngle,
          FaceRectangle = new TS.Core.Model.Rectangle((double) face1.LeftX, (double) face1.LeftY, (double) face1.Width, (double) face1.Height),
          LeftEye = new TS.Core.Model.Point((double) face1.LeftEyeX, (double) face1.LeftEyeY),
          RightEye = new TS.Core.Model.Point((double) face1.RightEyeX, (double) face1.RightEyeY)
        };
        return engine.ExtractTemplate(ModelImageConverter.ConvertFrom(imageFromArray), face2);
      }
    }

    public static void ReloadExtractors()
    {
      lock (ExtractorServer.Extractors)
      {
        foreach (BcDevices item_2 in ExtractorServer.AllDevices)
        {
          int local_1 = 0;
          foreach (ExtractorServer.KeyExtractor item_0 in ExtractorServer.Extractors)
          {
            if (item_0.Device.Id == item_2.Id && item_0.CurrentThread != null && item_0.CurrentThread.ThreadState != System.Threading.ThreadState.Aborted && item_0.CurrentThread.ThreadState != System.Threading.ThreadState.Stopped)
            {
              item_0.Device = item_2;
              ++local_1;
            }
          }
          if (local_1 < item_2.ExtractorCount)
          {
            for (; local_1 < item_2.ExtractorCount; ++local_1)
            {
              ExtractorServer.KeyExtractor local_2_1 = new ExtractorServer.KeyExtractor()
              {
                Device = item_2,
                CurrentThread = new Thread(new ParameterizedThreadStart(ExtractorServer.WorkerThread))
                {
                  IsBackground = true
                }
              };
              local_2_1.CurrentThread.Start((object) local_2_1);
              ExtractorServer.Extractors.Add(local_2_1);
            }
          }
          else if (local_1 > item_2.ExtractorCount)
          {
            while (local_1 > item_2.ExtractorCount)
            {
              ExtractorServer.KeyExtractor local_5 = (ExtractorServer.KeyExtractor) null;
              foreach (ExtractorServer.KeyExtractor item_1 in ExtractorServer.Extractors)
              {
                if (item_1.Device.Id == item_2.Id)
                {
                  local_5 = item_1;
                  local_5.StopFlag = true;
                  local_5.WaitForStop();
                  --local_1;
                  break;
                }
              }
              if (local_5 != null)
              {
                local_5.CurrentThread.Abort();
                ExtractorServer.Extractors.Remove(local_5);
              }
            }
          }
        }
      }
    }

    public static void UnBreakExtractor(Guid id)
    {
      lock (ExtractorServer.Extractors)
      {
        ExtractorServer.KeyExtractor local_0 = Enumerable.FirstOrDefault<ExtractorServer.KeyExtractor>((IEnumerable<ExtractorServer.KeyExtractor>) ExtractorServer.Extractors, (Func<ExtractorServer.KeyExtractor, bool>) (det => det.Device.Id == id));
        if (local_0 == null)
          return;
        local_0.BreakFlag = false;
        local_0.Breaked = false;
      }
    }

    public static void RefreshDevicesLoop()
    {
      while (!ExtractorServer.StopFlag)
      {
        if (ExtractorServer.IsLoaded)
        {
          try
          {
            ExtractorServer.RefreshDevices();
          }
          catch (Exception ex)
          {
            ExtractorServer.Logger.Error((object) "refresh device error ", ex);
          }
        }
        Thread.Sleep(10000);
      }
    }

    private static void RefreshDevices()
    {
      ExtractorServer.SyncCurrentAndActualDevices(BcDevicesStorageExtensions.LoadByEsid(ExtractorServer.MainServer.Id));
    }

    private static void SyncCurrentAndActualDevices(List<BcDevices> actualDevices)
    {
      foreach (BcDevices bcDevices in actualDevices)
      {
        BcDevices d1 = bcDevices;
        BcDevices d2 = Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, bool>) (bcDevice => bcDevice.Id == d1.Id));
        if (d2 != null)
        {
          if (ExtractorServer.HasDifferProperties(d1, d2))
          {
            ExtractorServer.BreakExtractor(d2.Id);
            d2.SetData(d1.GetData());
            ExtractorServer.UnBreakExtractor(d2.Id);
          }
        }
        else
        {
          d1.CurrentThread = new Thread(new ParameterizedThreadStart(ExtractorServer.VideoThread))
          {
            IsBackground = true
          };
          d1.CurrentThread.Start((object) d1);
          ExtractorServer.AllDevices.Add(d1);
        }
        ExtractorServer.ReloadExtractors();
      }
      foreach (BcDevices bcDevices in Enumerable.ToArray<BcDevices>(Enumerable.Where<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, bool>) (d => Enumerable.All<BcDevices>((IEnumerable<BcDevices>) actualDevices, (Func<BcDevices, bool>) (d2 => d2.Id != d.Id))))))
      {
        BcDevices bcDevicese = bcDevices;
        ExtractorServer.AllDevices.Remove(bcDevicese);
        foreach (ExtractorServer.KeyExtractor keyExtractor in Enumerable.ToArray<ExtractorServer.KeyExtractor>(Enumerable.Where<ExtractorServer.KeyExtractor>((IEnumerable<ExtractorServer.KeyExtractor>) ExtractorServer.Extractors, (Func<ExtractorServer.KeyExtractor, bool>) (d => d.Device.Id == bcDevicese.Id))))
        {
          keyExtractor.StopFlag = true;
          keyExtractor.WaitForStop();
          ExtractorServer.DestroyIfNeeded(keyExtractor.Device.CurrentThread);
        }
      }
    }

    private static void DestroyIfNeeded(Thread thread)
    {
      if (!thread.IsAlive)
        return;
      try
      {
        thread.Abort();
      }
      catch (Exception ex)
      {
        ExtractorServer.Logger.Error((object) "Device workthread was hard destroyed!", ex);
      }
    }

    public static void BreakExtractor(Guid id)
    {
      ExtractorServer.KeyExtractor keyExtractor = Enumerable.FirstOrDefault<ExtractorServer.KeyExtractor>((IEnumerable<ExtractorServer.KeyExtractor>) ExtractorServer.Extractors, (Func<ExtractorServer.KeyExtractor, bool>) (det => det.Device.Id == id));
      if (keyExtractor == null)
        return;
      keyExtractor.BreakFlag = true;
      keyExtractor.WaitForBreak();
    }

    private static bool HasDifferProperties(BcDevices d1, BcDevices d2)
    {
      return d1.ExtractorCount != d2.ExtractorCount;
    }

    public KeyFrame GetLastFrame(Guid deviceId)
    {
      BcDevices bcDevices = Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, bool>) (d => d.Id == deviceId));
      if (bcDevices == null)
        return (KeyFrame) null;
      return bcDevices.GetKeyFrame();
    }

    public void ClearResults(List<Guid> ids)
    {
      if (!ExtractorServer.IsLoaded)
        return;
      foreach (BcDevices bcDevices in Enumerable.Where<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, bool>) (d => ids.Contains(d.Id))))
        bcDevices.ExtractCount = 0;
    }

    public static void LoadServer(Guid serverId)
    {
      try
      {
        ExtractorServer.Logger.Info((object) "Extractor Server loaded");
        foreach (ExtractorServer.KeyExtractor keyExtractor in ExtractorServer.Extractors)
        {
          keyExtractor.StopFlag = true;
          keyExtractor.WaitForStop();
        }
        ExtractorServer.MainServer = BcExtractorServer.LoadById(serverId);
        ExtractorServer.IsLoaded = true;
      }
      catch (Exception ex)
      {
        ExtractorServer.Logger.Error((object) "Extractor Server load error", ex);
        throw;
      }
    }

    public void SetDevice(Guid id, Hashtable row, bool delete)
    {
      try
      {
        BcDevices deviceById = ExtractorServer.GetDeviceById(id);
        BcDevices bcDevices1 = BcDevicesStorageExtensions.LoadById(id);
        if (bcDevices1.Esid != ExtractorServer.MainServer.Id)
          delete = true;
        if (!delete)
        {
          if (deviceById.Id != Guid.Empty)
          {
            int extractorCount = deviceById.ExtractorCount;
            ExtractorServer.BreakExtractor(deviceById.Id);
            deviceById.SetData(row);
            if (extractorCount != deviceById.ExtractorCount)
              ExtractorServer.ReloadExtractors();
            ExtractorServer.UnBreakExtractor(deviceById.Id);
          }
          else
          {
            try
            {
              if (bcDevices1.Esid != ExtractorServer.MainServer.Id)
              {
                BcDevices bcDevices2 = BcDevicesStorageExtensions.LoadById(id);
                bcDevices2.CurrentThread = new Thread(new ParameterizedThreadStart(ExtractorServer.VideoThread))
                {
                  IsBackground = true
                };
                bcDevices2.CurrentThread.Start((object) bcDevices2);
                ExtractorServer.AllDevices.Add(bcDevices2);
                ExtractorServer.ReloadExtractors();
              }
            }
            catch (Exception ex)
            {
              ExtractorServer.Logger.Error((object) "Set device error", ex);
            }
          }
        }
        else
        {
          for (int index = 0; index < ExtractorServer.Extractors.Count; ++index)
          {
            if (ExtractorServer.Extractors.Count > 0 && index >= 0 && index < ExtractorServer.Extractors.Count)
            {
              ExtractorServer.KeyExtractor keyExtractor = ExtractorServer.Extractors[index];
              if (keyExtractor.Device.Id == id)
              {
                keyExtractor.StopFlag = true;
                keyExtractor.WaitForStop();
                ExtractorServer.Extractors.RemoveAt(index);
                break;
              }
            }
          }
          for (int index = 0; index < ExtractorServer.AllDevices.Count; ++index)
          {
            if (ExtractorServer.AllDevices[index].Id == id)
            {
              ExtractorServer.AllDevices.RemoveAt(index);
              break;
            }
          }
        }
      }
      catch
      {
      }
    }

    public DataTable GetDeviceInfo()
    {
      DataTable dataTable = new DataTable();
      dataTable.TableName = "EResults";
      dataTable.Columns.Add("ID", typeof (Guid));
      dataTable.Columns.Add("ExtractCount", typeof (int));
      dataTable.Columns.Add("ESState", typeof (string));
      dataTable.Columns.Add("CPUUsage", typeof (int));
      dataTable.Columns.Add("ThreadCount", typeof (int));
      dataTable.Columns.Add("MaxThreadCount", typeof (int));
      int cpuUsage = ExtractorServer.GetCpuUsage();
      int processorCount = Environment.ProcessorCount;
      int num = Enumerable.Sum<BcDevices>((IEnumerable<BcDevices>) ExtractorServer.AllDevices, (Func<BcDevices, int>) (dev => dev.ExtractorCount));
      foreach (BcDevices bcDevices in ExtractorServer.AllDevices)
        dataTable.Rows.Add((object) bcDevices.Id, (object) bcDevices.ExtractCount, (object) "Работает", (object) cpuUsage, (object) num, (object) processorCount);
      if (dataTable.Rows.Count == 0)
        dataTable.Rows.Add((object) Guid.Empty, (object) 0, (object) "Работает", (object) cpuUsage, (object) num, (object) processorCount);
      return dataTable;
    }

    public bool GetLoadState()
    {
      return ExtractorServer.IsLoaded;
    }

    public string GetConnectionString()
    {
      return CommonSettings.ConnectionString;
    }

    public static int GetCpuUsage()
    {
      return Convert.ToInt32(ExtractorServer.CpuCounter.NextValue());
    }

    public class KeyExtractor
    {
      public BcDevices Device = new BcDevices();
      public Thread CurrentThread;
      public bool StopFlag;
      public bool Stopped;
      public bool BreakFlag;
      public bool Breaked;

      public void WaitForBreak()
      {
        if (!this.BreakFlag)
          return;
        while (!this.Breaked)
          Thread.Sleep(10);
      }

      public void WaitForStop()
      {
        if (!this.StopFlag)
          return;
        while (!this.Stopped)
          Thread.Sleep(10);
      }
    }

    public class DetectorClientCallback : IDetectorServerCallback
    {
      public void CheckClient()
      {
      }
    }

    public class VerifierCallBack : IIdentificationServerCallback
    {
      public void CheckClient()
      {
      }

      public void SendResults(DataTable dt)
      {
      }

      public void SendStaticResults(Guid searchId, DataTable dt)
      {
      }
    }
  }
}
