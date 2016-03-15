// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmStatistic
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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BasicComponents;
using CS.DAL;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmStatistic : XtraForm
  {
    private float _maxScore = 0.55f;
    public DataTable DtResults = new DataTable();
    public List<BcDevices> Devices = new List<BcDevices>();
    private List<BcObjects> _objects = new List<BcObjects>();
    private DataTable _dtmain = new DataTable();
    private IContainer components = null;
    private LabelControl label6;
    private GroupBox groupBox3;
    private DateEdit dtpBefore;
    private DateEdit dtpFrom;
    private SimpleButton btSearch;
    private SimpleButton btPrint;
    private CheckEdit checkEditbefore;
    private CheckEdit checkEditfrom;
    private RadioGroup radioGroup1;
    private GroupControl groupBox1;
    private CheckedListBoxControl lvObjects;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colDate;
    private GridColumn colObject;
    private GridColumn colDevice;
    private GridColumn colResult;
    private GridColumn colTraffic;
    private LabelControl labelControl1;
    private Label label1;
    private GridColumn colTime;

    public FrmStatistic()
    {
      InitializeComponent();
      ReloadObjects();
    }

    public void ReloadObjects()
    {
      Devices = BcDevicesStorageExtensions.LoadAll();
      _objects = BcObjects.LoadAll();
      lvObjects.Items.Clear();
      foreach (BcDevices bcDevices in Devices)
      {
        lvObjects.Items.Add(bcDevices.Name);
        lvObjects.Items[lvObjects.Items.Count - 1].Value = bcDevices.Id;
        lvObjects.Items[lvObjects.Items.Count - 1].Description = bcDevices.Name;
      }
    }

    public float[] GetScore()
    {
      float[] numArray = new float[4];
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

    private void btSearch_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < lvObjects.Items.Count; ++index)
      {
        if (lvObjects.Items[index].CheckState == CheckState.Checked)
          str2 = string.Concat((object) str2, (object) "DeviceID = '", lvObjects.Items[index].Value, (object) "' or ");
      }
      if (str2 != "")
        str1 = str1 + "( " + str2.Substring(0, str2.Length - 3) + " ) and ";
      string str3 = "";
      if (checkEditbefore.Checked && checkEditfrom.Checked)
        str3 = " (Date>=Convert(datetime,'" + dtpFrom.DateTime.ToShortDateString() + "',104) and Date<=Convert(datetime,'" + dtpBefore.DateTime.ToShortDateString() + "',104)) and ";
      else if (checkEditbefore.Checked)
        str3 = " (Date<=Convert(datetime,'" + dtpBefore.DateTime.ToShortDateString() + "',104)) and ";
      else if (checkEditfrom.Checked)
        str3 = " (Date>=Convert(datetime,'" + dtpFrom.DateTime.ToShortDateString() + "',104)) and ";
      if (str3 != "")
        str1 += str3;
      if (str1 != "")
        str1 = str1.Substring(0, str1.Length - 4);
      string str4 = "Select ID,\r\nDeviceID,\r\nDate,\r\nFaceCount,\r\nResultCount \r\nfrom  IdentifierResults ";
      if (str1 != "")
        str4 = str4 + " Where " + str1;
      SqlCommand sqlCommand = new SqlCommand(str4 + " order by Date,DeviceID desc", new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      DtResults = new DataTable();
      for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
        DtResults.Columns.Add(sqlDataReader.GetName(ordinal), sqlDataReader.GetFieldType(ordinal));
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        DtResults.Rows.Add(values);
      }
      Cursor = Cursors.Default;
      sqlCommand.Connection.Close();
      RefreshGrid();
    }

    private void RefreshGrid()
    {
      Cursor = Cursors.WaitCursor;
      if (DtResults != null && DtResults.Rows.Count > 0)
      {
        _dtmain = new DataTable();
        _dtmain.Columns.AddRange(new DataColumn[6]
        {
          new DataColumn("Date"),
          new DataColumn("Time", typeof (string)),
          new DataColumn("Object"),
          new DataColumn("Position"),
          new DataColumn("Traffic"),
          new DataColumn("Result")
        });
        DateTime dateTime1 = checkEditfrom.Checked ? dtpFrom.DateTime : DateTime.Now.Date;
        DateTime dateTime2 = checkEditbefore.Checked ? dtpBefore.DateTime : DateTime.Now;
        if (radioGroup1.SelectedIndex == 0)
        {
          foreach (BcDevices bcDevices in Devices)
          {
            bool flag = false;
            if (lvObjects.CheckedItems.Count == 0)
            {
              flag = true;
            }
            else
            {
              foreach (ListBoxItem listBoxItem in lvObjects.CheckedItems)
              {
                if ((Guid) listBoxItem.Value == bcDevices.Id)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (flag)
            {
              DateTime dateTime3 = dateTime1;
              dateTime3 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime3.Hour, 0, 0);
              string str = bcDevices.Name + "-" + GetObjectById(bcDevices.Id);
              int num1 = 0;
              do
              {
                DateTime dateTime4 = dateTime3.AddHours(1.0);
                int num2 = 0;
                int num3 = 0;
                for (int index = num1; index < DtResults.Rows.Count && !((DateTime) DtResults.Rows[index]["Date"] > dateTime4); ++index)
                {
                  if (DtResults.Rows[index]["DeviceID"].ToString() == bcDevices.Id.ToString() && (DateTime) DtResults.Rows[index]["Date"] >= dateTime3)
                  {
                    num2 += (int) DtResults.Rows[index][3];
                    num3 += (int) DtResults.Rows[index][4];
                  }
                  ++num1;
                }
                _dtmain.Rows.Add((object) dateTime3.ToString(), (object) dateTime3.TimeOfDay, (object) str, (object) num2, (object) num3);
                dateTime3 = dateTime3.AddHours(1.0);
              }
              while (dateTime3 < dateTime2);
            }
          }
        }
        else if (radioGroup1.SelectedIndex == 1)
        {
          foreach (BcDevices bcDevices in Devices)
          {
            bool flag = false;
            if (lvObjects.CheckedItems.Count == 0)
            {
              flag = true;
            }
            else
            {
              foreach (ListBoxItem listBoxItem in lvObjects.CheckedItems)
              {
                if ((Guid) listBoxItem.Value == bcDevices.Id)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (flag)
            {
              DateTime dateTime3 = dateTime1;
              dateTime3 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, 0, 0, 0);
              string name = bcDevices.Name;
              int num1 = 0;
              do
              {
                DateTime dateTime4 = dateTime3.AddDays(1.0);
                int num2 = 0;
                int num3 = 0;
                for (int index = num1; index < DtResults.Rows.Count && !((DateTime) DtResults.Rows[index]["Date"] > dateTime4); ++index)
                {
                  if (DtResults.Rows[index]["DeviceID"].ToString() == bcDevices.Id.ToString() && (DateTime) DtResults.Rows[index]["Date"] >= dateTime3)
                  {
                    num2 += (int) DtResults.Rows[index][3];
                    num3 += (int) DtResults.Rows[index][4];
                  }
                  ++num1;
                }
                _dtmain.Rows.Add((object) dateTime3.ToString(), (object) dateTime3.TimeOfDay, (object) name, (object) num2, (object) num3);
                dateTime3 = dateTime3.AddDays(1.0);
              }
              while (dateTime3 < dateTime2);
            }
          }
        }
        else if (radioGroup1.SelectedIndex == 2)
        {
          foreach (BcDevices bcDevices in Devices)
          {
            bool flag = false;
            if (lvObjects.CheckedItems.Count == 0)
            {
              flag = true;
            }
            else
            {
              foreach (ListBoxItem listBoxItem in lvObjects.CheckedItems)
              {
                if ((Guid) listBoxItem.Value == bcDevices.Id)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (flag)
            {
              DateTime dateTime3 = dateTime1;
              dateTime3 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, 0, 0, 0);
              string str = bcDevices.Name + "-" + GetObjectById(bcDevices.Id);
              int num1 = 0;
              do
              {
                DateTime dateTime4 = dateTime3.AddDays(7.0);
                int num2 = 0;
                int num3 = 0;
                for (int index = num1; index < DtResults.Rows.Count && !((DateTime) DtResults.Rows[index]["Date"] > dateTime4); ++index)
                {
                  if (DtResults.Rows[index]["DeviceID"].ToString() == bcDevices.Id.ToString() && (DateTime) DtResults.Rows[index]["Date"] >= dateTime3)
                  {
                    num2 += (int) DtResults.Rows[index][3];
                    num3 += (int) DtResults.Rows[index][4];
                  }
                  ++num1;
                }
                _dtmain.Rows.Add((object) dateTime3.ToString(), (object) dateTime3.TimeOfDay, (object) str, (object) num2, (object) num3);
                dateTime3 = dateTime3.AddDays(7.0);
              }
              while (dateTime3 < dateTime2);
            }
          }
        }
        else if (radioGroup1.SelectedIndex == 3)
        {
          foreach (BcDevices bcDevices in Devices)
          {
            bool flag = false;
            if (lvObjects.CheckedItems.Count == 0)
            {
              flag = true;
            }
            else
            {
              foreach (ListBoxItem listBoxItem in lvObjects.CheckedItems)
              {
                if ((Guid) listBoxItem.Value == bcDevices.Id)
                {
                  flag = true;
                  break;
                }
              }
            }
            if (flag)
            {
              DateTime dateTime3 = dateTime1;
              dateTime3 = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, 0, 0, 0);
              string str = bcDevices.Name + "-" + GetObjectById(bcDevices.Id);
              int num1 = 0;
              do
              {
                DateTime dateTime4 = dateTime3.AddMonths(1);
                int num2 = 0;
                int num3 = 0;
                for (int index = num1; index < DtResults.Rows.Count && !((DateTime) DtResults.Rows[index]["Date"] > dateTime4); ++index)
                {
                  if (DtResults.Rows[index]["DeviceID"].ToString() == bcDevices.Id.ToString() && (DateTime) DtResults.Rows[index]["Date"] >= dateTime3)
                  {
                    num2 += (int) DtResults.Rows[index][3];
                    num3 += (int) DtResults.Rows[index][4];
                  }
                  ++num1;
                }
                _dtmain.Rows.Add((object) dateTime3.ToString(), (object) dateTime3.TimeOfDay, (object) str, (object) num2, (object) num3);
                dateTime3 = dateTime3.AddMonths(1);
              }
              while (dateTime3 < dateTime2);
            }
          }
        }
        gridControl1.DataSource = _dtmain;
      }
      Cursor = Cursors.Default;
    }

    private string GetObjectById(Guid id)
    {
      string str = "";
      for (int index = 0; index < lvObjects.Items.Count; ++index)
      {
        if (id == (Guid) lvObjects.Items[index].Value)
        {
          str = lvObjects.Items[index].Description;
          break;
        }
      }
      return str;
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
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

    private void rdbDay_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void rdbWeek_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void rdbMonth_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void frmStatistic_Load(object sender, EventArgs e)
    {
      DateEdit dateEdit1 = dtpBefore;
      DateTime now1 = DateTime.Now;
      int year1 = now1.Year;
      now1 = DateTime.Now;
      int month1 = now1.Month;
      now1 = DateTime.Now;
      int day1 = now1.Day;
      DateTime dateTime1 = new DateTime(year1, month1, day1);
      dateEdit1.DateTime = dateTime1;
      DateEdit dateEdit2 = dtpFrom;
      DateTime now2 = DateTime.Now;
      int year2 = now2.Year;
      now2 = DateTime.Now;
      int month2 = now2.Month;
      now2 = DateTime.Now;
      int day2 = now2.Day;
      DateTime dateTime2 = new DateTime(year2, month2, day2);
      dateEdit2.DateTime = dateTime2;
      ControlBox = false;
    }

    private void rdbHours_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void checkEditfrom_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditfrom.Checked)
        dtpFrom.Enabled = true;
      else
        dtpFrom.Enabled = false;
    }

    private void checkEditbefore_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditbefore.Checked)
        dtpBefore.Enabled = true;
      else
        dtpBefore.Enabled = false;
    }

    private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
    {
      RefreshGrid();
    }

    private void frmStatistic_Resize(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmStatistic));
      label6 = new LabelControl();
      groupBox3 = new GroupBox();
      label1 = new Label();
      checkEditbefore = new CheckEdit();
      checkEditfrom = new CheckEdit();
      dtpBefore = new DateEdit();
      dtpFrom = new DateEdit();
      btSearch = new SimpleButton();
      btPrint = new SimpleButton();
      radioGroup1 = new RadioGroup();
      groupBox1 = new GroupControl();
      lvObjects = new CheckedListBoxControl();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colDate = new GridColumn();
      colObject = new GridColumn();
      colDevice = new GridColumn();
      colTraffic = new GridColumn();
      colResult = new GridColumn();
      colTime = new GridColumn();
      labelControl1 = new LabelControl();
      groupBox3.SuspendLayout();
      checkEditbefore.Properties.BeginInit();
      checkEditfrom.Properties.BeginInit();
      dtpBefore.Properties.CalendarTimeProperties.BeginInit();
      dtpBefore.Properties.BeginInit();
      dtpFrom.Properties.CalendarTimeProperties.BeginInit();
      dtpFrom.Properties.BeginInit();
      radioGroup1.Properties.BeginInit();
      groupBox1.BeginInit();
      groupBox1.SuspendLayout();
      lvObjects.BeginInit();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(label6, "label6");
      label6.Appearance.Font = (Font) componentResourceManager.GetObject("label6.Appearance.Font");
      label6.Name = "label6";
      componentResourceManager.ApplyResources(groupBox3, "groupBox3");
      groupBox3.Controls.Add(label1);
      groupBox3.Controls.Add(checkEditbefore);
      groupBox3.Controls.Add(checkEditfrom);
      groupBox3.Controls.Add(dtpBefore);
      groupBox3.Controls.Add(dtpFrom);
      groupBox3.Name = "groupBox3";
      groupBox3.TabStop = false;
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(checkEditbefore, "checkEditbefore");
      checkEditbefore.Name = "checkEditbefore";
      checkEditbefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditbefore.Properties.Appearance.Font");
      checkEditbefore.Properties.Appearance.Options.UseFont = true;
      checkEditbefore.Properties.Caption = componentResourceManager.GetString("checkEditbefore.Properties.Caption");
      checkEditbefore.CheckedChanged += checkEditbefore_CheckedChanged;
      componentResourceManager.ApplyResources(checkEditfrom, "checkEditfrom");
      checkEditfrom.Name = "checkEditfrom";
      checkEditfrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditfrom.Properties.Appearance.Font");
      checkEditfrom.Properties.Appearance.Options.UseFont = true;
      checkEditfrom.Properties.Caption = componentResourceManager.GetString("checkEditfrom.Properties.Caption");
      checkEditfrom.CheckedChanged += checkEditfrom_CheckedChanged;
      componentResourceManager.ApplyResources(dtpBefore, "dtpBefore");
      dtpBefore.Name = "dtpBefore";
      dtpBefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.Font");
      dtpBefore.Properties.Appearance.Options.UseFont = true;
      dtpBefore.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      componentResourceManager.ApplyResources(dtpFrom, "dtpFrom");
      dtpFrom.Name = "dtpFrom";
      dtpFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.Font");
      dtpFrom.Properties.Appearance.Options.UseFont = true;
      dtpFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      componentResourceManager.ApplyResources(btSearch, "btSearch");
      btSearch.Appearance.Font = (Font) componentResourceManager.GetObject("btSearch.Appearance.Font");
      btSearch.Appearance.Options.UseFont = true;
      btSearch.Name = "btSearch";
      btSearch.Click += btSearch_Click;
      componentResourceManager.ApplyResources(btPrint, "btPrint");
      btPrint.Appearance.Font = (Font) componentResourceManager.GetObject("btPrint.Appearance.Font");
      btPrint.Appearance.Options.UseFont = true;
      btPrint.Name = "btPrint";
      btPrint.Click += btPrint_Click;
      componentResourceManager.ApplyResources(radioGroup1, "radioGroup1");
      radioGroup1.Name = "radioGroup1";
      radioGroup1.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("radioGroup1.Properties.Appearance.Font");
      radioGroup1.Properties.Appearance.Options.UseFont = true;
      radioGroup1.Properties.Items.AddRange(new RadioGroupItem[4]
      {
        new RadioGroupItem(componentResourceManager.GetObject("radioGroup1.Properties.Items"), componentResourceManager.GetString("radioGroup1.Properties.Items1")),
        new RadioGroupItem(componentResourceManager.GetObject("radioGroup1.Properties.Items2"), componentResourceManager.GetString("radioGroup1.Properties.Items3")),
        new RadioGroupItem(componentResourceManager.GetObject("radioGroup1.Properties.Items4"), componentResourceManager.GetString("radioGroup1.Properties.Items5")),
        new RadioGroupItem(componentResourceManager.GetObject("radioGroup1.Properties.Items6"), componentResourceManager.GetString("radioGroup1.Properties.Items7"))
      });
      radioGroup1.SelectedIndexChanged += radioGroup1_SelectedIndexChanged;
      groupBox1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox1.AppearanceCaption.Font");
      groupBox1.AppearanceCaption.Options.UseFont = true;
      groupBox1.Controls.Add(radioGroup1);
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Name = "groupBox1";
      groupBox1.ShowCaption = false;
      componentResourceManager.ApplyResources(lvObjects, "lvObjects");
      lvObjects.Appearance.Font = (Font) componentResourceManager.GetObject("lvObjects.Appearance.Font");
      lvObjects.Appearance.Options.UseFont = true;
      lvObjects.Name = "lvObjects";
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
      gridView1.Appearance.ColumnFilterButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.ForeColor");
      gridView1.Appearance.ColumnFilterButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.GradientMode");
      gridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor");
      gridView1.Appearance.ColumnFilterButtonActive.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor2");
      gridView1.Appearance.ColumnFilterButtonActive.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BorderColor");
      gridView1.Appearance.ColumnFilterButtonActive.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.ForeColor");
      gridView1.Appearance.ColumnFilterButtonActive.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.GradientMode");
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
      gridView1.Appearance.Empty.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Empty.BackColor");
      gridView1.Appearance.Empty.Options.UseBackColor = true;
      gridView1.Appearance.EvenRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.BackColor");
      gridView1.Appearance.EvenRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.ForeColor");
      gridView1.Appearance.EvenRow.Options.UseBackColor = true;
      gridView1.Appearance.EvenRow.Options.UseForeColor = true;
      gridView1.Appearance.FilterCloseButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor");
      gridView1.Appearance.FilterCloseButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor2");
      gridView1.Appearance.FilterCloseButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BorderColor");
      gridView1.Appearance.FilterCloseButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.ForeColor");
      gridView1.Appearance.FilterCloseButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.GradientMode");
      gridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
      gridView1.Appearance.FilterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.BackColor");
      gridView1.Appearance.FilterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.ForeColor");
      gridView1.Appearance.FilterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
      gridView1.Appearance.FixedLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.BackColor");
      gridView1.Appearance.FixedLine.Options.UseBackColor = true;
      gridView1.Appearance.FocusedCell.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.BackColor");
      gridView1.Appearance.FocusedCell.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.ForeColor");
      gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
      gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
      gridView1.Appearance.FocusedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.BackColor");
      gridView1.Appearance.FocusedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.ForeColor");
      gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
      gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
      gridView1.Appearance.FooterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor");
      gridView1.Appearance.FooterPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor2");
      gridView1.Appearance.FooterPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BorderColor");
      gridView1.Appearance.FooterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.ForeColor");
      gridView1.Appearance.FooterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.GradientMode");
      gridView1.Appearance.FooterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
      gridView1.Appearance.FooterPanel.Options.UseForeColor = true;
      gridView1.Appearance.GroupButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BackColor");
      gridView1.Appearance.GroupButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BorderColor");
      gridView1.Appearance.GroupButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.ForeColor");
      gridView1.Appearance.GroupButton.Options.UseBackColor = true;
      gridView1.Appearance.GroupButton.Options.UseBorderColor = true;
      gridView1.Appearance.GroupButton.Options.UseForeColor = true;
      gridView1.Appearance.GroupFooter.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BackColor");
      gridView1.Appearance.GroupFooter.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BorderColor");
      gridView1.Appearance.GroupFooter.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.ForeColor");
      gridView1.Appearance.GroupFooter.Options.UseBackColor = true;
      gridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
      gridView1.Appearance.GroupFooter.Options.UseForeColor = true;
      gridView1.Appearance.GroupPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.BackColor");
      gridView1.Appearance.GroupPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.ForeColor");
      gridView1.Appearance.GroupPanel.Options.UseBackColor = true;
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
      gridView1.Appearance.HideSelectionRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.ForeColor");
      gridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
      gridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
      gridView1.Appearance.HorzLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.BackColor");
      gridView1.Appearance.HorzLine.Options.UseBackColor = true;
      gridView1.Appearance.OddRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.BackColor");
      gridView1.Appearance.OddRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.ForeColor");
      gridView1.Appearance.OddRow.Options.UseBackColor = true;
      gridView1.Appearance.OddRow.Options.UseForeColor = true;
      gridView1.Appearance.Preview.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Preview.BackColor");
      gridView1.Appearance.Preview.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Preview.ForeColor");
      gridView1.Appearance.Preview.Options.UseBackColor = true;
      gridView1.Appearance.Preview.Options.UseForeColor = true;
      gridView1.Appearance.Row.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.BackColor");
      gridView1.Appearance.Row.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.ForeColor");
      gridView1.Appearance.Row.Options.UseBackColor = true;
      gridView1.Appearance.Row.Options.UseForeColor = true;
      gridView1.Appearance.RowSeparator.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.BackColor");
      gridView1.Appearance.RowSeparator.Options.UseBackColor = true;
      gridView1.Appearance.SelectedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.BackColor");
      gridView1.Appearance.SelectedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.ForeColor");
      gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
      gridView1.Appearance.SelectedRow.Options.UseForeColor = true;
      gridView1.Appearance.VertLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.VertLine.BackColor");
      gridView1.Appearance.VertLine.Options.UseBackColor = true;
      gridView1.Columns.AddRange(new GridColumn[6]
      {
        colDate,
        colObject,
        colDevice,
        colTraffic,
        colResult,
        colTime
      });
      gridView1.GridControl = gridControl1;
      gridView1.Name = "gridView1";
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.FindMode = FindMode.Always;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.EnableAppearanceEvenRow = true;
      gridView1.OptionsView.EnableAppearanceOddRow = true;
      gridView1.OptionsView.ShowFooter = true;
      gridView1.OptionsView.ShowGroupPanel = false;
      componentResourceManager.ApplyResources(colDate, "colDate");
      colDate.FieldName = "Date";
      colDate.Name = "colDate";
      colDate.OptionsColumn.AllowEdit = false;
      colDate.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(colObject, "colObject");
      colObject.FieldName = "Object";
      colObject.Name = "colObject";
      colObject.OptionsColumn.AllowEdit = false;
      colObject.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(colDevice, "colDevice");
      colDevice.FieldName = "Position";
      colDevice.Name = "colDevice";
      colDevice.OptionsColumn.AllowEdit = false;
      colDevice.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(colTraffic, "colTraffic");
      colTraffic.FieldName = "Traffic";
      colTraffic.Name = "colTraffic";
      colTraffic.OptionsColumn.AllowEdit = false;
      colTraffic.Summary.AddRange(new GridSummaryItem[1]
      {
        new GridColumnSummaryItem((SummaryItemType) componentResourceManager.GetObject("colTraffic.Summary"))
      });
      componentResourceManager.ApplyResources(colResult, "colResult");
      colResult.FieldName = "Result";
      colResult.Name = "colResult";
      colResult.OptionsColumn.AllowEdit = false;
      colResult.Summary.AddRange(new GridSummaryItem[1]
      {
        new GridColumnSummaryItem((SummaryItemType) componentResourceManager.GetObject("colResult.Summary"))
      });
      componentResourceManager.ApplyResources(colTime, "colTime");
      colTime.DisplayFormat.FormatString = "HH:mm";
      colTime.DisplayFormat.FormatType = FormatType.DateTime;
      colTime.FieldName = "Time";
      colTime.Name = "colTime";
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Name = "labelControl1";
      Appearance.Options.UseFont = true;
      AutoScaleMode = AutoScaleMode.None;
      componentResourceManager.ApplyResources(this, "$this");
      ControlBox = false;
      Controls.Add(labelControl1);
      Controls.Add(gridControl1);
      Controls.Add(lvObjects);
      Controls.Add(groupBox3);
      Controls.Add(groupBox1);
      Controls.Add(btSearch);
      Controls.Add(btPrint);
      Controls.Add(label6);
      FormBorderStyle = FormBorderStyle.None;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmStatistic";
      WindowState = FormWindowState.Maximized;
      Load += frmStatistic_Load;
      Resize += frmStatistic_Resize;
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      checkEditbefore.Properties.EndInit();
      checkEditfrom.Properties.EndInit();
      dtpBefore.Properties.CalendarTimeProperties.EndInit();
      dtpBefore.Properties.EndInit();
      dtpFrom.Properties.CalendarTimeProperties.EndInit();
      dtpFrom.Properties.EndInit();
      radioGroup1.Properties.EndInit();
      groupBox1.EndInit();
      groupBox1.ResumeLayout(false);
      lvObjects.EndInit();
      gridControl1.EndInit();
      gridView1.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
