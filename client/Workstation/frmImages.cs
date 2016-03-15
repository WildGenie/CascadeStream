// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmImages
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using BasicComponents;
using BasicComponents.IdentificationServer;
using BasicComponents.ManagmentServer;
using BasicComponents.VideoServer;
using CS.Client.Common.Abstract;
using CS.Client.Common.Views;
using CS.DAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Media;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using TS.Sdk.StaticFace.NetBinding;

namespace CascadeFlowClient
{
  public class FrmImages : XtraForm
  {
    public static BcWorkStation CurrentStation = new BcWorkStation();
    public static List<BcDevices> Devices = new List<BcDevices>();
    public static List<BcLogBases> Bases = new List<BcLogBases>();
    public static DsResult Results = new DsResult();
    public static List<BcAccessCategory> Categories = new List<BcAccessCategory>();
    public static DataTable DtResults = new DataTable();
    public static List<BcObjects> Objects = new List<BcObjects>();
    public Color BlueColor = Color.FromArgb(227, 239, (int) byte.MaxValue);
    public Color RedColor = Color.IndianRed;
    public string HardwareKey = "123";
    private FrmImages.ClientManagmentServerCallback _client = new FrmImages.ClientManagmentServerCallback();
    private readonly System.Windows.Forms.Timer _frameTimer = new System.Windows.Forms.Timer();
    private List<BcDevices> _activeDevs = new List<BcDevices>();
    private object _mainObj = (object) 1;
    private List<Thread> _refreshThreads = new List<Thread>();
    private BcLogBases _currentDb = new BcLogBases();
    private FrmImages.VerifierCallBack _iclient = new FrmImages.VerifierCallBack();
    private BcAccessCategory _unCategory = new BcAccessCategory();
    private Bitmap _imgWarning = new Bitmap(32, 32);
    private Bitmap _imgInfo = new Bitmap(32, 32);
    private IContainer components = (IContainer) null;
    public static IEngine MainDetector;
    public static BcUser CurrentUser;
    public static bool AllowClose;
    private ManagmentServerClient _service;
    public static bool SqlServerState;
    private int _deviceCount;
    private int _activeDeviceCount;
    private Thread _checkService;
    private FrmResults _listForm;
    private bool _breakFlag;
    public static float Period;
    public Thread MainTread;
    public Thread RefreshObjects;
    private BarManager barManager1;
    private Bar bar2;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private BarLargeButtonItem barLargeButtonItem1;
    private BarButtonItem barButtonItem1;
    private GroupControl gbCamera4;
    public PictureBox pbImage4;
    private GroupControl gbCamera2;
    private GroupControl gbCamera3;
    public PictureBox pbImage3;
    private GroupControl gbCamera1;
    private SplitterControl splitterControl2;
    private ServiceController serviceController1;
    private SplitContainerControl splitContainerControl1;
    private SplitContainerControl splitContainerControl2;
    private SplitContainerControl splitContainerControl3;
    private Label lbServiceState;
    private Label lbDBServer;
    private Label lbTime;
    private System.Windows.Forms.Timer timer1;
    private Label lbFaceCount;
    private Label label1;
    public PictureBox pbImage1;
    public PictureBox pbImage2;

    public FrmImages()
    {
      this.InitializeComponent();
    }

    private void frmImages_Load(object sender, EventArgs e)
    {
      FrmImages.Period = 1f;
      AuthorizationForm authorizationForm = new AuthorizationForm("CascadeFlowClient", (ILocalizationProvider) new Messages());
      if (authorizationForm.ShowDialog() != DialogResult.OK)
      {
        FrmImages.AllowClose = true;
        this.Close();
      }
      else
      {
        FrmImages.CurrentUser = authorizationForm.User;
        try
        {
          FrmImages.MainDetector = (IEngine) new Engine();
        }
        catch (Exception ex)
        {
        }
        FrmImages.SqlServerState = true;
        this.splitContainerControl1.SplitterPosition = this.splitContainerControl1.Width / 2;
        this.splitContainerControl2.SplitterPosition = this.splitContainerControl2.Height / 2;
        this.splitContainerControl3.SplitterPosition = this.splitContainerControl3.Height / 2;
        try
        {
          Console.WriteLine("Managment Server Loading");
          BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
          CommonSettings.ManagmentServerAddress = "net.tcp://" + (object) bcManagmentServer.Ip + ":" + (string) (object) bcManagmentServer.Port + "/CSManagmentServer/ManagmentServer";
          this._service = new ManagmentServerClient(new InstanceContext((object) this._client));
          this._service.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
          this._service.Open();
          string key = this._service.ConnectOperator();
          Console.WriteLine("Managment Server loaded");
          FrmImages.CurrentStation = BcWorkStation.LoadByHardwareKey(key);
          try
          {
            Console.WriteLine("Get devices");
            StreamReader streamReader = new StreamReader("devices.ini");
            string[] strArray = streamReader.ReadToEnd().Split(new string[1]
            {
              "##"
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 0)
            {
              this._activeDevs = new List<BcDevices>();
              for (int index = 0; index < strArray.Length; ++index)
              {
                BcDevices bcDevices1 = BcDevicesStorageExtensions.LoadById((Guid) new GuidConverter().ConvertFromString(strArray[index]));
                foreach (BcDevices bcDevices2 in FrmImages.Devices)
                {
                  if (bcDevices1.IsActive && bcDevices1.Id == bcDevices2.Id)
                  {
                    if (index == 0)
                    {
                      bcDevices1.ImgControl = this.pbImage1;
                      this.pbImage1.Tag = (object) bcDevices1;
                      this.gbCamera1.Text = "(" + bcDevices1.Name + ")";
                    }
                    if (index == 1)
                    {
                      bcDevices1.ImgControl = this.pbImage2;
                      this.pbImage2.Tag = (object) bcDevices1;
                      this.gbCamera2.Text = "(" + bcDevices1.Name + ")";
                    }
                    if (index == 2)
                    {
                      bcDevices1.ImgControl = this.pbImage3;
                      this.pbImage3.Tag = (object) bcDevices1;
                      this.gbCamera3.Text = "(" + bcDevices1.Name + ")";
                    }
                    if (index == 3)
                    {
                      bcDevices1.ImgControl = this.pbImage4;
                      this.pbImage4.Tag = (object) bcDevices1;
                      this.gbCamera4.Text = "(" + bcDevices1.Name + ")";
                    }
                    bcDevices1.CurrentThread = new Thread(new ParameterizedThreadStart(this.WorkerThread))
                    {
                      IsBackground = true
                    };
                    bcDevices1.CurrentThread.Start((object) bcDevices1);
                    bcDevices1.CurrentIndex = index;
                    this._activeDevs.Add(bcDevices1);
                    break;
                  }
                }
              }
            }
            streamReader.Close();
            Console.WriteLine("devices loaded");
          }
          catch (Exception ex)
          {
            Console.WriteLine("Error get device {0} - {1}", (object) ex.Message, (object) ex.StackTrace);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error load Managment service {0} - {1}", (object) ex.Message, (object) ex.StackTrace);
          int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Console.WriteLine("Start check thread");
        this._checkService = new Thread(new ThreadStart(this.CheckService))
        {
          IsBackground = true
        };
        this._checkService.Start();
        FrmImages.DtResults.Columns.Add("ID", typeof (int));
        FrmImages.DtResults.Columns.Add("FaceID", typeof (Guid));
        FrmImages.DtResults.Columns.Add("DeviceID", typeof (Guid));
        FrmImages.DtResults.Columns.Add("ImageID", typeof (Guid));
        FrmImages.DtResults.Columns.Add("CategoryID", typeof (int));
        FrmImages.DtResults.Columns.Add("ObjectID", typeof (int));
        FrmImages.DtResults.Columns.Add("Date", typeof (DateTime));
        FrmImages.DtResults.Columns.Add("ImgType", typeof (Bitmap));
        FrmImages.DtResults.Columns.Add("Score", typeof (float));
        FrmImages.DtResults.Columns.Add("ImageIcon", typeof (Bitmap));
        FrmImages.DtResults.Columns.Add("Name", typeof (string));
        FrmImages.DtResults.Columns.Add("Category", typeof (string));
        FrmImages.DtResults.Columns.Add("Position", typeof (string));
        FrmImages.DtResults.Columns.Add("Status", typeof (bool));
        FrmImages.DtResults.Columns.Add("DBName", typeof (string));
        FrmImages.DtResults.Columns.Add("GroupID", typeof (string));
        FrmImages.DtResults.Columns.Add("ParentID", typeof (string));
        FrmImages.DtResults.Columns.Add("DeviceNumber", typeof (string));
        this._imgWarning = new Bitmap(Image.FromFile(Application.StartupPath + "\\warning64.png"), new Size(32, 32));
        this._imgInfo = new Bitmap(Image.FromFile(Application.StartupPath + "\\information64.png"), new Size(32, 32));
        if (this._listForm == null || this._listForm.IsDisposed)
          this._listForm = new FrmResults(this);
        this._listForm.Visible = false;
        this._listForm.Show();
        this._listForm.Visible = true;
        this._listForm.Hide();
        this.RefreshObjects = new Thread(new ThreadStart(this.ReloadObjects))
        {
          IsBackground = true
        };
        this.RefreshObjects.Start();
        this._frameTimer.Interval = 1000;
        this._frameTimer.Tick += new EventHandler(this.FrameTimer_Tick);
        this._frameTimer.Start();
      }
    }

    private void FrameTimer_Tick(object sender, EventArgs e)
    {
      foreach (BcDevices bcDevices in this._activeDevs)
        ++bcDevices.DetectionFaces;
    }

    private void RefreshDevices()
    {
      foreach (BcDevices bcDevices1 in this._activeDevs)
      {
        bool flag = false;
        foreach (BcDevices bcDevices2 in FrmImages.Devices)
        {
          if (bcDevices1.Id == bcDevices2.Id)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this._activeDevs.RemoveAt(this._activeDevs.IndexOf(bcDevices1));
          try
          {
            bcDevices1.CurrentThread.Abort();
            bcDevices1.ImgControl.Image = (Image) new Bitmap(bcDevices1.ImgControl.Width, bcDevices1.ImgControl.Height);
            bcDevices1.ImgControl.Parent.Text = "";
          }
          catch
          {
          }
        }
      }
    }

    private void CheckService()
    {
      while (true)
      {
        try
        {
          this._deviceCount = 0;
          this._activeDeviceCount = 0;
          this.Invoke((Delegate) new FrmImages.RefreshDevicesFunc(this.RefreshDevices));
          FrmImages.CurrentStation = BcWorkStation.LoadByHardwareKey(this._service.ConnectOperator());
          if (this._service.State == CommunicationState.Opened)
          {
            if (!FrmImages.CurrentStation.Status)
            {
              foreach (BcDevices bcDevices in this._activeDevs)
                bcDevices.CurrentThread.Abort();
            }
            this._activeDeviceCount = this._activeDevs.Count;
            this._deviceCount = FrmImages.Devices.Count;
            this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.ManagmentServerWork, (object) 0);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Service unavailible {0} - {1}", (object) ex.Message, (object) ex.StackTrace);
          try
          {
            this._service.Abort();
            this._service.Close();
          }
          catch
          {
          }
          try
          {
            BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
            CommonSettings.ManagmentServerAddress = "net.tcp://" + (object) bcManagmentServer.Ip + ":" + (string) (object) bcManagmentServer.Port + "/CSManagmentServer/ManagmentServer";
            this._service = new ManagmentServerClient(new InstanceContext((object) this._client));
            this._service.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
            this._service.Open();
          }
          catch
          {
          }
          this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.ManagmentServerUnavailble, (object) 0);
        }
        if (FrmImages.SqlServerState)
        {
          try
          {
            this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.DatabaseServerAvailable, (object) 2);
          }
          catch
          {
          }
        }
        else
        {
          try
          {
            this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.DatabaseServerUnavailable, (object) 2);
          }
          catch
          {
          }
        }
        Thread.Sleep(5000);
      }
    }

    private void RefreshStatus(string meaasage, int val)
    {
      this.label1.Text = Messages.AllDevices + (object) ": " + (string) (object) this._deviceCount + ", " + Messages.AllViewDevices + ": " + (string) (object) this._activeDeviceCount;
      if (val == 0)
        this.lbServiceState.Text = meaasage;
      else
        this.lbDBServer.Text = meaasage;
    }

    private void WorkerThread(object device)
    {
      while (true)
      {
        bool flag = true;
        BcDevices bcDevices = (BcDevices) device;
        BcVideoServer bcVideoServer = BcVideoServer.LoadById(bcDevices.Vsid);
        try
        {
          VideoServerClient videoServerClient = new VideoServerClient();
          videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
          videoServerClient.Open();
          bcDevices.DetectorCount = 0;
          bcDevices.DetectionFaces = 1;
          while (true)
          {
            flag = true;
            byte[] imageFromDevice = videoServerClient.GetImageFromDevice(bcDevices.Id);
            if (imageFromDevice != null && imageFromDevice.Length > 0)
            {
              ++bcDevices.DetectorCount;
              Bitmap bitmap = new Bitmap((Stream) new MemoryStream(imageFromDevice));
              try
              {
                this.Invoke((Delegate) new FrmImages.Newimagefunc(this.NewImage), (object) bcDevices.ImgControl, (object) bitmap, (object) bcDevices.CurrentIndex, (object) bcDevices);
              }
              catch
              {
              }
            }
            else
              Thread.Sleep(30);
          }
        }
        catch
        {
          Thread.Sleep(100);
        }
      }
    }

    private void NewImage(PictureBox pb, Bitmap bmp, int index, BcDevices dev)
    {
      try
      {
        pb.Image.Dispose();
      }
      catch
      {
      }
      pb.Image = (Image) bmp;
    }

    private void frmImages_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._checkService != null)
        this._checkService.Abort();
      if (!FrmImages.AllowClose)
      {
        if (XtraMessageBox.Show(Messages.DoYouWantToExitNow, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          this._breakFlag = true;
          foreach (Thread thread in this._refreshThreads)
          {
            try
            {
              thread.Abort();
            }
            catch
            {
            }
          }
          FrmImages.AllowClose = true;
          this._listForm.Close();
          try
          {
            this.MainTread.Abort();
          }
          catch
          {
          }
          try
          {
            this.RefreshObjects.Abort();
          }
          catch
          {
          }
          foreach (BcDevices bcDevices in this._activeDevs)
          {
            if (bcDevices.CurrentThread != null)
              bcDevices.CurrentThread.Abort();
          }
          StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\devices.ini", false);
          string str = "";
          foreach (BcDevices bcDevices in this._activeDevs)
            str = str + (object) bcDevices.Id + "##";
          streamWriter.Write(str);
          streamWriter.Close();
        }
        else
          e.Cancel = true;
      }
      else
      {
        FrmImages.AllowClose = true;
        try
        {
          this._listForm.Close();
        }
        catch
        {
        }
        try
        {
          this.MainTread.Abort();
        }
        catch
        {
        }
        try
        {
          this.RefreshObjects.Abort();
        }
        catch
        {
        }
        foreach (BcDevices bcDevices in this._activeDevs)
        {
          try
          {
            bcDevices.CurrentThread.Abort();
          }
          catch
          {
          }
        }
      }
    }

    private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this._listForm == null || this._listForm.IsDisposed)
        this._listForm = new FrmResults(this);
      this._listForm.Activate();
      this._listForm.Show();
    }

    private void barLargeButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmDevices frmDevices = new FrmDevices(this._activeDevs);
      if (frmDevices.ShowDialog() != DialogResult.OK)
        return;
      foreach (Thread thread in this._refreshThreads)
        thread.Abort();
      foreach (BcDevices bcDevices in this._activeDevs)
      {
        if (bcDevices.CurrentThread != null)
        {
          bcDevices.CurrentThread.Abort();
          while (true)
          {
            if (bcDevices.CurrentThread.ThreadState != ThreadState.Aborted && bcDevices.CurrentThread.ThreadState != ThreadState.Stopped)
              Thread.Sleep(100);
            else
              break;
          }
        }
        bcDevices.CurrentThread = (Thread) null;
      }
      Thread.Sleep(100);
      this._activeDevs.Clear();
      int num = 0;
      this.pbImage1.Image = (Image) null;
      this.gbCamera1.Text = "";
      this.pbImage2.Image = (Image) null;
      this.gbCamera2.Text = "";
      this.pbImage3.Image = (Image) null;
      this.gbCamera3.Text = "";
      this.pbImage4.Image = (Image) null;
      this.gbCamera4.Text = "";
      foreach (BcDevices bcDevices in frmDevices.ActiveDevices)
      {
        if (bcDevices.IsActive)
        {
          this._activeDevs.Add(bcDevices);
          if (num == 0)
          {
            bcDevices.ImgControl = this.pbImage1;
            this.gbCamera1.Text = "(" + bcDevices.Name + ")";
            this.pbImage1.Tag = (object) bcDevices;
          }
          if (num == 1)
          {
            bcDevices.ImgControl = this.pbImage2;
            this.gbCamera2.Text = "(" + bcDevices.Name + ")";
            this.pbImage2.Tag = (object) bcDevices;
          }
          if (num == 2)
          {
            bcDevices.ImgControl = this.pbImage3;
            this.gbCamera3.Text = "(" + bcDevices.Name + ")";
            this.pbImage3.Tag = (object) bcDevices;
          }
          if (num == 3)
          {
            bcDevices.ImgControl = this.pbImage4;
            this.gbCamera4.Text = "(" + bcDevices.Name + ")";
            this.pbImage4.Tag = (object) bcDevices;
          }
          ++num;
          bcDevices.CurrentIndex = num;
          if (bcDevices.IsActive)
          {
            Thread thread = new Thread(new ParameterizedThreadStart(this.RefreshGrid))
            {
              Name = bcDevices.Id.ToString(),
              IsBackground = true
            };
            thread.Start((object) bcDevices);
            this._refreshThreads.Add(thread);
            bcDevices.CurrentThread = new Thread(new ParameterizedThreadStart(this.WorkerThread))
            {
              IsBackground = true
            };
            bcDevices.CurrentThread.Start((object) bcDevices);
          }
        }
      }
    }

    private void splitContainerControl3_SplitterPositionChanged(object sender, EventArgs e)
    {
      this.splitContainerControl2.SplitterPosition = this.splitContainerControl3.SplitterPosition;
    }

    private void splitContainerControl2_SplitterPositionChanged(object sender, EventArgs e)
    {
      this.splitContainerControl3.SplitterPosition = this.splitContainerControl2.SplitterPosition;
    }

    private void AddNewRow(DataRow row)
    {
      lock (this._mainObj)
      {
        DateTime local_0 = new DateTime();
        DateTime local_0_2 = ((DateTime) row["Date"]).AddSeconds(-(double) FrmImages.Period);
        if (row["Status"].ToString() != "" && !Convert.ToBoolean(row["Status"]))
        {
          BcDevices local_2 = FrmImages.GetDeviceById((Guid) row["DeviceID"]);
          if (this.pbImage1.Tag != null && ((BcDevices) this.pbImage1.Tag).Id == local_2.Id)
            this.pbImage1.BackColor = this.RedColor;
          else if (this.pbImage2.Tag != null && ((BcDevices) this.pbImage2.Tag).Id == local_2.Id)
            this.pbImage2.BackColor = this.RedColor;
          else if (this.pbImage3.Tag != null && ((BcDevices) this.pbImage3.Tag).Id == local_2.Id)
            this.pbImage3.BackColor = this.RedColor;
          else if (this.pbImage4.Tag != null && ((BcDevices) this.pbImage4.Tag).Id == local_2.Id)
            this.pbImage4.BackColor = this.RedColor;
        }
        if (row["FaceID"].ToString() != Guid.Empty.ToString() && row["FaceID"].ToString() != "")
        {
          DataRow[] local_3 = FrmImages.DtResults.Select("FaceID = '" + row["FaceID"] + "' and Date > '" + (string) (object) local_0_2 + "' and DeviceID = '" + (string) row["DeviceID"] + "'");
          if (local_3.Length > 0)
          {
            row["GroupID"] = (object) FrmImages.DtResults.Rows.Count;
            row["ParentID"] = local_3[0]["ParentID"];
            FrmImages.DtResults.Rows.Add(row.ItemArray);
          }
          else
          {
            row["GroupID"] = (object) FrmImages.DtResults.Rows.Count;
            row["ParentID"] = row["GroupID"];
            FrmImages.DtResults.Rows.Add(row.ItemArray);
          }
        }
        else
        {
          DateTime local_4 = (DateTime) row["Date"];
          int local_16 = local_4.Month;
          string local_5 = local_16.ToString();
          if (local_4.Month < 9)
            local_5 = "0" + (object) local_4.Month;
          local_16 = local_4.Day;
          string local_6 = local_16.ToString();
          if (local_4.Day < 9)
            local_6 = "0" + (object) local_4.Day;
          string local_7 = (string) (object) local_4.Year + (object) local_5 + local_6;
          string local_1_1 = row["Name"].ToString();
          if (local_1_1.Length > 30)
            local_1_1 = local_1_1.Substring(0, 30);
          else if (local_1_1.Length < 30)
          {
            string local_8 = "";
            for (int local_9 = 0; local_9 < 30 - local_1_1.Length; ++local_9)
              local_8 += " ";
            local_1_1 = local_8 + local_1_1;
          }
          string local_10 = row["FaceID"].ToString();
          if (local_10.Length > 30)
            local_10 = local_10.Substring(0, 30);
          else if (local_10.Length < 30)
          {
            string local_8_1 = "";
            for (int local_9_1 = 0; local_9_1 < 30 - local_1_1.Length; ++local_9_1)
              local_8_1 += "0";
            local_10 = local_8_1 + local_10;
          }
          row["GroupID"] = (object) (local_7 + (object) "%Date" + (string) (object) local_4 + "%ID" + local_10 + " %Name" + (string) (object) Guid.NewGuid() + " %Time(" + (string) (object) new TimeSpan(local_4.Hour, local_4.Minute, local_4.Second) + "-" + (string) (object) new TimeSpan(local_4.Hour, local_4.Minute, local_4.Second + (int) FrmImages.Period) + ")" + (string) row["DeviceNumber"]);
          row["ParentID"] = row["GroupID"];
          FrmImages.DtResults.Rows.Add(row.ItemArray);
        }
        this._listForm.treeList1.MoveFirst();
        this.lbFaceCount.Text = Messages.PeopleCount + (object) ": " + (string) (object) FrmImages.DtResults.Rows.Count;
      }
    }

    private void ReloadObjects()
    {
      while (true)
      {
        try
        {
          FrmImages.Objects = BcObjects.LoadAll();
          FrmImages.SqlServerState = true;
          if (FrmImages.Results.dtImageType.Rows.Count == 0)
          {
            FrmImages.Results.dtImageType.Rows.Add((object) 1, (object) this._imgWarning);
            FrmImages.Results.dtImageType.Rows.Add((object) 2, (object) new Bitmap(128, 128));
          }
          FrmImages.Categories = BcAccessCategory.LoadAll();
          foreach (BcAccessCategory bcAccessCategory in FrmImages.Categories)
          {
            if (!bcAccessCategory.InCategory)
              this._unCategory = bcAccessCategory;
            bcAccessCategory.GetData();
            bool flag = true;
            foreach (DataRow dataRow in (InternalDataCollectionBase) FrmImages.Results.dtCategories.Rows)
            {
              if (dataRow[0].ToString() == bcAccessCategory.Id.ToString())
              {
                flag = false;
                dataRow[1] = (object) bcAccessCategory.Name;
                break;
              }
            }
            if (flag)
              FrmImages.Results.dtCategories.Rows.Add((object) bcAccessCategory.Id, (object) bcAccessCategory.Name);
          }
          try
          {
            lock (this._currentDb)
            {
              FrmImages.Bases = BcLogBases.LoadAll();
              foreach (BcLogBases item_1 in FrmImages.Bases)
              {
                if (item_1.CloseDate == DateTime.MinValue && this._currentDb.Id != item_1.Id)
                {
                  this._currentDb = item_1;
                  break;
                }
              }
            }
            FrmImages.Categories = BcAccessCategory.LoadAll();
            FrmImages.Objects = BcObjects.LoadAll();
            BcLogBases.LoadAll();
            List<BcDevices> list = BcDevicesStorageExtensions.LoadAllByWorkStationId(FrmImages.CurrentStation.Id);
            foreach (Thread thread in this._refreshThreads)
            {
              bool flag = true;
              foreach (BcDevices bcDevices in list)
              {
                if (bcDevices.Id.ToString() == thread.Name)
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                thread.Abort();
            }
            foreach (BcDevices bcDevices1 in list)
            {
              bool flag1 = true;
              foreach (BcDevices bcDevices2 in FrmImages.Devices)
              {
                if (bcDevices2.Id == bcDevices1.Id)
                {
                  flag1 = false;
                  break;
                }
              }
              if (flag1)
                FrmImages.Devices.Add(bcDevices1);
              bool flag2 = true;
              foreach (DataRow dataRow in (InternalDataCollectionBase) FrmImages.Results.dtDevices.Rows)
              {
                if (dataRow[0].ToString() == bcDevices1.Id.ToString())
                {
                  flag2 = false;
                  using (List<BcObjects>.Enumerator enumerator = FrmImages.Objects.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      BcObjects current = enumerator.Current;
                      if (current.Id == bcDevices1.ObjectId)
                      {
                        current.GetData();
                        string str = current.Name;
                        if (bcDevices1.TableId != Guid.Empty)
                          str = current.Name + "-" + BcObjectsData.GetObjectById(current.Data, bcDevices1.TableId).Name;
                        dataRow[0] = (object) bcDevices1.Id;
                        dataRow[1] = (object) bcDevices1.Name;
                        dataRow[2] = (object) bcDevices1.ObjectId;
                        dataRow[3] = (object) bcDevices1.TableId;
                        dataRow[4] = (object) str;
                        break;
                      }
                    }
                    break;
                  }
                }
              }
              if (flag2)
              {
                foreach (BcObjects bcObjects in FrmImages.Objects)
                {
                  if (bcObjects.Id == bcDevices1.ObjectId)
                  {
                    bcObjects.GetData();
                    string str = bcObjects.Name;
                    if (bcDevices1.TableId != Guid.Empty)
                      str = bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, bcDevices1.TableId).Name;
                    FrmImages.Results.dtDevices.Rows.Add((object) bcDevices1.Id, (object) bcDevices1.Name, (object) bcDevices1.ObjectId, (object) bcDevices1.TableId, (object) str);
                    break;
                  }
                }
              }
            }
            FrmImages.SqlServerState = true;
          }
          catch
          {
            FrmImages.SqlServerState = false;
          }
        }
        catch
        {
          FrmImages.SqlServerState = false;
        }
        this.RefreshObjects.Join(10000);
      }
    }

    public static BcAccessCategory GetCategoryById(int id)
    {
      BcAccessCategory bcAccessCategory1 = new BcAccessCategory();
      foreach (BcAccessCategory bcAccessCategory2 in FrmImages.Categories)
      {
        if (id == bcAccessCategory2.Id)
          return bcAccessCategory2;
      }
      return bcAccessCategory1;
    }

    public static BcDevices GetDeviceById(Guid id)
    {
      BcDevices bcDevices1 = new BcDevices();
      foreach (BcDevices bcDevices2 in FrmImages.Devices)
      {
        if (id == bcDevices2.Id)
          return bcDevices2;
      }
      return bcDevices1;
    }

    private BcObjects GetObjectById(List<BcObjects> obj, int id)
    {
      BcObjects bcObjects1 = new BcObjects();
      foreach (BcObjects bcObjects2 in obj)
      {
        if (id == bcObjects2.Id)
          return bcObjects2;
      }
      return bcObjects1;
    }

    public int GetMaxIdByDeviceId(Guid id)
    {
      int num = -1;
      foreach (DataRow dataRow in (InternalDataCollectionBase) FrmImages.DtResults.Rows)
      {
        if ((Guid) dataRow["DeviceID"] == id && num < (int) dataRow["ID"])
          num = (int) dataRow["ID"];
      }
      return num;
    }

    private void RefreshGrid(object objects)
    {
      BcDevices bcDevices = (BcDevices) objects;
      Console.WriteLine("Identification Server starting device name {0}", (object) bcDevices.Name);
      while (true)
      {
        bool flag1 = true;
        if (!this._breakFlag)
        {
          Thread.Sleep(1000);
          try
          {
            BcIdentificationServer identificationServer = BcIdentificationServer.LoadById(bcDevices.Isid);
            IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext((object) this._iclient));
            identificationServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) identificationServer.Ip + ":" + (string) (object) identificationServer.Port + "/FaceIdentification/IdentificationServer");
            identificationServerClient.Open();
            Console.WriteLine("Identification Server Opened device name {0}", (object) bcDevices.Name);
            this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.IdentificationServerWork, (object) 1);
            while (true)
            {
              flag1 = true;
              this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.IdentificationServerWork, (object) 1);
              if (!this._breakFlag)
              {
                Thread.Sleep(1000);
                try
                {
                  DataTable dataTable = new DataTable();
                  dataTable.Columns.Add("ID", typeof (int));
                  dataTable.Columns.Add("FaceID", typeof (Guid));
                  dataTable.Columns.Add("DeviceID", typeof (Guid));
                  dataTable.Columns.Add("ImageID", typeof (Guid));
                  dataTable.Columns.Add("CategoryID", typeof (int));
                  dataTable.Columns.Add("ObjectID", typeof (int));
                  dataTable.Columns.Add("Date", typeof (DateTime));
                  dataTable.Columns.Add("imgType", typeof (Bitmap));
                  dataTable.Columns.Add("Score", typeof (float));
                  dataTable.Columns.Add("ImageIcon", typeof (Bitmap));
                  dataTable.Columns.Add("Name", typeof (string));
                  dataTable.Columns.Add("Category", typeof (string));
                  dataTable.Columns.Add("Position", typeof (string));
                  dataTable.Columns.Add("Status", typeof (bool));
                  dataTable.Columns.Add("DBName", typeof (string));
                  dataTable.Columns.Add("GroupID", typeof (string));
                  dataTable.Columns.Add("ParentID", typeof (string));
                  dataTable.Columns.Add("DeviceNumber", typeof (string));
                  SqlCommand sqlCommand1 = new SqlCommand("Select\r\nLog.ID,\r\nlog.faceID,\r\nLog.DeviceID,\r\nLog.ImageID,\r\nface.AccessID as CategoryID,\r\nLog.ObjectID,\r\n0 as imgTypeID,\r\nLog.Date,\r\nLog.Score,\r\nLog.ImageIcon,\r\nface.Surname+' '+face.FirstName+' '+ face.LastName as Name,\r\nLog.Status,\r\nDB_NAME() AS DBName,\r\nCascadeStream.dbo.AccessCategory.Name as Category\r\nfrom  [dbo].[Log] left outer join CascadeStream.dbo.Faces as face  on face.ID = Log.FaceID left outer join \r\nCascadeStream.dbo.Devices as dev on Log.DeviceID = dev.ID\r\nleft outer join \r\nCascadeStream.dbo.AccessCategory on\r\nface.AccessID = CascadeStream.dbo.AccessCategory.ID\r\nWhere\r\nLog.ID  = @ID ", new SqlConnection(CommonSettings.ConnectionStringLog));
                  sqlCommand1.CommandTimeout = 0;
                  int maxIdByDeviceId = this.GetMaxIdByDeviceId(bcDevices.Id);
                  if (maxIdByDeviceId != -1)
                  {
                    Console.WriteLine("Load next data device name {0}", (object) bcDevices.Name);
                    int[] lastDataByDevId = identificationServerClient.GetLastDataByDevId(maxIdByDeviceId, bcDevices.Id);
                    Console.WriteLine("ids count {0}, device name {1}", (object) lastDataByDevId.Length, (object) bcDevices.Name);
                    foreach (int num in lastDataByDevId)
                    {
                      sqlCommand1.Parameters.Clear();
                      sqlCommand1.Connection.Open();
                      sqlCommand1.Parameters.Add(new SqlParameter("@ID", (object) num));
                      SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                      while (sqlDataReader.Read())
                      {
                        try
                        {
                          BcAccessCategory bcAccessCategory = sqlDataReader["CategoryID"].ToString() != "" ? FrmImages.GetCategoryById(Convert.ToInt32(sqlDataReader["CategoryID"])) : this._unCategory;
                          if (bcAccessCategory.Id == -1)
                            bcAccessCategory = this._unCategory;
                          BcObjects objectById1 = this.GetObjectById(bcAccessCategory.Data, bcDevices.ObjectId);
                          BcObjectsData objectById2 = BcObjectsData.GetObjectById(objectById1.Data, bcDevices.TableId);
                          bool flag2 = false;
                          if (bcDevices.TableId != Guid.Empty)
                          {
                            objectById2 = BcObjectsData.GetObjectById(objectById1.Data, bcDevices.TableId);
                            if (objectById2.InList || objectById2.Sound || objectById2.Warning)
                              flag2 = true;
                          }
                          else if (objectById1.InList || objectById1.Sound || objectById1.Warning)
                            flag2 = true;
                          if (flag2)
                          {
                            SoundPlayer soundPlayer = new SoundPlayer();
                            DataRow dataRow = dataTable.Rows.Add();
                            dataRow["ID"] = sqlDataReader["ID"];
                            dataRow["ImageID"] = sqlDataReader["ImageID"];
                            dataRow["FaceID"] = sqlDataReader["FaceID"];
                            dataRow["DeviceID"] = sqlDataReader["DeviceID"];
                            dataRow["CategoryID"] = !(sqlDataReader["CategoryID"].ToString() != "") ? (object) -1 : sqlDataReader["CategoryID"];
                            dataRow["ObjectID"] = sqlDataReader["ObjectID"];
                            dataRow["Date"] = sqlDataReader["Date"];
                            dataRow["Score"] = sqlDataReader["Score"];
                            Bitmap bitmap = new Bitmap((Stream) new MemoryStream((byte[]) sqlDataReader["ImageIcon"]));
                            Graphics.FromImage((Image) bitmap).DrawRectangle(new Pen(Brushes.Blue, 2.5f), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                            dataRow["DeviceNumber"] = (object) bcDevices.Name;
                            dataRow["ImageIcon"] = (object) bitmap;
                            dataRow["Name"] = sqlDataReader["Name"];
                            dataRow["Category"] = !(sqlDataReader["CategoryID"].ToString() != "") ? (object) "Не в категории" : sqlDataReader["Category"];
                            dataRow["Status"] = sqlDataReader["Status"];
                            dataRow["DBName"] = sqlDataReader["DbName"];
                            if (FrmImages.Results.dtDevices.Select("DeviceID = '" + dataRow["DeviceID"] + "'").Length > 0)
                              dataRow["Position"] = FrmImages.Results.dtDevices.Select("DeviceID = '" + dataRow["DeviceID"] + "'")[0]["Position"];
                            else
                              dataTable.Rows[dataTable.Rows.Count - 1]["Position"] = (object) "";
                            if (bcDevices.ObjectId != -1)
                            {
                              if (bcDevices.TableId == Guid.Empty && !objectById1.Warning)
                              {
                                if (objectById1.Sound)
                                  soundPlayer.SoundLocation = Application.StartupPath + "\\List.wav";
                                dataRow["imgType"] = (object) this._imgInfo;
                              }
                              else if (bcDevices.TableId == Guid.Empty && objectById1.Warning)
                              {
                                if (objectById1.Sound)
                                  soundPlayer.SoundLocation = Application.StartupPath + "\\Warning.wav";
                                dataRow["imgType"] = (object) this._imgWarning;
                              }
                              else if (bcDevices.TableId != Guid.Empty && objectById2.Warning)
                              {
                                if (objectById2.Sound)
                                  soundPlayer.SoundLocation = Application.StartupPath + "\\Warning.wav";
                                dataRow["imgType"] = (object) this._imgWarning;
                              }
                              else
                              {
                                if (objectById2.Sound)
                                  soundPlayer.SoundLocation = Application.StartupPath + "\\List.wav";
                                dataRow["imgType"] = (object) this._imgInfo;
                              }
                            }
                            else
                              dataRow["imgType"] = (object) this._imgInfo;
                            this.Invoke((Delegate) new FrmImages.AddNewRowsFunc(this.AddNewRow), (object) dataRow);
                            if (dataRow["Status"].ToString() != "" && !Convert.ToBoolean(dataRow["Status"]) && soundPlayer.SoundLocation != "")
                              soundPlayer.Play();
                          }
                        }
                        catch (Exception ex)
                        {
                          Console.WriteLine("Error loading data from datareader {0}, {1},{2}", (object) bcDevices.Name, (object) ex.Message, (object) ex.StackTrace);
                        }
                        dataTable.Rows.Clear();
                      }
                      sqlDataReader.Close();
                      sqlCommand1.Connection.Close();
                    }
                  }
                  else
                  {
                    Console.WriteLine("Load first data device name {0}", (object) bcDevices.Name);
                    SqlCommand sqlCommand2 = new SqlCommand("Select\r\nLog.ID,\r\nlog.faceID,\r\nLog.DeviceID,\r\nLog.ImageID,\r\nface.AccessID as CategoryID,\r\nLog.ObjectID,\r\n0 as imgTypeID,\r\nLog.Date,\r\nLog.Score,\r\nLog.ImageIcon,\r\nface.Surname+' '+face.FirstName+' '+ face.LastName as Name,\r\nLog.Status,\r\nDB_NAME() AS DBName,\r\nCascadeStream.dbo.AccessCategory.Name as Category\r\nfrom  [dbo].[Log] left outer join CascadeStream.dbo.Faces as face  on face.ID = Log.FaceID left outer join \r\nCascadeStream.dbo.Devices as dev on Log.DeviceID = dev.ID\r\nleft outer join \r\nCascadeStream.dbo.AccessCategory on\r\nface.AccessID = CascadeStream.dbo.AccessCategory.ID\r\nWhere\r\nLog.Date > @date and DeviceID = '" + (object) bcDevices.Id + "'\r\norder by Log.ID asc \r\n ", new SqlConnection(CommonSettings.ConnectionStringLog));
                    sqlCommand2.CommandTimeout = 0;
                    DateTime dateTime = DateTime.Now.AddHours(-10.0);
                    sqlCommand2.Parameters.Add(new SqlParameter("@date", (object) dateTime));
                    sqlCommand2.Connection.Open();
                    SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                      try
                      {
                        BcAccessCategory bcAccessCategory = sqlDataReader["CategoryID"].ToString() != "" ? FrmImages.GetCategoryById(Convert.ToInt32(sqlDataReader["CategoryID"])) : this._unCategory;
                        if (bcAccessCategory.Id == -1)
                          bcAccessCategory = this._unCategory;
                        BcObjects objectById1 = this.GetObjectById(bcAccessCategory.Data, bcDevices.ObjectId);
                        BcObjectsData objectById2 = BcObjectsData.GetObjectById(objectById1.Data, bcDevices.TableId);
                        bool flag2 = false;
                        if (bcDevices.TableId != Guid.Empty)
                        {
                          objectById2 = BcObjectsData.GetObjectById(objectById1.Data, bcDevices.TableId);
                          if (objectById2.InList || objectById2.Sound || objectById2.Warning)
                            flag2 = true;
                        }
                        else if (objectById1.InList || objectById1.Sound || objectById1.Warning)
                          flag2 = true;
                        if (flag2)
                        {
                          SoundPlayer soundPlayer = new SoundPlayer();
                          DataRow dataRow = dataTable.Rows.Add();
                          dataRow["ID"] = sqlDataReader["ID"];
                          dataRow["DeviceNumber"] = (object) bcDevices.Name;
                          dataRow["ImageID"] = sqlDataReader["ImageID"];
                          dataRow["FaceID"] = sqlDataReader["FaceID"];
                          dataRow["DeviceID"] = sqlDataReader["DeviceID"];
                          dataRow["CategoryID"] = !(sqlDataReader["CategoryID"].ToString() != "") ? (object) -1 : sqlDataReader["CategoryID"];
                          dataRow["ObjectID"] = sqlDataReader["ObjectID"];
                          dataRow["Date"] = sqlDataReader["Date"];
                          dataRow["Score"] = sqlDataReader["Score"];
                          Bitmap bitmap = new Bitmap((Stream) new MemoryStream((byte[]) sqlDataReader["ImageIcon"]));
                          Graphics.FromImage((Image) bitmap).DrawRectangle(new Pen(Brushes.Blue, 2.5f), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                          dataRow["ImageIcon"] = (object) bitmap;
                          dataRow["Name"] = sqlDataReader["Name"];
                          dataRow["Category"] = !(sqlDataReader["Category"].ToString() != "") ? (object) "Не в категории" : sqlDataReader["Category"];
                          dataRow["Status"] = sqlDataReader["Status"];
                          dataRow["DBName"] = sqlDataReader["DbName"];
                          if (FrmImages.Results.dtDevices.Select("DeviceID = '" + dataRow["DeviceID"] + "'").Length > 0)
                            dataRow["Position"] = FrmImages.Results.dtDevices.Select("DeviceID = '" + dataRow["DeviceID"] + "'")[0]["Position"];
                          else
                            dataTable.Rows[dataTable.Rows.Count - 1]["Position"] = (object) "";
                          if (bcDevices.ObjectId != -1)
                          {
                            if (bcDevices.TableId == Guid.Empty && !objectById1.Warning)
                            {
                              if (objectById1.Sound)
                                soundPlayer.SoundLocation = Application.StartupPath + "\\List.wav";
                              dataRow["imgType"] = (object) this._imgInfo;
                            }
                            else if (bcDevices.TableId == Guid.Empty && objectById1.Warning)
                            {
                              if (objectById1.Sound)
                                soundPlayer.SoundLocation = Application.StartupPath + "\\Warning.wav";
                              dataRow["imgType"] = (object) this._imgWarning;
                            }
                            else if (bcDevices.TableId != Guid.Empty && objectById2.Warning)
                            {
                              if (objectById2.Sound)
                                soundPlayer.SoundLocation = Application.StartupPath + "\\Warning.wav";
                              dataRow["imgType"] = (object) this._imgWarning;
                            }
                            else
                            {
                              if (objectById2.Sound)
                                soundPlayer.SoundLocation = Application.StartupPath + "\\List.wav";
                              dataRow["imgType"] = (object) this._imgInfo;
                            }
                          }
                          else
                            dataRow["imgType"] = (object) this._imgInfo;
                          this.Invoke((Delegate) new FrmImages.AddNewRowsFunc(this.AddNewRow), (object) dataRow);
                          if (dataRow["Status"].ToString() != "" && !Convert.ToBoolean(dataRow["Status"]) && soundPlayer.SoundLocation != "")
                            soundPlayer.Play();
                        }
                      }
                      catch (Exception ex)
                      {
                        Console.WriteLine("Error loading data from datareader {0}, {1},{2}", (object) bcDevices.Name, (object) ex.Message, (object) ex.StackTrace);
                      }
                      dataTable.Rows.Clear();
                    }
                    Thread.Sleep(1000);
                  }
                  FrmImages.SqlServerState = true;
                }
                catch (Exception ex)
                {
                  Console.WriteLine("Error loading data from server {0}, {1},{2}", (object) bcDevices.Name, (object) ex.Message, (object) ex.StackTrace);
                  break;
                }
              }
              else
                break;
            }
          }
          catch
          {
            try
            {
              this.Invoke((Delegate) new FrmImages.RefreshStatusFunc(this.RefreshStatus), (object) Messages.IdentificationServerUnavailble, (object) 1);
            }
            catch
            {
            }
          }
        }
        else
          break;
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      Label label = this.lbTime;
      DateTime now = DateTime.Now;
      string str1 = now.ToLongTimeString();
      string str2 = "//";
      now = DateTime.Now;
      string str3 = now.ToLongDateString();
      string str4 = str1 + str2 + str3;
      label.Text = str4;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmImages));
      this.splitContainerControl1 = new SplitContainerControl();
      this.splitContainerControl2 = new SplitContainerControl();
      this.gbCamera1 = new GroupControl();
      this.splitterControl2 = new SplitterControl();
      this.pbImage1 = new PictureBox();
      this.gbCamera3 = new GroupControl();
      this.pbImage3 = new PictureBox();
      this.splitContainerControl3 = new SplitContainerControl();
      this.gbCamera2 = new GroupControl();
      this.pbImage2 = new PictureBox();
      this.gbCamera4 = new GroupControl();
      this.pbImage4 = new PictureBox();
      this.barManager1 = new BarManager();
      this.bar2 = new Bar();
      this.barLargeButtonItem1 = new BarLargeButtonItem();
      this.barButtonItem1 = new BarButtonItem();
      this.barDockControlTop = new BarDockControl();
      this.barDockControlBottom = new BarDockControl();
      this.barDockControlLeft = new BarDockControl();
      this.barDockControlRight = new BarDockControl();
      this.serviceController1 = new ServiceController();
      this.lbServiceState = new Label();
      this.lbDBServer = new Label();
      this.lbTime = new Label();
      this.timer1 = new System.Windows.Forms.Timer();
      this.label1 = new Label();
      this.lbFaceCount = new Label();
      this.splitContainerControl1.BeginInit();
      this.splitContainerControl1.SuspendLayout();
      this.splitContainerControl2.BeginInit();
      this.splitContainerControl2.SuspendLayout();
      this.gbCamera1.BeginInit();
      this.gbCamera1.SuspendLayout();
      ((ISupportInitialize) this.pbImage1).BeginInit();
      this.gbCamera3.BeginInit();
      this.gbCamera3.SuspendLayout();
      ((ISupportInitialize) this.pbImage3).BeginInit();
      this.splitContainerControl3.BeginInit();
      this.splitContainerControl3.SuspendLayout();
      this.gbCamera2.BeginInit();
      this.gbCamera2.SuspendLayout();
      ((ISupportInitialize) this.pbImage2).BeginInit();
      this.gbCamera4.BeginInit();
      this.gbCamera4.SuspendLayout();
      ((ISupportInitialize) this.pbImage4).BeginInit();
      this.barManager1.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.splitContainerControl1, "splitContainerControl1");
      this.splitContainerControl1.Name = "splitContainerControl1";
      this.splitContainerControl1.Panel1.Controls.Add((Control) this.splitContainerControl2);
      componentResourceManager.ApplyResources((object) this.splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
      this.splitContainerControl1.Panel2.Controls.Add((Control) this.splitContainerControl3);
      componentResourceManager.ApplyResources((object) this.splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
      this.splitContainerControl1.SplitterPosition = 636;
      componentResourceManager.ApplyResources((object) this.splitContainerControl2, "splitContainerControl2");
      this.splitContainerControl2.Horizontal = false;
      this.splitContainerControl2.Name = "splitContainerControl2";
      this.splitContainerControl2.Panel1.Controls.Add((Control) this.gbCamera1);
      componentResourceManager.ApplyResources((object) this.splitContainerControl2.Panel1, "splitContainerControl2.Panel1");
      this.splitContainerControl2.Panel2.Controls.Add((Control) this.gbCamera3);
      componentResourceManager.ApplyResources((object) this.splitContainerControl2.Panel2, "splitContainerControl2.Panel2");
      this.splitContainerControl2.SplitterPosition = 366;
      this.splitContainerControl2.SplitterPositionChanged += new EventHandler(this.splitContainerControl2_SplitterPositionChanged);
      this.gbCamera1.Appearance.Font = (Font) componentResourceManager.GetObject("gbCamera1.Appearance.Font");
      this.gbCamera1.Appearance.Options.UseFont = true;
      this.gbCamera1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("gbCamera1.AppearanceCaption.Font");
      this.gbCamera1.AppearanceCaption.Options.UseFont = true;
      this.gbCamera1.Controls.Add((Control) this.splitterControl2);
      this.gbCamera1.Controls.Add((Control) this.pbImage1);
      componentResourceManager.ApplyResources((object) this.gbCamera1, "gbCamera1");
      this.gbCamera1.Name = "gbCamera1";
      componentResourceManager.ApplyResources((object) this.splitterControl2, "splitterControl2");
      this.splitterControl2.Name = "splitterControl2";
      this.splitterControl2.TabStop = false;
      componentResourceManager.ApplyResources((object) this.pbImage1, "pbImage1");
      this.pbImage1.Name = "pbImage1";
      this.pbImage1.TabStop = false;
      this.gbCamera3.Appearance.Font = (Font) componentResourceManager.GetObject("gbCamera3.Appearance.Font");
      this.gbCamera3.Appearance.Options.UseFont = true;
      this.gbCamera3.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("gbCamera3.AppearanceCaption.Font");
      this.gbCamera3.AppearanceCaption.Options.UseFont = true;
      this.gbCamera3.Controls.Add((Control) this.pbImage3);
      componentResourceManager.ApplyResources((object) this.gbCamera3, "gbCamera3");
      this.gbCamera3.Name = "gbCamera3";
      componentResourceManager.ApplyResources((object) this.pbImage3, "pbImage3");
      this.pbImage3.Name = "pbImage3";
      this.pbImage3.TabStop = false;
      componentResourceManager.ApplyResources((object) this.splitContainerControl3, "splitContainerControl3");
      this.splitContainerControl3.Horizontal = false;
      this.splitContainerControl3.Name = "splitContainerControl3";
      this.splitContainerControl3.Panel1.Controls.Add((Control) this.gbCamera2);
      componentResourceManager.ApplyResources((object) this.splitContainerControl3.Panel1, "splitContainerControl3.Panel1");
      this.splitContainerControl3.Panel2.Controls.Add((Control) this.gbCamera4);
      componentResourceManager.ApplyResources((object) this.splitContainerControl3.Panel2, "splitContainerControl3.Panel2");
      this.splitContainerControl3.SplitterPosition = 366;
      this.splitContainerControl3.SplitterPositionChanged += new EventHandler(this.splitContainerControl3_SplitterPositionChanged);
      this.gbCamera2.Appearance.Font = (Font) componentResourceManager.GetObject("gbCamera2.Appearance.Font");
      this.gbCamera2.Appearance.Options.UseFont = true;
      this.gbCamera2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("gbCamera2.AppearanceCaption.Font");
      this.gbCamera2.AppearanceCaption.Options.UseFont = true;
      this.gbCamera2.Controls.Add((Control) this.pbImage2);
      componentResourceManager.ApplyResources((object) this.gbCamera2, "gbCamera2");
      this.gbCamera2.Name = "gbCamera2";
      componentResourceManager.ApplyResources((object) this.pbImage2, "pbImage2");
      this.pbImage2.Name = "pbImage2";
      this.pbImage2.TabStop = false;
      this.gbCamera4.Appearance.Font = (Font) componentResourceManager.GetObject("gbCamera4.Appearance.Font");
      this.gbCamera4.Appearance.Options.UseFont = true;
      this.gbCamera4.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("gbCamera4.AppearanceCaption.Font");
      this.gbCamera4.AppearanceCaption.Options.UseFont = true;
      this.gbCamera4.Controls.Add((Control) this.pbImage4);
      componentResourceManager.ApplyResources((object) this.gbCamera4, "gbCamera4");
      this.gbCamera4.Name = "gbCamera4";
      componentResourceManager.ApplyResources((object) this.pbImage4, "pbImage4");
      this.pbImage4.Name = "pbImage4";
      this.pbImage4.TabStop = false;
      this.barManager1.AllowShowToolbarsPopup = false;
      this.barManager1.Bars.AddRange(new Bar[1]
      {
        this.bar2
      });
      this.barManager1.CloseButtonAffectAllTabs = false;
      this.barManager1.DockControls.Add(this.barDockControlTop);
      this.barManager1.DockControls.Add(this.barDockControlBottom);
      this.barManager1.DockControls.Add(this.barDockControlLeft);
      this.barManager1.DockControls.Add(this.barDockControlRight);
      this.barManager1.Form = (Control) this;
      this.barManager1.Items.AddRange(new BarItem[2]
      {
        (BarItem) this.barLargeButtonItem1,
        (BarItem) this.barButtonItem1
      });
      this.barManager1.MainMenu = this.bar2;
      this.barManager1.MaxItemId = 3;
      this.barManager1.MdiMenuMergeStyle = BarMdiMenuMergeStyle.Never;
      this.barManager1.ShowFullMenusAfterDelay = false;
      this.barManager1.ShowScreenTipsInToolbars = false;
      this.barManager1.ShowShortcutInScreenTips = false;
      this.bar2.BarAppearance.Normal.Font = (Font) componentResourceManager.GetObject("bar2.BarAppearance.Normal.Font");
      this.bar2.BarAppearance.Normal.Options.UseFont = true;
      this.bar2.BarName = "Main menu";
      this.bar2.DockCol = 0;
      this.bar2.DockRow = 0;
      this.bar2.DockStyle = BarDockStyle.Top;
      this.bar2.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
      {
        new LinkPersistInfo((BarItem) this.barLargeButtonItem1),
        new LinkPersistInfo((BarItem) this.barButtonItem1)
      });
      this.bar2.OptionsBar.AllowQuickCustomization = false;
      this.bar2.OptionsBar.DisableClose = true;
      this.bar2.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources((object) this.bar2, "bar2");
      componentResourceManager.ApplyResources((object) this.barLargeButtonItem1, "barLargeButtonItem1");
      this.barLargeButtonItem1.Id = 0;
      this.barLargeButtonItem1.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("barLargeButtonItem1.ItemAppearance.Normal.Font");
      this.barLargeButtonItem1.ItemAppearance.Normal.Options.UseFont = true;
      this.barLargeButtonItem1.Name = "barLargeButtonItem1";
      this.barLargeButtonItem1.ItemClick += new ItemClickEventHandler(this.barLargeButtonItem1_ItemClick);
      componentResourceManager.ApplyResources((object) this.barButtonItem1, "barButtonItem1");
      this.barButtonItem1.Id = 1;
      this.barButtonItem1.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("barButtonItem1.ItemAppearance.Normal.Font");
      this.barButtonItem1.ItemAppearance.Normal.Options.UseFont = true;
      this.barButtonItem1.Name = "barButtonItem1";
      this.barButtonItem1.ItemClick += new ItemClickEventHandler(this.barButtonItem1_ItemClick);
      this.barDockControlTop.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlTop, "barDockControlTop");
      this.barDockControlBottom.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlBottom, "barDockControlBottom");
      this.barDockControlLeft.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlLeft, "barDockControlLeft");
      this.barDockControlRight.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlRight, "barDockControlRight");
      componentResourceManager.ApplyResources((object) this.lbServiceState, "lbServiceState");
      this.lbServiceState.Name = "lbServiceState";
      componentResourceManager.ApplyResources((object) this.lbDBServer, "lbDBServer");
      this.lbDBServer.Name = "lbDBServer";
      componentResourceManager.ApplyResources((object) this.lbTime, "lbTime");
      this.lbTime.Name = "lbTime";
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.lbFaceCount, "lbFaceCount");
      this.lbFaceCount.Name = "lbFaceCount";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lbFaceCount);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lbTime);
      this.Controls.Add((Control) this.lbDBServer);
      this.Controls.Add((Control) this.splitContainerControl1);
      this.Controls.Add((Control) this.lbServiceState);
      this.Controls.Add((Control) this.barDockControlLeft);
      this.Controls.Add((Control) this.barDockControlRight);
      this.Controls.Add((Control) this.barDockControlBottom);
      this.Controls.Add((Control) this.barDockControlTop);
      this.Name = "FrmImages";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.frmImages_FormClosing);
      this.Load += new EventHandler(this.frmImages_Load);
      this.splitContainerControl1.EndInit();
      this.splitContainerControl1.ResumeLayout(false);
      this.splitContainerControl2.EndInit();
      this.splitContainerControl2.ResumeLayout(false);
      this.gbCamera1.EndInit();
      this.gbCamera1.ResumeLayout(false);
      ((ISupportInitialize) this.pbImage1).EndInit();
      this.gbCamera3.EndInit();
      this.gbCamera3.ResumeLayout(false);
      ((ISupportInitialize) this.pbImage3).EndInit();
      this.splitContainerControl3.EndInit();
      this.splitContainerControl3.ResumeLayout(false);
      this.gbCamera2.EndInit();
      this.gbCamera2.ResumeLayout(false);
      ((ISupportInitialize) this.pbImage2).EndInit();
      this.gbCamera4.EndInit();
      this.gbCamera4.ResumeLayout(false);
      ((ISupportInitialize) this.pbImage4).EndInit();
      this.barManager1.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class ClientManagmentServerCallback : IManagmentServerCallback
    {
      public void CheckClient()
      {
      }

      public void SendDetectorError(Guid id)
      {
      }

      public void SendDetectorInfo(DataTable devices)
      {
      }

      public void SendExtractorError(Guid id)
      {
      }

      public void SendExtractorInfo(DataTable devices)
      {
      }

      public void SendIdentificationError(Guid id)
      {
      }

      public void SendIdentificationInfo(DataTable devices)
      {
      }

      public void SendVideoError(Guid id)
      {
      }

      public void SendResults(DataTable id)
      {
      }

      public void SendVideoInfo(DataTable devices)
      {
      }
    }

    private delegate void RefreshDevicesFunc();

    private delegate void RefreshStatusFunc(string meaasage, int val);

    private delegate void Newimagefunc(PictureBox pb, Bitmap bmp, int index, BcDevices dev);

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

    private delegate void AddNewRowsFunc(DataRow dt);
  }
}
