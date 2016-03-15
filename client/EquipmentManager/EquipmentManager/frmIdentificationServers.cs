// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmIdentificationcServers
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using BasicComponents.ManagmentServer;
using CascadeEquipment.Properties;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class FrmIdentificationcServers : XtraForm
  {
    private DataTable _dtMain = new DataTable();
    public bool ListFlag = false;
    public BcIdentificationServer MainServer = new BcIdentificationServer();
    private IContainer components = (IContainer) null;
    public Thread MainThread;
    private bool _loading;
    private GridControl gridControl1;
    private GridView gvServers;
    private GridColumn colID;
    private GridColumn colNumber;
    private GridColumn colName;
    private GridColumn colComment;
    private GridColumn colIP;
    private GridColumn colPort;
    private GridColumn colCreateDate;
    private GridColumn colModifiedDate;
    private GridColumn colServerState;
    private RepositoryItemTextEdit tbIP;
    private BarManager barManager1;
    private Bar bar1;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private BarButtonItem btAdd;
    private BarButtonItem btSave;
    private BarButtonItem btDelete;
    private BarEditItem tbPort;
    private RepositoryItemTextEdit repositoryItemTextEdit1;
    private SimpleButton btCancel;
    private SimpleButton btAccept;
    private BarEditItem tbNumber;
    private GridColumn colChanged;
    private GridColumn colTracking;
    private GridColumn colMinScore;
    private GridColumn colUseMinScore;
    private GridColumn colThreadCount;
    private GridColumn colUsePBDCheck;
    private GridColumn colUseFBDCheck;
    private GridColumn colUseErrorCheck;
    private GridColumn colUseOwnScore;
    private GridColumn colSaveNewKeys;
    private GridColumn colSaveNewFaces;
    private GridColumn colTrackingPeriod;
    private GridColumn colMinus;

    public FrmIdentificationcServers()
    {
      this.InitializeComponent();
    }

    private void RefreshState(DataTable dt)
    {
      foreach (DataRow dataRow1 in (InternalDataCollectionBase) this._dtMain.Rows)
      {
        foreach (DataRow dataRow2 in (InternalDataCollectionBase) dt.Rows)
        {
          if (dataRow1["ID"].ToString() == dataRow2["ID"].ToString())
          {
            dataRow1["ServerState"] = !(dataRow2["ServerState"].ToString().ToUpper() == "Не работает".ToUpper()) ? (object) Messages.Available : (object) Messages.Unavailable;
            break;
          }
        }
      }
    }

    public void CheckServices()
    {
      while (!FrmDevices.ClosFlag)
      {
        try
        {
          ManagmentServerClient managmentServerClient = new ManagmentServerClient(new InstanceContext((object) FrmDevices.MainClient));
          managmentServerClient.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
          managmentServerClient.Open();
          while (!FrmDevices.ClosFlag)
          {
            this.Invoke((Delegate) new FrmIdentificationcServers.RefreshStateFunc(this.RefreshState), (object) managmentServerClient.GetIdentificationServersState());
            Thread.Sleep(5000);
          }
          try
          {
            managmentServerClient.Abort();
          }
          catch
          {
          }
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(1000);
      }
    }

    private void ReloadGrid()
    {
      this._loading = true;
      this._dtMain = new DataTable();
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandText = "Select *, '" + Messages.Unavailable + "' as ServerState,cast('false' as bit)as Changed\r\nfrom IdentificationServers";
      sqlCommand.Connection = new SqlConnection(CommonSettings.ConnectionString);
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
        this._dtMain.Columns.Add(sqlDataReader.GetName(ordinal), sqlDataReader.GetFieldType(ordinal));
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        this._dtMain.Rows.Add(values);
      }
      this.gridControl1.DataSource = (object) this._dtMain;
      sqlCommand.Connection.Close();
      this._loading = false;
    }

    private void frmDetectorServers_Load(object sender, EventArgs e)
    {
      this.MainThread = new Thread(new ThreadStart(this.CheckServices))
      {
        IsBackground = true
      };
      this.MainThread.Start();
      if (!this.ListFlag)
      {
        this.btAccept.Visible = false;
        this.btCancel.Visible = false;
      }
      this.ReloadGrid();
      this.tbNumber.EditValue = (object) (BcIdentificationServer.LoadMaxNumber() + 1);
      this.tbPort.EditValue = (object) BcIdentificationServer.LoadMaxPort();
    }

    private void btAdd_ItemClick(object sender, ItemClickEventArgs e)
    {
      int num1 = (int) this.tbNumber.EditValue;
      int num2 = (int) this.tbPort.EditValue;
      for (int index = 0; index < this._dtMain.Rows.Count; ++index)
      {
        if ((int) this._dtMain.Rows[index]["Number"] >= num1)
          num1 = (int) this._dtMain.Rows[index]["Number"] + 1;
      }
      this._dtMain.Rows.Add((object) Guid.Empty, (object) num1, (object) (Messages.IdentificationServer + (object) num1), (object) "", (object) "0.0.0.0", (object) num2, (object) false, (object) 0.53f, (object) false, (object) 4, (object) false, (object) false, (object) false, (object) false, (object) false, (object) false, (object) 300, (object) 3, (object) DateTime.Now, (object) DateTime.Now, (object) "Не работает", (object) true);
    }

    private void btSave_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gvServers.IsEditorFocused)
        this.gvServers.SetFocusedValue(this.gvServers.EditingValue);
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._dtMain.Rows)
      {
        if ((bool) dataRow["Changed"])
        {
          BcIdentificationServer identificationServer = new BcIdentificationServer();
          identificationServer.Id = (Guid) dataRow["ID"];
          identificationServer.Ip = dataRow["IP"].ToString();
          identificationServer.Port = (int) dataRow["Port"];
          identificationServer.Name = dataRow["Name"].ToString();
          identificationServer.Comment = dataRow["Comment"].ToString();
          identificationServer.Number = (int) dataRow["Number"];
          identificationServer.Number = (int) dataRow["Number"];
          identificationServer.Tracking = (bool) dataRow["Tracking"];
          identificationServer.MinScore = Convert.ToSingle(dataRow["MinScore"]);
          identificationServer.UseMinScore = (bool) dataRow["UseMinScore"];
          identificationServer.ThreadCount = (int) dataRow["ThreadCount"];
          identificationServer.UsePbdCheck = (bool) dataRow["UsePBDCheck"];
          identificationServer.UseFbdCheck = (bool) dataRow["UseFBDCheck"];
          identificationServer.UseErrorCheck = (bool) dataRow["UseErrorCheck"];
          identificationServer.UseOwnScore = (bool) dataRow["UseOwnScore"];
          identificationServer.SaveNewKeys = (bool) dataRow["SaveNewKeys"];
          identificationServer.SaveNewFaces = (bool) dataRow["SaveNewFaces"];
          identificationServer.TrackingPeriod = (int) dataRow["TrackingPeriod"];
          identificationServer.Minus = (int) dataRow["Minus"];
          identificationServer.Save();
          dataRow["ID"] = (object) identificationServer.Id;
          dataRow["Changed"] = (object) false;
        }
      }
    }

    private void btDelete_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gvServers.SelectedRowsCount <= 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      foreach (int rowHandle in this.gvServers.GetSelectedRows())
      {
        DataRow dataRow = this.gvServers.GetDataRow(rowHandle);
        new BcIdentificationServer()
        {
          Id = ((Guid) dataRow["ID"])
        }.Delete();
      }
      this.gvServers.DeleteSelectedRows();
    }

    private void tbPort_ItemClick(object sender, ItemClickEventArgs e)
    {
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      if (this.gvServers.FocusedRowHandle < 0)
        return;
      DataRow dataRow = this.gvServers.GetDataRow(this.gvServers.FocusedRowHandle);
      this.MainServer = new BcIdentificationServer();
      this.MainServer.Id = (Guid) dataRow["ID"];
      this.MainServer.Ip = dataRow["IP"].ToString();
      this.MainServer.Port = (int) dataRow["Port"];
      this.MainServer.Name = dataRow["Name"].ToString();
      this.MainServer.Comment = dataRow["Comment"].ToString();
      this.MainServer.Number = (int) dataRow["Number"];
      this.MainServer.Tracking = (bool) dataRow["Tracking"];
      this.MainServer.MinScore = Convert.ToSingle(dataRow["MinScore"]);
      this.MainServer.UseMinScore = (bool) dataRow["UseMinScore"];
      this.MainServer.ThreadCount = (int) dataRow["ThreadCount"];
      this.MainServer.UsePbdCheck = (bool) dataRow["UsePBDCheck"];
      this.MainServer.UseFbdCheck = (bool) dataRow["UseFBDCheck"];
      this.MainServer.UseErrorCheck = (bool) dataRow["UseErrorCheck"];
      this.MainServer.UseOwnScore = (bool) dataRow["UseOwnScore"];
      this.MainServer.SaveNewKeys = (bool) dataRow["SaveNewKeys"];
      this.MainServer.SaveNewFaces = (bool) dataRow["SaveNewFaces"];
      this.MainServer.TrackingPeriod = (int) dataRow["TrackingPeriod"];
      this.MainServer.Minus = (int) dataRow["Minus"];
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void gvServers_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (e.RowHandle < 0)
        return;
      e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    private void gvServers_RowStyle(object sender, RowStyleEventArgs e)
    {
      if (e.RowHandle < 0 || !(bool) this.gvServers.GetDataRow(e.RowHandle)["Changed"])
        return;
      e.Appearance.BackColor = Color.White;
      e.Appearance.BackColor2 = Color.MistyRose;
    }

    private void gvServers_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
      if (this._loading || e.Column == this.colChanged || e.Column == this.colServerState)
        return;
      this.gvServers.GetDataRow(e.RowHandle)["Changed"] = (object) true;
    }

    private void frmDetectorServers_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        this.MainThread.Abort();
      }
      catch
      {
      }
    }

    private void gvServers_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
    {
    }

    private void gvServers_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
      if (e.Column != this.colServerState || !(e.CellValue.ToString() == Messages.Unavailable))
        return;
      e.Appearance.BackColor = Color.Red;
      e.Appearance.BackColor2 = Color.Red;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmIdentificationcServers));
      this.gridControl1 = new GridControl();
      this.gvServers = new GridView();
      this.colID = new GridColumn();
      this.colNumber = new GridColumn();
      this.colName = new GridColumn();
      this.colComment = new GridColumn();
      this.colIP = new GridColumn();
      this.tbIP = new RepositoryItemTextEdit();
      this.colPort = new GridColumn();
      this.colCreateDate = new GridColumn();
      this.colModifiedDate = new GridColumn();
      this.colServerState = new GridColumn();
      this.colChanged = new GridColumn();
      this.colTracking = new GridColumn();
      this.colMinScore = new GridColumn();
      this.colUseMinScore = new GridColumn();
      this.colThreadCount = new GridColumn();
      this.colUsePBDCheck = new GridColumn();
      this.colUseFBDCheck = new GridColumn();
      this.colUseErrorCheck = new GridColumn();
      this.colUseOwnScore = new GridColumn();
      this.colSaveNewKeys = new GridColumn();
      this.colSaveNewFaces = new GridColumn();
      this.colTrackingPeriod = new GridColumn();
      this.colMinus = new GridColumn();
      this.barManager1 = new BarManager();
      this.bar1 = new Bar();
      this.btAdd = new BarButtonItem();
      this.btSave = new BarButtonItem();
      this.btDelete = new BarButtonItem();
      this.tbPort = new BarEditItem();
      this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
      this.tbNumber = new BarEditItem();
      this.barDockControlTop = new BarDockControl();
      this.barDockControlBottom = new BarDockControl();
      this.barDockControlLeft = new BarDockControl();
      this.barDockControlRight = new BarDockControl();
      this.btAccept = new SimpleButton();
      this.btCancel = new SimpleButton();
      this.gridControl1.BeginInit();
      this.gvServers.BeginInit();
      this.tbIP.BeginInit();
      this.barManager1.BeginInit();
      this.repositoryItemTextEdit1.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.gridControl1, "gridControl1");
      this.gridControl1.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleDescription");
      this.gridControl1.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleName");
      this.gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip");
      this.gridControl1.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.Anchor");
      this.gridControl1.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImage");
      this.gridControl1.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImageLayout");
      this.gridControl1.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ImeMode");
      this.gridControl1.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.MaximumSize");
      this.gridControl1.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.TextLocation");
      this.gridControl1.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTip");
      this.gridControl1.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ToolTipIconType");
      this.gridControl1.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTipTitle");
      this.gridControl1.MainView = (BaseView) this.gvServers;
      this.gridControl1.Name = "gridControl1";
      this.gridControl1.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.tbIP
      });
      this.gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gvServers
      });
      componentResourceManager.ApplyResources((object) this.gvServers, "gvServers");
      this.gvServers.ColumnPanelRowHeight = 40;
      this.gvServers.Columns.AddRange(new GridColumn[22]
      {
        this.colID,
        this.colNumber,
        this.colName,
        this.colComment,
        this.colIP,
        this.colPort,
        this.colCreateDate,
        this.colModifiedDate,
        this.colServerState,
        this.colChanged,
        this.colTracking,
        this.colMinScore,
        this.colUseMinScore,
        this.colThreadCount,
        this.colUsePBDCheck,
        this.colUseFBDCheck,
        this.colUseErrorCheck,
        this.colUseOwnScore,
        this.colSaveNewKeys,
        this.colSaveNewFaces,
        this.colTrackingPeriod,
        this.colMinus
      });
      this.gvServers.GridControl = this.gridControl1;
      this.gvServers.IndicatorWidth = 50;
      this.gvServers.Name = "gvServers";
      this.gvServers.OptionsCustomization.AllowGroup = false;
      this.gvServers.OptionsFind.AlwaysVisible = true;
      this.gvServers.OptionsView.ColumnAutoWidth = false;
      this.gvServers.OptionsView.ShowGroupPanel = false;
      this.gvServers.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(this.gvServers_CustomDrawRowIndicator);
      this.gvServers.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvServers_CustomDrawCell);
      this.gvServers.CustomDrawFooterCell += new FooterCellCustomDrawEventHandler(this.gvServers_CustomDrawFooterCell);
      this.gvServers.RowStyle += new RowStyleEventHandler(this.gvServers_RowStyle);
      this.gvServers.CellValueChanged += new CellValueChangedEventHandler(this.gvServers_CellValueChanged);
      this.colID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      this.colID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceCell.FontSizeDelta");
      this.colID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceCell.FontStyleDelta");
      this.colID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceCell.GradientMode");
      this.colID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colID.AppearanceCell.Image");
      this.colID.AppearanceCell.Options.UseFont = true;
      this.colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      this.colID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceHeader.FontSizeDelta");
      this.colID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceHeader.FontStyleDelta");
      this.colID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceHeader.GradientMode");
      this.colID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colID.AppearanceHeader.Image");
      this.colID.AppearanceHeader.Options.UseFont = true;
      this.colID.AppearanceHeader.Options.UseTextOptions = true;
      this.colID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colID, "colID");
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colNumber.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colNumber.AppearanceCell.Font");
      this.colNumber.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colNumber.AppearanceCell.FontSizeDelta");
      this.colNumber.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colNumber.AppearanceCell.FontStyleDelta");
      this.colNumber.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colNumber.AppearanceCell.GradientMode");
      this.colNumber.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colNumber.AppearanceCell.Image");
      this.colNumber.AppearanceCell.Options.UseFont = true;
      this.colNumber.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colNumber.AppearanceHeader.Font");
      this.colNumber.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colNumber.AppearanceHeader.FontSizeDelta");
      this.colNumber.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colNumber.AppearanceHeader.FontStyleDelta");
      this.colNumber.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colNumber.AppearanceHeader.GradientMode");
      this.colNumber.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colNumber.AppearanceHeader.Image");
      this.colNumber.AppearanceHeader.Options.UseFont = true;
      this.colNumber.AppearanceHeader.Options.UseTextOptions = true;
      this.colNumber.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colNumber.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colNumber, "colNumber");
      this.colNumber.FieldName = "Number";
      this.colNumber.Name = "colNumber";
      this.colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      this.colName.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceCell.FontSizeDelta");
      this.colName.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceCell.FontStyleDelta");
      this.colName.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceCell.GradientMode");
      this.colName.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colName.AppearanceCell.Image");
      this.colName.AppearanceCell.Options.UseFont = true;
      this.colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      this.colName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
      this.colName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
      this.colName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
      this.colName.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colName.AppearanceHeader.Image");
      this.colName.AppearanceHeader.Options.UseFont = true;
      this.colName.AppearanceHeader.Options.UseTextOptions = true;
      this.colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.FieldName = "Name";
      this.colName.Name = "colName";
      this.colComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceCell.Font");
      this.colComment.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colComment.AppearanceCell.FontSizeDelta");
      this.colComment.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colComment.AppearanceCell.FontStyleDelta");
      this.colComment.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colComment.AppearanceCell.GradientMode");
      this.colComment.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colComment.AppearanceCell.Image");
      this.colComment.AppearanceCell.Options.UseFont = true;
      this.colComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceHeader.Font");
      this.colComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colComment.AppearanceHeader.FontSizeDelta");
      this.colComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colComment.AppearanceHeader.FontStyleDelta");
      this.colComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colComment.AppearanceHeader.GradientMode");
      this.colComment.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colComment.AppearanceHeader.Image");
      this.colComment.AppearanceHeader.Options.UseFont = true;
      this.colComment.AppearanceHeader.Options.UseTextOptions = true;
      this.colComment.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colComment.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colComment, "colComment");
      this.colComment.FieldName = "Comment";
      this.colComment.Name = "colComment";
      this.colIP.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceCell.Font");
      this.colIP.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colIP.AppearanceCell.FontSizeDelta");
      this.colIP.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIP.AppearanceCell.FontStyleDelta");
      this.colIP.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIP.AppearanceCell.GradientMode");
      this.colIP.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colIP.AppearanceCell.Image");
      this.colIP.AppearanceCell.Options.UseFont = true;
      this.colIP.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceHeader.Font");
      this.colIP.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colIP.AppearanceHeader.FontSizeDelta");
      this.colIP.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIP.AppearanceHeader.FontStyleDelta");
      this.colIP.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIP.AppearanceHeader.GradientMode");
      this.colIP.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colIP.AppearanceHeader.Image");
      this.colIP.AppearanceHeader.Options.UseFont = true;
      this.colIP.AppearanceHeader.Options.UseTextOptions = true;
      this.colIP.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colIP.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colIP, "colIP");
      this.colIP.ColumnEdit = (RepositoryItem) this.tbIP;
      this.colIP.FieldName = "IP";
      this.colIP.Name = "colIP";
      componentResourceManager.ApplyResources((object) this.tbIP, "tbIP");
      this.tbIP.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbIP.Mask.AutoComplete");
      this.tbIP.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbIP.Mask.BeepOnError");
      this.tbIP.Mask.EditMask = componentResourceManager.GetString("tbIP.Mask.EditMask");
      this.tbIP.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbIP.Mask.IgnoreMaskBlank");
      this.tbIP.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbIP.Mask.MaskType");
      this.tbIP.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbIP.Mask.PlaceHolder");
      this.tbIP.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbIP.Mask.SaveLiteral");
      this.tbIP.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbIP.Mask.ShowPlaceHolders");
      this.tbIP.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbIP.Mask.UseMaskAsDisplayFormat");
      this.tbIP.Name = "tbIP";
      this.colPort.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPort.AppearanceCell.Font");
      this.colPort.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colPort.AppearanceCell.FontSizeDelta");
      this.colPort.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPort.AppearanceCell.FontStyleDelta");
      this.colPort.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPort.AppearanceCell.GradientMode");
      this.colPort.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colPort.AppearanceCell.Image");
      this.colPort.AppearanceCell.Options.UseFont = true;
      this.colPort.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPort.AppearanceHeader.Font");
      this.colPort.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colPort.AppearanceHeader.FontSizeDelta");
      this.colPort.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPort.AppearanceHeader.FontStyleDelta");
      this.colPort.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPort.AppearanceHeader.GradientMode");
      this.colPort.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colPort.AppearanceHeader.Image");
      this.colPort.AppearanceHeader.Options.UseFont = true;
      this.colPort.AppearanceHeader.Options.UseTextOptions = true;
      this.colPort.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colPort.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colPort, "colPort");
      this.colPort.FieldName = "Port";
      this.colPort.Name = "colPort";
      this.colCreateDate.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colCreateDate.AppearanceCell.Font");
      this.colCreateDate.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colCreateDate.AppearanceCell.FontSizeDelta");
      this.colCreateDate.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCreateDate.AppearanceCell.FontStyleDelta");
      this.colCreateDate.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCreateDate.AppearanceCell.GradientMode");
      this.colCreateDate.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colCreateDate.AppearanceCell.Image");
      this.colCreateDate.AppearanceCell.Options.UseFont = true;
      this.colCreateDate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colCreateDate.AppearanceHeader.Font");
      this.colCreateDate.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colCreateDate.AppearanceHeader.FontSizeDelta");
      this.colCreateDate.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCreateDate.AppearanceHeader.FontStyleDelta");
      this.colCreateDate.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCreateDate.AppearanceHeader.GradientMode");
      this.colCreateDate.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colCreateDate.AppearanceHeader.Image");
      this.colCreateDate.AppearanceHeader.Options.UseFont = true;
      this.colCreateDate.AppearanceHeader.Options.UseTextOptions = true;
      this.colCreateDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colCreateDate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colCreateDate, "colCreateDate");
      this.colCreateDate.FieldName = "CreateDate";
      this.colCreateDate.Name = "colCreateDate";
      this.colModifiedDate.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colModifiedDate.AppearanceCell.Font");
      this.colModifiedDate.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colModifiedDate.AppearanceCell.FontSizeDelta");
      this.colModifiedDate.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colModifiedDate.AppearanceCell.FontStyleDelta");
      this.colModifiedDate.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colModifiedDate.AppearanceCell.GradientMode");
      this.colModifiedDate.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colModifiedDate.AppearanceCell.Image");
      this.colModifiedDate.AppearanceCell.Options.UseFont = true;
      this.colModifiedDate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.Font");
      this.colModifiedDate.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.FontSizeDelta");
      this.colModifiedDate.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.FontStyleDelta");
      this.colModifiedDate.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.GradientMode");
      this.colModifiedDate.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.Image");
      this.colModifiedDate.AppearanceHeader.Options.UseFont = true;
      this.colModifiedDate.AppearanceHeader.Options.UseTextOptions = true;
      this.colModifiedDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colModifiedDate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colModifiedDate, "colModifiedDate");
      this.colModifiedDate.FieldName = "ModifiedDate";
      this.colModifiedDate.Name = "colModifiedDate";
      this.colServerState.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colServerState.AppearanceCell.Font");
      this.colServerState.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colServerState.AppearanceCell.FontSizeDelta");
      this.colServerState.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colServerState.AppearanceCell.FontStyleDelta");
      this.colServerState.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colServerState.AppearanceCell.GradientMode");
      this.colServerState.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colServerState.AppearanceCell.Image");
      this.colServerState.AppearanceCell.Options.UseFont = true;
      this.colServerState.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colServerState.AppearanceHeader.Font");
      this.colServerState.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colServerState.AppearanceHeader.FontSizeDelta");
      this.colServerState.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colServerState.AppearanceHeader.FontStyleDelta");
      this.colServerState.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colServerState.AppearanceHeader.GradientMode");
      this.colServerState.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colServerState.AppearanceHeader.Image");
      this.colServerState.AppearanceHeader.Options.UseFont = true;
      this.colServerState.AppearanceHeader.Options.UseTextOptions = true;
      this.colServerState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colServerState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colServerState, "colServerState");
      this.colServerState.FieldName = "ServerState";
      this.colServerState.Name = "colServerState";
      this.colServerState.OptionsColumn.AllowEdit = false;
      this.colServerState.OptionsColumn.ReadOnly = true;
      this.colChanged.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colChanged.AppearanceCell.Font");
      this.colChanged.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colChanged.AppearanceCell.FontSizeDelta");
      this.colChanged.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colChanged.AppearanceCell.FontStyleDelta");
      this.colChanged.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colChanged.AppearanceCell.GradientMode");
      this.colChanged.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colChanged.AppearanceCell.Image");
      this.colChanged.AppearanceCell.Options.UseFont = true;
      this.colChanged.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colChanged.AppearanceHeader.Font");
      this.colChanged.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colChanged.AppearanceHeader.FontSizeDelta");
      this.colChanged.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colChanged.AppearanceHeader.FontStyleDelta");
      this.colChanged.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colChanged.AppearanceHeader.GradientMode");
      this.colChanged.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colChanged.AppearanceHeader.Image");
      this.colChanged.AppearanceHeader.Options.UseFont = true;
      this.colChanged.AppearanceHeader.Options.UseTextOptions = true;
      this.colChanged.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colChanged.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colChanged, "colChanged");
      this.colChanged.FieldName = "Changed";
      this.colChanged.Name = "colChanged";
      this.colTracking.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colTracking.AppearanceCell.Font");
      this.colTracking.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colTracking.AppearanceCell.FontSizeDelta");
      this.colTracking.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colTracking.AppearanceCell.FontStyleDelta");
      this.colTracking.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colTracking.AppearanceCell.GradientMode");
      this.colTracking.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colTracking.AppearanceCell.Image");
      this.colTracking.AppearanceCell.Options.UseFont = true;
      this.colTracking.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colTracking.AppearanceHeader.Font");
      this.colTracking.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colTracking.AppearanceHeader.FontSizeDelta");
      this.colTracking.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colTracking.AppearanceHeader.FontStyleDelta");
      this.colTracking.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colTracking.AppearanceHeader.GradientMode");
      this.colTracking.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colTracking.AppearanceHeader.Image");
      this.colTracking.AppearanceHeader.Options.UseFont = true;
      this.colTracking.AppearanceHeader.Options.UseTextOptions = true;
      this.colTracking.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colTracking.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colTracking, "colTracking");
      this.colTracking.FieldName = "Tracking";
      this.colTracking.Name = "colTracking";
      this.colMinScore.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colMinScore.AppearanceCell.Font");
      this.colMinScore.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colMinScore.AppearanceCell.FontSizeDelta");
      this.colMinScore.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colMinScore.AppearanceCell.FontStyleDelta");
      this.colMinScore.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colMinScore.AppearanceCell.GradientMode");
      this.colMinScore.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colMinScore.AppearanceCell.Image");
      this.colMinScore.AppearanceCell.Options.UseFont = true;
      this.colMinScore.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colMinScore.AppearanceHeader.Font");
      this.colMinScore.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colMinScore.AppearanceHeader.FontSizeDelta");
      this.colMinScore.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colMinScore.AppearanceHeader.FontStyleDelta");
      this.colMinScore.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colMinScore.AppearanceHeader.GradientMode");
      this.colMinScore.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colMinScore.AppearanceHeader.Image");
      this.colMinScore.AppearanceHeader.Options.UseFont = true;
      this.colMinScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colMinScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colMinScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colMinScore, "colMinScore");
      this.colMinScore.FieldName = "MinScore";
      this.colMinScore.Name = "colMinScore";
      this.colUseMinScore.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUseMinScore.AppearanceCell.Font");
      this.colUseMinScore.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colUseMinScore.AppearanceCell.FontSizeDelta");
      this.colUseMinScore.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseMinScore.AppearanceCell.FontStyleDelta");
      this.colUseMinScore.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseMinScore.AppearanceCell.GradientMode");
      this.colUseMinScore.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUseMinScore.AppearanceCell.Image");
      this.colUseMinScore.AppearanceCell.Options.UseFont = true;
      this.colUseMinScore.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUseMinScore.AppearanceHeader.Font");
      this.colUseMinScore.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colUseMinScore.AppearanceHeader.FontSizeDelta");
      this.colUseMinScore.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseMinScore.AppearanceHeader.FontStyleDelta");
      this.colUseMinScore.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseMinScore.AppearanceHeader.GradientMode");
      this.colUseMinScore.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUseMinScore.AppearanceHeader.Image");
      this.colUseMinScore.AppearanceHeader.Options.UseFont = true;
      this.colUseMinScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colUseMinScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colUseMinScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colUseMinScore, "colUseMinScore");
      this.colUseMinScore.FieldName = "UseMinScore";
      this.colUseMinScore.Name = "colUseMinScore";
      this.colThreadCount.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colThreadCount.AppearanceCell.Font");
      this.colThreadCount.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colThreadCount.AppearanceCell.FontSizeDelta");
      this.colThreadCount.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colThreadCount.AppearanceCell.FontStyleDelta");
      this.colThreadCount.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colThreadCount.AppearanceCell.GradientMode");
      this.colThreadCount.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colThreadCount.AppearanceCell.Image");
      this.colThreadCount.AppearanceCell.Options.UseFont = true;
      this.colThreadCount.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colThreadCount.AppearanceHeader.Font");
      this.colThreadCount.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colThreadCount.AppearanceHeader.FontSizeDelta");
      this.colThreadCount.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colThreadCount.AppearanceHeader.FontStyleDelta");
      this.colThreadCount.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colThreadCount.AppearanceHeader.GradientMode");
      this.colThreadCount.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colThreadCount.AppearanceHeader.Image");
      this.colThreadCount.AppearanceHeader.Options.UseFont = true;
      this.colThreadCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colThreadCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colThreadCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colThreadCount, "colThreadCount");
      this.colThreadCount.FieldName = "ThreadCount";
      this.colThreadCount.Name = "colThreadCount";
      this.colUsePBDCheck.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUsePBDCheck.AppearanceCell.Font");
      this.colUsePBDCheck.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colUsePBDCheck.AppearanceCell.FontSizeDelta");
      this.colUsePBDCheck.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUsePBDCheck.AppearanceCell.FontStyleDelta");
      this.colUsePBDCheck.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUsePBDCheck.AppearanceCell.GradientMode");
      this.colUsePBDCheck.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUsePBDCheck.AppearanceCell.Image");
      this.colUsePBDCheck.AppearanceCell.Options.UseFont = true;
      this.colUsePBDCheck.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUsePBDCheck.AppearanceHeader.Font");
      this.colUsePBDCheck.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colUsePBDCheck.AppearanceHeader.FontSizeDelta");
      this.colUsePBDCheck.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUsePBDCheck.AppearanceHeader.FontStyleDelta");
      this.colUsePBDCheck.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUsePBDCheck.AppearanceHeader.GradientMode");
      this.colUsePBDCheck.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUsePBDCheck.AppearanceHeader.Image");
      this.colUsePBDCheck.AppearanceHeader.Options.UseFont = true;
      this.colUsePBDCheck.AppearanceHeader.Options.UseTextOptions = true;
      this.colUsePBDCheck.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colUsePBDCheck.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colUsePBDCheck, "colUsePBDCheck");
      this.colUsePBDCheck.FieldName = "UsePBDCheck";
      this.colUsePBDCheck.Name = "colUsePBDCheck";
      this.colUseFBDCheck.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUseFBDCheck.AppearanceCell.Font");
      this.colUseFBDCheck.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colUseFBDCheck.AppearanceCell.FontSizeDelta");
      this.colUseFBDCheck.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseFBDCheck.AppearanceCell.FontStyleDelta");
      this.colUseFBDCheck.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseFBDCheck.AppearanceCell.GradientMode");
      this.colUseFBDCheck.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUseFBDCheck.AppearanceCell.Image");
      this.colUseFBDCheck.AppearanceCell.Options.UseFont = true;
      this.colUseFBDCheck.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUseFBDCheck.AppearanceHeader.Font");
      this.colUseFBDCheck.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colUseFBDCheck.AppearanceHeader.FontSizeDelta");
      this.colUseFBDCheck.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseFBDCheck.AppearanceHeader.FontStyleDelta");
      this.colUseFBDCheck.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseFBDCheck.AppearanceHeader.GradientMode");
      this.colUseFBDCheck.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUseFBDCheck.AppearanceHeader.Image");
      this.colUseFBDCheck.AppearanceHeader.Options.UseFont = true;
      this.colUseFBDCheck.AppearanceHeader.Options.UseTextOptions = true;
      this.colUseFBDCheck.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colUseFBDCheck.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colUseFBDCheck, "colUseFBDCheck");
      this.colUseFBDCheck.FieldName = "UseFBDCheck";
      this.colUseFBDCheck.Name = "colUseFBDCheck";
      this.colUseErrorCheck.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUseErrorCheck.AppearanceCell.Font");
      this.colUseErrorCheck.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colUseErrorCheck.AppearanceCell.FontSizeDelta");
      this.colUseErrorCheck.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseErrorCheck.AppearanceCell.FontStyleDelta");
      this.colUseErrorCheck.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseErrorCheck.AppearanceCell.GradientMode");
      this.colUseErrorCheck.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUseErrorCheck.AppearanceCell.Image");
      this.colUseErrorCheck.AppearanceCell.Options.UseFont = true;
      this.colUseErrorCheck.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUseErrorCheck.AppearanceHeader.Font");
      this.colUseErrorCheck.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colUseErrorCheck.AppearanceHeader.FontSizeDelta");
      this.colUseErrorCheck.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseErrorCheck.AppearanceHeader.FontStyleDelta");
      this.colUseErrorCheck.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseErrorCheck.AppearanceHeader.GradientMode");
      this.colUseErrorCheck.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUseErrorCheck.AppearanceHeader.Image");
      this.colUseErrorCheck.AppearanceHeader.Options.UseFont = true;
      this.colUseErrorCheck.AppearanceHeader.Options.UseTextOptions = true;
      this.colUseErrorCheck.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colUseErrorCheck.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colUseErrorCheck, "colUseErrorCheck");
      this.colUseErrorCheck.FieldName = "UseErrorCheck";
      this.colUseErrorCheck.Name = "colUseErrorCheck";
      this.colUseOwnScore.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colUseOwnScore.AppearanceCell.Font");
      this.colUseOwnScore.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colUseOwnScore.AppearanceCell.FontSizeDelta");
      this.colUseOwnScore.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseOwnScore.AppearanceCell.FontStyleDelta");
      this.colUseOwnScore.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseOwnScore.AppearanceCell.GradientMode");
      this.colUseOwnScore.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colUseOwnScore.AppearanceCell.Image");
      this.colUseOwnScore.AppearanceCell.Options.UseFont = true;
      this.colUseOwnScore.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUseOwnScore.AppearanceHeader.Font");
      this.colUseOwnScore.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colUseOwnScore.AppearanceHeader.FontSizeDelta");
      this.colUseOwnScore.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colUseOwnScore.AppearanceHeader.FontStyleDelta");
      this.colUseOwnScore.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colUseOwnScore.AppearanceHeader.GradientMode");
      this.colUseOwnScore.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colUseOwnScore.AppearanceHeader.Image");
      this.colUseOwnScore.AppearanceHeader.Options.UseFont = true;
      this.colUseOwnScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colUseOwnScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colUseOwnScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colUseOwnScore, "colUseOwnScore");
      this.colUseOwnScore.FieldName = "UseOwnScore";
      this.colUseOwnScore.Name = "colUseOwnScore";
      this.colSaveNewKeys.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colSaveNewKeys.AppearanceCell.Font");
      this.colSaveNewKeys.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colSaveNewKeys.AppearanceCell.FontSizeDelta");
      this.colSaveNewKeys.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colSaveNewKeys.AppearanceCell.FontStyleDelta");
      this.colSaveNewKeys.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colSaveNewKeys.AppearanceCell.GradientMode");
      this.colSaveNewKeys.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colSaveNewKeys.AppearanceCell.Image");
      this.colSaveNewKeys.AppearanceCell.Options.UseFont = true;
      this.colSaveNewKeys.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colSaveNewKeys.AppearanceHeader.Font");
      this.colSaveNewKeys.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colSaveNewKeys.AppearanceHeader.FontSizeDelta");
      this.colSaveNewKeys.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colSaveNewKeys.AppearanceHeader.FontStyleDelta");
      this.colSaveNewKeys.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colSaveNewKeys.AppearanceHeader.GradientMode");
      this.colSaveNewKeys.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colSaveNewKeys.AppearanceHeader.Image");
      this.colSaveNewKeys.AppearanceHeader.Options.UseFont = true;
      this.colSaveNewKeys.AppearanceHeader.Options.UseTextOptions = true;
      this.colSaveNewKeys.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colSaveNewKeys.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colSaveNewKeys, "colSaveNewKeys");
      this.colSaveNewKeys.FieldName = "SaveNewKeys";
      this.colSaveNewKeys.Name = "colSaveNewKeys";
      this.colSaveNewFaces.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colSaveNewFaces.AppearanceCell.Font");
      this.colSaveNewFaces.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colSaveNewFaces.AppearanceCell.FontSizeDelta");
      this.colSaveNewFaces.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colSaveNewFaces.AppearanceCell.FontStyleDelta");
      this.colSaveNewFaces.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colSaveNewFaces.AppearanceCell.GradientMode");
      this.colSaveNewFaces.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colSaveNewFaces.AppearanceCell.Image");
      this.colSaveNewFaces.AppearanceCell.Options.UseFont = true;
      this.colSaveNewFaces.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colSaveNewFaces.AppearanceHeader.Font");
      this.colSaveNewFaces.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colSaveNewFaces.AppearanceHeader.FontSizeDelta");
      this.colSaveNewFaces.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colSaveNewFaces.AppearanceHeader.FontStyleDelta");
      this.colSaveNewFaces.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colSaveNewFaces.AppearanceHeader.GradientMode");
      this.colSaveNewFaces.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colSaveNewFaces.AppearanceHeader.Image");
      this.colSaveNewFaces.AppearanceHeader.Options.UseFont = true;
      this.colSaveNewFaces.AppearanceHeader.Options.UseTextOptions = true;
      this.colSaveNewFaces.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colSaveNewFaces.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colSaveNewFaces, "colSaveNewFaces");
      this.colSaveNewFaces.FieldName = "SaveNewFaces";
      this.colSaveNewFaces.Name = "colSaveNewFaces";
      this.colTrackingPeriod.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colTrackingPeriod.AppearanceCell.Font");
      this.colTrackingPeriod.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colTrackingPeriod.AppearanceCell.FontSizeDelta");
      this.colTrackingPeriod.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colTrackingPeriod.AppearanceCell.FontStyleDelta");
      this.colTrackingPeriod.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colTrackingPeriod.AppearanceCell.GradientMode");
      this.colTrackingPeriod.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colTrackingPeriod.AppearanceCell.Image");
      this.colTrackingPeriod.AppearanceCell.Options.UseFont = true;
      this.colTrackingPeriod.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colTrackingPeriod.AppearanceHeader.Font");
      this.colTrackingPeriod.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colTrackingPeriod.AppearanceHeader.FontSizeDelta");
      this.colTrackingPeriod.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colTrackingPeriod.AppearanceHeader.FontStyleDelta");
      this.colTrackingPeriod.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colTrackingPeriod.AppearanceHeader.GradientMode");
      this.colTrackingPeriod.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colTrackingPeriod.AppearanceHeader.Image");
      this.colTrackingPeriod.AppearanceHeader.Options.UseFont = true;
      this.colTrackingPeriod.AppearanceHeader.Options.UseTextOptions = true;
      this.colTrackingPeriod.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colTrackingPeriod.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colTrackingPeriod, "colTrackingPeriod");
      this.colTrackingPeriod.FieldName = "TrackingPeriod";
      this.colTrackingPeriod.Name = "colTrackingPeriod";
      this.colMinus.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colMinus.AppearanceCell.Font");
      this.colMinus.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colMinus.AppearanceCell.FontSizeDelta");
      this.colMinus.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colMinus.AppearanceCell.FontStyleDelta");
      this.colMinus.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colMinus.AppearanceCell.GradientMode");
      this.colMinus.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colMinus.AppearanceCell.Image");
      this.colMinus.AppearanceCell.Options.UseFont = true;
      this.colMinus.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colMinus.AppearanceHeader.Font");
      this.colMinus.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colMinus.AppearanceHeader.FontSizeDelta");
      this.colMinus.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colMinus.AppearanceHeader.FontStyleDelta");
      this.colMinus.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colMinus.AppearanceHeader.GradientMode");
      this.colMinus.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colMinus.AppearanceHeader.Image");
      this.colMinus.AppearanceHeader.Options.UseFont = true;
      this.colMinus.AppearanceHeader.Options.UseTextOptions = true;
      this.colMinus.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colMinus.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colMinus, "colMinus");
      this.colMinus.FieldName = "Minus";
      this.colMinus.Name = "colMinus";
      this.barManager1.Bars.AddRange(new Bar[1]
      {
        this.bar1
      });
      this.barManager1.DockControls.Add(this.barDockControlTop);
      this.barManager1.DockControls.Add(this.barDockControlBottom);
      this.barManager1.DockControls.Add(this.barDockControlLeft);
      this.barManager1.DockControls.Add(this.barDockControlRight);
      this.barManager1.Form = (Control) this;
      this.barManager1.Items.AddRange(new BarItem[5]
      {
        (BarItem) this.btAdd,
        (BarItem) this.btSave,
        (BarItem) this.btDelete,
        (BarItem) this.tbPort,
        (BarItem) this.tbNumber
      });
      this.barManager1.MaxItemId = 5;
      this.barManager1.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.repositoryItemTextEdit1
      });
      this.bar1.BarName = "Tools";
      this.bar1.DockCol = 0;
      this.bar1.DockRow = 0;
      this.bar1.DockStyle = BarDockStyle.Top;
      this.bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[5]
      {
        new LinkPersistInfo((BarItem) this.btAdd),
        new LinkPersistInfo((BarItem) this.btSave),
        new LinkPersistInfo((BarItem) this.btDelete),
        new LinkPersistInfo((BarItem) this.tbPort),
        new LinkPersistInfo((BarItem) this.tbNumber)
      });
      this.bar1.OptionsBar.AllowQuickCustomization = false;
      componentResourceManager.ApplyResources((object) this.bar1, "bar1");
      componentResourceManager.ApplyResources((object) this.btAdd, "btAdd");
      this.btAdd.Border = BorderStyles.Default;
      this.btAdd.Glyph = (Image) Resources.document_new;
      this.btAdd.Id = 1;
      this.btAdd.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btAdd.ItemAppearance.Normal.Font");
      this.btAdd.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btAdd.ItemAppearance.Normal.FontSizeDelta");
      this.btAdd.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAdd.ItemAppearance.Normal.FontStyleDelta");
      this.btAdd.ItemAppearance.Normal.Options.UseFont = true;
      this.btAdd.Name = "btAdd";
      this.btAdd.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btAdd.ItemClick += new ItemClickEventHandler(this.btAdd_ItemClick);
      componentResourceManager.ApplyResources((object) this.btSave, "btSave");
      this.btSave.Border = BorderStyles.Default;
      this.btSave.Glyph = (Image) Resources._22document_save;
      this.btSave.Id = 2;
      this.btSave.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.Font");
      this.btSave.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.FontSizeDelta");
      this.btSave.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.ItemAppearance.Normal.FontStyleDelta");
      this.btSave.ItemAppearance.Normal.Options.UseFont = true;
      this.btSave.Name = "btSave";
      this.btSave.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btSave.ItemClick += new ItemClickEventHandler(this.btSave_ItemClick);
      componentResourceManager.ApplyResources((object) this.btDelete, "btDelete");
      this.btDelete.Border = BorderStyles.Default;
      this.btDelete.Glyph = (Image) Resources.document_delete22;
      this.btDelete.Id = 3;
      this.btDelete.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btDelete.ItemAppearance.Normal.Font");
      this.btDelete.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("btDelete.ItemAppearance.Normal.FontSizeDelta");
      this.btDelete.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btDelete.ItemAppearance.Normal.FontStyleDelta");
      this.btDelete.ItemAppearance.Normal.Options.UseFont = true;
      this.btDelete.Name = "btDelete";
      this.btDelete.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btDelete.ItemClick += new ItemClickEventHandler(this.btDelete_ItemClick);
      componentResourceManager.ApplyResources((object) this.tbPort, "tbPort");
      this.tbPort.Border = BorderStyles.Default;
      this.tbPort.Edit = (RepositoryItem) this.repositoryItemTextEdit1;
      this.tbPort.EditValue = (object) "0";
      this.tbPort.Id = 4;
      this.tbPort.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("tbPort.ItemAppearance.Normal.Font");
      this.tbPort.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("tbPort.ItemAppearance.Normal.FontSizeDelta");
      this.tbPort.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbPort.ItemAppearance.Normal.FontStyleDelta");
      this.tbPort.ItemAppearance.Normal.Options.UseFont = true;
      this.tbPort.Name = "tbPort";
      this.tbPort.PaintStyle = BarItemPaintStyle.Caption;
      this.tbPort.ItemClick += new ItemClickEventHandler(this.tbPort_ItemClick);
      componentResourceManager.ApplyResources((object) this.repositoryItemTextEdit1, "repositoryItemTextEdit1");
      this.repositoryItemTextEdit1.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.AutoComplete");
      this.repositoryItemTextEdit1.Mask.BeepOnError = (bool) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.BeepOnError");
      this.repositoryItemTextEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemTextEdit1.Mask.EditMask");
      this.repositoryItemTextEdit1.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.IgnoreMaskBlank");
      this.repositoryItemTextEdit1.Mask.MaskType = (MaskType) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.MaskType");
      this.repositoryItemTextEdit1.Mask.PlaceHolder = (char) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.PlaceHolder");
      this.repositoryItemTextEdit1.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.SaveLiteral");
      this.repositoryItemTextEdit1.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.ShowPlaceHolders");
      this.repositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("repositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat");
      this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
      componentResourceManager.ApplyResources((object) this.tbNumber, "tbNumber");
      this.tbNumber.Border = BorderStyles.Default;
      this.tbNumber.Edit = (RepositoryItem) this.repositoryItemTextEdit1;
      this.tbNumber.EditValue = (object) "0";
      this.tbNumber.Id = 4;
      this.tbNumber.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("tbNumber.ItemAppearance.Normal.Font");
      this.tbNumber.ItemAppearance.Normal.FontSizeDelta = (int) componentResourceManager.GetObject("tbNumber.ItemAppearance.Normal.FontSizeDelta");
      this.tbNumber.ItemAppearance.Normal.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbNumber.ItemAppearance.Normal.FontStyleDelta");
      this.tbNumber.ItemAppearance.Normal.Options.UseFont = true;
      this.tbNumber.Name = "tbNumber";
      this.tbNumber.PaintStyle = BarItemPaintStyle.Caption;
      componentResourceManager.ApplyResources((object) this.barDockControlTop, "barDockControlTop");
      this.barDockControlTop.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlTop.Appearance.FontSizeDelta");
      this.barDockControlTop.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlTop.Appearance.FontStyleDelta");
      this.barDockControlTop.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlTop.Appearance.GradientMode");
      this.barDockControlTop.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlTop.Appearance.Image");
      this.barDockControlTop.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlBottom, "barDockControlBottom");
      this.barDockControlBottom.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlBottom.Appearance.FontSizeDelta");
      this.barDockControlBottom.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlBottom.Appearance.FontStyleDelta");
      this.barDockControlBottom.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlBottom.Appearance.GradientMode");
      this.barDockControlBottom.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlBottom.Appearance.Image");
      this.barDockControlBottom.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlLeft, "barDockControlLeft");
      this.barDockControlLeft.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlLeft.Appearance.FontSizeDelta");
      this.barDockControlLeft.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlLeft.Appearance.FontStyleDelta");
      this.barDockControlLeft.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlLeft.Appearance.GradientMode");
      this.barDockControlLeft.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlLeft.Appearance.Image");
      this.barDockControlLeft.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlRight, "barDockControlRight");
      this.barDockControlRight.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("barDockControlRight.Appearance.FontSizeDelta");
      this.barDockControlRight.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("barDockControlRight.Appearance.FontStyleDelta");
      this.barDockControlRight.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("barDockControlRight.Appearance.GradientMode");
      this.barDockControlRight.Appearance.Image = (Image) componentResourceManager.GetObject("barDockControlRight.Appearance.Image");
      this.barDockControlRight.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.btAccept, "btAccept");
      this.btAccept.Appearance.Font = (Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      this.btAccept.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
      this.btAccept.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
      this.btAccept.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
      this.btAccept.Appearance.Image = (Image) componentResourceManager.GetObject("btAccept.Appearance.Image");
      this.btAccept.Appearance.Options.UseFont = true;
      this.btAccept.DialogResult = DialogResult.OK;
      this.btAccept.Name = "btAccept";
      this.btAccept.Click += new EventHandler(this.btAccept_Click);
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      this.btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      this.btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      this.btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      this.btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      this.btCancel.Appearance.Options.UseFont = true;
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btAccept);
      this.Controls.Add((Control) this.gridControl1);
      this.Controls.Add((Control) this.barDockControlLeft);
      this.Controls.Add((Control) this.barDockControlRight);
      this.Controls.Add((Control) this.barDockControlBottom);
      this.Controls.Add((Control) this.barDockControlTop);
      this.Name = "FrmIdentificationcServers";
      this.FormClosing += new FormClosingEventHandler(this.frmDetectorServers_FormClosing);
      this.Load += new EventHandler(this.frmDetectorServers_Load);
      this.gridControl1.EndInit();
      this.gvServers.EndInit();
      this.tbIP.EndInit();
      this.barManager1.EndInit();
      this.repositoryItemTextEdit1.EndInit();
      this.ResumeLayout(false);
    }

    private delegate void RefreshStateFunc(DataTable dt);
  }
}
