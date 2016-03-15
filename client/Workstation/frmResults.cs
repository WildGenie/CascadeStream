// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmResults
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using BasicComponents;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Word;

namespace CascadeFlowClient
{
  public class FrmResults : XtraForm
  {
    private IContainer components = (IContainer) null;
    private FrmImages _imageForm;
    public static bool IsLoad;
    private SimpleButton btEdit;
    private SimpleButton btShow;
    private CheckEdit chbHide;
    private Label lbCategoryResult;
    private Label lbNameResult;
    private Label lbSexResult;
    public TreeList treeList1;
    private TreeListColumn colID;
    private TreeListColumn colFaceID;
    private TreeListColumn colDeviceID;
    private TreeListColumn colCategoryID;
    private TreeListColumn colObjectID;
    private TreeListColumn colDate;
    private TreeListColumn colImgType;
    private TreeListColumn colScore;
    private TreeListColumn colImageIcon;
    private TreeListColumn colName;
    private TreeListColumn colCategory;
    private TreeListColumn colPosition;
    private TreeListColumn colStatus;
    private TreeListColumn colDBName;
    private RepositoryItemDateEdit repositoryItemDateEdit1;
    private RepositoryItemImageEdit repositoryItemImageEdit1;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private SimpleButton btEditSelected;
    private Label lbBirthday;
    private Label label3;
    private GroupBox groupBox1;
    private SimpleButton btPrint;
    private TreeListColumn colImageID;
    private SimpleButton btAccept;
    private LabelControl labelControl1;
    private SpinEdit spinEdit1;
    private TreeListColumn treeListColumn1;
    private GroupControl groupControl1;
    private PictureEdit pbImgSource;
    private PictureEdit pbImgResult;
    private SimpleButton btAdd;

    public FrmResults()
    {
      this.InitializeComponent();
    }

    public FrmResults(FrmImages frm)
    {
      this._imageForm = frm;
      this.InitializeComponent();
    }

    private void btEdit_Click(object sender, EventArgs e)
    {
      try
      {
        TreeListNode focusedNode = this.treeList1.FocusedNode;
        new BcLog()
        {
          Id = ((int) focusedNode[(object) "ID"])
        }.ChangeStatus(focusedNode[(object) "DBName"].ToString());
        if (focusedNode.Checked)
        {
          this.btEdit.Text = Messages.Process;
          focusedNode.Checked = false;
        }
        else
        {
          this.btEdit.Text = Messages.RemoveProcess;
          focusedNode.Checked = true;
          if (this.chbHide.Checked)
          {
            focusedNode.Visible = false;
            if (focusedNode.HasChildren)
            {
              foreach (TreeListNode treeListNode in focusedNode.Nodes)
              {
                if (treeListNode.Checked)
                {
                  treeListNode.Visible = false;
                }
                else
                {
                  focusedNode.Visible = true;
                  this.treeList1.SetFocusedNode(focusedNode);
                  focusedNode.Selected = true;
                }
              }
            }
          }
        }
        this.treeList1_AfterCheckNode(new object(), new NodeEventArgs(this.treeList1.Nodes[0]));
      }
      catch
      {
      }
    }

    private void btShow_Click(object sender, EventArgs e)
    {
      if (this.pbImgResult.Image == null)
        return;
      try
      {
        if (this.treeList1.FocusedNode != null)
        {
          BcFace bcFace = BcFace.LoadById((Guid) this.treeList1.FocusedNode[(object) "FaceID"]);
          BcLog bcLog = BcLog.LoadById((int) this.treeList1.FocusedNode[(object) "ID"]);
          int num = (int) new FrmCompareImages()
          {
            CurrentEmployer = bcFace,
            CurrentLog = bcLog
          }.ShowDialog();
        }
      }
      catch
      {
      }
    }

    private void chbHide_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        lock (this.treeList1)
        {
          if (this.chbHide.Checked)
          {
            foreach (TreeListNode item_1 in this.treeList1.Nodes)
            {
              if (item_1.Checked)
                item_1.Visible = false;
              foreach (TreeListNode item_0 in item_1.Nodes)
              {
                if (item_0.Checked)
                  item_0.Visible = false;
                else
                  item_1.Visible = true;
              }
            }
          }
          else
          {
            foreach (TreeListNode item_3 in this.treeList1.Nodes)
            {
              if (item_3.Checked)
                item_3.Visible = true;
              foreach (TreeListNode item_2 in item_3.Nodes)
              {
                if (item_2.Checked)
                  item_2.Visible = true;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void frmResults_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (FrmImages.AllowClose)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void frmResults_Load(object sender, EventArgs e)
    {
      lock (FrmImages.DtResults)
      {
        this.WindowState = FormWindowState.Maximized;
        this.treeList1.DataSource = (object) FrmImages.DtResults;
      }
    }

    private BcObjects GetObjectById(List<BcObjects> obj, int id)
    {
      BcObjects bcObjects1 = new BcObjects();
      foreach (BcObjects bcObjects2 in obj)
      {
        if (id == bcObjects2.Id)
          return bcObjects2;
      }
      return bcObjects1;
    }

    private void treeList1_SelectionChanged(object sender, EventArgs e)
    {
    }

    private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
    {
      if (e.Node == null)
        return;
      try
      {
        try
        {
          if (!this.treeList1.FocusedNode.Checked)
            this.btEdit.Text = Messages.Process;
          else
            this.btEdit.Text = Messages.RemoveProcess;
          try
          {
            this.pbImgSource.Image.Dispose();
          }
          catch
          {
          }
          this.pbImgSource.Image = (Image) null;
          this.pbImgResult.Image = Image.FromStream((Stream) new MemoryStream(BcLog.LoadById((int) this.treeList1.FocusedNode[(object) "ID"]).Image));
          BcFace bcFace = BcFace.LoadById((Guid) this.treeList1.FocusedNode.GetValue((object) 1));
          if (bcFace.Id != Guid.Empty)
          {
            this.lbCategoryResult.Text = FrmImages.GetCategoryById(bcFace.AccessId).Name;
            this.lbNameResult.Text = bcFace.Surname + " " + bcFace.FirstName + " " + bcFace.LastName;
            this.lbBirthday.Text = bcFace.Birthday.ToShortDateString();
            if (bcFace.Sex == 0)
              this.lbSexResult.Text = Messages.Male;
            else
              this.lbSexResult.Text = Messages.Female;
            BcImage bcImage = BcImage.LoadMainImageByFaceId((Guid) this.treeList1.FocusedNode[(object) "FaceID"]);
            if (bcImage.Image != null && bcImage.Image.Length > 0)
            {
              try
              {
                FileStream fileStream = new FileStream(System.Windows.Forms.Application.StartupPath + "\\mainImage.jpg", FileMode.OpenOrCreate);
                fileStream.Write(bcImage.Image, 0, bcImage.Image.Length);
                fileStream.Close();
                this.pbImgSource.Image = (Image) new Bitmap(System.Windows.Forms.Application.StartupPath + "\\mainImage.jpg");
              }
              catch
              {
              }
            }
          }
          else
          {
            this.lbCategoryResult.Text = "-";
            this.lbNameResult.Text = "-";
            this.lbSexResult.Text = "-";
            this.pbImgSource.Image = (Image) null;
          }
        }
        catch
        {
        }
      }
      catch
      {
      }
    }

    private void treeList1_ValidateNode(object sender, ValidateNodeEventArgs e)
    {
    }

    private void treeList1_Validated(object sender, EventArgs e)
    {
    }

    private void treeList1_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
    }

    private void treeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
    {
    }

    private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
    {
      if (FrmResults.IsLoad)
        return;
      new BcLog()
      {
        Id = ((int) e.Node[(object) "ID"])
      }.ChangeStatus(e.Node[(object) "DBName"].ToString());
      if (this.chbHide.Checked && e.Node.Checked)
      {
        e.Node.Visible = false;
        if (e.Node.HasChildren)
        {
          foreach (TreeListNode treeListNode in e.Node.Nodes)
          {
            if (treeListNode.Checked)
              treeListNode.Visible = false;
            else
              e.Node.Visible = true;
          }
        }
      }
      if (e.Node.Checked)
      {
        try
        {
          BcDevices deviceById = FrmImages.GetDeviceById((Guid) e.Node[(object) "DeviceID"]);
          bool flag1 = true;
          foreach (TreeListNode treeListNode1 in this.treeList1.Nodes)
          {
            if (treeListNode1[(object) "DeviceID"].ToString() == deviceById.Id.ToString() && !treeListNode1.Checked)
            {
              flag1 = false;
              break;
            }
            bool flag2 = false;
            foreach (TreeListNode treeListNode2 in treeListNode1.Nodes)
            {
              if (treeListNode2[(object) "DeviceID"].ToString() == deviceById.Id.ToString() && !treeListNode2.Checked)
              {
                flag1 = false;
                flag2 = true;
                break;
              }
            }
            if (flag2)
              break;
          }
          if (flag1)
          {
            if (this._imageForm.pbImage1.Tag != null && ((BcDevices) this._imageForm.pbImage1.Tag).Id == deviceById.Id)
              this._imageForm.pbImage1.BackColor = this._imageForm.BlueColor;
            else if (this._imageForm.pbImage2.Tag != null && ((BcDevices) this._imageForm.pbImage2.Tag).Id == deviceById.Id)
              this._imageForm.pbImage2.BackColor = this._imageForm.BlueColor;
            else if (this._imageForm.pbImage3.Tag != null && ((BcDevices) this._imageForm.pbImage3.Tag).Id == deviceById.Id)
              this._imageForm.pbImage3.BackColor = this._imageForm.BlueColor;
            else if (this._imageForm.pbImage4.Tag != null && ((BcDevices) this._imageForm.pbImage4.Tag).Id == deviceById.Id)
              this._imageForm.pbImage4.BackColor = this._imageForm.BlueColor;
          }
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          BcDevices deviceById = FrmImages.GetDeviceById((Guid) e.Node[(object) "DeviceID"]);
          if (this._imageForm.pbImage1.Tag != null && ((BcDevices) this._imageForm.pbImage1.Tag).Id == deviceById.Id)
            this._imageForm.pbImage1.BackColor = this._imageForm.RedColor;
          else if (this._imageForm.pbImage2.Tag != null && ((BcDevices) this._imageForm.pbImage2.Tag).Id == deviceById.Id)
            this._imageForm.pbImage2.BackColor = this._imageForm.RedColor;
          else if (this._imageForm.pbImage3.Tag != null && ((BcDevices) this._imageForm.pbImage3.Tag).Id == deviceById.Id)
            this._imageForm.pbImage3.BackColor = this._imageForm.RedColor;
          else if (this._imageForm.pbImage4.Tag != null && ((BcDevices) this._imageForm.pbImage4.Tag).Id == deviceById.Id)
            this._imageForm.pbImage4.BackColor = this._imageForm.RedColor;
        }
        catch
        {
        }
      }
    }

    private void treeList1_NodeChanged(object sender, NodeChangedEventArgs e)
    {
      if (e.ChangeType != NodeChangeTypeEnum.Add || !(bool) e.Node.GetValue((object) "Status"))
        return;
      FrmResults.IsLoad = true;
      e.Node.Checked = true;
      FrmResults.IsLoad = false;
    }

    private void btEditSelected_Click(object sender, EventArgs e)
    {
      try
      {
        foreach (TreeListNode node in this.treeList1.Nodes)
        {
          if (node.Selected)
          {
            if (node.HasChildren)
            {
              foreach (TreeListNode treeListNode in node.Nodes)
              {
                if (!treeListNode.Checked)
                {
                  new BcLog()
                  {
                    Id = ((int) treeListNode[(object) "ID"])
                  }.ChangeStatus(treeListNode[(object) "DBName"].ToString());
                  treeListNode.Checked = true;
                }
              }
            }
            if (!node.Checked)
            {
              node.Checked = true;
              new BcLog()
              {
                Id = ((int) node[(object) "ID"])
              }.ChangeStatus(node[(object) "DBName"].ToString());
            }
          }
          else
          {
            foreach (TreeListNode treeListNode in node.Nodes)
            {
              if (treeListNode.Selected)
              {
                new BcLog()
                {
                  Id = ((int) treeListNode[(object) "ID"])
                }.ChangeStatus(treeListNode[(object) "DBName"].ToString());
                if (!treeListNode.Checked)
                {
                  treeListNode.Checked = true;
                  if (this.chbHide.Checked)
                    treeListNode.Visible = false;
                }
              }
            }
          }
          if (this.chbHide.Checked)
          {
            node.Visible = false;
            if (node.HasChildren)
            {
              foreach (TreeListNode treeListNode in node.Nodes)
              {
                if (treeListNode.Checked)
                {
                  treeListNode.Visible = false;
                }
                else
                {
                  node.Visible = true;
                  this.treeList1.SetFocusedNode(node);
                  node.Selected = true;
                }
              }
            }
          }
        }
        try
        {
          this.treeList1_AfterCheckNode(new object(), new NodeEventArgs(this.treeList1.Nodes[0]));
        }
        catch
        {
        }
      }
      catch
      {
      }
    }

    private void btPrint_Click(object sender, EventArgs e)
    {
      try
      {
        object obj = (object) Missing.Value;
        object Template = (object) (System.Windows.Forms.Application.StartupPath + "\\ImageDoc.dot");
        ApplicationClass applicationClass = new ApplicationClass();
        Document document = applicationClass.Documents.Add(ref Template, ref obj, ref obj, ref obj);
        if (this.pbImgResult.Image == null)
          this.pbImgResult.Image = (Image) new Bitmap(320, 240);
        if (this.pbImgSource.Image == null)
          this.pbImgSource.Image = (Image) new Bitmap(320, 240);
        if (this.pbImgResult.Image != null && this.pbImgSource.Image != null)
        {
          document.Tables.Item(1).Rows.Item(1).Cells.Item(1)[][] = Messages.PhotoFromDataBase;
          Word.Range range1 = document.Tables.Item(1).Rows.Item(2).Cells.Item(1)[];
          Image image1 = this.pbImgSource.Image;
          double num = (double) (640 / image1.Width);
          Bitmap bitmap1 = new Bitmap(image1, new Size(240 * image1.Width / image1.Height, 240));
          string str1 = string.Concat(new object[4]
          {
            (object) System.Windows.Forms.Application.StartupPath,
            (object) "\\",
            (object) Guid.NewGuid(),
            (object) ".bmp"
          });
          bitmap1.Save(str1, ImageFormat.Bmp);
          range1.InlineShapes.AddPicture(str1, ref obj, ref obj, ref obj);
          File.Delete(str1);
          document.Tables.Item(1).Rows.Item(3).Cells.Item(1)[][] = Messages.CapturedFrame;
          Word.Range range2 = document.Tables.Item(1).Rows.Item(4).Cells.Item(1)[];
          Image image2 = this.pbImgResult.Image;
          Bitmap bitmap2 = new Bitmap(image2, new Size(240 * image2.Width / image2.Height, 240));
          string str2 = string.Concat(new object[4]
          {
            (object) System.Windows.Forms.Application.StartupPath,
            (object) "\\",
            (object) Guid.NewGuid(),
            (object) ".bmp"
          });
          bitmap2.Save(str2, ImageFormat.Bmp);
          range2.InlineShapes.AddPicture(str2, ref obj, ref obj, ref obj);
          File.Delete(str2);
          document.Tables.Item(1).Rows.Item(5).Cells.Item(1)[][] = this.lbNameResult.Text + "\r\n" + this.lbCategoryResult.Text + "\r\n" + this.lbSexResult.Text + "\r\n" + this.lbBirthday.Text;
        }
        applicationClass.Visible = true;
      }
      catch
      {
      }
    }

    private void treeList1_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
    {
    }

    private void treeList1_DoubleClick(object sender, EventArgs e)
    {
      if (this.treeList1.FocusedNode == null)
        return;
      if (!this.treeList1.FocusedNode.HasChildren)
        this.btShow_Click(sender, e);
      else if (this.treeList1.FocusedNode.HasChildren && this.treeList1.FocusedNode.Expanded)
        this.btShow_Click(sender, e);
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      FrmImages.Period = (float) this.spinEdit1.Value;
      for (int index = 0; index < FrmImages.DtResults.Rows.Count; ++index)
      {
        DataRow dataRow = FrmImages.DtResults.Rows[index];
        dataRow["GroupID"] = (object) "";
        dataRow["ParentID"] = (object) "";
      }
      int num1 = 0;
      for (int index1 = FrmImages.DtResults.Rows.Count - 1; index1 >= 0; --index1)
      {
        try
        {
          DataRow dataRow1 = FrmImages.DtResults.Rows[index1];
          DateTime dateTime1 = ((DateTime) dataRow1["Date"]).AddSeconds(-(double) FrmImages.Period);
          DateTime dateTime2 = (DateTime) dataRow1["Date"];
          int num2 = dateTime2.Month;
          string str1 = num2.ToString();
          if (dateTime2.Month < 9)
            str1 = "0" + (object) dateTime2.Month;
          num2 = dateTime2.Day;
          string str2 = num2.ToString();
          if (dateTime2.Day < 9)
            str2 = "0" + (object) dateTime2.Day;
          string str3 = (string) (object) dateTime2.Year + (object) str1 + str2;
          string str4 = dataRow1["Name"].ToString();
          if (str4.Length > 30)
            str4 = str4.Substring(0, 30);
          else if (str4.Length < 30)
          {
            string str5 = "";
            for (int index2 = 0; index2 < 30 - str4.Length; ++index2)
              str5 += " ";
            str4 = str5 + str4;
          }
          string str6 = dataRow1["FaceID"].ToString();
          if (str6.Length > 30)
            str6 = str6.Substring(0, 30);
          else if (str6.Length < 30)
          {
            string str5 = "";
            for (int index2 = 0; index2 < 30 - str4.Length; ++index2)
              str5 += "0";
            str6 = str5 + str6;
          }
          if ((double) FrmImages.Period == 0.0)
          {
            dataRow1["ParentID"] = (object) num1++;
            dataRow1["GroupID"] = (object) num1++;
          }
          if (dataRow1["GroupID"].ToString() == "")
            dataRow1["GroupID"] = (object) (str3 + (object) "%Date" + (string) (object) dateTime2 + "%ID" + str6 + " %Name" + str4 + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second + (int) FrmImages.Period) + ")" + (string) dataRow1["DeviceNumber"]);
          if (dataRow1["ParentID"].ToString() == "")
            dataRow1["ParentID"] = (object) (str3 + (object) "%Date" + (string) (object) dateTime2 + "%ID" + str6 + " %Name" + str4 + " %Time(" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second) + "-" + (string) (object) new TimeSpan(dateTime2.Hour, dateTime2.Minute, dateTime2.Second + (int) FrmImages.Period) + ")" + (string) dataRow1["DeviceNumber"]);
          else
            dataRow1["GroupID"] = (object) num1++;
          if ((double) FrmImages.Period > 0.0)
          {
            DataRow[] dataRowArray = FrmImages.DtResults.Select("ParentID = '' and FaceID = '" + dataRow1["FaceID"] + "' and Date >='" + (string) (object) dateTime1 + "' and DeviceID = '" + (string) dataRow1["DeviceID"] + "'");
            if (dataRowArray.Length > 0)
            {
              foreach (DataRow dataRow2 in dataRowArray)
              {
                if (dataRow2["GroupID"].ToString() == "")
                  dataRow2["GroupID"] = (object) num1++;
                dataRow2["ParentID"] = dataRow1["ParentID"];
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      try
      {
        this.treeList1.RefreshDataSource();
      }
      catch
      {
      }
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
      List<BcImage> list1 = new List<BcImage>();
      List<Guid> list2 = new List<Guid>();
      foreach (TreeListNode treeListNode1 in this.treeList1.Nodes)
      {
        if (treeListNode1.Selected && treeListNode1[(object) "Category"].ToString() == Messages.NonCategory)
        {
          if (treeListNode1.HasChildren)
          {
            foreach (TreeListNode treeListNode2 in treeListNode1.Nodes)
            {
              if (treeListNode2.Selected && treeListNode2[(object) "Category"].ToString() == Messages.NonCategory)
              {
                BcLog bcLog = BcLog.LoadById((int) treeListNode2[(object) "ID"]);
                list1.Add(new BcImage()
                {
                  ImageIcon = bcLog.ImageIcon,
                  Image = bcLog.Image,
                  Comment = Messages.CapturedFrame + treeListNode2[(object) "DeviceName"]
                });
                list2.Add((Guid) treeListNode2[(object) "FaceID"]);
              }
            }
          }
          BcLog bcLog1 = BcLog.LoadById((int) treeListNode1[(object) "ID"]);
          list1.Add(new BcImage()
          {
            ImageIcon = bcLog1.ImageIcon,
            Image = bcLog1.Image,
            Comment = Messages.CapturedFrame + treeListNode1[(object) "DeviceName"]
          });
          list2.Add((Guid) treeListNode1[(object) "FaceID"]);
        }
        else
        {
          foreach (TreeListNode treeListNode2 in treeListNode1.Nodes)
          {
            if (treeListNode2.Selected && treeListNode2[(object) "Category"].ToString() == Messages.NonCategory)
            {
              BcLog bcLog = BcLog.LoadById((int) treeListNode2[(object) "ID"]);
              list1.Add(new BcImage()
              {
                ImageIcon = bcLog.ImageIcon,
                Image = bcLog.Image,
                Comment = Messages.CapturedFrame + treeListNode2[(object) "DeviceName"]
              });
              list2.Add((Guid) treeListNode2[(object) "FaceID"]);
            }
          }
        }
      }
      if (list1.Count > 0)
      {
        FrmEditFace frmEditFace = new FrmEditFace();
        frmEditFace.Images = list1;
        if (frmEditFace.ShowDialog() != DialogResult.OK)
          return;
        List<Guid> list3 = new List<Guid>();
        foreach (DataRow dataRow in (InternalDataCollectionBase) FrmImages.DtResults.Rows)
        {
          Guid guid = (Guid) dataRow["FaceID"];
          if (list2.Contains(guid))
          {
            if (!list3.Contains(guid))
            {
              SqlCommand sqlCommand = new SqlCommand("\r\nUpdate CSLogInfo.dbo.Log Set FaceID = @FaceID,\r\nCategory = @Category Where FaceID = @id", new SqlConnection(CommonSettings.ConnectionStringLog));
              sqlCommand.Parameters.AddWithValue("@FaceID", (object) frmEditFace.CurrentEmployer.Id);
              sqlCommand.Parameters.AddWithValue("@id", (object) guid);
              sqlCommand.Parameters.AddWithValue("@Category", (object) frmEditFace.Category);
              sqlCommand.Connection.Open();
              sqlCommand.ExecuteNonQuery();
              sqlCommand.Connection.Close();
              list3.Add(guid);
            }
            dataRow["FaceID"] = (object) frmEditFace.CurrentEmployer.Id;
            dataRow["Category"] = (object) frmEditFace.Category;
            dataRow["Name"] = (object) (frmEditFace.CurrentEmployer.Surname + " " + frmEditFace.CurrentEmployer.FirstName + " " + frmEditFace.CurrentEmployer.LastName);
          }
        }
      }
      else
      {
        int num = (int) XtraMessageBox.Show(Messages.SelectNonCategoryRecordToAdd, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmResults));
      this.btEdit = new SimpleButton();
      this.btShow = new SimpleButton();
      this.chbHide = new CheckEdit();
      this.lbCategoryResult = new Label();
      this.lbSexResult = new Label();
      this.lbNameResult = new Label();
      this.treeList1 = new TreeList();
      this.colID = new TreeListColumn();
      this.colFaceID = new TreeListColumn();
      this.colDeviceID = new TreeListColumn();
      this.colCategoryID = new TreeListColumn();
      this.colObjectID = new TreeListColumn();
      this.colImgType = new TreeListColumn();
      this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      this.colDate = new TreeListColumn();
      this.repositoryItemDateEdit1 = new RepositoryItemDateEdit();
      this.colImageIcon = new TreeListColumn();
      this.colScore = new TreeListColumn();
      this.colName = new TreeListColumn();
      this.colCategory = new TreeListColumn();
      this.colPosition = new TreeListColumn();
      this.colStatus = new TreeListColumn();
      this.colDBName = new TreeListColumn();
      this.colImageID = new TreeListColumn();
      this.treeListColumn1 = new TreeListColumn();
      this.repositoryItemImageEdit1 = new RepositoryItemImageEdit();
      this.btEditSelected = new SimpleButton();
      this.btAccept = new SimpleButton();
      this.labelControl1 = new LabelControl();
      this.spinEdit1 = new SpinEdit();
      this.btPrint = new SimpleButton();
      this.groupBox1 = new GroupBox();
      this.label3 = new Label();
      this.lbBirthday = new Label();
      this.groupControl1 = new GroupControl();
      this.btAdd = new SimpleButton();
      this.pbImgResult = new PictureEdit();
      this.pbImgSource = new PictureEdit();
      this.chbHide.Properties.BeginInit();
      this.treeList1.BeginInit();
      this.repositoryItemPictureEdit1.BeginInit();
      this.repositoryItemDateEdit1.BeginInit();
      this.repositoryItemDateEdit1.CalendarTimeProperties.BeginInit();
      this.repositoryItemImageEdit1.BeginInit();
      this.spinEdit1.Properties.BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupControl1.BeginInit();
      this.groupControl1.SuspendLayout();
      this.pbImgResult.Properties.BeginInit();
      this.pbImgSource.Properties.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.btEdit, "btEdit");
      this.btEdit.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btEdit.Appearance.Font");
      this.btEdit.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btEdit.Appearance.FontSizeDelta");
      this.btEdit.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btEdit.Appearance.FontStyleDelta");
      this.btEdit.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btEdit.Appearance.GradientMode");
      this.btEdit.Appearance.Image = (Image) componentResourceManager.GetObject("btEdit.Appearance.Image");
      this.btEdit.Appearance.Options.UseFont = true;
      this.btEdit.Name = "btEdit";
      this.btEdit.Click += new EventHandler(this.btEdit_Click);
      componentResourceManager.ApplyResources((object) this.btShow, "btShow");
      this.btShow.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btShow.Appearance.Font");
      this.btShow.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btShow.Appearance.FontSizeDelta");
      this.btShow.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btShow.Appearance.FontStyleDelta");
      this.btShow.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btShow.Appearance.GradientMode");
      this.btShow.Appearance.Image = (Image) componentResourceManager.GetObject("btShow.Appearance.Image");
      this.btShow.Appearance.Options.UseFont = true;
      this.btShow.Name = "btShow";
      this.btShow.Click += new EventHandler(this.btShow_Click);
      componentResourceManager.ApplyResources((object) this.chbHide, "chbHide");
      this.chbHide.Name = "chbHide";
      this.chbHide.Properties.AccessibleDescription = componentResourceManager.GetString("chbHide.Properties.AccessibleDescription");
      this.chbHide.Properties.AccessibleName = componentResourceManager.GetString("chbHide.Properties.AccessibleName");
      this.chbHide.Properties.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("chbHide.Properties.Appearance.Font");
      this.chbHide.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("chbHide.Properties.Appearance.FontSizeDelta");
      this.chbHide.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("chbHide.Properties.Appearance.FontStyleDelta");
      this.chbHide.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("chbHide.Properties.Appearance.GradientMode");
      this.chbHide.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("chbHide.Properties.Appearance.Image");
      this.chbHide.Properties.Appearance.Options.UseFont = true;
      this.chbHide.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbHide.Properties.AutoHeight");
      this.chbHide.Properties.Caption = componentResourceManager.GetString("chbHide.Properties.Caption");
      this.chbHide.Properties.DisplayValueChecked = componentResourceManager.GetString("chbHide.Properties.DisplayValueChecked");
      this.chbHide.Properties.DisplayValueGrayed = componentResourceManager.GetString("chbHide.Properties.DisplayValueGrayed");
      this.chbHide.Properties.DisplayValueUnchecked = componentResourceManager.GetString("chbHide.Properties.DisplayValueUnchecked");
      this.chbHide.CheckedChanged += new EventHandler(this.chbHide_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.lbCategoryResult, "lbCategoryResult");
      this.lbCategoryResult.Name = "lbCategoryResult";
      componentResourceManager.ApplyResources((object) this.lbSexResult, "lbSexResult");
      this.lbSexResult.Name = "lbSexResult";
      componentResourceManager.ApplyResources((object) this.lbNameResult, "lbNameResult");
      this.lbNameResult.Name = "lbNameResult";
      componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
      this.treeList1.Columns.AddRange(new TreeListColumn[16]
      {
        this.colID,
        this.colFaceID,
        this.colDeviceID,
        this.colCategoryID,
        this.colObjectID,
        this.colImgType,
        this.colDate,
        this.colImageIcon,
        this.colScore,
        this.colName,
        this.colCategory,
        this.colPosition,
        this.colStatus,
        this.colDBName,
        this.colImageID,
        this.treeListColumn1
      });
      this.treeList1.IndicatorWidth = 50;
      this.treeList1.KeyFieldName = "GroupID";
      this.treeList1.Name = "treeList1";
      this.treeList1.OptionsBehavior.AllowIncrementalSearch = true;
      this.treeList1.OptionsBehavior.AutoChangeParent = false;
      this.treeList1.OptionsBehavior.CloseEditorOnLostFocus = false;
      this.treeList1.OptionsBehavior.EnableFiltering = true;
      this.treeList1.OptionsBehavior.ExpandNodesOnIncrementalSearch = true;
      this.treeList1.OptionsBehavior.ImmediateEditor = false;
      this.treeList1.OptionsMenu.EnableColumnMenu = false;
      this.treeList1.OptionsMenu.EnableFooterMenu = false;
      this.treeList1.OptionsSelection.MultiSelect = true;
      this.treeList1.OptionsView.AutoWidth = false;
      this.treeList1.OptionsView.ShowCheckBoxes = true;
      this.treeList1.PreviewFieldName = "ParentID";
      this.treeList1.RepositoryItems.AddRange(new RepositoryItem[3]
      {
        (RepositoryItem) this.repositoryItemDateEdit1,
        (RepositoryItem) this.repositoryItemImageEdit1,
        (RepositoryItem) this.repositoryItemPictureEdit1
      });
      this.treeList1.CustomNodeCellEdit += new GetCustomNodeCellEditEventHandler(this.treeList1_CustomNodeCellEdit);
      this.treeList1.AfterCheckNode += new NodeEventHandler(this.treeList1_AfterCheckNode);
      this.treeList1.NodeChanged += new NodeChangedEventHandler(this.treeList1_NodeChanged);
      this.treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
      this.treeList1.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.treeList1_FocusedColumnChanged);
      this.treeList1.ValidateNode += new ValidateNodeEventHandler(this.treeList1_ValidateNode);
      this.treeList1.SelectionChanged += new EventHandler(this.treeList1_SelectionChanged);
      this.treeList1.CellValueChanged += new CellValueChangedEventHandler(this.treeList1_CellValueChanged);
      this.treeList1.DoubleClick += new EventHandler(this.treeList1_DoubleClick);
      this.treeList1.Validated += new EventHandler(this.treeList1_Validated);
      componentResourceManager.ApplyResources((object) this.colID, "colID");
      this.colID.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      this.colID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceCell.FontSizeDelta");
      this.colID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceCell.FontStyleDelta");
      this.colID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceCell.GradientMode");
      this.colID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colID.AppearanceCell.Image");
      this.colID.AppearanceCell.Options.UseFont = true;
      this.colID.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      this.colID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceHeader.FontSizeDelta");
      this.colID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceHeader.FontStyleDelta");
      this.colID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceHeader.GradientMode");
      this.colID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colID.AppearanceHeader.Image");
      this.colID.AppearanceHeader.Options.UseFont = true;
      this.colID.AppearanceHeader.Options.UseTextOptions = true;
      this.colID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colID.FieldName = "ID";
      this.colID.Name = "colID";
      componentResourceManager.ApplyResources((object) this.colFaceID, "colFaceID");
      this.colFaceID.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colFaceID.AppearanceCell.Font");
      this.colFaceID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colFaceID.AppearanceCell.FontSizeDelta");
      this.colFaceID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colFaceID.AppearanceCell.FontStyleDelta");
      this.colFaceID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colFaceID.AppearanceCell.GradientMode");
      this.colFaceID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colFaceID.AppearanceCell.Image");
      this.colFaceID.AppearanceCell.Options.UseFont = true;
      this.colFaceID.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colFaceID.AppearanceHeader.Font");
      this.colFaceID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colFaceID.AppearanceHeader.FontSizeDelta");
      this.colFaceID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colFaceID.AppearanceHeader.FontStyleDelta");
      this.colFaceID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colFaceID.AppearanceHeader.GradientMode");
      this.colFaceID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colFaceID.AppearanceHeader.Image");
      this.colFaceID.AppearanceHeader.Options.UseFont = true;
      this.colFaceID.AppearanceHeader.Options.UseTextOptions = true;
      this.colFaceID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colFaceID.FieldName = "FaceID";
      this.colFaceID.Name = "colFaceID";
      componentResourceManager.ApplyResources((object) this.colDeviceID, "colDeviceID");
      this.colDeviceID.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDeviceID.AppearanceCell.Font");
      this.colDeviceID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colDeviceID.AppearanceCell.FontSizeDelta");
      this.colDeviceID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDeviceID.AppearanceCell.FontStyleDelta");
      this.colDeviceID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDeviceID.AppearanceCell.GradientMode");
      this.colDeviceID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colDeviceID.AppearanceCell.Image");
      this.colDeviceID.AppearanceCell.Options.UseFont = true;
      this.colDeviceID.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDeviceID.AppearanceHeader.Font");
      this.colDeviceID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colDeviceID.AppearanceHeader.FontSizeDelta");
      this.colDeviceID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDeviceID.AppearanceHeader.FontStyleDelta");
      this.colDeviceID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDeviceID.AppearanceHeader.GradientMode");
      this.colDeviceID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colDeviceID.AppearanceHeader.Image");
      this.colDeviceID.AppearanceHeader.Options.UseFont = true;
      this.colDeviceID.AppearanceHeader.Options.UseTextOptions = true;
      this.colDeviceID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDeviceID.FieldName = "DeviceID";
      this.colDeviceID.Name = "colDeviceID";
      componentResourceManager.ApplyResources((object) this.colCategoryID, "colCategoryID");
      this.colCategoryID.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colCategoryID.AppearanceCell.Font");
      this.colCategoryID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colCategoryID.AppearanceCell.FontSizeDelta");
      this.colCategoryID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCategoryID.AppearanceCell.FontStyleDelta");
      this.colCategoryID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCategoryID.AppearanceCell.GradientMode");
      this.colCategoryID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colCategoryID.AppearanceCell.Image");
      this.colCategoryID.AppearanceCell.Options.UseFont = true;
      this.colCategoryID.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.Font");
      this.colCategoryID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.FontSizeDelta");
      this.colCategoryID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.FontStyleDelta");
      this.colCategoryID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.GradientMode");
      this.colCategoryID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colCategoryID.AppearanceHeader.Image");
      this.colCategoryID.AppearanceHeader.Options.UseFont = true;
      this.colCategoryID.AppearanceHeader.Options.UseTextOptions = true;
      this.colCategoryID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colCategoryID.FieldName = "CategoryID";
      this.colCategoryID.Name = "colCategoryID";
      componentResourceManager.ApplyResources((object) this.colObjectID, "colObjectID");
      this.colObjectID.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colObjectID.AppearanceCell.Font");
      this.colObjectID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colObjectID.AppearanceCell.FontSizeDelta");
      this.colObjectID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colObjectID.AppearanceCell.FontStyleDelta");
      this.colObjectID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colObjectID.AppearanceCell.GradientMode");
      this.colObjectID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colObjectID.AppearanceCell.Image");
      this.colObjectID.AppearanceCell.Options.UseFont = true;
      this.colObjectID.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colObjectID.AppearanceHeader.Font");
      this.colObjectID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colObjectID.AppearanceHeader.FontSizeDelta");
      this.colObjectID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colObjectID.AppearanceHeader.FontStyleDelta");
      this.colObjectID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colObjectID.AppearanceHeader.GradientMode");
      this.colObjectID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colObjectID.AppearanceHeader.Image");
      this.colObjectID.AppearanceHeader.Options.UseFont = true;
      this.colObjectID.AppearanceHeader.Options.UseTextOptions = true;
      this.colObjectID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colObjectID.FieldName = "ObjectID";
      this.colObjectID.Name = "colObjectID";
      componentResourceManager.ApplyResources((object) this.colImgType, "colImgType");
      this.colImgType.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colImgType.AppearanceCell.Font");
      this.colImgType.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImgType.AppearanceCell.FontSizeDelta");
      this.colImgType.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImgType.AppearanceCell.FontStyleDelta");
      this.colImgType.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImgType.AppearanceCell.GradientMode");
      this.colImgType.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImgType.AppearanceCell.Image");
      this.colImgType.AppearanceCell.Options.UseFont = true;
      this.colImgType.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colImgType.AppearanceHeader.Font");
      this.colImgType.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImgType.AppearanceHeader.FontSizeDelta");
      this.colImgType.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImgType.AppearanceHeader.FontStyleDelta");
      this.colImgType.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImgType.AppearanceHeader.GradientMode");
      this.colImgType.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImgType.AppearanceHeader.Image");
      this.colImgType.AppearanceHeader.Options.UseFont = true;
      this.colImgType.AppearanceHeader.Options.UseTextOptions = true;
      this.colImgType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colImgType.ColumnEdit = (RepositoryItem) this.repositoryItemPictureEdit1;
      this.colImgType.FieldName = "ImgType";
      this.colImgType.Name = "colImgType";
      this.colImgType.OptionsColumn.AllowEdit = false;
      this.colImgType.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
      this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      componentResourceManager.ApplyResources((object) this.colDate, "colDate");
      this.colDate.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDate.AppearanceCell.Font");
      this.colDate.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colDate.AppearanceCell.FontSizeDelta");
      this.colDate.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDate.AppearanceCell.FontStyleDelta");
      this.colDate.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDate.AppearanceCell.GradientMode");
      this.colDate.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colDate.AppearanceCell.Image");
      this.colDate.AppearanceCell.Options.UseFont = true;
      this.colDate.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDate.AppearanceHeader.Font");
      this.colDate.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colDate.AppearanceHeader.FontSizeDelta");
      this.colDate.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDate.AppearanceHeader.FontStyleDelta");
      this.colDate.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDate.AppearanceHeader.GradientMode");
      this.colDate.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colDate.AppearanceHeader.Image");
      this.colDate.AppearanceHeader.Options.UseFont = true;
      this.colDate.AppearanceHeader.Options.UseTextOptions = true;
      this.colDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDate.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      this.colDate.ColumnEdit = (RepositoryItem) this.repositoryItemDateEdit1;
      this.colDate.FieldName = "Date";
      this.colDate.Name = "colDate";
      this.colDate.OptionsColumn.AllowEdit = false;
      this.colDate.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.repositoryItemDateEdit1, "repositoryItemDateEdit1");
      this.repositoryItemDateEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemDateEdit1.Buttons"))
      });
      this.repositoryItemDateEdit1.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("repositoryItemDateEdit1.CalendarTimeProperties.AccessibleDescription");
      this.repositoryItemDateEdit1.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("repositoryItemDateEdit1.CalendarTimeProperties.AccessibleName");
      this.repositoryItemDateEdit1.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.AutoHeight");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.AutoComplete");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.BeepOnError");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("repositoryItemDateEdit1.CalendarTimeProperties.Mask.EditMask");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.MaskType");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.PlaceHolder");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.SaveLiteral");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.ShowPlaceHolders");
      this.repositoryItemDateEdit1.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      this.repositoryItemDateEdit1.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("repositoryItemDateEdit1.CalendarTimeProperties.NullValuePrompt");
      this.repositoryItemDateEdit1.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      this.repositoryItemDateEdit1.DisplayFormat.FormatString = "HH:mm:ss - dd.MM.yyyy";
      this.repositoryItemDateEdit1.DisplayFormat.FormatType = FormatType.DateTime;
      this.repositoryItemDateEdit1.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.AutoComplete");
      this.repositoryItemDateEdit1.Mask.BeepOnError = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.BeepOnError");
      this.repositoryItemDateEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemDateEdit1.Mask.EditMask");
      this.repositoryItemDateEdit1.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.IgnoreMaskBlank");
      this.repositoryItemDateEdit1.Mask.MaskType = (MaskType) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.MaskType");
      this.repositoryItemDateEdit1.Mask.PlaceHolder = (char) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.PlaceHolder");
      this.repositoryItemDateEdit1.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.SaveLiteral");
      this.repositoryItemDateEdit1.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.ShowPlaceHolders");
      this.repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("repositoryItemDateEdit1.Mask.UseMaskAsDisplayFormat");
      this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
      componentResourceManager.ApplyResources((object) this.colImageIcon, "colImageIcon");
      this.colImageIcon.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colImageIcon.AppearanceCell.Font");
      this.colImageIcon.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImageIcon.AppearanceCell.FontSizeDelta");
      this.colImageIcon.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageIcon.AppearanceCell.FontStyleDelta");
      this.colImageIcon.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageIcon.AppearanceCell.GradientMode");
      this.colImageIcon.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImageIcon.AppearanceCell.Image");
      this.colImageIcon.AppearanceCell.Options.UseFont = true;
      this.colImageIcon.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colImageIcon.AppearanceHeader.Font");
      this.colImageIcon.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImageIcon.AppearanceHeader.FontSizeDelta");
      this.colImageIcon.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageIcon.AppearanceHeader.FontStyleDelta");
      this.colImageIcon.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageIcon.AppearanceHeader.GradientMode");
      this.colImageIcon.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImageIcon.AppearanceHeader.Image");
      this.colImageIcon.AppearanceHeader.Options.UseFont = true;
      this.colImageIcon.AppearanceHeader.Options.UseTextOptions = true;
      this.colImageIcon.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colImageIcon.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      this.colImageIcon.ColumnEdit = (RepositoryItem) this.repositoryItemPictureEdit1;
      this.colImageIcon.FieldName = "ImageIcon";
      this.colImageIcon.Name = "colImageIcon";
      this.colImageIcon.OptionsColumn.AllowEdit = false;
      this.colImageIcon.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colScore, "colScore");
      this.colScore.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colScore.AppearanceCell.Font");
      this.colScore.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colScore.AppearanceCell.FontSizeDelta");
      this.colScore.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colScore.AppearanceCell.FontStyleDelta");
      this.colScore.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colScore.AppearanceCell.GradientMode");
      this.colScore.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colScore.AppearanceCell.Image");
      this.colScore.AppearanceCell.Options.UseFont = true;
      this.colScore.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colScore.AppearanceHeader.Font");
      this.colScore.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colScore.AppearanceHeader.FontSizeDelta");
      this.colScore.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colScore.AppearanceHeader.FontStyleDelta");
      this.colScore.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colScore.AppearanceHeader.GradientMode");
      this.colScore.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colScore.AppearanceHeader.Image");
      this.colScore.AppearanceHeader.Options.UseFont = true;
      this.colScore.AppearanceHeader.Options.UseTextOptions = true;
      this.colScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      this.colScore.FieldName = "Score";
      this.colScore.Name = "colScore";
      this.colScore.OptionsColumn.AllowEdit = false;
      this.colScore.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      this.colName.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceCell.FontSizeDelta");
      this.colName.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceCell.FontStyleDelta");
      this.colName.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceCell.GradientMode");
      this.colName.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colName.AppearanceCell.Image");
      this.colName.AppearanceCell.Options.UseFont = true;
      this.colName.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      this.colName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
      this.colName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
      this.colName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
      this.colName.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colName.AppearanceHeader.Image");
      this.colName.AppearanceHeader.Options.UseFont = true;
      this.colName.AppearanceHeader.Options.UseTextOptions = true;
      this.colName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colName.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      this.colName.FieldName = "Name";
      this.colName.Name = "colName";
      this.colName.OptionsColumn.AllowEdit = false;
      this.colName.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colCategory, "colCategory");
      this.colCategory.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colCategory.AppearanceCell.Font");
      this.colCategory.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colCategory.AppearanceCell.FontSizeDelta");
      this.colCategory.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCategory.AppearanceCell.FontStyleDelta");
      this.colCategory.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCategory.AppearanceCell.GradientMode");
      this.colCategory.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colCategory.AppearanceCell.Image");
      this.colCategory.AppearanceCell.Options.UseFont = true;
      this.colCategory.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colCategory.AppearanceHeader.Font");
      this.colCategory.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colCategory.AppearanceHeader.FontSizeDelta");
      this.colCategory.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colCategory.AppearanceHeader.FontStyleDelta");
      this.colCategory.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colCategory.AppearanceHeader.GradientMode");
      this.colCategory.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colCategory.AppearanceHeader.Image");
      this.colCategory.AppearanceHeader.Options.UseFont = true;
      this.colCategory.AppearanceHeader.Options.UseTextOptions = true;
      this.colCategory.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colCategory.FieldName = "Category";
      this.colCategory.Name = "colCategory";
      this.colCategory.OptionsColumn.AllowEdit = false;
      this.colCategory.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colPosition, "colPosition");
      this.colPosition.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colPosition.AppearanceCell.Font");
      this.colPosition.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colPosition.AppearanceCell.FontSizeDelta");
      this.colPosition.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPosition.AppearanceCell.FontStyleDelta");
      this.colPosition.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPosition.AppearanceCell.GradientMode");
      this.colPosition.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colPosition.AppearanceCell.Image");
      this.colPosition.AppearanceCell.Options.UseFont = true;
      this.colPosition.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      this.colPosition.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontSizeDelta");
      this.colPosition.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontStyleDelta");
      this.colPosition.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPosition.AppearanceHeader.GradientMode");
      this.colPosition.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colPosition.AppearanceHeader.Image");
      this.colPosition.AppearanceHeader.Options.UseFont = true;
      this.colPosition.AppearanceHeader.Options.UseTextOptions = true;
      this.colPosition.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colPosition.FieldName = "Position";
      this.colPosition.Name = "colPosition";
      this.colPosition.OptionsColumn.AllowEdit = false;
      this.colPosition.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colStatus, "colStatus");
      this.colStatus.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colStatus.AppearanceCell.Font");
      this.colStatus.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colStatus.AppearanceCell.FontSizeDelta");
      this.colStatus.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colStatus.AppearanceCell.FontStyleDelta");
      this.colStatus.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colStatus.AppearanceCell.GradientMode");
      this.colStatus.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colStatus.AppearanceCell.Image");
      this.colStatus.AppearanceCell.Options.UseFont = true;
      this.colStatus.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colStatus.AppearanceHeader.Font");
      this.colStatus.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colStatus.AppearanceHeader.FontSizeDelta");
      this.colStatus.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colStatus.AppearanceHeader.FontStyleDelta");
      this.colStatus.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colStatus.AppearanceHeader.GradientMode");
      this.colStatus.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colStatus.AppearanceHeader.Image");
      this.colStatus.AppearanceHeader.Options.UseFont = true;
      this.colStatus.AppearanceHeader.Options.UseTextOptions = true;
      this.colStatus.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colStatus.FieldName = "Status";
      this.colStatus.Name = "colStatus";
      this.colStatus.OptionsColumn.AllowEdit = false;
      this.colStatus.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.colDBName, "colDBName");
      this.colDBName.AppearanceCell.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDBName.AppearanceCell.Font");
      this.colDBName.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colDBName.AppearanceCell.FontSizeDelta");
      this.colDBName.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDBName.AppearanceCell.FontStyleDelta");
      this.colDBName.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDBName.AppearanceCell.GradientMode");
      this.colDBName.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colDBName.AppearanceCell.Image");
      this.colDBName.AppearanceCell.Options.UseFont = true;
      this.colDBName.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("colDBName.AppearanceHeader.Font");
      this.colDBName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colDBName.AppearanceHeader.FontSizeDelta");
      this.colDBName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colDBName.AppearanceHeader.FontStyleDelta");
      this.colDBName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colDBName.AppearanceHeader.GradientMode");
      this.colDBName.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colDBName.AppearanceHeader.Image");
      this.colDBName.AppearanceHeader.Options.UseFont = true;
      this.colDBName.AppearanceHeader.Options.UseTextOptions = true;
      this.colDBName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.colDBName.FieldName = "DBName";
      this.colDBName.Name = "colDBName";
      componentResourceManager.ApplyResources((object) this.colImageID, "colImageID");
      this.colImageID.FieldName = "ImageID";
      this.colImageID.Name = "colImageID";
      componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
      this.treeListColumn1.AppearanceHeader.Font = (System.Drawing.Font) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Font");
      this.treeListColumn1.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontSizeDelta");
      this.treeListColumn1.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontStyleDelta");
      this.treeListColumn1.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.GradientMode");
      this.treeListColumn1.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Image");
      this.treeListColumn1.AppearanceHeader.Options.UseFont = true;
      this.treeListColumn1.AppearanceHeader.Options.UseTextOptions = true;
      this.treeListColumn1.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      this.treeListColumn1.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      this.treeListColumn1.FieldName = "DeviceNumber";
      this.treeListColumn1.Name = "treeListColumn1";
      this.treeListColumn1.OptionsColumn.AllowEdit = false;
      this.treeListColumn1.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.repositoryItemImageEdit1, "repositoryItemImageEdit1");
      this.repositoryItemImageEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemImageEdit1.Buttons"))
      });
      this.repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
      componentResourceManager.ApplyResources((object) this.btEditSelected, "btEditSelected");
      this.btEditSelected.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btEditSelected.Appearance.Font");
      this.btEditSelected.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btEditSelected.Appearance.FontSizeDelta");
      this.btEditSelected.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btEditSelected.Appearance.FontStyleDelta");
      this.btEditSelected.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btEditSelected.Appearance.GradientMode");
      this.btEditSelected.Appearance.Image = (Image) componentResourceManager.GetObject("btEditSelected.Appearance.Image");
      this.btEditSelected.Appearance.Options.UseFont = true;
      this.btEditSelected.Name = "btEditSelected";
      this.btEditSelected.Click += new EventHandler(this.btEditSelected_Click);
      componentResourceManager.ApplyResources((object) this.btAccept, "btAccept");
      this.btAccept.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      this.btAccept.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
      this.btAccept.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
      this.btAccept.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
      this.btAccept.Appearance.Image = (Image) componentResourceManager.GetObject("btAccept.Appearance.Image");
      this.btAccept.Appearance.Options.UseFont = true;
      this.btAccept.Name = "btAccept";
      this.btAccept.Click += new EventHandler(this.btAccept_Click);
      componentResourceManager.ApplyResources((object) this.labelControl1, "labelControl1");
      this.labelControl1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.DisabledImage");
      this.labelControl1.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      this.labelControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl1.Appearance.FontSizeDelta");
      this.labelControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl1.Appearance.FontStyleDelta");
      this.labelControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl1.Appearance.GradientMode");
      this.labelControl1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.HoverImage");
      this.labelControl1.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl1.Appearance.Image");
      this.labelControl1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.PressedImage");
      this.labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources((object) this.spinEdit1, "spinEdit1");
      this.spinEdit1.Name = "spinEdit1";
      this.spinEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("spinEdit1.Properties.AccessibleDescription");
      this.spinEdit1.Properties.AccessibleName = componentResourceManager.GetString("spinEdit1.Properties.AccessibleName");
      this.spinEdit1.Properties.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("spinEdit1.Properties.Appearance.Font");
      this.spinEdit1.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("spinEdit1.Properties.Appearance.FontSizeDelta");
      this.spinEdit1.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("spinEdit1.Properties.Appearance.FontStyleDelta");
      this.spinEdit1.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("spinEdit1.Properties.Appearance.GradientMode");
      this.spinEdit1.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("spinEdit1.Properties.Appearance.Image");
      this.spinEdit1.Properties.Appearance.Options.UseFont = true;
      this.spinEdit1.Properties.AutoHeight = (bool) componentResourceManager.GetObject("spinEdit1.Properties.AutoHeight");
      this.spinEdit1.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.spinEdit1.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("spinEdit1.Properties.Mask.AutoComplete");
      this.spinEdit1.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("spinEdit1.Properties.Mask.BeepOnError");
      this.spinEdit1.Properties.Mask.EditMask = componentResourceManager.GetString("spinEdit1.Properties.Mask.EditMask");
      this.spinEdit1.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("spinEdit1.Properties.Mask.IgnoreMaskBlank");
      this.spinEdit1.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("spinEdit1.Properties.Mask.MaskType");
      this.spinEdit1.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("spinEdit1.Properties.Mask.PlaceHolder");
      this.spinEdit1.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("spinEdit1.Properties.Mask.SaveLiteral");
      this.spinEdit1.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("spinEdit1.Properties.Mask.ShowPlaceHolders");
      this.spinEdit1.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("spinEdit1.Properties.Mask.UseMaskAsDisplayFormat");
      this.spinEdit1.Properties.NullValuePrompt = componentResourceManager.GetString("spinEdit1.Properties.NullValuePrompt");
      this.spinEdit1.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("spinEdit1.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources((object) this.btPrint, "btPrint");
      this.btPrint.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btPrint.Appearance.Font");
      this.btPrint.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPrint.Appearance.FontSizeDelta");
      this.btPrint.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPrint.Appearance.FontStyleDelta");
      this.btPrint.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPrint.Appearance.GradientMode");
      this.btPrint.Appearance.Image = (Image) componentResourceManager.GetObject("btPrint.Appearance.Image");
      this.btPrint.Appearance.Options.UseFont = true;
      this.btPrint.Name = "btPrint";
      this.btPrint.Click += new EventHandler(this.btPrint_Click);
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.lbCategoryResult);
      this.groupBox1.Controls.Add((Control) this.lbSexResult);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.lbNameResult);
      this.groupBox1.Controls.Add((Control) this.lbBirthday);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.lbBirthday, "lbBirthday");
      this.lbBirthday.Name = "lbBirthday";
      componentResourceManager.ApplyResources((object) this.groupControl1, "groupControl1");
      this.groupControl1.Controls.Add((Control) this.btAdd);
      this.groupControl1.Controls.Add((Control) this.btEditSelected);
      this.groupControl1.Controls.Add((Control) this.pbImgResult);
      this.groupControl1.Controls.Add((Control) this.btAccept);
      this.groupControl1.Controls.Add((Control) this.pbImgSource);
      this.groupControl1.Controls.Add((Control) this.labelControl1);
      this.groupControl1.Controls.Add((Control) this.groupBox1);
      this.groupControl1.Controls.Add((Control) this.spinEdit1);
      this.groupControl1.Controls.Add((Control) this.chbHide);
      this.groupControl1.Controls.Add((Control) this.btPrint);
      this.groupControl1.Controls.Add((Control) this.btShow);
      this.groupControl1.Controls.Add((Control) this.btEdit);
      this.groupControl1.Name = "groupControl1";
      componentResourceManager.ApplyResources((object) this.btAdd, "btAdd");
      this.btAdd.Appearance.Font = (System.Drawing.Font) componentResourceManager.GetObject("btAdd.Appearance.Font");
      this.btAdd.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAdd.Appearance.FontSizeDelta");
      this.btAdd.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAdd.Appearance.FontStyleDelta");
      this.btAdd.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAdd.Appearance.GradientMode");
      this.btAdd.Appearance.Image = (Image) componentResourceManager.GetObject("btAdd.Appearance.Image");
      this.btAdd.Appearance.Options.UseFont = true;
      this.btAdd.Name = "btAdd";
      this.btAdd.Click += new EventHandler(this.btAdd_Click);
      componentResourceManager.ApplyResources((object) this.pbImgResult, "pbImgResult");
      this.pbImgResult.Name = "pbImgResult";
      this.pbImgResult.Properties.AccessibleDescription = componentResourceManager.GetString("pbImgResult.Properties.AccessibleDescription");
      this.pbImgResult.Properties.AccessibleName = componentResourceManager.GetString("pbImgResult.Properties.AccessibleName");
      this.pbImgResult.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this.pbImgSource, "pbImgSource");
      this.pbImgSource.Name = "pbImgSource";
      this.pbImgSource.Properties.AccessibleDescription = componentResourceManager.GetString("pbImgSource.Properties.AccessibleDescription");
      this.pbImgSource.Properties.AccessibleName = componentResourceManager.GetString("pbImgSource.Properties.AccessibleName");
      this.pbImgSource.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.groupControl1);
      this.Controls.Add((Control) this.treeList1);
      this.Name = "FrmResults";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.frmResults_FormClosing);
      this.Load += new EventHandler(this.frmResults_Load);
      this.chbHide.Properties.EndInit();
      this.treeList1.EndInit();
      this.repositoryItemPictureEdit1.EndInit();
      this.repositoryItemDateEdit1.CalendarTimeProperties.EndInit();
      this.repositoryItemDateEdit1.EndInit();
      this.repositoryItemImageEdit1.EndInit();
      this.spinEdit1.Properties.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupControl1.EndInit();
      this.groupControl1.ResumeLayout(false);
      this.groupControl1.PerformLayout();
      this.pbImgResult.Properties.EndInit();
      this.pbImgSource.Properties.EndInit();
      this.ResumeLayout(false);
    }
  }
}
