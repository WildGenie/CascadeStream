// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmCommonSettings
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmCommonSettings : XtraForm
  {
    private List<BcKeySettings> _keysettings = new List<BcKeySettings>();
    private bool _allowClose = true;
    private List<BcKeySettings> _settings = new List<BcKeySettings>();
    private DataTable _dt = new DataTable();
    private List<BcKeySettings> _deletedKeys = new List<BcKeySettings>();
    private IContainer components = null;
    private bool _allowChange;
    private SimpleButton btCancel;
    private SimpleButton btSave;
    private GroupControl groupControl1;
    private ProgressBarControl progressBar1;
    private SimpleButton btAdd;
    private SimpleButton btDelete;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colLeft;
    private GridColumn colRight;
    private GridColumn colDown;
    private GridColumn colUp;
    private GridColumn colChangeFlag;
    private LabelControl lbCount;

    public FrmCommonSettings()
    {
      InitializeComponent();
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      foreach (BcKeySettings bcKeySettings in _deletedKeys)
        bcKeySettings.Delete();
      _deletedKeys.Clear();
      _keysettings = new List<BcKeySettings>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) _dt.Rows)
      {
        if (Convert.ToInt32(dataRow[0]) == -1 || Convert.ToBoolean(dataRow[5]))
        {
          BcKeySettings bcKeySettings = new BcKeySettings();
          float num1 = Convert.ToSingle(dataRow[3].ToString().Replace(".", ","));
          float num2 = Convert.ToSingle(dataRow[4].ToString().Replace(".", ","));
          float num3 = Convert.ToSingle(dataRow[1].ToString().Replace(".", ","));
          float num4 = Convert.ToSingle(dataRow[2].ToString().Replace(".", ","));
          bcKeySettings.X = (double) num1 == 0.0 ? -Math.Abs(num2) : Math.Abs(num1);
          bcKeySettings.Y = (double) num3 == 0.0 ? Math.Abs(num4) : -Math.Abs(num3);
          bcKeySettings.Id = Convert.ToInt32(dataRow[0]);
          if (bcKeySettings.X != 0.0 || bcKeySettings.Y != 0.0)
          {
            bcKeySettings.Save();
            dataRow[0] = bcKeySettings.Id;
            _keysettings.Add(bcKeySettings);
          }
        }
      }
      if (_keysettings.Count <= 0)
        ;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      gridControl1.Enabled = true;
      btAdd.Enabled = true;
      btDelete.Enabled = true;
      btSave.Enabled = true;
    }

    private void frmKeySettrings_Load(object sender, EventArgs e)
    {
      progressBar1.Visible = false;
      _dt = new DataTable();
      _dt.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("ID"),
        new DataColumn("Left"),
        new DataColumn("Right"),
        new DataColumn("Down"),
        new DataColumn("Up"),
        new DataColumn("ChangeFlag")
      });
      _allowChange = false;
      _settings = BcKeySettings.LoadAll();
      foreach (BcKeySettings bcKeySettings in _settings)
      {
        float num1 = bcKeySettings.X;
        float num2 = bcKeySettings.X;
        float num3 = bcKeySettings.Y;
        float num4 = bcKeySettings.Y;
        if (bcKeySettings.X > 0.0)
          num2 = 0.0f;
        else
          num1 = 0.0f;
        if (bcKeySettings.Y > 0.0)
          num3 = 0.0f;
        else
          num4 = 0.0f;
        _dt.Rows.Add((object) bcKeySettings.Id, (object) Math.Abs(num3), (object) Math.Abs(num4), (object) Math.Abs(num1), (object) Math.Abs(num2), (object) false);
      }
      gridControl1.DataSource = _dt;
      _allowChange = true;
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
      _dt.Rows.Add((object) -1, (object) 0, (object) 0, (object) 0, (object) 0, (object) false);
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (gridView1.SelectedRowsCount <= 0)
        return;
      if (Convert.ToInt32(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0]) != -1)
        _deletedKeys.Add(new BcKeySettings
        {
          Id = Convert.ToInt32(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0])
        });
      _dt.Rows.RemoveAt(gridView1.GetSelectedRows()[0]);
    }

    private void frmKeySettrings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_allowClose)
        return;
      e.Cancel = true;
    }

    private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
      if (e.Column.VisibleIndex <= -1 || e.Column.VisibleIndex >= 5 || !_allowChange)
        return;
      _dt.Rows[e.RowHandle][5] = true;
    }

    private void gridView1_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
    {
      try
      {
        double num = Convert.ToSingle(e.Value.ToString().Replace(".", ","));
      }
      catch
      {
        e.Valid = false;
        int num = (int) XtraMessageBox.Show(Messages.IncorrectInputFormat, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void frmCommonSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
      frmKeySettrings_FormClosing(new object(), e);
    }

    private void frmCommonSettings_Load(object sender, EventArgs e)
    {
      CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
      cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
      cultureInfo.DateTimeFormat.DateSeparator = ".";
      cultureInfo.DateTimeFormat.PMDesignator = "";
      cultureInfo.DateTimeFormat.AMDesignator = "";
      cultureInfo.DateTimeFormat.TimeSeparator = ":";
      cultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
      cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
      cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
      frmKeySettrings_Load(new object(), new EventArgs());
    }

    private void frmCommonSettings_Resize(object sender, EventArgs e)
    {
      ControlBox = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCommonSettings));
      progressBar1 = new ProgressBarControl();
      btCancel = new SimpleButton();
      btSave = new SimpleButton();
      groupControl1 = new GroupControl();
      lbCount = new LabelControl();
      btAdd = new SimpleButton();
      btDelete = new SimpleButton();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colLeft = new GridColumn();
      colRight = new GridColumn();
      colDown = new GridColumn();
      colUp = new GridColumn();
      colChangeFlag = new GridColumn();
      progressBar1.Properties.BeginInit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(progressBar1, "progressBar1");
      progressBar1.Name = "progressBar1";
      progressBar1.Properties.AccessibleDescription = componentResourceManager.GetString("progressBar1.Properties.AccessibleDescription");
      progressBar1.Properties.AccessibleName = componentResourceManager.GetString("progressBar1.Properties.AccessibleName");
      progressBar1.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBar1.Properties.Appearance.GradientMode");
      progressBar1.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("progressBar1.Properties.Appearance.Image");
      progressBar1.Properties.AppearanceDisabled.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBar1.Properties.AppearanceDisabled.GradientMode");
      progressBar1.Properties.AppearanceDisabled.Image = (Image) componentResourceManager.GetObject("progressBar1.Properties.AppearanceDisabled.Image");
      progressBar1.Properties.AppearanceFocused.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBar1.Properties.AppearanceFocused.GradientMode");
      progressBar1.Properties.AppearanceFocused.Image = (Image) componentResourceManager.GetObject("progressBar1.Properties.AppearanceFocused.Image");
      progressBar1.Properties.AppearanceReadOnly.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBar1.Properties.AppearanceReadOnly.GradientMode");
      progressBar1.Properties.AppearanceReadOnly.Image = (Image) componentResourceManager.GetObject("progressBar1.Properties.AppearanceReadOnly.Image");
      progressBar1.Properties.AutoHeight = (bool) componentResourceManager.GetObject("progressBar1.Properties.AutoHeight");
      progressBar1.Properties.ShowTitle = true;
      progressBar1.Properties.Step = 1;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      btCancel.Appearance.Options.UseFont = true;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      btSave.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btSave.Appearance.GradientMode");
      btSave.Appearance.Image = (Image) componentResourceManager.GetObject("btSave.Appearance.Image");
      btSave.Appearance.Options.UseFont = true;
      btSave.Name = "btSave";
      btSave.Click += btSave_Click;
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Controls.Add(lbCount);
      groupControl1.Controls.Add(btAdd);
      groupControl1.Controls.Add(btDelete);
      groupControl1.Controls.Add(gridControl1);
      groupControl1.Name = "groupControl1";
      groupControl1.ShowCaption = false;
      componentResourceManager.ApplyResources(lbCount, "lbCount");
      lbCount.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.DisabledImage");
      lbCount.Appearance.Font = (Font) componentResourceManager.GetObject("lbCount.Appearance.Font");
      lbCount.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbCount.Appearance.GradientMode");
      lbCount.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.HoverImage");
      lbCount.Appearance.Image = (Image) componentResourceManager.GetObject("lbCount.Appearance.Image");
      lbCount.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.PressedImage");
      lbCount.Name = "lbCount";
      componentResourceManager.ApplyResources(btAdd, "btAdd");
      btAdd.Appearance.Font = (Font) componentResourceManager.GetObject("btAdd.Appearance.Font");
      btAdd.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAdd.Appearance.GradientMode");
      btAdd.Appearance.Image = (Image) componentResourceManager.GetObject("btAdd.Appearance.Image");
      btAdd.Appearance.Options.UseFont = true;
      btAdd.Name = "btAdd";
      btAdd.Click += btAdd_Click;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btDelete.Appearance.GradientMode");
      btDelete.Appearance.Image = (Image) componentResourceManager.GetObject("btDelete.Appearance.Image");
      btDelete.Appearance.Options.UseFont = true;
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleDescription");
      gridControl1.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleName");
      gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip");
      gridControl1.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.Anchor");
      gridControl1.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImage");
      gridControl1.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImageLayout");
      gridControl1.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ImeMode");
      gridControl1.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.MaximumSize");
      gridControl1.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.TextLocation");
      gridControl1.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTip");
      gridControl1.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ToolTipIconType");
      gridControl1.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTipTitle");
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridView1.Appearance.ColumnFilterButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.Font");
      gridView1.Appearance.ColumnFilterButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.GradientMode");
      gridView1.Appearance.ColumnFilterButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.Image");
      gridView1.Appearance.ColumnFilterButton.Options.UseFont = true;
      gridView1.Appearance.ColumnFilterButtonActive.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.Font");
      gridView1.Appearance.ColumnFilterButtonActive.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.GradientMode");
      gridView1.Appearance.ColumnFilterButtonActive.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.Image");
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
      gridView1.Appearance.CustomizationFormHint.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.Font");
      gridView1.Appearance.CustomizationFormHint.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.GradientMode");
      gridView1.Appearance.CustomizationFormHint.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.Image");
      gridView1.Appearance.CustomizationFormHint.Options.UseFont = true;
      gridView1.Appearance.DetailTip.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.Font");
      gridView1.Appearance.DetailTip.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.GradientMode");
      gridView1.Appearance.DetailTip.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.Image");
      gridView1.Appearance.DetailTip.Options.UseFont = true;
      gridView1.Appearance.Empty.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Empty.Font");
      gridView1.Appearance.Empty.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Empty.GradientMode");
      gridView1.Appearance.Empty.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Empty.Image");
      gridView1.Appearance.Empty.Options.UseFont = true;
      gridView1.Appearance.EvenRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.Font");
      gridView1.Appearance.EvenRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.GradientMode");
      gridView1.Appearance.EvenRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.Image");
      gridView1.Appearance.EvenRow.Options.UseFont = true;
      gridView1.Appearance.FilterCloseButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.Font");
      gridView1.Appearance.FilterCloseButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.GradientMode");
      gridView1.Appearance.FilterCloseButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.Image");
      gridView1.Appearance.FilterCloseButton.Options.UseFont = true;
      gridView1.Appearance.FilterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.Font");
      gridView1.Appearance.FilterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.GradientMode");
      gridView1.Appearance.FilterPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.Image");
      gridView1.Appearance.FilterPanel.Options.UseFont = true;
      gridView1.Appearance.FixedLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.Font");
      gridView1.Appearance.FixedLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.GradientMode");
      gridView1.Appearance.FixedLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.Image");
      gridView1.Appearance.FixedLine.Options.UseFont = true;
      gridView1.Appearance.FocusedCell.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.Font");
      gridView1.Appearance.FocusedCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.GradientMode");
      gridView1.Appearance.FocusedCell.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.Image");
      gridView1.Appearance.FocusedCell.Options.UseFont = true;
      gridView1.Appearance.FocusedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.Font");
      gridView1.Appearance.FocusedRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.GradientMode");
      gridView1.Appearance.FocusedRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.Image");
      gridView1.Appearance.FocusedRow.Options.UseFont = true;
      gridView1.Appearance.FooterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.Font");
      gridView1.Appearance.FooterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.GradientMode");
      gridView1.Appearance.FooterPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.Image");
      gridView1.Appearance.FooterPanel.Options.UseFont = true;
      gridView1.Appearance.GroupButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.Font");
      gridView1.Appearance.GroupButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.GradientMode");
      gridView1.Appearance.GroupButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.Image");
      gridView1.Appearance.GroupButton.Options.UseFont = true;
      gridView1.Appearance.GroupFooter.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.Font");
      gridView1.Appearance.GroupFooter.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.GradientMode");
      gridView1.Appearance.GroupFooter.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.Image");
      gridView1.Appearance.GroupFooter.Options.UseFont = true;
      gridView1.Appearance.GroupPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.Font");
      gridView1.Appearance.GroupPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.GradientMode");
      gridView1.Appearance.GroupPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.Image");
      gridView1.Appearance.GroupPanel.Options.UseFont = true;
      gridView1.Appearance.GroupRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.Font");
      gridView1.Appearance.GroupRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.GradientMode");
      gridView1.Appearance.GroupRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.Image");
      gridView1.Appearance.GroupRow.Options.UseFont = true;
      gridView1.Appearance.HeaderPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.Font");
      gridView1.Appearance.HeaderPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.GradientMode");
      gridView1.Appearance.HeaderPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.Image");
      gridView1.Appearance.HeaderPanel.Options.UseFont = true;
      gridView1.Appearance.HideSelectionRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.Font");
      gridView1.Appearance.HideSelectionRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.GradientMode");
      gridView1.Appearance.HideSelectionRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.Image");
      gridView1.Appearance.HideSelectionRow.Options.UseFont = true;
      gridView1.Appearance.HorzLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.Font");
      gridView1.Appearance.HorzLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.GradientMode");
      gridView1.Appearance.HorzLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.Image");
      gridView1.Appearance.HorzLine.Options.UseFont = true;
      gridView1.Appearance.OddRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.OddRow.Font");
      gridView1.Appearance.OddRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.OddRow.GradientMode");
      gridView1.Appearance.OddRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.OddRow.Image");
      gridView1.Appearance.OddRow.Options.UseFont = true;
      gridView1.Appearance.Preview.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Preview.Font");
      gridView1.Appearance.Preview.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Preview.GradientMode");
      gridView1.Appearance.Preview.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Preview.Image");
      gridView1.Appearance.Preview.Options.UseFont = true;
      gridView1.Appearance.Row.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Row.Font");
      gridView1.Appearance.Row.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Row.GradientMode");
      gridView1.Appearance.Row.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Row.Image");
      gridView1.Appearance.Row.Options.UseFont = true;
      gridView1.Appearance.RowSeparator.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.Font");
      gridView1.Appearance.RowSeparator.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.GradientMode");
      gridView1.Appearance.RowSeparator.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.Image");
      gridView1.Appearance.RowSeparator.Options.UseFont = true;
      gridView1.Appearance.SelectedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.Font");
      gridView1.Appearance.SelectedRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.GradientMode");
      gridView1.Appearance.SelectedRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.Image");
      gridView1.Appearance.SelectedRow.Options.UseFont = true;
      gridView1.Appearance.TopNewRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.Font");
      gridView1.Appearance.TopNewRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.GradientMode");
      gridView1.Appearance.TopNewRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.Image");
      gridView1.Appearance.TopNewRow.Options.UseFont = true;
      gridView1.Appearance.VertLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.VertLine.Font");
      gridView1.Appearance.VertLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.VertLine.GradientMode");
      gridView1.Appearance.VertLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.VertLine.Image");
      gridView1.Appearance.VertLine.Options.UseFont = true;
      gridView1.Appearance.ViewCaption.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.Font");
      gridView1.Appearance.ViewCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.GradientMode");
      gridView1.Appearance.ViewCaption.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.Image");
      gridView1.Appearance.ViewCaption.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.Columns.AddRange(new GridColumn[6]
      {
        colID,
        colLeft,
        colRight,
        colDown,
        colUp,
        colChangeFlag
      });
      gridView1.GridControl = gridControl1;
      gridView1.Name = "gridView1";
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.CellValueChanged += gridView1_CellValueChanged;
      gridView1.ValidatingEditor += gridView1_ValidatingEditor;
      colID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      colID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceCell.GradientMode");
      colID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colID.AppearanceCell.Image");
      colID.AppearanceCell.Options.UseFont = true;
      colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      colID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceHeader.GradientMode");
      colID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colID.AppearanceHeader.Image");
      colID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colID, "colID");
      colID.FieldName = "ID";
      colID.Name = "colID";
      colLeft.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colLeft.AppearanceCell.Font");
      colLeft.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colLeft.AppearanceCell.GradientMode");
      colLeft.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colLeft.AppearanceCell.Image");
      colLeft.AppearanceCell.Options.UseFont = true;
      colLeft.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colLeft.AppearanceHeader.Font");
      colLeft.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colLeft.AppearanceHeader.GradientMode");
      colLeft.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colLeft.AppearanceHeader.Image");
      colLeft.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colLeft, "colLeft");
      colLeft.FieldName = "Left";
      colLeft.Name = "colLeft";
      colRight.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colRight.AppearanceCell.Font");
      colRight.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colRight.AppearanceCell.GradientMode");
      colRight.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colRight.AppearanceCell.Image");
      colRight.AppearanceCell.Options.UseFont = true;
      colRight.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colRight.AppearanceHeader.Font");
      colRight.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colRight.AppearanceHeader.GradientMode");
      colRight.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colRight.AppearanceHeader.Image");
      colRight.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colRight, "colRight");
      colRight.FieldName = "Right";
      colRight.Name = "colRight";
      colDown.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colDown.AppearanceCell.Font");
      colDown.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDown.AppearanceCell.GradientMode");
      colDown.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colDown.AppearanceCell.Image");
      colDown.AppearanceCell.Options.UseFont = true;
      colDown.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colDown.AppearanceHeader.Font");
      colDown.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDown.AppearanceHeader.GradientMode");
      colDown.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colDown.AppearanceHeader.Image");
      colDown.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colDown, "colDown");
      colDown.FieldName = "Down";
      colDown.Name = "colDown";
      colUp.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUp.AppearanceCell.Font");
      colUp.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUp.AppearanceCell.GradientMode");
      colUp.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUp.AppearanceCell.Image");
      colUp.AppearanceCell.Options.UseFont = true;
      colUp.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUp.AppearanceHeader.Font");
      colUp.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUp.AppearanceHeader.GradientMode");
      colUp.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUp.AppearanceHeader.Image");
      colUp.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colUp, "colUp");
      colUp.FieldName = "Up";
      colUp.Name = "colUp";
      colChangeFlag.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colChangeFlag.AppearanceCell.Font");
      colChangeFlag.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colChangeFlag.AppearanceCell.GradientMode");
      colChangeFlag.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colChangeFlag.AppearanceCell.Image");
      colChangeFlag.AppearanceCell.Options.UseFont = true;
      colChangeFlag.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colChangeFlag.AppearanceHeader.Font");
      colChangeFlag.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colChangeFlag.AppearanceHeader.GradientMode");
      colChangeFlag.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colChangeFlag.AppearanceHeader.Image");
      colChangeFlag.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colChangeFlag, "colChangeFlag");
      colChangeFlag.FieldName = "lChangeFlag";
      colChangeFlag.Name = "colChangeFlag";
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(progressBar1);
      Controls.Add(groupControl1);
      Controls.Add(btCancel);
      Controls.Add(btSave);
      FormBorderStyle = FormBorderStyle.None;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmCommonSettings";
      WindowState = FormWindowState.Maximized;
      FormClosing += frmCommonSettings_FormClosing;
      Load += frmCommonSettings_Load;
      Resize += frmCommonSettings_Resize;
      progressBar1.Properties.EndInit();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      groupControl1.PerformLayout();
      gridControl1.EndInit();
      gridView1.EndInit();
      ResumeLayout(false);
    }
  }
}
