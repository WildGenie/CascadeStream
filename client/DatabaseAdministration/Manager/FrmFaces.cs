// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmFaces
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
using System.Threading;
using System.Windows.Forms;
using BasicComponents;
using CascadeManager.CascadeFlowClient;
using CascadeManager.Properties;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = System.Drawing.Image;

namespace CascadeManager
{
  public class FrmFaces : XtraForm
  {
    private string _queryString = "";
    private DataTable _dtMain = new DataTable();
    private string _selectCommand = "Select \r\nFaces.ID,\r\nSurname+' '+\r\nFirstName+' '+\r\nLastName as FIO,\r\nPassport,\r\nBirthday,\r\nPost,\r\nDepartment.DepartmentName,\r\nOrganization.Organization,\r\ncase when Country!='' then Country+', '\r\nelse ''end \r\n+\r\ncase when Region!=''then \r\n' ' +Region+', ' \r\nelse ''end \r\n+\r\ncase when City!='' then ', '+City+', '\r\nelse '' end  +\r\ncase when District!='' then \r\n',  ' +District+', '\r\nelse '' end\r\n+\r\ncase when Street!='' then \r\n', '+Street+', '\r\n else ''end+\r\ncase when Home!=''then \r\n', '+ Home+', ' else ''end+\r\ncase when Flat!=''then \r\n', '+ Flat+', '\r\n else ''end as Address,\r\nDateEmployment,\r\nModifiedDate,\r\ncase when Status = 0 then '" + Messages.Active + "' else '" + Messages.Archive + "' end as Status,\r\nArchiveDate,\r\nUsers.Name as UserName,\r\nTiming.Name as Timing,\r\nAccessCategory.Name,\r\ncase when \r\n(select count(ID) from CSKeys.dbo.Keys Where FaceID = Faces.ID)>0\r\nthen cast(1 as bit)\r\nelse\r\ncast(0 as bit)\r\nend Template\r\n\r\n\r\n from [dbo].[Faces]\r\nleft outer join  \r\nDepartment on \r\nDepartment.ID = Faces.Department\r\nleft outer join Organization on \r\nOrganization.ID = Faces.Organization\r\nleft outer join  \r\nTiming on Timing.ID = Faces.TimingID\r\nleft outer join  \r\nAccessCategory on \r\nAccessCategory.ID =  Faces.AccessID\r\nleft outer join  Users on Users.ID = Faces.EditUserID\r\n ";
    private bool _allowClose = false;
    private IContainer components = null;
    private bool _allowDelete;
    private bool _isCtrl;
    private Thread _keyThread;
    public bool StopFlag;
    private PanelControl panelControl1;
    private GroupControl groupControl1;
    private SimpleButton btPrint;
    private SimpleButton btArchive;
    private SimpleButton btDeleteArchive;
    private SimpleButton btDelete;
    private SimpleButton btEdit;
    private SimpleButton btAdd;
    private PictureEdit pictureEdit1;
    private LabelControl labelControl1;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colFIO;
    private GridColumn colPassport;
    private GridColumn colBirthday;
    private GridColumn colAddress;
    private GridColumn colModifiedDate;
    private GridColumn colStatus;
    private GridColumn colArchiveDate;
    private GridColumn colUserName;
    private GridColumn colName;
    private GridColumn colPhotoDb;
    private SimpleButton btImport;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem toolStripMenuItem4;
    private ToolStripMenuItem toolStripMenuItem5;
    private ToolStripMenuItem toolStripMenuItem6;
    private SimpleButton btRemoveKeys;
    private SimpleButton btCreateKeys;
    private SimpleButton btStop;
    private ProgressBarControl progressBarControl1;
    private GroupControl gbMain;
    private GridColumn colTemplate;

    public FrmFaces()
    {
      InitializeComponent();
      ControlBox = false;
    }

    public static bool SetScore(float[] val)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand("update ScoreSettings set Score1 = " + val[0].ToString().Replace(",", ".") + ",\r\nScore2 = " + val[1].ToString().Replace(",", ".") + ",\r\nQuality = " + val[2].ToString().Replace(",", ".") + ",\r\nPeriod = " + val[3].ToString().Replace(",", ".") + ", Time = " + val[4].ToString().Replace(",", "."), new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
        sqlCommand.Connection.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static float[] GetScore()
    {
      float[] numArray = new float[5];
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select * from ScoreSettings", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
          numArray[0] = Convert.ToSingle(sqlDataReader[0].ToString().Replace(".", ","));
          numArray[1] = Convert.ToSingle(sqlDataReader[1].ToString().Replace(".", ","));
          numArray[2] = Convert.ToSingle(sqlDataReader[2].ToString().Replace(".", ","));
          numArray[3] = Convert.ToSingle(sqlDataReader[3].ToString().Replace(".", ","));
          numArray[4] = Convert.ToSingle(sqlDataReader[4].ToString().Replace(".", ","));
        }
        sqlCommand.Connection.Close();
      }
      catch
      {
      }
      return numArray;
    }

    private void ReloadGrid()
    {
      _dtMain = new DataTable();
      string str = _selectCommand;
      if (_queryString != "")
        str = _selectCommand + " Where " + _queryString;
      SqlConnection connection = new SqlConnection(CommonSettings.ConnectionString);
      SqlCommand sqlCommand = new SqlCommand(str + " Order By FIO", connection);
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
        _dtMain.Columns.Add(new DataColumn
        {
          DataType = sqlDataReader.GetFieldType(ordinal),
          ColumnName = sqlDataReader.GetName(ordinal)
        });
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        if (values[7].ToString().Length > 2)
          values[7] = values[7].ToString().Substring(0, values[7].ToString().Length - 2);
        if (values[3].ToString() == "" || Convert.ToDateTime(values[3]) == new DateTime(1900, 1, 1))
          values[3] = null;
        if (values[11].ToString() == "" || Convert.ToDateTime(values[11]) == new DateTime(1900, 1, 1))
          values[11] = null;
        _dtMain.Rows.Add(values);
      }
      sqlCommand.Connection.Close();
      try
      {
        gridControl1.DataSource = _dtMain;
      }
      catch
      {
      }
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
      EditFaceForm editFaceForm = new EditFaceForm();
      editFaceForm.CurrentEmployer = new BcFace();
      if (editFaceForm.ShowDialog() != DialogResult.OK)
        return;
      SqlCommand sqlCommand = new SqlCommand(string.Concat((object) _selectCommand, (object) " Where Faces.ID like '", (object) editFaceForm.CurrentEmployer.Id, (object) "'"), new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      while (sqlDataReader.Read())
      {
        object[] values = new object[sqlDataReader.FieldCount];
        sqlDataReader.GetValues(values);
        if (values[7].ToString().Length > 1)
          values[7] = values[7].ToString().Substring(0, values[7].ToString().Length - 1);
        if (values[3].ToString() == "" || Convert.ToDateTime(values[3]) == new DateTime(1900, 1, 1))
          values[3] = null;
        if (values[11].ToString() == "" || Convert.ToDateTime(values[11]) == new DateTime(1900, 1, 1))
          values[11] = null;
        _dtMain.Rows.Add(values);
      }
      sqlCommand.Connection.Close();
      if (editFaceForm.AddNewValue)
        btAdd_Click(sender, e);
    }

    private void btEdit_Click(object sender, EventArgs e)
    {
      if (gridView1.SelectedRowsCount <= 0)
        return;
      EditFaceForm editFaceForm = new EditFaceForm();
      editFaceForm.CurrentEmployer = BcFace.LoadById((Guid) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0]);
      if (editFaceForm.CurrentEmployer.Status == 1)
      {
        if (XtraMessageBox.Show(Messages.PersonInArchive, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          editFaceForm.CurrentEmployer.Status = 0;
          editFaceForm.CurrentEmployer.Save();
        }
        if (gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"].ToString() == Messages.Archive)
          btDeleteArchive.Enabled = true;
        else
          btDeleteArchive.Enabled = false;
      }
      if (editFaceForm.ShowDialog() == DialogResult.OK)
      {
        SqlCommand sqlCommand = new SqlCommand(string.Concat((object) _selectCommand, (object) " Where Faces.ID like '", gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0], (object) "'"), new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
        {
          object[] values = new object[sqlDataReader.FieldCount];
          sqlDataReader.GetValues(values);
          if (values[7].ToString().Length > 1)
            values[7] = values[7].ToString().Substring(0, values[7].ToString().Length - 1);
          if (values[3].ToString() == "" || Convert.ToDateTime(values[3]) == new DateTime(1900, 1, 1))
            values[3] = null;
          if (values[11].ToString() == "" || Convert.ToDateTime(values[11]) == new DateTime(1900, 1, 1))
            values[11] = null;
          gridView1.GetDataRow(gridView1.GetSelectedRows()[0]).ItemArray = values;
        }
        sqlCommand.Connection.Close();
        gridView1_SelectionChanged(gridView1, new SelectionChangedEventArgs());
      }
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (gridView1.GetSelectedRows().Length <= 0 || XtraMessageBox.Show(Messages.DouYouWantToSendArchive, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      foreach (int rowHandle in gridView1.GetSelectedRows())
      {
        if (rowHandle >= 0)
        {
          new BcFace
          {
            Id = ((Guid) gridView1.GetDataRow(rowHandle)[0]),
            EditUserId = MainForm.CurrentUser.Id,
            ArchiveDate = DateTime.Now
          }.Delete();
          gridView1.GetDataRow(rowHandle)["Status"] = Messages.Archive;
          gridView1.GetDataRow(rowHandle)["ArchiveDate"] = DateTime.Now;
        }
      }
      if (gridView1.GetDataRow(gridView1.GetSelectedRows()[0])["Status"].ToString() == Messages.Archive)
        btDeleteArchive.Enabled = true;
      else
        btDeleteArchive.Enabled = false;
    }

    private void EmplooyersForm_Load(object sender, EventArgs e)
    {
      ControlBox = false;
      ReloadGrid();
      int[] actions = MainForm.CurrentUser.GetActions();
      for (int index = 0; index < actions.Length; ++index)
      {
        if (actions[index] == 1)
        {
          btAdd.Enabled = true;
          btImport.Enabled = true;
        }
        if (actions[index] == 2)
          btEdit.Enabled = true;
        if (actions[index] == 7)
        {
          btCreateKeys.Enabled = true;
          btRemoveKeys.Enabled = true;
        }
        if (actions[index] == 5)
          btDelete.Enabled = true;
        if (actions[index] == 3)
        {
          _allowDelete = true;
          btDeleteArchive.Enabled = true;
          gridView1.ClearSelection();
          if (gridView1.SelectedRowsCount == 0)
            btDeleteArchive.Enabled = false;
        }
      }
      colBirthday.Visible = true;
      colPassport.Visible = true;
    }

    private void btArchive_Click(object sender, EventArgs e)
    {
      if (btArchive.Text == Messages.Archives)
      {
        btDelete.Enabled = false;
        btArchive.Text = Messages.All;
        _queryString = "Status=1";
        ReloadGrid();
      }
      else
      {
        btDelete.Enabled = true;
        btArchive.Text = Messages.Archives;
        _queryString = "";
        ReloadGrid();
      }
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

    private void EmplooyersForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_allowClose)
        return;
      if (XtraMessageBox.Show(Messages.DoYouWantToExitNow, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      {
        e.Cancel = true;
      }
      else
      {
        GetColumnIndexs gci = new GetColumnIndexs();
        gci.FileName = Application.StartupPath + "\\Персонал";
        for (int index = 0; index < gridView1.Columns.Count; ++index)
          gci.ColumnDisplayIndexs.Add(gridView1.Columns[index].VisibleIndex);
        GetColumnIndexs.Save(gci);
      }
    }

    private void btDeleteArchive_Click(object sender, EventArgs e)
    {
      if (gridView1.GetSelectedRows().Length <= 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      foreach (int rowHandle in gridView1.GetSelectedRows())
      {
        if (gridView1.GetDataRow(rowHandle)["Status"].ToString() == Messages.Archive)
        {
          BcFace bcFace = new BcFace();
          bcFace.Id = (Guid) gridView1.GetDataRow(rowHandle)[0];
          new BcKey
          {
            FaceId = bcFace.Id
          }.DeleteByFaceId();
          new BcImage
          {
            FaceId = bcFace.Id
          }.DeleteFaceId();
          bcFace.DeleteArchive();
        }
      }
      gridView1.DeleteSelectedRows();
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int[] selectedRows = gridView1.GetSelectedRows();
      try
      {
        pictureEdit1.Image.Dispose();
        pictureEdit1.Image = null;
      }
      catch
      {
      }
      if (selectedRows.Length > 0)
      {
        byte[] buffer = BcFace.LoadIcon((Guid) gridView1.GetDataRow(selectedRows[0])["ID"]);
        try
        {
          pictureEdit1.Image = buffer.Length <= 0 ? null : Image.FromStream(new MemoryStream(buffer));
        }
        catch
        {
        }
        if (gridView1.GetDataRow(selectedRows[0])["Status"].ToString() == Messages.Archive && _allowDelete)
          btDeleteArchive.Enabled = true;
        else
          btDeleteArchive.Enabled = false;
      }
      else
        btDeleteArchive.Enabled = false;
    }

    private void frmFaces_FormClosing(object sender, FormClosingEventArgs e)
    {
      GetColumnIndexs gci = new GetColumnIndexs();
      gci.FileName = Application.StartupPath + "\\Персонал";
      for (int index = 0; index < gridView1.Columns.Count; ++index)
        gci.ColumnDisplayIndexs.Add(gridView1.Columns[index].VisibleIndex);
      GetColumnIndexs.Save(gci);
    }

    private void frmFaces_Resize(object sender, EventArgs e)
    {
      ControlBox = false;
    }

    private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (!e.Info.IsRowIndicator || e.RowHandle < 0)
        return;
      e.Info.DisplayText = (e.RowHandle + 1).ToString();
    }

    private void btImport_Click(object sender, EventArgs e)
    {
      FrmImport frmImport = new FrmImport();
      try
      {
        int num = (int) frmImport.ShowDialog();
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      ReloadGrid();
    }

    private void gridView1_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!_isCtrl)
        return;
      if (e.KeyChar == 18)
      {
        int[] selectedRows = gridView1.GetSelectedRows();
        for (int index = 0; index < selectedRows.Length; ++index)
        {
          foreach (BcKey bcKey in BcKey.LoadKyesByFaceId((Guid) gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0]))
          {
            if (bcKey.Ksid != -2)
              bcKey.DeleteByKsid();
          }
        }
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (e.KeyChar == 11)
      {
        int[] selectedRows = gridView1.GetSelectedRows();
        for (int index = 0; index < selectedRows.Length; ++index)
        {
          BcFace bcFace = new BcFace();
          bcFace.Id = (Guid) gridView1.GetDataRow(gridView1.GetSelectedRows()[index])[0];
          foreach (BcImage bcImage in BcImage.LoadByFaceId(bcFace.Id))
          {
            new BcKey
            {
              ImageId = bcImage.Id
            }.DeleteByImageId();
            TS.Sdk.StaticFace.Model.Image image;
            using (Bitmap source = new Bitmap(new MemoryStream(bcImage.Image)))
              image = source.ConvertFrom();
            FaceInfo face = MainForm.Engine.DetectMaxFace(image, null);
            if (face != null)
              new BcKey
              {
                ImageKey = MainForm.Engine.ExtractTemplate(image, face),
                FaceId = bcFace.Id,
                ImageId = bcImage.Id
              }.Save();
          }
        }
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void gridControl1_KeyUp(object sender, KeyEventArgs e)
    {
    }

    private void gridView1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.ControlKey)
        return;
      _isCtrl = false;
    }

    private void gridView1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.ControlKey)
        return;
      _isCtrl = true;
    }

    private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      if (e.ClickedItem == toolStripMenuItem2)
      {
        int num1 = (int) new FrmDeleteKeys().ShowDialog();
      }
      else if (e.ClickedItem == toolStripMenuItem1)
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          if (File.Exists(saveFileDialog.FileName))
            File.Delete(saveFileDialog.FileName);
        }
        catch
        {
        }
        FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate);
        foreach (int rowHandle in gridView1.GetSelectedRows())
        {
          SqlCommand sqlCommand = new SqlCommand("Select ImageKey from Keys WHERE KSID = -1 and FaceID = '" + (Guid) gridView1.GetDataRow(rowHandle)["ID"] + "'", new SqlConnection(CommonSettings.ConnectionStringKeys));
          sqlCommand.Connection.Open();
          SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
          while (sqlDataReader.Read())
          {
            try
            {
              byte[] buffer = (byte[]) sqlDataReader[0];
              if (buffer.Length > 0)
                fileStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
            }
          }
          sqlCommand.Connection.Close();
        }
        fileStream.Close();
        int num2 = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (gridView1.FocusedRowHandle >= 0 && e.ClickedItem == toolStripMenuItem3)
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          if (File.Exists(saveFileDialog.FileName))
            File.Delete(saveFileDialog.FileName);
        }
        catch
        {
        }
        FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate);
        SqlCommand sqlCommand = new SqlCommand("Select ImageKey from Keys WHERE KSID = -2 and FaceID = '" + (Guid) gridView1.GetDataRow(gridView1.FocusedRowHandle)["ID"] + "'", new SqlConnection(CommonSettings.ConnectionStringKeys));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        int num2 = 0;
        while (sqlDataReader.Read())
        {
          try
          {
            byte[] buffer = (byte[]) sqlDataReader[0];
            if (buffer.Length > 0)
              fileStream.Write(buffer, 0, buffer.Length);
          }
          catch
          {
          }
          ++num2;
        }
        sqlCommand.Connection.Close();
        fileStream.Close();
        int num3 = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (e.ClickedItem == toolStripMenuItem4)
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (gridView1.FocusedRowHandle < 0 || openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
          if (File.Exists(openFileDialog.FileName))
          {
            FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            byte[] buffer = new byte[10988];
            while (fileStream.Read(buffer, 0, 10988) != 0)
              new BcKey
              {
                ImageKey = buffer,
                Ksid = -2,
                FaceId = ((Guid) dataRow["ID"])
              }.Save();
            fileStream.Close();
          }
          int num2 = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch
        {
        }
      }
      else if (e.ClickedItem == toolStripMenuItem5)
      {
        foreach (int rowHandle in gridView1.GetSelectedRows())
        {
          Guid guid = (Guid) gridView1.GetDataRow(rowHandle)["ID"];
          SqlCommand sqlCommand = new SqlCommand("\r\ninsert into DeletedKeys \r\nselect\r\nID,FaceID,ImageID,getdate()from Keys Where KSID = -2 and FaceID = '" + guid + "'Delete  Keys WHERE KSID = -2 and FaceID = '" + (string) (object) guid + "'", new SqlConnection(CommonSettings.ConnectionStringKeys));
          sqlCommand.Connection.Open();
          sqlCommand.ExecuteNonQuery();
          sqlCommand.Connection.Close();
        }
        int num2 = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (e.ClickedItem != toolStripMenuItem6)
          return;
        FrmScoreValue frmScoreValue = new FrmScoreValue();
        if (frmScoreValue.ShowDialog() == DialogResult.OK)
        {
          foreach (int rowHandle in gridView1.GetSelectedRows())
          {
            Guid guid = (Guid) gridView1.GetDataRow(rowHandle)["ID"];
            SqlCommand sqlCommand = new SqlCommand("Update  Faces \r\nSet Score = @Score WHERE ID = @ID Update CSKeys.dbo.Keys Set ModifiedDate = getdate() Where FaceID = @ID", new SqlConnection(CommonSettings.ConnectionString));
            float num2 = Convert.ToSingle(frmScoreValue.ScoreVal);
            sqlCommand.Parameters.AddWithValue("@Score", num2);
            sqlCommand.Parameters.AddWithValue("@ID", guid);
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
          }
        }
        int num3 = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void CreateKeys(object obj)
    {
      BcKeySettings.LoadAll();
      List<Guid> list1 = (List<Guid>) obj;
      Invoke(new UpdateFormFunc(UpdateForm), (object) 0, (object) list1.Count, (object) 0);
      SqlCommand sqlCommand1 = new SqlCommand("Select distinct FaceID from Keys Where KSID = -1 ", new SqlConnection(CommonSettings.ConnectionStringKeys));
      sqlCommand1.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
      List<Guid> list2 = new List<Guid>();
      while (sqlDataReader.Read())
        list2.Add((Guid) sqlDataReader[0]);
      sqlCommand1.Connection.Close();
      int num = 0;
      foreach (Guid id in list1)
      {
        if (!StopFlag)
        {
          try
          {
            foreach (BcImage bcImage in BcImage.LoadByFaceId(id))
            {
              if (!StopFlag)
              {
                if (bcImage.Image != null && bcImage.Image.Length > 0)
                {
                  TS.Sdk.StaticFace.Model.Image image;
                  using (Bitmap source = new Bitmap(new MemoryStream(bcImage.Image)))
                    image = source.ConvertFrom();
                  FaceInfo face = MainForm.Engine.DetectMaxFace(image, null);
                  if (face != null && !list2.Contains(id))
                  {
                    byte[] template = MainForm.Engine.ExtractTemplate(image, face);
                    new BcKey
                    {
                      ImageKey = template,
                      ImageId = bcImage.Id,
                      FaceId = bcImage.FaceId,
                      Ksid = -1
                    }.Save();
                  }
                }
              }
              else
                break;
            }
          }
          catch
          {
          }
          try
          {
            SqlCommand sqlCommand2 = new SqlCommand("Update Faces Set keymodifieddate = getdate() Where id ='" + id + "'", new SqlConnection(CommonSettings.ConnectionString));
            sqlCommand2.Connection.Open();
            sqlCommand2.ExecuteNonQuery();
            sqlCommand2.Connection.Close();
          }
          catch
          {
          }
          ++num;
          Invoke(new UpdateFormFunc(UpdateForm), (object) 2, (object) list1.Count, (object) num);
        }
        else
          break;
      }
      Invoke(new UpdateFormFunc(UpdateForm), (object) 1, (object) list1.Count, (object) num);
    }

    private void UpdateImage(Bitmap bmp)
    {
      pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      pictureEdit1.Image.Dispose();
      pictureEdit1.Image = bmp;
    }

    private void UpdateForm(int type, int max, int val)
    {
      progressBarControl1.Properties.Maximum = max;
      progressBarControl1.EditValue = val;
      if (type == 0)
      {
        progressBarControl1.Visible = true;
        btStop.Visible = true;
        gbMain.Enabled = false;
      }
      else
      {
        if (type != 1)
          return;
        progressBarControl1.Visible = false;
        btStop.Visible = false;
        gbMain.Enabled = true;
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btCreateKeys_Click(object sender, EventArgs e)
    {
      StopFlag = false;
      _keyThread = new Thread(CreateKeys)
      {
        IsBackground = true
      };
      List<Guid> list = new List<Guid>();
      foreach (int rowHandle in gridView1.GetSelectedRows())
        list.Add((Guid) gridView1.GetDataRow(rowHandle)["ID"]);
      _keyThread.Start(list);
    }

    private void btRemoveKeys_Click(object sender, EventArgs e)
    {
      foreach (int rowHandle in gridView1.GetSelectedRows())
      {
        Guid guid = (Guid) gridView1.GetDataRow(rowHandle)["ID"];
        SqlCommand sqlCommand = new SqlCommand("\r\ninsert into DeletedKeys \r\nselect\r\nID,FaceID,ImageID,getdate()from Keys Where KSID != -1 and KSID != -2 and FaceID = '" + guid + "'Delete  Keys WHERE KSID != -1 and KSID != -2 and FaceID = '" + (string) (object) guid + "'", new SqlConnection(CommonSettings.ConnectionStringKeys));
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
        sqlCommand.Connection.Close();
      }
      int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btStop_Click(object sender, EventArgs e)
    {
      StopFlag = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmFaces));
      panelControl1 = new PanelControl();
      groupControl1 = new GroupControl();
      gbMain = new GroupControl();
      btAdd = new SimpleButton();
      btEdit = new SimpleButton();
      btRemoveKeys = new SimpleButton();
      btDelete = new SimpleButton();
      btCreateKeys = new SimpleButton();
      btDeleteArchive = new SimpleButton();
      btImport = new SimpleButton();
      btArchive = new SimpleButton();
      btPrint = new SimpleButton();
      btStop = new SimpleButton();
      progressBarControl1 = new ProgressBarControl();
      pictureEdit1 = new PictureEdit();
      labelControl1 = new LabelControl();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colFIO = new GridColumn();
      colPassport = new GridColumn();
      colBirthday = new GridColumn();
      colAddress = new GridColumn();
      colModifiedDate = new GridColumn();
      colStatus = new GridColumn();
      colArchiveDate = new GridColumn();
      colUserName = new GridColumn();
      colName = new GridColumn();
      colPhotoDb = new GridColumn();
      colTemplate = new GridColumn();
      menuStrip1 = new MenuStrip();
      toolStripMenuItem2 = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripMenuItem();
      toolStripMenuItem3 = new ToolStripMenuItem();
      toolStripMenuItem4 = new ToolStripMenuItem();
      toolStripMenuItem5 = new ToolStripMenuItem();
      toolStripMenuItem6 = new ToolStripMenuItem();
      panelControl1.BeginInit();
      panelControl1.SuspendLayout();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      gbMain.BeginInit();
      gbMain.SuspendLayout();
      progressBarControl1.Properties.BeginInit();
      pictureEdit1.Properties.BeginInit();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      menuStrip1.SuspendLayout();
      SuspendLayout();
      panelControl1.Controls.Add(groupControl1);
      panelControl1.Controls.Add(gridControl1);
      panelControl1.Controls.Add(menuStrip1);
      componentResourceManager.ApplyResources(panelControl1, "panelControl1");
      panelControl1.Name = "panelControl1";
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Appearance.Font = (Font) componentResourceManager.GetObject("groupControl1.Appearance.Font");
      groupControl1.Appearance.Options.UseFont = true;
      groupControl1.Controls.Add(gbMain);
      groupControl1.Controls.Add(btStop);
      groupControl1.Controls.Add(progressBarControl1);
      groupControl1.Controls.Add(pictureEdit1);
      groupControl1.Controls.Add(labelControl1);
      groupControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      groupControl1.Name = "groupControl1";
      groupControl1.ShowCaption = false;
      gbMain.Controls.Add(btAdd);
      gbMain.Controls.Add(btEdit);
      gbMain.Controls.Add(btRemoveKeys);
      gbMain.Controls.Add(btDelete);
      gbMain.Controls.Add(btCreateKeys);
      gbMain.Controls.Add(btDeleteArchive);
      gbMain.Controls.Add(btImport);
      gbMain.Controls.Add(btArchive);
      gbMain.Controls.Add(btPrint);
      componentResourceManager.ApplyResources(gbMain, "gbMain");
      gbMain.Name = "gbMain";
      gbMain.ShowCaption = false;
      btAdd.Appearance.Font = (Font) componentResourceManager.GetObject("btAdd.Appearance.Font");
      btAdd.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btAdd, "btAdd");
      btAdd.Image = (Image) componentResourceManager.GetObject("btAdd.Image");
      btAdd.LookAndFeel.SkinName = "Office 2007 Blue";
      btAdd.Name = "btAdd";
      btAdd.Click += btAdd_Click;
      btEdit.Appearance.Font = (Font) componentResourceManager.GetObject("btEdit.Appearance.Font");
      btEdit.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btEdit, "btEdit");
      btEdit.Image = (Image) componentResourceManager.GetObject("btEdit.Image");
      btEdit.LookAndFeel.SkinName = "Office 2007 Blue";
      btEdit.Name = "btEdit";
      btEdit.Click += btEdit_Click;
      btRemoveKeys.Appearance.Font = (Font) componentResourceManager.GetObject("btRemoveKeys.Appearance.Font");
      btRemoveKeys.Appearance.Options.UseFont = true;
      btRemoveKeys.Image = Resources.RemoveKeys;
      componentResourceManager.ApplyResources(btRemoveKeys, "btRemoveKeys");
      btRemoveKeys.LookAndFeel.SkinName = "Office 2007 Blue";
      btRemoveKeys.Name = "btRemoveKeys";
      btRemoveKeys.Click += btRemoveKeys_Click;
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Image = (Image) componentResourceManager.GetObject("btDelete.Image");
      btDelete.LookAndFeel.SkinName = "Office 2007 Blue";
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      btCreateKeys.Appearance.Font = (Font) componentResourceManager.GetObject("btCreateKeys.Appearance.Font");
      btCreateKeys.Appearance.Options.UseFont = true;
      btCreateKeys.Image = Resources.CreateKeys;
      componentResourceManager.ApplyResources(btCreateKeys, "btCreateKeys");
      btCreateKeys.LookAndFeel.SkinName = "Office 2007 Blue";
      btCreateKeys.Name = "btCreateKeys";
      btCreateKeys.Click += btCreateKeys_Click;
      btDeleteArchive.Appearance.Font = (Font) componentResourceManager.GetObject("btDeleteArchive.Appearance.Font");
      btDeleteArchive.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btDeleteArchive, "btDeleteArchive");
      btDeleteArchive.Image = (Image) componentResourceManager.GetObject("btDeleteArchive.Image");
      btDeleteArchive.LookAndFeel.SkinName = "Office 2007 Blue";
      btDeleteArchive.Name = "btDeleteArchive";
      btDeleteArchive.Click += btDeleteArchive_Click;
      btImport.Appearance.Font = (Font) componentResourceManager.GetObject("btImport.Appearance.Font");
      btImport.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btImport, "btImport");
      btImport.Image = Resources.download_database_icon32;
      btImport.LookAndFeel.SkinName = "Office 2007 Blue";
      btImport.Name = "btImport";
      btImport.Click += btImport_Click;
      btArchive.Appearance.Font = (Font) componentResourceManager.GetObject("btArchive.Appearance.Font");
      btArchive.Appearance.Options.UseFont = true;
      btArchive.Image = (Image) componentResourceManager.GetObject("btArchive.Image");
      componentResourceManager.ApplyResources(btArchive, "btArchive");
      btArchive.LookAndFeel.SkinName = "Office 2007 Blue";
      btArchive.Name = "btArchive";
      btArchive.Click += btArchive_Click;
      btPrint.Appearance.Font = (Font) componentResourceManager.GetObject("btPrint.Appearance.Font");
      btPrint.Appearance.Options.UseFont = true;
      btPrint.Image = Resources.excel32;
      componentResourceManager.ApplyResources(btPrint, "btPrint");
      btPrint.LookAndFeel.SkinName = "Office 2007 Blue";
      btPrint.Name = "btPrint";
      btPrint.Click += btPrint_Click;
      componentResourceManager.ApplyResources(btStop, "btStop");
      btStop.Name = "btStop";
      btStop.Click += btStop_Click;
      componentResourceManager.ApplyResources(progressBarControl1, "progressBarControl1");
      progressBarControl1.Name = "progressBarControl1";
      progressBarControl1.Properties.ShowTitle = true;
      componentResourceManager.ApplyResources(pictureEdit1, "pictureEdit1");
      pictureEdit1.Name = "pictureEdit1";
      pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      labelControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      labelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridControl1.KeyUp += gridControl1_KeyUp;
      gridView1.ColumnPanelRowHeight = 50;
      gridView1.Columns.AddRange(new GridColumn[12]
      {
        colID,
        colFIO,
        colPassport,
        colBirthday,
        colAddress,
        colModifiedDate,
        colStatus,
        colArchiveDate,
        colUserName,
        colName,
        colPhotoDb,
        colTemplate
      });
      gridView1.GridControl = gridControl1;
      gridView1.IndicatorWidth = 60;
      gridView1.Name = "gridView1";
      gridView1.OptionsCustomization.AllowFilter = false;
      gridView1.OptionsFind.AlwaysVisible = true;
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.AutoCalcPreviewLineCount = true;
      gridView1.OptionsView.ColumnAutoWidth = false;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
      gridView1.SelectionChanged += gridView1_SelectionChanged;
      gridView1.KeyDown += gridView1_KeyDown;
      gridView1.KeyUp += gridView1_KeyUp;
      gridView1.KeyPress += gridView1_KeyPress;
      colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      colID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colID, "colID");
      colID.FieldName = "ID";
      colID.Name = "colID";
      colID.OptionsColumn.AllowEdit = false;
      colID.OptionsColumn.ReadOnly = true;
      colFIO.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colFIO.AppearanceHeader.Font");
      colFIO.AppearanceHeader.Options.UseFont = true;
      colFIO.AppearanceHeader.Options.UseTextOptions = true;
      colFIO.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colFIO.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colFIO, "colFIO");
      colFIO.FieldName = "FIO";
      colFIO.Name = "colFIO";
      colFIO.OptionsColumn.AllowEdit = false;
      colFIO.OptionsColumn.ReadOnly = true;
      colPassport.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPassport.AppearanceHeader.Font");
      colPassport.AppearanceHeader.Options.UseFont = true;
      colPassport.AppearanceHeader.Options.UseTextOptions = true;
      colPassport.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colPassport.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colPassport, "colPassport");
      colPassport.FieldName = "Passport";
      colPassport.Name = "colPassport";
      colPassport.OptionsColumn.AllowEdit = false;
      colPassport.OptionsColumn.ReadOnly = true;
      colBirthday.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colBirthday.AppearanceHeader.Font");
      colBirthday.AppearanceHeader.Options.UseFont = true;
      colBirthday.AppearanceHeader.Options.UseTextOptions = true;
      colBirthday.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colBirthday.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colBirthday, "colBirthday");
      colBirthday.FieldName = "Birthday";
      colBirthday.Name = "colBirthday";
      colBirthday.OptionsColumn.AllowEdit = false;
      colBirthday.OptionsColumn.ReadOnly = true;
      colAddress.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colAddress.AppearanceHeader.Font");
      colAddress.AppearanceHeader.Options.UseFont = true;
      colAddress.AppearanceHeader.Options.UseTextOptions = true;
      colAddress.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colAddress.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colAddress, "colAddress");
      colAddress.FieldName = "Address";
      colAddress.Name = "colAddress";
      colAddress.OptionsColumn.AllowEdit = false;
      colAddress.OptionsColumn.ReadOnly = true;
      colModifiedDate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colModifiedDate.AppearanceHeader.Font");
      colModifiedDate.AppearanceHeader.Options.UseFont = true;
      colModifiedDate.AppearanceHeader.Options.UseTextOptions = true;
      colModifiedDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colModifiedDate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colModifiedDate, "colModifiedDate");
      colModifiedDate.FieldName = "ModifiedDate";
      colModifiedDate.Name = "colModifiedDate";
      colModifiedDate.OptionsColumn.AllowEdit = false;
      colModifiedDate.OptionsColumn.ReadOnly = true;
      colStatus.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colStatus.AppearanceHeader.Font");
      colStatus.AppearanceHeader.Options.UseFont = true;
      colStatus.AppearanceHeader.Options.UseTextOptions = true;
      colStatus.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colStatus.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colStatus, "colStatus");
      colStatus.FieldName = "Status";
      colStatus.Name = "colStatus";
      colStatus.OptionsColumn.AllowEdit = false;
      colStatus.OptionsColumn.ReadOnly = true;
      colArchiveDate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colArchiveDate.AppearanceHeader.Font");
      colArchiveDate.AppearanceHeader.Options.UseFont = true;
      colArchiveDate.AppearanceHeader.Options.UseTextOptions = true;
      colArchiveDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colArchiveDate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colArchiveDate, "colArchiveDate");
      colArchiveDate.FieldName = "ArchiveDate";
      colArchiveDate.Name = "colArchiveDate";
      colArchiveDate.OptionsColumn.AllowEdit = false;
      colArchiveDate.OptionsColumn.ReadOnly = true;
      colUserName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colUserName.AppearanceHeader.Font");
      colUserName.AppearanceHeader.Options.UseFont = true;
      colUserName.AppearanceHeader.Options.UseTextOptions = true;
      colUserName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colUserName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colUserName, "colUserName");
      colUserName.FieldName = "UserName";
      colUserName.Name = "colUserName";
      colUserName.OptionsColumn.AllowEdit = false;
      colUserName.OptionsColumn.ReadOnly = true;
      colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      colName.AppearanceHeader.Options.UseFont = true;
      colName.AppearanceHeader.Options.UseTextOptions = true;
      colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colName, "colName");
      colName.FieldName = "Name";
      colName.Name = "colName";
      colName.OptionsColumn.AllowEdit = false;
      colName.OptionsColumn.ReadOnly = true;
      colPhotoDb.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPhotoDb.AppearanceHeader.Font");
      colPhotoDb.AppearanceHeader.Options.UseFont = true;
      colPhotoDb.AppearanceHeader.Options.UseTextOptions = true;
      colPhotoDb.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colPhotoDb.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colPhotoDb, "colPhotoDb");
      colPhotoDb.FieldName = "PhotoDb";
      colPhotoDb.Name = "colPhotoDb";
      colPhotoDb.OptionsColumn.AllowEdit = false;
      colPhotoDb.OptionsColumn.ReadOnly = true;
      colTemplate.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colTemplate.AppearanceHeader.Font");
      colTemplate.AppearanceHeader.Options.UseFont = true;
      colTemplate.AppearanceHeader.Options.UseTextOptions = true;
      colTemplate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colTemplate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colTemplate, "colTemplate");
      colTemplate.FieldName = "Template";
      colTemplate.Name = "colTemplate";
      colTemplate.OptionsColumn.AllowEdit = false;
      colTemplate.OptionsColumn.ReadOnly = true;
      menuStrip1.Items.AddRange(new ToolStripItem[6]
      {
        toolStripMenuItem2,
        toolStripMenuItem1,
        toolStripMenuItem3,
        toolStripMenuItem4,
        toolStripMenuItem5,
        toolStripMenuItem6
      });
      componentResourceManager.ApplyResources(menuStrip1, "menuStrip1");
      menuStrip1.Name = "menuStrip1";
      menuStrip1.ItemClicked += menuStrip1_ItemClicked;
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      componentResourceManager.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      componentResourceManager.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
      toolStripMenuItem3.Name = "toolStripMenuItem3";
      componentResourceManager.ApplyResources(toolStripMenuItem3, "toolStripMenuItem3");
      toolStripMenuItem4.Name = "toolStripMenuItem4";
      componentResourceManager.ApplyResources(toolStripMenuItem4, "toolStripMenuItem4");
      toolStripMenuItem5.Name = "toolStripMenuItem5";
      componentResourceManager.ApplyResources(toolStripMenuItem5, "toolStripMenuItem5");
      toolStripMenuItem6.Name = "toolStripMenuItem6";
      componentResourceManager.ApplyResources(toolStripMenuItem6, "toolStripMenuItem6");
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(panelControl1);
      FormBorderStyle = FormBorderStyle.None;
      MainMenuStrip = menuStrip1;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmFaces";
      ShowIcon = false;
      ShowInTaskbar = false;
      SizeGripStyle = SizeGripStyle.Hide;
      WindowState = FormWindowState.Maximized;
      FormClosing += frmFaces_FormClosing;
      Load += EmplooyersForm_Load;
      Resize += frmFaces_Resize;
      panelControl1.EndInit();
      panelControl1.ResumeLayout(false);
      panelControl1.PerformLayout();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      groupControl1.PerformLayout();
      gbMain.EndInit();
      gbMain.ResumeLayout(false);
      progressBarControl1.Properties.EndInit();
      pictureEdit1.Properties.EndInit();
      gridControl1.EndInit();
      gridView1.EndInit();
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      ResumeLayout(false);
    }

    private delegate void UpdateImageFunc(Bitmap bmp);

    private delegate void UpdateFormFunc(int type, int max, int val);
  }
}
