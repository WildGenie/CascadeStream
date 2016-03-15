// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmLogSearch
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
using System.ServiceModel;
using System.Windows.Forms;
using BasicComponents;
using BasicComponents.IdentificationServer;
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
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = TS.Sdk.StaticFace.Model.Image;

namespace CascadeManager
{
  public class FrmLogSearch : XtraForm
  {
    public List<BcDevices> Devices = new List<BcDevices>();
    private List<BcObjects> _objects = new List<BcObjects>();
    private List<BcLogBases> _bases = new List<BcLogBases>();
    public DsResult Results = new DsResult();
    private ClientCallback _client = new ClientCallback();
    private Timer _maintimer = new Timer();
    private BcKey _currentKey = new BcKey();
    private DataTable _mainTable = new DataTable();
    private DateTime _startDate = DateTime.Now;
    private IContainer components = null;
    public bool BreakFlag;
    private GroupBox groupBox1;
    private SimpleButton btSearch;
    private SimpleButton btLoadPicture;
    private PictureEdit pictureEdit1;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colFaceID;
    private GridColumn colName;
    private GridColumn colPosition;
    private GridColumn colPicture;
    private GridColumn colScore;
    private GridColumn colBirthday;
    private GridColumn colCategory;
    private Label lbInfo;
    private SimpleButton btPrint;
    private LabelControl labelControl2;
    private CheckedListBoxControl lvObjects;
    private LabelControl lbObjects;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private RepositoryItemDateEdit repositoryItemDateEdit1;
    private MarqueeProgressBarControl marqueeProgressBarControl1;
    private LabelControl labelControl3;
    private LabelControl labelControl1;
    private SpinEdit spinEdit1;
    private LabelControl lbDate;
    private TextEdit tbScore;
    private GroupBox gbDate;
    private LabelControl labelControl5;
    private LabelControl labelControl4;
    private DateEdit dtBefore;
    private DateEdit dtFrom;
    private SimpleButton btClear;

    public FrmLogSearch()
    {
      InitializeComponent();
      ReloadObjects();
    }

    private BcObjects GetObjectById(int id)
    {
      BcObjects bcObjects1 = new BcObjects();
      foreach (BcObjects bcObjects2 in _objects)
      {
        if (id == bcObjects2.Id)
          return bcObjects2;
      }
      return bcObjects1;
    }

    public void ReloadObjects()
    {
      try
      {
        Results.dtImageType.Rows.Clear();
        Results.dtCategories.Rows.Clear();
        Results.dtDevices.Rows.Clear();
        Devices = BcDevicesStorageExtensions.LoadAll();
        _objects = BcObjects.LoadAll();
        _bases = BcLogBases.LoadAll();
        lvObjects.Items.Clear();
        foreach (BcDevices bcDevices in Devices)
        {
          lvObjects.Items.Add("");
          lvObjects.Items[lvObjects.Items.Count - 1].Value = bcDevices.Id;
          lvObjects.Items[lvObjects.Items.Count - 1].Description = bcDevices.Name + "-";
          foreach (BcObjects bcObjects in _objects)
          {
            if (bcObjects.Id == bcDevices.ObjectId)
            {
              bcObjects.GetData();
              if (bcDevices.TableId != Guid.Empty)
              {
                CheckedListBoxItem checkedListBoxItem = lvObjects.Items[lvObjects.Items.Count - 1];
                string str1 = checkedListBoxItem.Description + "(" + bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, bcDevices.TableId).Name + ")";
                checkedListBoxItem.Description = str1;
                string str2 = bcObjects.Name + "-" + BcObjectsData.GetObjectById(bcObjects.Data, bcDevices.TableId).Name;
                Results.dtDevices.Rows.Add((object) bcDevices.Id, (object) bcDevices.Name, (object) bcDevices.ObjectId, (object) bcDevices.TableId, (object) str2);
                break;
              }
              lvObjects.Items[lvObjects.Items.Count - 1].Description += bcObjects.Name;
              string str = "(" + bcObjects.Name + ")";
              Results.dtDevices.Rows.Add((object) bcDevices.Id, (object) bcDevices.Name, (object) bcDevices.ObjectId, (object) bcDevices.TableId, (object) str);
              break;
            }
          }
        }
      }
      catch
      {
      }
    }

    private void btLoadPicture_Click(object sender, EventArgs e)
    {
      ControlBox = false;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "Images|*.jpg;*.bmp;*.png;*.jpeg";
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        Bitmap source = new Bitmap(openFileDialog2.FileName);
        pictureEdit1.Image = source;
        Image image = source.ConvertFrom();
        FaceInfo face = MainForm.Engine.DetectMaxFace(image, null);
        Cursor = Cursors.Default;
        _currentKey = null;
        if (face != null)
        {
          _currentKey = new BcKey
          {
            ImageKey = MainForm.Engine.ExtractTemplate(image, face)
          };
        }
        else
        {
          int num = (int) XtraMessageBox.Show(Messages.NoFaceWasFound, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          _currentKey = null;
        }
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void simpleButton2_Click(object sender, EventArgs e)
    {
      if (pictureEdit1.Image == null)
        return;
      if (MainForm.Engine.DetectAllFaces(((Bitmap) pictureEdit1.Image).ConvertFrom(), null).Length > 0)
      {
        foreach (BcIdentificationServer identificationServer in BcIdentificationServer.LoadAll())
        {
          try
          {
            IdentificationServerClient identificationServerClient = new IdentificationServerClient(new InstanceContext(_client));
            identificationServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) identificationServer.Ip + ":" + (string) (object) identificationServer.Port + "/FaceIdentification/IdentificationServer");
            identificationServerClient.Open();
            List<Guid> list = new List<Guid>();
            foreach (CheckedListBoxItem checkedListBoxItem in lvObjects.Items)
            {
              if (checkedListBoxItem.CheckState == CheckState.Checked)
                list.Add((Guid) checkedListBoxItem.Value);
            }
            _startDate = DateTime.Now;
            marqueeProgressBarControl1.Visible = true;
            _maintimer.Enabled = true;
          }
          catch (Exception ex)
          {
            int num = (int) XtraMessageBox.Show(ex.Message, Messages.IdentificationServerUnavailble, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      else
      {
        int num1 = (int) XtraMessageBox.Show(Messages.NoFaceWasFound, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    public void WorkerThread(DataTable dt)
    {
      foreach (DataRow dataRow in (InternalDataCollectionBase) dt.Rows)
      {
        try
        {
          SqlCommand sqlCommand = new SqlCommand("Select Log.ID as ID,\r\nCascadeStream.dbo.Faces.Surname +' '\r\n+CascadeStream.dbo.Faces.FirstName+' '+\r\nCascadeStream.dbo.Faces.LastName as Name,Log.Date,ObjectID,TableID,isnull(\r\ncase when Category like '' then '" + (object) Messages.NonCategory + "' else Category\r\nend, '" + Messages.NonCategory + "') as 'Category',ImageKey,Log.ImageIcon\r\nfrom\r\nLog left outer join CascadeStream.dbo.Faces\r\non \r\nCascadeStream.dbo.Faces.ID = Log.FaceID WHERE Log.ID = " + (string) dataRow["ID"], new SqlConnection(CommonSettings.ConnectionStringLog));
          sqlCommand.Connection.Open();
          SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
          while (sqlDataReader.Read())
          {
            try
            {
              BcObjects objectById1 = GetObjectById((int) sqlDataReader["ObjectID"]);
              string str = objectById1.Name;
              if ((Guid) sqlDataReader["TableID"] != Guid.Empty)
              {
                BcObjectsData objectById2 = BcObjectsData.GetObjectById(objectById1.Data, (Guid) sqlDataReader["TableID"]);
                str = str + "-" + objectById2.Name;
              }
              try
              {
                Invoke(new AddNewRowFunc(AddNewRow), (object) new object[7]
                {
                  (object) Convert.ToInt32(sqlDataReader["ID"]),
                  sqlDataReader["ImageIcon"],
                  (object) sqlDataReader["Name"].ToString(),
                  (object) (float) dataRow["Score"],
                  (object) (DateTime) sqlDataReader["Date"],
                  (object) sqlDataReader["Category"].ToString(),
                  (object) str
                }, (object) 0);
              }
              catch
              {
              }
            }
            catch
            {
            }
          }
          sqlCommand.Connection.Close();
        }
        catch
        {
        }
      }
      Invoke(new AddNewRowFunc(AddNewRow), null, (object) 1);
    }

    public void EndWorkerThread(IAsyncResult res)
    {
    }

    private void client_GetResult(DataTable dt)
    {
      WorkerThreadFunc workerThreadFunc = WorkerThread;
      workerThreadFunc.BeginInvoke(dt, EndWorkerThread, workerThreadFunc);
    }

    private void AddNewRow(object[] ar, int action)
    {
      if (action == 0)
      {
        _mainTable.Rows.Add(ar);
        gridView1.FocusedRowHandle = 0;
      }
      else
      {
        marqueeProgressBarControl1.Visible = false;
        _maintimer.Enabled = true;
      }
    }

    private void frmDBSearch_Load(object sender, EventArgs e)
    {
      _client.GetResult += client_GetResult;
      _mainTable.Columns.AddRange(new DataColumn[7]
      {
        new DataColumn("LogID", typeof (int)),
        new DataColumn("Image", typeof (Bitmap)),
        new DataColumn("Name", typeof (string)),
        new DataColumn("Score", typeof (Decimal)),
        new DataColumn("Date", typeof (DateTime)),
        new DataColumn("Category", typeof (string)),
        new DataColumn("Comment", typeof (string))
      });
      gridControl1.DataSource = _mainTable;
      _maintimer.Tick += maintimer_Tick;
      _maintimer.Interval = 1000;
      gridView1.DoubleClick += gridView1_DoubleClick;
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
      if (gridView1.FocusedRowHandle < 0)
        return;
      DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
      FrmCompareImages frmCompareImages = new FrmCompareImages();
      frmCompareImages.StartPosition = FormStartPosition.CenterScreen;
      try
      {
        Bitmap bitmap = new Bitmap(new MemoryStream(BcLog.LoadById((int) dataRow["LogID"]).Image));
        frmCompareImages.pbMainImage.Image = pictureEdit1.Image;
        frmCompareImages.pbCamImage.Image = bitmap;
        int num = (int) frmCompareImages.ShowDialog();
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void maintimer_Tick(object sender, EventArgs e)
    {
      marqueeProgressBarControl1.Properties.ShowTitle = true;
      TimeSpan timeSpan = DateTime.Now - _startDate;
      marqueeProgressBarControl1.Text = (string) (object) timeSpan.Minutes + (object) ":" + (string) (object) timeSpan.Seconds;
    }

    private void frmDBSearch_Resize(object sender, EventArgs e)
    {
      ControlBox = false;
    }

    private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
    {
      if (!e.Info.IsRowIndicator || e.RowHandle < 0)
        return;
      e.Info.DisplayText = (e.RowHandle + 1).ToString();
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

    private void groupBox3_Enter(object sender, EventArgs e)
    {
    }

    private void tbScore_Validating(object sender, CancelEventArgs e)
    {
      try
      {
        double num = Convert.ToSingle(tbScore.Text.Replace(".", ","));
      }
      catch
      {
        e.Cancel = true;
        int num = (int) XtraMessageBox.Show(Messages.IncorrectInputFormat, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void tbScore_TextChanged(object sender, EventArgs e)
    {
      tbScore.Text = tbScore.Text.Replace(".", ",");
    }

    private void pbImgResult_Click(object sender, EventArgs e)
    {
    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {
    }

    private void frmLogSearch_FormClosing(object sender, FormClosingEventArgs e)
    {
      BreakFlag = true;
    }

    private void btClear_Click(object sender, EventArgs e)
    {
      _mainTable.Rows.Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmLogSearch));
      repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      repositoryItemDateEdit1 = new RepositoryItemDateEdit();
      groupBox1 = new GroupBox();
      labelControl3 = new LabelControl();
      labelControl1 = new LabelControl();
      spinEdit1 = new SpinEdit();
      lvObjects = new CheckedListBoxControl();
      lbObjects = new LabelControl();
      lbDate = new LabelControl();
      tbScore = new TextEdit();
      lbInfo = new Label();
      btSearch = new SimpleButton();
      btLoadPicture = new SimpleButton();
      pictureEdit1 = new PictureEdit();
      gbDate = new GroupBox();
      labelControl5 = new LabelControl();
      labelControl4 = new LabelControl();
      dtBefore = new DateEdit();
      dtFrom = new DateEdit();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colFaceID = new GridColumn();
      colPicture = new GridColumn();
      colName = new GridColumn();
      colScore = new GridColumn();
      colBirthday = new GridColumn();
      colCategory = new GridColumn();
      colPosition = new GridColumn();
      btPrint = new SimpleButton();
      labelControl2 = new LabelControl();
      marqueeProgressBarControl1 = new MarqueeProgressBarControl();
      btClear = new SimpleButton();
      repositoryItemPictureEdit1.BeginInit();
      repositoryItemDateEdit1.BeginInit();
      repositoryItemDateEdit1.CalendarTimeProperties.BeginInit();
      groupBox1.SuspendLayout();
      spinEdit1.Properties.BeginInit();
      lvObjects.BeginInit();
      tbScore.Properties.BeginInit();
      pictureEdit1.Properties.BeginInit();
      gbDate.SuspendLayout();
      dtBefore.Properties.CalendarTimeProperties.BeginInit();
      dtBefore.Properties.BeginInit();
      dtFrom.Properties.CalendarTimeProperties.BeginInit();
      dtFrom.Properties.BeginInit();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      marqueeProgressBarControl1.Properties.BeginInit();
      SuspendLayout();
      repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.ByteArray;
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
      groupBox1.Controls.Add(labelControl3);
      groupBox1.Controls.Add(labelControl1);
      groupBox1.Controls.Add(spinEdit1);
      groupBox1.Controls.Add(lvObjects);
      groupBox1.Controls.Add(lbObjects);
      groupBox1.Controls.Add(lbDate);
      groupBox1.Controls.Add(tbScore);
      groupBox1.Controls.Add(lbInfo);
      groupBox1.Controls.Add(btSearch);
      groupBox1.Controls.Add(btLoadPicture);
      groupBox1.Controls.Add(pictureEdit1);
      groupBox1.Controls.Add(gbDate);
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Name = "groupBox1";
      groupBox1.TabStop = false;
      groupBox1.Enter += groupBox1_Enter;
      labelControl3.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl3.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl3, "labelControl3");
      labelControl3.Name = "labelControl3";
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(spinEdit1, "spinEdit1");
      spinEdit1.Name = "spinEdit1";
      spinEdit1.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      componentResourceManager.ApplyResources(lvObjects, "lvObjects");
      lvObjects.Appearance.Font = (Font) componentResourceManager.GetObject("lvObjects.Appearance.Font");
      lvObjects.Appearance.Options.UseFont = true;
      lvObjects.Name = "lvObjects";
      lbObjects.Appearance.Font = (Font) componentResourceManager.GetObject("lbObjects.Appearance.Font");
      componentResourceManager.ApplyResources(lbObjects, "lbObjects");
      lbObjects.Name = "lbObjects";
      lbDate.Appearance.Font = (Font) componentResourceManager.GetObject("lbDate.Appearance.Font");
      componentResourceManager.ApplyResources(lbDate, "lbDate");
      lbDate.Name = "lbDate";
      componentResourceManager.ApplyResources(tbScore, "tbScore");
      tbScore.Name = "tbScore";
      tbScore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbScore.Properties.Appearance.Font");
      tbScore.Properties.Appearance.Options.UseFont = true;
      tbScore.TextChanged += tbScore_TextChanged;
      tbScore.Validating += tbScore_Validating;
      componentResourceManager.ApplyResources(lbInfo, "lbInfo");
      lbInfo.Name = "lbInfo";
      componentResourceManager.ApplyResources(btSearch, "btSearch");
      btSearch.Appearance.Font = (Font) componentResourceManager.GetObject("btSearch.Appearance.Font");
      btSearch.Appearance.Options.UseFont = true;
      btSearch.Name = "btSearch";
      btSearch.Click += simpleButton2_Click;
      btLoadPicture.Appearance.Font = (Font) componentResourceManager.GetObject("btLoadPicture.Appearance.Font");
      btLoadPicture.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btLoadPicture, "btLoadPicture");
      btLoadPicture.Name = "btLoadPicture";
      btLoadPicture.Click += btLoadPicture_Click;
      componentResourceManager.ApplyResources(pictureEdit1, "pictureEdit1");
      pictureEdit1.Name = "pictureEdit1";
      pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      gbDate.Controls.Add(labelControl5);
      gbDate.Controls.Add(labelControl4);
      gbDate.Controls.Add(dtBefore);
      gbDate.Controls.Add(dtFrom);
      componentResourceManager.ApplyResources(gbDate, "gbDate");
      gbDate.Name = "gbDate";
      gbDate.TabStop = false;
      gbDate.Enter += groupBox3_Enter;
      labelControl5.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl5.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl5, "labelControl5");
      labelControl5.Name = "labelControl5";
      labelControl4.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl4.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl4, "labelControl4");
      labelControl4.Name = "labelControl4";
      componentResourceManager.ApplyResources(dtBefore, "dtBefore");
      dtBefore.Name = "dtBefore";
      dtBefore.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtBefore.Properties.Buttons"))
      });
      dtBefore.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtBefore.Properties.DisplayFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtBefore.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      dtBefore.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtBefore.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtBefore.Properties.Mask.EditMask = componentResourceManager.GetString("dtBefore.Properties.Mask.EditMask");
      componentResourceManager.ApplyResources(dtFrom, "dtFrom");
      dtFrom.Name = "dtFrom";
      dtFrom.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtFrom.Properties.Buttons"))
      });
      dtFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtFrom.Properties.DisplayFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtFrom.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      dtFrom.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtFrom.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtFrom.Properties.Mask.EditMask = componentResourceManager.GetString("dtFrom.Properties.Mask.EditMask");
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridView1.ColumnPanelRowHeight = 50;
      gridView1.Columns.AddRange(new GridColumn[7]
      {
        colFaceID,
        colPicture,
        colName,
        colScore,
        colBirthday,
        colCategory,
        colPosition
      });
      gridView1.GridControl = gridControl1;
      gridView1.IndicatorWidth = 60;
      gridView1.Name = "gridView1";
      gridView1.OptionsBehavior.Editable = false;
      gridView1.OptionsCustomization.AllowFilter = false;
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.FindMode = FindMode.Always;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.RowAutoHeight = true;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.SortInfo.AddRange(new GridColumnSortInfo[1]
      {
        new GridColumnSortInfo(colScore, ColumnSortOrder.Descending)
      });
      gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
      colFaceID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colFaceID.AppearanceCell.Font");
      colFaceID.AppearanceCell.Options.UseFont = true;
      colFaceID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colFaceID.AppearanceHeader.Font");
      colFaceID.AppearanceHeader.Options.UseFont = true;
      colFaceID.AppearanceHeader.Options.UseTextOptions = true;
      colFaceID.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colFaceID, "colFaceID");
      colFaceID.FieldName = "FaceID";
      colFaceID.Name = "colFaceID";
      colFaceID.OptionsColumn.AllowEdit = false;
      colFaceID.OptionsColumn.ReadOnly = true;
      colPicture.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPicture.AppearanceCell.Font");
      colPicture.AppearanceCell.Options.UseFont = true;
      colPicture.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPicture.AppearanceHeader.Font");
      colPicture.AppearanceHeader.Options.UseFont = true;
      colPicture.AppearanceHeader.Options.UseTextOptions = true;
      colPicture.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colPicture, "colPicture");
      colPicture.ColumnEdit = repositoryItemPictureEdit1;
      colPicture.FieldName = "Image";
      colPicture.Name = "colPicture";
      colPicture.OptionsColumn.AllowEdit = false;
      colPicture.OptionsColumn.AllowSort = DefaultBoolean.False;
      colPicture.OptionsColumn.ReadOnly = true;
      colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      colName.AppearanceCell.Options.UseFont = true;
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
      colScore.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colScore.AppearanceCell.Font");
      colScore.AppearanceCell.Options.UseFont = true;
      colScore.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colScore.AppearanceHeader.Font");
      colScore.AppearanceHeader.Options.UseFont = true;
      colScore.AppearanceHeader.Options.UseTextOptions = true;
      colScore.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colScore.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colScore, "colScore");
      colScore.FieldName = "Score";
      colScore.Name = "colScore";
      colScore.OptionsColumn.AllowEdit = false;
      colScore.OptionsColumn.FixedWidth = true;
      colScore.OptionsColumn.ReadOnly = true;
      colBirthday.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colBirthday.AppearanceCell.Font");
      colBirthday.AppearanceCell.Options.UseFont = true;
      colBirthday.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colBirthday.AppearanceHeader.Font");
      colBirthday.AppearanceHeader.Options.UseFont = true;
      colBirthday.AppearanceHeader.Options.UseTextOptions = true;
      colBirthday.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colBirthday.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colBirthday, "colBirthday");
      colBirthday.ColumnEdit = repositoryItemDateEdit1;
      colBirthday.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      colBirthday.DisplayFormat.FormatType = FormatType.DateTime;
      colBirthday.FieldName = "Date";
      colBirthday.Name = "colBirthday";
      colBirthday.OptionsColumn.AllowEdit = false;
      colBirthday.OptionsColumn.ReadOnly = true;
      colCategory.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colCategory.AppearanceCell.Font");
      colCategory.AppearanceCell.Options.UseFont = true;
      colCategory.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colCategory.AppearanceHeader.Font");
      colCategory.AppearanceHeader.Options.UseFont = true;
      colCategory.AppearanceHeader.Options.UseTextOptions = true;
      colCategory.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      componentResourceManager.ApplyResources(colCategory, "colCategory");
      colCategory.FieldName = "Category";
      colCategory.Name = "colCategory";
      colCategory.OptionsColumn.ReadOnly = true;
      colPosition.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceCell.Font");
      colPosition.AppearanceCell.Options.UseFont = true;
      colPosition.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      colPosition.AppearanceHeader.Options.UseFont = true;
      colPosition.AppearanceHeader.Options.UseTextOptions = true;
      colPosition.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
      colPosition.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
      componentResourceManager.ApplyResources(colPosition, "colPosition");
      colPosition.FieldName = "Comment";
      colPosition.Name = "colPosition";
      colPosition.OptionsColumn.AllowEdit = false;
      colPosition.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(btPrint, "btPrint");
      btPrint.Appearance.Font = (Font) componentResourceManager.GetObject("btPrint.Appearance.Font");
      btPrint.Appearance.Options.UseFont = true;
      btPrint.Name = "btPrint";
      btPrint.Click += btPrint_Click;
      labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      componentResourceManager.ApplyResources(labelControl2, "labelControl2");
      labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources(marqueeProgressBarControl1, "marqueeProgressBarControl1");
      marqueeProgressBarControl1.Name = "marqueeProgressBarControl1";
      componentResourceManager.ApplyResources(btClear, "btClear");
      btClear.Appearance.Font = (Font) componentResourceManager.GetObject("btClear.Appearance.Font");
      btClear.Appearance.Options.UseFont = true;
      btClear.Name = "btClear";
      btClear.Click += btClear_Click;
      Appearance.Options.UseFont = true;
      AutoScaleMode = AutoScaleMode.None;
      componentResourceManager.ApplyResources(this, "$this");
      Controls.Add(btClear);
      Controls.Add(labelControl2);
      Controls.Add(marqueeProgressBarControl1);
      Controls.Add(groupBox1);
      Controls.Add(btPrint);
      Controls.Add(gridControl1);
      FormBorderStyle = FormBorderStyle.None;
      Name = "FrmLogSearch";
      ShowIcon = false;
      WindowState = FormWindowState.Maximized;
      FormClosing += frmLogSearch_FormClosing;
      Load += frmDBSearch_Load;
      Resize += frmDBSearch_Resize;
      repositoryItemPictureEdit1.EndInit();
      repositoryItemDateEdit1.CalendarTimeProperties.EndInit();
      repositoryItemDateEdit1.EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      spinEdit1.Properties.EndInit();
      lvObjects.EndInit();
      tbScore.Properties.EndInit();
      pictureEdit1.Properties.EndInit();
      gbDate.ResumeLayout(false);
      gbDate.PerformLayout();
      dtBefore.Properties.CalendarTimeProperties.EndInit();
      dtBefore.Properties.EndInit();
      dtFrom.Properties.CalendarTimeProperties.EndInit();
      dtFrom.Properties.EndInit();
      gridControl1.EndInit();
      gridView1.EndInit();
      marqueeProgressBarControl1.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    public delegate void GetResultEventHandler(DataTable dt);

    public class ClientCallback : IIdentificationServerCallback
    {
      public event GetResultEventHandler GetResult;

      public void CheckClient()
      {
      }

      public void SendStaticResults(Guid searchId, DataTable dt)
      {
      }

      public void SendResults(DataTable dt)
      {
        if (GetResult == null)
          return;
        GetResult(dt);
      }
    }

    public delegate void WorkerThreadFunc(DataTable dt);

    private delegate void AddNewRowFunc(object[] ar, int action);

    public delegate void ShowServerStateFunc(bool val);
  }
}
