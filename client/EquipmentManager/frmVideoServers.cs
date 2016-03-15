// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmVideoServers
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
  public class FrmVideoServers : XtraForm
  {
    private DataTable _dtMain = new DataTable();
    public bool ListFlag = false;
    public BcVideoServer MainServer = new BcVideoServer();
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

    public FrmVideoServers()
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
            dataRow1["ServerState"] = !string.Equals(dataRow2["ServerState"].ToString(), "Не работает", StringComparison.CurrentCultureIgnoreCase) ? (object) Messages.Available : (object) Messages.Unavailable;
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
            this.Invoke((Delegate) new FrmVideoServers.RefreshStateFunc(this.RefreshState), (object) managmentServerClient.GetVideoServerState());
            Thread.Sleep(5000);
          }
          try
          {
            managmentServerClient.Abort();
          }
          catch (Exception ex)
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
      sqlCommand.CommandText = "Select ID,Number,Name,Comment,IP,Port, CreateDate, ModifiedDate, '" + Messages.Unavailable + "' as ServerState,cast('false' as bit)as Changed\r\nfrom VideoServers";
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

    private void frmVideoServers_Load(object sender, EventArgs e)
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
      this.tbNumber.EditValue = (object) (BcVideoServer.LoadMaxNumber() + 1);
      this.tbPort.EditValue = (object) BcVideoServer.LoadMaxPort();
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
      this._dtMain.Rows.Add((object) Guid.Empty, (object) num1, (object) (Messages.VideoServer + (object) num1), (object) "", (object) "0.0.0.0", (object) num2, (object) DateTime.Now, (object) DateTime.Now, (object) Messages.Unavailable, (object) true);
    }

    private void btSave_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gvServers.IsEditorFocused)
        this.gvServers.SetFocusedValue(this.gvServers.EditingValue);
      this.gvServers.SetFocusedRowModified();
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._dtMain.Rows)
      {
        if ((bool) dataRow["Changed"])
        {
          BcVideoServer bcVideoServer = new BcVideoServer();
          bcVideoServer.Id = (Guid) dataRow["ID"];
          bcVideoServer.Ip = dataRow["IP"].ToString();
          bcVideoServer.Port = (int) dataRow["Port"];
          bcVideoServer.Name = dataRow["Name"].ToString();
          bcVideoServer.Comment = dataRow["Comment"].ToString();
          bcVideoServer.Number = (int) dataRow["Number"];
          bcVideoServer.Save();
          dataRow["ID"] = (object) bcVideoServer.Id;
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
        new BcVideoServer()
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
      this.MainServer = new BcVideoServer();
      this.MainServer.Id = (Guid) dataRow["ID"];
      this.MainServer.Ip = dataRow["IP"].ToString();
      this.MainServer.Port = (int) dataRow["Port"];
      this.MainServer.Name = dataRow["Name"].ToString();
      this.MainServer.Comment = dataRow["Comment"].ToString();
      this.MainServer.Number = (int) dataRow["Number"];
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
      e.Appearance.BackColor = Color.MistyRose;
      e.Appearance.BackColor2 = Color.MistyRose;
    }

    private void gvServers_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
      if (this._loading || e.Column == this.colChanged || e.Column == this.colServerState)
        return;
      this.gvServers.GetDataRow(e.RowHandle)["Changed"] = (object) true;
    }

    private void frmVideoServers_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        this.MainThread.Abort();
      }
      catch (Exception ex)
      {
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmVideoServers));
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
      this.gvServers.Columns.AddRange(new GridColumn[10]
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
        this.colChanged
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
      componentResourceManager.ApplyResources((object) this.colChanged, "colChanged");
      this.colChanged.FieldName = "Changed";
      this.colChanged.Name = "colChanged";
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
      this.Name = "FrmVideoServers";
      this.FormClosing += new FormClosingEventHandler(this.frmVideoServers_FormClosing);
      this.Load += new EventHandler(this.frmVideoServers_Load);
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
