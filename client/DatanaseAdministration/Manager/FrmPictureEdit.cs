// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmPictureEdit
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using CascadeManager.Properties;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FontStyle = System.Drawing.FontStyle;

namespace CascadeManager
{
  public class FrmPictureEdit : XtraForm
  {
    public EditPictureControl WpfControl = new EditPictureControl();
    public byte[] Picture = new byte[0];
    private IContainer components;
    private ElementHost elementHost1;
    private BarManager barManager1;
    private Bar bar1;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private BarButtonItem barButtonItem5;
    private SimpleButton btCancel;
    private SimpleButton btAccept;
    public BarButtonItem btOpen;
    public BarButtonItem btSave;
    public BarButtonItem btCancelEdit;
    public BarButtonItem btSelect;
    public BarButtonItem btEye;
    public BarButtonItem btRotateLeft;
    public BarButtonItem btRotateRight;
    public BarButtonItem btMirrow;
    public BarButtonItem btCutSelection;
    public TrackBarControl btBrightness;
    public LabelControl lbSize;
    public TrackBarControl btSize;
    public LabelControl lbContrast;
    public TrackBarControl btContrast;
    public LabelControl lbBrightness;

    public FrmPictureEdit()
    {
      InitializeComponent();
      WpfControl = new EditPictureControl();
      if (Picture.Length > 0)
        WpfControl.LoadImage(Picture);
      elementHost1.Child = WpfControl;
      WpfControl.Mainform = this;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
    }

    private void btOpen_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbOpen1_Click(new object(), new RoutedEventArgs());
    }

    private void btSave_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbSave_Click(new object(), new RoutedEventArgs());
    }

    private void btCancelEdit_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbCancelEdit_Click(new object(), new RoutedEventArgs());
    }

    private void btSelect_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbEye_Click(new object(), new RoutedEventArgs());
      btEye.Down = false;
    }

    private void btEye_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbEye_Click(new object(), new RoutedEventArgs());
      btSelect.Down = false;
    }

    private void btRotateLeft_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbRotate_Click(new object(), new RoutedEventArgs());
    }

    private void btRotateRight_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbRotateRight_Click(new object(), new RoutedEventArgs());
    }

    private void btMirrow_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbFlip_Click(new object(), new RoutedEventArgs());
    }

    private void btCutSelection_ItemClick(object sender, ItemClickEventArgs e)
    {
      WpfControl.rdbCutArea_Click(new object(), new RoutedEventArgs());
    }

    private void frmPictureEdit_Load(object sender, EventArgs e)
    {
    }

    private void btBrightness_EditValueChanged(object sender, EventArgs e)
    {
      WpfControl.slBrightness_ValueChanged(new object(), new RoutedPropertyChangedEventArgs<double>(0.0, Convert.ToDouble(btBrightness.Value)));
    }

    private void btContrast_EditValueChanged(object sender, EventArgs e)
    {
      WpfControl.slContrast_ValueChanged(new object(), new RoutedPropertyChangedEventArgs<double>(0.0, Convert.ToDouble(btContrast.Value)));
    }

    private void btSize_EditValueChanged_1(object sender, EventArgs e)
    {
      WpfControl.slSize_ValueChanged(new object(), new RoutedPropertyChangedEventArgs<double>(0.0, Convert.ToDouble(btSize.Value)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      components = new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmPictureEdit));
      elementHost1 = new ElementHost();
      barManager1 = new BarManager(components);
      bar1 = new Bar();
      btOpen = new BarButtonItem();
      btSave = new BarButtonItem();
      btCancelEdit = new BarButtonItem();
      btSelect = new BarButtonItem();
      btEye = new BarButtonItem();
      btRotateLeft = new BarButtonItem();
      btRotateRight = new BarButtonItem();
      btMirrow = new BarButtonItem();
      btCutSelection = new BarButtonItem();
      barDockControlTop = new BarDockControl();
      barDockControlBottom = new BarDockControl();
      barDockControlLeft = new BarDockControl();
      barDockControlRight = new BarDockControl();
      barButtonItem5 = new BarButtonItem();
      btAccept = new SimpleButton();
      btCancel = new SimpleButton();
      btBrightness = new TrackBarControl();
      lbBrightness = new LabelControl();
      lbContrast = new LabelControl();
      btContrast = new TrackBarControl();
      lbSize = new LabelControl();
      btSize = new TrackBarControl();
      barManager1.BeginInit();
      ((ISupportInitialize) btBrightness).BeginInit();
      btBrightness.Properties.BeginInit();
      ((ISupportInitialize) btContrast).BeginInit();
      btContrast.Properties.BeginInit();
      ((ISupportInitialize) btSize).BeginInit();
      btSize.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(elementHost1, "elementHost1");
      elementHost1.Name = "elementHost1";
      elementHost1.Child = null;
      barManager1.Bars.AddRange(new Bar[1]
      {
        bar1
      });
      barManager1.DockControls.Add(barDockControlTop);
      barManager1.DockControls.Add(barDockControlBottom);
      barManager1.DockControls.Add(barDockControlLeft);
      barManager1.DockControls.Add(barDockControlRight);
      barManager1.Form = this;
      barManager1.Items.AddRange(new BarItem[10]
      {
        btOpen,
        btSave,
        btCancelEdit,
        btSelect,
        btEye,
        barButtonItem5,
        btRotateLeft,
        btRotateRight,
        btMirrow,
        btCutSelection
      });
      barManager1.MaxItemId = 20;
      bar1.BarName = "Tools";
      bar1.DockCol = 0;
      bar1.DockRow = 0;
      bar1.DockStyle = BarDockStyle.Top;
      bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[9]
      {
        new LinkPersistInfo(btOpen),
        new LinkPersistInfo(btSave),
        new LinkPersistInfo(btCancelEdit),
        new LinkPersistInfo(btSelect),
        new LinkPersistInfo(btEye),
        new LinkPersistInfo(btRotateLeft),
        new LinkPersistInfo(btRotateRight),
        new LinkPersistInfo(btMirrow),
        new LinkPersistInfo(btCutSelection)
      });
      bar1.OptionsBar.AllowQuickCustomization = false;
      bar1.OptionsBar.MultiLine = true;
      bar1.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources(bar1, "bar1");
      componentResourceManager.ApplyResources(btOpen, "btOpen");
      btOpen.Border = BorderStyles.Default;
      btOpen.Glyph = Resources._10;
      btOpen.Id = 8;
      btOpen.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btOpen.ItemAppearance.Normal.Font");
      btOpen.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btOpen.ItemAppearance.Normal.FontSizeDelta");
      btOpen.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btOpen.ItemAppearance.Normal.FontStyleDelta");
      btOpen.ItemAppearance.Normal.Options.UseFont = true;
      btOpen.Name = "btOpen";
      btOpen.ItemClick += btOpen_ItemClick;
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Border = BorderStyles.Default;
      btSave.Glyph = Resources._13;
      btSave.Id = 9;
      btSave.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.Font");
      btSave.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.FontSizeDelta");
      btSave.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.FontStyleDelta");
      btSave.ItemAppearance.Normal.Options.UseFont = true;
      btSave.Name = "btSave";
      btSave.ItemClick += btSave_ItemClick;
      componentResourceManager.ApplyResources(btCancelEdit, "btCancelEdit");
      btCancelEdit.Border = BorderStyles.Default;
      btCancelEdit.Glyph = Resources._11;
      btCancelEdit.Id = 10;
      btCancelEdit.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btCancelEdit.ItemAppearance.Normal.Font");
      btCancelEdit.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btCancelEdit.ItemAppearance.Normal.FontSizeDelta");
      btCancelEdit.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancelEdit.ItemAppearance.Normal.FontStyleDelta");
      btCancelEdit.ItemAppearance.Normal.Options.UseFont = true;
      btCancelEdit.Name = "btCancelEdit";
      btCancelEdit.ItemClick += btCancelEdit_ItemClick;
      componentResourceManager.ApplyResources(btSelect, "btSelect");
      btSelect.Border = BorderStyles.Default;
      btSelect.ButtonStyle = BarButtonStyle.Check;
      btSelect.Glyph = Resources._08;
      btSelect.Id = 11;
      btSelect.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btSelect.ItemAppearance.Normal.Font");
      btSelect.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btSelect.ItemAppearance.Normal.FontSizeDelta");
      btSelect.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSelect.ItemAppearance.Normal.FontStyleDelta");
      btSelect.ItemAppearance.Normal.Options.UseFont = true;
      btSelect.Name = "btSelect";
      btSelect.ItemClick += btSelect_ItemClick;
      componentResourceManager.ApplyResources(btEye, "btEye");
      btEye.Border = BorderStyles.Default;
      btEye.ButtonStyle = BarButtonStyle.Check;
      btEye.Glyph = Resources._01;
      btEye.Id = 12;
      btEye.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btEye.ItemAppearance.Normal.Font");
      btEye.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btEye.ItemAppearance.Normal.FontSizeDelta");
      btEye.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btEye.ItemAppearance.Normal.FontStyleDelta");
      btEye.ItemAppearance.Normal.Options.UseFont = true;
      btEye.Name = "btEye";
      btEye.ItemClick += btEye_ItemClick;
      componentResourceManager.ApplyResources(btRotateLeft, "btRotateLeft");
      btRotateLeft.Border = BorderStyles.Default;
      btRotateLeft.Glyph = Resources._09;
      btRotateLeft.Id = 14;
      btRotateLeft.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btRotateLeft.ItemAppearance.Normal.Font");
      btRotateLeft.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btRotateLeft.ItemAppearance.Normal.FontSizeDelta");
      btRotateLeft.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btRotateLeft.ItemAppearance.Normal.FontStyleDelta");
      btRotateLeft.ItemAppearance.Normal.Options.UseFont = true;
      btRotateLeft.Name = "btRotateLeft";
      btRotateLeft.ItemClick += btRotateLeft_ItemClick;
      componentResourceManager.ApplyResources(btRotateRight, "btRotateRight");
      btRotateRight.Border = BorderStyles.Default;
      btRotateRight.Glyph = Resources._12;
      btRotateRight.Id = 15;
      btRotateRight.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btRotateRight.ItemAppearance.Normal.Font");
      btRotateRight.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btRotateRight.ItemAppearance.Normal.FontSizeDelta");
      btRotateRight.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btRotateRight.ItemAppearance.Normal.FontStyleDelta");
      btRotateRight.ItemAppearance.Normal.Options.UseFont = true;
      btRotateRight.Name = "btRotateRight";
      btRotateRight.ItemClick += btRotateRight_ItemClick;
      componentResourceManager.ApplyResources(btMirrow, "btMirrow");
      btMirrow.Border = BorderStyles.Default;
      btMirrow.Glyph = Resources._07;
      btMirrow.Id = 16;
      btMirrow.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btMirrow.ItemAppearance.Normal.Font");
      btMirrow.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btMirrow.ItemAppearance.Normal.FontSizeDelta");
      btMirrow.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btMirrow.ItemAppearance.Normal.FontStyleDelta");
      btMirrow.ItemAppearance.Normal.Options.UseFont = true;
      btMirrow.Name = "btMirrow";
      btMirrow.ItemClick += btMirrow_ItemClick;
      componentResourceManager.ApplyResources(btCutSelection, "btCutSelection");
      btCutSelection.Border = BorderStyles.Default;
      btCutSelection.Glyph = Resources._06;
      btCutSelection.Id = 17;
      btCutSelection.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btCutSelection.ItemAppearance.Normal.Font");
      btCutSelection.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btCutSelection.ItemAppearance.Normal.FontSizeDelta");
      btCutSelection.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCutSelection.ItemAppearance.Normal.FontStyleDelta");
      btCutSelection.ItemAppearance.Normal.Options.UseFont = true;
      btCutSelection.Name = "btCutSelection";
      btCutSelection.ItemClick += btCutSelection_ItemClick;
      componentResourceManager.ApplyResources(barDockControlTop, "barDockControlTop");
      barDockControlTop.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlTop.Appearance.FontSizeDelta");
      barDockControlTop.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlTop.Appearance.FontStyleDelta");
      barDockControlTop.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlTop.Appearance.GradientMode");
      barDockControlTop.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlTop.Appearance.Image");
      barDockControlTop.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlBottom, "barDockControlBottom");
      barDockControlBottom.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlBottom.Appearance.FontSizeDelta");
      barDockControlBottom.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlBottom.Appearance.FontStyleDelta");
      barDockControlBottom.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlBottom.Appearance.GradientMode");
      barDockControlBottom.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlBottom.Appearance.Image");
      barDockControlBottom.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlLeft, "barDockControlLeft");
      barDockControlLeft.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlLeft.Appearance.FontSizeDelta");
      barDockControlLeft.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlLeft.Appearance.FontStyleDelta");
      barDockControlLeft.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlLeft.Appearance.GradientMode");
      barDockControlLeft.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlLeft.Appearance.Image");
      barDockControlLeft.CausesValidation = false;
      componentResourceManager.ApplyResources(barDockControlRight, "barDockControlRight");
      barDockControlRight.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlRight.Appearance.FontSizeDelta");
      barDockControlRight.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlRight.Appearance.FontStyleDelta");
      barDockControlRight.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlRight.Appearance.GradientMode");
      barDockControlRight.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlRight.Appearance.Image");
      barDockControlRight.CausesValidation = false;
      componentResourceManager.ApplyResources(barButtonItem5, "barButtonItem5");
      barButtonItem5.Glyph = Resources._09;
      barButtonItem5.Id = 13;
      barButtonItem5.Name = "barButtonItem5";
      componentResourceManager.ApplyResources(btAccept, "btAccept");
      btAccept.DialogResult = DialogResult.OK;
      btAccept.Name = "btAccept";
      btAccept.Click += btAccept_Click;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.DialogResult = DialogResult.Cancel;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(btBrightness, "btBrightness");
      btBrightness.MenuManager = barManager1;
      btBrightness.Name = "btBrightness";
      btBrightness.Properties.AccessibleDescription = componentResourceManager.GetString("btBrightness.Properties.AccessibleDescription");
      btBrightness.Properties.AccessibleName = componentResourceManager.GetString("btBrightness.Properties.AccessibleName");
      btBrightness.Properties.AutoSize = false;
      btBrightness.Properties.Maximum = byte.MaxValue;
      btBrightness.Properties.Minimum = -255;
      btBrightness.Properties.Orientation = (Orientation) componentResourceManager.GetObject("btBrightness.Properties.Orientation");
      btBrightness.Value = -255;
      btBrightness.EditValueChanged += btBrightness_EditValueChanged;
      componentResourceManager.ApplyResources(lbBrightness, "lbBrightness");
      lbBrightness.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbBrightness.Appearance.DisabledImage");
      lbBrightness.Appearance.Font = (Font) componentResourceManager.GetObject("lbBrightness.Appearance.Font");
      lbBrightness.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbBrightness.Appearance.FontSizeDelta");
      lbBrightness.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbBrightness.Appearance.FontStyleDelta");
      lbBrightness.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbBrightness.Appearance.GradientMode");
      lbBrightness.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbBrightness.Appearance.HoverImage");
      lbBrightness.Appearance.Image = (Image) componentResourceManager.GetObject("lbBrightness.Appearance.Image");
      lbBrightness.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbBrightness.Appearance.PressedImage");
      lbBrightness.Name = "lbBrightness";
      componentResourceManager.ApplyResources(lbContrast, "lbContrast");
      lbContrast.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbContrast.Appearance.DisabledImage");
      lbContrast.Appearance.Font = (Font) componentResourceManager.GetObject("lbContrast.Appearance.Font");
      lbContrast.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbContrast.Appearance.FontSizeDelta");
      lbContrast.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbContrast.Appearance.FontStyleDelta");
      lbContrast.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbContrast.Appearance.GradientMode");
      lbContrast.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbContrast.Appearance.HoverImage");
      lbContrast.Appearance.Image = (Image) componentResourceManager.GetObject("lbContrast.Appearance.Image");
      lbContrast.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbContrast.Appearance.PressedImage");
      lbContrast.Name = "lbContrast";
      componentResourceManager.ApplyResources(btContrast, "btContrast");
      btContrast.MenuManager = barManager1;
      btContrast.Name = "btContrast";
      btContrast.Properties.AccessibleDescription = componentResourceManager.GetString("btContrast.Properties.AccessibleDescription");
      btContrast.Properties.AccessibleName = componentResourceManager.GetString("btContrast.Properties.AccessibleName");
      btContrast.Properties.AutoSize = false;
      btContrast.Properties.Maximum = sbyte.MaxValue;
      btContrast.Properties.Minimum = -127;
      btContrast.Properties.Orientation = (Orientation) componentResourceManager.GetObject("btContrast.Properties.Orientation");
      btContrast.Value = -127;
      btContrast.EditValueChanged += btContrast_EditValueChanged;
      componentResourceManager.ApplyResources(lbSize, "lbSize");
      lbSize.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSize.Appearance.DisabledImage");
      lbSize.Appearance.Font = (Font) componentResourceManager.GetObject("lbSize.Appearance.Font");
      lbSize.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSize.Appearance.FontSizeDelta");
      lbSize.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSize.Appearance.FontStyleDelta");
      lbSize.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSize.Appearance.GradientMode");
      lbSize.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSize.Appearance.HoverImage");
      lbSize.Appearance.Image = (Image) componentResourceManager.GetObject("lbSize.Appearance.Image");
      lbSize.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSize.Appearance.PressedImage");
      lbSize.Name = "lbSize";
      componentResourceManager.ApplyResources(btSize, "btSize");
      btSize.MenuManager = barManager1;
      btSize.Name = "btSize";
      btSize.Properties.AccessibleDescription = componentResourceManager.GetString("btSize.Properties.AccessibleDescription");
      btSize.Properties.AccessibleName = componentResourceManager.GetString("btSize.Properties.AccessibleName");
      btSize.Properties.AutoSize = false;
      btSize.Properties.Maximum = 500;
      btSize.Properties.Orientation = (Orientation) componentResourceManager.GetObject("btSize.Properties.Orientation");
      btSize.Value = 100;
      btSize.EditValueChanged += btSize_EditValueChanged_1;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(lbSize);
      Controls.Add(btSize);
      Controls.Add(lbContrast);
      Controls.Add(btContrast);
      Controls.Add(lbBrightness);
      Controls.Add(btBrightness);
      Controls.Add(btCancel);
      Controls.Add(btAccept);
      Controls.Add(elementHost1);
      Controls.Add(barDockControlLeft);
      Controls.Add(barDockControlRight);
      Controls.Add(barDockControlBottom);
      Controls.Add(barDockControlTop);
      Name = "FrmPictureEdit";
      ShowIcon = false;
      Load += frmPictureEdit_Load;
      barManager1.EndInit();
      btBrightness.Properties.EndInit();
      ((ISupportInitialize) btBrightness).EndInit();
      btContrast.Properties.EndInit();
      ((ISupportInitialize) btContrast).EndInit();
      btSize.Properties.EndInit();
      ((ISupportInitialize) btSize).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
