// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmWebCam
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using BasicComponents;
using BasicComponents.VideoServer;
using CascadeManager.Properties;
using CS.DAL;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Events;
using DevExpress.XtraLayout;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = System.Drawing.Image;
using NewFrameEventArgs = AForge.Video.NewFrameEventArgs;
using Padding = DevExpress.XtraLayout.Utils.Padding;
using Rectangle = TS.Core.Model.Rectangle;

namespace CascadeManager
{
  public class FrmWebCam : XtraForm
  {
    private List<BcDevices> _devices = BcDevicesStorageExtensions.LoadAll();
    public BcDevices MainDevice = new BcDevices();
    private DataTable _dtmain = new DataTable();
    private List<int> _detectors = new List<int>();
    private readonly List<IEngine> _faces = new List<IEngine>();
    private float _fquality = 0.5f;
    public string Cap = "";
    public string WebCamName = "";
    private IContainer components = null;
    private FilterInfoCollection _videoDevices;
    private bool _detectfaceFlag;
    public static VideoCaptureDevice VideoSource;
    public static int SelectedCap;
    public bool StopFlag;
    public Thread MainThread;
    public Bitmap ImgResult;
    private bool _allowclose;
    private string _vDevice;
    private PanelControl panelControl1;
    private SimpleButton btClear;
    private SplitContainerControl splitContainerControl1;
    private GridControl gcImages;
    private LayoutView lvImages;
    private LayoutViewColumn colImage;
    private LayoutViewField layoutViewField_layoutViewColumn1;
    private LayoutViewCard layoutViewCard1;
    private GridView gridView1;
    private SimpleButton btCancel;
    private SimpleButton btAccept;
    private SimpleButton btStop;
    private SimpleButton btPlay;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private CheckEdit chbDetector;
    private LabelControl label2;
    public ComboBoxEdit cbCapabilities;
    private ComboBoxEdit devicesCombo;
    private LabelControl label1;
    private PictureEdit pbImage;

    public FrmWebCam()
    {
      InitializeComponent();
      _dtmain = new DataTable();
      _dtmain.Columns.Add("Image", typeof (Bitmap));
      gcImages.DataSource = _dtmain;
    }

    public FrmWebCam(bool flag)
    {
      InitializeComponent();
      _dtmain = new DataTable();
      _dtmain.Columns.Add("Image", typeof (Bitmap));
      gcImages.DataSource = _dtmain;
      chbDetector.Checked = flag;
    }

    private void vScrollBar1_Click(object sender, EventArgs e)
    {
    }

    private void frmWebCam_Load(object sender, EventArgs e)
    {
      if (!chbDetector.Checked)
      {
        splitContainerControl1.SplitterPosition = 1024;
        _detectfaceFlag = false;
      }
      else
      {
        splitContainerControl1.SplitterPosition = 700;
        _detectfaceFlag = true;
      }
      string str = "";
      try
      {
        devicesCombo.Properties.Items.Clear();
        cbCapabilities.Properties.Items.Clear();
        devicesCombo.SelectedIndex = -1;
        devicesCombo.Text = "";
        cbCapabilities.SelectedIndex = -1;
        cbCapabilities.Text = "";
        StreamReader streamReader = new StreamReader(MainForm.ApplicationData + "WebCamSettings.ini");
        try
        {
          str = streamReader.ReadLine();
        }
        catch
        {
        }
        streamReader.Close();
      }
      catch
      {
      }
      try
      {
        _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        foreach (FilterInfo filterInfo in _videoDevices)
        {
          devicesCombo.Properties.Items.Add(filterInfo.Name);
          if (str != "" && filterInfo.Name == str)
            devicesCombo.SelectedIndex = devicesCombo.Properties.Items.Count - 1;
        }
        foreach (BcDevices bcDevices in _devices)
        {
          devicesCombo.Properties.Items.Add(bcDevices.Name);
          if (str == bcDevices.Name)
            devicesCombo.SelectedIndex = devicesCombo.Properties.Items.Count - 1;
        }
      }
      catch (ApplicationException ex)
      {
        devicesCombo.Properties.Items.Add("No local capture devices");
        devicesCombo.Enabled = false;
      }
    }

    private int GetFreeHandle()
    {
      for (int index = 0; index < _detectors.Count; ++index)
      {
        if (_detectors[index] == 0)
        {
          _detectors[index] = 1;
          return index;
        }
      }
      return -1;
    }

    private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
      try
      {
        if (_detectfaceFlag)
        {
          int freeHandle = GetFreeHandle();
          if (freeHandle != -1)
          {
            DetectFaceFunc detectFaceFunc = DetectFaces;
            detectFaceFunc.BeginInvoke((Bitmap) eventArgs.Frame.Clone(), freeHandle, DetectCallBack, detectFaceFunc);
          }
        }
        Invoke(new NewValueFunc(NewValue), (object) (Bitmap) eventArgs.Frame.Clone(), (object) "");
      }
      catch
      {
      }
    }

    private void NewValue(Bitmap img, string time)
    {
      if (pbImage.Image != null)
        pbImage.Image.Dispose();
      pbImage.Image = img;
    }

    private void OpenVideoSource(IVideoSource source)
    {
      if (VideoSource != null)
      {
        try
        {
          Cursor = Cursors.WaitCursor;
          VideoSource.NewFrame += videoSource_NewFrame;
          VideoSource.Stop();
          VideoSource.Start();
          StartDetection();
        }
        catch
        {
        }
        Cursor = Cursors.Default;
      }
      Cursor = Cursors.Default;
    }

    private void btPlay_Click(object sender, EventArgs e)
    {
      SelectedCap = cbCapabilities.SelectedIndex;
      if (devicesCombo.SelectedIndex > -1 && devicesCombo.SelectedIndex < _videoDevices.Count)
      {
        _vDevice = _videoDevices[devicesCombo.SelectedIndex].MonikerString;
        WebCamName = _videoDevices[devicesCombo.SelectedIndex].Name;
        Cap = cbCapabilities.Text;
        try
        {
          StreamWriter streamWriter = new StreamWriter(MainForm.ApplicationData + "WebCamSettings.ini", false);
          try
          {
            streamWriter.WriteLine(devicesCombo.Text);
            streamWriter.WriteLine(cbCapabilities.SelectedIndex);
          }
          catch
          {
          }
          streamWriter.Close();
        }
        catch
        {
        }
        try
        {
          VideoSource.Stop();
        }
        catch
        {
        }
        VideoSource = !(_vDevice != "") ? null : new VideoCaptureDevice(_vDevice);
        OpenVideoSource(VideoSource);
      }
      else
      {
        if (devicesCombo.SelectedIndex <= 0)
          return;
        OpenRemouteDevice();
      }
    }

    public void StopRemouteDevice()
    {
      StopFlag = true;
      Thread.Sleep(1000);
      if (MainThread == null)
        return;
      MainThread.Abort();
    }

    public void OpenRemouteDevice()
    {
      MainDevice = _devices[devicesCombo.SelectedIndex - _videoDevices.Count];
      StopFlag = true;
      if (MainThread != null)
        MainThread.Abort();
      StartDetection();
      MainThread = new Thread(WorkerThread)
      {
        IsBackground = true
      };
      StopFlag = false;
      MainThread.Start();
    }

    public void WorkerThread()
    {
      while (true)
      {
        bool flag = true;
        if (!StopFlag)
        {
          try
          {
            BcVideoServer bcVideoServer = BcVideoServer.LoadById(MainDevice.Vsid);
            if (bcVideoServer.Id != Guid.Empty)
            {
              int num = 0;
              VideoServerClient videoServerClient = new VideoServerClient();
              videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
              videoServerClient.Open();
              while (true)
              {
                flag = true;
                ++num;
                if (!StopFlag)
                {
                  byte[] imageFromDevice = videoServerClient.GetImageFromDevice(MainDevice.Id);
                  if (imageFromDevice != null && imageFromDevice.Length > 0)
                  {
                    MemoryStream memoryStream = new MemoryStream(imageFromDevice);
                    Bitmap bitmap = new Bitmap(memoryStream);
                    try
                    {
                      if (_detectfaceFlag)
                      {
                        int freeHandle = GetFreeHandle();
                        if (freeHandle != -1)
                        {
                          Bitmap img = new Bitmap(bitmap);
                          DetectFaceFunc detectFaceFunc = DetectFaces;
                          detectFaceFunc.BeginInvoke(img, freeHandle, DetectCallBack, detectFaceFunc);
                        }
                      }
                    }
                    catch
                    {
                    }
                    Invoke(new NewValueFunc(NewValue), (object) new Bitmap(bitmap), (object) "");
                    bitmap.Dispose();
                    memoryStream.Close();
                    memoryStream.Dispose();
                  }
                  Thread.Sleep(20);
                }
                else
                  break;
              }
            }
          }
          catch
          {
          }
          Thread.Sleep(1000);
        }
        else
          break;
      }
    }

    private void btStop_Click(object sender, EventArgs e)
    {
      if (devicesCombo.SelectedIndex >= 0 && devicesCombo.SelectedIndex < _videoDevices.Count)
        VideoSource.Stop();
      else
        StopFlag = true;
    }

    private void btOpen_Click(object sender, EventArgs e)
    {
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      _allowclose = true;
      if (lvImages.SelectedRowsCount > 0 && _detectfaceFlag)
      {
        try
        {
          ImgResult = (Bitmap) ((Image) lvImages.GetDataRow(lvImages.GetSelectedRows()[0])[0]).Clone();
          Close();
        }
        catch (Exception ex)
        {
          int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else if (!_detectfaceFlag && pbImage.Image != null)
      {
        ImgResult = (Bitmap) pbImage.Image.Clone();
        pbImage.Image.Dispose();
      }
      else
      {
        int num = (int) XtraMessageBox.Show(Messages.SelectRecord, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        _allowclose = false;
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      _allowclose = true;
    }

    private void btClear_Click(object sender, EventArgs e)
    {
      foreach (DataRow dataRow in (InternalDataCollectionBase) _dtmain.Rows)
        ((Image) dataRow[0]).Dispose();
      _dtmain.Rows.Clear();
    }

    private void AddImage(Bitmap img)
    {
      try
      {
        _dtmain.Rows.Add((object) (Bitmap) img.Clone());
      }
      catch
      {
      }
    }

    private void DetectCallBack(IAsyncResult iar)
    {
      ((DetectFaceFunc) iar.AsyncState).EndInvoke(iar);
    }

    private void ReleasDetectors()
    {
      for (int index = 0; index < _faces.Count; ++index)
        _faces[index].Dispose();
      _faces.Clear();
    }

    private void StartDetection()
    {
      try
      {
        int processorCount = Environment.ProcessorCount;
        if (processorCount > 1)
          --processorCount;
        ReleasDetectors();
        _detectors = new List<int>();
        for (int index = 0; index < processorCount; ++index)
        {
          _detectors.Add(0);
          _faces.Add(new Engine());
        }
      }
      catch
      {
      }
    }

    private void DetectFaces(Bitmap img, int index)
    {
      try
      {
        FaceInfo[] faceInfoArray = _faces[index].DetectAllFaces(img.ConvertFrom(), null);
        if (faceInfoArray.Length > 0)
        {
          for (int index1 = 0; index1 < faceInfoArray.Length; ++index1)
          {
            double num1 = Math.Abs(faceInfoArray[index1].PitchAngle);
            double num2 = Math.Abs(faceInfoArray[index1].YawAngle);
            if (faceInfoArray[index1].DetectionProbability >= _fquality && num1 <= 0.600000023841858 && num2 <= 0.800000011920929)
            {
              Rectangle faceRectangle = faceInfoArray[index1].FaceRectangle;
              System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle
              {
                X = (int) (faceRectangle.X - 0.4 * faceRectangle.Width),
                Y = (int) (faceRectangle.Y - 0.4 * faceRectangle.Height),
                Height = (int) (faceRectangle.Height + 0.8 * faceRectangle.Height),
                Width = (int) (faceRectangle.Width + 0.8 * faceRectangle.Width)
              };
              if (srcRect.Y < 0)
                srcRect.Y = 0;
              if (srcRect.X + srcRect.Width > img.Width)
                srcRect.Width = img.Width - srcRect.X;
              if (faceRectangle.Y + srcRect.Height > img.Height)
                srcRect.Height = img.Height - srcRect.Y;
              using (Bitmap bitmap = new Bitmap(srcRect.Width, srcRect.Height))
              {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                  try
                  {
                    graphics.DrawImage(img, new System.Drawing.Rectangle(0, 0, srcRect.Width, srcRect.Height), srcRect, GraphicsUnit.Pixel);
                    Invoke(new AddImageFunc(AddImage), (object) bitmap);
                  }
                  catch
                  {
                  }
                }
              }
            }
          }
        }
        img.Dispose();
        try
        {
          _detectors[index] = 0;
        }
        catch
        {
        }
      }
      catch
      {
        try
        {
          _detectors[index] = 0;
        }
        catch
        {
        }
        try
        {
          img.Dispose();
        }
        catch
        {
        }
      }
    }

    private void frmWebCam_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.None)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) _dtmain.Rows)
          ((Image) dataRow[0]).Dispose();
        _dtmain.Rows.Clear();
        ReleasDetectors();
        try
        {
          StopFlag = true;
          VideoSource.Stop();
        }
        catch
        {
        }
      }
      else if (_allowclose)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) _dtmain.Rows)
          ((Image) dataRow[0]).Dispose();
        _dtmain.Rows.Clear();
        ReleasDetectors();
        try
        {
          StopFlag = true;
          VideoSource.Stop();
        }
        catch
        {
        }
      }
      else
        e.Cancel = true;
    }

    private void lvImages_CustomDrawCardCaption(object sender, LayoutViewCustomDrawCardCaptionEventArgs e)
    {
      e.CardCaption = (e.RowHandle + 1).ToString();
    }

    private void chbDetector_CheckedChanged(object sender, EventArgs e)
    {
      if (!chbDetector.Checked)
      {
        splitContainerControl1.SplitterPosition = 1024;
        _detectfaceFlag = false;
      }
      else
      {
        splitContainerControl1.SplitterPosition = 700;
        _detectfaceFlag = true;
      }
    }

    private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (devicesCombo.SelectedIndex != -1 && devicesCombo.SelectedIndex < _videoDevices.Count)
      {
        StopFlag = true;
        cbCapabilities.Enabled = true;
        cbCapabilities.Properties.Items.Clear();
        if (_videoDevices.Count > 0)
        {
          cbCapabilities.Enabled = true;
          VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice(_videoDevices[devicesCombo.SelectedIndex].MonikerString);
          if (videoCaptureDevice.VideoCapabilities != null)
          {
            foreach (VideoCapabilities videoCapabilities in videoCaptureDevice.VideoCapabilities)
              cbCapabilities.Properties.Items.Add(string.Format("{0}x{1}", videoCapabilities.FrameSize.Width, videoCapabilities.FrameSize.Height));
            if (SelectedCap != -1)
              cbCapabilities.SelectedIndex = SelectedCap;
            else if (cbCapabilities.Properties.Items.Count > 0)
              cbCapabilities.SelectedIndex = 0;
          }
        }
        SelectedCap = cbCapabilities.SelectedIndex;
        _vDevice = _videoDevices[devicesCombo.SelectedIndex].MonikerString;
        WebCamName = _videoDevices[devicesCombo.SelectedIndex].Name;
        Cap = cbCapabilities.Text;
        try
        {
          StreamWriter streamWriter = new StreamWriter(MainForm.ApplicationData + "WebCamSettings.ini", false);
          try
          {
            streamWriter.WriteLine(devicesCombo.Text);
            streamWriter.WriteLine(cbCapabilities.SelectedIndex);
          }
          catch
          {
          }
          streamWriter.Close();
        }
        catch
        {
        }
        try
        {
          VideoSource.Stop();
        }
        catch
        {
        }
        VideoSource = !(_vDevice != "") ? null : new VideoCaptureDevice(_vDevice);
        OpenVideoSource(VideoSource);
      }
      else
      {
        if (devicesCombo.SelectedIndex < 0)
          return;
        try
        {
          VideoSource.Stop();
        }
        catch
        {
        }
        cbCapabilities.Enabled = false;
        OpenRemouteDevice();
      }
    }

    private void cbCapabilities_SelectedIndexChanged(object sender, EventArgs e)
    {
      SelectedCap = cbCapabilities.SelectedIndex;
      _vDevice = _videoDevices[devicesCombo.SelectedIndex].MonikerString;
      WebCamName = _videoDevices[devicesCombo.SelectedIndex].Name;
      Cap = cbCapabilities.Text;
      try
      {
        StreamWriter streamWriter = new StreamWriter(MainForm.ApplicationData + "WebCamSettings.ini", false);
        try
        {
          streamWriter.WriteLine(devicesCombo.Text);
          streamWriter.WriteLine(cbCapabilities.SelectedIndex);
        }
        catch
        {
        }
        streamWriter.Close();
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmWebCam));
      GridLevelNode gridLevelNode = new GridLevelNode();
      splitContainerControl1 = new SplitContainerControl();
      panelControl1 = new PanelControl();
      pbImage = new PictureEdit();
      label2 = new LabelControl();
      cbCapabilities = new ComboBoxEdit();
      devicesCombo = new ComboBoxEdit();
      label1 = new LabelControl();
      gcImages = new GridControl();
      lvImages = new LayoutView();
      colImage = new LayoutViewColumn();
      repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      layoutViewField_layoutViewColumn1 = new LayoutViewField();
      layoutViewCard1 = new LayoutViewCard();
      gridView1 = new GridView();
      btClear = new SimpleButton();
      btCancel = new SimpleButton();
      btAccept = new SimpleButton();
      btStop = new SimpleButton();
      btPlay = new SimpleButton();
      chbDetector = new CheckEdit();
      splitContainerControl1.BeginInit();
      splitContainerControl1.SuspendLayout();
      panelControl1.BeginInit();
      panelControl1.SuspendLayout();
      pbImage.Properties.BeginInit();
      cbCapabilities.Properties.BeginInit();
      devicesCombo.Properties.BeginInit();
      gcImages.BeginInit();
      lvImages.BeginInit();
      repositoryItemPictureEdit1.BeginInit();
      layoutViewField_layoutViewColumn1.BeginInit();
      layoutViewCard1.BeginInit();
      gridView1.BeginInit();
      chbDetector.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(splitContainerControl1, "splitContainerControl1");
      splitContainerControl1.Name = "splitContainerControl1";
      componentResourceManager.ApplyResources(splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
      splitContainerControl1.Panel1.Controls.Add(panelControl1);
      componentResourceManager.ApplyResources(splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
      splitContainerControl1.Panel2.Controls.Add(gcImages);
      splitContainerControl1.Panel2.ShowCaption = true;
      splitContainerControl1.SplitterPosition = 739;
      componentResourceManager.ApplyResources(panelControl1, "panelControl1");
      panelControl1.ContentImageAlignment = ContentAlignment.MiddleRight;
      panelControl1.Controls.Add(pbImage);
      panelControl1.Controls.Add(label2);
      panelControl1.Controls.Add(cbCapabilities);
      panelControl1.Controls.Add(devicesCombo);
      panelControl1.Controls.Add(label1);
      panelControl1.Name = "panelControl1";
      componentResourceManager.ApplyResources(pbImage, "pbImage");
      pbImage.Name = "pbImage";
      pbImage.Properties.AccessibleDescription = componentResourceManager.GetString("pbImage.Properties.AccessibleDescription");
      pbImage.Properties.AccessibleName = componentResourceManager.GetString("pbImage.Properties.AccessibleName");
      pbImage.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(label2, "label2");
      label2.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label2.Appearance.DisabledImage");
      label2.Appearance.Font = (Font) componentResourceManager.GetObject("label2.Appearance.Font");
      label2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label2.Appearance.FontSizeDelta");
      label2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label2.Appearance.FontStyleDelta");
      label2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label2.Appearance.GradientMode");
      label2.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label2.Appearance.HoverImage");
      label2.Appearance.Image = (Image) componentResourceManager.GetObject("label2.Appearance.Image");
      label2.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label2.Appearance.PressedImage");
      label2.Name = "label2";
      componentResourceManager.ApplyResources(cbCapabilities, "cbCapabilities");
      cbCapabilities.Name = "cbCapabilities";
      cbCapabilities.Properties.AccessibleDescription = componentResourceManager.GetString("cbCapabilities.Properties.AccessibleDescription");
      cbCapabilities.Properties.AccessibleName = componentResourceManager.GetString("cbCapabilities.Properties.AccessibleName");
      cbCapabilities.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbCapabilities.Properties.Appearance.Font");
      cbCapabilities.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbCapabilities.Properties.Appearance.FontSizeDelta");
      cbCapabilities.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbCapabilities.Properties.Appearance.FontStyleDelta");
      cbCapabilities.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbCapabilities.Properties.Appearance.GradientMode");
      cbCapabilities.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbCapabilities.Properties.Appearance.Image");
      cbCapabilities.Properties.Appearance.Options.UseFont = true;
      cbCapabilities.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbCapabilities.Properties.AutoHeight");
      cbCapabilities.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbCapabilities.Properties.Buttons"))
      });
      cbCapabilities.Properties.NullValuePrompt = componentResourceManager.GetString("cbCapabilities.Properties.NullValuePrompt");
      cbCapabilities.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbCapabilities.Properties.NullValuePromptShowForEmptyValue");
      cbCapabilities.SelectedIndexChanged += cbCapabilities_SelectedIndexChanged;
      componentResourceManager.ApplyResources(devicesCombo, "devicesCombo");
      devicesCombo.Name = "devicesCombo";
      devicesCombo.Properties.AccessibleDescription = componentResourceManager.GetString("devicesCombo.Properties.AccessibleDescription");
      devicesCombo.Properties.AccessibleName = componentResourceManager.GetString("devicesCombo.Properties.AccessibleName");
      devicesCombo.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("devicesCombo.Properties.Appearance.Font");
      devicesCombo.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("devicesCombo.Properties.Appearance.FontSizeDelta");
      devicesCombo.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("devicesCombo.Properties.Appearance.FontStyleDelta");
      devicesCombo.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("devicesCombo.Properties.Appearance.GradientMode");
      devicesCombo.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("devicesCombo.Properties.Appearance.Image");
      devicesCombo.Properties.Appearance.Options.UseFont = true;
      devicesCombo.Properties.AutoHeight = (bool) componentResourceManager.GetObject("devicesCombo.Properties.AutoHeight");
      devicesCombo.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("devicesCombo.Properties.Buttons"))
      });
      devicesCombo.Properties.NullValuePrompt = componentResourceManager.GetString("devicesCombo.Properties.NullValuePrompt");
      devicesCombo.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("devicesCombo.Properties.NullValuePromptShowForEmptyValue");
      devicesCombo.SelectedIndexChanged += devicesCombo_SelectedIndexChanged;
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label1.Appearance.DisabledImage");
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      label1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label1.Appearance.FontSizeDelta");
      label1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label1.Appearance.FontStyleDelta");
      label1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label1.Appearance.GradientMode");
      label1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label1.Appearance.HoverImage");
      label1.Appearance.Image = (Image) componentResourceManager.GetObject("label1.Appearance.Image");
      label1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label1.Appearance.PressedImage");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(gcImages, "gcImages");
      gcImages.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcImages.EmbeddedNavigator.AccessibleDescription");
      gcImages.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcImages.EmbeddedNavigator.AccessibleName");
      gcImages.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.AllowHtmlTextInToolTip");
      gcImages.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.Anchor");
      gcImages.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.BackgroundImage");
      gcImages.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.BackgroundImageLayout");
      gcImages.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.ImeMode");
      gcImages.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.MaximumSize");
      gcImages.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.TextLocation");
      gcImages.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcImages.EmbeddedNavigator.ToolTip");
      gcImages.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gcImages.EmbeddedNavigator.ToolTipIconType");
      gcImages.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcImages.EmbeddedNavigator.ToolTipTitle");
      gridLevelNode.RelationName = "Level1";
      gcImages.LevelTree.Nodes.AddRange(new GridLevelNode[1]
      {
        gridLevelNode
      });
      gcImages.MainView = lvImages;
      gcImages.Name = "gcImages";
      gcImages.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        repositoryItemPictureEdit1
      });
      gcImages.ViewCollection.AddRange(new BaseView[2]
      {
        lvImages,
        gridView1
      });
      componentResourceManager.ApplyResources(lvImages, "lvImages");
      lvImages.CardHorzInterval = 0;
      lvImages.CardMinSize = new Size(150, 150);
      lvImages.CardVertInterval = 0;
      lvImages.Columns.AddRange(new LayoutViewColumn[1]
      {
        colImage
      });
      lvImages.GridControl = gcImages;
      lvImages.Name = "lvImages";
      lvImages.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      lvImages.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
      lvImages.OptionsBehavior.AllowExpandCollapse = false;
      lvImages.OptionsBehavior.AutoPopulateColumns = false;
      lvImages.OptionsBehavior.AutoSelectAllInEditor = false;
      lvImages.OptionsCustomization.AllowFilter = false;
      lvImages.OptionsCustomization.AllowSort = false;
      lvImages.OptionsItemText.AlignMode = FieldTextAlignMode.CustomSize;
      lvImages.OptionsItemText.TextToControlDistance = 0;
      lvImages.OptionsLayout.Columns.AddNewColumns = false;
      lvImages.OptionsLayout.Columns.RemoveOldColumns = false;
      lvImages.OptionsLayout.Columns.StoreLayout = false;
      lvImages.OptionsLayout.StoreDataSettings = false;
      lvImages.OptionsLayout.StoreVisualOptions = false;
      lvImages.OptionsSelection.MultiSelect = true;
      lvImages.OptionsView.AllowHotTrackFields = false;
      lvImages.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      lvImages.OptionsView.ShowCardExpandButton = false;
      lvImages.OptionsView.ShowCardFieldBorders = true;
      lvImages.OptionsView.ShowCardLines = false;
      lvImages.OptionsView.ShowFieldHints = false;
      lvImages.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
      lvImages.OptionsView.ShowHeaderPanel = false;
      lvImages.OptionsView.ViewMode = LayoutViewMode.MultiColumn;
      lvImages.TemplateCard = layoutViewCard1;
      lvImages.CustomDrawCardCaption += lvImages_CustomDrawCardCaption;
      colImage.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceCell.Font");
      colImage.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceCell.FontSizeDelta");
      colImage.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceCell.FontStyleDelta");
      colImage.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceCell.GradientMode");
      colImage.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceCell.Image");
      colImage.AppearanceCell.Options.UseFont = true;
      colImage.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
      colImage.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceHeader.FontSizeDelta");
      colImage.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceHeader.FontStyleDelta");
      colImage.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceHeader.GradientMode");
      colImage.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceHeader.Image");
      colImage.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colImage, "colImage");
      colImage.ColumnEdit = repositoryItemPictureEdit1;
      colImage.FieldName = "Image";
      colImage.LayoutViewField = layoutViewField_layoutViewColumn1;
      colImage.Name = "colImage";
      colImage.OptionsColumn.AllowEdit = false;
      colImage.OptionsColumn.AllowMove = false;
      colImage.OptionsColumn.AllowShowHide = false;
      colImage.OptionsColumn.AllowSize = false;
      colImage.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
      repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.Image;
      repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
      layoutViewField_layoutViewColumn1.EditorPreferredWidth = 122;
      layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
      layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
      layoutViewField_layoutViewColumn1.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn1.Size = new Size(129, 16);
      layoutViewField_layoutViewColumn1.TextSize = new Size(7, 13);
      componentResourceManager.ApplyResources(layoutViewCard1, "layoutViewCard1");
      layoutViewCard1.ExpandButtonLocation = GroupElementLocation.AfterText;
      layoutViewCard1.Items.AddRange(new BaseLayoutItem[1]
      {
        layoutViewField_layoutViewColumn1
      });
      layoutViewCard1.Name = "layoutViewTemplateCard";
      layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
      layoutViewCard1.Padding = new Padding(0, 0, 0, 0);
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.GridControl = gcImages;
      gridView1.Name = "gridView1";
      componentResourceManager.ApplyResources(btClear, "btClear");
      btClear.Appearance.Font = (Font) componentResourceManager.GetObject("btClear.Appearance.Font");
      btClear.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btClear.Appearance.FontSizeDelta");
      btClear.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btClear.Appearance.FontStyleDelta");
      btClear.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btClear.Appearance.GradientMode");
      btClear.Appearance.Image = (Image) componentResourceManager.GetObject("btClear.Appearance.Image");
      btClear.Appearance.Options.UseFont = true;
      btClear.Name = "btClear";
      btClear.Click += btClear_Click;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      btCancel.Appearance.Options.UseFont = true;
      btCancel.DialogResult = DialogResult.Cancel;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(btAccept, "btAccept");
      btAccept.Appearance.Font = (Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      btAccept.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
      btAccept.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
      btAccept.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
      btAccept.Appearance.Image = (Image) componentResourceManager.GetObject("btAccept.Appearance.Image");
      btAccept.Appearance.Options.UseFont = true;
      btAccept.DialogResult = DialogResult.OK;
      btAccept.Name = "btAccept";
      btAccept.Click += btAccept_Click;
      componentResourceManager.ApplyResources(btStop, "btStop");
      btStop.Appearance.Font = (Font) componentResourceManager.GetObject("btStop.Appearance.Font");
      btStop.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btStop.Appearance.FontSizeDelta");
      btStop.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btStop.Appearance.FontStyleDelta");
      btStop.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btStop.Appearance.GradientMode");
      btStop.Appearance.Image = (Image) componentResourceManager.GetObject("btStop.Appearance.Image");
      btStop.Appearance.Options.UseFont = true;
      btStop.Image = Resources.stop36;
      btStop.Name = "btStop";
      btStop.Click += btStop_Click;
      componentResourceManager.ApplyResources(btPlay, "btPlay");
      btPlay.Appearance.Font = (Font) componentResourceManager.GetObject("btPlay.Appearance.Font");
      btPlay.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPlay.Appearance.FontSizeDelta");
      btPlay.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPlay.Appearance.FontStyleDelta");
      btPlay.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPlay.Appearance.GradientMode");
      btPlay.Appearance.Image = (Image) componentResourceManager.GetObject("btPlay.Appearance.Image");
      btPlay.Appearance.Options.UseFont = true;
      btPlay.Image = Resources.play36;
      btPlay.Name = "btPlay";
      btPlay.Click += btPlay_Click;
      componentResourceManager.ApplyResources(chbDetector, "chbDetector");
      chbDetector.Name = "chbDetector";
      chbDetector.Properties.AccessibleDescription = componentResourceManager.GetString("chbDetector.Properties.AccessibleDescription");
      chbDetector.Properties.AccessibleName = componentResourceManager.GetString("chbDetector.Properties.AccessibleName");
      chbDetector.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbDetector.Properties.Appearance.Font");
      chbDetector.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("chbDetector.Properties.Appearance.FontSizeDelta");
      chbDetector.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("chbDetector.Properties.Appearance.FontStyleDelta");
      chbDetector.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("chbDetector.Properties.Appearance.GradientMode");
      chbDetector.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("chbDetector.Properties.Appearance.Image");
      chbDetector.Properties.Appearance.Options.UseFont = true;
      chbDetector.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbDetector.Properties.AutoHeight");
      chbDetector.Properties.Caption = componentResourceManager.GetString("chbDetector.Properties.Caption");
      chbDetector.Properties.DisplayValueChecked = componentResourceManager.GetString("chbDetector.Properties.DisplayValueChecked");
      chbDetector.Properties.DisplayValueGrayed = componentResourceManager.GetString("chbDetector.Properties.DisplayValueGrayed");
      chbDetector.Properties.DisplayValueUnchecked = componentResourceManager.GetString("chbDetector.Properties.DisplayValueUnchecked");
      chbDetector.CheckedChanged += chbDetector_CheckedChanged;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(chbDetector);
      Controls.Add(btClear);
      Controls.Add(splitContainerControl1);
      Controls.Add(btCancel);
      Controls.Add(btAccept);
      Controls.Add(btStop);
      Controls.Add(btPlay);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmWebCam";
      FormClosing += frmWebCam_FormClosing;
      Load += frmWebCam_Load;
      splitContainerControl1.EndInit();
      splitContainerControl1.ResumeLayout(false);
      panelControl1.EndInit();
      panelControl1.ResumeLayout(false);
      panelControl1.PerformLayout();
      pbImage.Properties.EndInit();
      cbCapabilities.Properties.EndInit();
      devicesCombo.Properties.EndInit();
      gcImages.EndInit();
      lvImages.EndInit();
      repositoryItemPictureEdit1.EndInit();
      layoutViewField_layoutViewColumn1.EndInit();
      layoutViewCard1.EndInit();
      gridView1.EndInit();
      chbDetector.Properties.EndInit();
      ResumeLayout(false);
    }

    private delegate void NewValueFunc(Bitmap img, string time);

    private delegate void DetectFaceFunc(Bitmap img, int index);

    private delegate void AddImageFunc(Bitmap img);
  }
}
