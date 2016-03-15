// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmDevices
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using BasicComponents;
using CS.DAL;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CascadeFlowClient
{
  public class FrmDevices : XtraForm
  {
    private DataTable _mainTable = new DataTable();
    private List<BcObjects> _objects = new List<BcObjects>();
    private List<BcDevices> _devices = new List<BcDevices>();
    public List<BcDevices> ActiveDevices = new List<BcDevices>();
    private IContainer components = (IContainer) null;
    private bool _loading;
    private SimpleButton btAccept;
    private SimpleButton btCancel;
    private GridColumn colID;
    private GridColumn colSelect;
    private RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private GridColumn colName;
    private GridColumn colIP;
    private GridColumn colPosition;
    private GridColumn colComment;
    private GridColumn colState;
    private GridControl gridControl2;
    private GridView gridView2;
    private GridColumn gridColumn1;
    private RepositoryItemCheckEdit repositoryItemCheckEdit2;

    public FrmDevices()
    {
      this.InitializeComponent();
      this._objects = BcObjects.LoadAll();
    }

    public FrmDevices(List<BcDevices> devs)
    {
      this.InitializeComponent();
      try
      {
        this._objects = BcObjects.LoadAll();
        BcDevices[] array = new BcDevices[devs.Count];
        devs.CopyTo(array, 0);
        this.ActiveDevices.AddRange((IEnumerable<BcDevices>) array);
        FrmImages.SqlServerState = true;
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        FrmImages.SqlServerState = false;
      }
    }

    private void RefreshForm()
    {
      this._loading = true;
      this._mainTable.Rows.Clear();
      try
      {
        this._devices = BcDevicesStorageExtensions.LoadAllByWorkStationId(FrmImages.CurrentStation.Id);
        foreach (BcDevices bcDevices1 in this._devices)
        {
          string str = Messages.Active;
          if (!bcDevices1.IsActive)
            str = Messages.NoActive;
          bool flag = false;
          foreach (BcDevices bcDevices2 in this.ActiveDevices)
          {
            if (bcDevices2.Id == bcDevices1.Id)
            {
              flag = true;
              break;
            }
          }
          this._mainTable.Rows.Add((object) bcDevices1.Id, (object) (bool) (flag ? 1 : 0), (object) bcDevices1.Name, (object) bcDevices1.ConnectionString, (object) "", (object) str);
          foreach (BcObjects bcObjects in this._objects)
          {
            if (bcObjects.Id == bcDevices1.ObjectId)
            {
              bcObjects.GetData();
              if (bcDevices1.TableId != Guid.Empty)
                this._mainTable.Rows[this._mainTable.Rows.Count - 1]["Comment"] = (object) (bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, bcDevices1.TableId).Name);
            }
          }
        }
        this.gridControl2.DataSource = (object) this._mainTable;
        FrmImages.SqlServerState = true;
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        FrmImages.SqlServerState = false;
      }
      this._loading = false;
    }

    private void frnDevices_Load(object sender, EventArgs e)
    {
      this._mainTable = new DataTable();
      this._mainTable.Columns.AddRange(new DataColumn[6]
      {
        new DataColumn("ID", typeof (Guid)),
        new DataColumn("Select", typeof (bool)),
        new DataColumn("Name"),
        new DataColumn("ConnectionString"),
        new DataColumn("Comment"),
        new DataColumn("State")
      });
      this.RefreshForm();
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      this.ActiveDevices.Clear();
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._mainTable.Rows)
      {
        try
        {
          if ((bool) dataRow[1])
          {
            foreach (BcDevices bcDevices in this._devices)
            {
              if ((Guid) dataRow[0] == bcDevices.Id)
                this.ActiveDevices.Add(bcDevices);
            }
          }
        }
        catch
        {
        }
      }
    }

    private bool CheckGrid()
    {
      int num = 0;
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._mainTable.Rows)
      {
        if (Convert.ToBoolean(dataRow[1]))
          ++num;
      }
      return num >= 4;
    }

    private void gridView2_CellValueChanging(object sender, CellValueChangedEventArgs e)
    {
      if (this._loading)
        return;
      if (e.Column.FieldName == "Select")
      {
        try
        {
          if (Convert.ToBoolean(e.Value) && this.CheckGrid())
          {
            this.gridView2.GetDataRow(e.RowHandle)[1] = (object) false;
            int num = (int) XtraMessageBox.Show(Messages.ChooseUpToFourDevices, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
            this.gridView2.GetDataRow(e.RowHandle)[1] = e.Value;
        }
        catch
        {
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmDevices));
      this.repositoryItemCheckEdit2 = new RepositoryItemCheckEdit();
      this.btAccept = new SimpleButton();
      this.btCancel = new SimpleButton();
      this.colID = new GridColumn();
      this.colSelect = new GridColumn();
      this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      this.colName = new GridColumn();
      this.colIP = new GridColumn();
      this.colPosition = new GridColumn();
      this.colComment = new GridColumn();
      this.colState = new GridColumn();
      this.gridControl2 = new GridControl();
      this.gridView2 = new GridView();
      this.gridColumn1 = new GridColumn();
      this.repositoryItemCheckEdit2.BeginInit();
      this.repositoryItemCheckEdit1.BeginInit();
      this.gridControl2.BeginInit();
      this.gridView2.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.repositoryItemCheckEdit2, "repositoryItemCheckEdit2");
      this.repositoryItemCheckEdit2.AllowGrayed = true;
      this.repositoryItemCheckEdit2.AutoWidth = true;
      this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
      this.repositoryItemCheckEdit2.NullStyle = StyleIndeterminate.Unchecked;
      componentResourceManager.ApplyResources((object) this.btAccept, "btAccept");
      this.btAccept.DialogResult = DialogResult.OK;
      this.btAccept.Name = "btAccept";
      this.btAccept.Click += new EventHandler(this.btAccept_Click);
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      componentResourceManager.ApplyResources((object) this.colID, "colID");
      this.colID.DisplayFormat.FormatType = FormatType.Custom;
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colID.OptionsColumn.AllowEdit = false;
      this.colID.OptionsColumn.ReadOnly = true;
      this.colSelect.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colSelect.AppearanceHeader.Font");
      this.colSelect.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colSelect.AppearanceHeader.FontSizeDelta");
      this.colSelect.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colSelect.AppearanceHeader.FontStyleDelta");
      this.colSelect.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colSelect.AppearanceHeader.GradientMode");
      this.colSelect.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colSelect.AppearanceHeader.Image");
      this.colSelect.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colSelect, "colSelect");
      this.colSelect.ColumnEdit = (RepositoryItem) this.repositoryItemCheckEdit2;
      this.colSelect.FieldName = "Select";
      this.colSelect.Name = "colSelect";
      componentResourceManager.ApplyResources((object) this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      this.colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      this.colName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
      this.colName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
      this.colName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
      this.colName.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colName.AppearanceHeader.Image");
      this.colName.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.FieldName = "Name";
      this.colName.Name = "colName";
      this.colIP.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceHeader.Font");
      this.colIP.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colIP.AppearanceHeader.FontSizeDelta");
      this.colIP.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIP.AppearanceHeader.FontStyleDelta");
      this.colIP.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIP.AppearanceHeader.GradientMode");
      this.colIP.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colIP.AppearanceHeader.Image");
      this.colIP.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colIP, "colIP");
      this.colIP.FieldName = "IP";
      this.colIP.Name = "colIP";
      this.colIP.OptionsColumn.AllowEdit = false;
      this.colIP.OptionsColumn.ReadOnly = true;
      this.colPosition.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      this.colPosition.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontSizeDelta");
      this.colPosition.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontStyleDelta");
      this.colPosition.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPosition.AppearanceHeader.GradientMode");
      this.colPosition.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colPosition.AppearanceHeader.Image");
      this.colPosition.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colPosition, "colPosition");
      this.colPosition.FieldName = "Position";
      this.colPosition.Name = "colPosition";
      this.colPosition.OptionsColumn.AllowEdit = false;
      this.colComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceHeader.Font");
      this.colComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colComment.AppearanceHeader.FontSizeDelta");
      this.colComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colComment.AppearanceHeader.FontStyleDelta");
      this.colComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colComment.AppearanceHeader.GradientMode");
      this.colComment.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colComment.AppearanceHeader.Image");
      this.colComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colComment, "colComment");
      this.colComment.FieldName = "Comment";
      this.colComment.Name = "colComment";
      this.colComment.OptionsColumn.AllowEdit = false;
      this.colState.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colState.AppearanceHeader.Font");
      this.colState.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colState.AppearanceHeader.FontSizeDelta");
      this.colState.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colState.AppearanceHeader.FontStyleDelta");
      this.colState.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colState.AppearanceHeader.GradientMode");
      this.colState.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colState.AppearanceHeader.Image");
      this.colState.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colState, "colState");
      this.colState.FieldName = "State";
      this.colState.Name = "colState";
      this.colState.OptionsColumn.AllowEdit = false;
      componentResourceManager.ApplyResources((object) this.gridControl2, "gridControl2");
      this.gridControl2.Cursor = Cursors.Default;
      this.gridControl2.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gridControl2.EmbeddedNavigator.AccessibleDescription");
      this.gridControl2.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gridControl2.EmbeddedNavigator.AccessibleName");
      this.gridControl2.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.AllowHtmlTextInToolTip");
      this.gridControl2.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.Anchor");
      this.gridControl2.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.BackgroundImage");
      this.gridControl2.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.BackgroundImageLayout");
      this.gridControl2.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.ImeMode");
      this.gridControl2.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.MaximumSize");
      this.gridControl2.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.TextLocation");
      this.gridControl2.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gridControl2.EmbeddedNavigator.ToolTip");
      this.gridControl2.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gridControl2.EmbeddedNavigator.ToolTipIconType");
      this.gridControl2.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gridControl2.EmbeddedNavigator.ToolTipTitle");
      this.gridControl2.MainView = (BaseView) this.gridView2;
      this.gridControl2.Name = "gridControl2";
      this.gridControl2.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView2
      });
      componentResourceManager.ApplyResources((object) this.gridView2, "gridView2");
      this.gridView2.Columns.AddRange(new GridColumn[7]
      {
        this.colID,
        this.colSelect,
        this.colName,
        this.colIP,
        this.colPosition,
        this.colComment,
        this.colState
      });
      this.gridView2.GridControl = this.gridControl2;
      this.gridView2.Name = "gridView2";
      this.gridView2.OptionsView.ShowGroupPanel = false;
      this.gridView2.CellValueChanging += new CellValueChangedEventHandler(this.gridView2_CellValueChanging);
      componentResourceManager.ApplyResources((object) this.gridColumn1, "gridColumn1");
      this.gridColumn1.FieldName = "ID";
      this.gridColumn1.Name = "gridColumn1";
      this.AcceptButton = (IButtonControl) this.btAccept;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("frmDevices.Appearance.FontSizeDelta");
      this.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("frmDevices.Appearance.FontStyleDelta");
      this.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("frmDevices.Appearance.GradientMode");
      this.Appearance.Image = (Image) componentResourceManager.GetObject("frmDevices.Appearance.Image");
      this.Appearance.Options.UseFont = true;
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btCancel;
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btAccept);
      this.Controls.Add((Control) this.gridControl2);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmDevices";
      this.Load += new EventHandler(this.frnDevices_Load);
      this.repositoryItemCheckEdit2.EndInit();
      this.repositoryItemCheckEdit1.EndInit();
      this.gridControl2.EndInit();
      this.gridView2.EndInit();
      this.ResumeLayout(false);
    }
  }
}
