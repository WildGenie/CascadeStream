// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.DetectorServer
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using BasicComponents;
using CommonContracts;
using CS.DAL;
using log4net;
using onvif.core;
using onvif.utils;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using TS.Sdk.StaticFace.NetBinding;

namespace CSDetectorServer
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = true)]
  public class DetectorServer : IDetectorServer
  {
    private static readonly ILog _logger = LogManager.GetLogger("PerfLog");
    public static readonly ILog Logger = LogManager.GetLogger(typeof (DetectorServer));
    private static readonly IBcFrameManager BcFrameManager = (IBcFrameManager) new CS.DAL.BcFrameManager((IBcFrameRepository) new BcFrameRepository(), (IBcDetectedFaceRepository) new BcDetectedFaceRepository());
    private static readonly IVideoServerInteraction VideoServerInteraction = (IVideoServerInteraction) new CSDetectorServer.VideoServerInteraction((IVideoServerCache) new VideoServerCache(DetectorServer.Logger), DetectorServer.Logger);
    private static readonly IEngine Engine = (IEngine) new TS.Sdk.StaticFace.NetBinding.Engine();
    private static readonly ConcurrentDictionary<Guid, Detector> Detectors = new ConcurrentDictionary<Guid, Detector>();
    public static BcDetectorServer MainServer = new BcDetectorServer();
    public static PerformanceCounter CpuCounter = new PerformanceCounter()
    {
      CategoryName = "Processor",
      CounterName = "% Processor Time",
      InstanceName = "_Total"
    };
    private static bool _isLoaded;
    public static string ApplicationFolder;
    public static string ImageFolder;
    public static bool StopFlag;

    public static async void RefreshDevicesLoop()
    {
      while (!DetectorServer.StopFlag)
      {
        if (DetectorServer._isLoaded)
          DetectorServer.TryRefreshDevices();
        await Task.Delay(10000);
      }
    }

    private static void TryRefreshDevices()
    {
      try
      {
        DetectorServer.SyncCurrentAndActualDevices(BcDevicesStorageExtensions.LoadByDsid(DetectorServer.MainServer.Id));
      }
      catch (Exception ex)
      {
        DetectorServer.Logger.Error((object) "refresh device error ", ex);
      }
    }

    private static void SyncCurrentAndActualDevices(List<BcDevices> actualDevices)
    {
      foreach (Guid key in Enumerable.Except<Guid>((IEnumerable<Guid>) DetectorServer.Detectors.Keys, Enumerable.Select<BcDevices, Guid>((IEnumerable<BcDevices>) actualDevices, (Func<BcDevices, Guid>) (devices => devices.Id))))
      {
        Detector detector;
        if (DetectorServer.Detectors.TryRemove(key, out detector))
          detector.Dispose();
      }
      foreach (KeyValuePair<Guid, Detector> keyValuePair in DetectorServer.Detectors)
      {
        KeyValuePair<Guid, Detector> detector = keyValuePair;
        BcDevices bcDevices = Enumerable.First<BcDevices>((IEnumerable<BcDevices>) actualDevices, (Func<BcDevices, bool>) (devices => devices.Id == detector.Key));
        detector.Value.UpdateDevice(bcDevices);
      }
      foreach (BcDevices bcDevices in Enumerable.Where<BcDevices>((IEnumerable<BcDevices>) actualDevices, (Func<BcDevices, bool>) (devices => !DetectorServer.Detectors.ContainsKey(devices.Id))))
      {
        Detector detector = new Detector(DetectorServer.Logger, DetectorServer.BcFrameManager, DetectorServer.VideoServerInteraction, DetectorServer.Engine);
        DetectorServer.Detectors[bcDevices.Id] = detector;
        detector.UpdateDevice(bcDevices);
      }
    }

    public FaceFrame GetLastFrame(Guid deviceId)
    {
      Detector detector;
      if (!DetectorServer.Detectors.TryGetValue(deviceId, out detector))
        return (FaceFrame) null;
      FaceFrame faceFrame = detector.GetFaceFrame();
      DetectorServer._logger.Debug((object) string.Format("Response for LastFrame DevId={0}; FrameId={1}", (object) deviceId, faceFrame != null ? (object) faceFrame.FrameId.ToString() : (object) "NULL"));
      return faceFrame;
    }

    public void ClearResults(List<Guid> ids)
    {
      if (!DetectorServer._isLoaded)
        return;
      foreach (KeyValuePair<Guid, Detector> keyValuePair in Enumerable.Where<KeyValuePair<Guid, Detector>>((IEnumerable<KeyValuePair<Guid, Detector>>) DetectorServer.Detectors, (Func<KeyValuePair<Guid, Detector>, bool>) (pair => ids.Contains(pair.Key))))
        keyValuePair.Value.ResetStats();
    }

    public bool GetLoadState()
    {
      return DetectorServer._isLoaded;
    }

    public string GetConnectionString()
    {
      return CommonSettings.ConnectionString;
    }

    public static void LoadServer(Guid serverId)
    {
      DetectorServer.Logger.Info((object) (" Server Loaded - ServerID -" + (object) serverId));
      try
      {
        DetectorServer.TryRefreshDevices();
        DetectorServer.MainServer = BcDetectorServer.LoadById(serverId);
      }
      catch (Exception ex)
      {
        DetectorServer.Logger.Error((object) ex);
      }
      DetectorServer._isLoaded = true;
    }

    public void SetDevice(Guid id, Hashtable row, bool delete)
    {
      try
      {
        DetectorServer.TryRefreshDevices();
      }
      catch (Exception ex)
      {
        DetectorServer.Logger.Error((object) "Error set device", ex);
      }
    }

    public DataTable GetDeviceInfo()
    {
      DataTable dataTable = new DataTable()
      {
        TableName = "DResults"
      };
      dataTable.Columns.Add("ID", typeof (Guid));
      dataTable.Columns.Add("DetectionCount", typeof (int));
      dataTable.Columns.Add("DetectionFaces", typeof (int));
      dataTable.Columns.Add("DSState", typeof (string));
      dataTable.Columns.Add("CPUUsage", typeof (int));
      dataTable.Columns.Add("ThreadCount", typeof (int));
      dataTable.Columns.Add("MaxThreadCount", typeof (int));
      dataTable.Columns.Add("FrameCount", typeof (int));
      int cpuUsage = DetectorServer.GetCpuUsage();
      int processorCount = Environment.ProcessorCount;
      int num = Enumerable.Sum<KeyValuePair<Guid, Detector>>((IEnumerable<KeyValuePair<Guid, Detector>>) DetectorServer.Detectors, (Func<KeyValuePair<Guid, Detector>, int>) (kvp => kvp.Value.Count));
      foreach (KeyValuePair<Guid, Detector> keyValuePair in DetectorServer.Detectors)
      {
        BcDevices device = keyValuePair.Value.Device;
        if (device != null)
          dataTable.Rows.Add((object) keyValuePair.Key, (object) device.DetectionCount, (object) device.DetectionFaces, (object) "Работает", (object) cpuUsage, (object) num, (object) processorCount, (object) device.FrameCount);
      }
      if (dataTable.Rows.Count == 0)
        dataTable.Rows.Add((object) Guid.Empty, (object) 0, (object) 0, (object) "Работает", (object) cpuUsage, (object) num, (object) processorCount);
      return dataTable;
    }

    public static void ConnectToOnvifService(out OdmSession onvifHelper, BcDevices dev)
    {
      onvifHelper = (OdmSession) null;
      if (!(dev.Type == "Onvif Kipod Server"))
        return;
      try
      {
        NvtSessionFactory nvtSessionFactory = new NvtSessionFactory(new NetworkCredential(dev.Login, dev.Password));
        DetectorServer.Logger.Info((object) "onvifHelper Successfuly created");
        DetectorServer.Logger.Info((object) "subscribing to evets...");
        DetectorServer.Logger.Info((object) "Start Subscribe");
        try
        {
        }
        catch (Exception ex)
        {
          DetectorServer.Logger.Error((object) "Error subscribe ", ex);
        }
      }
      catch (Exception ex)
      {
        DetectorServer.Logger.Error((object) "Onvif Error", ex);
      }
    }

    public static int GetCpuUsage()
    {
      return Convert.ToInt32(DetectorServer.CpuCounter.NextValue());
    }
  }
}
