// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmResults
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BasicComponents;
using CascadeManager.CascadeFlowClient;
using CascadeManager.Properties;
using CS.DAL;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace CascadeManager
{
  public class FrmResults : XtraForm
  {
    private DataTable _dtdeletedRows = new DataTable();
    private Bitmap _imgWarning = new Bitmap(64, 64);
    private Bitmap _imginfo = new Bitmap(64, 64);
    private float _period = 1f;
    private BcAccessCategory _unCategory = new BcAccessCategory();
    private float _maxScore = 0.55f;
    public List<BcDevices> Devices = new List<BcDevices>();
    private List<BcAccessCategory> _categories = new List<BcAccessCategory>();
    private List<BcObjects> _objects = new List<BcObjects>();
    private List<BcLogBases> _bases = new List<BcLogBases>();
    public DsResult Results = new DsResult();
    public DataTable MainTable = new DataTable();
    private DataTable _removedrows = new DataTable();
    private IContainer components = null;
    private bool _removedState;
    private Panel groupBox2;
    private LabelControl lbSex;
    private LabelControl lbName;
    private PictureBox pbImgSource;
    private PictureBox pbImgResult;
    private LabelControl lbCategory;
    private SimpleButton btShow;
    private LabelControl label2;
    private LabelControl label1;
    private GroupControl groupBox1;
    private GroupControl groupBox3;
    private DateEdit dtpFrom;
    private SimpleButton btSearch;
    private SimpleButton btPrint;
    private LabelControl label6;
    private LabelControl label5;
    private LabelControl lbCategoryResult;
    private LabelControl lbSexResult;
    private LabelControl lbNameResult;
    private CheckedListBoxControl lvCategory;
    private CheckedListBoxControl lvObjects;
    private SimpleButton btDelete;
    private SimpleButton btEdit;
    private CheckEdit chbHide;
    private DateEdit dtpBefore;
    private CheckEdit checkEditFrom;
    private CheckEdit checkEditBefore;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colFaceID;
    private GridColumn colCategoryID;
    private GridColumn colObjectID;
    private GridColumn colDate;
    private GridColumn colScore;
    private GridColumn colImage;
    private GridColumn colName;
    private GridColumn colCategory;
    private GridColumn colStatus;
    private GridColumn colPosition;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private RepositoryItemPictureEdit repositoryItemPictureEdit2;
    private RepositoryItemDateEdit repositoryItemDateEdit1;
    private GridColumn GroupColumn;
    private SimpleButton btCollapse;
    private SimpleButton btExpand;
    private CheckEdit chbGroup;
    private CheckEdit chbCollapse;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem toolStripMenuItem3;
    private GridColumn colImageID;
    private LabelControl labelControl2;
    private SpinEdit tbPeriod;
    private GridColumn colDeviceName;
    private GridColumn colTime;
    private GridColumn colKSID;

    public FrmResults()
    {
      InitializeComponent();
      _imgWarning = Resources.Actions_list_add_user_icon36;
      _imginfo = Resources.Actions_list_remove_user_icon36;
      ReloadObjects();
      MainTable.Columns.Add("ID", typeof (int));
      MainTable.Columns.Add("FaceID", typeof (Guid));
      MainTable.Columns.Add("DeviceID", typeof (Guid));
      MainTable.Columns.Add("CategoryID", typeof (int));
      MainTable.Columns.Add("ObjectID", typeof (int));
      MainTable.Columns.Add("Date", typeof (DateTime));
      MainTable.Columns.Add("Time", typeof (string));
      MainTable.Columns.Add("Score", typeof (float));
      MainTable.Columns.Add("imgType", typeof (Bitmap));
      MainTable.Columns.Add("ImageIcon", typeof (Bitmap));
      MainTable.Columns.Add("Name", typeof (string));
      MainTable.Columns.Add("Category", typeof (string));
      MainTable.Columns.Add("Position", typeof (string));
      MainTable.Columns.Add("Status", typeof (bool));
      MainTable.Columns.Add("GroupColumn", typeof (string));
      MainTable.Columns.Add("ImageID", typeof (Guid));
      MainTable.Columns.Add("DeviceName", typeof (string));
      MainTable.Columns.Add("Number", typeof (int));
      _removedrows.Columns.Add("ID", typeof (int));
      _removedrows.Columns.Add("FaceID", typeof (Guid));
      _removedrows.Columns.Add("DeviceID", typeof (Guid));
      _removedrows.Columns.Add("CategoryID", typeof (int));
      _removedrows.Columns.Add("ObjectID", typeof (int));
      _removedrows.Columns.Add("Date", typeof (DateTime));
      _removedrows.Columns.Add("Time", typeof (string));
      _removedrows.Columns.Add("Score", typeof (float));
      _removedrows.Columns.Add("imgType", typeof (Bitmap));
      _removedrows.Columns.Add("ImageIcon", typeof (Bitmap));
      _removedrows.Columns.Add("Name", typeof (string));
      _removedrows.Columns.Add("Category", typeof (string));
      _removedrows.Columns.Add("Position", typeof (string));
      _removedrows.Columns.Add("Status", typeof (bool));
      _removedrows.Columns.Add("GroupColumn", typeof (string));
      _removedrows.Columns.Add("ImageID", typeof (Guid));
      _removedrows.Columns.Add("DeviceName", typeof (string));
      _removedrows.Columns.Add("Number", typeof (int));
      _dtdeletedRows.Columns.Add("ID", typeof (int));
      _dtdeletedRows.Columns.Add("FaceID", typeof (Guid));
      _dtdeletedRows.Columns.Add("DeviceID", typeof (Guid));
      _dtdeletedRows.Columns.Add("CategoryID", typeof (int));
      _dtdeletedRows.Columns.Add("ObjectID", typeof (int));
      _dtdeletedRows.Columns.Add("Date", typeof (DateTime));
      _dtdeletedRows.Columns.Add("Time", typeof (string));
      _dtdeletedRows.Columns.Add("Score", typeof (float));
      _dtdeletedRows.Columns.Add("imgType", typeof (Bitmap));
      _dtdeletedRows.Columns.Add("ImageIcon", typeof (Bitmap));
      _dtdeletedRows.Columns.Add("Name", typeof (string));
      _dtdeletedRows.Columns.Add("Category", typeof (string));
      _dtdeletedRows.Columns.Add("Position", typeof (string));
      _dtdeletedRows.Columns.Add("Status", typeof (bool));
      _dtdeletedRows.Columns.Add("GroupColumn", typeof (string));
      _dtdeletedRows.Columns.Add("ImageID", typeof (Guid));
      _dtdeletedRows.Columns.Add("DeviceName", typeof (string));
      _dtdeletedRows.Columns.Add("Number", typeof (int));
    }

    public void ReloadObjects()
    {
      try
      {
        Results.dtImageType.Rows.Clear();
        Results.dtCategories.Rows.Clear();
        Results.dtDevices.Rows.Clear();
        _categories = BcAccessCategory.LoadAll();
        Devices = BcDevicesStorageExtensions.LoadAll();
        _objects = BcObjects.LoadAll();
        _bases = BcLogBases.LoadAll();
        lvObjects.Items.Clear();
        lvCategory.Items.Clear();
        foreach (BcDevices bcDevices in Devices)
        {
          lvObjects.Items.Add(bcDevices.Name);
          lvObjects.Items[lvObjects.Items.Count - 1].Value = bcDevices.Id;
          lvObjects.Items[lvObjects.Items.Count - 1].Description = bcDevices.Name;
        }
        Results.dtImageType.Rows.Add((object) 1, (object) _imgWarning);
        Results.dtImageType.Rows.Add((object) 2, (object) new Bitmap(128, 128));
        foreach (BcAccessCategory bcAccessCategory in _categories)
        {
          if (!bcAccessCategory.InCategory)
            _unCategory = bcAccessCategory;
          Results.dtCategories.Rows.Add((object) bcAccessCategory.Id, (object) bcAccessCategory.Name);
          lvCategory.Items.Add(bcAccessCategory.Id);
          lvCategory.Items[lvCategory.Items.Count - 1].Description = bcAccessCategory.Name;
        }
        GetScore();
      }
      catch
      {
      }
    }

    public float[] GetScore()
    {
      float[] numArray = new float[5];
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select * from ScoreSettings", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
          numArray[0] = Convert.ToSingle(sqlDataReader[0]);
          _maxScore = numArray[1] = Convert.ToSingle(sqlDataReader[1]);
          numArray[2] = Convert.ToSingle(sqlDataReader[2]);
          numArray[3] = Convert.ToSingle(sqlDataReader[3]);
        }
        sqlCommand.Connection.Close();
      }
      catch
      {
      }
      return numArray;
    }

    private void frmResults_Load(object sender, EventArgs e)
    {
      dtpBefore.DateTime = DateTime.Now;
      dtpFrom.DateTime = DateTime.Now;
      ControlBox = false;
      int[] actions = MainForm.CurrentUser.GetActions();
      btDelete.Enabled = false;
      foreach (int num in actions)
      {
        if (num == 14)
        {
          btDelete.Enabled = true;
          break;
        }
      }
    }

    private void RefreshGrid()
    {
      _period = (int) tbPeriod.Value;
      _dtdeletedRows.Rows.Clear();
      chbCollapse.Checked = false;
      Cursor = Cursors.WaitCursor;
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < lvCategory.Items.Count; ++index)
      {
        if (lvCategory.Items[index].CheckState == CheckState.Checked)
        {
          if (lvCategory.Items[index].Description == Messages.NonCategory)
            str2 = str2 + (object) " ( Log.Category like '" + Messages.NonCategory + "' or Log.Category is null or face.AccessID =" + (string) lvCategory.Items[index].Value + ") or ";
          else
            str2 = string.Concat((object) str2, (object) " face.AccessID = ", lvCategory.Items[index].Value, (object) " or ");
        }
      }
      if (str2 != "")
        str1 = str1 + "( " + str2.Substring(0, str2.Length - 3) + " ) and ";
      string str3 = "";
      for (int index = 0; index < lvObjects.Items.Count; ++index)
      {
        if (lvObjects.Items[index].CheckState == CheckState.Checked)
          str3 = string.Concat((object) str3, (object) "Log.DeviceID = '", lvObjects.Items[index].Value, (object) "' or ");
      }
      if (str3 != "")
        str1 = str1 + "( " + str3.Substring(0, str3.Length - 3) + " ) and ";
      string str4 = !checkEditFrom.Checked || !checkEditBefore.Checked ? (!checkEditBefore.Checked ? (!checkEditFrom.Checked ? " (Log.Date>=@datefrom) and " : " (Log.Date>=@datefrom) and ") : " (Log.Date <= @datebefore) and ") : " (Log.Date>=@datefrom and Log.Date <= @datebefore) and ";
      if (str4 != "")
        str1 += str4;
      if (str1 != "")
        str1 = str1.Substring(0, str1.Length - 4);
      string str5 = "Select\r\nLog.ID ,\r\nlog.faceID,\r\nLog.DeviceID,\r\nface.AccessID as CategoryID,\r\nLog.ObjectID,\r\n0 as imgTypeID,\r\nLog.Date,\r\nCONVERT(VARCHAR(8),Date,108) as 'Time',\r\nLog.Score,\r\nLog.ImageIcon,\r\nface.Surname+' '+face.FirstName+' '+ face.LastName as Name,\r\nLog.Status,\r\nCascadeStream.dbo.AccessCategory.Name as Category,ImageID,\r\ndev.Name as DeviceName,\r\ndev.Number as Number\r\n\r\n\r\nfrom  [dbo].[Log] left outer join CascadeStream.dbo.Faces as face  on face.ID = Log.FaceID left outer join \r\nCascadeStream.dbo.Devices as dev on Log.DeviceID = dev.ID\r\nleft outer join \r\nCascadeStream.dbo.AccessCategory on\r\nface.AccessID = CascadeStream.dbo.AccessCategory.ID\r\n\r\n";
      if (str1 != "")
        str5 = str5 + " Where " + str1;
      string cmdText = str5 + " order by FaceID, Date desc";
      MainTable.Rows.Clear();
      gridControl1.DataSource = null;
      SqlCommand sqlCommand = new SqlCommand(cmdText, new SqlConnection(CommonSettings.ConnectionStringLog));
      sqlCommand.CommandTimeout = 0;
      sqlCommand.Connection.Open();
      if (checkEditFrom.Checked)
        sqlCommand.Parameters.AddWithValue("@datefrom", dtpFrom.DateTime);
      if (checkEditBefore.Checked)
        sqlCommand.Parameters.AddWithValue("@datebefore", dtpBefore.DateTime);
      if (!checkEditBefore.Checked && !checkEditFrom.Checked)
        sqlCommand.Parameters.AddWithValue("@datefrom", DateTime.Now.Date);
      try
      {
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        DateTime dateTime1 = new DateTime();
        string str6 = "";
        string str7 = "";
        while (sqlDataReader.Read())
        {
          MainTable.Rows.Add();
          MainTable.Rows[MainTable.Rows.Count - 1]["ID"] = sqlDataReader["ID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["ImageID"] = sqlDataReader["ImageID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["FaceID"] = sqlDataReader["FaceID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["DeviceID"] = sqlDataReader["DeviceID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["CategoryID"] = sqlDataReader["CategoryID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["ObjectID"] = sqlDataReader["ObjectID"];
          MainTable.Rows[MainTable.Rows.Count - 1]["Date"] = sqlDataReader["Date"];
          MainTable.Rows[MainTable.Rows.Count - 1]["Time"] = sqlDataReader["Time"];
          MainTable.Rows[MainTable.Rows.Count - 1]["Score"] = sqlDataReader["Score"];
          MainTable.Rows[MainTable.Rows.Count - 1]["DeviceName"] = sqlDataReader["DeviceName"];
          MainTable.Rows[MainTable.Rows.Count - 1]["Number"] = sqlDataReader["Number"];
          Bitmap bitmap = new Bitmap(new MemoryStream((byte[]) sqlDataReader["ImageIcon"]));
          Graphics.FromImage(bitmap).DrawRectangle(new Pen(Brushes.Blue, 2.5f), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
          MainTable.Rows[MainTable.Rows.Count - 1]["ImageIcon"] = bitmap;
          MainTable.Rows[MainTable.Rows.Count - 1]["Name"] = sqlDataReader["Name"];
          MainTable.Rows[MainTable.Rows.Count - 1]["Category"] = sqlDataReader["Category"];
          if (MainTable.Rows[MainTable.Rows.Count - 1]["Category"].ToString() == "")
            MainTable.Rows[MainTable.Rows.Count - 1]["Category"] = Messages.NonCategory;
          MainTable.Rows[MainTable.Rows.Count - 1]["Status"] = sqlDataReader["Status"];
          if (Results.dtDevices.Select("DeviceID = '" + MainTable.Rows[MainTable.Rows.Count - 1]["DeviceID"] + "'").Length > 0)
            MainTable.Rows[MainTable.Rows.Count - 1]["Position"] = Results.dtDevices.Select("DeviceID = '" + MainTable.Rows[MainTable.Rows.Count - 1]["DeviceID"] + "'")[0]["Position"];
          else
            MainTable.Rows[MainTable.Rows.Count - 1]["Position"] = "";
          BcAccessCategory bcAccessCategory1 = new BcAccessCategory();
          BcAccessCategory bcAccessCategory2 = !(sqlDataReader[3].ToString() != "") ? _unCategory : GetCategoryById(Convert.ToInt32(sqlDataReader[3]));
          BcDevices deviceById = GetDeviceById((Guid) sqlDataReader[2]);
          if (deviceById.ObjectId != -1)
          {
            BcObjects objectById1 = GetObjectById(bcAccessCategory2.Data, deviceById.ObjectId);
            BcObjectsData objectById2 = BcObjectsData.GetObjectById(objectById1.Data, deviceById.TableId);
            if (deviceById.TableId == Guid.Empty && !objectById1.Warning && (objectById1.InList || objectById1.Sound))
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imginfo;
            else if (deviceById.TableId == Guid.Empty && objectById1.Warning)
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imgWarning;
            else if (deviceById.TableId != Guid.Empty && objectById2.Warning)
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imgWarning;
            else if (deviceById.TableId != Guid.Empty && (objectById2.InList || objectById2.Sound))
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imginfo;
            else if (deviceById.TableId == Guid.Empty && (objectById1.InList || objectById1.Sound))
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imginfo;
            else
              MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imginfo;
          }
          else
            MainTable.Rows[MainTable.Rows.Count - 1]["imgType"] = _imginfo;
          DateTime dateTime2 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
          int num = dateTime2.Month;
          string str8 = num.ToString();
          if (dateTime2.Month < 9)
            str8 = "0" + dateTime2.Month;
          num = dateTime2.Day;
          string str9 = num.ToString();
          if (dateTime2.Day < 9)
            str9 = "0" + dateTime2.Day;
          string str10 = (string) (object) dateTime2.Year + (object) str8 + str9;
          string str11 = sqlDataReader[1].ToString();
          Guid guid;
          if (MainTable.Rows.Count == 1)
          {
            if (sqlDataReader[1].ToString() != "" && sqlDataReader[1].ToString() != "-1")
            {
              dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
              string str12 = MainTable.Rows[MainTable.Rows.Count - 1]["Name"].ToString();
              MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str10 + (object) " %ID" + str11 + "%Date" + (string) (object) dateTime2 + " %Name" + str12 + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second - (int) _period) + ")";
              str7 = MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"].ToString();
            }
            else
            {
              dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
              guid = Guid.NewGuid();
              str6 = guid.ToString();
              MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str10 + (object) "%Date" + (string) (object) dateTime2 + " %Name" + (string) (object) Guid.NewGuid() + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second - (int) _period) + ")";
              str7 = MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"].ToString();
            }
          }
          else if (str11 == MainTable.Rows[MainTable.Rows.Count - 2]["FaceID"].ToString())
          {
            if (_period > (dateTime1 - (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"]).TotalSeconds)
            {
              dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
              MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str7;
            }
            else
            {
              dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
              string str12 = MainTable.Rows[MainTable.Rows.Count - 1]["Name"].ToString();
              MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str10 + (object) " %ID" + str11 + "%Date" + (string) (object) dateTime2 + " %Name" + str12 + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second - (int) _period) + ")";
              str7 = MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"].ToString();
            }
          }
          else if (sqlDataReader[1].ToString() != "" && sqlDataReader[1].ToString() != "-1")
          {
            dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
            string str12 = MainTable.Rows[MainTable.Rows.Count - 1]["Name"].ToString();
            MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str10 + (object) " %ID" + str11 + "%Date" + (string) (object) dateTime2 + " %Name" + str12 + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second - (int) _period) + ")";
            str7 = MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"].ToString();
          }
          else
          {
            dateTime1 = (DateTime) MainTable.Rows[MainTable.Rows.Count - 1]["Date"];
            guid = Guid.NewGuid();
            str6 = guid.ToString();
            MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"] = str10 + (object) "%Date" + (string) (object) dateTime2 + " %Name" + (string) (object) Guid.NewGuid() + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second - (int) _period) + ")";
            str7 = MainTable.Rows[MainTable.Rows.Count - 1]["GroupColumn"].ToString();
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      sqlCommand.Connection.Close();
      gridControl1.DataSource = MainTable;
      if (chbGroup.Checked)
      {
        gridView1.Columns["GroupColumn"].Group();
        gridView1.Columns["GroupColumn"].Visible = true;
      }
      else
      {
        gridView1.Columns["GroupColumn"].UnGroup();
        gridView1.Columns["GroupColumn"].Visible = false;
      }
      Cursor = Cursors.Default;
    }

    private BcDevices GetDeviceById(Guid id)
    {
      BcDevices bcDevices1 = new BcDevices();
      foreach (BcDevices bcDevices2 in Devices)
      {
        if (id == bcDevices2.Id)
        {
          bcDevices1 = bcDevices2;
          break;
        }
      }
      return bcDevices1;
    }

    private BcAccessCategory GetCategoryById(int id)
    {
      BcAccessCategory bcAccessCategory1 = new BcAccessCategory();
      foreach (BcAccessCategory bcAccessCategory2 in _categories)
      {
        if (id == bcAccessCategory2.Id)
        {
          bcAccessCategory1 = bcAccessCategory2;
          break;
        }
      }
      return bcAccessCategory1;
    }

    private BcObjects GetObjectById(List<BcObjects> obj, int id)
    {
      BcObjects bcObjects1 = new BcObjects();
      foreach (BcObjects bcObjects2 in obj)
      {
        if (id == bcObjects2.Id)
        {
          bcObjects1 = bcObjects2;
          break;
        }
      }
      return bcObjects1;
    }

    private void frmResults_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void btShow_Click(object sender, EventArgs e)
    {
      if (gridView1.SelectedRowsCount <= 0 || pbImgSource.Image == null || !(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[1].ToString() != ""))
        return;
      int num = (int) new FrmSearch((Bitmap) pbImgResult.Image, (Guid) gridView1.GetDataRow(gridView1.FocusedRowHandle)[1], Convert.ToDateTime(gridView1.GetDataRow(gridView1.FocusedRowHandle)[5]), (Guid) gridView1.GetDataRow(gridView1.FocusedRowHandle)["ImageID"]).ShowDialog();
    }

    private void btSearch_Click(object sender, EventArgs e)
    {
      RefreshGrid();
    }

    private void btPrint_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Excel 2003|*.xls";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      gridControl1.ExportToXls(saveFileDialog.FileName);
      Process.Start(saveFileDialog.FileName);
    }

    private void btClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        int[] selectedRows = gridView1.GetSelectedRows();
        _removedState = true;
        try
        {
          SqlCommand sqlCommand1 = new SqlCommand("Delete CSLogINfo.dbo.Log Where ");
          foreach (int rowHandle in selectedRows)
          {
            if (rowHandle >= 0)
            {
              DataRow dataRow1 = gridView1.GetDataRow(rowHandle);
              SqlCommand sqlCommand2 = sqlCommand1;
              string str1 = string.Concat((object) sqlCommand2.CommandText, (object) "ID = ", dataRow1["ID"], (object) " or ");
              sqlCommand2.CommandText = str1;
              if (chbCollapse.Checked)
              {
                foreach (DataRow dataRow2 in _dtdeletedRows.Select("GroupColumn = '" + dataRow1["GroupColumn"] + "'"))
                {
                  SqlCommand sqlCommand3 = sqlCommand1;
                  string str2 = string.Concat((object) sqlCommand3.CommandText, (object) "ID = ", dataRow2["ID"], (object) " or ");
                  sqlCommand3.CommandText = str2;
                }
              }
            }
          }
          if (selectedRows.Length > 0)
          {
            sqlCommand1.CommandText = sqlCommand1.CommandText.Substring(0, sqlCommand1.CommandText.Length - 3);
            sqlCommand1.Connection = new SqlConnection(CommonSettings.ConnectionStringLog);
            sqlCommand1.Connection.Open();
            sqlCommand1.CommandTimeout = 0;
            sqlCommand1.ExecuteNonQuery();
            sqlCommand1.Connection.Close();
            gridView1.DeleteSelectedRows();
          }
        }
        catch (Exception ex)
        {
          int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      _removedState = false;
    }

    private void btEdit_Click(object sender, EventArgs e)
    {
      if (pbImgResult.Image == null || gridView1.SelectedRowsCount <= 0)
        return;
      new BcLog().Id = (int) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["ID"];
      if (gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"].ToString() == "" || !Convert.ToBoolean(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"]))
      {
        gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"] = true;
        if (chbHide.Checked)
        {
          _removedrows.Rows.Add((object[]) gridView1.GetDataRow(gridView1.GetSelectedRows()[0]).ItemArray.Clone());
          MainTable.Rows.Remove(gridView1.GetDataRow(gridView1.GetSelectedRows()[0]));
        }
        btEdit.Text = "Снять статус";
      }
      else
      {
        gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"] = false;
        btEdit.Text = "Отработать";
      }
    }

    private void chbHide_CheckedChanged(object sender, EventArgs e)
    {
      if (chbHide.Checked)
      {
        for (int index = 0; index < MainTable.Rows.Count; ++index)
        {
          if (index > -1 && (bool) MainTable.Rows[index]["Status"])
          {
            _removedrows.Rows.Add((object[]) MainTable.Rows[index].ItemArray.Clone());
            MainTable.Rows.RemoveAt(index);
            --index;
          }
        }
      }
      else
      {
        for (int index = 0; index < _removedrows.Rows.Count; ++index)
        {
          if (index > -1 && (bool) _removedrows.Rows[index]["Status"])
          {
            MainTable.Rows.Add((object[]) _removedrows.Rows[index].ItemArray.Clone());
            _removedrows.Rows.RemoveAt(index);
            --index;
          }
        }
      }
      gridControl1.DataSource = MainTable;
    }

    private void checkEditFrom_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditFrom.Checked)
        dtpFrom.Enabled = true;
      else
        dtpFrom.Enabled = false;
    }

    private void checkEditBefore_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditBefore.Checked)
        dtpBefore.Enabled = true;
      else
        dtpBefore.Enabled = false;
    }

    private void groupBox3_Paint(object sender, PaintEventArgs e)
    {
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_removedState)
        return;
      if (gridView1.SelectedRowsCount <= 0)
        return;
      try
      {
        if (gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"].ToString() == "" || !Convert.ToBoolean(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"]))
          btEdit.Text = Messages.Process;
        else
          btEdit.Text = Messages.RemoveProcess;
        BcLog bcLog = BcLog.LoadById((int) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0]);
        if (pbImgResult.Image != null)
          pbImgResult.Image.Dispose();
        pbImgResult.Image = new Bitmap(new MemoryStream(bcLog.Image));
        BcFace bcFace = BcFace.LoadById((Guid) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[1]);
        if (bcFace.Id != Guid.Empty)
        {
          lbCategoryResult.Text = GetCategoryById(bcFace.AccessId).Name;
          lbNameResult.Text = bcFace.Surname + " " + bcFace.FirstName + " " + bcFace.LastName;
          if (bcFace.Sex == 0)
            lbSexResult.Text = Messages.Male;
          else
            lbSexResult.Text = Messages.Female;
          BcImage bcImage = BcImage.LoadMainImageByFaceId((Guid) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["FaceID"]);
          if (pbImgSource.Image != null)
            pbImgSource.Image.Dispose();
          if (bcImage.Image.Length > 0 && bcImage.Image != null)
            pbImgSource.Image = new Bitmap(new MemoryStream(bcImage.Image));
        }
        else
        {
          lbCategoryResult.Text = Messages.NonCategory;
          lbNameResult.Text = "-";
          lbSexResult.Text = "-";
          pbImgSource.Image = null;
        }
      }
      catch
      {
      }
    }

    private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
    }

    private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
    {
      if (e.RowHandle <= 0)
        return;
      try
      {
        if (_maxScore <= (double) Convert.ToSingle(((DataRowView) gridView1.GetRow(e.RowHandle))[6]))
        {
          e.Appearance.BackColor = Color.Salmon;
          e.Appearance.BackColor2 = Color.Salmon;
        }
      }
      catch
      {
      }
    }

    private void gridView1_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
    {
      GridGroupRowInfo gridGroupRowInfo = e.Info as GridGroupRowInfo;
      if (gridGroupRowInfo == null || !(gridGroupRowInfo.Column.FieldName == "GroupColumn"))
        return;
      string str1 = gridView1.GetDataRow(e.RowHandle)["GroupColumn"].ToString();
      int num1 = str1.IndexOf(" %Time");
      int num2 = str1.IndexOf("%Date");
      int num3 = str1.IndexOf(" %Name");
      string text = str1.Substring(num3 + 6, num1 - num3 - 6);
      string str2 = str1.Substring(num2 + 5, num3 - 5 - num2);
      string str3 = str1.Substring(num1 + 6, str1.Length - num1 - 6);
      try
      {
        if (text.Length == 36)
        {
          Guid guid = (Guid) new GuidConverter().ConvertFromString(text);
          gridGroupRowInfo.GroupText = " " + Convert.ToDateTime(str2).ToShortDateString() + " " + str3;
        }
        else
          gridGroupRowInfo.GroupText = text + " " + Convert.ToDateTime(str2).ToShortDateString() + " " + str3;
      }
      catch
      {
        gridGroupRowInfo.GroupText = text + " " + Convert.ToDateTime(str2).ToShortDateString() + " " + str3;
      }
    }

    private void btCollapse_Click(object sender, EventArgs e)
    {
      gridView1.CollapseAllGroups();
    }

    private void btExpand_Click(object sender, EventArgs e)
    {
      gridView1.ExpandAllGroups();
    }

    private void frmResults_Resize(object sender, EventArgs e)
    {
      ControlBox = false;
    }

    private void chbGroup_CheckedChanged(object sender, EventArgs e)
    {
      if (chbGroup.Checked)
      {
        gridView1.Columns["GroupColumn"].Group();
        gridView1.Columns["GroupColumn"].Visible = false;
        gridView1.OptionsView.ShowGroupPanel = false;
      }
      else
      {
        gridView1.Columns["GroupColumn"].UnGroup();
        gridView1.Columns["GroupColumn"].Visible = false;
      }
    }

    private void chbCollapse_CheckedChanged(object sender, EventArgs e)
    {
      if (chbCollapse.Checked)
      {
        for (int index1 = 0; index1 < MainTable.Rows.Count; ++index1)
        {
          DataRow[] dataRowArray = MainTable.Select("GroupColumn = '" + MainTable.Rows[index1]["GroupColumn"] + "'");
          float num = (float) MainTable.Rows[index1]["Score"];
          for (int index2 = 0; index2 < dataRowArray.Length; ++index2)
          {
            if (num < (double) (float) dataRowArray[index2]["Score"])
              num = (float) dataRowArray[index2]["Score"];
          }
          for (int index2 = 0; index2 < dataRowArray.Length; ++index2)
          {
            if (num != (double) (float) dataRowArray[index2]["Score"])
            {
              _dtdeletedRows.Rows.Add(dataRowArray[index2].ItemArray);
              MainTable.Rows.Remove(dataRowArray[index2]);
            }
          }
        }
      }
      else
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) _dtdeletedRows.Rows)
          MainTable.Rows.Add(dataRow.ItemArray);
        _dtdeletedRows.Rows.Clear();
      }
    }

    private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      if (e.ClickedItem == toolStripMenuItem1)
      {
        FrmReplace frmReplace = new FrmReplace();
        if (frmReplace.ShowDialog() != DialogResult.OK || !(frmReplace.MainFace.Id != Guid.Empty))
          return;
        foreach (int rowHandle in gridView1.GetSelectedRows())
        {
          DataRow dataRow = gridView1.GetDataRow(rowHandle);
          dataRow["FaceID"] = frmReplace.MainFace.Id;
          dataRow["CategoryID"] = frmReplace.MainFace.AccessId;
          dataRow["Name"] = frmReplace.MainFace.Surname + " " + frmReplace.MainFace.FirstName + " " + frmReplace.MainFace.LastName;
          BcAccessCategory categoryById = GetCategoryById(frmReplace.MainFace.AccessId);
          dataRow["Category"] = categoryById.Name;
          SqlCommand sqlCommand = new SqlCommand("UPDATE LOG SET FaceID = '" + frmReplace.MainFace.Id + "', Category = '" + categoryById.Name + "' Where ID = " + (string) dataRow["ID"], new SqlConnection(CommonSettings.ConnectionStringLog));
          sqlCommand.Connection.Open();
          sqlCommand.ExecuteNonQuery();
          sqlCommand.Connection.Close();
        }
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (e.ClickedItem == toolStripMenuItem2)
      {
        foreach (int rowHandle in gridView1.GetSelectedRows())
        {
          BcLog bcLog = BcLog.LoadById((int) gridView1.GetDataRow(rowHandle)["ID"]);
          new BcKey
          {
            FaceId = bcLog.FaceId,
            ImageId = bcLog.ImageId,
            Ksid = -2,
            ImageKey = bcLog.ImageKey
          }.Save();
        }
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (e.ClickedItem != toolStripMenuItem3)
          return;
        foreach (int rowHandle in gridView1.GetSelectedRows())
        {
          SqlCommand sqlCommand = new SqlCommand("\r\nDelete Keys.[dbo].[Keys]\r\nWHERE FaceID  = '" + BcLog.LoadById((int) gridView1.GetDataRow(rowHandle)["ID"]).FaceId + "' and KSID = -2", new SqlConnection(CommonSettings.ConnectionStringKeys));
          sqlCommand.Connection.Open();
          sqlCommand.ExecuteNonQuery();
          sqlCommand.Connection.Close();
        }
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (e.RowHandle < 0)
        return;
      e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmResults));
      repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      groupBox2 = new Panel();
      btEdit = new SimpleButton();
      lbCategoryResult = new LabelControl();
      lbSexResult = new LabelControl();
      lbNameResult = new LabelControl();
      label2 = new LabelControl();
      btShow = new SimpleButton();
      label1 = new LabelControl();
      pbImgResult = new PictureBox();
      lbCategory = new LabelControl();
      pbImgSource = new PictureBox();
      lbSex = new LabelControl();
      lbName = new LabelControl();
      menuStrip1 = new MenuStrip();
      toolStripMenuItem1 = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripMenuItem();
      toolStripMenuItem3 = new ToolStripMenuItem();
      groupBox1 = new GroupControl();
      labelControl2 = new LabelControl();
      tbPeriod = new SpinEdit();
      chbCollapse = new CheckEdit();
      chbGroup = new CheckEdit();
      btCollapse = new SimpleButton();
      btExpand = new SimpleButton();
      chbHide = new CheckEdit();
      btDelete = new SimpleButton();
      lvCategory = new CheckedListBoxControl();
      lvObjects = new CheckedListBoxControl();
      label6 = new LabelControl();
      label5 = new LabelControl();
      btPrint = new SimpleButton();
      groupBox3 = new GroupControl();
      checkEditBefore = new CheckEdit();
      checkEditFrom = new CheckEdit();
      dtpBefore = new DateEdit();
      dtpFrom = new DateEdit();
      btSearch = new SimpleButton();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colFaceID = new GridColumn();
      colCategoryID = new GridColumn();
      colObjectID = new GridColumn();
      colStatus = new GridColumn();
      colDate = new GridColumn();
      repositoryItemDateEdit1 = new RepositoryItemDateEdit();
      colTime = new GridColumn();
      colImage = new GridColumn();
      repositoryItemPictureEdit2 = new RepositoryItemPictureEdit();
      colName = new GridColumn();
      colCategory = new GridColumn();
      colPosition = new GridColumn();
      colScore = new GridColumn();
      GroupColumn = new GridColumn();
      colImageID = new GridColumn();
      colDeviceName = new GridColumn();
      colKSID = new GridColumn();
      GridColumn gridColumn1 = new GridColumn();
      GridColumn gridColumn2 = new GridColumn();
      repositoryItemPictureEdit1.BeginInit();
      groupBox2.SuspendLayout();
      ((ISupportInitialize) pbImgResult).BeginInit();
      ((ISupportInitialize) pbImgSource).BeginInit();
      menuStrip1.SuspendLayout();
      groupBox1.BeginInit();
      groupBox1.SuspendLayout();
      tbPeriod.Properties.BeginInit();
      chbCollapse.Properties.BeginInit();
      chbGroup.Properties.BeginInit();
      chbHide.Properties.BeginInit();
      lvCategory.BeginInit();
      lvObjects.BeginInit();
      groupBox3.BeginInit();
      groupBox3.SuspendLayout();
      checkEditBefore.Properties.BeginInit();
      checkEditFrom.Properties.BeginInit();
      dtpBefore.Properties.CalendarTimeProperties.BeginInit();
      dtpBefore.Properties.BeginInit();
      dtpFrom.Properties.CalendarTimeProperties.BeginInit();
      dtpFrom.Properties.BeginInit();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      repositoryItemDateEdit1.BeginInit();
      repositoryItemDateEdit1.CalendarTimeProperties.BeginInit();
      repositoryItemPictureEdit2.BeginInit();
      SuspendLayout();
      gridColumn1.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImgType.AppearanceCell.Font");
      gridColumn1.AppearanceCell.Options.UseFont = true;
      gridColumn1.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImgType.AppearanceHeader.Font");
      gridColumn1.AppearanceHeader.Options.UseFont = true;
      gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
      gridColumn1.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(gridColumn1, "colImgType");
      gridColumn1.ColumnEdit = repositoryItemPictureEdit1;
      gridColumn1.FieldName = "imgType";
      gridColumn1.Name = "colImgType";
      gridColumn1.OptionsColumn.AllowEdit = false;
      gridColumn1.OptionsColumn.ReadOnly = true;
      repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      gridColumn2.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      gridColumn2.AppearanceCell.Options.UseFont = true;
      gridColumn2.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      gridColumn2.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridColumn2, "colID");
      gridColumn2.FieldName = "ID";
      gridColumn2.Name = "colID";
      componentResourceManager.ApplyResources(groupBox2, "groupBox2");
      groupBox2.BorderStyle = BorderStyle.Fixed3D;
      groupBox2.Controls.Add(btEdit);
      groupBox2.Controls.Add(lbCategoryResult);
      groupBox2.Controls.Add(lbSexResult);
      groupBox2.Controls.Add(lbNameResult);
      groupBox2.Controls.Add(label2);
      groupBox2.Controls.Add(btShow);
      groupBox2.Controls.Add(label1);
      groupBox2.Controls.Add(pbImgResult);
      groupBox2.Controls.Add(lbCategory);
      groupBox2.Controls.Add(pbImgSource);
      groupBox2.Controls.Add(lbSex);
      groupBox2.Controls.Add(lbName);
      groupBox2.Controls.Add(menuStrip1);
      groupBox2.Name = "groupBox2";
      componentResourceManager.ApplyResources(btEdit, "btEdit");
      btEdit.Appearance.Font = (Font) componentResourceManager.GetObject("btEdit.Appearance.Font");
      btEdit.Appearance.Options.UseFont = true;
      btEdit.Name = "btEdit";
      btEdit.Click += btEdit_Click;
      lbCategoryResult.Appearance.Font = (Font) componentResourceManager.GetObject("lbCategoryResult.Appearance.Font");
      componentResourceManager.ApplyResources(lbCategoryResult, "lbCategoryResult");
      lbCategoryResult.Name = "lbCategoryResult";
      lbSexResult.Appearance.Font = (Font) componentResourceManager.GetObject("lbSexResult.Appearance.Font");
      componentResourceManager.ApplyResources(lbSexResult, "lbSexResult");
      lbSexResult.Name = "lbSexResult";
      lbNameResult.Appearance.Font = (Font) componentResourceManager.GetObject("lbNameResult.Appearance.Font");
      componentResourceManager.ApplyResources(lbNameResult, "lbNameResult");
      lbNameResult.Name = "lbNameResult";
      componentResourceManager.ApplyResources(label2, "label2");
      label2.Appearance.Font = (Font) componentResourceManager.GetObject("label2.Appearance.Font");
      label2.Name = "label2";
      componentResourceManager.ApplyResources(btShow, "btShow");
      btShow.Appearance.Font = (Font) componentResourceManager.GetObject("btShow.Appearance.Font");
      btShow.Appearance.Options.UseFont = true;
      btShow.Name = "btShow";
      btShow.Click += btShow_Click;
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(pbImgResult, "pbImgResult");
      pbImgResult.Name = "pbImgResult";
      pbImgResult.TabStop = false;
      componentResourceManager.ApplyResources(lbCategory, "lbCategory");
      lbCategory.Appearance.Font = (Font) componentResourceManager.GetObject("lbCategory.Appearance.Font");
      lbCategory.Name = "lbCategory";
      componentResourceManager.ApplyResources(pbImgSource, "pbImgSource");
      pbImgSource.Name = "pbImgSource";
      pbImgSource.TabStop = false;
      componentResourceManager.ApplyResources(lbSex, "lbSex");
      lbSex.Appearance.Font = (Font) componentResourceManager.GetObject("lbSex.Appearance.Font");
      lbSex.Name = "lbSex";
      componentResourceManager.ApplyResources(lbName, "lbName");
      lbName.Appearance.Font = (Font) componentResourceManager.GetObject("lbName.Appearance.Font");
      lbName.Name = "lbName";
      menuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        toolStripMenuItem1,
        toolStripMenuItem2,
        toolStripMenuItem3
      });
      componentResourceManager.ApplyResources(menuStrip1, "menuStrip1");
      menuStrip1.Name = "menuStrip1";
      menuStrip1.ItemClicked += menuStrip1_ItemClicked;
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      componentResourceManager.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      componentResourceManager.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
      toolStripMenuItem3.Name = "toolStripMenuItem3";
      componentResourceManager.ApplyResources(toolStripMenuItem3, "toolStripMenuItem3");
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox1.AppearanceCaption.Font");
      groupBox1.AppearanceCaption.Options.UseFont = true;
      groupBox1.Controls.Add(labelControl2);
      groupBox1.Controls.Add(tbPeriod);
      groupBox1.Controls.Add(chbCollapse);
      groupBox1.Controls.Add(chbGroup);
      groupBox1.Controls.Add(btCollapse);
      groupBox1.Controls.Add(btExpand);
      groupBox1.Controls.Add(chbHide);
      groupBox1.Controls.Add(btDelete);
      groupBox1.Controls.Add(lvCategory);
      groupBox1.Controls.Add(lvObjects);
      groupBox1.Controls.Add(label6);
      groupBox1.Controls.Add(label5);
      groupBox1.Controls.Add(btPrint);
      groupBox1.Name = "groupBox1";
      groupBox1.ShowCaption = false;
      componentResourceManager.ApplyResources(labelControl2, "labelControl2");
      labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources(tbPeriod, "tbPeriod");
      tbPeriod.Name = "tbPeriod";
      tbPeriod.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      tbPeriod.Properties.Mask.EditMask = componentResourceManager.GetString("tbPeriod.Properties.Mask.EditMask");
      tbPeriod.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbPeriod.Properties.Mask.UseMaskAsDisplayFormat");
      componentResourceManager.ApplyResources(chbCollapse, "chbCollapse");
      chbCollapse.Name = "chbCollapse";
      chbCollapse.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbCollapse.Properties.Appearance.Font");
      chbCollapse.Properties.Appearance.Options.UseFont = true;
      chbCollapse.Properties.Caption = componentResourceManager.GetString("chbCollapse.Properties.Caption");
      chbCollapse.CheckedChanged += chbCollapse_CheckedChanged;
      componentResourceManager.ApplyResources(chbGroup, "chbGroup");
      chbGroup.Name = "chbGroup";
      chbGroup.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbGroup.Properties.Appearance.Font");
      chbGroup.Properties.Appearance.Options.UseFont = true;
      chbGroup.Properties.Caption = componentResourceManager.GetString("chbGroup.Properties.Caption");
      chbGroup.CheckedChanged += chbGroup_CheckedChanged;
      componentResourceManager.ApplyResources(btCollapse, "btCollapse");
      btCollapse.Appearance.Font = (Font) componentResourceManager.GetObject("btCollapse.Appearance.Font");
      btCollapse.Appearance.Options.UseFont = true;
      btCollapse.Name = "btCollapse";
      btCollapse.Click += btCollapse_Click;
      componentResourceManager.ApplyResources(btExpand, "btExpand");
      btExpand.Appearance.Font = (Font) componentResourceManager.GetObject("btExpand.Appearance.Font");
      btExpand.Appearance.Options.UseFont = true;
      btExpand.Name = "btExpand";
      btExpand.Click += btExpand_Click;
      componentResourceManager.ApplyResources(chbHide, "chbHide");
      chbHide.Name = "chbHide";
      chbHide.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbHide.Properties.Appearance.Font");
      chbHide.Properties.Appearance.Options.UseFont = true;
      chbHide.Properties.Caption = componentResourceManager.GetString("chbHide.Properties.Caption");
      chbHide.CheckedChanged += chbHide_CheckedChanged;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.Options.UseFont = true;
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      componentResourceManager.ApplyResources(lvCategory, "lvCategory");
      lvCategory.Appearance.Font = (Font) componentResourceManager.GetObject("lvCategory.Appearance.Font");
      lvCategory.Appearance.Options.UseFont = true;
      lvCategory.Name = "lvCategory";
      componentResourceManager.ApplyResources(lvObjects, "lvObjects");
      lvObjects.Appearance.Font = (Font) componentResourceManager.GetObject("lvObjects.Appearance.Font");
      lvObjects.Appearance.Options.UseFont = true;
      lvObjects.Name = "lvObjects";
      componentResourceManager.ApplyResources(label6, "label6");
      label6.Appearance.Font = (Font) componentResourceManager.GetObject("label6.Appearance.Font");
      label6.Name = "label6";
      componentResourceManager.ApplyResources(label5, "label5");
      label5.Appearance.Font = (Font) componentResourceManager.GetObject("label5.Appearance.Font");
      label5.Name = "label5";
      componentResourceManager.ApplyResources(btPrint, "btPrint");
      btPrint.Appearance.Font = (Font) componentResourceManager.GetObject("btPrint.Appearance.Font");
      btPrint.Appearance.Options.UseFont = true;
      btPrint.Name = "btPrint";
      btPrint.Click += btPrint_Click;
      componentResourceManager.ApplyResources(groupBox3, "groupBox3");
      groupBox3.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox3.AppearanceCaption.Font");
      groupBox3.AppearanceCaption.Options.UseFont = true;
      groupBox3.Controls.Add(checkEditBefore);
      groupBox3.Controls.Add(checkEditFrom);
      groupBox3.Controls.Add(dtpBefore);
      groupBox3.Controls.Add(dtpFrom);
      groupBox3.Controls.Add(btSearch);
      groupBox3.Name = "groupBox3";
      groupBox3.ShowCaption = false;
      groupBox3.Paint += groupBox3_Paint;
      componentResourceManager.ApplyResources(checkEditBefore, "checkEditBefore");
      checkEditBefore.Name = "checkEditBefore";
      checkEditBefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.Font");
      checkEditBefore.Properties.Appearance.Options.UseFont = true;
      checkEditBefore.Properties.Caption = componentResourceManager.GetString("checkEditBefore.Properties.Caption");
      checkEditBefore.CheckedChanged += checkEditBefore_CheckedChanged;
      componentResourceManager.ApplyResources(checkEditFrom, "checkEditFrom");
      checkEditFrom.Name = "checkEditFrom";
      checkEditFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.Font");
      checkEditFrom.Properties.Appearance.Options.UseFont = true;
      checkEditFrom.Properties.Caption = componentResourceManager.GetString("checkEditFrom.Properties.Caption");
      checkEditFrom.CheckedChanged += checkEditFrom_CheckedChanged;
      componentResourceManager.ApplyResources(dtpBefore, "dtpBefore");
      dtpBefore.Name = "dtpBefore";
      dtpBefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.Font");
      dtpBefore.Properties.Appearance.Options.UseFont = true;
      dtpBefore.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpBefore.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpBefore.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpBefore.Properties.CalendarTimeProperties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpBefore.Properties.CalendarTimeProperties.EditFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpBefore.Properties.CalendarTimeProperties.Mask.EditMask");
      dtpBefore.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.MaskType");
      dtpBefore.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      dtpBefore.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtpBefore.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.Mask.EditMask = componentResourceManager.GetString("dtpBefore.Properties.Mask.EditMask");
      dtpBefore.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBefore.Properties.Mask.MaskType");
      componentResourceManager.ApplyResources(dtpFrom, "dtpFrom");
      dtpFrom.Name = "dtpFrom";
      dtpFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.Font");
      dtpFrom.Properties.Appearance.Options.UseFont = true;
      dtpFrom.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpFrom.Properties.CalendarTimeProperties.EditFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpFrom.Properties.CalendarTimeProperties.Mask.EditMask");
      dtpFrom.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.MaskType");
      dtpFrom.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      dtpFrom.Properties.DisplayFormat.FormatType = FormatType.Custom;
      dtpFrom.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtpFrom.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.Mask.EditMask = componentResourceManager.GetString("dtpFrom.Properties.Mask.EditMask");
      dtpFrom.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpFrom.Properties.Mask.MaskType");
      btSearch.Appearance.Font = (Font) componentResourceManager.GetObject("btSearch.Appearance.Font");
      btSearch.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btSearch, "btSearch");
      btSearch.Name = "btSearch";
      btSearch.Click += btSearch_Click;
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.RepositoryItems.AddRange(new RepositoryItem[3]
      {
        repositoryItemPictureEdit1,
        repositoryItemPictureEdit2,
        repositoryItemDateEdit1
      });
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridView1.Columns.AddRange(new GridColumn[17]
      {
        gridColumn2,
        colFaceID,
        colCategoryID,
        colObjectID,
        colStatus,
        colDate,
        colTime,
        colImage,
        colName,
        colCategory,
        colPosition,
        colScore,
        GroupColumn,
        gridColumn1,
        colImageID,
        colDeviceName,
        colKSID
      });
      gridView1.GridControl = gridControl1;
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.IndicatorWidth = 60;
      gridView1.Name = "gridView1";
      gridView1.OptionsCustomization.AllowFilter = false;
      gridView1.OptionsCustomization.AllowGroup = false;
      gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
      gridView1.OptionsFilter.AllowFilterEditor = false;
      gridView1.OptionsFilter.AllowMRUFilterList = false;
      gridView1.OptionsFind.AllowFindPanel = false;
      gridView1.OptionsMenu.EnableColumnMenu = false;
      gridView1.OptionsMenu.EnableFooterMenu = false;
      gridView1.OptionsMenu.EnableGroupPanelMenu = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.ColumnAutoWidth = false;
      gridView1.OptionsView.GroupFooterShowMode = GroupFooterShowMode.Hidden;
      gridView1.OptionsView.RowAutoHeight = true;
      gridView1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.SortInfo.AddRange(new GridColumnSortInfo[1]
      {
        new GridColumnSortInfo(GroupColumn, ColumnSortOrder.Descending)
      });
      gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
      gridView1.CustomDrawGroupRow += gridView1_CustomDrawGroupRow;
      gridView1.RowStyle += gridView1_RowStyle;
      gridView1.SelectionChanged += gridView1_SelectionChanged;
      gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;
      colFaceID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colFaceID.AppearanceCell.Font");
      colFaceID.AppearanceCell.Options.UseFont = true;
      colFaceID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colFaceID.AppearanceHeader.Font");
      colFaceID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colFaceID, "colFaceID");
      colFaceID.FieldName = "FaceID";
      colFaceID.Name = "colFaceID";
      colCategoryID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colCategoryID.AppearanceCell.Font");
      colCategoryID.AppearanceCell.Options.UseFont = true;
      colCategoryID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.Font");
      colCategoryID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colCategoryID, "colCategoryID");
      colCategoryID.FieldName = "CategoryID";
      colCategoryID.Name = "colCategoryID";
      colObjectID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colObjectID.AppearanceCell.Font");
      colObjectID.AppearanceCell.Options.UseFont = true;
      colObjectID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colObjectID.AppearanceHeader.Font");
      colObjectID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colObjectID, "colObjectID");
      colObjectID.FieldName = "ObjectID";
      colObjectID.Name = "colObjectID";
      colStatus.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colStatus.AppearanceCell.Font");
      colStatus.AppearanceCell.Options.UseFont = true;
      colStatus.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colStatus.AppearanceHeader.Font");
      colStatus.AppearanceHeader.Options.UseFont = true;
      colStatus.AppearanceHeader.Options.UseTextOptions = true;
      colStatus.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colStatus, "colStatus");
      colStatus.FieldName = "Status";
      colStatus.Name = "colStatus";
      colStatus.OptionsColumn.AllowEdit = false;
      colDate.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colDate.AppearanceCell.Font");
      colDate.AppearanceCell.Options.UseFont = true;
      colDate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colDate.AppearanceHeader.Font");
      colDate.AppearanceHeader.Options.UseFont = true;
      colDate.AppearanceHeader.Options.UseTextOptions = true;
      colDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colDate, "colDate");
      colDate.ColumnEdit = repositoryItemDateEdit1;
      colDate.DisplayFormat.FormatString = "dd.MM.yyyy";
      colDate.DisplayFormat.FormatType = FormatType.DateTime;
      colDate.FieldName = "Date";
      colDate.Name = "colDate";
      colDate.OptionsColumn.AllowEdit = false;
      componentResourceManager.ApplyResources(repositoryItemDateEdit1, "repositoryItemDateEdit1");
      repositoryItemDateEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemDateEdit1.Buttons"))
      });
      repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
      colTime.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colTime.AppearanceCell.Font");
      colTime.AppearanceCell.Options.UseFont = true;
      colTime.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colTime.AppearanceHeader.Font");
      colTime.AppearanceHeader.Options.UseFont = true;
      colTime.AppearanceHeader.Options.UseTextOptions = true;
      colTime.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colTime, "colTime");
      colTime.DisplayFormat.FormatString = "HH:mm:ss";
      colTime.DisplayFormat.FormatType = FormatType.Custom;
      colTime.FieldName = "Time";
      colTime.Name = "colTime";
      colImage.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceCell.Font");
      colImage.AppearanceCell.Options.UseFont = true;
      colImage.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
      colImage.AppearanceHeader.Options.UseFont = true;
      colImage.AppearanceHeader.Options.UseTextOptions = true;
      colImage.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colImage, "colImage");
      colImage.ColumnEdit = repositoryItemPictureEdit2;
      colImage.DisplayFormat.FormatType = FormatType.Custom;
      colImage.FieldName = "ImageIcon";
      colImage.Name = "colImage";
      colImage.OptionsColumn.AllowEdit = false;
      repositoryItemPictureEdit2.Name = "repositoryItemPictureEdit2";
      colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      colName.AppearanceCell.Options.UseFont = true;
      colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      colName.AppearanceHeader.Options.UseFont = true;
      colName.AppearanceHeader.Options.UseTextOptions = true;
      colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colName, "colName");
      colName.FieldName = "Name";
      colName.Name = "colName";
      colName.OptionsColumn.AllowEdit = false;
      colName.OptionsColumn.ReadOnly = true;
      colCategory.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colCategory.AppearanceCell.Font");
      colCategory.AppearanceCell.Options.UseFont = true;
      colCategory.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colCategory.AppearanceHeader.Font");
      colCategory.AppearanceHeader.Options.UseFont = true;
      colCategory.AppearanceHeader.Options.UseTextOptions = true;
      colCategory.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colCategory, "colCategory");
      colCategory.FieldName = "Category";
      colCategory.Name = "colCategory";
      colCategory.OptionsColumn.AllowEdit = false;
      colPosition.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceCell.Font");
      colPosition.AppearanceCell.Options.UseFont = true;
      colPosition.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      colPosition.AppearanceHeader.Options.UseFont = true;
      colPosition.AppearanceHeader.Options.UseTextOptions = true;
      colPosition.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colPosition, "colPosition");
      colPosition.FieldName = "Number";
      colPosition.Name = "colPosition";
      colPosition.OptionsColumn.AllowEdit = false;
      colScore.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colScore.AppearanceCell.Font");
      colScore.AppearanceCell.Options.UseFont = true;
      colScore.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colScore.AppearanceHeader.Font");
      colScore.AppearanceHeader.Options.UseFont = true;
      colScore.AppearanceHeader.Options.UseTextOptions = true;
      colScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colScore, "colScore");
      colScore.FieldName = "Score";
      colScore.Name = "colScore";
      colScore.OptionsColumn.AllowEdit = false;
      GroupColumn.AppearanceCell.Font = (Font) componentResourceManager.GetObject("GroupColumn.AppearanceCell.Font");
      GroupColumn.AppearanceCell.Options.UseFont = true;
      GroupColumn.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("GroupColumn.AppearanceHeader.Font");
      GroupColumn.AppearanceHeader.Options.UseFont = true;
      GroupColumn.AppearanceHeader.Options.UseTextOptions = true;
      GroupColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(GroupColumn, "GroupColumn");
      GroupColumn.FieldName = "GroupColumn";
      GroupColumn.Name = "GroupColumn";
      GroupColumn.OptionsColumn.AllowEdit = false;
      GroupColumn.SortMode = ColumnSortMode.Custom;
      componentResourceManager.ApplyResources(colImageID, "colImageID");
      colImageID.FieldName = "ImageID";
      colImageID.Name = "colImageID";
      colDeviceName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colDeviceName.AppearanceCell.Font");
      colDeviceName.AppearanceCell.Options.UseFont = true;
      colDeviceName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colDeviceName.AppearanceHeader.Font");
      colDeviceName.AppearanceHeader.Options.UseFont = true;
      colDeviceName.AppearanceHeader.Options.UseTextOptions = true;
      colDeviceName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colDeviceName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colDeviceName, "colDeviceName");
      colDeviceName.FieldName = "DeviceName";
      colDeviceName.Name = "colDeviceName";
      colDeviceName.OptionsColumn.AllowEdit = false;
      colDeviceName.OptionsColumn.ReadOnly = true;
      colKSID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colKSID.AppearanceCell.Font");
      colKSID.AppearanceCell.Options.UseFont = true;
      componentResourceManager.ApplyResources(colKSID, "colKSID");
      colKSID.FieldName = "KSID";
      colKSID.Name = "colKSID";
      colKSID.OptionsColumn.AllowEdit = false;
      colKSID.OptionsColumn.ReadOnly = true;
      Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(groupBox1);
      Controls.Add(groupBox2);
      Controls.Add(groupBox3);
      Controls.Add(gridControl1);
      FormBorderStyle = FormBorderStyle.None;
      MainMenuStrip = menuStrip1;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmResults";
      WindowState = FormWindowState.Maximized;
      FormClosing += frmResults_FormClosing;
      Load += frmResults_Load;
      Resize += frmResults_Resize;
      repositoryItemPictureEdit1.EndInit();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ((ISupportInitialize) pbImgResult).EndInit();
      ((ISupportInitialize) pbImgSource).EndInit();
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      groupBox1.EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      tbPeriod.Properties.EndInit();
      chbCollapse.Properties.EndInit();
      chbGroup.Properties.EndInit();
      chbHide.Properties.EndInit();
      lvCategory.EndInit();
      lvObjects.EndInit();
      groupBox3.EndInit();
      groupBox3.ResumeLayout(false);
      checkEditBefore.Properties.EndInit();
      checkEditFrom.Properties.EndInit();
      dtpBefore.Properties.CalendarTimeProperties.EndInit();
      dtpBefore.Properties.EndInit();
      dtpFrom.Properties.CalendarTimeProperties.EndInit();
      dtpFrom.Properties.EndInit();
      gridControl1.EndInit();
      gridView1.EndInit();
      repositoryItemDateEdit1.CalendarTimeProperties.EndInit();
      repositoryItemDateEdit1.EndInit();
      repositoryItemPictureEdit2.EndInit();
      ResumeLayout(false);
    }
  }
}
