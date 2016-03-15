// Decompiled with JetBrains decompiler
// Type: FaceIdentification.IdentificationServer
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using BasicComponents;
using BasicComponents.ExtractorServer;
using CommonContracts;
using CS.DAL;
using CSIdentificationServer;
using CSIdentificationServer.Properties;
using log4net;
using NotificationProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using TS.MessagesCore;

namespace FaceIdentification
{
  [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext = true)]
  public class IdentificationServer : IIdentificationServer
  {
    public static ILog Logger = LogManager.GetLogger(typeof (IdentificationServer));
    public static List<BcNotificationSystem> NotificationSystems = new List<BcNotificationSystem>();
    public static string LoginCom = "";
    public static string GetZoneInfo = "";
    public static string AllowPass = "";
    public static List<BcAccessCategory> Categories = new List<BcAccessCategory>();
    public static BcIdentificationServer MainServer = new BcIdentificationServer();
    public static Guid ServerId = Guid.Empty;
    private static readonly LimitedConcurrentQueue<FrameInfo> Requests = new LimitedConcurrentQueue<FrameInfo>(Convert.ToInt32(ConfigurationManager.AppSettings["MaxFrameCountInQueue"]));
    private static readonly List<CompareRequest> CommonRequests = new List<CompareRequest>();
    private static readonly List<BcDevices> AllDevices = new List<BcDevices>();
    public static PerformanceCounter CpuCounter = new PerformanceCounter()
    {
      CategoryName = "Processor",
      CounterName = "% Processor Time",
      InstanceName = "_Total"
    };
    public static SfinksClient DeviceClient;
    public static bool IsLoaded;
    public static bool StopFlag;
    public static IEngineWorker EngineWorker;
    public static IIniFileWorker IniFileWorker;
    public static Thread MainThread;

    public static string ApplicationFolder { get; set; }

    public static string StartPath
    {
      get
      {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
      }
    }

    public List<CommonContracts.SearchResult> SearchArchive(SearchRequest request)
    {
      throw new NotImplementedException();
    }

    public List<int> GetLastData(int lastLogId)
    {
      throw new NotImplementedException();
    }

    public List<int> GetLastDataByDevId(int lastLogId, Guid devId)
    {
      throw new NotImplementedException();
    }

    public void DbSearch(int resCount, float score, long dtFrom, long dtBefore, int sex, byte[] key)
    {
      throw new NotImplementedException();
    }

    public static void SendNotify(BcDevices dev, BcLog log, bool hasface, BcNotificationSystem sys)
    {
      QueueParameters notificationQueue = new QueueParameters()
      {
        HostName = sys.HostName,
        Password = sys.Password,
        UserName = sys.UserName,
        VirtualHost = sys.VirtualHost,
        QueueName = string.Concat(new object[4]
        {
          (object) sys.Fqdn,
          (object) ".",
          (object) sys.SystemId,
          (object) ".NotificationAPI.input"
        })
      };
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      try
      {
        byte[] numArray = (byte[]) null;
        DateTime dateTime = DateTime.Now;
        Guid faceId = log.FaceId;
        try
        {
          if (hasface)
          {
            using (SqlCommand sqlCommand = new SqlCommand("Select ImageIcon,Surname,FirstName,LastName, Birthday, AccessCategory.Name as category from Faces left outer join\r\n                                AccessCategory on Accesscategory.ID = Faces.AccessID  Where Faces.ID=@ID", new SqlConnection(CommonSettings.ConnectionString)))
            {
              sqlCommand.Parameters.Add(new SqlParameter("@ID", (object) log.FaceId));
              sqlCommand.Connection.Open();
              SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
              sqlDataReader.Read();
              numArray = (byte[]) sqlDataReader[0];
              str1 = sqlDataReader["category"].ToString();
              str3 = sqlDataReader["FirstName"].ToString();
              str4 = sqlDataReader["Surname"].ToString();
              str2 = sqlDataReader["LastName"].ToString();
              dateTime = (DateTime) sqlDataReader["Birthday"];
              sqlCommand.Connection.Close();
            }
          }
        }
        catch (Exception ex)
        {
          IdentificationServer.Logger.Error((object) "Notification error", ex);
        }
        NotificationParameters parameters = new NotificationParameters()
        {
          Id = Guid.NewGuid(),
          Category = str1,
          Camera = dev.Name,
          EventTime = DateTime.Now,
          Image1 = log.Image,
          Image2 = numArray
        };
        if (hasface)
        {
          parameters.NotificationData.Add("Birthday", dateTime.ToString());
          parameters.NotificationData.Add("FaceID", faceId.ToString());
        }
        parameters.NotificationData["Lastname"] = str2;
        parameters.NotificationData["Name"] = str3;
        parameters.NotificationData["Surname"] = str4;
        using (NotificationProtocolClient notificationProtocolClient = new NotificationProtocolClient(notificationQueue))
        {
          notificationProtocolClient.Open();
          notificationProtocolClient.SendNotification("Identification", parameters);
        }
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) "Notification error", ex);
      }
    }

    public static void SendCommand(BcDevices dev, int accessId, int sfinksId)
    {
      try
      {
        IdentificationServer.DeviceClient.SendCommand(IdentificationServer.LoginCom);
        IdentificationServer.DeviceClient.SendCommand(string.Concat(new object[4]
        {
          (object) "CASCADE_FACEDETECTED ",
          (object) sfinksId,
          (object) " ",
          (object) dev.Id.ToString().Replace("-", "")
        }));
        if (accessId == -1)
          return;
        BcAccessCategory catout;
        IdentificationServer.ReadWriteAccessCategory(false, accessId, (BcAccessCategory) null, out catout);
        if (catout == null || !IdentificationServer.DeviceClient.IsConnected)
          return;
        BcObjects bcObjects = Enumerable.FirstOrDefault<BcObjects>((IEnumerable<BcObjects>) catout.Data, (Func<BcObjects, bool>) (o => o.Id == dev.ObjectId));
        if (bcObjects == null)
          return;
        if (bcObjects.Device && dev.TableId == Guid.Empty)
        {
          try
          {
            IdentificationServer.DeviceClient.SendCommand(IdentificationServer.LoginCom);
            IdentificationServer.DeviceClient.SendCommand(IdentificationServer.AllowPass);
          }
          catch (Exception ex)
          {
            IdentificationServer.Logger.Error((object) "sfinks net error", ex);
          }
        }
        else if (dev.TableId != Guid.Empty)
        {
          if (BcObjectsData.GetObjectById(bcObjects.Data, dev.TableId).Device)
          {
            try
            {
              IdentificationServer.DeviceClient.SendCommand(IdentificationServer.LoginCom);
              IdentificationServer.DeviceClient.SendCommand(IdentificationServer.AllowPass);
            }
            catch (Exception ex)
            {
              IdentificationServer.Logger.Error((object) "sfinks net error", ex);
            }
          }
        }
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) "sfinks net error", ex);
      }
    }

    public static BcDevices GetDeviceById(Guid id)
    {
      return Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (d => d.Id == id)) ?? new BcDevices();
    }

    public static async void RefreshDevices()
    {
      while (!IdentificationServer.StopFlag)
      {
        if (!IdentificationServer.IsLoaded)
        {
          await Task.Delay(10);
        }
        else
        {
          IdentificationServer.TrySyncNotifications();
          IdentificationServer.TrySyncDevices();
          IdentificationServer.TrySyncBcAccessCategories();
          await Task.Delay(10000);
        }
      }
    }

    private static void TrySyncBcAccessCategories()
    {
      try
      {
        foreach (BcAccessCategory cat in BcAccessCategory.LoadAll())
        {
          BcAccessCategory catout;
          IdentificationServer.ReadWriteAccessCategory(true, cat.Id, cat, out catout);
        }
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) ex);
      }
    }

    private static void TrySyncDevices()
    {
      try
      {
        List<BcDevices> devices = BcDevicesStorageExtensions.LoadByIsid(IdentificationServer.ServerId);
        foreach (BcDevices bcDevices1 in devices)
        {
          BcDevices newDev = bcDevices1;
          BcDevices bcDevices2 = Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (n => n.Id == newDev.Id));
          if (bcDevices2 == null)
          {
            IdentificationServer.AllDevices.Add(newDev);
            newDev.CurrentThread = new Thread(new ParameterizedThreadStart(IdentificationServer.VideoThread))
            {
              IsBackground = true
            };
            newDev.CurrentThread.Start((object) newDev);
          }
          else
            bcDevices2.SetData(newDev.GetData());
        }
        foreach (BcDevices bcDevices in Enumerable.ToArray<BcDevices>(Enumerable.Where<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (n => Enumerable.All<BcDevices>((IEnumerable<BcDevices>) devices, (Func<BcDevices, bool>) (nn => nn.Id != n.Id))))))
          IdentificationServer.DestroyIfNeeded(bcDevices.CurrentThread);
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) ex);
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
        IdentificationServer.Logger.Error((object) "Device work thread was hard destroyed!", ex);
      }
    }

    private static void TrySyncNotifications()
    {
    }

    private static void ReadWriteAccessCategory(bool write, int id, BcAccessCategory cat, out BcAccessCategory catout)
    {
      lock (IdentificationServer.Categories)
      {
        catout = (BcAccessCategory) null;
        if (write)
        {
          bool local_0 = true;
          foreach (BcAccessCategory item_0 in IdentificationServer.Categories)
          {
            if (item_0.Id == id)
            {
              local_0 = false;
              if (item_0.ObjectData.Length != cat.ObjectData.Length)
              {
                item_0.ObjectData = cat.ObjectData;
                item_0.InCategory = cat.InCategory;
                item_0.Data = cat.GetData();
                item_0.Name = cat.Name;
                break;
              }
              break;
            }
          }
          if (!local_0)
            return;
          cat.Data = cat.GetData();
          IdentificationServer.Categories.Add(cat);
        }
        else if (id > 0)
        {
          foreach (BcAccessCategory item_1 in IdentificationServer.Categories)
          {
            if (item_1.Id == id)
            {
              catout = item_1;
              break;
            }
          }
        }
        else
        {
          foreach (BcAccessCategory item_2 in IdentificationServer.Categories)
          {
            if (!item_2.InCategory)
            {
              catout = item_2;
              break;
            }
          }
        }
      }
    }

    public static void WorkerThread()
    {
      while (!IdentificationServer.StopFlag)
      {
        IdentificationServer.ClearCompletedTasks();
        if (!IdentificationServer.IsLoaded)
          Thread.Sleep(100);
        else if (IdentificationServer.Requests.IsEmpty)
        {
          Thread.Sleep(100);
        }
        else
        {
          int trackingPeriod = IdentificationServer.SetGetServerSettings(false).TrackingPeriod * 1000;
          FrameInfo result;
          while (!IdentificationServer.StopFlag && IdentificationServer.Requests.TryDequeue(out result))
          {
            try
            {
              FrameInfo frame = result;
              IdentificationServer.ClearCompletedTasks();
              bool flag = false;
              foreach (CompareRequest compareRequest in Enumerable.Where<CompareRequest>((IEnumerable<CompareRequest>) IdentificationServer.CommonRequests, (Func<CompareRequest, bool>) (request => request.DeviceId == frame.Device.Id && request.RequestStatus == RequestStatus.Wait)))
              {
                if (compareRequest.HasSimilar(Settings.Default.MinScore, frame) && compareRequest.RequestStatus == RequestStatus.Wait)
                {
                  compareRequest.Add(frame);
                  flag = true;
                }
              }
              if (!flag)
              {
                CompareRequest compareRequest = new CompareRequest(frame.Device, IdentificationServer.Logger, IdentificationServer.EngineWorker);
                compareRequest.Add(frame);
                compareRequest.ProcessAfter(trackingPeriod);
                IdentificationServer.CommonRequests.Add(compareRequest);
              }
            }
            catch (Exception ex)
            {
              IdentificationServer.Logger.Error((object) "WorkerThread Errror - ", ex);
            }
          }
        }
      }
    }

    private static void ClearCompletedTasks()
    {
      IdentificationServer.CommonRequests.RemoveAll((Predicate<CompareRequest>) (request => request.RequestStatus == RequestStatus.Complete));
    }

    public static async void RefreshFaces()
    {
      while (!IdentificationServer.StopFlag)
      {
        if (IdentificationServer.IsLoaded)
        {
          try
          {
            bool flag1 = IdentificationServer.IniFileWorker.IsFileExists();
            DateTime lastUpdateDate = IdentificationServer.IniFileWorker.GetLastUpdateDate();
            DateTime databaseDate = IdentificationServer.GetDatabaseDate();
            if (Math.Abs((lastUpdateDate - databaseDate).TotalSeconds) > 10.0)
            {
              IEnumerable<KeysDataRow> enumerable = !flag1 || IdentificationServer.EngineWorker.TemplatesCount == 0 ? IdentificationServer.GetAllFaces() : IdentificationServer.GetLastModifiedFaces(lastUpdateDate);
              bool flag2 = false;
              foreach (KeysDataRow template in enumerable)
              {
                flag2 = true;
                IdentificationServer.EngineWorker.UpdateTemplateInfo(template);
              }
              foreach (Guid id in IdentificationServer.GetRemovedFaces(lastUpdateDate))
              {
                flag2 = true;
                IdentificationServer.EngineWorker.RemoveTemplate(id);
              }
              if (flag2)
                IdentificationServer.IniFileWorker.SetLastUpdateDate(databaseDate);
            }
          }
          catch (Exception ex)
          {
            IdentificationServer.Logger.Error((object) "Error", ex);
          }
        }
        await Task.Delay(5000);
      }
    }

    private static DateTime GetDatabaseDate()
    {
      return new DateTime(CommonSettings.GetSeverDate());
    }

    private static IEnumerable<KeysDataRow> GetLastModifiedFaces(DateTime dt)
    {
      using (SqlConnection connection = new SqlConnection(CommonSettings.ConnectionString))
      {
        using (SqlCommand sqlCommand = new SqlCommand("Select \r\nCSKeys.dbo.Keys.ID,CSKeys.dbo.Keys.FaceID,\r\ncase when CSKeys.dbo.Keys.ImageID is null\r\nthen cast(cast(0 as binary) as uniqueidentifier)  \r\nelse CSKeys.dbo.Keys.ImageID end as ImageID,\r\nCSKeys.dbo.Keys.ImageKey,\r\nCSKeys.dbo.Keys.KSID,AccessID,Faces.Score,cast('false' as bit) as Deleted,\r\nCSKeys.dbo.Keys.ModifiedDate,CascadeStream.dbo.Faces.Sex,CascadeStream.dbo.Faces.Birthday,\r\nCascadeStream.dbo.Faces.SfinksID, CascadeStream.dbo.Faces.Surname\r\nfrom CSKeys.dbo.Keys inner join CascadeStream.dbo.Faces on CascadeStream.dbo.Faces.ID = CSKeys.dbo.Keys.FaceID\r\nWHERE CSKeys.dbo.Keys.ModifiedDate >=@date order by Faces.ModifiedDate", connection))
        {
          sqlCommand.Connection.Open();
          sqlCommand.Parameters.Add(new SqlParameter("@date", (object) dt));
          SqlDataReader reader = sqlCommand.ExecuteReader();
          while (reader.Read())
            yield return new KeysDataRow((IDataRecord) reader);
        }
      }
    }

    private static IEnumerable<KeysDataRow> GetAllFaces()
    {
      using (SqlConnection connection = new SqlConnection(CommonSettings.ConnectionString))
      {
        using (SqlCommand sqlCommand = new SqlCommand("\r\nSelect\r\nCSKeys.dbo.Keys.ID,CSKeys.dbo.Keys.FaceID,\r\ncase when CSKeys.dbo.Keys.ImageID is null\r\nthen cast(cast(0 as binary) as uniqueidentifier)  \r\nelse CSKeys.dbo.Keys.ImageID end as ImageID,\r\nCSKeys.dbo.Keys.ImageKey,\r\nCSKeys.dbo.Keys.KSID,AccessID,\r\nFaces.Score,cast('false' as bit) as Deleted, \r\nCSKeys.dbo.Keys.ModifiedDate,CascadeStream.dbo.Faces.Sex,CascadeStream.dbo.Faces.Birthday,CascadeStream.dbo.Faces.SfinksID,\r\nCascadeStream.dbo.Faces.Surname\r\nfrom CSKeys.dbo.Keys inner join CascadeStream.dbo.Faces on CascadeStream.dbo.Faces.ID = CSKeys.dbo.Keys.FaceID order by CSKeys.dbo.Keys.CreateDate", connection))
        {
          sqlCommand.Connection.Open();
          SqlDataReader reader = sqlCommand.ExecuteReader();
          while (reader.Read())
            yield return new KeysDataRow((IDataRecord) reader);
        }
      }
    }

    private static IEnumerable<Guid> GetRemovedFaces(DateTime dt)
    {
      using (SqlConnection connection = new SqlConnection(CommonSettings.ConnectionString))
      {
        using (SqlCommand sqlCommand = new SqlCommand("Select CSKeys.dbo.DeletedKeys.KeyID from CSKeys.dbo.DeletedKeys Where DeletedDate>@date", connection))
        {
          sqlCommand.Parameters.Add(new SqlParameter("@date", (object) dt));
          sqlCommand.Connection.Open();
          using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
          {
            while (sqlDataReader.Read())
              yield return (Guid) sqlDataReader[0];
          }
        }
      }
    }

    public static BcIdentificationServer SetGetServerSettings(bool set)
    {
      lock (IdentificationServer.MainServer)
      {
        if (!set)
          return IdentificationServer.MainServer;
        try
        {
          IdentificationServer.MainServer = BcIdentificationServer.LoadById(IdentificationServer.ServerId);
        }
        catch (Exception exception_0)
        {
          IdentificationServer.Logger.Error((object) "Error while loading BcIdentificationServer", exception_0);
        }
        return IdentificationServer.MainServer;
      }
    }

    public void ClearResults(List<Guid> ids)
    {
      if (!IdentificationServer.IsLoaded)
        return;
      foreach (BcDevices bcDevices in IdentificationServer.AllDevices)
      {
        if (ids.Contains(bcDevices.Id))
        {
          bcDevices.IdentifierCount = 0;
          bcDevices.ResultCount = 0;
        }
      }
    }

    public DataTable GetDeviceInfo()
    {
      DataTable dataTable = new DataTable();
      dataTable.TableName = "IResults";
      dataTable.Columns.Add("ID", typeof (Guid));
      dataTable.Columns.Add("IdentifierCount", typeof (int));
      dataTable.Columns.Add("ResultCount", typeof (int));
      dataTable.Columns.Add("CPUUsage", typeof (int));
      dataTable.Columns.Add("ThreadCount", typeof (int));
      dataTable.Columns.Add("MaxThreadCount", typeof (int));
      dataTable.Columns.Add("ISState", typeof (string));
      int threadCount = IdentificationServer.MainServer.ThreadCount;
      int cpuUsage = IdentificationServer.GetCpuUsage();
      int processorCount = Environment.ProcessorCount;
      foreach (BcDevices bcDevices in IdentificationServer.AllDevices)
        dataTable.Rows.Add((object) bcDevices.Id, (object) bcDevices.IdentifierCount, (object) bcDevices.ResultCount, (object) cpuUsage, (object) threadCount, (object) processorCount, (object) "Работает");
      if (dataTable.Rows.Count > 0)
        dataTable.Rows.Add((object) Guid.Empty, (object) 0, (object) 0, (object) cpuUsage, (object) threadCount, (object) processorCount, (object) "Работает");
      return dataTable;
    }

    public bool GetLoadState()
    {
      return IdentificationServer.IsLoaded;
    }

    public string GetConnectionString()
    {
      return CommonSettings.ConnectionString;
    }

    public static void LoadServer(Guid serverId)
    {
      IdentificationServer.ServerId = serverId;
      IdentificationServer.MainServer = BcIdentificationServer.LoadById(IdentificationServer.ServerId);
      try
      {
        List<BcDevices> actualDevices = BcDevicesStorageExtensions.LoadByIsid(IdentificationServer.ServerId);
        foreach (BcDevices bcDevices in Enumerable.ToArray<BcDevices>(Enumerable.Where<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (devices => Enumerable.All<BcDevices>((IEnumerable<BcDevices>) actualDevices, (Func<BcDevices, bool>) (bcDevices => bcDevices.Id != devices.Id))))))
        {
          if (bcDevices.CurrentThread != null)
            IdentificationServer.DestroyIfNeeded(bcDevices.CurrentThread);
          IdentificationServer.AllDevices.Remove(bcDevices);
        }
        foreach (BcDevices bcDevices1 in actualDevices)
        {
          BcDevices d1 = bcDevices1;
          BcDevices bcDevices2 = Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (dev => dev.Id == d1.Id));
          if (bcDevices2 != null)
          {
            bcDevices2.SetData(d1.GetData());
          }
          else
          {
            IdentificationServer.AllDevices.Add(d1);
            d1.CurrentThread = new Thread(new ParameterizedThreadStart(IdentificationServer.VideoThread))
            {
              IsBackground = true
            };
            d1.CurrentThread.Start((object) d1);
          }
        }
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) ex);
      }
      IdentificationServer.IsLoaded = true;
      IdentificationServer.SetGetServerSettings(true);
      Task.Factory.StartNew(new Action(IdentificationServer.RefreshFaces));
      Task.Factory.StartNew(new Action(IdentificationServer.RefreshDevices));
    }

    public static void VideoThread(object device)
    {
      BcDevices bcDevices = (BcDevices) device;
      while (!IdentificationServer.StopFlag)
      {
        try
        {
          if (!bcDevices.IsActive)
          {
            Thread.Sleep(1000);
            continue;
          }
          BcExtractorServer bcExtractorServer = BcExtractorServer.LoadById(bcDevices.Esid);
          using (ExtractorServerClient extractorServerClient = new ExtractorServerClient(new InstanceContext((object) new IdentificationServer.ExtarctorClient())))
          {
            extractorServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcExtractorServer.Ip + ":" + (string) (object) bcExtractorServer.Port + "/FaceExtractorServer/ExtractorServer");
            extractorServerClient.Open();
            while (!IdentificationServer.StopFlag && bcDevices.IsActive)
            {
              KeyFrame lastFrame = extractorServerClient.GetLastFrame(bcDevices.Id);
              if (lastFrame == null)
              {
                Thread.Sleep(50);
              }
              else
              {
                IdentificationServer.Logger.Warn((object) ("New template recieved deviceId = " + (object) bcDevices.Id));
                byte[] imageIcon = IdentificationServer.GetImageIcon(lastFrame.Frame.Frame);
                if (lastFrame.Key != null && (int) lastFrame.Key[0] != 0)
                {
                  FrameInfo frameInfo = new FrameInfo()
                  {
                    Image = lastFrame.Frame.Frame,
                    ImageIcon = imageIcon,
                    Template = lastFrame.Key,
                    Date = lastFrame.Frame.Date.Ticks,
                    Device = bcDevices,
                    MinScore = (double) bcDevices.MinScore,
                    DetectedFrameId = lastFrame.Frame.FrameId
                  };
                  IdentificationServer.Requests.Enqueue(frameInfo);
                }
              }
            }
          }
        }
        catch (CommunicationException ex)
        {
          IdentificationServer.Logger.Error((object) ("Unable to connect to FaceExtractorServer " + ex.Message));
        }
        catch (Exception ex)
        {
          IdentificationServer.Logger.Error((object) ex);
        }
        Thread.Sleep(1000);
      }
    }

    private static byte[] GetImageIcon(byte[] imBytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(imBytes))
      {
        using (Bitmap bitmap1 = new Bitmap((Stream) memoryStream1))
        {
          using (Bitmap bitmap2 = new Bitmap((Image) bitmap1, new Size(72, 72)))
          {
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
              bitmap2.Save((Stream) memoryStream2, ImageFormat.Jpeg);
              return memoryStream2.ToArray();
            }
          }
        }
      }
    }

    public void SetDevice(Guid id, Hashtable row, bool delete)
    {
      BcDevices deviceById = IdentificationServer.GetDeviceById(id);
      BcDevices bcDevices1 = BcDevicesStorageExtensions.LoadById(id);
      if (bcDevices1.Isid != IdentificationServer.MainServer.Id)
        delete = true;
      if (delete)
      {
        BcDevices bcDevices2 = Enumerable.FirstOrDefault<BcDevices>((IEnumerable<BcDevices>) IdentificationServer.AllDevices, (Func<BcDevices, bool>) (d => d.Id == id));
        if (bcDevices2 == null)
          return;
        IdentificationServer.DestroyIfNeeded(bcDevices2.CurrentThread);
        IdentificationServer.AllDevices.Remove(bcDevices2);
      }
      else if (deviceById.Id != Guid.Empty)
        deviceById.SetData(row);
      else if (bcDevices1.Isid == IdentificationServer.MainServer.Id)
      {
        deviceById.SetData(row);
        IdentificationServer.AllDevices.Add(deviceById);
        deviceById.CurrentThread = new Thread(new ParameterizedThreadStart(IdentificationServer.VideoThread))
        {
          IsBackground = true
        };
        deviceById.CurrentThread.Start((object) deviceById);
      }
    }

    public void SetServerSettings()
    {
      IdentificationServer.SetGetServerSettings(true);
    }

    public static int GetCpuUsage()
    {
      return Convert.ToInt32(IdentificationServer.CpuCounter.NextValue());
    }

    public class ExtarctorClient : IExtractorServerCallback
    {
      public void CheckClient()
      {
      }
    }

    public delegate void SendNotifyFunc(BcDevices dev, BcLog log, bool hasface, BcNotificationSystem sys);
  }
}
