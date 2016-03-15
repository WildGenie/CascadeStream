// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmVideo
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using BasicComponents.BasicComponents.Video;
using CascadeManager.Properties;
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
using Padding = DevExpress.XtraLayout.Utils.Padding;
using Rectangle = TS.Core.Model.Rectangle;
using Timer = System.Windows.Forms.Timer;

namespace CascadeManager
{
  public class FrmVideo : XtraForm
  {
    private float _fquality = 0.7f;
    private DataTable _dtmain = new DataTable();
    private string _filename = "";
    private List<int> _detectors = new List<int>();
    private List<IEngine> _faces = new List<IEngine>();
    private IContainer components = null;
    private SeekableFileSource _device;
    public Timer TimerPos;
    private bool _nextPos;
    public Bitmap ImgResult;
    private SimpleButton btPlay;
    private SimpleButton btPause;
    private SimpleButton btStop;
    private SimpleButton btOpen;
    private SimpleButton btCancel;
    private SimpleButton btAccept;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private SplitContainerControl splitContainerControl1;
    private TrackBarControl trackBarControl1;
    private LabelControl labelControl1;
    private GridControl gcImages;
    private LayoutView lvImages;
    private LayoutViewColumn colImage;
    private GridView gridView1;
    private PictureBox pbImage;
    private LayoutViewField layoutViewField_layoutViewColumn1;
    private LayoutViewCard layoutViewCard1;
    private SimpleButton btClear;

    public FrmVideo()
    {
      InitializeComponent();
      _dtmain.Columns.Add("Image", typeof (Bitmap));
      gcImages.DataSource = _dtmain;
      lvImages.CustomCardLayout += lvImages_CustomCardLayout;
    }

    private void lvImages_CustomCardLayout(object sender, LayoutViewCustomCardLayoutEventArgs e)
    {
    }

    private string GetTime(double val)
    {
      return GetHours(val) + ":" + GetMinuts(val) + ":" + GetSeconds(val);
    }

    private string GetHours(double val)
    {
      int num = (int) val / 3600;
      if (num >= 10)
        return num.ToString();
      if (num > 0)
        return "0" + num;
      return "00";
    }

    private string GetMinuts(double val)
    {
      int num1 = (int) val % 3600;
      if (num1 <= 0)
        return "00";
      int num2 = num1 / 60;
      if (num2 >= 10)
        return num2.ToString();
      if (num2 > 0)
        return "0" + num2;
      return "00";
    }

    private string GetSeconds(double val)
    {
      int num1 = (int) val % 3600;
      if (num1 <= 0)
        return "00";
      int num2 = num1 % 60;
      if (num2 <= 0)
        return "00";
      if (num2 >= 10)
        return num2.ToString();
      if (num2 > 0)
        return "0" + num2;
      return "00";
    }

    private void AddImage(Bitmap img)
    {
      _dtmain.Rows.Add((object) (Bitmap) img.Clone());
    }

    private void DetectFaces(Bitmap img, int index)
    {
      try
      {
        FaceInfo[] faceInfoArray = _faces[index].DetectAllFaces(img.ConvertFrom(), null);
        if (faceInfoArray.Length > 0)
        {
          try
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
          catch
          {
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

    private void DetectCallBack(IAsyncResult iar)
    {
      ((Detect) iar.AsyncState).EndInvoke(iar);
    }

    private void btPlay_Click(object sender, EventArgs e)
    {
      if (_device != null && !_device.IsRunning)
      {
        try
        {
          _device.Start();
          StartDetection();
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          _device.Stop();
        }
        catch
        {
        }
        try
        {
          if (_filename == "")
          {
            btOpen_Click(new object(), new EventArgs());
          }
          else
          {
            _device = new SeekableFileSource(_filename);
            _device.NewFrame += device_NewFrame;
            _device.Start();
            trackBarControl1.Properties.Maximum = (int) _device.Length;
            TimerPos = new Timer();
            TimerPos.Tick += timerPos_Tick;
            TimerPos.Enabled = true;
            TimerPos.Interval = 1000;
            StartDetection();
          }
        }
        catch
        {
        }
      }
    }

    private void btPause_Click(object sender, EventArgs e)
    {
      if (_device != null && _device.IsRunning)
      {
        try
        {
          _device.Stop();
        }
        catch
        {
        }
      }
      else
      {
        if (_device == null || _device.IsRunning)
          return;
        try
        {
          _device.Start();
          StartDetection();
        }
        catch
        {
        }
      }
    }

    private void btStop_Click(object sender, EventArgs e)
    {
      try
      {
        _device.Stop();
      }
      catch
      {
      }
    }

    private void ReleasDetectors()
    {
      for (int index = 0; index < _faces.Count; ++index)
        _faces[index].Dispose();
      _faces.Clear();
    }

    private void StartDetection()
    {
      Engine.Initialize(0U);
      Thread.Sleep(1500);
      int processorCount = Environment.ProcessorCount;
      if (processorCount > 1)
        --processorCount;
      int[] numArray1 = new int[processorCount];
      int[] numArray2 = new int[processorCount];
      int[] numArray3 = new int[processorCount];
      _detectors = new List<int>();
      ReleasDetectors();
      for (int index = 0; index < processorCount; ++index)
      {
        _faces.Add(new Engine());
        _detectors.Add(0);
      }
    }

    private void btOpen_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        _device.Stop();
      }
      catch
      {
      }
      _filename = openFileDialog.FileName;
      _device = new SeekableFileSource(openFileDialog.FileName);
      _device.NewFrame += device_NewFrame;
      _device.Start();
      trackBarControl1.Properties.Maximum = (int) _device.Length;
      TimerPos = new Timer();
      TimerPos.Tick += timerPos_Tick;
      TimerPos.Enabled = true;
      TimerPos.Interval = 1000;
      StartDetection();
    }

    private void SetNewPos(int val)
    {
      try
      {
        trackBarControl1.Value = val;
      }
      catch
      {
      }
    }

    private void timerPos_Tick(object sender, EventArgs e)
    {
      try
      {
        _nextPos = true;
        Invoke(new SetNewPosFunc(SetNewPos), (object) (int) (double) _device.Length);
        _nextPos = false;
      }
      catch
      {
      }
    }

    private void NewValue(Bitmap img, string time)
    {
      try
      {
        pbImage.Image.Dispose();
      }
      catch
      {
      }
      try
      {
        pbImage.Image = img;
      }
      catch
      {
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

    private void device_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
      try
      {
        int freeHandle = GetFreeHandle();
        if (freeHandle != -1)
        {
          Detect detect = DetectFaces;
          detect.BeginInvoke((Bitmap) eventArgs.Frame.Clone(), freeHandle, DetectCallBack, detect);
        }
        Invoke(new NewValueFunc(NewValue), (object) (Bitmap) eventArgs.Frame.Clone(), (object) "");
      }
      catch
      {
      }
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      if (lvImages.SelectedRowsCount > 0)
      {
        try
        {
          ImgResult = (Bitmap) lvImages.GetDataRow(lvImages.GetSelectedRows()[0])[0];
          Close();
        }
        catch (Exception ex)
        {
          int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        int num1 = (int) XtraMessageBox.Show(Messages.SelectRecord, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void frmVideo_Load(object sender, EventArgs e)
    {
    }

    private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
    {
      try
      {
        if (!_nextPos)
          _device.Seek(trackBarControl1.Value);
        labelControl1.Text = GetTime(trackBarControl1.Value) + "//" + GetTime(trackBarControl1.Properties.Maximum);
      }
      catch
      {
      }
    }

    private void frmVideo_FormClosing(object sender, FormClosingEventArgs e)
    {
      ReleasDetectors();
      try
      {
        TimerPos.Stop();
      }
      catch
      {
      }
      try
      {
        _device.Stop();
      }
      catch
      {
      }
    }

    private void lvImages_CustomDrawCardCaption(object sender, LayoutViewCustomDrawCardCaptionEventArgs e)
    {
      e.CardCaption = (e.RowHandle + 1).ToString();
    }

    private void btClear_Click(object sender, EventArgs e)
    {
      _dtmain.Rows.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmVideo));
      GridLevelNode gridLevelNode = new GridLevelNode();
      splitContainerControl1 = new SplitContainerControl();
      pbImage = new PictureBox();
      gcImages = new GridControl();
      lvImages = new LayoutView();
      colImage = new LayoutViewColumn();
      repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      layoutViewField_layoutViewColumn1 = new LayoutViewField();
      layoutViewCard1 = new LayoutViewCard();
      gridView1 = new GridView();
      btPlay = new SimpleButton();
      btPause = new SimpleButton();
      btStop = new SimpleButton();
      btOpen = new SimpleButton();
      btCancel = new SimpleButton();
      btAccept = new SimpleButton();
      trackBarControl1 = new TrackBarControl();
      labelControl1 = new LabelControl();
      btClear = new SimpleButton();
      splitContainerControl1.BeginInit();
      splitContainerControl1.SuspendLayout();
      ((ISupportInitialize) pbImage).BeginInit();
      gcImages.BeginInit();
      lvImages.BeginInit();
      repositoryItemPictureEdit1.BeginInit();
      layoutViewField_layoutViewColumn1.BeginInit();
      layoutViewCard1.BeginInit();
      gridView1.BeginInit();
      ((ISupportInitialize) trackBarControl1).BeginInit();
      trackBarControl1.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(splitContainerControl1, "splitContainerControl1");
      splitContainerControl1.Name = "splitContainerControl1";
      componentResourceManager.ApplyResources(splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
      splitContainerControl1.Panel1.Controls.Add(pbImage);
      componentResourceManager.ApplyResources(splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
      splitContainerControl1.Panel2.Controls.Add(gcImages);
      splitContainerControl1.SplitterPosition = 751;
      componentResourceManager.ApplyResources(pbImage, "pbImage");
      pbImage.Name = "pbImage";
      pbImage.TabStop = false;
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
      repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
      layoutViewField_layoutViewColumn1.EditorPreferredWidth = 122;
      layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
      layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
      layoutViewField_layoutViewColumn1.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn1.Size = new Size(129, 22);
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
      componentResourceManager.ApplyResources(btPause, "btPause");
      btPause.Appearance.Font = (Font) componentResourceManager.GetObject("btPause.Appearance.Font");
      btPause.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPause.Appearance.FontSizeDelta");
      btPause.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPause.Appearance.FontStyleDelta");
      btPause.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPause.Appearance.GradientMode");
      btPause.Appearance.Image = (Image) componentResourceManager.GetObject("btPause.Appearance.Image");
      btPause.Appearance.Options.UseFont = true;
      btPause.Image = Resources.pause36;
      btPause.Name = "btPause";
      btPause.Click += btPause_Click;
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
      componentResourceManager.ApplyResources(btOpen, "btOpen");
      btOpen.Appearance.Font = (Font) componentResourceManager.GetObject("btOpen.Appearance.Font");
      btOpen.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btOpen.Appearance.FontSizeDelta");
      btOpen.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btOpen.Appearance.FontStyleDelta");
      btOpen.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btOpen.Appearance.GradientMode");
      btOpen.Appearance.Image = (Image) componentResourceManager.GetObject("btOpen.Appearance.Image");
      btOpen.Appearance.Options.UseFont = true;
      btOpen.Image = Resources.Open3;
      btOpen.Name = "btOpen";
      btOpen.Click += btOpen_Click;
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
      btAccept.Name = "btAccept";
      btAccept.Click += btAccept_Click;
      componentResourceManager.ApplyResources(trackBarControl1, "trackBarControl1");
      trackBarControl1.Name = "trackBarControl1";
      trackBarControl1.Properties.AccessibleDescription = componentResourceManager.GetString("trackBarControl1.Properties.AccessibleDescription");
      trackBarControl1.Properties.AccessibleName = componentResourceManager.GetString("trackBarControl1.Properties.AccessibleName");
      trackBarControl1.Properties.Orientation = (Orientation) componentResourceManager.GetObject("trackBarControl1.Properties.Orientation");
      trackBarControl1.EditValueChanged += trackBarControl1_EditValueChanged;
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(btClear, "btClear");
      btClear.Appearance.Font = (Font) componentResourceManager.GetObject("btClear.Appearance.Font");
      btClear.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btClear.Appearance.FontSizeDelta");
      btClear.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btClear.Appearance.FontStyleDelta");
      btClear.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btClear.Appearance.GradientMode");
      btClear.Appearance.Image = (Image) componentResourceManager.GetObject("btClear.Appearance.Image");
      btClear.Appearance.Options.UseFont = true;
      btClear.Name = "btClear";
      btClear.Click += btClear_Click;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(btClear);
      Controls.Add(labelControl1);
      Controls.Add(trackBarControl1);
      Controls.Add(splitContainerControl1);
      Controls.Add(btCancel);
      Controls.Add(btAccept);
      Controls.Add(btOpen);
      Controls.Add(btStop);
      Controls.Add(btPause);
      Controls.Add(btPlay);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmVideo";
      FormClosing += frmVideo_FormClosing;
      Load += frmVideo_Load;
      splitContainerControl1.EndInit();
      splitContainerControl1.ResumeLayout(false);
      ((ISupportInitialize) pbImage).EndInit();
      gcImages.EndInit();
      lvImages.EndInit();
      repositoryItemPictureEdit1.EndInit();
      layoutViewField_layoutViewColumn1.EndInit();
      layoutViewCard1.EndInit();
      gridView1.EndInit();
      trackBarControl1.Properties.EndInit();
      ((ISupportInitialize) trackBarControl1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    private delegate void AddImageFunc(Bitmap img);

    private delegate void NewValueFunc(Bitmap img, string time);

    private delegate void SetNewPosFunc(int val);

    private delegate void Detect(Bitmap bmp, int index);
  }
}
