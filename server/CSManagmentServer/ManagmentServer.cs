// Decompiled with JetBrains decompiler
// Type: CSManagmentServer.ManagmentServer
// Assembly: CSManagmentServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: C5B7D3C1-7999-4FC6-B40F-178E2CEECAE4
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\manager\CSManagmentServer.exe

using BasicComponents;
using BasicComponents.DetectorServer;
using BasicComponents.ExtractorServer;
using BasicComponents.IdentificationServer;
using BasicComponents.VideoServer;
using CNAPIclr;
using CNAPIclr.Messages;
using CNAPIclr.Messages.Notifications;
using CNAPIclr.Messages.Search;
using CommonContracts;
using CS.BaronInterop.Common;
using CS.DAL;
using CS.StorableModel.Abstract;
using IdentificationSystem.ControlProtocol;
using IdentificationSystem.ControlProtocol.Messages;
using log4net;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Xml.Serialization;
using TS.IdentificationSystem.ControlProtocol.Model;

namespace CSManagmentServer
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = true)]
  public class ManagmentServer : IManagmentServer
  {
    public static Dictionary<Guid, string> UserActions = new Dictionary<Guid, string>();
    public static Dictionary<Guid, string> UserRoles = new Dictionary<Guid, string>();
    public static int KeyLength = 10988;
    public static List<DeviceInfo> CommonDeviceInfo = new List<DeviceInfo>();
    public static List<ManagmentServer.Operator> Operators = new List<ManagmentServer.Operator>();
    public static List<ManagmentServer.WorkStations> Clients = new List<ManagmentServer.WorkStations>();
    private static readonly SynchronizedCollection<IServer> Servers = new SynchronizedCollection<IServer>();
    public static List<BcDevices> Devices = new List<BcDevices>();
    public static ManagmentServer.ClientDetectorCallback MainDetectorClient = new ManagmentServer.ClientDetectorCallback();
    public static ManagmentServer.ClientExtractorCallback MainExtractorClient = new ManagmentServer.ClientExtractorCallback();
    public static ManagmentServer.ClientIdentificationCallback MainIdentClient = new ManagmentServer.ClientIdentificationCallback();
    public static List<Guid> DeletedOperators = new List<Guid>();
    public static ILog Logger = LogManager.GetLogger(typeof (ManagmentServer));
    public static CNAPIclr.Messages.Node CurrentNode = new CNAPIclr.Messages.Node()
    {
      CPU = new CPU()
      {
        CPUS = new List<CPU>()
      }
    };
    public static CNAPIclr.Messages.Node Node1 = new CNAPIclr.Messages.Node()
    {
      CPU = new CPU()
      {
        CPUS = new List<CPU>()
      }
    };
    public static CNAPIclr.Messages.Node Node15 = new CNAPIclr.Messages.Node()
    {
      CPU = new CPU()
      {
        CPUS = new List<CPU>()
      }
    };
    public static CNAPIclr.Messages.Node NodeBuf = new CNAPIclr.Messages.Node()
    {
      CPU = new CPU()
      {
        CPUS = new List<CPU>()
      }
    };
    public static List<BcFace> AllModels = new List<BcFace>();
    public static DateTime NullDate = new DateTime(1900, 1, 1);
    public static float MinScore = 0.45f;
    public static long LastSendedId = 0;
    public static object WriteLockObj = new object();
    public static string KarsFilename = "";
    public static KarsConfig KarsConfig = new KarsConfig();
    public static List<ManagmentServer.SearchData> CompleteSearchs = new List<ManagmentServer.SearchData>();
    public static SelfCheck CheckResult = new SelfCheck();
    public static DateTime LastTestDate = new DateTime(1970, 1, 1);
    public StatsResponse Resp = new StatsResponse();
    public static ControlServer Server;
    public static bool StopFlag;
    public static Thread CheckOperatorsThread;
    public static Thread StatisticThread;
    public static Thread RefreshThread;
    public static Thread StatsThread;
    public static Thread PerfomanceThread;
    public static int UpdateInterval;
    public static Thread SentHotListThread;
    public static Thread ModelThread;
    public static CNAPIObj CnapiObj;
    public static bool GetSearchsBusy;

    public static string ApplicationFolder { get; set; }

    public static KeyValuePair<Guid, string> GetRoleByName(int name)
    {
      int num = 0;
      foreach (KeyValuePair<Guid, string> keyValuePair in ManagmentServer.UserRoles)
      {
        if (num == name)
          return keyValuePair;
        ++num;
      }
      return new KeyValuePair<Guid, string>(Guid.Empty, "");
    }

    public static KeyValuePair<Guid, string> GetActionByName(int name)
    {
      int num = 0;
      foreach (KeyValuePair<Guid, string> keyValuePair in ManagmentServer.UserActions)
      {
        if (num == name)
          return keyValuePair;
        ++num;
      }
      return new KeyValuePair<Guid, string>(Guid.Empty, "");
    }

    public static void CopyMessageToDevice(BcDevices dev, CameraSettings cam)
    {
      dev.Type = cam.CameraType;
      dev.DetectorScore = (Decimal) cam.DetectionThreshold;
      dev.Dsid = cam.DetectorServerId;
      dev.DetectorCount = cam.DetectorThreads;
      dev.Esid = cam.ExtractorServerId;
      dev.ExtractorCount = cam.ExtractorThreads;
      dev.Isid = cam.IdentifierServerId;
      dev.IsAccesable = cam.IsActive;
      dev.Login = cam.Login;
      dev.MinScore = (Decimal) cam.MeasureThreshold;
      dev.MinFace = cam.MinFaceSize;
      dev.Name = cam.Name;
      dev.Password = cam.Password;
      dev.SaveNonCategory = cam.SaveDetectedFacesInDb;
      dev.SaveFace = cam.SaveFaces;
      dev.Vsid = cam.VideoServerId;
    }

    public static void CopyDeviceToMessage(BcDevices dev, CameraSettings cam)
    {
      cam.CameraType = dev.Type;
      cam.DetectionThreshold = (double) dev.DetectorScore;
      cam.DetectorServerId = dev.Dsid;
      cam.DetectorThreads = dev.DetectorCount;
      cam.ExtractorServerId = dev.Esid;
      cam.ExtractorThreads = dev.ExtractorCount;
      cam.IdentifierServerId = dev.Isid;
      cam.IsActive = dev.IsAccesable;
      cam.Login = dev.Login;
      cam.MeasureThreshold = (double) dev.MinScore;
      cam.MinFaceSize = dev.MinFace;
      cam.Name = dev.Name;
      cam.Password = dev.Password;
      cam.SaveDetectedFacesInDb = dev.SaveNonCategory;
      cam.SaveFaces = dev.SaveFace;
      cam.VideoServerId = dev.Vsid;
    }

    public static void dev_StateChanged(object sender, StateChangedEventArgs e)
    {
      try
      {
        switch (e.Type)
        {
          case ObjectType.Device:
            try
            {
            }
            catch
            {
              break;
            }
            break;
          case ObjectType.VideoServer:
            BcVideoServer bcVideoServer = (BcVideoServer) sender;
            try
            {
              ManagmentServer.Server.SendSystemMessage(new SystemMessageBody()
              {
                MessageText = MessageConstructor.GetMessageString(bcVideoServer.Ip, bcVideoServer.Id, bcVideoServer.IsAlive, ObjectType.VideoServer, ObjEventType.StateChanged),
                MessageCode = 5,
                MessageSeverity = MessageSeverity.Info
              });
              break;
            }
            catch
            {
              break;
            }
          case ObjectType.DetectorServer:
            BcDetectorServer bcDetectorServer = (BcDetectorServer) sender;
            try
            {
              ManagmentServer.Server.SendSystemMessage(new SystemMessageBody()
              {
                MessageText = MessageConstructor.GetMessageString(bcDetectorServer.Ip, bcDetectorServer.Id, bcDetectorServer.IsAlive, ObjectType.DetectorServer, ObjEventType.StateChanged),
                MessageCode = 2,
                MessageSeverity = MessageSeverity.Info
              });
              break;
            }
            catch
            {
              break;
            }
          case ObjectType.IdentificationServer:
            BcIdentificationServer identificationServer = (BcIdentificationServer) sender;
            try
            {
              ManagmentServer.Server.SendSystemMessage(new SystemMessageBody()
              {
                MessageText = MessageConstructor.GetMessageString(identificationServer.Ip, identificationServer.Id, identificationServer.IsAlive, ObjectType.IdentificationServer, ObjEventType.StateChanged),
                MessageCode = 4,
                MessageSeverity = MessageSeverity.Info
              });
              break;
            }
            catch
            {
              break;
            }
          case ObjectType.ExtractorServer:
            BcExtractorServer bcExtractorServer = (BcExtractorServer) sender;
            try
            {
              ManagmentServer.Server.SendSystemMessage(new SystemMessageBody()
              {
                MessageText = MessageConstructor.GetMessageString(bcExtractorServer.Ip, bcExtractorServer.Id, bcExtractorServer.IsAlive, ObjectType.ExtractorServer, ObjEventType.StateChanged),
                MessageCode = 3,
                MessageSeverity = MessageSeverity.Info
              });
              break;
            }
            catch
            {
              break;
            }
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Info((object) ("dev state changed error " + ex.Message + "\r\n" + ex.StackTrace));
      }
    }

    private static void TrySendMessage(SystemMessageBody message)
    {
      try
      {
        if (ManagmentServer.Server == null)
          return;
        ManagmentServer.Server.SendSystemMessage(message);
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.ErrorFormat("Unable to send message {0} ({1}). Error: {2}", (object) message.MessageCode, (object) message.MessageText, (object) ex);
      }
    }

    private static void TrySendServerMessage(IServer server, int messageCode, ObjEventType evt)
    {
      ManagmentServer.TrySendMessage(new SystemMessageBody()
      {
        MessageCode = messageCode,
        MessageSeverity = MessageSeverity.Info,
        MessageText = MessageConstructor.GetMessageString(server.Ip, server.Id, server.IsAlive, server.Type, evt)
      });
    }

    [DllImport("iphlpapi.dll")]
    public static extern int SendARP(int destIp, int srcIp, [Out] byte[] pMacAddr, ref int phyAddrLen);

    public List<DeviceInfo> GetDevicesInfo()
    {
      return ManagmentServer.CommonDeviceInfo;
    }

    public DataTable GetVideoServerState()
    {
      return this.RetrieveServersState(ObjectType.VideoServer, "VideoServers");
    }

    public DataTable GetIdentificationServersState()
    {
      return this.RetrieveServersState(ObjectType.IdentificationServer, "IdentificationServers");
    }

    public DataTable GetDetectorServerState()
    {
      return this.RetrieveServersState(ObjectType.DetectorServer, "DetectorServers");
    }

    public DataTable GetExtractorServerState()
    {
      return this.RetrieveServersState(ObjectType.ExtractorServer, "ExtractorServers");
    }

    private DataTable RetrieveServersState(ObjectType type, string tableName)
    {
      DataTable dataTable = new DataTable()
      {
        TableName = tableName
      };
      dataTable.Columns.Add("ID", typeof (Guid));
      dataTable.Columns.Add("ServerState", typeof (string));
      IServer[] serverArray = Enumerable.ToArray<IServer>(Enumerable.Where<IServer>((IEnumerable<IServer>) ManagmentServer.Servers, (Func<IServer, bool>) (s => s.Type == type)));
      dataTable.BeginLoadData();
      foreach (IServer server in serverArray)
        dataTable.Rows.Add((object) server.Id, (object) server.State);
      dataTable.EndLoadData();
      return dataTable;
    }

    public void Connect(Guid id)
    {
      ManagmentServer.WorkStations workStations = Enumerable.FirstOrDefault<ManagmentServer.WorkStations>((IEnumerable<ManagmentServer.WorkStations>) ManagmentServer.Clients, (Func<ManagmentServer.WorkStations, bool>) (c => c.Id == id));
      if (workStations == null)
      {
        workStations = new ManagmentServer.WorkStations()
        {
          Id = id
        };
        ManagmentServer.Clients.Add(workStations);
      }
      workStations.Client = OperationContext.Current.GetCallbackChannel<IManagerClientCallback>();
      workStations.Connected = true;
    }

    public string ConnectOperator()
    {
      RemoteEndpointMessageProperty endpointMessageProperty = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
      IPAddress ipAddress = IPAddress.Parse(endpointMessageProperty.Address);
      byte[] pMacAddr = new byte[6];
      int length = pMacAddr.Length;
      ManagmentServer.SendARP((int) ipAddress.Address, 0, pMacAddr, ref length);
      string key = BitConverter.ToString(pMacAddr, 0, 6);
      bool flag = false;
      foreach (ManagmentServer.Operator @operator in ManagmentServer.Operators)
      {
        if (@operator.MacAddress == key)
        {
          if (!@operator.Connected)
            @operator.Client = OperationContext.Current.GetCallbackChannel<IClientCallback>();
          @operator.Connected = true;
          flag = true;
          @operator.MacAddress = key;
          @operator.IpAddress = endpointMessageProperty.Address;
          break;
        }
      }
      if (!flag)
      {
        try
        {
          BcWorkStation bcWorkStation = BcWorkStation.LoadByHardwareKey(key);
          ManagmentServer.Operator @operator = new ManagmentServer.Operator();
          @operator.Client = @operator.Client = OperationContext.Current.GetCallbackChannel<IClientCallback>();
          @operator.Connected = true;
          @operator.Id = !(bcWorkStation.Id != Guid.Empty) ? Guid.NewGuid() : bcWorkStation.Id;
          @operator.MacAddress = key;
          @operator.IpAddress = endpointMessageProperty.Address;
          ManagmentServer.Operators.Add(@operator);
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "Error", ex);
        }
      }
      return key;
    }

    public void DeleteOperator(Guid id)
    {
      new BcWorkStation()
      {
        Id = id
      }.Delete();
      new BcWorkStationRule()
      {
        WorkStationId = id
      }.DeleteByWorkStationId();
      ManagmentServer.DeletedOperators.Add(id);
    }

    public void SetWorkStationByKey(string key)
    {
      try
      {
        BcWorkStation bcWorkStation = BcWorkStation.LoadByHardwareKey(key);
        ManagmentServer.Operator @operator = Enumerable.FirstOrDefault<ManagmentServer.Operator>((IEnumerable<ManagmentServer.Operator>) ManagmentServer.Operators, (Func<ManagmentServer.Operator, bool>) (o => o.MacAddress == key));
        if (@operator == null)
          return;
        @operator.Station = bcWorkStation;
        @operator.Id = bcWorkStation.Id;
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
    }

    public DataTable GetAllOperators()
    {
      DataTable dataTable = new DataTable()
      {
        TableName = "Workstations"
      };
      try
      {
        dataTable.Columns.Add("ID", typeof (Guid));
        dataTable.Columns.Add("Connected", typeof (string));
        dataTable.Columns.Add("IP", typeof (string));
        dataTable.Columns.Add("HardwareKey", typeof (string));
        dataTable.Columns.Add("Name", typeof (string));
        dataTable.Columns.Add("Comment", typeof (string));
        dataTable.Columns.Add("Status", typeof (string));
        foreach (ManagmentServer.Operator @operator in ManagmentServer.Operators)
        {
          string str1 = "Работает";
          if (!@operator.Connected)
            str1 = "Не работает";
          string str2 = "Активна";
          if (!@operator.Station.Status)
            str2 = "Не активна";
          dataTable.Rows.Add((object) @operator.Id, (object) str1, (object) @operator.IpAddress, (object) @operator.MacAddress, (object) @operator.Station.Name, (object) @operator.Station.Comment, (object) str2);
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
      return dataTable;
    }

    public void SendDevice(Guid id, bool delete)
    {
      try
      {
        BcDevices dev = BcDevicesStorageExtensions.LoadById(id);
        ParallelEnumerable.ForAll<IServer>(ParallelEnumerable.AsParallel<IServer>((IEnumerable<IServer>) ManagmentServer.Servers), (Action<IServer>) (server => this.SetDevice(server, id, dev, delete)));
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
    }

    public void SendVideoServer(Guid id)
    {
      ManagmentServer.CheckAndAddServer(id, ObjectType.VideoServer, new Func<Guid, IServer>(BcVideoServer.LoadById), new Action<object>(ManagmentServer.VideoWork));
    }

    public void SendDetectorServer(Guid id)
    {
      ManagmentServer.CheckAndAddServer(id, ObjectType.DetectorServer, new Func<Guid, IServer>(BcDetectorServer.LoadById), new Action<object>(ManagmentServer.DetectorWork));
    }

    public void SendExtractorServer(Guid id)
    {
      ManagmentServer.CheckAndAddServer(id, ObjectType.ExtractorServer, new Func<Guid, IServer>(BcExtractorServer.LoadById), new Action<object>(ManagmentServer.ExtractorWork));
    }

    public void SendIdentificationServer(Guid id)
    {
      IServer server = ManagmentServer.CheckAndAddServer(id, ObjectType.IdentificationServer, new Func<Guid, IServer>(BcIdentificationServer.LoadById), new Action<object>(ManagmentServer.IdentificationWork));
      IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient));
      identificationServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) server.Ip + ":" + (string) (object) server.Port + "/FaceIdentification/IdentificationServer");
      identificationServerClient.Open();
      identificationServerClient.SetServerSettings();
      identificationServerClient.Close();
    }

    public void ClearResults(List<Guid> ids)
    {
      foreach (IServer objServer in Enumerable.ToArray<IServer>((IEnumerable<IServer>) ManagmentServer.Servers))
      {
        ManagmentServer.ClearResultsFunc clearResultsFunc = new ManagmentServer.ClearResultsFunc(this.ClearServerResults);
        clearResultsFunc.BeginInvoke(ids, objServer, new AsyncCallback(this.EndClearserverResults), (object) clearResultsFunc);
      }
    }

    public static void CheckOperators()
    {
      while (!ManagmentServer.StopFlag)
      {
        try
        {
          int count1 = ManagmentServer.Operators.Count;
          if (ManagmentServer.DeletedOperators.Count > 0)
          {
            int count2 = ManagmentServer.DeletedOperators.Count;
            for (int index1 = 0; index1 < count2; ++index1)
            {
              if (index1 >= 0 && index1 < ManagmentServer.DeletedOperators.Count)
              {
                for (int index2 = 0; index2 < count1; ++index2)
                {
                  if (index2 >= 0 && index2 < count1 && ManagmentServer.Operators[index2].Id == ManagmentServer.DeletedOperators[index1])
                  {
                    if (!ManagmentServer.Operators[index2].Connected)
                    {
                      ManagmentServer.Operators[index2].Station = new BcWorkStation();
                      ManagmentServer.Operators.RemoveAt(index2);
                      count1 = ManagmentServer.Operators.Count;
                      ManagmentServer.DeletedOperators.RemoveAt(index1);
                      --index1;
                      count2 = ManagmentServer.DeletedOperators.Count;
                      break;
                    }
                    ManagmentServer.Operators[index2].Id = Guid.Empty;
                    ManagmentServer.Operators[index2].Station = new BcWorkStation();
                    break;
                  }
                }
              }
            }
          }
          foreach (BcWorkStation bcWorkStation in BcWorkStation.LoadAll())
          {
            int count2 = ManagmentServer.Operators.Count;
            bool flag = false;
            for (int index = 0; index < count2; ++index)
            {
              try
              {
                if (ManagmentServer.Operators[index].Connected)
                  ManagmentServer.Operators[index].Client.CheckClient();
              }
              catch
              {
                ManagmentServer.Operators[index].Connected = false;
              }
              try
              {
                if (ManagmentServer.Operators[index].MacAddress == bcWorkStation.HardwareKey)
                {
                  flag = true;
                  if (ManagmentServer.Operators[index].Station.Id != bcWorkStation.Id)
                  {
                    ManagmentServer.Operators[index].Id = bcWorkStation.Id;
                    ManagmentServer.Operators[index].Station = bcWorkStation;
                    break;
                  }
                  break;
                }
              }
              catch
              {
                ManagmentServer.Operators[index].Connected = false;
              }
            }
            if (!flag)
              ManagmentServer.Operators.Add(new ManagmentServer.Operator()
              {
                Connected = false,
                Id = bcWorkStation.Id,
                IpAddress = "",
                MacAddress = bcWorkStation.HardwareKey,
                Station = bcWorkStation
              });
          }
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "Error", ex);
        }
        Thread.Sleep(1000);
      }
    }

    public void SetDevice(IServer objServer, Guid id, BcDevices dev, bool delete)
    {
      string str = string.Concat(new object[4]
      {
        (object) "net.tcp://",
        (object) objServer.Ip,
        (object) ":",
        (object) objServer.Port
      });
      Dictionary<object, object> row = Enumerable.ToDictionary<DictionaryEntry, object, object>(Enumerable.Cast<DictionaryEntry>((IEnumerable) dev.GetData()), (Func<DictionaryEntry, object>) (d => d.Key), (Func<DictionaryEntry, object>) (d => d.Value));
      try
      {
        switch (objServer.Type)
        {
          case ObjectType.VideoServer:
            using (VideoServerClient videoServerClient = new VideoServerClient())
            {
              videoServerClient.Endpoint.Address = new EndpointAddress(str + "/VideoStreamServer/VideoServer");
              videoServerClient.Open();
              videoServerClient.SetDevice(id, delete);
              break;
            }
          case ObjectType.DetectorServer:
            using (DetectorServerClient detectorServerClient = new DetectorServerClient(new InstanceContext((object) ManagmentServer.MainDetectorClient)))
            {
              detectorServerClient.Endpoint.Address = new EndpointAddress(str + "/CSDetectorServer/DetectorServer");
              detectorServerClient.Open();
              detectorServerClient.SetDevice(id, row, delete);
              break;
            }
          case ObjectType.IdentificationServer:
            using (IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient)))
            {
              identificationServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceIdentification/IdentificationServer");
              identificationServerClient.Open();
              identificationServerClient.SetDevice(id, row, delete);
              break;
            }
          case ObjectType.ExtractorServer:
            using (ExtractorServerClient extractorServerClient = new ExtractorServerClient(new InstanceContext((object) ManagmentServer.MainExtractorClient)))
            {
              extractorServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceExtractorServer/ExtractorServer");
              extractorServerClient.Open();
              extractorServerClient.SetDevice(id, row, delete);
              break;
            }
        }
      }
      catch (CommunicationException ex)
      {
        ManagmentServer.Logger.Error((object) ("Server Unavailable " + str));
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) ex);
      }
    }

    public void ClearServerResults(List<Guid> ids, IServer objServer)
    {
      string str = string.Concat(new object[4]
      {
        (object) "net.tcp://",
        (object) objServer.Ip,
        (object) ":",
        (object) objServer.Port
      });
      try
      {
        switch (objServer.Type)
        {
          case ObjectType.VideoServer:
            using (VideoServerClient videoServerClient = new VideoServerClient())
            {
              videoServerClient.Endpoint.Address = new EndpointAddress(str + "/VideoStreamServer/VideoServer");
              videoServerClient.Open();
              videoServerClient.ClearResults(ids.ToArray());
              break;
            }
          case ObjectType.DetectorServer:
            using (DetectorServerClient detectorServerClient = new DetectorServerClient(new InstanceContext((object) ManagmentServer.MainDetectorClient)))
            {
              detectorServerClient.Endpoint.Address = new EndpointAddress(str + "/CSDetectorServer/DetectorServer");
              detectorServerClient.Open();
              detectorServerClient.ClearResults(ids.ToArray());
              break;
            }
          case ObjectType.IdentificationServer:
            using (IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient)))
            {
              identificationServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceIdentification/IdentificationServer");
              identificationServerClient.Open();
              identificationServerClient.ClearResults(ids.ToArray());
              break;
            }
          case ObjectType.ExtractorServer:
            using (ExtractorServerClient extractorServerClient = new ExtractorServerClient(new InstanceContext((object) ManagmentServer.MainExtractorClient)))
            {
              extractorServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceExtractorServer/ExtractorServer");
              extractorServerClient.Open();
              extractorServerClient.ClearResults(ids.ToArray());
              break;
            }
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
    }

    public void EndClearserverResults(IAsyncResult res)
    {
    }

    public static void SetLocalDevice(IServer objServer, Guid id, BcDevices dev, bool delete)
    {
      string str = string.Concat(new object[4]
      {
        (object) "net.tcp://",
        (object) objServer.Ip,
        (object) ":",
        (object) objServer.Port
      });
      try
      {
        switch (objServer.Type)
        {
          case ObjectType.VideoServer:
            using (VideoServerClient videoServerClient = new VideoServerClient())
            {
              videoServerClient.Endpoint.Address = new EndpointAddress(str + "/VideoStreamServer/VideoServer");
              videoServerClient.Open();
              videoServerClient.SetDevice(id, delete);
              break;
            }
          case ObjectType.DetectorServer:
            using (DetectorServerClient detectorServerClient = new DetectorServerClient(new InstanceContext((object) ManagmentServer.MainDetectorClient)))
            {
              detectorServerClient.Endpoint.Address = new EndpointAddress(str + "/CSDetectorServer/DetectorServer");
              detectorServerClient.Open();
              detectorServerClient.SetDevice(id, Enumerable.ToDictionary<DictionaryEntry, object, object>(Enumerable.Cast<DictionaryEntry>((IEnumerable) dev.GetData()), (Func<DictionaryEntry, object>) (d => d.Key), (Func<DictionaryEntry, object>) (d => d.Value)), delete);
              break;
            }
          case ObjectType.IdentificationServer:
            using (IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient)))
            {
              identificationServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceIdentification/IdentificationServer");
              identificationServerClient.Open();
              identificationServerClient.SetDevice(id, Enumerable.ToDictionary<DictionaryEntry, object, object>(Enumerable.Cast<DictionaryEntry>((IEnumerable) dev.GetData()), (Func<DictionaryEntry, object>) (d => d.Key), (Func<DictionaryEntry, object>) (d => d.Value)), delete);
              break;
            }
          case ObjectType.ExtractorServer:
            using (ExtractorServerClient extractorServerClient = new ExtractorServerClient(new InstanceContext((object) ManagmentServer.MainExtractorClient)))
            {
              extractorServerClient.Endpoint.Address = new EndpointAddress(str + "/FaceExtractorServer/ExtractorServer");
              extractorServerClient.Open();
              extractorServerClient.SetDevice(id, Enumerable.ToDictionary<DictionaryEntry, object, object>(Enumerable.Cast<DictionaryEntry>((IEnumerable) dev.GetData()), (Func<DictionaryEntry, object>) (d => d.Key), (Func<DictionaryEntry, object>) (d => d.Value)), delete);
              break;
            }
        }
      }
      catch (CommunicationException ex)
      {
        ManagmentServer.Logger.Error((object) ("Server Unavailable " + str));
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
    }

    public static void SendLocalDevice(Guid id, bool delete)
    {
      try
      {
        BcDevices dev = BcDevicesStorageExtensions.LoadById(id);
        ParallelEnumerable.ForAll<IServer>(ParallelEnumerable.AsParallel<IServer>((IEnumerable<IServer>) ManagmentServer.Servers), (Action<IServer>) (server => ManagmentServer.SetLocalDevice(server, id, dev, delete)));
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Error", ex);
      }
    }

    public static void VideoWork(object vserver)
    {
      while (!ManagmentServer.StopFlag)
      {
        BcVideoServer bcVideoServer = (BcVideoServer) vserver;
        try
        {
          DataTable dataTable = new DataTable();
          dataTable.TableName = "DeviceInfo";
          dataTable.Columns.Add("ID", typeof (Guid));
          dataTable.Columns.Add("DeviceState", typeof (string));
          dataTable.Columns.Add("FrameCount", typeof (int));
          dataTable.Columns.Add("DetectionCount", typeof (int));
          dataTable.Columns.Add("DetectionFaces", typeof (int));
          dataTable.Columns.Add("VSState", typeof (string));
          dataTable.Columns.Add("CPUUsage", typeof (int));
          dataTable.Columns.Add("ThreadCount", typeof (int));
          dataTable.Columns.Add("MaxThreadCount", typeof (int));
          VideoServerClient videoServerClient = new VideoServerClient();
          videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
          videoServerClient.Open();
          while (true)
          {
            try
            {
              dataTable.Rows.Clear();
              bcVideoServer.State = "Работает";
              DataTable deviceInfo1 = videoServerClient.GetDeviceInfo();
              int num = 0;
              foreach (DataRow dataRow1 in (InternalDataCollectionBase) deviceInfo1.Rows)
              {
                DataRow dataRow2 = (DataRow) null;
                if (dataRow1[0].ToString() != Guid.Empty.ToString())
                {
                  dataTable.Rows.Add((object[]) dataRow1.ItemArray.Clone());
                  dataRow2 = dataTable.Rows.Add((object[]) dataRow1.ItemArray.Clone());
                }
                if (num == 0)
                {
                  bcVideoServer.MaxThreadsCount = Convert.ToInt32(dataRow1["MaxThreadCount"]);
                  bcVideoServer.IsAlive = true;
                  bcVideoServer.CurrentProcessorLoad = Convert.ToInt32(dataRow1["CPUUsage"]);
                  bcVideoServer.CurrentThreadCount = Convert.ToInt32(dataRow1["ThreadCount"]);
                }
                foreach (BcDevices bcDevices in ManagmentServer.Devices)
                {
                  if (bcDevices.Id.ToString() == dataRow2["ID"].ToString())
                  {
                    bcDevices.FrameCount = (int) dataRow2["FrameCount"];
                    if (bcDevices.Dsid == Guid.Empty)
                    {
                      bcDevices.DetectionCount = (int) dataRow2["DetectionCount"];
                      bcDevices.DetectionFaces = (int) dataRow2["DetectionFaces"];
                    }
                    bcDevices.IsAccesable = dataRow1["DeviceState"].ToString() == "Работает";
                  }
                }
                foreach (DeviceInfo deviceInfo2 in ManagmentServer.CommonDeviceInfo)
                {
                  if (deviceInfo2.Id.ToString() == dataRow2["ID"].ToString())
                  {
                    deviceInfo2.VsState = true;
                    deviceInfo2.FrameCount = (int) dataRow2["FrameCount"];
                    if (deviceInfo2.Dsid == Guid.Empty)
                    {
                      deviceInfo2.DetectionCount = (int) dataRow2["DetectionCount"];
                      deviceInfo2.DetectionFaces = (int) dataRow2["DetectionFaces"];
                    }
                    deviceInfo2.DeviceState = dataRow1["DeviceState"].ToString() == "Работает";
                  }
                }
                ++num;
              }
              deviceInfo1.Dispose();
            }
            catch (Exception ex)
            {
              ManagmentServer.Logger.Info((object) ("VideoWork errors  Видео сервер " + (object) bcVideoServer.Ip + " - Error" + (string) (object) bcVideoServer.Number + " -- " + ex.Message + ", Source:" + ex.Source));
              bcVideoServer.State = "Не работает";
              bcVideoServer.IsAlive = false;
              using (List<DeviceInfo>.Enumerator enumerator = ManagmentServer.CommonDeviceInfo.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  DeviceInfo current = enumerator.Current;
                  if (current.Vsid == bcVideoServer.Id)
                  {
                    current.VsState = false;
                    current.DeviceState = false;
                  }
                }
                break;
              }
            }
            Thread.Sleep(500);
          }
          videoServerClient.Close();
        }
        catch (Exception ex)
        {
          bcVideoServer.State = "Не работает";
          bcVideoServer.IsAlive = false;
          foreach (DeviceInfo deviceInfo in ManagmentServer.CommonDeviceInfo)
          {
            if (deviceInfo.Vsid == bcVideoServer.Id)
            {
              deviceInfo.VsState = false;
              deviceInfo.DeviceState = false;
            }
          }
          ManagmentServer.Logger.Error((object) "VideoWork Error ", ex);
        }
        Thread.Sleep(10000);
      }
    }

    public static void DetectorWork(object dserver)
    {
      while (!ManagmentServer.StopFlag)
      {
        DataTable dataTable = new DataTable();
        dataTable.TableName = "DResults";
        dataTable.Columns.Add("ID", typeof (Guid));
        dataTable.Columns.Add("DetectionCount", typeof (int));
        dataTable.Columns.Add("DetectionFaces", typeof (int));
        dataTable.Columns.Add("DSState", typeof (string));
        dataTable.Columns.Add("CPUUsage", typeof (int));
        dataTable.Columns.Add("ThreadCount", typeof (int));
        dataTable.Columns.Add("MaxThreadCount", typeof (int));
        dataTable.Columns.Add("FrameCount", typeof (int));
        BcDetectorServer bcDetectorServer = (BcDetectorServer) dserver;
        try
        {
          using (DetectorServerClient detectorServerClient = new DetectorServerClient(new InstanceContext((object) ManagmentServer.MainDetectorClient)))
          {
            detectorServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcDetectorServer.Ip + ":" + (string) (object) bcDetectorServer.Port + "/CSDetectorServer/DetectorServer");
            detectorServerClient.Open();
            while (true)
            {
              try
              {
                bcDetectorServer.State = "Работает";
                bcDetectorServer.IsAlive = true;
                DataTable deviceInfo1 = detectorServerClient.GetDeviceInfo();
                int num = 0;
                foreach (DataRow dataRow1 in (InternalDataCollectionBase) deviceInfo1.Rows)
                {
                  DataRow dataRow2 = (DataRow) null;
                  if (dataRow1[0].ToString() != Guid.Empty.ToString())
                  {
                    dataTable.Rows.Add((object[]) dataRow1.ItemArray.Clone());
                    dataRow2 = dataTable.Rows.Add((object[]) dataRow1.ItemArray.Clone());
                  }
                  if (num == 0)
                  {
                    bcDetectorServer.MaxThreadsCount = Convert.ToInt32(dataRow1["MaxThreadCount"]);
                    bcDetectorServer.IsAlive = true;
                    bcDetectorServer.CurrentProcessorLoad = Convert.ToInt32(dataRow1["CPUUsage"]);
                    bcDetectorServer.CurrentThreadCount = Convert.ToInt32(dataRow1["ThreadCount"]);
                  }
                  if (dataRow2 != null)
                  {
                    foreach (BcDevices bcDevices in ManagmentServer.Devices)
                    {
                      if (bcDevices.Id.ToString() == dataRow2["ID"].ToString())
                      {
                        bcDevices.DetectionCount = (int) dataRow2["DetectionCount"];
                        bcDevices.DetectionFaces = (int) dataRow2["DetectionFaces"];
                        bcDevices.DetectionFaces = (int) dataRow2["DetectionFaces"];
                        break;
                      }
                    }
                    foreach (DeviceInfo deviceInfo2 in ManagmentServer.CommonDeviceInfo)
                    {
                      if (deviceInfo2.Id.ToString() == dataRow2["ID"].ToString())
                      {
                        deviceInfo2.DetectionCount = (int) dataRow2["DetectionCount"];
                        deviceInfo2.DetectionFaces = (int) dataRow2["DetectionFaces"];
                        deviceInfo2.DetectionFaces = (int) dataRow2["DetectionFaces"];
                        deviceInfo2.DsState = true;
                        break;
                      }
                    }
                  }
                  ++num;
                }
                deviceInfo1.Dispose();
              }
              catch (Exception ex)
              {
                ManagmentServer.Logger.Error((object) "DetectorWork Error ", ex);
                bcDetectorServer.State = "Не работает";
                bcDetectorServer.IsAlive = false;
                using (List<DeviceInfo>.Enumerator enumerator = ManagmentServer.CommonDeviceInfo.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    DeviceInfo current = enumerator.Current;
                    if (current.Dsid == bcDetectorServer.Id)
                    {
                      current.DsState = false;
                      break;
                    }
                  }
                  break;
                }
              }
              Thread.Sleep(2500);
            }
          }
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "DetectorWork Error ", ex);
          bcDetectorServer.State = "Не работает";
          bcDetectorServer.IsAlive = false;
          foreach (DeviceInfo deviceInfo in ManagmentServer.CommonDeviceInfo)
          {
            if (deviceInfo.Dsid == bcDetectorServer.Id)
            {
              deviceInfo.DsState = false;
              break;
            }
          }
        }
        Thread.Sleep(10000);
      }
    }

    public static void SaveStatistic()
    {
      while (!ManagmentServer.StopFlag)
      {
        Thread.Sleep(20000);
        foreach (BcDevices bcDevices in ManagmentServer.Devices)
        {
          try
          {
            BcIdentifierResults identifierResults = new BcIdentifierResults()
            {
              Id = -1,
              DeviceId = bcDevices.Id,
              FaceCount = bcDevices.LastSaveFace == 0 ? bcDevices.DetectionFaces : bcDevices.DetectionFaces - bcDevices.LastSaveFace,
              ResultCount = bcDevices.LastSaveResult == 0 ? bcDevices.ResultCount : bcDevices.ResultCount - bcDevices.LastSaveResult
            };
            bcDevices.LastSaveFace = bcDevices.DetectionFaces;
            bcDevices.LastSaveResult = bcDevices.ResultCount;
            if (identifierResults.FaceCount < 0)
              identifierResults.FaceCount = 0;
            if (identifierResults.ResultCount < 0)
              identifierResults.ResultCount = 0;
            identifierResults.Save();
          }
          catch (Exception ex)
          {
            ManagmentServer.Logger.Error((object) "Error", ex);
          }
        }
      }
    }

    public static void ExtractorWork(object eserver)
    {
      while (!ManagmentServer.StopFlag)
      {
        BcExtractorServer bcExtractorServer = (BcExtractorServer) eserver;
        try
        {
          using (DataTable dataTable = new DataTable())
          {
            dataTable.TableName = "EResults";
            dataTable.Columns.Add("ID", typeof (Guid));
            dataTable.Columns.Add("ExtractCount", typeof (int));
            dataTable.Columns.Add("ESState", typeof (string));
            dataTable.Columns.Add("CPUUsage", typeof (int));
            dataTable.Columns.Add("ThreadCount", typeof (int));
            dataTable.Columns.Add("MaxThreadCount", typeof (int));
            using (ExtractorServerClient extractorServerClient = new ExtractorServerClient(new InstanceContext((object) ManagmentServer.MainExtractorClient)))
            {
              extractorServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcExtractorServer.Ip + ":" + (string) (object) bcExtractorServer.Port + "/FaceExtractorServer/ExtractorServer");
              extractorServerClient.Open();
              while (!ManagmentServer.StopFlag)
              {
                try
                {
                  bcExtractorServer.State = "Работает";
                  using (DataTable deviceInfo1 = extractorServerClient.GetDeviceInfo())
                  {
                    int num = 0;
                    foreach (DataRow dataRow in (InternalDataCollectionBase) deviceInfo1.Rows)
                    {
                      if (dataRow[0].ToString() != Guid.Empty.ToString())
                        dataTable.Rows.Add((object[]) dataRow.ItemArray.Clone());
                      if (num == 0)
                      {
                        bcExtractorServer.MaxThreadsCount = Convert.ToInt32(dataRow["MaxThreadCount"]);
                        bcExtractorServer.IsAlive = true;
                        bcExtractorServer.CurrentProcessorLoad = Convert.ToInt32(dataRow["CPUUsage"]);
                        bcExtractorServer.CurrentThreadCount = Convert.ToInt32(dataRow["ThreadCount"]);
                      }
                      foreach (BcDevices bcDevices in ManagmentServer.Devices)
                      {
                        if (bcDevices.Id.ToString() == dataRow["ID"].ToString())
                          bcDevices.ExtractCount = (int) dataRow["ExtractCount"];
                      }
                      foreach (DeviceInfo deviceInfo2 in ManagmentServer.CommonDeviceInfo)
                      {
                        if (deviceInfo2.Id.ToString() == dataRow["ID"].ToString())
                        {
                          deviceInfo2.ExtractCount = (int) dataRow["ExtractCount"];
                          deviceInfo2.EsState = true;
                        }
                      }
                      ++num;
                    }
                  }
                }
                catch (Exception ex)
                {
                  ManagmentServer.Logger.Error((object) "ExtractorWork Error", ex);
                  using (List<DeviceInfo>.Enumerator enumerator = ManagmentServer.CommonDeviceInfo.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      DeviceInfo current = enumerator.Current;
                      if (current.Esid == bcExtractorServer.Id)
                        current.EsState = false;
                    }
                    break;
                  }
                }
                Thread.Sleep(2500);
              }
            }
          }
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "ExtractorWork Error - ", ex);
          bcExtractorServer.IsAlive = false;
          bcExtractorServer.State = "Не работает";
          foreach (DeviceInfo deviceInfo in ManagmentServer.CommonDeviceInfo)
          {
            if (deviceInfo.Esid == bcExtractorServer.Id)
              deviceInfo.EsState = false;
          }
        }
        Thread.Sleep(10000);
      }
    }

    public static void IdentificationWork(object iserver)
    {
      while (!ManagmentServer.StopFlag)
      {
        BcIdentificationServer identificationServer = (BcIdentificationServer) iserver;
        try
        {
          using (IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient)))
          {
            identificationServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) identificationServer.Ip + ":" + (string) (object) identificationServer.Port + "/FaceIdentification/IdentificationServer");
            identificationServerClient.Open();
            identificationServer.State = "Работает";
            while (true)
            {
              try
              {
                DataTable deviceInfo1 = identificationServerClient.GetDeviceInfo();
                int num = 0;
                foreach (DataRow dataRow in (InternalDataCollectionBase) deviceInfo1.Rows)
                {
                  if (num == 0)
                  {
                    identificationServer.MaxThreadsCount = Convert.ToInt32(dataRow["MaxThreadCount"]);
                    identificationServer.IsAlive = true;
                    identificationServer.CurrentProcessorLoad = Convert.ToInt32(dataRow["CPUUsage"]);
                    identificationServer.CurrentThreadCount = Convert.ToInt32(dataRow["ThreadCount"]);
                  }
                  foreach (BcDevices bcDevices in ManagmentServer.Devices)
                  {
                    if (bcDevices.Id.ToString() == dataRow["ID"].ToString())
                    {
                      bcDevices.ResultCount = (int) dataRow["ResultCount"];
                      bcDevices.IdentifierCount = (int) dataRow["IdentifierCount"];
                    }
                  }
                  foreach (DeviceInfo deviceInfo2 in ManagmentServer.CommonDeviceInfo)
                  {
                    if (deviceInfo2.Id.ToString() == dataRow["ID"].ToString())
                    {
                      deviceInfo2.ResultCount = (int) dataRow["ResultCount"];
                      deviceInfo2.IdentifierCount = (int) dataRow["IdentifierCount"];
                      deviceInfo2.IsState = true;
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                ManagmentServer.Logger.Error((object) "IdentificationWork", ex);
                identificationServer.State = "Не работает";
                identificationServer.IsAlive = false;
                using (List<DeviceInfo>.Enumerator enumerator = ManagmentServer.CommonDeviceInfo.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    DeviceInfo current = enumerator.Current;
                    if (current.Isid == identificationServer.Id)
                      current.IsState = false;
                  }
                  break;
                }
              }
              Thread.Sleep(2500);
            }
          }
        }
        catch (Exception ex)
        {
          if (ex is CommunicationException)
            ManagmentServer.Logger.Error((object) "CommunicationException");
          else
            ManagmentServer.Logger.Error((object) "IdentificationWork", ex);
          identificationServer.State = "Не работает";
          identificationServer.IsAlive = false;
          foreach (DeviceInfo deviceInfo in ManagmentServer.CommonDeviceInfo)
          {
            if (deviceInfo.Isid == identificationServer.Id)
              deviceInfo.IsState = false;
          }
        }
        Thread.Sleep(1000);
      }
    }

    private static void devInfo_NewDeviceState(DeviceInfo info, bool newState, bool oldState)
    {
      try
      {
        if (ManagmentServer.CnapiObj == null)
          return;
        ManagmentServer.CnapiObj.SendOnCamState(new OnCamState()
        {
          messageId = Guid.NewGuid(),
          camID = info.Id,
          stateCode = newState ? 1 : 2,
          statusCode = newState ? 0 : 13
        });
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "CnapiObj Error", ex);
      }
    }

    public static void RefreshAll()
    {
      while (!ManagmentServer.StopFlag)
      {
        ManagmentServer.RefreshDevices();
        ManagmentServer.RefreshServers(new Func<IEnumerable<IServer>>(BcVideoServer.LoadAll), ObjectType.VideoServer, new Action<object>(ManagmentServer.VideoWork));
        ManagmentServer.RefreshServers(new Func<IEnumerable<IServer>>(BcDetectorServer.LoadAll), ObjectType.DetectorServer, new Action<object>(ManagmentServer.DetectorWork));
        ManagmentServer.RefreshServers(new Func<IEnumerable<IServer>>(BcExtractorServer.LoadAll), ObjectType.ExtractorServer, new Action<object>(ManagmentServer.ExtractorWork));
        ManagmentServer.RefreshServers(new Func<IEnumerable<IServer>>(BcIdentificationServer.LoadAll), ObjectType.IdentificationServer, new Action<object>(ManagmentServer.IdentificationWork));
        Thread.Sleep(10000);
      }
    }

    private static void RefreshServers(Func<IEnumerable<IServer>> loadFunc, ObjectType type, Action<object> worker)
    {
      try
      {
        IServer[] serverArray1 = Enumerable.ToArray<IServer>(loadFunc());
        IServer[] serverArray2 = Enumerable.ToArray<IServer>(Enumerable.Where<IServer>((IEnumerable<IServer>) ManagmentServer.Servers, (Func<IServer, bool>) (s => s.Type == type)));
        foreach (IServer server1 in serverArray1)
        {
          IServer loadedServer = server1;
          IServer server2 = Enumerable.FirstOrDefault<IServer>((IEnumerable<IServer>) serverArray2, (Func<IServer, bool>) (s => s.Id == loadedServer.Id));
          if (server2 == null || (server2.Ip != loadedServer.Ip || server2.Port != loadedServer.Port))
            ManagmentServer.CheckAndAddServer(loadedServer.Id, loadedServer.Type, (Func<Guid, IServer>) (guid => loadedServer), worker);
        }
        for (int index = 0; index < serverArray2.Length; ++index)
        {
          IServer cachedServer = serverArray2[index];
          if (!Enumerable.Any<IServer>((IEnumerable<IServer>) serverArray1, (Func<IServer, bool>) (s => s.Id == cachedServer.Id)))
          {
            try
            {
              cachedServer.Stop();
            }
            catch (Exception ex)
            {
              ManagmentServer.Logger.Error((object) "Error", ex);
            }
            ManagmentServer.Servers.Remove(cachedServer);
            ManagmentServer.TrySendServerMessage(cachedServer, 8, ObjEventType.Delete);
          }
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "refresh serveres error", ex);
      }
    }

    private static void RefreshDevices()
    {
      try
      {
        List<BcDevices> list = BcDevicesStorageExtensions.LoadAll();
        foreach (BcDevices bcDevices1 in list)
        {
          bool flag = false;
          foreach (BcDevices bcDevices2 in ManagmentServer.Devices)
          {
            if (bcDevices2.Id == bcDevices1.Id)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            ManagmentServer.Devices.Add(bcDevices1);
            bcDevices1.IsAccesable = false;
            DeviceInfo deviceInfo = new DeviceInfo()
            {
              DetectionCount = bcDevices1.DetectionCount,
              DetectionFaces = bcDevices1.DetectionFaces,
              DeviceState = false,
              Dsid = bcDevices1.Dsid,
              DsState = false,
              Esid = bcDevices1.Esid,
              EsState = false,
              ExtractCount = bcDevices1.ExtractCount,
              FrameCount = bcDevices1.FrameCount,
              Id = bcDevices1.Id,
              IdentifierCount = bcDevices1.IdentifierCount,
              Isid = bcDevices1.Isid,
              IsState = false,
              ResultCount = bcDevices1.ResultCount,
              Vsid = bcDevices1.Vsid,
              VsState = false
            };
            try
            {
              if (ManagmentServer.CnapiObj != null)
                ManagmentServer.CnapiObj.SendOnCamState(new OnCamState()
                {
                  messageId = Guid.NewGuid(),
                  camID = deviceInfo.Id,
                  stateCode = 8,
                  statusCode = 0
                });
            }
            catch (Exception ex)
            {
              ManagmentServer.Logger.Error((object) "Error", ex);
            }
            deviceInfo.NewDeviceState += new NewDeviceStateEventHandler(ManagmentServer.devInfo_NewDeviceState);
            ManagmentServer.CommonDeviceInfo.Add(deviceInfo);
            ManagmentServer.TrySendMessage(new SystemMessageBody()
            {
              MessageCode = 14,
              MessageSeverity = MessageSeverity.Info,
              MessageText = MessageConstructor.GetMessageString(bcDevices1.Name, bcDevices1.Id, bcDevices1.IsAccesable, ObjectType.Device, ObjEventType.Add)
            });
            bcDevices1.StateChanged += new EventHandler<StateChangedEventArgs>(ManagmentServer.dev_StateChanged);
          }
        }
        for (int index = 0; index < ManagmentServer.Devices.Count; ++index)
        {
          bool flag = false;
          BcDevices bcDevices1 = ManagmentServer.Devices[index];
          foreach (BcDevices bcDevices2 in list)
          {
            if (bcDevices2.Id == bcDevices1.Id)
              flag = true;
          }
          if (!flag)
          {
            ManagmentServer.TrySendMessage(new SystemMessageBody()
            {
              MessageCode = 7,
              MessageSeverity = MessageSeverity.Info,
              MessageText = MessageConstructor.GetMessageString(bcDevices1.Name, bcDevices1.Id, bcDevices1.IsAccesable, ObjectType.Device, ObjEventType.Delete)
            });
            ManagmentServer.Devices.RemoveAt(index);
            --index;
          }
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Refresh devices error", ex);
      }
    }

    private static IServer CheckAndAddServer(Guid id, ObjectType type, Func<Guid, IServer> loadFunc, Action<object> worker)
    {
      try
      {
        IServer server = loadFunc(id);
        if (server.Id == Guid.Empty)
        {
          ManagmentServer.Logger.WarnFormat("Server with id = {0} not found", (object) id);
          return (IServer) null;
        }
        IServer server1 = Enumerable.FirstOrDefault<IServer>((IEnumerable<IServer>) ManagmentServer.Servers, (Func<IServer, bool>) (s => s.Id == server.Id && s.Type == type));
        if (server1 != null)
        {
          server.IsAlive = server1.IsAlive;
          ManagmentServer.TrySendServerMessage(server, 15, ObjEventType.Update);
        }
        else
        {
          ManagmentServer.TrySendServerMessage(server, 16, ObjEventType.Add);
          server.StateChanged += new EventHandler<StateChangedEventArgs>(ManagmentServer.dev_StateChanged);
          server.Start(worker, (object) server);
          ManagmentServer.Servers.Add(server);
        }
        return server;
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "SetServer", ex);
      }
      return (IServer) null;
    }

    private static void LoadStats(CNAPIclr.Messages.Node node)
    {
      node.blockdev = new Blockdev();
      node.blockdev.block = new List<Block>();
      PerformanceCounterCategory performanceCounterCategory1 = new PerformanceCounterCategory("PhysicalDisk");
      foreach (string instanceName in performanceCounterCategory1.GetInstanceNames())
      {
        Block block = new Block();
        node.blockdev.block.Add(block);
        block.ident = instanceName;
        block.name = instanceName;
        foreach (PerformanceCounter performanceCounter in performanceCounterCategory1.GetCounters(instanceName))
        {
          if (performanceCounter.CounterName == "Disk Reads/sec")
          {
            double num = (double) performanceCounter.NextValue();
            Thread.Sleep(200);
            block.reads = performanceCounter.NextValue().ToString();
          }
          if (performanceCounter.CounterName == "Disk Writes/sec")
          {
            double num = (double) performanceCounter.NextValue();
            Thread.Sleep(200);
            block.wrtns = performanceCounter.NextValue().ToString();
          }
          if (performanceCounter.CounterName == "Disk Read Bytes/sec")
          {
            double num = (double) performanceCounter.NextValue();
            Thread.Sleep(200);
            block.read = performanceCounter.NextValue().ToString();
          }
          if (performanceCounter.CounterName == "Disk Write Bytes/sec")
          {
            double num = (double) performanceCounter.NextValue();
            Thread.Sleep(200);
            block.write = performanceCounter.NextValue().ToString();
          }
          if (performanceCounter.CounterName == "Avg. Disk Bytes/Transfer")
          {
            double num = (double) performanceCounter.NextValue();
            Thread.Sleep(200);
            block.tps = performanceCounter.NextValue().ToString();
          }
          performanceCounter.Dispose();
        }
      }
      PerformanceCounterCategory performanceCounterCategory2 = new PerformanceCounterCategory("Processor");
      node.CPU = new CPU();
      node.CPU.CPUS = new List<CPU>();
      node.CPU.count = Environment.ProcessorCount.ToString();
      foreach (string str in performanceCounterCategory2.GetInstanceNames())
      {
        int result;
        if (int.TryParse(str, out result))
        {
          CPU cpu1 = new CPU();
          cpu1.index = str;
          cpu1.load = new Load();
          foreach (PerformanceCounter performanceCounter in performanceCounterCategory2.GetCounters(str))
          {
            if (performanceCounter.CounterName == "% Processor Time")
            {
              double num = (double) performanceCounter.NextValue();
              Thread.Sleep(100);
              cpu1.load.curr = performanceCounter.NextValue();
              foreach (CPU cpu2 in ManagmentServer.Node1.CPU.CPUS)
              {
                if (cpu2.load != null && cpu2.index == cpu1.index)
                  cpu1.load.curr1 = cpu2.load.curr;
              }
              foreach (CPU cpu2 in ManagmentServer.Node15.CPU.CPUS)
              {
                if (cpu2.load != null && cpu2.index == cpu1.index)
                  cpu1.load.curr15 = cpu2.load.curr;
              }
            }
            performanceCounter.Dispose();
          }
          node.CPU.CPUS.Add(cpu1);
        }
        else
        {
          node.CPU.load = new Load();
          foreach (PerformanceCounter performanceCounter in performanceCounterCategory2.GetCounters(str))
          {
            if (performanceCounter.CounterName == "% Processor Time")
            {
              double num = (double) performanceCounter.NextValue();
              Thread.Sleep(100);
              node.CPU.load.curr = performanceCounter.NextValue();
              if (ManagmentServer.Node1.CPU.load != null)
                node.CPU.load.curr1 = ManagmentServer.Node1.CPU.load.curr;
              if (ManagmentServer.Node15.CPU.load != null)
                node.CPU.load.curr15 = ManagmentServer.Node15.CPU.load.curr;
            }
            performanceCounter.Dispose();
          }
        }
      }
      foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("select CurrentClockSpeed,MaxClockSpeed,Availability from Win32_Processor").Get())
      {
        node.CPU.maxfreq = managementBaseObject["MaxClockSpeed"].ToString();
        node.CPU.currfreq = managementBaseObject["CurrentClockSpeed"].ToString();
        node.CPU.mode = ((ManagmentServer.ProcessorMode) Convert.ToInt32(managementBaseObject["Availability"])).ToString();
      }
      node.netdev = new Netdev();
      node.netdev.Iface = new List<Iface>();
      foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        IPInterfaceStatistics ipStatistics = networkInterface.GetIPStatistics();
        node.netdev.Iface.Add(new Iface()
        {
          Ident = networkInterface.Name + " - " + networkInterface.Id,
          Type = networkInterface.NetworkInterfaceType.ToString(),
          RxDataRate = ipStatistics.BytesReceived / 1024L,
          RxErrsRate = ipStatistics.IncomingPacketsWithErrors,
          RxPktRate = ipStatistics.NonUnicastPacketsReceived + ipStatistics.UnicastPacketsReceived,
          TxDataRate = ipStatistics.BytesSent / 1024L,
          TxErrsRate = ipStatistics.OutgoingPacketsWithErrors,
          TxPktRate = ipStatistics.NonUnicastPacketsSent + ipStatistics.UnicastPacketsSent,
          TxCollRate = ipStatistics.OutputQueueLength,
          RxOverRate = ipStatistics.IncomingPacketsDiscarded
        });
      }
      ComputerInfo computerInfo = new ComputerInfo();
      node.memory = new Memory();
      node.memory.avail = computerInfo.AvailablePhysicalMemory.ToString();
      Memory memory1 = node.memory;
      ulong num1 = computerInfo.AvailableVirtualMemory;
      string str1 = num1.ToString();
      memory1.swapavail = str1;
      Memory memory2 = node.memory;
      num1 = computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory;
      string str2 = num1.ToString();
      memory2.use = str2;
      Memory memory3 = node.memory;
      num1 = computerInfo.TotalVirtualMemory - computerInfo.AvailableVirtualMemory;
      string str3 = num1.ToString();
      memory3.swapuse = str3;
      node.cameras = new List<cam>();
      foreach (BcDevices bcDevices in ManagmentServer.Devices)
        node.cameras.Add(new cam()
        {
          objnum = bcDevices.Id,
          status = bcDevices.IsAccesable ? 0 : 1,
          state = bcDevices.IsAccesable ? "Работает" : "Не работает"
        });
    }

    public static void PerfomanceWork()
    {
      int num = 0;
      while (true)
      {
        if (num % 15 == 0)
        {
          ManagmentServer.Node15 = new CNAPIclr.Messages.Node();
          ManagmentServer.Node15.blockdev = ManagmentServer.NodeBuf.blockdev;
          ManagmentServer.Node15.CPU = ManagmentServer.NodeBuf.CPU;
          ManagmentServer.Node15.memory = ManagmentServer.NodeBuf.memory;
          ManagmentServer.Node15.netdev = ManagmentServer.NodeBuf.netdev;
          ManagmentServer.NodeBuf = new CNAPIclr.Messages.Node();
          ManagmentServer.NodeBuf.blockdev = ManagmentServer.CurrentNode.blockdev;
          ManagmentServer.NodeBuf.CPU = ManagmentServer.CurrentNode.CPU;
          ManagmentServer.NodeBuf.memory = ManagmentServer.CurrentNode.memory;
          ManagmentServer.NodeBuf.netdev = ManagmentServer.CurrentNode.netdev;
        }
        ManagmentServer.Node1.blockdev = ManagmentServer.CurrentNode.blockdev;
        ManagmentServer.Node1.CPU = ManagmentServer.CurrentNode.CPU;
        ManagmentServer.Node1.memory = ManagmentServer.CurrentNode.memory;
        ManagmentServer.Node1.netdev = ManagmentServer.CurrentNode.netdev;
        ManagmentServer.CurrentNode = new CNAPIclr.Messages.Node();
        ManagmentServer.LoadStats(ManagmentServer.CurrentNode);
        ++num;
        Thread.Sleep(60000);
      }
    }

    public static void StatsWork()
    {
      CNAPIclr.Messages.Node node = new CNAPIclr.Messages.Node();
      while (true)
      {
        if (ManagmentServer.UpdateInterval > 0)
        {
          ManagmentServer.LoadStats(node);
          StatsInform StatsInform = new StatsInform();
          StatsInform.node = node;
          StatsInform.messageId = Guid.NewGuid();
          try
          {
            using (SqlCommand sqlCommand1 = new SqlCommand())
            {
              using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionString))
              {
                sqlCommand1.Connection = sqlConnection;
                sqlCommand1.CommandText = "Select State, Count(ID) as ModelCount from Faces group by State ";
                SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
                while (sqlDataReader1.Read())
                {
                  if (Convert.ToInt32(sqlDataReader1[0]) == 0)
                    StatsInform.hotlist = Convert.ToInt32(sqlDataReader1[1]);
                  else
                    StatsInform.pasvlist = Convert.ToInt32(sqlDataReader1[2]);
                }
                sqlDataReader1.Close();
                sqlDataReader1.Dispose();
                sqlCommand1.CommandText = "Select ID from Faces";
                SqlDataReader sqlDataReader2 = sqlCommand1.ExecuteReader();
                StatsInform.archive = new archive();
                StatsInform.archive.model = new List<model>();
                while (sqlDataReader2.Read())
                {
                  using (SqlCommand sqlCommand2 = new SqlCommand("Select  Date, DeviceID from CSLogInfo.dbo.Log\r\n                            Where FaceID = @ID order by Date", new SqlConnection(CommonSettings.ConnectionString)))
                  {
                    sqlCommand2.Parameters.Add(new SqlParameter("@ID", (object) (Guid) sqlDataReader2["ID"]));
                    sqlCommand2.Connection.Open();
                    SqlDataReader sqlDataReader3 = sqlCommand1.ExecuteReader();
                    model model = new model();
                    bool flag = false;
                    model.hitdates = new List<hitdates>();
                    while (sqlDataReader3.Read())
                    {
                      flag = true;
                      model.hitdates.Add(new hitdates()
                      {
                        cam = sqlDataReader3["DeviceID"].ToString(),
                        hitdate = BcFace.GetDate((DateTime) sqlDataReader3["Date"])
                      });
                    }
                    sqlDataReader3.Close();
                    if (flag)
                    {
                      model.hitcount = model.hitdates.Count.ToString();
                      StatsInform.archive.model.Add(model);
                    }
                    sqlCommand2.Connection.Close();
                  }
                }
              }
            }
          }
          catch
          {
          }
          ManagmentServer.CnapiObj.SendStatsInform(StatsInform);
          Thread.Sleep(ManagmentServer.UpdateInterval * 1000);
        }
        else
          Thread.Sleep(1000);
      }
    }

    public static void WorkModels()
    {
      while (!ManagmentServer.StopFlag)
      {
        try
        {
          if (ManagmentServer.CnapiObj != null && ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
          {
            foreach (BcFace bcFace in Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => face.FaceData.EndDate != ManagmentServer.NullDate && !face.FaceData.SentExpired && face.FaceData.EndDate < DateTime.Now)))
            {
              ManagmentServer.SendOnModelState(ManagmentServer.CnapiObj, Guid.NewGuid(), bcFace.Id);
              bcFace.FaceData.SaveExpired();
              bcFace.FaceData.SentExpired = true;
            }
          }
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "send Model Error", ex);
        }
        Thread.Sleep(5000);
      }
    }

    public static void WriteLastSendId(long id)
    {
      lock (ManagmentServer.WriteLockObj)
      {
        StreamWriter local_0 = new StreamWriter(ManagmentServer.ApplicationFolder + "\\lastsendID.ini", false);
        local_0.WriteLine(id.ToString());
        local_0.Close();
      }
    }

    public static void LoadCnapi()
    {
      try
      {
        Debug.WriteLine("test debug");
        if (System.IO.File.Exists(ManagmentServer.KarsFilename))
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (KarsConfig));
          FileStream fileStream = new FileStream(ManagmentServer.KarsFilename, FileMode.Open, FileAccess.Read);
          ManagmentServer.KarsConfig = (KarsConfig) xmlSerializer.Deserialize((Stream) fileStream);
          fileStream.Close();
        }
        CNAPISettings.AdminDepartment = ManagmentServer.KarsConfig.AdminDepartment;
        CNAPISettings.AdminFio = ManagmentServer.KarsConfig.AdminFio;
        CNAPISettings.AdminPhone = ManagmentServer.KarsConfig.AdminPhone;
        CNAPISettings.AdminPosition = ManagmentServer.KarsConfig.AdminPosition;
        CNAPISettings.ConnectionAddress = ManagmentServer.KarsConfig.ConnectionAddress;
        CNAPISettings.ConnectionPort = ManagmentServer.KarsConfig.ConnectionPort;
        CNAPISettings.HostId = ManagmentServer.KarsConfig.HostId;
        CNAPISettings.IterationsNumber = ManagmentServer.KarsConfig.IterationCount;
        CNAPISettings.KarsTypeId = ManagmentServer.KarsConfig.KarsTypeId;
        CNAPISettings.Major = ManagmentServer.KarsConfig.Version.Major;
        CNAPISettings.Minor = ManagmentServer.KarsConfig.Version.Minor;
        CNAPISettings.Name = ManagmentServer.KarsConfig.Name;
        CNAPISettings.PassPhrase = ManagmentServer.KarsConfig.PassPhrase;
        CNAPISettings.PingInterval = ManagmentServer.KarsConfig.PingInterval;
        CNAPISettings.PingTimeout = ManagmentServer.KarsConfig.PingTimeout;
        CNAPISettings.Salt = ManagmentServer.KarsConfig.Salt;
        CNAPISettings.Shifr = ManagmentServer.KarsConfig.Shifr;
        CNAPISettings.TechDepartment = ManagmentServer.KarsConfig.TechDepartment;
        CNAPISettings.TechFio = ManagmentServer.KarsConfig.TechFio;
        CNAPISettings.TechPhone = ManagmentServer.KarsConfig.TechPhone;
        CNAPISettings.TechPosition = ManagmentServer.KarsConfig.TechPosition;
        CNAPISettings.VendorAddress = ManagmentServer.KarsConfig.VendorAddress;
        CNAPISettings.VendorName = ManagmentServer.KarsConfig.VendorName;
        CNAPIObj.ConnectionString = "tcp://" + CNAPISettings.ConnectionAddress + ":" + CNAPISettings.ConnectionPort;
        ManagmentServer.CnapiObj = new CNAPIObj();
        ManagmentServer.CnapiObj.NewGetModel += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetModel);
        ManagmentServer.CnapiObj.NewSetModelCode += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewSetModelCode);
        ManagmentServer.CnapiObj.NewClearModelCode += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewClearModelCode);
        ManagmentServer.CnapiObj.NewGetModelCount += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetModelCount);
        ManagmentServer.CnapiObj.NewGetActiveModelCount += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetActiveModelCount);
        ManagmentServer.CnapiObj.NewGetModels += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetModels);
        ManagmentServer.CnapiObj.NewDeleteModelFromLists += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewDeleteModelFromLists);
        ManagmentServer.CnapiObj.NewAddModelToHotlist += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewAddModelToHotlist);
        ManagmentServer.CnapiObj.NewMoveModelPassive += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewMoveModelPassive);
        ManagmentServer.CnapiObj.NewMoveModelActive += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewMoveModelActive);
        ManagmentServer.CnapiObj.NewAddModelPassive += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewAddModelPassive);
        ManagmentServer.CnapiObj.NewDelArchive += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewDelArchive);
        ManagmentServer.CnapiObj.NewStartSearch += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewStartSearch);
        ManagmentServer.CnapiObj.NewGetSearch += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetSearch);
        ManagmentServer.CnapiObj.NewStopSearch += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewStopSearch);
        ManagmentServer.CnapiObj.NewGetSearches += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetSearches);
        ManagmentServer.CnapiObj.NewOnSearchHitResponse += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewOnSearchHitResponse);
        ManagmentServer.CnapiObj.NewGetVersion += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetVersion);
        ManagmentServer.CnapiObj.NewGetSystemState += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetSystemState);
        ManagmentServer.CnapiObj.NewGetStatus += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetStatus);
        ManagmentServer.CnapiObj.NewSelfCheck += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewSelfCheck);
        ManagmentServer.CnapiObj.NewGetStats += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetStats);
        ManagmentServer.CnapiObj.NewSetStatsUpdateInterval += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewSetStatsUpdateInterval);
        ManagmentServer.CnapiObj.NewGetCamCount += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetCamCount);
        ManagmentServer.CnapiObj.NewGetActiveCamCount += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetActiveCamCount);
        ManagmentServer.CnapiObj.NewonHotListHitResponse += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewonHotListHitResponse);
        ManagmentServer.CnapiObj.NewGetCamInfo += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewGetCamInfo);
        ManagmentServer.CnapiObj.NewonSearchResponse += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewonSearchResponse);
        ManagmentServer.CnapiObj.NewOnModelStateAck += new CNAPIObj.NewMessageEventHandler(ManagmentServer.CNAPIObj_NewOnModelStateAck);
        ManagmentServer.CnapiObj.WorkType = WorkType.Server;
        ManagmentServer.AllModels = BcFace.LoadAll();
        foreach (BcFace bcFace in ManagmentServer.AllModels)
        {
          bcFace.FaceData = BcFaceData.LoadByFaceId(bcFace.Id);
          bcFace.StateChanged += new BcFace.ModelStateChangedHandler(ManagmentServer.f_StateChanged);
        }
        ManagmentServer.CnapiObj.Start();
        ManagmentServer.StatsThread = new Thread(new ThreadStart(ManagmentServer.StatsWork))
        {
          IsBackground = true
        };
        ManagmentServer.StatsThread.Start();
        ManagmentServer.PerfomanceThread = new Thread(new ThreadStart(ManagmentServer.PerfomanceWork))
        {
          IsBackground = true
        };
        ManagmentServer.PerfomanceThread.Start();
        ManagmentServer.ModelThread = new Thread(new ThreadStart(ManagmentServer.WorkModels))
        {
          IsBackground = true
        };
        ManagmentServer.ModelThread.Start();
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Load CNAPI error", ex);
      }
    }

    private static void CNAPIObj_NewOnModelStateAck(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (f => f.Id == name.OnModelStateAck.model)));
      if (bcFace == null)
        return;
      foreach (Guid guid in bcFace.SentState.ToArray())
      {
        if (guid == name.OnModelStateAck.messageId)
          bcFace.SentState.Remove(guid);
      }
    }

    private static void f_StateChanged(BcFace face, int newState, int oldState)
    {
      ManagmentServer.SendOnModelState(ManagmentServer.CnapiObj, Guid.NewGuid(), face.Id);
    }

    private static void CNAPIObj_NewonSearchResponse(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
    }

    public static int GetModelState(Guid id, out string statusString)
    {
      statusString = "";
      bool flag = false;
      BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => face.Id == id)));
      if (bcFace == null)
        return -1;
      if (bcFace.SentHits.Count > 0)
        flag = true;
      int num1 = -1;
      if (bcFace.FaceData.State == 0)
      {
        statusString = " in active list";
        num1 = CNAPIObj.InActiveList;
      }
      if (bcFace.FaceData.State == 1)
      {
        statusString = " in passive list";
        num1 = CNAPIObj.InPassiveList;
      }
      long num2 = 0;
      long num3 = 0;
      DateTime dateTime;
      if (bcFace.FaceData.StartDate != ManagmentServer.NullDate)
      {
        dateTime = bcFace.FaceData.StartDate;
        num2 = dateTime.Ticks;
      }
      if (bcFace.FaceData.EndDate != ManagmentServer.NullDate)
      {
        dateTime = bcFace.FaceData.EndDate;
        num3 = dateTime.Ticks;
      }
      int num4 = 0;
      long num5 = num3;
      dateTime = DateTime.Now;
      long ticks1 = dateTime.Ticks;
      if (num5 < ticks1)
      {
        statusString += ", Expired";
        num4 = CNAPIObj.Expired;
      }
      else
      {
        long num6 = num2;
        dateTime = DateTime.Now;
        long ticks2 = dateTime.Ticks;
        if (num6 > ticks2)
        {
          statusString += ", Suspended";
          num4 = CNAPIObj.Suspended;
        }
      }
      if (flag)
      {
        statusString += ", Hit";
        num4 |= CNAPIObj.Hit;
      }
      if (num4 != 0)
        return num1 | num4;
      return num1;
    }

    private static void WaitSearchHitResponce(OnSearchHit hit, ManagmentServer.SearchData search)
    {
      while (!ManagmentServer.StopFlag)
      {
        Thread.Sleep(60000);
        if (!search.SentSearchHits.Contains(hit))
          break;
        if (ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
          ManagmentServer.CnapiObj.SendOnSearchHit(hit);
      }
    }

    public static void SearchProcess(ManagmentServer.SearchData search)
    {
      SearchRequest request = new SearchRequest()
      {
        DateBefore = search.EndDate,
        DateFrom = search.StartDate,
        DeleteId = search.DeleteId,
        ModelId = search.ModelId,
        SearchId = search.Id,
        Template = search.Template,
        Score = search.Score,
        ExcludeDates = new List<CommonContracts.ExcludeDate>()
      };
      List<CommonContracts.SearchResult> list = new List<CommonContracts.SearchResult>();
      for (int index = 0; index < search.ExcludeDates.Count; ++index)
      {
        ManagmentServer.ExcludeDate excludeDate = search.ExcludeDates[index];
        request.ExcludeDates.Add(new CommonContracts.ExcludeDate()
        {
          EndDate = excludeDate.EndDate,
          StartDate = excludeDate.StartDate
        });
      }
      int num = 0;
      IServer[] serverArray = Enumerable.ToArray<IServer>(Enumerable.Where<IServer>((IEnumerable<IServer>) ManagmentServer.Servers, (Func<IServer, bool>) (s => s.Type == ObjectType.IdentificationServer)));
      foreach (IServer server in serverArray)
      {
        try
        {
          IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) ManagmentServer.MainIdentClient));
          identificationServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) server.Ip + ":" + (string) (object) server.Port + "/FaceIdentification/IdentificationServer");
          identificationServerClient.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 2);
          identificationServerClient.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
          identificationServerClient.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
          identificationServerClient.Open();
          list.AddRange((IEnumerable<CommonContracts.SearchResult>) identificationServerClient.SearchArchive(request));
          identificationServerClient.Close();
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) "Send Search to Server Error", ex);
        }
        ++num;
        search.Progress = (int) ((double) num / (double) serverArray.Length * 100.0);
      }
      try
      {
        search.ResultCount = list.Count;
        search.State = ManagmentServer.SearchState.Complete;
        foreach (CommonContracts.SearchResult searchResult in list)
        {
          if (search.State != ManagmentServer.SearchState.Stop && ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
          {
            OnSearchHit onSearchHit = new OnSearchHit();
            onSearchHit.messageId = Guid.NewGuid();
            onSearchHit.model = search.ModelId.ToString();
            onSearchHit.searchID = search.Id;
            onSearchHit.results = new List<hitresult>();
            BcLog bcLog = BcLog.LoadById(searchResult.LogId);
            while (ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession && search.State != ManagmentServer.SearchState.Stop)
            {
              onSearchHit.results.Add(new hitresult()
              {
                cam = bcLog.DeviceId,
                hitcount = 1,
                hitdate = BcFace.GetDate(bcLog.Date),
                image = new Image()
                {
                  Base64String = Convert.ToBase64String(bcLog.Image),
                  format = "JPEG"
                },
                prob = searchResult.Score
              });
              if (ManagmentServer.CnapiObj.waitSendRedy.WaitOne(60000))
              {
                ManagmentServer.CnapiObj.SendOnSearchHit(onSearchHit);
                new ManagmentServer.WaitSearchHitResponceFunc(ManagmentServer.WaitSearchHitResponce).BeginInvoke(onSearchHit, search, (AsyncCallback) null, (object) null);
                break;
              }
              Thread.Sleep(30);
            }
          }
          else
            break;
        }
        if (list.Count > 0)
        {
          search.State = ManagmentServer.SearchState.Ending;
          new ManagmentServer.WaitSearchResponceFunc(ManagmentServer.WaitSearchResponce).BeginInvoke(search, (AsyncCallback) null, (object) null);
        }
        else
        {
          search.State = ManagmentServer.SearchState.End;
          search.StateChanged -= new ManagmentServer.SearchData.SearchStateChangeEventHandler(ManagmentServer.search_StateChanged);
          ManagmentServer.CompleteSearchs.Remove(search);
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "Receive Search Result Error", ex);
      }
    }

    private static void WaitSearchResponce(ManagmentServer.SearchData search)
    {
      while (ManagmentServer.StopFlag)
      {
        Thread.Sleep(1000);
        if (search.State == ManagmentServer.SearchState.Stop || ManagmentServer.CnapiObj.SessionState != SessionState.ActiveSession)
          break;
        if (search.SentSearchHits.Count == 0)
        {
          search.State = ManagmentServer.SearchState.End;
          search.StateChanged -= new ManagmentServer.SearchData.SearchStateChangeEventHandler(ManagmentServer.search_StateChanged);
          ManagmentServer.CompleteSearchs.Remove(search);
          break;
        }
      }
    }

    private static void WairModelSentHits(BcFace f)
    {
      while (f.SentHits.Count > 0)
        Thread.Sleep(1000);
      string statusString;
      f.ModelState = ManagmentServer.GetModelState(f.Id, out statusString);
    }

    public static void WaitHitResponce(onHotListHit hit, BcFace f)
    {
      bool flag = true;
      while (flag)
      {
        Thread.Sleep(30000);
        flag = false;
        foreach (Guid guid in f.SentHits.ToArray())
        {
          if (guid == hit.messageId)
            flag = true;
        }
        if (flag && ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
          ManagmentServer.CnapiObj.SendonHotListHit(hit);
      }
    }

    public static void SendCamState(int type)
    {
      for (int index = 0; index < ManagmentServer.Devices.Count; ++index)
      {
        if (type == 1 && ManagmentServer.Devices[index].IsAccesable)
          ManagmentServer.CnapiObj.SendOnCamState(new OnCamState()
          {
            messageId = Guid.NewGuid(),
            camID = ManagmentServer.Devices[index].Id,
            stateCode = ManagmentServer.Devices[index].IsAccesable ? 1 : 2,
            statusCode = ManagmentServer.Devices[index].IsAccesable ? 0 : 13
          });
        else if (type == 0)
          ManagmentServer.CnapiObj.SendOnCamState(new OnCamState()
          {
            messageId = Guid.NewGuid(),
            camID = ManagmentServer.Devices[index].Id,
            stateCode = ManagmentServer.Devices[index].IsAccesable ? 1 : 2,
            statusCode = ManagmentServer.Devices[index].IsAccesable ? 0 : 13
          });
      }
    }

    public static void SendCamInfo()
    {
      for (int index = 0; index < ManagmentServer.Devices.Count; ++index)
        ManagmentServer.CnapiObj.SendOnCamInfo(new OnCamInfo()
        {
          messageId = Guid.NewGuid(),
          camDescr = ManagmentServer.Devices[index].Name,
          camID = ManagmentServer.Devices[index].Id,
          camType = ManagmentServer.Devices[index].Type,
          stateCode = ManagmentServer.Devices[index].IsAccesable ? 1 : 2,
          statusCode = ManagmentServer.Devices[index].IsAccesable ? 0 : 13
        });
    }

    private static void CNAPIObj_NewOnSearchHitResponse(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.SearchData searchData = Enumerable.FirstOrDefault<ManagmentServer.SearchData>(Enumerable.Where<ManagmentServer.SearchData>((IEnumerable<ManagmentServer.SearchData>) ManagmentServer.CompleteSearchs, (Func<ManagmentServer.SearchData, bool>) (s => s.Id == name.onSearchHitResponse.searchID)));
      if (searchData == null)
        return;
      OnSearchHit onSearchHit = Enumerable.FirstOrDefault<OnSearchHit>(Enumerable.Where<OnSearchHit>((IEnumerable<OnSearchHit>) searchData.SentSearchHits, (Func<OnSearchHit, bool>) (h => h.messageId == name.onSearchHitResponse.messageId)));
      if (searchData.SentSearchHits.Contains(onSearchHit))
        searchData.SentSearchHits.Remove(onSearchHit);
    }

    private static void CNAPIObj_NewonHotListHitResponse(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      foreach (BcFace bcFace in ManagmentServer.AllModels)
      {
        if (name.onHotListHitResponse.model == bcFace.Id)
        {
          foreach (Guid guid in bcFace.SentHits.ToArray())
          {
            if (guid == name.onHotListHitResponse.messageId)
            {
              bcFace.SentHits.Remove(guid);
              break;
            }
          }
          break;
        }
      }
    }

    private static void CNAPIObj_NewGetCamInfo(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      obj.SendGetCamInfoResponse(new GetCamInfoResponse()
      {
        messageId = name.GetCamInfo.messageId,
        statusCode = 0,
        statusString = ""
      });
      new ManagmentServer.SendCamInfoFunc(ManagmentServer.SendCamInfo).BeginInvoke((AsyncCallback) null, (object) null);
    }

    private static void CNAPIObj_NewGetActiveCamCount(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      int num = 0;
      for (int index = 0; index < ManagmentServer.Devices.Count; ++index)
      {
        if (ManagmentServer.Devices[index].IsAccesable)
          ++num;
      }
      obj.SendGetActiveCamCountResponse(new GetActiveCamCountResponse()
      {
        messageId = name.GetActiveCamCount.messageId,
        count = num
      });
      new ManagmentServer.SendCamStateFunc(ManagmentServer.SendCamState).BeginInvoke(1, (AsyncCallback) null, (object) null);
    }

    private static void CNAPIObj_NewGetCamCount(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      obj.SendGetCamCountResponse(new GetCamCountResponse()
      {
        messageId = name.GetCamCount.messageId,
        count = ManagmentServer.Devices.Count,
        status = 0,
        statusDescr = "Успешно"
      });
      new ManagmentServer.SendCamStateFunc(ManagmentServer.SendCamState).BeginInvoke(0, (AsyncCallback) null, (object) null);
    }

    private static void CNAPIObj_NewSetStatsUpdateInterval(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      obj.SendSetStatsUpdateIntervalResponse(new SetStatsUpdateIntervalResponse()
      {
        messageId = name.SetStatsUpdateInterval.messageId,
        Timeval = name.SetStatsUpdateInterval.timeval
      });
      ManagmentServer.UpdateInterval = name.SetStatsUpdateInterval.timeval;
    }

    private static void CNAPIObj_NewDelArchive(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.Logger.Info((object) ("DelArchive " + message));
      try
      {
        ManagmentServer.SearchData search = new ManagmentServer.SearchData()
        {
          Id = Guid.NewGuid(),
          State = ManagmentServer.SearchState.Progress,
          ModelId = new Guid(name.DelArchive.model.uuid),
          Score = name.DelArchive.model.probMin
        };
        search.DeleteId = search.Id;
        DateTime dateTime1 = DateTime.MinValue;
        if (name.DelArchive.model.hitdates[0].startDate != "")
        {
          DateTime dateTime2 = BcFace.ConvertDate(name.DelArchive.model.hitdates[0].startDate);
          search.StartDate = dateTime2.Ticks;
          dateTime1 = dateTime2.AddMinutes(-10.0);
        }
        else
          search.StartDate = 0L;
        DateTime dateTime3 = DateTime.MinValue;
        if (name.DelArchive.model.hitdates[0].endDate != "")
        {
          dateTime3 = BcFace.ConvertDate(name.DelArchive.model.hitdates[0].endDate).AddMinutes(-10.0);
          search.EndDate = dateTime3.Ticks;
        }
        else
          search.EndDate = 0L;
        if (name.DelArchive.model.hitdates != null && name.DelArchive.model.hitdates.Count > 0)
        {
          if (search.StartDate != 0L && dateTime1 > DateTime.Now.AddMinutes((double) -DateTime.Now.Minute))
          {
            ManagmentServer.Logger.Info((object) string.Concat(new object[4]
            {
              (object) "ErrorDelArchive - startDate > Now; StartDate = ",
              (object) BcFace.ConvertDate(name.DelArchive.model.hitdates[0].startDate),
              (object) " Now = ",
              (object) DateTime.Now.Date
            }));
            obj.SendDelArchiveResponse(new DelArchiveResponse()
            {
              messageId = name.DelArchive.messageId,
              status = 22
            });
            return;
          }
          if (search.EndDate != 0L && search.StartDate != 0L && dateTime1 > dateTime3)
          {
            ManagmentServer.Logger.Info((object) string.Concat(new object[4]
            {
              (object) "ErrorDelArchive - startDate > endDate; StartDate = ",
              (object) dateTime1,
              (object) " EndDate = ",
              (object) dateTime3
            }));
            obj.SendDelArchiveResponse(new DelArchiveResponse()
            {
              messageId = name.DelArchive.messageId,
              status = 22
            });
            return;
          }
          if (search.EndDate != 0L && search.StartDate != 0L && dateTime1 == dateTime3)
          {
            ManagmentServer.Logger.Info((object) string.Concat(new object[4]
            {
              (object) "ErrorDelArchive - EndDate == StartDate; StartDate = ",
              (object) dateTime1,
              (object) " EndDate = ",
              (object) dateTime3
            }));
            obj.SendDelArchiveResponse(new DelArchiveResponse()
            {
              messageId = name.DelArchive.messageId,
              status = 22
            });
            return;
          }
          if (name.DelArchive.model.hitdates[0].excludeDates != null && name.DelArchive.model.hitdates[0].excludeDates.Count > 0)
          {
            foreach (exclude exclude in name.DelArchive.model.hitdates[0].excludeDates)
            {
              if (exclude.excludeStart != "" && exclude.excludeEnd != "")
              {
                if (BcFace.ConvertDate(name.DelArchive.model.hitdates[0].startDate) > BcFace.ConvertDate(exclude.excludeStart) && BcFace.ConvertDate(name.DelArchive.model.hitdates[0].endDate) < BcFace.ConvertDate(exclude.excludeEnd))
                {
                  obj.SendDelArchiveResponse(new DelArchiveResponse()
                  {
                    messageId = name.DelArchive.messageId,
                    status = 22
                  });
                  return;
                }
                List<ManagmentServer.ExcludeDate> excludeDates = search.ExcludeDates;
                ManagmentServer.ExcludeDate excludeDate1 = new ManagmentServer.ExcludeDate();
                ManagmentServer.ExcludeDate excludeDate2 = excludeDate1;
                DateTime dateTime2 = BcFace.ConvertDate(exclude.excludeEnd);
                long ticks1 = dateTime2.Ticks;
                excludeDate2.EndDate = ticks1;
                ManagmentServer.ExcludeDate excludeDate3 = excludeDate1;
                dateTime2 = BcFace.ConvertDate(exclude.excludeStart);
                long ticks2 = dateTime2.Ticks;
                excludeDate3.StartDate = ticks2;
                ManagmentServer.ExcludeDate excludeDate4 = excludeDate1;
                excludeDates.Add(excludeDate4);
              }
            }
          }
        }
        if ((name.DelArchive.model.template == null || name.DelArchive.model.template == "") && search.ModelId == Guid.Empty)
        {
          obj.SendDelArchiveResponse(new DelArchiveResponse()
          {
            messageId = name.DelArchive.messageId,
            status = 126
          });
        }
        else
        {
          if (name.DelArchive.model.template != null && name.DelArchive.model.template != "")
          {
            try
            {
              search.Template = Convert.FromBase64String(name.DelArchive.model.template);
            }
            catch
            {
              obj.SendDelArchiveResponse(new DelArchiveResponse()
              {
                messageId = name.DelArchive.messageId,
                status = 124
              });
              return;
            }
            if (search.Template.Length != ManagmentServer.KeyLength && search.ModelId == Guid.Empty)
            {
              obj.SendDelArchiveResponse(new DelArchiveResponse()
              {
                messageId = name.DelArchive.messageId,
                status = 124
              });
              return;
            }
            new ManagmentServer.SearchProcessFunc(ManagmentServer.SearchProcess).BeginInvoke(search, (AsyncCallback) null, (object) null);
            ManagmentServer.CompleteSearchs.Add(search);
          }
          else
          {
            List<BcKey> list = BcKey.LoadKyesByFaceId(search.ModelId);
            if (list != null && list.Count > 0)
            {
              search.Template = list[0].ImageKey;
              new ManagmentServer.SearchProcessFunc(ManagmentServer.SearchProcess).BeginInvoke(search, (AsyncCallback) null, (object) null);
              ManagmentServer.CompleteSearchs.Add(search);
            }
            else
            {
              obj.SendDelArchiveResponse(new DelArchiveResponse()
              {
                messageId = name.DelArchive.messageId,
                status = 2
              });
              return;
            }
          }
          obj.SendDelArchiveResponse(new DelArchiveResponse()
          {
            messageId = name.DelArchive.messageId,
            status = 0
          });
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "DelArchive Error", ex);
        obj.SendError(new Error()
        {
          messageId = name.DelArchive.messageId,
          errorcode = 11,
          errorstring = DelArchiveErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewGetStats(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      CNAPIclr.Messages.Node node = new CNAPIclr.Messages.Node();
      ManagmentServer.LoadStats(node);
      StatsResponse stats = new StatsResponse()
      {
        messageId = name.GetStats.messageId,
        status = 0,
        statusDescr = "Успех"
      };
      StatsInform StatsInform = new StatsInform()
      {
        messageId = name.GetStats.messageId,
        node = new CNAPIclr.Messages.Node(),
        archive = new archive(),
        pasvlist = 0
      };
      StatsInform.node = node;
      try
      {
        using (SqlCommand sqlCommand = new SqlCommand())
        {
          using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionString))
          {
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = "Select State, Count(ID) as ModelCount from Faces group by State ";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
              if (Convert.ToInt32(sqlDataReader[0]) == 0)
                StatsInform.hotlist = Convert.ToInt32(sqlDataReader[1]);
              else
                StatsInform.pasvlist = Convert.ToInt32(sqlDataReader[1]);
            }
            sqlDataReader.Close();
            sqlDataReader.Dispose();
          }
        }
        using (SqlCommand sqlCommand1 = new SqlCommand())
        {
          using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionString))
          {
            sqlCommand1.Connection = sqlConnection;
            sqlConnection.Open();
            sqlCommand1.CommandText = "Select ID from Faces";
            SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
            StatsInform.archive = new archive();
            StatsInform.archive.model = new List<model>();
            while (sqlDataReader1.Read())
            {
              using (SqlCommand sqlCommand2 = new SqlCommand("Select  Date, DeviceID from CSLogInfo.dbo.Log\r\n                            Where FaceID = @ID order by Date", new SqlConnection(CommonSettings.ConnectionString)))
              {
                sqlCommand2.Parameters.Add(new SqlParameter("@ID", (object) (Guid) sqlDataReader1["ID"]));
                sqlCommand2.Connection.Open();
                SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader();
                model model = new model();
                model.uuid = sqlDataReader1["ID"].ToString();
                bool flag = false;
                model.hitdates = new List<hitdates>();
                while (sqlDataReader2.Read())
                {
                  flag = true;
                  model.hitdates.Add(new hitdates()
                  {
                    cam = sqlDataReader2["DeviceID"].ToString(),
                    hitdate = BcFace.GetDate((DateTime) sqlDataReader2["Date"])
                  });
                }
                sqlDataReader2.Close();
                if (flag)
                {
                  model.hitcount = model.hitdates.Count.ToString();
                  StatsInform.archive.model.Add(model);
                }
                sqlCommand2.Connection.Close();
              }
            }
          }
        }
        obj.SendStatsResponse(stats);
        obj.SendStatsInform(StatsInform);
        ManagmentServer.Logger.Info((object) "SendStatsInform");
      }
      catch (Exception ex)
      {
        Error error = new Error()
        {
          messageId = name.GetStats.messageId,
          errorcode = 11,
          errorstring = ex.Message,
          msgTyp = GetStatsErrorMessages.EAGAIN
        };
        obj.SendError(error);
      }
    }

    private static void CNAPIObj_NewSelfCheck(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.LastTestDate = DateTime.Now;
      SelfCheckResponse selfCheck = new SelfCheckResponse();
      selfCheck.messageId = new Guid(name.SelfCheck.messageId);
      ManagmentServer.CheckResult.CheckCode = selfCheck.checkCode = 0;
      selfCheck.statusString = "Система прошла тестирование всех функций, и работает стабильно";
      obj.SendSelfCheckResponse(selfCheck);
    }

    private static void CNAPIObj_NewGetStatus(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.CheckResult.LastDate = BcFace.GetDate(ManagmentServer.LastTestDate);
      obj.SendStatusResponse(new StatusResponse()
      {
        messageId = name.GetStatus.messageId,
        StatusCode = 0,
        StatusString = "Система работает исправно",
        SelfCheck = ManagmentServer.CheckResult
      });
    }

    public static void SystemState(CNAPIObj obj)
    {
      try
      {
        ComputerInfo computerInfo = new ComputerInfo();
        using (SqlCommand sqlCommand = new SqlCommand("Select Count(Faces.ID) from Faces inner join FaceData on FaceData.FaceID = Faces.ID Where FaceData.state = 0\r\nunion all\r\nSelect Count(ID) from Faces inner join FaceData on FaceData.FaceID = Faces.ID Where FaceData.state = 1\r\nunion all\r\nSelect Count(ID) from Searchs\r\n"))
        {
          using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionString))
          {
            sqlConnection.Open();
            sqlCommand.Connection = sqlConnection;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
              OnSystemState data = new OnSystemState();
              data.messageId = Guid.NewGuid();
              data.inputQueue = 0;
              data.stateCode = 0;
              data.statusCode = 0;
              data.listQueueStat = new List<queue>();
              data.searchQueueStat = new List<queue>();
              int num = 0;
              while (sqlDataReader.Read())
              {
                data.inputQueue += Convert.ToInt32(sqlDataReader[0]);
                switch (num)
                {
                  case 0:
                    data.listQueueStat.Add(new queue()
                    {
                      type = queue.active,
                      inputQueueDepth = (int) (computerInfo.TotalPhysicalMemory / (ulong) ManagmentServer.KeyLength),
                      inputQueueObjs = Convert.ToInt32(sqlDataReader[0])
                    });
                    break;
                  case 1:
                    data.listQueueStat.Add(new queue()
                    {
                      type = queue.passive,
                      inputQueueDepth = (int) (computerInfo.TotalPhysicalMemory / (ulong) ManagmentServer.KeyLength),
                      inputQueueObjs = Convert.ToInt32(sqlDataReader[0])
                    });
                    break;
                  case 2:
                    data.searchQueueStat.Add(new queue()
                    {
                      inputQueueDepth = (int) (computerInfo.TotalPhysicalMemory / (ulong) ManagmentServer.KeyLength),
                      inputQueueObjs = Convert.ToInt32(sqlDataReader[0])
                    });
                    break;
                }
                ++num;
              }
              obj.SendOnSystemState(data);
            }
          }
        }
      }
      catch
      {
      }
    }

    private static void CNAPIObj_NewGetSystemState(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      obj.SendGetSystemStateResponse(new GetSystemStateResponse()
      {
        messageId = name.GetSystemState.messageId,
        code = 0
      });
      new ManagmentServer.OnSystemStatefunc(ManagmentServer.SystemState).BeginInvoke(obj, (AsyncCallback) null, (object) null);
    }

    private static void CNAPIObj_NewGetVersion(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      obj.SendVersionResponse(new VersionResponse()
      {
        messageId = name.GetVersion.messageId,
        Manuf = CNAPISettings.VendorName,
        objectType = new objectType()
        {
          Uuid = CNAPISettings.HostId
        },
        systemType = new systemType()
        {
          Uuid = CNAPISettings.KarsTypeId
        },
        Version = new CNAPIclr.Messages.Version()
        {
          Major = CNAPISettings.Major,
          Minor = CNAPISettings.Minor
        }
      });
    }

    public static void GetSearchAll(Guid messageId)
    {
      try
      {
        ManagmentServer.GetSearchsBusy = true;
        foreach (ManagmentServer.SearchData searchData in ManagmentServer.CompleteSearchs)
        {
          if (searchData.State != ManagmentServer.SearchState.Stop && searchData.State != ManagmentServer.SearchState.End)
            ManagmentServer.CnapiObj.SendOnSearch(new onSearch()
            {
              searchID = searchData.Id,
              state = (int) searchData.State,
              status = 0,
              messageId = Guid.NewGuid()
            });
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.CnapiObj.SendError(new Error()
        {
          messageId = messageId,
          errorcode = 11,
          errorstring = GetSearchErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
      ManagmentServer.GetSearchsBusy = false;
    }

    private static void CNAPIObj_NewGetSearches(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      GetSearchesResponse data1 = new GetSearchesResponse();
      data1.status = 0;
      data1.messageId = name.GetSearches.messageId;
      if (ManagmentServer.GetSearchsBusy)
        data1.status = 115;
      else
        new ManagmentServer.OnSearchAllFunc(ManagmentServer.GetSearchAll).BeginInvoke(name.GetSearches.messageId, (AsyncCallback) null, (object) null);
      obj.SendGetSearchesResponse(data1);
    }

    private static void CNAPIObj_NewStopSearch(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        StopSearchResponse data1 = new StopSearchResponse();
        data1.status = 0;
        data1.messageId = name.StopSearch.messageId;
        if (name.StopSearch.searchID == "")
        {
          data1.searchID = name.StopSearch.searchID;
          data1.status = 126;
          obj.SendStopSearchResponse(data1);
        }
        else
        {
          ManagmentServer.SearchData searchData = Enumerable.FirstOrDefault<ManagmentServer.SearchData>(Enumerable.Where<ManagmentServer.SearchData>((IEnumerable<ManagmentServer.SearchData>) ManagmentServer.CompleteSearchs, (Func<ManagmentServer.SearchData, bool>) (s => s.Id == new Guid(name.StopSearch.searchID))));
          if (searchData == null || Guid.Empty == new Guid(name.StopSearch.searchID) || searchData.State == ManagmentServer.SearchState.End)
          {
            data1.searchID = name.StopSearch.searchID;
            data1.status = 2;
            obj.SendStopSearchResponse(data1);
          }
          else if (name.StopSearch.reasonCode > 1 || name.StopSearch.reasonCode < 0)
          {
            data1.status = 22;
            obj.SendStopSearchResponse(data1);
          }
          else
          {
            data1.searchID = searchData.Id.ToString();
            searchData.SentSearchHits.Clear();
            obj.SendStopSearchResponse(data1);
            searchData.State = ManagmentServer.SearchState.Stop;
          }
        }
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.StopSearch.messageId,
          errorcode = 11,
          errorstring = StopSearchErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewGetSearch(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        if (name.GetSearch.searchID == "")
        {
          obj.SendGetSearchResponse(new GetSearchResponse()
          {
            status = 2,
            messageId = name.GetSearch.messageId
          });
        }
        else
        {
          Guid result;
          if (!Guid.TryParse(name.GetSearch.searchID, out result))
          {
            obj.SendGetSearchResponse(new GetSearchResponse()
            {
              status = 2,
              messageId = name.GetSearch.messageId
            });
          }
          else
          {
            ManagmentServer.SearchData searchData = Enumerable.FirstOrDefault<ManagmentServer.SearchData>(Enumerable.Where<ManagmentServer.SearchData>((IEnumerable<ManagmentServer.SearchData>) ManagmentServer.CompleteSearchs, (Func<ManagmentServer.SearchData, bool>) (s => s.Id == new Guid(name.GetSearch.searchID))));
            if (searchData == null || searchData.State == ManagmentServer.SearchState.End)
            {
              obj.SendGetSearchResponse(new GetSearchResponse()
              {
                status = 2,
                searchID = new Guid(name.GetSearch.searchID),
                messageId = name.GetSearch.messageId
              });
            }
            else
            {
              GetSearchResponse data1 = new GetSearchResponse();
              data1.searchID = searchData.Id;
              data1.searchStat = new searchStat();
              data1.status = (int) searchData.State;
              data1.messageId = name.GetSearch.messageId;
              if (searchData.State == ManagmentServer.SearchState.Progress || searchData.State == ManagmentServer.SearchState.Error)
              {
                data1.searchStat.currentHit = 0;
                data1.searchStat.progress = searchData.Progress;
              }
              else if (searchData.State == ManagmentServer.SearchState.Ending)
              {
                data1.searchStat.currentHit = 0;
                data1.searchStat.progress = searchData.Progress;
              }
              else if (searchData.State == ManagmentServer.SearchState.Stop)
              {
                data1.searchStat.currentHit = 0;
                data1.searchStat.progress = searchData.Progress;
              }
              else if (searchData.State == ManagmentServer.SearchState.End)
              {
                try
                {
                  data1.searchStat.currentHit = 0;
                  data1.searchStat.currentHit = searchData.ResultCount;
                  data1.searchStat.progress = searchData.Progress;
                }
                catch (Exception ex)
                {
                  obj.SendError(new Error()
                  {
                    messageId = name.GetSearch.messageId,
                    errorcode = 11,
                    errorstring = ex.Message,
                    msgTyp = ex.Message
                  });
                }
              }
              obj.SendGetSearchResponse(data1);
            }
          }
        }
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.GetSearch.messageId,
          errorcode = 11,
          errorstring = GetSearchErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewStartSearch(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.Logger.Info((object) ("StartSearch- " + message));
      try
      {
        ManagmentServer.SearchData search = new ManagmentServer.SearchData()
        {
          Id = Guid.NewGuid(),
          ModelId = new Guid(name.StartSearch.model.uuid),
          Score = name.StartSearch.model.probMin
        };
        DateTime dateTime1 = DateTime.MinValue;
        if (name.StartSearch.model.hitdates[0].startDate != "")
        {
          DateTime dateTime2 = BcFace.ConvertDate(name.StartSearch.model.hitdates[0].startDate);
          dateTime1 = dateTime2.AddMinutes((double) -dateTime2.Minute);
          search.StartDate = dateTime1.Ticks;
        }
        else
          search.StartDate = 0L;
        DateTime dateTime3 = DateTime.MinValue;
        search.EndDate = 0L;
        if (name.StartSearch.model.hitdates[0].endDate != "")
        {
          DateTime dateTime2 = BcFace.ConvertDate(name.StartSearch.model.hitdates[0].endDate);
          dateTime3 = dateTime2.AddMinutes((double) -dateTime2.Minute);
          search.EndDate = dateTime3.Ticks;
        }
        if (name.StartSearch.model.hitdates != null && name.StartSearch.model.hitdates.Count > 0)
        {
          if (search.StartDate != 0L && dateTime1 > DateTime.Now.AddMinutes((double) -DateTime.Now.Minute))
          {
            ManagmentServer.Logger.Error((object) string.Concat(new object[4]
            {
              (object) "ErrorStartSearch - startDate > Now; StartDate = ",
              (object) BcFace.ConvertDate(name.StartSearch.model.hitdates[0].startDate),
              (object) " Now = ",
              (object) DateTime.Now.Date
            }));
            obj.SendStartSearchResponse(new StartSearchResponse()
            {
              messageId = name.StartSearch.messageId,
              status = 22
            });
            return;
          }
          if (search.EndDate != 0L && search.StartDate != 0L && dateTime1 > dateTime3)
          {
            ManagmentServer.Logger.Error((object) string.Concat(new object[4]
            {
              (object) "ErrorStartSearch - startDate > endDate; StartDate = ",
              (object) dateTime1,
              (object) " EndDate = ",
              (object) dateTime3
            }));
            obj.SendStartSearchResponse(new StartSearchResponse()
            {
              messageId = name.StartSearch.messageId,
              status = 22
            });
            return;
          }
          if (search.EndDate != 0L && search.StartDate != 0L && dateTime1 == dateTime3)
          {
            ManagmentServer.Logger.Error((object) string.Concat(new object[4]
            {
              (object) "ErrorStartSearch - EndDate == StartDate; StartDate = ",
              (object) dateTime1,
              (object) " EndDate = ",
              (object) dateTime3
            }));
            obj.SendStartSearchResponse(new StartSearchResponse()
            {
              messageId = name.StartSearch.messageId,
              status = 22
            });
            return;
          }
          if (name.StartSearch.model.hitdates[0].excludeDates != null && name.StartSearch.model.hitdates[0].excludeDates.Count > 0)
          {
            foreach (exclude exclude in name.StartSearch.model.hitdates[0].excludeDates)
            {
              if (exclude.excludeStart != "" && exclude.excludeEnd != "")
              {
                if (BcFace.ConvertDate(name.StartSearch.model.hitdates[0].startDate) > BcFace.ConvertDate(exclude.excludeStart) && BcFace.ConvertDate(name.StartSearch.model.hitdates[0].endDate) < BcFace.ConvertDate(exclude.excludeEnd))
                {
                  obj.SendStartSearchResponse(new StartSearchResponse()
                  {
                    messageId = name.StartSearch.messageId,
                    status = 22
                  });
                  return;
                }
                List<ManagmentServer.ExcludeDate> excludeDates = search.ExcludeDates;
                ManagmentServer.ExcludeDate excludeDate1 = new ManagmentServer.ExcludeDate();
                ManagmentServer.ExcludeDate excludeDate2 = excludeDate1;
                DateTime dateTime2 = BcFace.ConvertDate(exclude.excludeEnd);
                long ticks1 = dateTime2.Ticks;
                excludeDate2.EndDate = ticks1;
                ManagmentServer.ExcludeDate excludeDate3 = excludeDate1;
                dateTime2 = BcFace.ConvertDate(exclude.excludeStart);
                long ticks2 = dateTime2.Ticks;
                excludeDate3.StartDate = ticks2;
                ManagmentServer.ExcludeDate excludeDate4 = excludeDate1;
                excludeDates.Add(excludeDate4);
              }
            }
          }
        }
        if ((name.StartSearch.model.template == null || name.StartSearch.model.template == "") && search.ModelId == Guid.Empty)
        {
          obj.SendStartSearchResponse(new StartSearchResponse()
          {
            messageId = name.StartSearch.messageId,
            status = 126
          });
        }
        else
        {
          if (name.StartSearch.model.template != null && name.StartSearch.model.template != "")
          {
            try
            {
              search.Template = Convert.FromBase64String(name.StartSearch.model.template);
            }
            catch
            {
              obj.SendStartSearchResponse(new StartSearchResponse()
              {
                messageId = name.StartSearch.messageId,
                status = 124
              });
              return;
            }
            if (search.Template.Length != ManagmentServer.KeyLength && search.ModelId == Guid.Empty)
            {
              obj.SendStartSearchResponse(new StartSearchResponse()
              {
                messageId = name.StartSearch.messageId,
                status = 124
              });
              return;
            }
          }
          else
          {
            List<BcKey> list = BcKey.LoadKyesByFaceId(search.ModelId);
            if (list != null && list.Count > 0)
            {
              search.Template = list[0].ImageKey;
            }
            else
            {
              obj.SendStartSearchResponse(new StartSearchResponse()
              {
                messageId = name.StartSearch.messageId,
                status = 2
              });
              return;
            }
          }
          obj.SendStartSearchResponse(new StartSearchResponse()
          {
            messageId = name.StartSearch.messageId,
            searchID = search.Id,
            status = 0
          });
          ManagmentServer.CompleteSearchs.Add(search);
          search.StateChanged += new ManagmentServer.SearchData.SearchStateChangeEventHandler(ManagmentServer.search_StateChanged);
          search.State = ManagmentServer.SearchState.Progress;
          new ManagmentServer.SearchProcessFunc(ManagmentServer.SearchProcess).BeginInvoke(search, (AsyncCallback) null, (object) null);
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) "StartSearch Error", ex);
        obj.SendError(new Error()
        {
          messageId = name.StartSearch.messageId,
          errorcode = 11,
          errorstring = StartSearchErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void search_StateChanged(ManagmentServer.SearchState oldState, ManagmentServer.SearchState newState, ManagmentServer.SearchData searchData)
    {
      ManagmentServer.CnapiObj.SendOnSearch(new onSearch()
      {
        messageId = Guid.NewGuid(),
        searchID = searchData.Id,
        state = (int) searchData.State,
        status = 0
      });
    }

    private static void CNAPIObj_NewAddModelPassive(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        long num1 = 0;
        long num2 = 0;
        if (name.AddModelPassive.model.modelData == null || name.AddModelPassive.model.modelData == "")
        {
          obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
          {
            model = new Guid(name.AddModelPassive.model.uuid),
            messageId = name.AddModelPassive.messageId,
            status = 123
          });
        }
        else
        {
          byte[] numArray1;
          try
          {
            numArray1 = Convert.FromBase64String(name.AddModelPassive.model.modelData);
          }
          catch
          {
            obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
            {
              model = new Guid(name.AddModelPassive.model.uuid),
              messageId = name.AddModelPassive.messageId,
              status = 124
            });
            return;
          }
          if (numArray1.Length != ManagmentServer.KeyLength)
          {
            obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
            {
              model = new Guid(name.AddModelPassive.model.uuid),
              messageId = name.AddModelPassive.messageId,
              status = 124
            });
          }
          else
          {
            if (name.AddModelPassive.startDate != "")
              num1 = BcFace.ConvertDate(name.AddModelPassive.startDate).Ticks;
            if (name.AddModelPassive.endDate != "")
              num2 = BcFace.ConvertDate(name.AddModelPassive.endDate).Ticks;
            if (num2 < DateTime.Now.Date.Ticks && num2 != 0L || num1 < DateTime.Now.Date.Ticks && num1 != 0L || num1 > num2 && num2 != 0L)
              obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
              {
                model = new Guid(name.AddModelPassive.model.uuid),
                messageId = name.AddModelPassive.messageId,
                status = 22
              });
            else if (name.AddModelPassive.model.uuid == Guid.Empty.ToString())
              obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
              {
                model = new Guid(name.AddModelPassive.model.uuid),
                messageId = name.AddModelPassive.messageId,
                status = 126
              });
            else if (BcFace.LoadById(new Guid(name.AddModelPassive.model.uuid)).Id != Guid.Empty)
            {
              obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
              {
                model = new Guid(name.AddModelPassive.model.uuid),
                messageId = name.AddModelPassive.messageId,
                status = 17
              });
            }
            else
            {
              BcFace bcFace = new BcFace();
              bcFace.FaceData.HostId = obj.ClientHostID;
              bcFace.NewFlag = true;
              bcFace.Id = new Guid(name.AddModelPassive.model.uuid);
              bcFace.FaceData.State = 1;
              bcFace.FaceData.Results = name.AddModelPassive.results;
              if (name.AddModelPassive.endDate != null)
                bcFace.FaceData.EndDate = BcFace.ConvertDate(name.AddModelPassive.endDate);
              if (name.AddModelPassive.startDate != null)
                bcFace.FaceData.StartDate = BcFace.ConvertDate(name.AddModelPassive.startDate);
              bcFace.Save();
              ManagmentServer.AllModels.Add(bcFace);
              bcFace.StateChanged += new BcFace.ModelStateChangedHandler(ManagmentServer.f_StateChanged);
              if (numArray1.Length >= ManagmentServer.KeyLength)
              {
                byte[] numArray2 = new byte[ManagmentServer.KeyLength];
                for (int index = 0; index < numArray1.Length / ManagmentServer.KeyLength; ++index)
                {
                  BcKey bcKey = new BcKey();
                  bcKey.FaceId = bcFace.Id;
                  Buffer.BlockCopy((Array) numArray1, index * ManagmentServer.KeyLength, (Array) numArray2, 0, ManagmentServer.KeyLength);
                  bcKey.ImageKey = numArray2;
                  bcKey.ImageId = Guid.Empty;
                  bcKey.Save();
                }
              }
              obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
              {
                status = 0,
                model = bcFace.Id,
                messageId = name.AddModelPassive.messageId,
                maxHitCount = name.AddModelPassive.maxHitCount
              });
              bcFace.ModelState = 1;
            }
          }
        }
      }
      catch (SqlException ex)
      {
        obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
        {
          model = new Guid(name.AddModelPassive.model.uuid),
          messageId = name.AddModelPassive.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelPassive error - ", (Exception) ex);
      }
      catch (IndexOutOfRangeException ex)
      {
        obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
        {
          model = new Guid(name.AddModelPassive.model.uuid),
          messageId = name.AddModelPassive.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelPassive error - ", (Exception) ex);
      }
      catch (Exception ex)
      {
        obj.SendAddModelPassiveResponse(new AddModelPassiveResponse()
        {
          model = new Guid(name.AddModelPassive.model.uuid),
          messageId = name.AddModelPassive.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelPassive error - ", ex);
      }
    }

    private static void CNAPIObj_NewMoveModelActive(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      if (name.MoveModelActive.model != "" && name.MoveModelActive.model != null && new Guid(name.MoveModelActive.model) != Guid.Empty)
      {
        BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => face.Id == new Guid(name.MoveModelActive.model))));
        if (bcFace != null)
        {
          if (bcFace.FaceData.State != 0)
          {
            bcFace.FaceData.Reactioncode = name.MoveModelActive.reactionCode;
            bcFace.FaceData.State = 0;
            bcFace.Save();
            obj.SendMoveModelActiveResponse(new MoveModelActiveResponse()
            {
              messageId = name.MoveModelActive.messageId,
              status = 0,
              model = bcFace.Id
            });
            ManagmentServer.SendOnModelState(obj, name.MoveModelActive.messageId, bcFace.Id);
          }
          else
            obj.SendMoveModelActiveResponse(new MoveModelActiveResponse()
            {
              messageId = name.MoveModelActive.messageId,
              status = 17
            });
        }
        else
          obj.SendMoveModelActiveResponse(new MoveModelActiveResponse()
          {
            messageId = name.MoveModelActive.messageId,
            status = 2
          });
      }
      else
        obj.SendMoveModelActiveResponse(new MoveModelActiveResponse()
        {
          messageId = name.MoveModelActive.messageId,
          status = 126
        });
    }

    private static void CNAPIObj_NewMoveModelPassive(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        if (name.MoveModelPassive.model != "" && name.MoveModelPassive.model != null && new Guid(name.MoveModelPassive.model) != Guid.Empty)
        {
          BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => face.Id == new Guid(name.MoveModelPassive.model))));
          if (bcFace != null)
          {
            if (bcFace.FaceData.State != 1)
            {
              bcFace.FaceData.Reactioncode = name.MoveModelPassive.reactionCode;
              bcFace.FaceData.State = 1;
              bcFace.FaceData.Results = name.MoveModelPassive.results;
              bcFace.Save();
              obj.SendMoveModelPassiveResponse(new MoveModelPassiveResponse()
              {
                messageId = name.MoveModelPassive.messageId,
                maxHitCount = name.MoveModelPassive.maxHitCount,
                model = bcFace.Id,
                results = bcFace.FaceData.Results,
                status = 0
              });
              ManagmentServer.SendOnModelState(obj, name.MoveModelPassive.messageId, bcFace.Id);
            }
            else
              obj.SendMoveModelPassiveResponse(new MoveModelPassiveResponse()
              {
                messageId = name.MoveModelPassive.messageId,
                status = 17
              });
          }
          else
            obj.SendMoveModelPassiveResponse(new MoveModelPassiveResponse()
            {
              messageId = name.MoveModelPassive.messageId,
              status = 2
            });
        }
        else
          obj.SendMoveModelPassiveResponse(new MoveModelPassiveResponse()
          {
            messageId = name.MoveModelPassive.messageId,
            status = 126
          });
      }
      catch (Exception ex)
      {
        ManagmentServer.Logger.Error((object) ex);
        obj.SendMoveModelPassiveResponse(new MoveModelPassiveResponse()
        {
          messageId = name.MoveModelPassive.messageId,
          status = 11
        });
      }
    }

    private static void CNAPIObj_NewAddModelToHotlist(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        long num1 = 0;
        long num2 = 0;
        if (name.AddModelToHotlist.model.modelData == null || name.AddModelToHotlist.model.modelData == "")
        {
          obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
          {
            model = new Guid(name.AddModelToHotlist.model.uuid),
            messageId = name.AddModelToHotlist.messageId,
            status = 123
          });
        }
        else
        {
          byte[] numArray1;
          try
          {
            numArray1 = Convert.FromBase64String(name.AddModelToHotlist.model.modelData);
          }
          catch
          {
            obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
            {
              model = new Guid(name.AddModelToHotlist.model.uuid),
              messageId = name.AddModelToHotlist.messageId,
              status = 124
            });
            return;
          }
          if (numArray1.Length != ManagmentServer.KeyLength)
          {
            obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
            {
              model = new Guid(name.AddModelToHotlist.model.uuid),
              messageId = name.AddModelToHotlist.messageId,
              status = 124
            });
          }
          else
          {
            if (name.AddModelToHotlist.startDate != "")
              num1 = BcFace.ConvertDate(name.AddModelToHotlist.startDate).Ticks;
            if (name.AddModelToHotlist.endDate != "")
              num2 = BcFace.ConvertDate(name.AddModelToHotlist.endDate).Ticks;
            if (num2 < DateTime.Now.Date.Ticks && num2 != 0L || num1 < DateTime.Now.Date.Ticks && num1 != 0L || num1 > num2 && num2 != 0L)
              obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
              {
                model = new Guid(name.AddModelToHotlist.model.uuid),
                messageId = name.AddModelToHotlist.messageId,
                status = 22
              });
            else if (new Guid(name.AddModelToHotlist.model.uuid) == Guid.Empty)
              obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
              {
                model = new Guid(name.AddModelToHotlist.model.uuid),
                messageId = name.AddModelToHotlist.messageId,
                status = 126
              });
            else if (BcFace.LoadById(new Guid(name.AddModelToHotlist.model.uuid)).Id != Guid.Empty)
            {
              obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
              {
                model = new Guid(name.AddModelToHotlist.model.uuid),
                messageId = name.AddModelToHotlist.messageId,
                status = 17
              });
            }
            else
            {
              BcFace bcFace = new BcFace();
              bcFace.FaceData.HostId = obj.ClientHostID;
              bcFace.NewFlag = true;
              bcFace.Id = new Guid(name.AddModelToHotlist.model.uuid);
              bcFace.FaceData.Reactioncode = name.AddModelToHotlist.reactionCode;
              bcFace.FaceData.State = 0;
              bcFace.FaceData.Results = name.AddModelToHotlist.results;
              if (name.AddModelToHotlist.endDate != null)
                bcFace.FaceData.EndDate = BcFace.ConvertDate(name.AddModelToHotlist.endDate);
              if (name.AddModelToHotlist.startDate != null)
                bcFace.FaceData.StartDate = BcFace.ConvertDate(name.AddModelToHotlist.startDate);
              bcFace.Save();
              bcFace.StateChanged += new BcFace.ModelStateChangedHandler(ManagmentServer.f_StateChanged);
              ManagmentServer.AllModels.Add(bcFace);
              if (numArray1.Length >= ManagmentServer.KeyLength)
              {
                byte[] numArray2 = new byte[ManagmentServer.KeyLength];
                for (int index = 0; index < numArray1.Length / ManagmentServer.KeyLength; ++index)
                {
                  BcKey bcKey = new BcKey();
                  bcKey.FaceId = bcFace.Id;
                  Buffer.BlockCopy((Array) numArray1, index * ManagmentServer.KeyLength, (Array) numArray2, 0, ManagmentServer.KeyLength);
                  bcKey.ImageKey = numArray2;
                  bcKey.ImageId = Guid.Empty;
                  bcKey.Save();
                }
              }
              AddModelToHotlistResponse data1 = new AddModelToHotlistResponse();
              data1.status = 0;
              data1.messageId = name.AddModelToHotlist.messageId;
              data1.model = bcFace.Id;
              bcFace.ModelState = 0;
              obj.SendAddModelToHotlistResponse(data1);
            }
          }
        }
      }
      catch (SqlException ex)
      {
        obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
        {
          model = new Guid(name.AddModelToHotlist.model.uuid),
          messageId = name.AddModelToHotlist.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelToHotlist error - ", (Exception) ex);
      }
      catch (IndexOutOfRangeException ex)
      {
        obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
        {
          model = new Guid(name.AddModelToHotlist.model.uuid),
          messageId = name.AddModelToHotlist.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelToHotlist error", (Exception) ex);
      }
      catch (Exception ex)
      {
        obj.SendAddModelToHotlistResponse(new AddModelToHotlistResponse()
        {
          model = new Guid(name.AddModelToHotlist.model.uuid),
          messageId = name.AddModelToHotlist.messageId,
          status = 11
        });
        ManagmentServer.Logger.Error((object) "CNAPIObj_NewAddModelToHotlist error", ex);
      }
    }

    private static void CNAPIObj_NewDeleteModelFromLists(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        if (name.DeleteModelFromLists.model.ToString() != "" && name.DeleteModelFromLists.model != Guid.Empty)
        {
          BcFace f = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => name.DeleteModelFromLists.model == face.Id)));
          if (f != null)
          {
            f.Delete();
            f.DeleteArchive();
            obj.SendDeleteModelFromListsResponse(new DeleteModelFromListsResponse()
            {
              messageId = name.DeleteModelFromLists.messageId,
              status = 0
            });
            OnModelState onModelState = new OnModelState()
            {
              messageId = Guid.NewGuid(),
              modelInfo = new modelInfo()
              {
                model = f.Id,
                status = CNAPIObj.Deleted,
                statusString = "Deleted"
              }
            };
            obj.SendOnModelState(onModelState);
            f.SentState.Add(onModelState.messageId);
            new ManagmentServer.WaitModelStateResponceFunc(ManagmentServer.WaitModelStateResponce).BeginInvoke(f, onModelState, (AsyncCallback) null, (object) null);
            ManagmentServer.AllModels.Remove(f);
          }
          else
            obj.SendDeleteModelFromListsResponse(new DeleteModelFromListsResponse()
            {
              messageId = name.DeleteModelFromLists.messageId,
              status = 2
            });
        }
        else
          obj.SendDeleteModelFromListsResponse(new DeleteModelFromListsResponse()
          {
            messageId = name.DeleteModelFromLists.messageId,
            status = 2
          });
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.DeleteModelFromLists.messageId,
          errorcode = 11,
          errorstring = DeleteModelFromListsErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewGetModels(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      ManagmentServer.SendOnModelState(obj, name.GetModels.messageId, Guid.Empty);
      foreach (BcFace bcFace in ManagmentServer.AllModels)
        ManagmentServer.SendOnModelState(ManagmentServer.CnapiObj, Guid.NewGuid(), bcFace.Id);
    }

    public static void SendOnModelState(CNAPIObj obj, Guid messageId, Guid modelId)
    {
      try
      {
        if (modelId == Guid.Empty)
        {
          using (SqlCommand sqlCommand = new SqlCommand("Select Count(ID) from Faces", new SqlConnection(CommonSettings.ConnectionString)))
          {
            sqlCommand.Connection.Open();
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
              int num = 0;
              if (sqlDataReader.Read())
                num = Convert.ToInt32(sqlDataReader[0]);
              obj.SendGetModelsResponse(new GetModelsResponse()
              {
                messageId = messageId,
                modelNum = num
              });
            }
          }
        }
        else
        {
          SqlCommand sqlCommand1 = new SqlCommand("Select Faces.ID,State,CreateDate,Reactioncode from Faces inner join FaceData on FaceData.FaceID = Faces.ID", new SqlConnection(CommonSettings.ConnectionString));
          if (modelId != Guid.Empty)
          {
            sqlCommand1.CommandText += " WHERE Faces.ID = @ID";
            sqlCommand1.Parameters.Add(new SqlParameter("@ID", (object) modelId));
          }
          sqlCommand1.Connection.Open();
          SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
          while (sqlDataReader1.Read())
          {
            OnModelState onModelState = new OnModelState();
            onModelState.modelInfo = new modelInfo();
            onModelState.modelInfo.model = (Guid) sqlDataReader1["ID"];
            onModelState.modelInfo.setDate = BcFace.GetDate((DateTime) sqlDataReader1["CreateDate"]);
            int num = Convert.ToInt32(sqlDataReader1["State"].ToString());
            string statusString;
            onModelState.modelInfo.status = ManagmentServer.GetModelState((Guid) sqlDataReader1["ID"], out statusString);
            onModelState.modelInfo.statusString = statusString;
            onModelState.messageId = Guid.NewGuid();
            if (num != 0)
            {
              onModelState.modelInfo.reactionCode = 0;
              onModelState.modelInfo.passiveState = new passiveState();
              onModelState.modelInfo.passiveState.hitdates = new List<hitdate>();
              using (SqlCommand sqlCommand2 = new SqlCommand("Select FaceID,DeviceID,Date\r\nfrom CSLogInfo.dbo.Log Where FaceID = '" + sqlDataReader1["ID"] + "'", new SqlConnection(CommonSettings.ConnectionString)))
              {
                sqlCommand2.Connection.Open();
                using (SqlDataReader sqlDataReader2 = sqlCommand2.ExecuteReader())
                {
                  while (sqlDataReader2.Read())
                    onModelState.modelInfo.passiveState.hitdates.Add(new hitdate()
                    {
                      cam = (Guid) sqlDataReader2["DeviceID"],
                      date = BcFace.GetDate((DateTime) sqlDataReader2["Date"])
                    });
                  onModelState.modelInfo.passiveState.hitcount = onModelState.modelInfo.passiveState.hitdates.Count;
                }
              }
            }
            BcFace f1 = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (f => f.Id == modelId)));
            if (f1 != null)
            {
              onModelState.modelInfo.reactionCode = f1.FaceData.Reactioncode;
              obj.SendOnModelState(onModelState);
              f1.SentState.Add(onModelState.messageId);
              new ManagmentServer.WaitModelStateResponceFunc(ManagmentServer.WaitModelStateResponce).BeginInvoke(f1, onModelState, (AsyncCallback) null, (object) null);
            }
          }
          sqlCommand1.Connection.Close();
        }
      }
      catch (Exception ex)
      {
        ManagmentServer.CnapiObj.SendError(new Error()
        {
          errorcode = 11,
          errorstring = GetModelsErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void WaitModelStateResponce(BcFace f, OnModelState state)
    {
      bool flag = false;
      while (flag)
      {
        if (ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
        {
          if (f.ModelState != state.modelInfo.status)
            break;
          flag = false;
          foreach (Guid guid in f.SentState.ToArray())
          {
            if (guid == state.messageId)
              flag = true;
          }
          if (flag)
            ManagmentServer.CnapiObj.SendOnModelState(state);
        }
        Thread.Sleep(20000);
      }
    }

    private static void CNAPIObj_NewGetActiveModelCount(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select Faces.ID from Faces inner join FaceData on FaceData.FaceID = Faces.ID Where FaceData.State = 0", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        List<Guid> list = new List<Guid>();
        while (sqlDataReader.Read())
          list.Add((Guid) sqlDataReader[0]);
        obj.SendGetActiveModelCountResponse(new GetActiveModelCountResponse()
        {
          messageId = name.GetActiveModelCount.messageId,
          model = list,
          modelNum = list.Count
        });
        sqlCommand.Connection.Close();
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.GetActiveModelCount.messageId,
          errorcode = 11,
          errorstring = GetActiveModelCountErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewGetModelCount(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select ID from Faces", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        List<Guid> list = new List<Guid>();
        while (sqlDataReader.Read())
          list.Add((Guid) sqlDataReader[0]);
        obj.SendGetModelCountResponse(new GetModelCountResponse()
        {
          messageId = name.GetModelCount.messageId,
          model = list,
          modelNum = list.Count
        });
        sqlCommand.Connection.Close();
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.GetModelCount.messageId,
          errorcode = 11,
          errorstring = GetModelCountErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewClearModelCode(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        if (name.ClearModelCode.model.ToString() != "" && name.ClearModelCode.model != Guid.Empty)
        {
          BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => name.ClearModelCode.model == face.Id)));
          if (bcFace != null)
          {
            bcFace.FaceData.Reactioncode = 0;
            bcFace.Save();
            ClearModelCodeResponse data1 = new ClearModelCodeResponse()
            {
              state = 0,
              model = bcFace.Id,
              reactionCode = bcFace.FaceData.Reactioncode,
              messageId = name.ClearModelCode.messageId
            };
            obj.SendClearModelCodeResponse(data1);
            ManagmentServer.SendOnModelState(obj, Guid.NewGuid(), bcFace.Id);
          }
          else
            obj.SendClearModelCodeResponse(new ClearModelCodeResponse()
            {
              messageId = name.ClearModelCode.messageId,
              state = 2
            });
        }
        else
          obj.SendClearModelCodeResponse(new ClearModelCodeResponse()
          {
            messageId = name.ClearModelCode.messageId,
            state = 126
          });
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.ClearModelCode.messageId,
          errorcode = 11,
          errorstring = ClearModelCodeErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewSetModelCode(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        if (name.SetModelCode.model.ToString() != "" && name.SetModelCode.model != Guid.Empty)
        {
          BcFace bcFace = Enumerable.FirstOrDefault<BcFace>(Enumerable.Where<BcFace>((IEnumerable<BcFace>) ManagmentServer.AllModels, (Func<BcFace, bool>) (face => name.SetModelCode.model == face.Id)));
          if (bcFace != null)
          {
            bcFace.FaceData.Reactioncode = name.SetModelCode.reactionCode;
            bcFace.Save();
            SetModelCodeResponse data1 = new SetModelCodeResponse()
            {
              model = bcFace.Id,
              reactionCode = bcFace.FaceData.Reactioncode,
              messageId = name.SetModelCode.messageId
            };
            data1.state = 0;
            obj.SendSetModelCodeResponse(data1);
            ManagmentServer.SendOnModelState(obj, Guid.NewGuid(), bcFace.Id);
          }
          else
            obj.SendSetModelCodeResponse(new SetModelCodeResponse()
            {
              messageId = name.SetModelCode.messageId,
              state = 2,
              model = name.SetModelCode.model
            });
        }
        else
          obj.SendSetModelCodeResponse(new SetModelCodeResponse()
          {
            messageId = name.MoveModelPassive.messageId,
            state = 126,
            model = name.SetModelCode.model
          });
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.SetModelCode.messageId,
          errorcode = 11,
          errorstring = SetModelCodeErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    private static void CNAPIObj_NewGetModel(ApplicationMessage data, Namespace name, object message, CNAPIObj obj)
    {
      try
      {
        GetModelResponse data1 = new GetModelResponse();
        data1.model = name.GetModel.model;
        data1.messageId = name.GetModel.messageId;
        if (name.GetModel.model == Guid.Empty)
        {
          data1.status = -126;
          obj.SendGetModelResponse(data1);
        }
        else
        {
          BcFace bcFace = BcFace.LoadById(name.GetModel.model);
          string statusString;
          data1.status = !(bcFace.Id != Guid.Empty) ? -2 : ManagmentServer.GetModelState(bcFace.Id, out statusString);
          obj.SendGetModelResponse(data1);
        }
      }
      catch (Exception ex)
      {
        obj.SendError(new Error()
        {
          messageId = name.GetModel.messageId,
          errorcode = 11,
          errorstring = GetModelErrorMessages.EAGAIN,
          msgTyp = ex.Message
        });
      }
    }

    public static void SendHits()
    {
      List<int> list = new List<int>();
      while (!ManagmentServer.StopFlag)
      {
        try
        {
          if (ManagmentServer.CnapiObj.SessionState == SessionState.ActiveSession)
          {
            foreach (BcFace f in ManagmentServer.AllModels)
            {
              if (!f.FaceData.Expired && !f.FaceData.Suspended)
              {
                bool flag = false;
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                  using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionString))
                  {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    sqlCommand.CommandText = "Select log.* from CSLogInfo.dbo.Log as log\r\ninner join Faces on log.FaceID = Faces.ID \r\ninner join FaceData on FaceData.FaceID = Faces.ID\r\nWhere Sent = 0 and FaceData.State = 0  and log.FaceID = '" + (object) f.Id + "'";
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    int num = 0;
                    while (sqlDataReader.Read())
                    {
                      flag = true;
                      onHotListHit onHotListHit = new onHotListHit();
                      onHotListHit.messageId = Guid.NewGuid();
                      onHotListHit.cam = (Guid) sqlDataReader["DeviceID"];
                      onHotListHit.hitcount = 1;
                      onHotListHit.hitdate = BcFace.GetDate((DateTime) sqlDataReader["Date"]);
                      onHotListHit.image = new Image()
                      {
                        format = "jpeg",
                        Base64String = Convert.ToBase64String((byte[]) sqlDataReader["Image"])
                      };
                      onHotListHit.model = (Guid) sqlDataReader["FaceID"];
                      onHotListHit.prob = Convert.ToSingle(sqlDataReader["Score"]);
                      f.SentHits.Add(onHotListHit.messageId);
                      if (num == 0)
                      {
                        string statusString;
                        f.ModelState = ManagmentServer.GetModelState(f.Id, out statusString);
                      }
                      ManagmentServer.CnapiObj.SendonHotListHit(onHotListHit);
                      new ManagmentServer.WaitHitResponceFunc(ManagmentServer.WaitHitResponce).BeginInvoke(onHotListHit, f, (AsyncCallback) null, (object) null);
                      list.Add(Convert.ToInt32(sqlDataReader["ID"]));
                      Thread.Sleep(100);
                      ++num;
                    }
                  }
                }
                if (flag)
                  new ManagmentServer.WairModelSentHitsFunc(ManagmentServer.WairModelSentHits).BeginInvoke(f, (AsyncCallback) null, (object) null);
                foreach (int num in list)
                {
                  using (SqlCommand sqlCommand = new SqlCommand())
                  {
                    using (SqlConnection sqlConnection = new SqlConnection(CommonSettings.ConnectionStringLog))
                    {
                      sqlConnection.Open();
                      sqlCommand.Connection = sqlConnection;
                      sqlCommand.CommandText = "Update CSLogInfo.dbo.Log Set Sent = 1 Where ID = @ID";
                      sqlCommand.Parameters.Add(new SqlParameter("@ID", (object) num));
                      sqlCommand.ExecuteNonQuery();
                    }
                  }
                }
                list.Clear();
              }
            }
          }
        }
        catch (Exception ex)
        {
          ManagmentServer.Logger.Error((object) ex);
        }
        Thread.Sleep(3000);
      }
    }

    public class ClientDetectorCallback : IDetectorServerCallback
    {
      public void CheckClient()
      {
      }
    }

    public class ClientExtractorCallback : IExtractorServerCallback
    {
      public void CheckClient()
      {
      }
    }

    public delegate void NewSearchResults(ManagmentServer.ClientIdentificationCallback callback, Guid searchId, DataTable dt);

    public class ClientIdentificationCallback : IIdentificationServerCallback
    {
      public IdentificationServerClient Client { get; set; }

      public event ManagmentServer.NewSearchResults ReceivedSeacrhResuts;

      public void CheckClient()
      {
      }

      public void SendResults(DataTable dt)
      {
      }

      public void SendStaticResults(Guid searchId, DataTable dt)
      {
        if (this.ReceivedSeacrhResuts != null)
          this.ReceivedSeacrhResuts(this, searchId, dt);
        this.Client.Close();
      }
    }

    public class WorkStations
    {
      public Guid Id = Guid.Empty;
      public IManagerClientCallback Client;
      public bool Connected;
    }

    public class Operator
    {
      public BcWorkStation Station = new BcWorkStation();
      public Guid Id = Guid.Empty;
      public string MacAddress = "";
      public string IpAddress = "";
      public bool Connected;
      public IClientCallback Client;
    }

    public delegate void ClearResultsFunc(List<Guid> ids, IServer objServer);

    public enum ProcessorMode
    {
      Non,
      Other,
      Unknown,
      Running,
      Warning,
      InTest,
      NotApplicable,
      PowerOff,
      OffLine,
      OffDuty,
      Degraded,
      NotInstalled,
      InstallError,
      PowerSave,
      PowerSaveLowPowerMode,
      PowerSaveStandby,
      PowerCycle,
      PowerSaveWarning,
    }

    public enum SearchState
    {
      Progress,
      Complete,
      Ending,
      End,
      Stop,
      Error,
      None,
    }

    public class SearchData
    {
      private ManagmentServer.SearchState _state = ManagmentServer.SearchState.None;

      public List<OnSearchHit> SentSearchHits { get; set; }

      public ManagmentServer.SearchState State
      {
        get
        {
          return this._state;
        }
        set
        {
          if (this._state == value)
            return;
          ManagmentServer.SearchState oldState = this._state;
          this._state = value;
          if (this.StateChanged != null)
            this.StateChanged(oldState, this._state, this);
        }
      }

      public Guid DeleteId { get; set; }

      public int Progress { get; set; }

      public long StartDate { get; set; }

      public long EndDate { get; set; }

      public Guid Id { get; set; }

      public Guid ModelId { get; set; }

      public byte[] Template { get; set; }

      public List<ManagmentServer.ExcludeDate> ExcludeDates { get; set; }

      public float Score { get; set; }

      public int ResultCount { get; set; }

      public event ManagmentServer.SearchData.SearchStateChangeEventHandler StateChanged;

      public SearchData()
      {
        this.SentSearchHits = new List<OnSearchHit>();
        this.ExcludeDates = new List<ManagmentServer.ExcludeDate>();
      }

      public delegate void SearchStateChangeEventHandler(ManagmentServer.SearchState oldState, ManagmentServer.SearchState newState, ManagmentServer.SearchData searchData);
    }

    public class ExcludeDate
    {
      public long StartDate { get; set; }

      public long EndDate { get; set; }
    }

    private delegate void WaitSearchHitResponceFunc(OnSearchHit hit, ManagmentServer.SearchData search);

    private delegate void SearchProcessFunc(ManagmentServer.SearchData search);

    private delegate void WaitSearchResponceFunc(ManagmentServer.SearchData search);

    private delegate void WairModelSentHitsFunc(BcFace f);

    private delegate void WaitHitResponceFunc(onHotListHit hit, BcFace f);

    private delegate void SendCamStateFunc(int type);

    private delegate void SendCamInfoFunc();

    public delegate void OnSystemStatefunc(CNAPIObj obj);

    private delegate void OnSearchAllFunc(Guid messageId);

    public delegate void WaitModelStateResponceFunc(BcFace f, OnModelState state);
  }
}
