// Decompiled with JetBrains decompiler
// Type: VideoStreamServer.VideoServerContractImpl
// Assembly: CSVideoServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: 28813BD9-90C6-4DB5-ADB3-76EB1EEB4BDD
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\video\CSVideoServer.exe

using BasicComponents;
using CommonContracts;
using CS.DAL;
using CS.VideoSources.Core;
using CSVideoServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;

namespace VideoStreamServer
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = true)]
  public class VideoServerContractImpl : IVideoServer
  {
    private static readonly VideoServer _instance = new VideoServer();
    public static IClientCallback MainServer;

    internal static VideoServer Instance
    {
      get
      {
        return VideoServerContractImpl._instance;
      }
    }

    public void SetDevice(Guid id, bool delete)
    {
      BcDevices device = BcDevicesStorageExtensions.LoadById(id);
      if (device.Vsid != VideoServerContractImpl.Instance.Id)
        VideoServerContractImpl.Instance.TryToRemoveVideoSource(id);
      else
        VideoServerContractImpl.Instance.TryAddOrUpdateVideoSource(device);
    }

    public bool GetServerState()
    {
      throw new NotImplementedException();
    }

    public void SetServerState(bool val)
    {
      throw new NotImplementedException();
    }

    public bool GetLoadState()
    {
      throw new NotImplementedException();
    }

    public string GetConnectionString()
    {
      throw new NotImplementedException();
    }

    public List<object[]> GetAllDevices()
    {
      throw new NotImplementedException();
    }

    public DataTable GetDeviceInfo()
    {
      DataTable dataTable = new DataTable()
      {
        TableName = "DeviceInfo"
      };
      dataTable.Columns.Add("ID", typeof (Guid));
      dataTable.Columns.Add("DeviceState", typeof (string));
      dataTable.Columns.Add("FrameCount", typeof (int));
      dataTable.Columns.Add("DetectionCount", typeof (int));
      dataTable.Columns.Add("DetectionFaces", typeof (int));
      dataTable.Columns.Add("VSState", typeof (string));
      dataTable.Columns.Add("CPUUsage", typeof (int));
      dataTable.Columns.Add("ThreadCount", typeof (int));
      dataTable.Columns.Add("MaxThreadCount", typeof (int));
      int num = 0;
      int processorCount = Environment.ProcessorCount;
      int count = Process.GetCurrentProcess().Threads.Count;
      foreach (KeyValuePair<Guid, IVideoSource> keyValuePair in (IEnumerable<KeyValuePair<Guid, IVideoSource>>) VideoServerContractImpl.Instance.VideoSources)
      {
        IVideoSource videoSource = keyValuePair.Value;
        string str = videoSource.IsRunning ? "Работает" : "Не работает";
        dataTable.Rows.Add((object) keyValuePair.Key, (object) str, (object) videoSource.RecievedFrames, (object) -1, (object) -1, (object) "Работает", (object) num, (object) count, (object) processorCount);
      }
      if (dataTable.Rows.Count == 0)
        dataTable.Rows.Add((object) Guid.Empty, (object) "", (object) 0, (object) 0, (object) 0, (object) "Работает", (object) num, (object) count, (object) processorCount);
      return dataTable;
    }

    public void ClearResults(List<Guid> ids)
    {
      if (!VideoServerContractImpl._instance.IsLoaded)
        ;
    }

    public bool PlayStart(Guid devid)
    {
      throw new NotImplementedException();
    }

    public bool Stop(Guid devid)
    {
      throw new NotImplementedException();
    }

    public byte[] GetLastFrame(Guid deviceId)
    {
      throw new NotImplementedException();
    }

    public double GetCurrentPos(Guid devId)
    {
      throw new NotImplementedException();
    }

    public double GetDuration(Guid devId)
    {
      throw new NotImplementedException();
    }

    public bool SetPos(Guid devId, double pos)
    {
      throw new NotImplementedException();
    }

    public bool Pause(Guid devId)
    {
      throw new NotImplementedException();
    }

    public bool Run(Guid devId)
    {
      throw new NotImplementedException();
    }

    public byte[] GetImageFromDevice(Guid devId)
    {
      CommonContracts.VideoFrame lastFrameFromDevice = VideoServerContractImpl.Instance.GetLastFrameFromDevice(devId, false);
      return lastFrameFromDevice != null ? lastFrameFromDevice.Frame : (byte[]) null;
    }

    public CommonContracts.VideoFrame GetDetectorFrame(Guid devId)
    {
      return VideoServerContractImpl.Instance.GetLastFrameFromDevice(devId, true);
    }

    public static void LoadServer(Guid serverId)
    {
      VideoServerContractImpl.Instance.Load(serverId);
    }
  }
}
