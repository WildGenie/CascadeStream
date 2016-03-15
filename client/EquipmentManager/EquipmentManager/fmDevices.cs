// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmDevices
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using BasicComponents.ManagmentServer;
using CascadeEquipment.Properties;
using CommonContracts;
using CS.Client.Common.Abstract;
using CS.Client.Common.Views;
using CS.DAL;
using DevExpress.Utils;
using DevExpress.XtraBars;
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
using System.Data.SqlClient;
using System.Drawing;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class FrmDevices : XtraForm
  {
    public static FrmDevices.ManagmentServerCallback MainClient = new FrmDevices.ManagmentServerCallback();
    private string _selectStr = "Select\r\nDevices.ID,\r\nDevices.Name,\r\nDevices.Number,\r\n'" + Messages.Unavailable + "' as DeviceState,\r\n0 as FrameCount,\r\n0 as DetectionCount,\r\n0 as DetectionFaces,\r\n0 as ExtractCount,\r\n0 as ResultCount,\r\n0 as IdentifierCount,\r\nDetectorCount,\r\nExtractorCount,\r\n'' as ObjectName,\r\nDevices.VSID,\r\nDevices.DSID,\r\nDevices.ESID,\t\r\nDevices.ISID,\t\r\nVideoServers.IP as VideoServer,\r\nDetectorServers.IP as DetectorServer,\r\nExtractorServers.IP as ExtractorServer,\r\nIdentificationServers.IP as IdentificationServer,\r\nDevices.MinFace,\r\nSaveUnidentified,\r\ncast(Devices.DetectorScore as float) as DetectorScore,\r\ncast(Devices.MinScore as float) as MinScore,\r\nDevices.ObjectID,\r\nDevices.TableID,\r\nDevices.Position,\r\nDevices.MonikerString,\r\nDevices.Login,\r\ndevices.Password,\r\nDevices.Type,\r\nDevices.State,\r\n'" + Messages.Unavailable + "' as DSState,\r\n'" + Messages.Unavailable + "' as ESState,\r\n'" + Messages.Unavailable + "' as VSState,\r\n'" + Messages.Unavailable + "' as ISState,\r\nDevices.SaveFace,\r\nDevices.SaveImage,\r\nDevices.SaveNonCategory,\r\n\r\ncast('False' as bit) as Changed\r\nFROM Devices\r\nleft outer join VideoServers on VideoServers.ID = Devices.VSID\r\nleft outer join\r\nDetectorServers on DetectorServers.ID = Devices.DSID left outer join\r\nExtractorServers on ExtractorServers.ID = Devices.ESID\r\nleft outer join\r\nIdentificationServers on IdentificationServers.ID = Devices.ISID";
    public DataTable DtMain = new DataTable();
    private List<BcObjects> _objects = new List<BcObjects>();
    private int _updatePeriod = 3000;
    private IContainer components = (IContainer) null;
    public static ManagmentServerClient MainServer;
    public Thread CheckThread;
    private bool _loading;
    public static bool ClosFlag;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colName;
    private GridColumn colDeviceConnectionString;
    private GridColumn colPosition;
    private GridColumn colComment;
    private GridColumn colState;
    private GridColumn colVSID;
    private GridColumn colDSID;
    private GridColumn colESID;
    private GridColumn colVideoServer;
    private RepositoryItemGridLookUpEdit cbVideoServer;
    private GridView repositoryItemGridLookUpEdit1View;
    private GridColumn colExtractorServer;
    private GridColumn colDetectorCount;
    private GridColumn colDetectorServer;
    private RepositoryItemGridLookUpEdit cbDetectorServer;
    private GridColumn colExtractorCount;
    private GridColumn colMinFace;
    private GridColumn colDetectorScore;
    private GridColumn colObjectID;
    private GridColumn colTableID;
    private GridColumn colDeviceType;
    private GridColumn colDeviceState;
    private GridColumn colDSState;
    private GridColumn colESState;
    private GridColumn colImageCount;
    private GridColumn colDetectionCount;
    private GridColumn colFaceCount;
    private GridColumn colExtractCount;
    private GridColumn colCheckedCount;
    private GridColumn colResultCount;
    private GridColumn colMinScore;
    private GridColumn colVSState;
    private RepositoryItemButtonEdit btObjects;
    private RepositoryItemButtonEdit btVideoServer;
    private RepositoryItemButtonEdit btDetectorServer;
    private RepositoryItemButtonEdit btExtractorServer;
    private RepositoryItemComboBox cbMinFace;
    private RepositoryItemComboBox cbImageSize;
    private RepositoryItemSpinEdit repositoryItemSpinEdit1;
    private RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
    private RepositoryItemCheckEdit chbSecondDetector;
    private Bar bar2;
    private BarManager barManager1;
    private Bar bar1;
    private BarLargeButtonItem btAddDevice;
    private BarLargeButtonItem btSaveDevice;
    private BarLargeButtonItem btDeleteDevice;
    private Bar bar3;
    private BarLargeButtonItem btShowVS;
    private BarLargeButtonItem btShowDS;
    private BarLargeButtonItem btShowES;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private GridColumn colChanged;
    private BarLargeButtonItem btEditDevice;
    private GridColumn colSaveFace;
    private GridColumn colSaveImage;
    private RepositoryItemCheckEdit chbFaces;
    private RepositoryItemCheckEdit chbSaveImage;
    private BarLargeButtonItem btShowManagmentServer;
    private RepositoryItemCheckEdit chbTracking;
    private GridColumn colSaveNonCategory;
    private RepositoryItemCheckEdit chbNonCategory;
    private BarLargeButtonItem btShowIS;
    private BarSubItem btManagmentServerState;
    private BarLargeButtonItem btClear;
    private BarLargeButtonItem btShowVideo;
    private BarButtonItem btReload;
    private GridColumn colIdentificationServer;
    private RepositoryItemButtonEdit btIdentificationServer;
    private GridColumn colISState;
    private GridColumn colISID;
    private BarEditItem tbPeriod;
    private RepositoryItemSpinEdit repositoryItemSpinEdit2;

    public FrmDevices()
    {
      this.InitializeComponent();
    }

    public void CheckServerState()
    {
      while (true)
      {
        if (!FrmDevices.ClosFlag)
        {
          if (FrmDevices.MainServer == null || FrmDevices.MainServer.State == CommunicationState.Closed || FrmDevices.MainServer.State == CommunicationState.Faulted || FrmDevices.MainServer.State == CommunicationState.Closing)
            this.UpdateManagementServerState();
          if (FrmDevices.MainServer.State == CommunicationState.Opened)
          {
            try
            {
              foreach (DeviceInfo deviceInfo in FrmDevices.MainServer.GetDevicesInfo())
              {
                foreach (DataRow dataRow in (InternalDataCollectionBase) this.DtMain.Rows)
                {
                  if (dataRow["ID"].ToString() == deviceInfo.Id.ToString())
                    this.Invoke((Delegate) new FrmDevices.RefreshDataFunc(this.RefreshData), (object) dataRow, (object) deviceInfo);
                }
              }
              this.Invoke((Delegate) new FrmDevices.RefreshStateFunc(this.RefreshState), (object) false);
            }
            catch
            {
            }
          }
          Thread.Sleep(this._updatePeriod);
        }
        else
          break;
      }
    }

    private void UpdateManagementServerState()
    {
      try
      {
        BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
        CommonSettings.ManagmentServerAddress = "net.tcp://" + (object) bcManagmentServer.Ip + ":" + (string) (object) bcManagmentServer.Port + "/CSManagmentServer/ManagmentServer";
        FrmDevices.MainServer = new ManagmentServerClient(new InstanceContext((object) FrmDevices.MainClient));
        FrmDevices.MainServer.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
        FrmDevices.MainServer.Open();
        FrmDevices.MainServer.Connect(FrmDevices.MainClient.MainId);
      }
      catch (CommunicationException ex)
      {
        this.Invoke((Delegate) new FrmDevices.RefreshStateFunc(this.RefreshState), (object) true);
      }
      catch (Exception ex)
      {
      }
    }

    private void RefreshData(DataRow row, DeviceInfo info)
    {
      row["DetectionCount"] = (object) info.DetectionCount;
      row["DetectionFaces"] = (object) info.DetectionFaces;
      row["DeviceState"] = !info.DeviceState ? (object) Messages.Unavailable : (object) Messages.Available;
      row["DSID"] = (object) info.Dsid;
      row["DSState"] = !info.DsState ? (object) Messages.Unavailable : (object) Messages.Available;
      row["ESID"] = (object) info.Esid;
      row["ESState"] = !info.EsState ? (object) Messages.Unavailable : (object) Messages.Available;
      row["ExtractCount"] = (object) info.ExtractCount;
      row["FrameCount"] = (object) info.FrameCount;
      row["ID"] = (object) info.Id;
      row["IdentifierCount"] = (object) info.IdentifierCount;
      row["ISID"] = (object) info.Isid;
      row["ISState"] = !info.IsState ? (object) Messages.Unavailable : (object) Messages.Available;
      row["ResultCount"] = (object) info.ResultCount;
      row["VSID"] = (object) info.Vsid;
      if (info.VsState)
        row["VSState"] = (object) Messages.Available;
      else
        row["VSState"] = (object) Messages.Unavailable;
    }

    public void ReloadObjects()
    {
      this._objects = BcObjects.LoadAll();
    }

    private void ReloadGrid()
    {
      this._loading = true;
      this.DtMain = new DataTable();
      SqlCommand sqlCommand = new SqlCommand(this._selectStr, new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
        this.DtMain.Columns.Add(sqlDataReader.GetName(ordinal), sqlDataReader.GetFieldType(ordinal));
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        this.DtMain.Rows.Add(values);
        foreach (BcObjects bcObjects in this._objects)
        {
          if (bcObjects.Id.ToString() == sqlDataReader["ObjectID"].ToString())
          {
            bcObjects.GetData();
            if ((Guid) sqlDataReader["TableID"] != Guid.Empty)
              this.DtMain.Rows[this.DtMain.Rows.Count - 1]["ObjectName"] = (object) (bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, (Guid) sqlDataReader["TableID"]).Name);
          }
        }
      }
      sqlCommand.Connection.Close();
      this.gridControl1.DataSource = (object) this.DtMain;
      this._loading = false;
    }

    private void RefreshState(bool error)
    {
      this.btManagmentServerState.Caption = !error ? Messages.ManagmentServerWork : Messages.ManagmentServerUnavailble;
    }

    private void frmDevices_Load(object sender, EventArgs e)
    {
      this.gridView1.DoubleClick += new EventHandler(this.gridView1_DoubleClick);
      if (new AuthorizationForm("CascadeEquipment", (ILocalizationProvider) new Messages()).ShowDialog() == DialogResult.OK)
      {
        this.UpdateManagementServerState();
        this.CheckThread = new Thread(new ThreadStart(this.CheckServerState))
        {
          IsBackground = true
        };
        this.CheckThread.Start();
        this.ReloadObjects();
        this.ReloadGrid();
      }
      else
        this.Close();
    }

    private void btAddDevice_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmEditDevice frmEditDevice = new FrmEditDevice();
      if (frmEditDevice.ShowDialog() != DialogResult.OK)
        return;
      SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[4]
      {
        (object) this._selectStr,
        (object) " Where Devices.ID = '",
        (object) frmEditDevice.Device.Id,
        (object) "'"
      }), new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      this._loading = true;
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        this.DtMain.Rows.Add(values);
        foreach (BcObjects bcObjects in this._objects)
        {
          if (bcObjects.Id.ToString() == sqlDataReader["ObjectID"].ToString())
          {
            bcObjects.GetData();
            if ((Guid) sqlDataReader["TableID"] != Guid.Empty)
              this.DtMain.Rows[this.DtMain.Rows.Count - 1]["ObjectName"] = (object) (bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, (Guid) sqlDataReader["TableID"]).Name);
          }
        }
      }
      this._loading = false;
      sqlCommand.Connection.Close();
    }

    private void btEditDevice_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gridView1.FocusedRowHandle < 0)
        return;
      FrmEditDevice frmEditDevice = new FrmEditDevice();
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      frmEditDevice.Device = BcDevicesStorageExtensions.LoadById((Guid) dataRow["ID"]);
      if (frmEditDevice.ShowDialog() == DialogResult.OK)
      {
        this._loading = true;
        SqlCommand sqlCommand = new SqlCommand(string.Concat(new object[4]
        {
          (object) this._selectStr,
          (object) " Where Devices.ID = '",
          (object) frmEditDevice.Device.Id,
          (object) "'"
        }), new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
          object[] values = new object[sqlDataReader.FieldCount];
          sqlDataReader.GetValues(values);
          dataRow.ItemArray = values;
          foreach (BcObjects bcObjects in this._objects)
          {
            if (bcObjects.Id.ToString() == sqlDataReader["ObjectID"].ToString())
            {
              bcObjects.GetData();
              if ((Guid) sqlDataReader["TableID"] != Guid.Empty)
                dataRow["ObjectName"] = (object) (bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, (Guid) sqlDataReader["TableID"]).Name);
            }
          }
        }
        this._loading = false;
        sqlCommand.Connection.Close();
      }
    }

    private void btSaveDevice_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gridView1.IsEditorFocused)
        this.gridView1.SetFocusedValue(this.gridView1.EditingValue);
      foreach (DataRow dataRow in (InternalDataCollectionBase) this.DtMain.Rows)
      {
        if ((bool) dataRow["Changed"])
        {
          BcDevices devices = BcDevicesStorageExtensions.LoadById((Guid) dataRow["ID"]);
          devices.DetectorCount = (int) dataRow["DetectorCount"];
          devices.DetectorScore = (Decimal) ((double) dataRow["DetectorScore"]);
          devices.Dsid = (Guid) dataRow["DSID"];
          devices.Esid = (Guid) dataRow["ESID"];
          devices.Isid = (Guid) dataRow["ISID"];
          devices.ExtractorCount = (int) dataRow["ExtractorCount"];
          devices.ConnectionString = (string) dataRow["MonikerString"];
          devices.Login = (string) dataRow["Login"];
          devices.MinFace = (int) dataRow["MinFace"];
          devices.MinScore = (Decimal) ((double) dataRow["MinScore"]);
          devices.Name = (string) dataRow["Name"];
          devices.Password = (string) dataRow["Password"];
          devices.SaveFace = (bool) dataRow["SaveFace"];
          devices.SaveImage = (bool) dataRow["SaveImage"];
          devices.SaveUnidentified = (bool) dataRow["SaveUnidentified"];
          devices.Vsid = (Guid) dataRow["VSID"];
          BcDevicesStorageExtensions.Save(devices, true);
          dataRow["Changed"] = (object) false;
        }
      }
    }

    private void btDeleteDevice_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gridView1.SelectedRowsCount < 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      foreach (int rowHandle in this.gridView1.GetSelectedRows())
      {
        DataRow dataRow = this.gridView1.GetDataRow(rowHandle);
        BcDevicesStorageExtensions.Delete(new BcDevices()
        {
          Id = (Guid) dataRow["ID"]
        }, true);
      }
      this.gridView1.DeleteSelectedRows();
    }

    private void btShowVS_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmVideoServers frmVideoServers = new FrmVideoServers()
      {
        ListFlag = false
      };
      if (frmVideoServers.ShowDialog() != DialogResult.OK || !(frmVideoServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["VSID"] = (object) frmVideoServers.MainServer.Id;
      dataRow["VideoServer"] = (object) frmVideoServers.MainServer.Ip;
    }

    private void btShowDS_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmDetectorServers frmDetectorServers = new FrmDetectorServers()
      {
        ListFlag = false
      };
      if (frmDetectorServers.ShowDialog() != DialogResult.OK || !(frmDetectorServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["DSID"] = (object) frmDetectorServers.MainServer.Id;
      dataRow["DetectorServer"] = (object) frmDetectorServers.MainServer.Ip;
    }

    private void btShowES_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmExtractorServers extractorServers = new FrmExtractorServers()
      {
        ListFlag = false
      };
      if (extractorServers.ShowDialog() != DialogResult.OK || !(extractorServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["ESID"] = (object) extractorServers.MainServer.Id;
      dataRow["ExtractorServer"] = (object) extractorServers.MainServer.Ip;
    }

    private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (e.RowHandle < 0)
        return;
      e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
    {
      if (e.RowHandle < 0 || !(bool) this.gridView1.GetDataRow(e.RowHandle)["Changed"])
        return;
      e.Appearance.BackColor = Color.White;
      e.Appearance.BackColor2 = Color.MistyRose;
    }

    private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
      if (this._loading || e.Column == this.colChanged || (e.Column == this.colDetectionCount || e.Column == this.colImageCount) || (e.Column == this.colFaceCount || e.Column == this.colExtractCount || (e.Column == this.colResultCount || e.Column == this.colCheckedCount)) || (e.Column == this.colDeviceState || e.Column == this.colDSState || (e.Column == this.colVSState || e.Column == this.colESState)) || e.Column == this.colISState)
        return;
      this.gridView1.GetDataRow(e.RowHandle)["Changed"] = (object) true;
    }

    private void btObjects_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
    }

    private void btExtractorServer_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      FrmExtractorServers extractorServers = new FrmExtractorServers();
      extractorServers.ListFlag = true;
      if (extractorServers.ShowDialog() != DialogResult.OK || !(extractorServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["ESID"] = (object) extractorServers.MainServer.Id;
      dataRow["ExtractorServer"] = (object) extractorServers.MainServer.Ip;
      dataRow["Changed"] = (object) true;
    }

    private void btDetectorServer_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      FrmDetectorServers frmDetectorServers = new FrmDetectorServers();
      frmDetectorServers.ListFlag = true;
      if (frmDetectorServers.ShowDialog() != DialogResult.OK || !(frmDetectorServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["DSID"] = (object) frmDetectorServers.MainServer.Id;
      dataRow["DetectorServer"] = (object) frmDetectorServers.MainServer.Ip;
      dataRow["Changed"] = (object) true;
    }

    private void btVideoServer_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      FrmVideoServers frmVideoServers = new FrmVideoServers();
      frmVideoServers.ListFlag = true;
      if (frmVideoServers.ShowDialog() != DialogResult.OK || !(frmVideoServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["VSID"] = (object) frmVideoServers.MainServer.Id;
      dataRow["VideoServer"] = (object) frmVideoServers.MainServer.Ip;
      dataRow["Changed"] = (object) true;
    }

    private void btShowManagmentServer_ItemClick(object sender, ItemClickEventArgs e)
    {
      int num = (int) new ManagmentServer().ShowDialog();
    }

    private void btShowIdentificationServer_ItemClick(object sender, ItemClickEventArgs e)
    {
      FrmIdentificationcServers identificationcServers = new FrmIdentificationcServers();
      identificationcServers.ListFlag = false;
      if (identificationcServers.ShowDialog() != DialogResult.OK || !(identificationcServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["ISID"] = (object) identificationcServers.MainServer.Id;
      dataRow["IdentificationServer"] = (object) identificationcServers.MainServer.Ip;
    }

    private void frmDevices_FormClosing(object sender, FormClosingEventArgs e)
    {
      FrmDevices.ClosFlag = true;
    }

    private void btClear_ItemClick(object sender, ItemClickEventArgs e)
    {
      int[] selectedRows = this.gridView1.GetSelectedRows();
      List<Guid> list = new List<Guid>();
      foreach (int rowHandle in selectedRows)
        list.Add((Guid) this.gridView1.GetDataRow(rowHandle)["ID"]);
      ManagmentServerClient managmentServerClient = new ManagmentServerClient(new InstanceContext((object) FrmDevices.MainClient));
      managmentServerClient.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
      try
      {
        managmentServerClient.Open();
        managmentServerClient.ClearResults(list.ToArray());
        managmentServerClient.Close();
      }
      catch
      {
      }
    }

    private void btShowVideo_ItemClick(object sender, ItemClickEventArgs e)
    {
      if (this.gridView1.FocusedRowHandle < 0)
        return;
      BcDevices bcDevices = BcDevicesStorageExtensions.LoadById((Guid) this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle)["ID"]);
      new FrmVideo()
      {
        MainDevice = bcDevices
      }.Show();
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      if (this.gridView1.FocusedRowHandle < 0)
        return;
      BcDevices bcDevices = BcDevicesStorageExtensions.LoadById((Guid) this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle)["ID"]);
      new FrmVideo()
      {
        MainDevice = bcDevices
      }.Show();
    }

    private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
      if (e.Column != this.colDeviceState && e.Column != this.colVSState && (e.Column != this.colDSState && e.Column != this.colESState) && e.Column != this.colISState || !(e.CellValue.ToString() == Messages.Unavailable))
        return;
      e.Appearance.BackColor = Color.Red;
      e.Appearance.BackColor2 = Color.Red;
    }

    private void btReload_ItemClick(object sender, ItemClickEventArgs e)
    {
      this.ReloadObjects();
      this.ReloadGrid();
    }

    private void btIdentificationServer_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      FrmVideoServers frmVideoServers = new FrmVideoServers();
      frmVideoServers.ListFlag = true;
      if (frmVideoServers.ShowDialog() != DialogResult.OK || !(frmVideoServers.MainServer.Id != Guid.Empty))
        return;
      DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
      dataRow["ISID"] = (object) frmVideoServers.MainServer.Id;
      dataRow["IdentificationServer"] = (object) frmVideoServers.MainServer.Ip;
      dataRow["Changed"] = (object) true;
    }

    private void tbPeriod_ItemClick(object sender, ItemClickEventArgs e)
    {
    }

    private void tbPeriod_EditValueChanged(object sender, EventArgs e)
    {
      this._updatePeriod = (int) ((double) Convert.ToSingle(this.tbPeriod.EditValue) * 1000.0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmDevices));
      this.gridControl1 = new GridControl();
      this.gridView1 = new GridView();
      this.colID = new GridColumn();
      this.colName = new GridColumn();
      this.colDeviceState = new GridColumn();
      this.colImageCount = new GridColumn();
      this.colDetectionCount = new GridColumn();
      this.colFaceCount = new GridColumn();
      this.colExtractCount = new GridColumn();
      this.colResultCount = new GridColumn();
      this.colCheckedCount = new GridColumn();
      this.colDetectorCount = new GridColumn();
      this.colExtractorCount = new GridColumn();
      this.colComment = new GridColumn();
      this.btObjects = new RepositoryItemButtonEdit();
      this.colVSID = new GridColumn();
      this.colDSID = new GridColumn();
      this.colESID = new GridColumn();
      this.colVideoServer = new GridColumn();
      this.btVideoServer = new RepositoryItemButtonEdit();
      this.colDetectorServer = new GridColumn();
      this.btDetectorServer = new RepositoryItemButtonEdit();
      this.colExtractorServer = new GridColumn();
      this.btExtractorServer = new RepositoryItemButtonEdit();
      this.colIdentificationServer = new GridColumn();
      this.btIdentificationServer = new RepositoryItemButtonEdit();
      this.colMinFace = new GridColumn();
      this.cbMinFace = new RepositoryItemComboBox();
      this.colDetectorScore = new GridColumn();
      this.colMinScore = new GridColumn();
      this.colObjectID = new GridColumn();
      this.colTableID = new GridColumn();
      this.colPosition = new GridColumn();
      this.colDeviceConnectionString = new GridColumn();
      this.colDeviceType = new GridColumn();
      this.colState = new GridColumn();
      this.colDSState = new GridColumn();
      this.colESState = new GridColumn();
      this.colVSState = new GridColumn();
      this.colChanged = new GridColumn();
      this.colSaveFace = new GridColumn();
      this.chbFaces = new RepositoryItemCheckEdit();
      this.colSaveImage = new GridColumn();
      this.chbSaveImage = new RepositoryItemCheckEdit();
      this.colSaveNonCategory = new GridColumn();
      this.chbNonCategory = new RepositoryItemCheckEdit();
      this.colISState = new GridColumn();
      this.colISID = new GridColumn();
      this.cbVideoServer = new RepositoryItemGridLookUpEdit();
      this.cbDetectorServer = new RepositoryItemGridLookUpEdit();
      this.repositoryItemGridLookUpEdit1 = new RepositoryItemGridLookUpEdit();
      this.cbImageSize = new RepositoryItemComboBox();
      this.repositoryItemSpinEdit1 = new RepositoryItemSpinEdit();
      this.chbSecondDetector = new RepositoryItemCheckEdit();
      this.chbTracking = new RepositoryItemCheckEdit();
      this.repositoryItemGridLookUpEdit1View = new GridView();
      this.bar2 = new Bar();
      this.btManagmentServerState = new BarSubItem();
      this.barManager1 = new BarManager(this.components);
      this.bar1 = new Bar();
      this.btAddDevice = new BarLargeButtonItem();
      this.btDeleteDevice = new BarLargeButtonItem();
      this.btEditDevice = new BarLargeButtonItem();
      this.btSaveDevice = new BarLargeButtonItem();
      this.btReload = new BarButtonItem();
      this.btShowVideo = new BarLargeButtonItem();
      this.btClear = new BarLargeButtonItem();
      this.tbPeriod = new BarEditItem();
      this.repositoryItemSpinEdit2 = new RepositoryItemSpinEdit();
      this.bar3 = new Bar();
      this.btShowVS = new BarLargeButtonItem();
      this.btShowDS = new BarLargeButtonItem();
      this.btShowES = new BarLargeButtonItem();
      this.btShowIS = new BarLargeButtonItem();
      this.btShowManagmentServer = new BarLargeButtonItem();
      this.barDockControlTop = new BarDockControl();
      this.barDockControlBottom = new BarDockControl();
      this.barDockControlLeft = new BarDockControl();
      this.barDockControlRight = new BarDockControl();
      this.gridControl1.BeginInit();
      this.gridView1.BeginInit();
      this.btObjects.BeginInit();
      this.btVideoServer.BeginInit();
      this.btDetectorServer.BeginInit();
      this.btExtractorServer.BeginInit();
      this.btIdentificationServer.BeginInit();
      this.cbMinFace.BeginInit();
      this.chbFaces.BeginInit();
      this.chbSaveImage.BeginInit();
      this.chbNonCategory.BeginInit();
      this.cbVideoServer.BeginInit();
      this.cbDetectorServer.BeginInit();
      this.repositoryItemGridLookUpEdit1.BeginInit();
      this.cbImageSize.BeginInit();
      this.repositoryItemSpinEdit1.BeginInit();
      this.chbSecondDetector.BeginInit();
      this.chbTracking.BeginInit();
      this.repositoryItemGridLookUpEdit1View.BeginInit();
      this.barManager1.BeginInit();
      this.repositoryItemSpinEdit2.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.gridControl1, "gridControl1");
      this.gridControl1.MainView = (BaseView) this.gridView1;
      this.gridControl1.Name = "gridControl1";
      this.gridControl1.RepositoryItems.AddRange(new RepositoryItem[16]
      {
        (RepositoryItem) this.cbVideoServer,
        (RepositoryItem) this.cbDetectorServer,
        (RepositoryItem) this.btObjects,
        (RepositoryItem) this.btVideoServer,
        (RepositoryItem) this.btDetectorServer,
        (RepositoryItem) this.btExtractorServer,
        (RepositoryItem) this.cbMinFace,
        (RepositoryItem) this.repositoryItemGridLookUpEdit1,
        (RepositoryItem) this.cbImageSize,
        (RepositoryItem) this.repositoryItemSpinEdit1,
        (RepositoryItem) this.chbSecondDetector,
        (RepositoryItem) this.chbFaces,
        (RepositoryItem) this.chbSaveImage,
        (RepositoryItem) this.chbTracking,
        (RepositoryItem) this.chbNonCategory,
        (RepositoryItem) this.btIdentificationServer
      });
      this.gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.gridView1
      });
      this.gridView1.ColumnPanelRowHeight = 50;
      this.gridView1.Columns.AddRange(new GridColumn[37]
      {
        this.colID,
        this.colName,
        this.colDeviceState,
        this.colImageCount,
        this.colDetectionCount,
        this.colFaceCount,
        this.colExtractCount,
        this.colResultCount,
        this.colCheckedCount,
        this.colDetectorCount,
        this.colExtractorCount,
        this.colComment,
        this.colVSID,
        this.colDSID,
        this.colESID,
        this.colVideoServer,
        this.colDetectorServer,
        this.colExtractorServer,
        this.colIdentificationServer,
        this.colMinFace,
        this.colDetectorScore,
        this.colMinScore,
        this.colObjectID,
        this.colTableID,
        this.colPosition,
        this.colDeviceConnectionString,
        this.colDeviceType,
        this.colState,
        this.colDSState,
        this.colESState,
        this.colVSState,
        this.colChanged,
        this.colSaveFace,
        this.colSaveImage,
        this.colSaveNonCategory,
        this.colISState,
        this.colISID
      });
      this.gridView1.GridControl = this.gridControl1;
      this.gridView1.HorzScrollVisibility = ScrollVisibility.Always;
      this.gridView1.IndicatorWidth = 50;
      this.gridView1.Name = "gridView1";
      this.gridView1.OptionsCustomization.AllowFilter = false;
      this.gridView1.OptionsFind.AlwaysVisible = true;
      this.gridView1.OptionsFind.ClearFindOnClose = false;
      this.gridView1.OptionsFind.FindDelay = 10000;
      this.gridView1.OptionsFind.ShowCloseButton = false;
      this.gridView1.OptionsSelection.MultiSelect = true;
      this.gridView1.OptionsView.ColumnAutoWidth = false;
      this.gridView1.OptionsView.ShowGroupPanel = false;
      this.gridView1.VertScrollVisibility = ScrollVisibility.Always;
      this.gridView1.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
      this.gridView1.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
      this.gridView1.RowStyle += new RowStyleEventHandler(this.gridView1_RowStyle);
      this.gridView1.CellValueChanged += new CellValueChangedEventHandler(this.gridView1_CellValueChanged);
      this.colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      this.colID.AppearanceHeader.Options.UseFont = true;
      this.colID.AppearanceHeader.Options.UseTextOptions = true;
      this.colID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colID, "colID");
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      this.colID.OptionsColumn.AllowEdit = false;
      this.colID.OptionsColumn.ReadOnly = true;
      this.colName.AppearanceHeader.Options.UseTextOptions = true;
      this.colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.FieldName = "Name";
      this.colName.Name = "colName";
      this.colName.OptionsColumn.AllowEdit = false;
      this.colName.OptionsColumn.FixedWidth = true;
      this.colName.OptionsColumn.ReadOnly = true;
      this.colName.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDeviceState.AppearanceHeader.Options.UseTextOptions = true;
      this.colDeviceState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDeviceState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDeviceState, "colDeviceState");
      this.colDeviceState.FieldName = "DeviceState";
      this.colDeviceState.Name = "colDeviceState";
      this.colDeviceState.OptionsColumn.AllowEdit = false;
      this.colDeviceState.OptionsColumn.FixedWidth = true;
      this.colDeviceState.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colImageCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colImageCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colImageCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colImageCount, "colImageCount");
      this.colImageCount.FieldName = "FrameCount";
      this.colImageCount.Name = "colImageCount";
      this.colImageCount.OptionsColumn.AllowEdit = false;
      this.colImageCount.OptionsColumn.FixedWidth = true;
      this.colImageCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDetectionCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colDetectionCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDetectionCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDetectionCount, "colDetectionCount");
      this.colDetectionCount.FieldName = "DetectionCount";
      this.colDetectionCount.Name = "colDetectionCount";
      this.colDetectionCount.OptionsColumn.AllowEdit = false;
      this.colDetectionCount.OptionsColumn.FixedWidth = true;
      this.colDetectionCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colFaceCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colFaceCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colFaceCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colFaceCount, "colFaceCount");
      this.colFaceCount.FieldName = "DetectionFaces";
      this.colFaceCount.Name = "colFaceCount";
      this.colFaceCount.OptionsColumn.AllowEdit = false;
      this.colFaceCount.OptionsColumn.FixedWidth = true;
      this.colFaceCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colExtractCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colExtractCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colExtractCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colExtractCount, "colExtractCount");
      this.colExtractCount.FieldName = "ExtractCount";
      this.colExtractCount.Name = "colExtractCount";
      this.colExtractCount.OptionsColumn.AllowEdit = false;
      this.colExtractCount.OptionsColumn.FixedWidth = true;
      this.colExtractCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colResultCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colResultCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colResultCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colResultCount, "colResultCount");
      this.colResultCount.FieldName = "ResultCount";
      this.colResultCount.Name = "colResultCount";
      this.colResultCount.OptionsColumn.AllowEdit = false;
      this.colResultCount.OptionsColumn.FixedWidth = true;
      this.colResultCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colCheckedCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colCheckedCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colCheckedCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colCheckedCount, "colCheckedCount");
      this.colCheckedCount.FieldName = "IdentifierCount";
      this.colCheckedCount.Name = "colCheckedCount";
      this.colCheckedCount.OptionsColumn.AllowEdit = false;
      this.colCheckedCount.OptionsColumn.FixedWidth = true;
      this.colCheckedCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDetectorCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colDetectorCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDetectorCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDetectorCount, "colDetectorCount");
      this.colDetectorCount.FieldName = "DetectorCount";
      this.colDetectorCount.Name = "colDetectorCount";
      this.colDetectorCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colExtractorCount.AppearanceHeader.Options.UseTextOptions = true;
      this.colExtractorCount.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colExtractorCount.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colExtractorCount, "colExtractorCount");
      this.colExtractorCount.FieldName = "ExtractorCount";
      this.colExtractorCount.Name = "colExtractorCount";
      this.colExtractorCount.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colComment.AppearanceHeader.Options.UseTextOptions = true;
      this.colComment.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colComment.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colComment, "colComment");
      this.colComment.ColumnEdit = (RepositoryItem) this.btObjects;
      this.colComment.FieldName = "ObjectName";
      this.colComment.Name = "colComment";
      this.colComment.OptionsColumn.AllowEdit = false;
      this.colComment.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.btObjects, "btObjects");
      this.btObjects.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btObjects.Name = "btObjects";
      this.btObjects.ButtonClick += new ButtonPressedEventHandler(this.btObjects_ButtonClick);
      this.colVSID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colVSID.AppearanceCell.Font");
      this.colVSID.AppearanceCell.Options.UseFont = true;
      this.colVSID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colVSID.AppearanceHeader.Font");
      this.colVSID.AppearanceHeader.Options.UseFont = true;
      this.colVSID.AppearanceHeader.Options.UseTextOptions = true;
      this.colVSID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colVSID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colVSID, "colVSID");
      this.colVSID.FieldName = "VSID";
      this.colVSID.Name = "colVSID";
      this.colVSID.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDSID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colDSID.AppearanceCell.Font");
      this.colDSID.AppearanceCell.Options.UseFont = true;
      this.colDSID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colDSID.AppearanceHeader.Font");
      this.colDSID.AppearanceHeader.Options.UseFont = true;
      this.colDSID.AppearanceHeader.Options.UseTextOptions = true;
      this.colDSID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDSID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDSID, "colDSID");
      this.colDSID.FieldName = "DSID";
      this.colDSID.Name = "colDSID";
      this.colDSID.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colESID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colESID.AppearanceCell.Font");
      this.colESID.AppearanceCell.Options.UseFont = true;
      this.colESID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colESID.AppearanceHeader.Font");
      this.colESID.AppearanceHeader.Options.UseFont = true;
      this.colESID.AppearanceHeader.Options.UseTextOptions = true;
      this.colESID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colESID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colESID, "colESID");
      this.colESID.FieldName = "ESID";
      this.colESID.Name = "colESID";
      this.colESID.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colVideoServer.AppearanceHeader.Options.UseTextOptions = true;
      this.colVideoServer.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colVideoServer.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colVideoServer, "colVideoServer");
      this.colVideoServer.ColumnEdit = (RepositoryItem) this.btVideoServer;
      this.colVideoServer.FieldName = "VideoServer";
      this.colVideoServer.Name = "colVideoServer";
      this.colVideoServer.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.btVideoServer, "btVideoServer");
      this.btVideoServer.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btVideoServer.Name = "btVideoServer";
      this.btVideoServer.ButtonClick += new ButtonPressedEventHandler(this.btVideoServer_ButtonClick);
      this.colDetectorServer.AppearanceHeader.Options.UseTextOptions = true;
      this.colDetectorServer.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDetectorServer.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDetectorServer, "colDetectorServer");
      this.colDetectorServer.ColumnEdit = (RepositoryItem) this.btDetectorServer;
      this.colDetectorServer.FieldName = "DetectorServer";
      this.colDetectorServer.Name = "colDetectorServer";
      this.colDetectorServer.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.btDetectorServer, "btDetectorServer");
      this.btDetectorServer.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btDetectorServer.Name = "btDetectorServer";
      this.btDetectorServer.ButtonClick += new ButtonPressedEventHandler(this.btDetectorServer_ButtonClick);
      this.colExtractorServer.AppearanceHeader.Options.UseTextOptions = true;
      this.colExtractorServer.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colExtractorServer.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colExtractorServer, "colExtractorServer");
      this.colExtractorServer.ColumnEdit = (RepositoryItem) this.btExtractorServer;
      this.colExtractorServer.FieldName = "ExtractorServer";
      this.colExtractorServer.Name = "colExtractorServer";
      this.colExtractorServer.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.btExtractorServer, "btExtractorServer");
      this.btExtractorServer.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btExtractorServer.Name = "btExtractorServer";
      this.btExtractorServer.ButtonClick += new ButtonPressedEventHandler(this.btExtractorServer_ButtonClick);
      this.colIdentificationServer.AppearanceHeader.Options.UseTextOptions = true;
      this.colIdentificationServer.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colIdentificationServer.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colIdentificationServer, "colIdentificationServer");
      this.colIdentificationServer.ColumnEdit = (RepositoryItem) this.btIdentificationServer;
      this.colIdentificationServer.FieldName = "IdentificationServer";
      this.colIdentificationServer.Name = "colIdentificationServer";
      this.colIdentificationServer.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.btIdentificationServer, "btIdentificationServer");
      this.btIdentificationServer.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btIdentificationServer.Name = "btIdentificationServer";
      this.btIdentificationServer.ButtonClick += new ButtonPressedEventHandler(this.btIdentificationServer_ButtonClick);
      this.colMinFace.AppearanceHeader.Options.UseTextOptions = true;
      this.colMinFace.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colMinFace.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colMinFace, "colMinFace");
      this.colMinFace.ColumnEdit = (RepositoryItem) this.cbMinFace;
      this.colMinFace.FieldName = "MinFace";
      this.colMinFace.Name = "colMinFace";
      this.colMinFace.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.cbMinFace, "cbMinFace");
      this.cbMinFace.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbMinFace.Buttons"))
      });
      this.cbMinFace.Items.AddRange(new object[4]
      {
        (object) componentResourceManager.GetString("cbMinFace.Items"),
        (object) componentResourceManager.GetString("cbMinFace.Items1"),
        (object) componentResourceManager.GetString("cbMinFace.Items2"),
        (object) componentResourceManager.GetString("cbMinFace.Items3")
      });
      this.cbMinFace.Name = "cbMinFace";
      this.colDetectorScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colDetectorScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDetectorScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDetectorScore, "colDetectorScore");
      this.colDetectorScore.FieldName = "DetectorScore";
      this.colDetectorScore.Name = "colDetectorScore";
      this.colDetectorScore.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colMinScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colMinScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colMinScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colMinScore, "colMinScore");
      this.colMinScore.FieldName = "MinScore";
      this.colMinScore.Name = "colMinScore";
      this.colMinScore.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colObjectID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colObjectID.AppearanceCell.Font");
      this.colObjectID.AppearanceCell.Options.UseFont = true;
      this.colObjectID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colObjectID.AppearanceHeader.Font");
      this.colObjectID.AppearanceHeader.Options.UseFont = true;
      this.colObjectID.AppearanceHeader.Options.UseTextOptions = true;
      this.colObjectID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colObjectID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colObjectID, "colObjectID");
      this.colObjectID.FieldName = "ObjectID";
      this.colObjectID.Name = "colObjectID";
      this.colObjectID.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colTableID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colTableID.AppearanceCell.Font");
      this.colTableID.AppearanceCell.Options.UseFont = true;
      this.colTableID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colTableID.AppearanceHeader.Font");
      this.colTableID.AppearanceHeader.Options.UseFont = true;
      this.colTableID.AppearanceHeader.Options.UseTextOptions = true;
      this.colTableID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colTableID.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colTableID, "colTableID");
      this.colTableID.FieldName = "TableID";
      this.colTableID.Name = "colTableID";
      this.colTableID.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colPosition.AppearanceHeader.Options.UseTextOptions = true;
      this.colPosition.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colPosition.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colPosition, "colPosition");
      this.colPosition.FieldName = "Position";
      this.colPosition.Name = "colPosition";
      this.colPosition.OptionsColumn.AllowEdit = false;
      this.colPosition.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDeviceConnectionString.AppearanceHeader.Options.UseTextOptions = true;
      this.colDeviceConnectionString.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDeviceConnectionString.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDeviceConnectionString, "colDeviceConnectionString");
      this.colDeviceConnectionString.FieldName = "MonikerString";
      this.colDeviceConnectionString.Name = "colDeviceConnectionString";
      this.colDeviceConnectionString.OptionsColumn.AllowEdit = false;
      this.colDeviceConnectionString.OptionsColumn.ReadOnly = true;
      this.colDeviceConnectionString.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDeviceType.AppearanceHeader.Options.UseTextOptions = true;
      this.colDeviceType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDeviceType.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDeviceType, "colDeviceType");
      this.colDeviceType.FieldName = "Type";
      this.colDeviceType.Name = "colDeviceType";
      this.colDeviceType.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colState.AppearanceHeader.Options.UseTextOptions = true;
      this.colState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colState, "colState");
      this.colState.FieldName = "State";
      this.colState.Name = "colState";
      this.colState.OptionsColumn.AllowEdit = false;
      this.colState.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colDSState.AppearanceHeader.Options.UseTextOptions = true;
      this.colDSState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDSState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colDSState, "colDSState");
      this.colDSState.FieldName = "DSState";
      this.colDSState.Name = "colDSState";
      this.colDSState.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colESState.AppearanceHeader.Options.UseTextOptions = true;
      this.colESState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colESState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colESState, "colESState");
      this.colESState.FieldName = "ESState";
      this.colESState.Name = "colESState";
      this.colESState.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.colVSState.AppearanceHeader.Options.UseTextOptions = true;
      this.colVSState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colVSState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colVSState, "colVSState");
      this.colVSState.FieldName = "VSState";
      this.colVSState.Name = "colVSState";
      this.colVSState.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.colChanged, "colChanged");
      this.colChanged.FieldName = "Changed";
      this.colChanged.Name = "colChanged";
      this.colSaveFace.AppearanceHeader.Options.UseTextOptions = true;
      this.colSaveFace.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colSaveFace.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colSaveFace, "colSaveFace");
      this.colSaveFace.ColumnEdit = (RepositoryItem) this.chbFaces;
      this.colSaveFace.FieldName = "SaveFace";
      this.colSaveFace.Name = "colSaveFace";
      this.colSaveFace.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.chbFaces, "chbFaces");
      this.chbFaces.Name = "chbFaces";
      this.colSaveImage.AppearanceHeader.Options.UseTextOptions = true;
      this.colSaveImage.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colSaveImage.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colSaveImage, "colSaveImage");
      this.colSaveImage.ColumnEdit = (RepositoryItem) this.chbSaveImage;
      this.colSaveImage.FieldName = "SaveImage";
      this.colSaveImage.Name = "colSaveImage";
      this.colSaveImage.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.chbSaveImage, "chbSaveImage");
      this.chbSaveImage.Name = "chbSaveImage";
      this.colSaveNonCategory.AppearanceHeader.Options.UseTextOptions = true;
      this.colSaveNonCategory.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colSaveNonCategory.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colSaveNonCategory, "colSaveNonCategory");
      this.colSaveNonCategory.ColumnEdit = (RepositoryItem) this.chbNonCategory;
      this.colSaveNonCategory.FieldName = "SaveNonCategory";
      this.colSaveNonCategory.Name = "colSaveNonCategory";
      this.colSaveNonCategory.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      componentResourceManager.ApplyResources((object) this.chbNonCategory, "chbNonCategory");
      this.chbNonCategory.Name = "chbNonCategory";
      this.colISState.AppearanceHeader.Options.UseTextOptions = true;
      this.colISState.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colISState.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources((object) this.colISState, "colISState");
      this.colISState.FieldName = "ISState";
      this.colISState.Name = "colISState";
      componentResourceManager.ApplyResources((object) this.colISID, "colISID");
      this.colISID.FieldName = "ISID";
      this.colISID.Name = "colISID";
      this.cbVideoServer.Name = "cbVideoServer";
      this.cbDetectorServer.Name = "cbDetectorServer";
      this.repositoryItemGridLookUpEdit1.Name = "repositoryItemGridLookUpEdit1";
      componentResourceManager.ApplyResources((object) this.cbImageSize, "cbImageSize");
      this.cbImageSize.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbImageSize.Buttons"))
      });
      this.cbImageSize.Items.AddRange(new object[5]
      {
        (object) componentResourceManager.GetString("cbImageSize.Items"),
        (object) componentResourceManager.GetString("cbImageSize.Items1"),
        (object) componentResourceManager.GetString("cbImageSize.Items2"),
        (object) componentResourceManager.GetString("cbImageSize.Items3"),
        (object) componentResourceManager.GetString("cbImageSize.Items4")
      });
      this.cbImageSize.Name = "cbImageSize";
      componentResourceManager.ApplyResources((object) this.repositoryItemSpinEdit1, "repositoryItemSpinEdit1");
      this.repositoryItemSpinEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
      componentResourceManager.ApplyResources((object) this.chbSecondDetector, "chbSecondDetector");
      this.chbSecondDetector.Name = "chbSecondDetector";
      componentResourceManager.ApplyResources((object) this.chbTracking, "chbTracking");
      this.chbTracking.Name = "chbTracking";
      this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
      this.bar2.BarName = "Status bar";
      this.bar2.DockCol = 0;
      this.bar2.DockRow = 0;
      this.bar2.DockStyle = BarDockStyle.Bottom;
      this.bar2.FloatLocation = new Point(577, 793);
      this.bar2.LinksPersistInfo.AddRange(new LinkPersistInfo[1]
      {
        new LinkPersistInfo((BarItem) this.btManagmentServerState)
      });
      this.bar2.OptionsBar.MultiLine = true;
      this.bar2.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources((object) this.bar2, "bar2");
      componentResourceManager.ApplyResources((object) this.btManagmentServerState, "btManagmentServerState");
      this.btManagmentServerState.Id = 9;
      this.btManagmentServerState.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btManagmentServerState.ItemAppearance.Normal.Font");
      this.btManagmentServerState.ItemAppearance.Normal.Options.UseFont = true;
      this.btManagmentServerState.Name = "btManagmentServerState";
      this.barManager1.Bars.AddRange(new Bar[3]
      {
        this.bar1,
        this.bar3,
        this.bar2
      });
      this.barManager1.DockControls.Add(this.barDockControlTop);
      this.barManager1.DockControls.Add(this.barDockControlBottom);
      this.barManager1.DockControls.Add(this.barDockControlLeft);
      this.barManager1.DockControls.Add(this.barDockControlRight);
      this.barManager1.Form = (Control) this;
      this.barManager1.Items.AddRange(new BarItem[14]
      {
        (BarItem) this.btAddDevice,
        (BarItem) this.btSaveDevice,
        (BarItem) this.btDeleteDevice,
        (BarItem) this.btShowVS,
        (BarItem) this.btShowDS,
        (BarItem) this.btShowES,
        (BarItem) this.btEditDevice,
        (BarItem) this.btShowManagmentServer,
        (BarItem) this.btShowIS,
        (BarItem) this.btManagmentServerState,
        (BarItem) this.btClear,
        (BarItem) this.btShowVideo,
        (BarItem) this.btReload,
        (BarItem) this.tbPeriod
      });
      this.barManager1.MainMenu = this.bar3;
      this.barManager1.MaxItemId = 17;
      this.barManager1.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.repositoryItemSpinEdit2
      });
      this.barManager1.StatusBar = this.bar2;
      this.bar1.BarName = "Tools";
      this.bar1.DockCol = 0;
      this.bar1.DockRow = 1;
      this.bar1.DockStyle = BarDockStyle.Top;
      this.bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[8]
      {
        new LinkPersistInfo((BarItem) this.btAddDevice),
        new LinkPersistInfo((BarItem) this.btDeleteDevice),
        new LinkPersistInfo((BarItem) this.btEditDevice),
        new LinkPersistInfo((BarItem) this.btSaveDevice),
        new LinkPersistInfo((BarItem) this.btReload),
        new LinkPersistInfo((BarItem) this.btShowVideo),
        new LinkPersistInfo((BarItem) this.btClear),
        new LinkPersistInfo((BarItem) this.tbPeriod)
      });
      this.bar1.OptionsBar.AllowQuickCustomization = false;
      componentResourceManager.ApplyResources((object) this.bar1, "bar1");
      this.btAddDevice.Border = BorderStyles.Default;
      this.btAddDevice.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btAddDevice.Glyph = (Image) Resources.document_new;
      this.btAddDevice.Id = 0;
      this.btAddDevice.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btAddDevice.ItemAppearance.Normal.Font");
      this.btAddDevice.ItemAppearance.Normal.Options.UseFont = true;
      this.btAddDevice.MinSize = new Size(30, 0);
      this.btAddDevice.Name = "btAddDevice";
      this.btAddDevice.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btAddDevice.ItemClick += new ItemClickEventHandler(this.btAddDevice_ItemClick);
      this.btDeleteDevice.Border = BorderStyles.Default;
      this.btDeleteDevice.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btDeleteDevice.Glyph = (Image) Resources.document_delete22;
      this.btDeleteDevice.Id = 2;
      this.btDeleteDevice.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btDeleteDevice.ItemAppearance.Normal.Font");
      this.btDeleteDevice.ItemAppearance.Normal.Options.UseFont = true;
      this.btDeleteDevice.MinSize = new Size(30, 0);
      this.btDeleteDevice.Name = "btDeleteDevice";
      this.btDeleteDevice.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btDeleteDevice.ItemClick += new ItemClickEventHandler(this.btDeleteDevice_ItemClick);
      this.btEditDevice.Border = BorderStyles.Default;
      this.btEditDevice.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btEditDevice.Glyph = (Image) Resources.edit_3_;
      this.btEditDevice.Id = 6;
      this.btEditDevice.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btEditDevice.ItemAppearance.Normal.Font");
      this.btEditDevice.ItemAppearance.Normal.Options.UseFont = true;
      this.btEditDevice.MinSize = new Size(30, 0);
      this.btEditDevice.Name = "btEditDevice";
      this.btEditDevice.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btEditDevice.ItemClick += new ItemClickEventHandler(this.btEditDevice_ItemClick);
      this.btSaveDevice.Border = BorderStyles.Default;
      this.btSaveDevice.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btSaveDevice.Glyph = (Image) Resources._22document_save;
      this.btSaveDevice.Id = 1;
      this.btSaveDevice.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btSaveDevice.ItemAppearance.Normal.Font");
      this.btSaveDevice.ItemAppearance.Normal.Options.UseFont = true;
      this.btSaveDevice.MinSize = new Size(30, 0);
      this.btSaveDevice.Name = "btSaveDevice";
      this.btSaveDevice.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btSaveDevice.ItemClick += new ItemClickEventHandler(this.btSaveDevice_ItemClick);
      this.btReload.Border = BorderStyles.Default;
      this.btReload.Glyph = (Image) componentResourceManager.GetObject("btReload.Glyph");
      this.btReload.Id = 13;
      this.btReload.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btReload.ItemAppearance.Normal.Font");
      this.btReload.ItemAppearance.Normal.Options.UseFont = true;
      this.btReload.Name = "btReload";
      this.btReload.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btReload.ItemClick += new ItemClickEventHandler(this.btReload_ItemClick);
      this.btShowVideo.Border = BorderStyles.Default;
      this.btShowVideo.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btShowVideo.Glyph = (Image) Resources.windows_media_player22;
      this.btShowVideo.Id = 12;
      this.btShowVideo.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btShowVideo.ItemAppearance.Normal.Font");
      this.btShowVideo.ItemAppearance.Normal.Options.UseFont = true;
      this.btShowVideo.Name = "btShowVideo";
      this.btShowVideo.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowVideo.ItemClick += new ItemClickEventHandler(this.btShowVideo_ItemClick);
      this.btClear.Border = BorderStyles.Default;
      this.btClear.CaptionAlignment = BarItemCaptionAlignment.Right;
      this.btClear.Glyph = (Image) Resources.dialog_cancel_2_;
      this.btClear.Id = 11;
      this.btClear.ItemAppearance.Normal.Font = (Font) componentResourceManager.GetObject("btClear.ItemAppearance.Normal.Font");
      this.btClear.ItemAppearance.Normal.Options.UseFont = true;
      this.btClear.Name = "btClear";
      this.btClear.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btClear.ItemClick += new ItemClickEventHandler(this.btClear_ItemClick);
      this.tbPeriod.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.tbPeriod, "tbPeriod");
      this.tbPeriod.Edit = (RepositoryItem) this.repositoryItemSpinEdit2;
      this.tbPeriod.EditValue = (object) "3";
      this.tbPeriod.Id = 14;
      this.tbPeriod.Name = "tbPeriod";
      this.tbPeriod.PaintStyle = BarItemPaintStyle.Caption;
      this.tbPeriod.EditValueChanged += new EventHandler(this.tbPeriod_EditValueChanged);
      this.tbPeriod.ItemClick += new ItemClickEventHandler(this.tbPeriod_ItemClick);
      componentResourceManager.ApplyResources((object) this.repositoryItemSpinEdit2, "repositoryItemSpinEdit2");
      this.repositoryItemSpinEdit2.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
      this.bar3.BarName = "Main menu";
      this.bar3.DockCol = 0;
      this.bar3.DockRow = 0;
      this.bar3.DockStyle = BarDockStyle.Top;
      this.bar3.FloatLocation = new Point(-219, 138);
      this.bar3.LinksPersistInfo.AddRange(new LinkPersistInfo[5]
      {
        new LinkPersistInfo((BarItem) this.btShowVS, true),
        new LinkPersistInfo((BarItem) this.btShowDS),
        new LinkPersistInfo((BarItem) this.btShowES),
        new LinkPersistInfo((BarItem) this.btShowIS),
        new LinkPersistInfo((BarItem) this.btShowManagmentServer)
      });
      this.bar3.OptionsBar.AllowQuickCustomization = false;
      this.bar3.OptionsBar.MultiLine = true;
      this.bar3.OptionsBar.UseWholeRow = true;
      componentResourceManager.ApplyResources((object) this.bar3, "bar3");
      this.btShowVS.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.btShowVS, "btShowVS");
      this.btShowVS.Id = 3;
      this.btShowVS.MinSize = new Size(150, 0);
      this.btShowVS.Name = "btShowVS";
      this.btShowVS.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowVS.ItemClick += new ItemClickEventHandler(this.btShowVS_ItemClick);
      this.btShowDS.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.btShowDS, "btShowDS");
      this.btShowDS.Id = 4;
      this.btShowDS.MinSize = new Size(150, 0);
      this.btShowDS.Name = "btShowDS";
      this.btShowDS.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowDS.ItemClick += new ItemClickEventHandler(this.btShowDS_ItemClick);
      this.btShowES.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.btShowES, "btShowES");
      this.btShowES.Id = 5;
      this.btShowES.MinSize = new Size(150, 0);
      this.btShowES.Name = "btShowES";
      this.btShowES.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowES.ItemClick += new ItemClickEventHandler(this.btShowES_ItemClick);
      this.btShowIS.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.btShowIS, "btShowIS");
      this.btShowIS.Id = 8;
      this.btShowIS.MinSize = new Size(150, 0);
      this.btShowIS.Name = "btShowIS";
      this.btShowIS.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowIS.ItemClick += new ItemClickEventHandler(this.btShowIdentificationServer_ItemClick);
      this.btShowManagmentServer.Border = BorderStyles.Default;
      componentResourceManager.ApplyResources((object) this.btShowManagmentServer, "btShowManagmentServer");
      this.btShowManagmentServer.Id = 7;
      this.btShowManagmentServer.MinSize = new Size(150, 0);
      this.btShowManagmentServer.Name = "btShowManagmentServer";
      this.btShowManagmentServer.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      this.btShowManagmentServer.ItemClick += new ItemClickEventHandler(this.btShowManagmentServer_ItemClick);
      this.barDockControlTop.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlTop, "barDockControlTop");
      this.barDockControlBottom.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlBottom, "barDockControlBottom");
      this.barDockControlLeft.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlLeft, "barDockControlLeft");
      this.barDockControlRight.CausesValidation = false;
      componentResourceManager.ApplyResources((object) this.barDockControlRight, "barDockControlRight");
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gridControl1);
      this.Controls.Add((Control) this.barDockControlLeft);
      this.Controls.Add((Control) this.barDockControlRight);
      this.Controls.Add((Control) this.barDockControlBottom);
      this.Controls.Add((Control) this.barDockControlTop);
      this.Name = "FrmDevices";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.frmDevices_FormClosing);
      this.Load += new EventHandler(this.frmDevices_Load);
      this.gridControl1.EndInit();
      this.gridView1.EndInit();
      this.btObjects.EndInit();
      this.btVideoServer.EndInit();
      this.btDetectorServer.EndInit();
      this.btExtractorServer.EndInit();
      this.btIdentificationServer.EndInit();
      this.cbMinFace.EndInit();
      this.chbFaces.EndInit();
      this.chbSaveImage.EndInit();
      this.chbNonCategory.EndInit();
      this.cbVideoServer.EndInit();
      this.cbDetectorServer.EndInit();
      this.repositoryItemGridLookUpEdit1.EndInit();
      this.cbImageSize.EndInit();
      this.repositoryItemSpinEdit1.EndInit();
      this.chbSecondDetector.EndInit();
      this.chbTracking.EndInit();
      this.repositoryItemGridLookUpEdit1View.EndInit();
      this.barManager1.EndInit();
      this.repositoryItemSpinEdit2.EndInit();
      this.ResumeLayout(false);
    }

    public enum ReceiveDataType
    {
      Detector,
      Extractor,
      Video,
      Identification,
    }

    public delegate void ReceiveDataEventHandler(object sender, FrmDevices.ReceiveDataEvenAgrs e);

    public delegate void ReceiveErrorEventHandler(object sender, Guid id, FrmDevices.ReceiveDataType type);

    public class ReceiveDataEvenAgrs : EventArgs
    {
      private FrmDevices.ReceiveDataType _type;
      private DataTable _data;

      public FrmDevices.ReceiveDataType Type
      {
        get
        {
          return this._type;
        }
      }

      public DataTable Data
      {
        get
        {
          return this._data;
        }
      }

      public ReceiveDataEvenAgrs(DataTable d, FrmDevices.ReceiveDataType type)
      {
        this._data = d;
        this._type = type;
      }
    }

    public class ManagmentServerCallback : IManagmentServerCallback
    {
      public Guid MainId = Guid.NewGuid();

      public event FrmDevices.ReceiveDataEventHandler GetData;

      public event FrmDevices.ReceiveErrorEventHandler GetError;

      public void CheckClient()
      {
      }

      public void SendDetectorInfo(DataTable devices)
      {
        if (this.GetData == null)
          return;
        this.GetData((object) this, new FrmDevices.ReceiveDataEvenAgrs(devices, FrmDevices.ReceiveDataType.Detector));
      }

      public void SendExtractorInfo(DataTable devices)
      {
        if (this.GetData == null)
          return;
        this.GetData((object) this, new FrmDevices.ReceiveDataEvenAgrs(devices, FrmDevices.ReceiveDataType.Extractor));
      }

      public void SendIdentificationInfo(DataTable devices)
      {
        if (this.GetData == null)
          return;
        this.GetData((object) this, new FrmDevices.ReceiveDataEvenAgrs(devices, FrmDevices.ReceiveDataType.Identification));
      }

      public void SendVideoInfo(DataTable devices)
      {
        if (this.GetData == null)
          return;
        this.GetData((object) this, new FrmDevices.ReceiveDataEvenAgrs(devices, FrmDevices.ReceiveDataType.Video));
      }

      public void SendVideoError(Guid id)
      {
        if (this.GetError == null)
          return;
        this.GetError((object) this, id, FrmDevices.ReceiveDataType.Video);
      }

      public void SendResults(DataTable id)
      {
      }

      public void SendDetectorError(Guid id)
      {
        if (this.GetError == null)
          return;
        this.GetError((object) this, id, FrmDevices.ReceiveDataType.Detector);
      }

      public void SendExtractorError(Guid id)
      {
        if (this.GetError == null)
          return;
        this.GetError((object) this, id, FrmDevices.ReceiveDataType.Extractor);
      }

      public void SendIdentificationError(Guid id)
      {
        if (this.GetError == null)
          return;
        this.GetError((object) this, id, FrmDevices.ReceiveDataType.Identification);
      }
    }

    private delegate void RefreshDataFunc(DataRow row, DeviceInfo info);

    private delegate void RefreshStateFunc(bool error);
  }
}
