// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmWorkStatins
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using BasicComponents;
using BasicComponents.ManagmentServer;
using CascadeManager.Properties;
using CS.DAL;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmWorkStatins : XtraForm
  {
    private List<BcObjects> _objects = new List<BcObjects>();
    private List<BcDevices> _devices = new List<BcDevices>();
    public List<BcDevices> ActiveDevices = new List<BcDevices>();
    public DataTable MainTable = new DataTable();
    private BcWorkStation _selectedStation = new BcWorkStation();
    private DataTable _dtRules = new DataTable();
    private IContainer components = null;
    public static Thread checkService;
    public ManagmentServerClient Service;
    public bool BreakFlag;
    private DataRow _selectedRow;
    private bool _isLoad;
    public bool IsChanged;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colName;
    private GridColumn colPosition;
    private GridColumn colIP;
    private GridColumn colState;
    private GridColumn colStatus;
    private GridColumn colHardwareKey;
    private GroupControl groupControl1;
    private TextEdit tbName;
    private LabelControl label1;
    private LabelControl labelControl3;
    private TextEdit tbHardwareKey;
    private LabelControl labelControl2;
    private TextEdit tbIP;
    private LabelControl labelControl1;
    private LabelControl label2;
    private TextEdit tbComment;
    private TextEdit tbState;
    private LabelControl labelControl4;
    private LabelControl lbServiceState;
    private LabelControl lbServerState;
    private ComboBoxEdit cbStatus;
    private SimpleButton btSave;
    private GroupControl groupControl2;
    private GridControl gridControl2;
    private GridView gridView2;
    private GridColumn gridColumn1;
    private GridColumn gridColumn2;
    private GridColumn gridColumn4;
    private GridColumn colComment;
    private GridColumn colAccess;
    private RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private SimpleButton btDelete;

    public FrmWorkStatins()
    {
      InitializeComponent();
      _objects = BcObjects.LoadAll();
      _devices = BcDevicesStorageExtensions.LoadAll();
    }

    private void RefreshData(DataTable data)
    {
      foreach (DataRow dataRow1 in (InternalDataCollectionBase) data.Rows)
      {
        bool flag = false;
        foreach (DataRow dataRow2 in (InternalDataCollectionBase) MainTable.Rows)
        {
          if (dataRow2["HardwareKey"].ToString() == dataRow1["HardwareKey"].ToString())
          {
            flag = true;
            dataRow2["ID"] = dataRow1["ID"];
            dataRow2["Name"] = dataRow1["Name"];
            dataRow2["Comment"] = dataRow1["Comment"];
            dataRow2["IP"] = dataRow1["IP"];
            dataRow2["HardwareKey"] = dataRow1["HardwareKey"];
            dataRow2["Connected"] = dataRow1["Connected"];
            dataRow2["Status"] = dataRow1["Status"];
            break;
          }
        }
        if (!flag)
        {
          DataRow dataRow2 = MainTable.Rows.Add();
          dataRow2["ID"] = dataRow1["ID"];
          dataRow2["Name"] = dataRow1["Name"];
          dataRow2["Comment"] = dataRow1["Comment"];
          dataRow2["IP"] = dataRow1["IP"];
          dataRow2["HardwareKey"] = dataRow1["HardwareKey"];
          dataRow2["Connected"] = dataRow1["Connected"];
          dataRow2["Status"] = dataRow1["Status"];
        }
      }
      for (int index = 0; index < MainTable.Rows.Count; ++index)
      {
        bool flag = false;
        foreach (DataRow dataRow in (InternalDataCollectionBase) data.Rows)
        {
          if (MainTable.Rows[index]["HardwareKey"].ToString() == dataRow["HardwareKey"].ToString())
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          MainTable.Rows.RemoveAt(index);
          --index;
        }
      }
    }

    private void RefreshStatus(string meaasage, int val)
    {
      if (val == 0)
        lbServiceState.Text = meaasage;
      else
        lbServerState.Text = meaasage;
    }

    private void CheckService()
    {
      while (true)
      {
        bool flag = true;
        if (!BreakFlag)
        {
          try
          {
            lock (_devices)
              _devices = BcDevicesStorageExtensions.LoadAll();
            BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
            CommonSettings.ManagmentServerAddress = "net.tcp://" + (object) bcManagmentServer.Ip + ":" + (string) (object) bcManagmentServer.Port + "/CSManagmentServer/ManagmentServer";
            Service = new ManagmentServerClient(new InstanceContext(new ClientCallback()));
            Service.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
            Service.Open();
            try
            {
              Invoke(new RefreshStatusFunc(RefreshStatus), (object) Messages.ManagmentServerWork, (object) 0);
            }
            catch
            {
            }
            while (true)
            {
              flag = true;
              if (!BreakFlag)
              {
                Invoke(new RefreshDataFunc(RefreshData), (object) Service.GetAllOperators());
                Thread.Sleep(3000);
              }
              else
                break;
            }
            try
            {
              Service.Abort();
            }
            catch
            {
            }
          }
          catch
          {
            try
            {
              Invoke(new RefreshStatusFunc(RefreshStatus), (object) Messages.ManagmentServerUnavailble, (object) 0);
            }
            catch
            {
            }
          }
          Thread.Sleep(2000);
        }
        else
          break;
      }
    }

    private void frmWorkStatins_Load(object sender, EventArgs e)
    {
      ControlBox = false;
      MainTable = new DataTable();
      MainTable.Columns.AddRange(new DataColumn[7]
      {
        new DataColumn("ID", typeof (Guid)),
        new DataColumn("Name"),
        new DataColumn("Comment"),
        new DataColumn("Connected"),
        new DataColumn("Status"),
        new DataColumn("IP"),
        new DataColumn("HardwareKey")
      });
      _dtRules = new DataTable();
      _dtRules.Columns.AddRange(new DataColumn[5]
      {
        new DataColumn("ID", typeof (Guid)),
        new DataColumn("Access", typeof (bool)),
        new DataColumn("Name", typeof (string)),
        new DataColumn("Comment", typeof (string)),
        new DataColumn("Position", typeof (string))
      });
      gridControl2.DataSource = _dtRules;
      checkService = new Thread(CheckService)
      {
        IsBackground = true
      };
      checkService.Start();
      gridControl1.DataSource = MainTable;
    }

    private void frmWorkStatins_Resize(object sender, EventArgs e)
    {
      ControlBox = false;
    }

    private void btChangeState_Click(object sender, EventArgs e)
    {
      if (gridView1.SelectedRowsCount > 0)
      {
        _selectedRow = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
        _selectedStation = BcWorkStation.LoadByHardwareKey(_selectedRow["HardwareKey"].ToString());
        if (cbStatus.SelectedIndex == 0)
        {
          _selectedStation.Status = true;
          _selectedRow["Status"] = Messages.Active;
        }
        else
        {
          _selectedRow["Status"] = Messages.NoActive;
          _selectedStation.Status = false;
        }
        _selectedStation.Name = tbName.Text;
        _selectedStation.Comment = tbComment.Text;
        _selectedStation.HardwareKey = tbHardwareKey.Text;
        _selectedStation.Save();
        new BcWorkStationRule
        {
          WorkStationId = _selectedStation.Id
        }.DeleteByWorkStationId();
        foreach (DataRow dataRow in (InternalDataCollectionBase) _dtRules.Rows)
        {
          if ((bool) dataRow["Access"])
            new BcWorkStationRule
            {
              WorkStationId = _selectedStation.Id,
              DeviceId = ((Guid) dataRow["ID"])
            }.Save();
        }
        new List<object>().AddRange(new object[4]
        {
	        _selectedStation.Id,
	        _selectedStation.Name,
	        _selectedStation.Comment,
	        (bool) (_selectedStation.Status ? 1 : 0)
        });
        try
        {
          _selectedRow["ID"] = _selectedStation.Id;
          _selectedRow["Name"] = _selectedStation.Name;
          _selectedRow["Comment"] = _selectedStation.Comment;
          BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
          CommonSettings.ManagmentServerAddress = "net.tcp://" + (object) bcManagmentServer.Ip + ":" + (string) (object) bcManagmentServer.Port + "/CSManagmentServer/ManagmentServer";
          ManagmentServerClient managmentServerClient = new ManagmentServerClient(new InstanceContext(new ClientCallback()));
          managmentServerClient.Endpoint.Address = new EndpointAddress(CommonSettings.ManagmentServerAddress);
          managmentServerClient.Open();
          managmentServerClient.SetWorkStationByKey(_selectedStation.HardwareKey);
          managmentServerClient.Close();
        }
        catch
        {
        }
      }
      IsChanged = false;
    }

    public void frmWorkStatins_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (IsChanged && XtraMessageBox.Show(Messages.RecordHasBeenChanged, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btChangeState_Click(sender, e);
      IsChanged = false;
      BreakFlag = true;
      try
      {
        checkService.Abort();
      }
      catch
      {
      }
      try
      {
        Service.Abort();
      }
      catch
      {
      }
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (IsChanged && XtraMessageBox.Show(Messages.RecordHasBeenChanged, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btChangeState_Click(sender, e);
      IsChanged = false;
      _isLoad = true;
      if (gridView1.SelectedRowsCount > 0)
      {
        DataRow dataRow = gridView1.GetDataRow(gridView1.GetSelectedRows()[0]);
        _selectedRow = dataRow;
        tbComment.Text = dataRow["Comment"].ToString();
        tbName.Text = dataRow["Name"].ToString();
        tbIP.Text = dataRow["IP"].ToString();
        cbStatus.SelectedIndex = !(dataRow["Status"].ToString() == Messages.NoActive) && !(dataRow["Status"].ToString() == Messages.Active) ? 1 : 0;
        tbState.Text = dataRow["Connected"].ToString();
        tbHardwareKey.Text = dataRow["HardwareKey"].ToString();
        _dtRules.Clear();
        List<BcWorkStationRule> list = new List<BcWorkStationRule>();
        if (dataRow["ID"].ToString() != "-1")
        {
          try
          {
            _selectedStation = BcWorkStation.LoadById((Guid) dataRow["ID"]);
            list = BcWorkStationRule.LoadAllByWorkStationId((Guid) dataRow["ID"]);
          }
          catch
          {
          }
        }
        else
          _selectedStation = new BcWorkStation();
        _dtRules.Rows.Clear();
        foreach (BcDevices bcDevices in _devices)
        {
          bool flag = false;
          foreach (BcWorkStationRule bcWorkStationRule in list)
          {
            if (bcWorkStationRule.DeviceId == bcDevices.Id)
            {
              flag = true;
              break;
            }
          }
          _dtRules.Rows.Add((object) bcDevices.Id, (object) (bool) (flag ? 1 : 0), (object) bcDevices.Name, (object) "");
          foreach (BcObjects bcObjects in _objects)
          {
            if (bcObjects.Id == bcDevices.ObjectId)
            {
              bcObjects.GetData();
              if (bcDevices.TableId != Guid.Empty)
                _dtRules.Rows[_dtRules.Rows.Count - 1][3] = bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, bcDevices.TableId).Name;
            }
          }
        }
      }
      _isLoad = false;
    }

    private void tbName_TextChanged(object sender, EventArgs e)
    {
      if (_isLoad)
        return;
      IsChanged = true;
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      List<object> list = new List<object>();
      _selectedStation.Delete();
      _selectedStation.Name = "";
      _selectedStation.Status = false;
      _selectedStation.Comment = "";
      list.AddRange(new object[4]
      {
	      _selectedStation.Id,
	      _selectedStation.Name,
	      _selectedStation.Comment,
	      (bool) (_selectedStation.Status ? 1 : 0)
      });
      try
      {
        _selectedRow["ID"] = Guid.Empty;
        _selectedRow["Name"] = "";
        _selectedRow["Comment"] = "";
        Service.DeleteOperator(_selectedStation.Id);
        MainTable.Rows.Remove(_selectedRow);
        Thread.Sleep(2000);
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void gridView2_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
    }

    private void gridView2_CellValueChanging(object sender, CellValueChangedEventArgs e)
    {
      if (_isLoad)
        return;
      IsChanged = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmWorkStatins));
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colName = new GridColumn();
      colPosition = new GridColumn();
      colIP = new GridColumn();
      colState = new GridColumn();
      colStatus = new GridColumn();
      colHardwareKey = new GridColumn();
      groupControl1 = new GroupControl();
      btDelete = new SimpleButton();
      groupControl2 = new GroupControl();
      gridControl2 = new GridControl();
      gridView2 = new GridView();
      gridColumn1 = new GridColumn();
      colAccess = new GridColumn();
      repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      gridColumn2 = new GridColumn();
      colComment = new GridColumn();
      gridColumn4 = new GridColumn();
      cbStatus = new ComboBoxEdit();
      lbServiceState = new LabelControl();
      btSave = new SimpleButton();
      lbServerState = new LabelControl();
      tbState = new TextEdit();
      labelControl4 = new LabelControl();
      labelControl3 = new LabelControl();
      tbHardwareKey = new TextEdit();
      labelControl2 = new LabelControl();
      tbIP = new TextEdit();
      labelControl1 = new LabelControl();
      label2 = new LabelControl();
      tbComment = new TextEdit();
      tbName = new TextEdit();
      label1 = new LabelControl();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      groupControl2.BeginInit();
      groupControl2.SuspendLayout();
      gridControl2.BeginInit();
      gridView2.BeginInit();
      repositoryItemCheckEdit1.BeginInit();
      cbStatus.Properties.BeginInit();
      tbState.Properties.BeginInit();
      tbHardwareKey.Properties.BeginInit();
      tbIP.Properties.BeginInit();
      tbComment.Properties.BeginInit();
      tbName.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridView1.Appearance.ColumnFilterButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BackColor");
      gridView1.Appearance.ColumnFilterButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BackColor2");
      gridView1.Appearance.ColumnFilterButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BorderColor");
      gridView1.Appearance.ColumnFilterButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.Font");
      gridView1.Appearance.ColumnFilterButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.ForeColor");
      gridView1.Appearance.ColumnFilterButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.GradientMode");
      gridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseFont = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor");
      gridView1.Appearance.ColumnFilterButtonActive.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor2");
      gridView1.Appearance.ColumnFilterButtonActive.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BorderColor");
      gridView1.Appearance.ColumnFilterButtonActive.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.Font");
      gridView1.Appearance.ColumnFilterButtonActive.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.ForeColor");
      gridView1.Appearance.ColumnFilterButtonActive.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.GradientMode");
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
      gridView1.Appearance.CustomizationFormHint.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.Font");
      gridView1.Appearance.CustomizationFormHint.Options.UseFont = true;
      gridView1.Appearance.DetailTip.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.Font");
      gridView1.Appearance.DetailTip.Options.UseFont = true;
      gridView1.Appearance.Empty.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Empty.BackColor");
      gridView1.Appearance.Empty.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Empty.Font");
      gridView1.Appearance.Empty.Options.UseBackColor = true;
      gridView1.Appearance.Empty.Options.UseFont = true;
      gridView1.Appearance.EvenRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.BackColor");
      gridView1.Appearance.EvenRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.Font");
      gridView1.Appearance.EvenRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.ForeColor");
      gridView1.Appearance.EvenRow.Options.UseBackColor = true;
      gridView1.Appearance.EvenRow.Options.UseFont = true;
      gridView1.Appearance.EvenRow.Options.UseForeColor = true;
      gridView1.Appearance.FilterCloseButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor");
      gridView1.Appearance.FilterCloseButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor2");
      gridView1.Appearance.FilterCloseButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BorderColor");
      gridView1.Appearance.FilterCloseButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.Font");
      gridView1.Appearance.FilterCloseButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.ForeColor");
      gridView1.Appearance.FilterCloseButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.GradientMode");
      gridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseFont = true;
      gridView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
      gridView1.Appearance.FilterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.BackColor");
      gridView1.Appearance.FilterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.Font");
      gridView1.Appearance.FilterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.ForeColor");
      gridView1.Appearance.FilterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FilterPanel.Options.UseFont = true;
      gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
      gridView1.Appearance.FixedLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.BackColor");
      gridView1.Appearance.FixedLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.Font");
      gridView1.Appearance.FixedLine.Options.UseBackColor = true;
      gridView1.Appearance.FixedLine.Options.UseFont = true;
      gridView1.Appearance.FocusedCell.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.BackColor");
      gridView1.Appearance.FocusedCell.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.Font");
      gridView1.Appearance.FocusedCell.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.ForeColor");
      gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
      gridView1.Appearance.FocusedCell.Options.UseFont = true;
      gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
      gridView1.Appearance.FocusedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.BackColor");
      gridView1.Appearance.FocusedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.Font");
      gridView1.Appearance.FocusedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.ForeColor");
      gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
      gridView1.Appearance.FocusedRow.Options.UseFont = true;
      gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
      gridView1.Appearance.FooterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor");
      gridView1.Appearance.FooterPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor2");
      gridView1.Appearance.FooterPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BorderColor");
      gridView1.Appearance.FooterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.Font");
      gridView1.Appearance.FooterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.ForeColor");
      gridView1.Appearance.FooterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.GradientMode");
      gridView1.Appearance.FooterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
      gridView1.Appearance.FooterPanel.Options.UseFont = true;
      gridView1.Appearance.FooterPanel.Options.UseForeColor = true;
      gridView1.Appearance.GroupButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BackColor");
      gridView1.Appearance.GroupButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BorderColor");
      gridView1.Appearance.GroupButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.Font");
      gridView1.Appearance.GroupButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.ForeColor");
      gridView1.Appearance.GroupButton.Options.UseBackColor = true;
      gridView1.Appearance.GroupButton.Options.UseBorderColor = true;
      gridView1.Appearance.GroupButton.Options.UseFont = true;
      gridView1.Appearance.GroupButton.Options.UseForeColor = true;
      gridView1.Appearance.GroupFooter.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BackColor");
      gridView1.Appearance.GroupFooter.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BorderColor");
      gridView1.Appearance.GroupFooter.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.Font");
      gridView1.Appearance.GroupFooter.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.ForeColor");
      gridView1.Appearance.GroupFooter.Options.UseBackColor = true;
      gridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
      gridView1.Appearance.GroupFooter.Options.UseFont = true;
      gridView1.Appearance.GroupFooter.Options.UseForeColor = true;
      gridView1.Appearance.GroupPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.BackColor");
      gridView1.Appearance.GroupPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.Font");
      gridView1.Appearance.GroupPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.ForeColor");
      gridView1.Appearance.GroupPanel.Options.UseBackColor = true;
      gridView1.Appearance.GroupPanel.Options.UseFont = true;
      gridView1.Appearance.GroupPanel.Options.UseForeColor = true;
      gridView1.Appearance.GroupRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.BackColor");
      gridView1.Appearance.GroupRow.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.BorderColor");
      gridView1.Appearance.GroupRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.Font");
      gridView1.Appearance.GroupRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.ForeColor");
      gridView1.Appearance.GroupRow.Options.UseBackColor = true;
      gridView1.Appearance.GroupRow.Options.UseBorderColor = true;
      gridView1.Appearance.GroupRow.Options.UseFont = true;
      gridView1.Appearance.GroupRow.Options.UseForeColor = true;
      gridView1.Appearance.HeaderPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BackColor");
      gridView1.Appearance.HeaderPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BackColor2");
      gridView1.Appearance.HeaderPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BorderColor");
      gridView1.Appearance.HeaderPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.Font");
      gridView1.Appearance.HeaderPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.ForeColor");
      gridView1.Appearance.HeaderPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.GradientMode");
      gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
      gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
      gridView1.Appearance.HeaderPanel.Options.UseFont = true;
      gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
      gridView1.Appearance.HideSelectionRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.BackColor");
      gridView1.Appearance.HideSelectionRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.Font");
      gridView1.Appearance.HideSelectionRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.ForeColor");
      gridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
      gridView1.Appearance.HideSelectionRow.Options.UseFont = true;
      gridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
      gridView1.Appearance.HorzLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.BackColor");
      gridView1.Appearance.HorzLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.Font");
      gridView1.Appearance.HorzLine.Options.UseBackColor = true;
      gridView1.Appearance.HorzLine.Options.UseFont = true;
      gridView1.Appearance.OddRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.BackColor");
      gridView1.Appearance.OddRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.OddRow.Font");
      gridView1.Appearance.OddRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.ForeColor");
      gridView1.Appearance.OddRow.Options.UseBackColor = true;
      gridView1.Appearance.OddRow.Options.UseFont = true;
      gridView1.Appearance.OddRow.Options.UseForeColor = true;
      gridView1.Appearance.Preview.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Preview.BackColor");
      gridView1.Appearance.Preview.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Preview.Font");
      gridView1.Appearance.Preview.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Preview.ForeColor");
      gridView1.Appearance.Preview.Options.UseBackColor = true;
      gridView1.Appearance.Preview.Options.UseFont = true;
      gridView1.Appearance.Preview.Options.UseForeColor = true;
      gridView1.Appearance.Row.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.BackColor");
      gridView1.Appearance.Row.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Row.Font");
      gridView1.Appearance.Row.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.ForeColor");
      gridView1.Appearance.Row.Options.UseBackColor = true;
      gridView1.Appearance.Row.Options.UseFont = true;
      gridView1.Appearance.Row.Options.UseForeColor = true;
      gridView1.Appearance.RowSeparator.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.BackColor");
      gridView1.Appearance.RowSeparator.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.Font");
      gridView1.Appearance.RowSeparator.Options.UseBackColor = true;
      gridView1.Appearance.RowSeparator.Options.UseFont = true;
      gridView1.Appearance.SelectedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.BackColor");
      gridView1.Appearance.SelectedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.Font");
      gridView1.Appearance.SelectedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.ForeColor");
      gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
      gridView1.Appearance.SelectedRow.Options.UseFont = true;
      gridView1.Appearance.SelectedRow.Options.UseForeColor = true;
      gridView1.Appearance.TopNewRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.Font");
      gridView1.Appearance.TopNewRow.Options.UseFont = true;
      gridView1.Appearance.VertLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.VertLine.BackColor");
      gridView1.Appearance.VertLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.VertLine.Font");
      gridView1.Appearance.VertLine.Options.UseBackColor = true;
      gridView1.Appearance.VertLine.Options.UseFont = true;
      gridView1.Appearance.ViewCaption.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.Font");
      gridView1.Appearance.ViewCaption.Options.UseFont = true;
      gridView1.Columns.AddRange(new GridColumn[7]
      {
        colID,
        colName,
        colPosition,
        colIP,
        colState,
        colStatus,
        colHardwareKey
      });
      gridView1.FixedLineWidth = 12;
      gridView1.GridControl = gridControl1;
      gridView1.IndicatorWidth = 40;
      gridView1.Name = "gridView1";
      gridView1.OptionsBehavior.Editable = false;
      gridView1.OptionsCustomization.AllowFilter = false;
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.FindMode = FindMode.Always;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.EnableAppearanceEvenRow = true;
      gridView1.OptionsView.EnableAppearanceOddRow = true;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.SelectionChanged += gridView1_SelectionChanged;
      colID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      colID.AppearanceCell.Options.UseFont = true;
      colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      colID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colID, "colID");
      colID.FieldName = "ID";
      colID.Name = "colID";
      colID.OptionsColumn.AllowEdit = false;
      colID.OptionsColumn.ReadOnly = true;
      colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      colName.AppearanceCell.Options.UseFont = true;
      colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      colName.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colName, "colName");
      colName.FieldName = "Name";
      colName.Name = "colName";
      colName.OptionsColumn.AllowEdit = false;
      colName.OptionsColumn.ReadOnly = true;
      colPosition.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceCell.Font");
      colPosition.AppearanceCell.Options.UseFont = true;
      colPosition.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      colPosition.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colPosition, "colPosition");
      colPosition.FieldName = "Comment";
      colPosition.Name = "colPosition";
      colPosition.OptionsColumn.AllowEdit = false;
      colPosition.OptionsColumn.ReadOnly = true;
      colIP.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceCell.Font");
      colIP.AppearanceCell.Options.UseFont = true;
      colIP.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceHeader.Font");
      colIP.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colIP, "colIP");
      colIP.FieldName = "ConnectionString";
      colIP.Name = "colIP";
      colIP.OptionsColumn.AllowEdit = false;
      colIP.OptionsColumn.ReadOnly = true;
      colState.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colState.AppearanceCell.Font");
      colState.AppearanceCell.Options.UseFont = true;
      colState.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colState.AppearanceHeader.Font");
      colState.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colState, "colState");
      colState.FieldName = "Connected";
      colState.Name = "colState";
      colState.OptionsColumn.AllowEdit = false;
      colState.OptionsColumn.ReadOnly = true;
      colStatus.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colStatus.AppearanceCell.Font");
      colStatus.AppearanceCell.Options.UseFont = true;
      colStatus.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colStatus.AppearanceHeader.Font");
      colStatus.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colStatus, "colStatus");
      colStatus.FieldName = "Status";
      colStatus.Name = "colStatus";
      colStatus.OptionsColumn.AllowEdit = false;
      colStatus.OptionsColumn.ReadOnly = true;
      colHardwareKey.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colHardwareKey.AppearanceCell.Font");
      colHardwareKey.AppearanceCell.Options.UseFont = true;
      colHardwareKey.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colHardwareKey.AppearanceHeader.Font");
      colHardwareKey.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colHardwareKey, "colHardwareKey");
      colHardwareKey.FieldName = "HardwareKey";
      colHardwareKey.Name = "colHardwareKey";
      colHardwareKey.OptionsColumn.AllowEdit = false;
      colHardwareKey.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Appearance.Font = (Font) componentResourceManager.GetObject("groupControl1.Appearance.Font");
      groupControl1.Appearance.Options.UseFont = true;
      groupControl1.Controls.Add(btDelete);
      groupControl1.Controls.Add(groupControl2);
      groupControl1.Controls.Add(cbStatus);
      groupControl1.Controls.Add(lbServiceState);
      groupControl1.Controls.Add(btSave);
      groupControl1.Controls.Add(lbServerState);
      groupControl1.Controls.Add(tbState);
      groupControl1.Controls.Add(labelControl4);
      groupControl1.Controls.Add(labelControl3);
      groupControl1.Controls.Add(tbHardwareKey);
      groupControl1.Controls.Add(labelControl2);
      groupControl1.Controls.Add(tbIP);
      groupControl1.Controls.Add(labelControl1);
      groupControl1.Controls.Add(label2);
      groupControl1.Controls.Add(tbComment);
      groupControl1.Controls.Add(tbName);
      groupControl1.Controls.Add(label1);
      groupControl1.Name = "groupControl1";
      groupControl1.ShowCaption = false;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.Options.UseFont = true;
      btDelete.Cursor = Cursors.Default;
      btDelete.Image = Resources.document_delete_4_;
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      componentResourceManager.ApplyResources(groupControl2, "groupControl2");
      groupControl2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Font");
      groupControl2.AppearanceCaption.Options.UseFont = true;
      groupControl2.Controls.Add(gridControl2);
      groupControl2.Name = "groupControl2";
      componentResourceManager.ApplyResources(gridControl2, "gridControl2");
      gridControl2.LookAndFeel.SkinName = "Office 2007 Blue";
      gridControl2.MainView = gridView2;
      gridControl2.Name = "gridControl2";
      gridControl2.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        repositoryItemCheckEdit1
      });
      gridControl2.ViewCollection.AddRange(new BaseView[1]
      {
        gridView2
      });
      gridView2.Appearance.ColumnFilterButton.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.BackColor");
      gridView2.Appearance.ColumnFilterButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.BackColor2");
      gridView2.Appearance.ColumnFilterButton.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.BorderColor");
      gridView2.Appearance.ColumnFilterButton.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.Font");
      gridView2.Appearance.ColumnFilterButton.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.ForeColor");
      gridView2.Appearance.ColumnFilterButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButton.GradientMode");
      gridView2.Appearance.ColumnFilterButton.Options.UseBackColor = true;
      gridView2.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
      gridView2.Appearance.ColumnFilterButton.Options.UseFont = true;
      gridView2.Appearance.ColumnFilterButton.Options.UseForeColor = true;
      gridView2.Appearance.ColumnFilterButtonActive.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.BackColor");
      gridView2.Appearance.ColumnFilterButtonActive.BackColor2 = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.BackColor2");
      gridView2.Appearance.ColumnFilterButtonActive.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.BorderColor");
      gridView2.Appearance.ColumnFilterButtonActive.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.Font");
      gridView2.Appearance.ColumnFilterButtonActive.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.ForeColor");
      gridView2.Appearance.ColumnFilterButtonActive.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView2.Appearance.ColumnFilterButtonActive.GradientMode");
      gridView2.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
      gridView2.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
      gridView2.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
      gridView2.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
      gridView2.Appearance.CustomizationFormHint.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.CustomizationFormHint.Font");
      gridView2.Appearance.CustomizationFormHint.Options.UseFont = true;
      gridView2.Appearance.DetailTip.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.DetailTip.Font");
      gridView2.Appearance.DetailTip.Options.UseFont = true;
      gridView2.Appearance.Empty.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.Empty.BackColor");
      gridView2.Appearance.Empty.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.Empty.Font");
      gridView2.Appearance.Empty.Options.UseBackColor = true;
      gridView2.Appearance.Empty.Options.UseFont = true;
      gridView2.Appearance.EvenRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.EvenRow.BackColor");
      gridView2.Appearance.EvenRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.EvenRow.Font");
      gridView2.Appearance.EvenRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.EvenRow.ForeColor");
      gridView2.Appearance.EvenRow.Options.UseBackColor = true;
      gridView2.Appearance.EvenRow.Options.UseFont = true;
      gridView2.Appearance.EvenRow.Options.UseForeColor = true;
      gridView2.Appearance.FilterCloseButton.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.BackColor");
      gridView2.Appearance.FilterCloseButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.BackColor2");
      gridView2.Appearance.FilterCloseButton.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.BorderColor");
      gridView2.Appearance.FilterCloseButton.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.Font");
      gridView2.Appearance.FilterCloseButton.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.ForeColor");
      gridView2.Appearance.FilterCloseButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView2.Appearance.FilterCloseButton.GradientMode");
      gridView2.Appearance.FilterCloseButton.Options.UseBackColor = true;
      gridView2.Appearance.FilterCloseButton.Options.UseBorderColor = true;
      gridView2.Appearance.FilterCloseButton.Options.UseFont = true;
      gridView2.Appearance.FilterCloseButton.Options.UseForeColor = true;
      gridView2.Appearance.FilterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterPanel.BackColor");
      gridView2.Appearance.FilterPanel.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FilterPanel.Font");
      gridView2.Appearance.FilterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FilterPanel.ForeColor");
      gridView2.Appearance.FilterPanel.Options.UseBackColor = true;
      gridView2.Appearance.FilterPanel.Options.UseFont = true;
      gridView2.Appearance.FilterPanel.Options.UseForeColor = true;
      gridView2.Appearance.FixedLine.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FixedLine.BackColor");
      gridView2.Appearance.FixedLine.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FixedLine.Font");
      gridView2.Appearance.FixedLine.Options.UseBackColor = true;
      gridView2.Appearance.FixedLine.Options.UseFont = true;
      gridView2.Appearance.FocusedCell.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FocusedCell.BackColor");
      gridView2.Appearance.FocusedCell.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FocusedCell.Font");
      gridView2.Appearance.FocusedCell.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FocusedCell.ForeColor");
      gridView2.Appearance.FocusedCell.Options.UseBackColor = true;
      gridView2.Appearance.FocusedCell.Options.UseFont = true;
      gridView2.Appearance.FocusedCell.Options.UseForeColor = true;
      gridView2.Appearance.FocusedRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FocusedRow.BackColor");
      gridView2.Appearance.FocusedRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FocusedRow.Font");
      gridView2.Appearance.FocusedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FocusedRow.ForeColor");
      gridView2.Appearance.FocusedRow.Options.UseBackColor = true;
      gridView2.Appearance.FocusedRow.Options.UseFont = true;
      gridView2.Appearance.FocusedRow.Options.UseForeColor = true;
      gridView2.Appearance.FooterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.BackColor");
      gridView2.Appearance.FooterPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.BackColor2");
      gridView2.Appearance.FooterPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.BorderColor");
      gridView2.Appearance.FooterPanel.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.Font");
      gridView2.Appearance.FooterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.ForeColor");
      gridView2.Appearance.FooterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView2.Appearance.FooterPanel.GradientMode");
      gridView2.Appearance.FooterPanel.Options.UseBackColor = true;
      gridView2.Appearance.FooterPanel.Options.UseBorderColor = true;
      gridView2.Appearance.FooterPanel.Options.UseFont = true;
      gridView2.Appearance.FooterPanel.Options.UseForeColor = true;
      gridView2.Appearance.GroupButton.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupButton.BackColor");
      gridView2.Appearance.GroupButton.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupButton.BorderColor");
      gridView2.Appearance.GroupButton.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.GroupButton.Font");
      gridView2.Appearance.GroupButton.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupButton.ForeColor");
      gridView2.Appearance.GroupButton.Options.UseBackColor = true;
      gridView2.Appearance.GroupButton.Options.UseBorderColor = true;
      gridView2.Appearance.GroupButton.Options.UseFont = true;
      gridView2.Appearance.GroupButton.Options.UseForeColor = true;
      gridView2.Appearance.GroupFooter.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupFooter.BackColor");
      gridView2.Appearance.GroupFooter.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupFooter.BorderColor");
      gridView2.Appearance.GroupFooter.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.GroupFooter.Font");
      gridView2.Appearance.GroupFooter.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupFooter.ForeColor");
      gridView2.Appearance.GroupFooter.Options.UseBackColor = true;
      gridView2.Appearance.GroupFooter.Options.UseBorderColor = true;
      gridView2.Appearance.GroupFooter.Options.UseFont = true;
      gridView2.Appearance.GroupFooter.Options.UseForeColor = true;
      gridView2.Appearance.GroupPanel.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupPanel.BackColor");
      gridView2.Appearance.GroupPanel.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.GroupPanel.Font");
      gridView2.Appearance.GroupPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupPanel.ForeColor");
      gridView2.Appearance.GroupPanel.Options.UseBackColor = true;
      gridView2.Appearance.GroupPanel.Options.UseFont = true;
      gridView2.Appearance.GroupPanel.Options.UseForeColor = true;
      gridView2.Appearance.GroupRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupRow.BackColor");
      gridView2.Appearance.GroupRow.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupRow.BorderColor");
      gridView2.Appearance.GroupRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.GroupRow.Font");
      gridView2.Appearance.GroupRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.GroupRow.ForeColor");
      gridView2.Appearance.GroupRow.Options.UseBackColor = true;
      gridView2.Appearance.GroupRow.Options.UseBorderColor = true;
      gridView2.Appearance.GroupRow.Options.UseFont = true;
      gridView2.Appearance.GroupRow.Options.UseForeColor = true;
      gridView2.Appearance.HeaderPanel.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.BackColor");
      gridView2.Appearance.HeaderPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.BackColor2");
      gridView2.Appearance.HeaderPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.BorderColor");
      gridView2.Appearance.HeaderPanel.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.Font");
      gridView2.Appearance.HeaderPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.ForeColor");
      gridView2.Appearance.HeaderPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView2.Appearance.HeaderPanel.GradientMode");
      gridView2.Appearance.HeaderPanel.Options.UseBackColor = true;
      gridView2.Appearance.HeaderPanel.Options.UseBorderColor = true;
      gridView2.Appearance.HeaderPanel.Options.UseFont = true;
      gridView2.Appearance.HeaderPanel.Options.UseForeColor = true;
      gridView2.Appearance.HideSelectionRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HideSelectionRow.BackColor");
      gridView2.Appearance.HideSelectionRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.HideSelectionRow.Font");
      gridView2.Appearance.HideSelectionRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HideSelectionRow.ForeColor");
      gridView2.Appearance.HideSelectionRow.Options.UseBackColor = true;
      gridView2.Appearance.HideSelectionRow.Options.UseFont = true;
      gridView2.Appearance.HideSelectionRow.Options.UseForeColor = true;
      gridView2.Appearance.HorzLine.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.HorzLine.BackColor");
      gridView2.Appearance.HorzLine.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.HorzLine.Font");
      gridView2.Appearance.HorzLine.Options.UseBackColor = true;
      gridView2.Appearance.HorzLine.Options.UseFont = true;
      gridView2.Appearance.OddRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.OddRow.BackColor");
      gridView2.Appearance.OddRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.OddRow.Font");
      gridView2.Appearance.OddRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.OddRow.ForeColor");
      gridView2.Appearance.OddRow.Options.UseBackColor = true;
      gridView2.Appearance.OddRow.Options.UseFont = true;
      gridView2.Appearance.OddRow.Options.UseForeColor = true;
      gridView2.Appearance.Preview.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.Preview.BackColor");
      gridView2.Appearance.Preview.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.Preview.Font");
      gridView2.Appearance.Preview.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.Preview.ForeColor");
      gridView2.Appearance.Preview.Options.UseBackColor = true;
      gridView2.Appearance.Preview.Options.UseFont = true;
      gridView2.Appearance.Preview.Options.UseForeColor = true;
      gridView2.Appearance.Row.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.Row.BackColor");
      gridView2.Appearance.Row.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.Row.Font");
      gridView2.Appearance.Row.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.Row.ForeColor");
      gridView2.Appearance.Row.Options.UseBackColor = true;
      gridView2.Appearance.Row.Options.UseFont = true;
      gridView2.Appearance.Row.Options.UseForeColor = true;
      gridView2.Appearance.RowSeparator.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.RowSeparator.BackColor");
      gridView2.Appearance.RowSeparator.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.RowSeparator.Font");
      gridView2.Appearance.RowSeparator.Options.UseBackColor = true;
      gridView2.Appearance.RowSeparator.Options.UseFont = true;
      gridView2.Appearance.SelectedRow.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.SelectedRow.BackColor");
      gridView2.Appearance.SelectedRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.SelectedRow.Font");
      gridView2.Appearance.SelectedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.SelectedRow.ForeColor");
      gridView2.Appearance.SelectedRow.Options.UseBackColor = true;
      gridView2.Appearance.SelectedRow.Options.UseFont = true;
      gridView2.Appearance.SelectedRow.Options.UseForeColor = true;
      gridView2.Appearance.TopNewRow.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.TopNewRow.Font");
      gridView2.Appearance.TopNewRow.Options.UseFont = true;
      gridView2.Appearance.VertLine.BackColor = (Color) componentResourceManager.GetObject("gridView2.Appearance.VertLine.BackColor");
      gridView2.Appearance.VertLine.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.VertLine.Font");
      gridView2.Appearance.VertLine.Options.UseBackColor = true;
      gridView2.Appearance.VertLine.Options.UseFont = true;
      gridView2.Appearance.ViewCaption.Font = (Font) componentResourceManager.GetObject("gridView2.Appearance.ViewCaption.Font");
      gridView2.Appearance.ViewCaption.Options.UseFont = true;
      gridView2.Columns.AddRange(new GridColumn[5]
      {
        gridColumn1,
        colAccess,
        gridColumn2,
        colComment,
        gridColumn4
      });
      gridView2.GridControl = gridControl2;
      gridView2.Name = "gridView2";
      gridView2.OptionsCustomization.AllowFilter = false;
      gridView2.OptionsFind.ClearFindOnClose = false;
      gridView2.OptionsFind.FindDelay = 10000;
      gridView2.OptionsFind.FindMode = FindMode.Always;
      gridView2.OptionsFind.ShowCloseButton = false;
      gridView2.OptionsSelection.MultiSelect = true;
      gridView2.OptionsView.EnableAppearanceEvenRow = true;
      gridView2.OptionsView.EnableAppearanceOddRow = true;
      gridView2.OptionsView.ShowGroupPanel = false;
      gridView2.CellValueChanged += gridView2_CellValueChanged;
      gridView2.CellValueChanging += gridView2_CellValueChanging;
      gridColumn1.AppearanceCell.Font = (Font) componentResourceManager.GetObject("gridColumn1.AppearanceCell.Font");
      gridColumn1.AppearanceCell.Options.UseFont = true;
      gridColumn1.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.Font");
      gridColumn1.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridColumn1, "gridColumn1");
      gridColumn1.FieldName = "ID";
      gridColumn1.Name = "gridColumn1";
      gridColumn1.OptionsColumn.AllowEdit = false;
      gridColumn1.OptionsColumn.ReadOnly = true;
      colAccess.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colAccess.AppearanceCell.Font");
      colAccess.AppearanceCell.Options.UseFont = true;
      colAccess.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colAccess.AppearanceHeader.Font");
      colAccess.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colAccess, "colAccess");
      colAccess.ColumnEdit = repositoryItemCheckEdit1;
      colAccess.FieldName = "Access";
      colAccess.Name = "colAccess";
      componentResourceManager.ApplyResources(repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      gridColumn2.AppearanceCell.Font = (Font) componentResourceManager.GetObject("gridColumn2.AppearanceCell.Font");
      gridColumn2.AppearanceCell.Options.UseFont = true;
      gridColumn2.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("gridColumn2.AppearanceHeader.Font");
      gridColumn2.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridColumn2, "gridColumn2");
      gridColumn2.FieldName = "Name";
      gridColumn2.Name = "gridColumn2";
      gridColumn2.OptionsColumn.AllowEdit = false;
      gridColumn2.OptionsColumn.ReadOnly = true;
      colComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceCell.Font");
      colComment.AppearanceCell.Options.UseFont = true;
      colComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceHeader.Font");
      colComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colComment, "colComment");
      colComment.FieldName = "Comment";
      colComment.Name = "colComment";
      colComment.OptionsColumn.AllowEdit = false;
      gridColumn4.AppearanceCell.Font = (Font) componentResourceManager.GetObject("gridColumn4.AppearanceCell.Font");
      gridColumn4.AppearanceCell.Options.UseFont = true;
      gridColumn4.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("gridColumn4.AppearanceHeader.Font");
      gridColumn4.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridColumn4, "gridColumn4");
      gridColumn4.FieldName = "Position";
      gridColumn4.Name = "gridColumn4";
      gridColumn4.OptionsColumn.AllowEdit = false;
      componentResourceManager.ApplyResources(cbStatus, "cbStatus");
      cbStatus.Name = "cbStatus";
      cbStatus.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbStatus.Properties.Appearance.Font");
      cbStatus.Properties.Appearance.Options.UseFont = true;
      cbStatus.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbStatus.Properties.Buttons"))
      });
      cbStatus.Properties.Items.AddRange(new object[2]
      {
        componentResourceManager.GetString("cbStatus.Properties.Items"),
        componentResourceManager.GetString("cbStatus.Properties.Items1")
      });
      cbStatus.TextChanged += tbName_TextChanged;
      lbServiceState.Appearance.Font = (Font) componentResourceManager.GetObject("lbServiceState.Appearance.Font");
      componentResourceManager.ApplyResources(lbServiceState, "lbServiceState");
      lbServiceState.Name = "lbServiceState";
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      btSave.Appearance.Options.UseFont = true;
      btSave.Cursor = Cursors.Default;
      btSave.Image = Resources.document_save_4_;
      btSave.Name = "btSave";
      btSave.Click += btChangeState_Click;
      lbServerState.Appearance.Font = (Font) componentResourceManager.GetObject("lbServerState.Appearance.Font");
      componentResourceManager.ApplyResources(lbServerState, "lbServerState");
      lbServerState.Name = "lbServerState";
      componentResourceManager.ApplyResources(tbState, "tbState");
      tbState.Name = "tbState";
      tbState.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbState.Properties.Appearance.Font");
      tbState.Properties.Appearance.Options.UseFont = true;
      tbState.Properties.ReadOnly = true;
      labelControl4.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl4.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl4, "labelControl4");
      labelControl4.Name = "labelControl4";
      labelControl3.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl3.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl3, "labelControl3");
      labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources(tbHardwareKey, "tbHardwareKey");
      tbHardwareKey.Name = "tbHardwareKey";
      tbHardwareKey.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbHardwareKey.Properties.Appearance.Font");
      tbHardwareKey.Properties.Appearance.Options.UseFont = true;
      tbHardwareKey.Properties.ReadOnly = true;
      labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl2, "labelControl2");
      labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources(tbIP, "tbIP");
      tbIP.Name = "tbIP";
      tbIP.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbIP.Properties.Appearance.Font");
      tbIP.Properties.Appearance.Options.UseFont = true;
      tbIP.Properties.ReadOnly = true;
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Name = "labelControl1";
      label2.Appearance.Font = (Font) componentResourceManager.GetObject("label2.Appearance.Font");
      componentResourceManager.ApplyResources(label2, "label2");
      label2.Name = "label2";
      componentResourceManager.ApplyResources(tbComment, "tbComment");
      tbComment.Name = "tbComment";
      tbComment.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbComment.Properties.Appearance.Font");
      tbComment.Properties.Appearance.Options.UseFont = true;
      tbComment.Properties.Appearance.Options.UseTextOptions = true;
      tbComment.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
      tbComment.Properties.Appearance.TextOptions.VAlignment = VertAlignment.Top;
      tbComment.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbComment.Properties.AutoHeight");
      tbComment.TextChanged += tbName_TextChanged;
      componentResourceManager.ApplyResources(tbName, "tbName");
      tbName.Name = "tbName";
      tbName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbName.Properties.Appearance.Font");
      tbName.Properties.Appearance.Options.UseFont = true;
      tbName.TextChanged += tbName_TextChanged;
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(groupControl1);
      Controls.Add(gridControl1);
      FormBorderStyle = FormBorderStyle.None;
      Name = "FrmWorkStatins";
      ShowIcon = false;
      WindowState = FormWindowState.Maximized;
      FormClosing += frmWorkStatins_FormClosing;
      Load += frmWorkStatins_Load;
      Resize += frmWorkStatins_Resize;
      gridControl1.EndInit();
      gridView1.EndInit();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      groupControl1.PerformLayout();
      groupControl2.EndInit();
      groupControl2.ResumeLayout(false);
      gridControl2.EndInit();
      gridView2.EndInit();
      repositoryItemCheckEdit1.EndInit();
      cbStatus.Properties.EndInit();
      tbState.Properties.EndInit();
      tbHardwareKey.Properties.EndInit();
      tbIP.Properties.EndInit();
      tbComment.Properties.EndInit();
      tbName.Properties.EndInit();
      ResumeLayout(false);
    }

    public class ClientCallback : IManagmentServerCallback
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

      public void SendVideoInfo(DataTable devices)
      {
      }

      public void SendResults(DataTable id)
      {
      }
    }

    private delegate void RefreshDataFunc(DataTable data);

    private delegate void RefreshStatusFunc(string meaasage, int val);
  }
}
