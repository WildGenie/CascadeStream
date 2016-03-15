// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmCompareImages
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using BasicComponents;
using CS.DAL;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Events;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace CascadeFlowClient
{
  public class FrmCompareImages : XtraForm
  {
    public BcFace CurrentEmployer = new BcFace();
    public BcLog CurrentLog = new BcLog();
    public List<BcKey> Keys = new List<BcKey>();
    public List<BcKey> CurrentKeys = new List<BcKey>();
    public List<BcKeySettings> KeySettings = new List<BcKeySettings>();
    private bool _flagAfterAdd = false;
    private List<BcDepartment> _deps = new List<BcDepartment>();
    private List<BcOrganization> _orgs = new List<BcOrganization>();
    private List<BcTiming> _timinList = new List<BcTiming>();
    private DataTable _dtImages = new DataTable();
    private List<BcAccessCategory> _templateList = new List<BcAccessCategory>();
    private List<Guid> _deletedImages = new List<Guid>();
    private IContainer components = (IContainer) null;
    private bool _cancelFlag;
    public bool AddNewValue;
    private bool _isloading;
    private bool _changedValues;
    private GroupControl groupBox1;
    private LabelControl lbSurname;
    private LabelControl lbBirthday;
    private LabelControl label3;
    private LabelControl lbComment;
    private PageSetupDialog pageSetupDialog1;
    private GridControl gcImagesFullFace;
    private LayoutView lvImagesFullFace;
    private LayoutViewColumn colImage;
    private LayoutViewColumn colName;
    private LayoutViewColumn colImageComment;
    private LayoutViewColumn colImageID;
    private GridView gridView1;
    private RepositoryItemButtonEdit repositoryItemButtonEdit1;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private MemoEdit tbComment;
    private DateEdit dtpBithday;
    private TextEdit tbPassport;
    private LabelControl lbPassport;
    private ComboBoxEdit cbAccessTemplate;
    private LabelControl lbSex;
    private ComboBoxEdit cbSEX;
    private TextEdit tbLastName;
    private LabelControl lbLastName;
    private TextEdit tbFirstName;
    private LabelControl lbFirstName;
    private TextEdit tbSurname;
    private PictureEdit pictureEdit1;
    private GroupControl groupControl1;
    private MemoEdit tbAddress;
    private LabelControl labelControl1;
    private LayoutViewField layoutViewField_layoutViewColumn1;
    private LayoutViewField layoutViewField_layoutViewColumn2_1;
    private LayoutViewField layoutViewField_layoutViewColumn2;
    private LayoutViewField layoutViewField_layoutViewColumn1_1;
    private LayoutViewCard layoutViewCard1;
    private TextEdit tbCam;
    private LabelControl labelControl2;
    private LabelControl labelControl3;
    private DateEdit dtDate;

    public FrmCompareImages()
    {
      this.InitializeComponent();
    }

    private void LoadImages()
    {
      this._dtImages = new DataTable();
      this._dtImages.Columns.Add("ID", typeof (Guid));
      this._dtImages.Columns.Add("ImageIcon", typeof (Bitmap));
      this._dtImages.Columns.Add("Image", typeof (Bitmap));
      this._dtImages.Columns.Add("Name", typeof (string));
      this._dtImages.Columns.Add("Comment", typeof (string));
      this._dtImages.Columns.Add("IsMain", typeof (bool));
      this._dtImages.Columns.Add("Changed", typeof (bool));
      this._dtImages.Columns.Add("ImageChanged", typeof (bool));
      if (!(this.CurrentEmployer.Id != Guid.Empty))
        return;
      foreach (BcImage bcImage in BcImage.LoadByFaceId(this.CurrentEmployer.Id))
      {
        Bitmap bitmap1 = new Bitmap((Stream) new MemoryStream(bcImage.ImageIcon));
        Bitmap bitmap2 = new Bitmap((Stream) new MemoryStream(bcImage.Image));
        this._dtImages.Rows.Add((object) bcImage.Id, (object) bitmap1, (object) bitmap2, (object) bcImage.Name, (object) bcImage.Comment, (object) (bool) (bcImage.IsMain ? 1 : 0), (object) false, (object) false);
      }
    }

    private int IndexOfDep(Guid id)
    {
      int num = -1;
      foreach (BcDepartment bcDepartment in this._deps)
      {
        ++num;
        if (bcDepartment.Id == id)
          break;
      }
      return num;
    }

    private int IndexOfOrg(Guid id)
    {
      int num = -1;
      foreach (BcOrganization bcOrganization in this._orgs)
      {
        ++num;
        if (bcOrganization.Id == id)
          break;
      }
      return num;
    }

    private int IndexOfTiming(Guid id)
    {
      int num = -1;
      foreach (BcTiming bcTiming in this._timinList)
      {
        ++num;
        if (bcTiming.Id == id)
          break;
      }
      return num;
    }

    private int IndexOfAccessTemplate(int id)
    {
      int num = -1;
      foreach (BcAccessCategory bcAccessCategory in this._templateList)
      {
        ++num;
        if (bcAccessCategory.Id == id)
          break;
      }
      return num;
    }

    private void FindChanges()
    {
      if (this._changedValues)
      {
        if (XtraMessageBox.Show(Messages.RecordHasBeenChanged, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.btSave_Click(new object(), new EventArgs());
        this._changedValues = false;
      }
      else
        this._cancelFlag = false;
    }

    private void btSave_Click(object sender, EventArgs e)
    {
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.FindChanges();
      this.Close();
    }

    private void EditEmployerForm_Load(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Normal;
      this.StartPosition = FormStartPosition.CenterParent;
      this.dtDate.DateTime = this.CurrentLog.Date;
      this.tbCam.Text = BcDevicesStorageExtensions.LoadById(this.CurrentLog.DeviceId).Name;
      this.dtpBithday.DateTime = DateTime.Now;
      try
      {
        this.pictureEdit1.Image = (Image) new Bitmap((Stream) new MemoryStream(this.CurrentLog.Image));
      }
      catch
      {
      }
      this.cbSEX.SelectedIndex = 0;
      this._isloading = true;
      this._changedValues = false;
      SqlCommand sqlCommand = new SqlCommand(" \r\nSelect \r\ndistinct\r\nCountry, \r\nRegion ,\r\nCity, \r\nDistrict ,\r\nStreet\r\nFrom Faces\r\norder by\r\nCountry,\r\nRegion,\r\nCity,\r\nDistrict,\r\nStreet\r\n", new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      sqlCommand.ExecuteReader();
      sqlCommand.Connection.Close();
      this._deps = BcDepartment.LoadAll();
      this._timinList = BcTiming.LoadAll();
      this._templateList = BcAccessCategory.LoadAll();
      foreach (BcAccessCategory bcAccessCategory in this._templateList)
        this.cbAccessTemplate.Properties.Items.Add((object) bcAccessCategory.Name);
      this.LoadImages();
      this.gcImagesFullFace.DataSource = (object) this._dtImages;
      if (this.CurrentEmployer.Id != Guid.Empty)
      {
        this.Keys = new List<BcKey>();
        this.CurrentKeys = BcKey.LoadKyesByFaceId(this.CurrentEmployer.Id);
        this.tbAddress.Text = BcFace.FullAddress(this.CurrentEmployer);
        this.dtpBithday.DateTime = this.CurrentEmployer.Birthday;
        this.tbFirstName.Text = this.CurrentEmployer.FirstName;
        this.tbSurname.Text = this.CurrentEmployer.Surname;
        this.tbLastName.Text = this.CurrentEmployer.LastName;
        this.tbPassport.Text = this.CurrentEmployer.Passport;
        this.tbComment.Text = this.CurrentEmployer.Comment;
        this.cbSEX.SelectedIndex = 0;
        if (this.CurrentEmployer.Sex == 1)
          this.cbSEX.SelectedIndex = 1;
        this.cbAccessTemplate.SelectedIndex = this.IndexOfAccessTemplate(this.CurrentEmployer.AccessId);
      }
      this._isloading = false;
    }

    private void EditEmployerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
        this._cancelFlag = false;
      if (this._cancelFlag)
        e.Cancel = true;
      else if (this._flagAfterAdd)
      {
        if (XtraMessageBox.Show(Messages.NewRecordWasCreated, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.AddNewValue = true;
        else
          this.AddNewValue = false;
      }
      else
        this.FindChanges();
    }

    private void TbValueChanged(object sender, EventArgs e)
    {
      if (this._isloading)
        return;
      this._changedValues = true;
    }

    private void chbHolidayState_CheckedChanged(object sender, EventArgs e)
    {
      if (this._isloading)
        return;
      this._changedValues = true;
    }

    private void EditEmployerForm_HelpButtonClicked(object sender, CancelEventArgs e)
    {
      Help.ShowHelp((Control) this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/14.htm");
    }

    private void EditEmployerForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      Help.ShowHelp((Control) this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/14.htm");
    }

    private void tbPassport_EditValueChanged(object sender, EventArgs e)
    {
    }

    private void btFile_Click(object sender, EventArgs e)
    {
    }

    private void btPlayer_Click(object sender, EventArgs e)
    {
    }

    private void btCamera_Click(object sender, EventArgs e)
    {
    }

    private void btEditPicture_Click(object sender, EventArgs e)
    {
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (this.lvImagesFullFace.FocusedRowHandle < 0)
        return;
      DataRow dataRow = this.lvImagesFullFace.GetDataRow(this.lvImagesFullFace.FocusedRowHandle);
      this._deletedImages.Add((Guid) dataRow["ID"]);
      this._dtImages.Rows.Remove(dataRow);
    }

    private void lvImagesFullFace_CustomDrawCardCaption(object sender, LayoutViewCustomDrawCardCaptionEventArgs e)
    {
      if (e.RowHandle < 0)
        return;
      DataRow dataRow = this.lvImagesFullFace.GetDataRow(e.RowHandle);
      e.CardCaption = dataRow["Name"].ToString();
    }

    private void lvImagesFullFace_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCompareImages));
      GridLevelNode gridLevelNode = new GridLevelNode();
      SerializableAppearanceObject appearanceObject = new SerializableAppearanceObject();
      this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      this.groupBox1 = new GroupControl();
      this.tbAddress = new MemoEdit();
      this.labelControl1 = new LabelControl();
      this.tbComment = new MemoEdit();
      this.dtpBithday = new DateEdit();
      this.tbPassport = new TextEdit();
      this.lbBirthday = new LabelControl();
      this.lbPassport = new LabelControl();
      this.label3 = new LabelControl();
      this.cbAccessTemplate = new ComboBoxEdit();
      this.lbComment = new LabelControl();
      this.lbSex = new LabelControl();
      this.cbSEX = new ComboBoxEdit();
      this.tbLastName = new TextEdit();
      this.lbLastName = new LabelControl();
      this.tbFirstName = new TextEdit();
      this.lbFirstName = new LabelControl();
      this.tbSurname = new TextEdit();
      this.lbSurname = new LabelControl();
      this.pageSetupDialog1 = new PageSetupDialog();
      this.gcImagesFullFace = new GridControl();
      this.lvImagesFullFace = new LayoutView();
      this.colImage = new LayoutViewColumn();
      this.layoutViewField_layoutViewColumn1 = new LayoutViewField();
      this.colName = new LayoutViewColumn();
      this.layoutViewField_layoutViewColumn2_1 = new LayoutViewField();
      this.colImageComment = new LayoutViewColumn();
      this.layoutViewField_layoutViewColumn2 = new LayoutViewField();
      this.colImageID = new LayoutViewColumn();
      this.layoutViewField_layoutViewColumn1_1 = new LayoutViewField();
      this.layoutViewCard1 = new LayoutViewCard();
      this.repositoryItemButtonEdit1 = new RepositoryItemButtonEdit();
      this.gridView1 = new GridView();
      this.pictureEdit1 = new PictureEdit();
      this.groupControl1 = new GroupControl();
      this.dtDate = new DateEdit();
      this.tbCam = new TextEdit();
      this.labelControl2 = new LabelControl();
      this.labelControl3 = new LabelControl();
      this.repositoryItemPictureEdit1.BeginInit();
      this.repositoryItemCheckEdit1.BeginInit();
      this.groupBox1.BeginInit();
      this.groupBox1.SuspendLayout();
      this.tbAddress.Properties.BeginInit();
      this.tbComment.Properties.BeginInit();
      this.dtpBithday.Properties.CalendarTimeProperties.BeginInit();
      this.dtpBithday.Properties.BeginInit();
      this.tbPassport.Properties.BeginInit();
      this.cbAccessTemplate.Properties.BeginInit();
      this.cbSEX.Properties.BeginInit();
      this.tbLastName.Properties.BeginInit();
      this.tbFirstName.Properties.BeginInit();
      this.tbSurname.Properties.BeginInit();
      this.gcImagesFullFace.BeginInit();
      this.lvImagesFullFace.BeginInit();
      this.layoutViewField_layoutViewColumn1.BeginInit();
      this.layoutViewField_layoutViewColumn2_1.BeginInit();
      this.layoutViewField_layoutViewColumn2.BeginInit();
      this.layoutViewField_layoutViewColumn1_1.BeginInit();
      this.layoutViewCard1.BeginInit();
      this.repositoryItemButtonEdit1.BeginInit();
      this.gridView1.BeginInit();
      this.pictureEdit1.Properties.BeginInit();
      this.groupControl1.BeginInit();
      this.groupControl1.SuspendLayout();
      this.dtDate.Properties.CalendarTimeProperties.BeginInit();
      this.dtDate.Properties.BeginInit();
      this.tbCam.Properties.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
      this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      this.repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.Image;
      this.repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      this.repositoryItemCheckEdit1.AutoWidth = true;
      this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.tbAddress);
      this.groupBox1.Controls.Add((Control) this.labelControl1);
      this.groupBox1.Controls.Add((Control) this.tbComment);
      this.groupBox1.Controls.Add((Control) this.dtpBithday);
      this.groupBox1.Controls.Add((Control) this.tbPassport);
      this.groupBox1.Controls.Add((Control) this.lbBirthday);
      this.groupBox1.Controls.Add((Control) this.lbPassport);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.cbAccessTemplate);
      this.groupBox1.Controls.Add((Control) this.lbComment);
      this.groupBox1.Controls.Add((Control) this.lbSex);
      this.groupBox1.Controls.Add((Control) this.cbSEX);
      this.groupBox1.Controls.Add((Control) this.tbLastName);
      this.groupBox1.Controls.Add((Control) this.lbLastName);
      this.groupBox1.Controls.Add((Control) this.tbFirstName);
      this.groupBox1.Controls.Add((Control) this.lbFirstName);
      this.groupBox1.Controls.Add((Control) this.tbSurname);
      this.groupBox1.Controls.Add((Control) this.lbSurname);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.ShowCaption = false;
      componentResourceManager.ApplyResources((object) this.tbAddress, "tbAddress");
      this.tbAddress.Name = "tbAddress";
      this.tbAddress.Properties.AccessibleDescription = componentResourceManager.GetString("tbAddress.Properties.AccessibleDescription");
      this.tbAddress.Properties.AccessibleName = componentResourceManager.GetString("tbAddress.Properties.AccessibleName");
      this.tbAddress.Properties.NullValuePrompt = componentResourceManager.GetString("tbAddress.Properties.NullValuePrompt");
      this.tbAddress.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbAddress.Properties.NullValuePromptShowForEmptyValue");
      this.tbAddress.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.labelControl1, "labelControl1");
      this.labelControl1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.DisabledImage");
      this.labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      this.labelControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl1.Appearance.FontSizeDelta");
      this.labelControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl1.Appearance.FontStyleDelta");
      this.labelControl1.Appearance.ForeColor = (Color) componentResourceManager.GetObject("labelControl1.Appearance.ForeColor");
      this.labelControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl1.Appearance.GradientMode");
      this.labelControl1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.HoverImage");
      this.labelControl1.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl1.Appearance.Image");
      this.labelControl1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.PressedImage");
      this.labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources((object) this.tbComment, "tbComment");
      this.tbComment.Name = "tbComment";
      this.tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      this.tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      this.tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      this.tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      this.tbComment.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.dtpBithday, "dtpBithday");
      this.dtpBithday.Name = "dtpBithday";
      this.dtpBithday.Properties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.AccessibleDescription");
      this.dtpBithday.Properties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.AccessibleName");
      this.dtpBithday.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Font");
      this.dtpBithday.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontSizeDelta");
      this.dtpBithday.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontStyleDelta");
      this.dtpBithday.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.GradientMode");
      this.dtpBithday.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Image");
      this.dtpBithday.Properties.Appearance.Options.UseFont = true;
      this.dtpBithday.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBithday.Properties.AutoHeight");
      this.dtpBithday.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtpBithday.Properties.Buttons"))
      });
      this.dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription");
      this.dtpBithday.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleName");
      this.dtpBithday.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.AutoHeight");
      this.dtpBithday.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      this.dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      this.dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt");
      this.dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      this.dtpBithday.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBithday.Properties.Mask.AutoComplete");
      this.dtpBithday.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.BeepOnError");
      this.dtpBithday.Properties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.Mask.EditMask");
      this.dtpBithday.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.IgnoreMaskBlank");
      this.dtpBithday.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBithday.Properties.Mask.MaskType");
      this.dtpBithday.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBithday.Properties.Mask.PlaceHolder");
      this.dtpBithday.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.SaveLiteral");
      this.dtpBithday.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.ShowPlaceHolders");
      this.dtpBithday.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.UseMaskAsDisplayFormat");
      this.dtpBithday.Properties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.NullValuePrompt");
      this.dtpBithday.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBithday.Properties.NullValuePromptShowForEmptyValue");
      this.dtpBithday.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.tbPassport, "tbPassport");
      this.tbPassport.Name = "tbPassport";
      this.tbPassport.Properties.AccessibleDescription = componentResourceManager.GetString("tbPassport.Properties.AccessibleDescription");
      this.tbPassport.Properties.AccessibleName = componentResourceManager.GetString("tbPassport.Properties.AccessibleName");
      this.tbPassport.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbPassport.Properties.Appearance.Font");
      this.tbPassport.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontSizeDelta");
      this.tbPassport.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontStyleDelta");
      this.tbPassport.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbPassport.Properties.Appearance.GradientMode");
      this.tbPassport.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbPassport.Properties.Appearance.Image");
      this.tbPassport.Properties.Appearance.Options.UseFont = true;
      this.tbPassport.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbPassport.Properties.AutoHeight");
      this.tbPassport.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbPassport.Properties.Mask.AutoComplete");
      this.tbPassport.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.BeepOnError");
      this.tbPassport.Properties.Mask.EditMask = componentResourceManager.GetString("tbPassport.Properties.Mask.EditMask");
      this.tbPassport.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.IgnoreMaskBlank");
      this.tbPassport.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbPassport.Properties.Mask.MaskType");
      this.tbPassport.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbPassport.Properties.Mask.PlaceHolder");
      this.tbPassport.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.SaveLiteral");
      this.tbPassport.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.ShowPlaceHolders");
      this.tbPassport.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbPassport.Properties.NullValuePrompt = componentResourceManager.GetString("tbPassport.Properties.NullValuePrompt");
      this.tbPassport.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbPassport.Properties.NullValuePromptShowForEmptyValue");
      this.tbPassport.Properties.ReadOnly = true;
      this.tbPassport.EditValueChanged += new EventHandler(this.tbPassport_EditValueChanged);
      this.tbPassport.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbBirthday, "lbBirthday");
      this.lbBirthday.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.DisabledImage");
      this.lbBirthday.Appearance.Font = (Font) componentResourceManager.GetObject("lbBirthday.Appearance.Font");
      this.lbBirthday.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbBirthday.Appearance.FontSizeDelta");
      this.lbBirthday.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbBirthday.Appearance.FontStyleDelta");
      this.lbBirthday.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbBirthday.Appearance.ForeColor");
      this.lbBirthday.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbBirthday.Appearance.GradientMode");
      this.lbBirthday.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.HoverImage");
      this.lbBirthday.Appearance.Image = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.Image");
      this.lbBirthday.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.PressedImage");
      this.lbBirthday.Name = "lbBirthday";
      componentResourceManager.ApplyResources((object) this.lbPassport, "lbPassport");
      this.lbPassport.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.DisabledImage");
      this.lbPassport.Appearance.Font = (Font) componentResourceManager.GetObject("lbPassport.Appearance.Font");
      this.lbPassport.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbPassport.Appearance.FontSizeDelta");
      this.lbPassport.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbPassport.Appearance.FontStyleDelta");
      this.lbPassport.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbPassport.Appearance.ForeColor");
      this.lbPassport.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbPassport.Appearance.GradientMode");
      this.lbPassport.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.HoverImage");
      this.lbPassport.Appearance.Image = (Image) componentResourceManager.GetObject("lbPassport.Appearance.Image");
      this.lbPassport.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.PressedImage");
      this.lbPassport.Name = "lbPassport";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label3.Appearance.DisabledImage");
      this.label3.Appearance.Font = (Font) componentResourceManager.GetObject("label3.Appearance.Font");
      this.label3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label3.Appearance.FontSizeDelta");
      this.label3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label3.Appearance.FontStyleDelta");
      this.label3.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label3.Appearance.ForeColor");
      this.label3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label3.Appearance.GradientMode");
      this.label3.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label3.Appearance.HoverImage");
      this.label3.Appearance.Image = (Image) componentResourceManager.GetObject("label3.Appearance.Image");
      this.label3.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label3.Appearance.PressedImage");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.cbAccessTemplate, "cbAccessTemplate");
      this.cbAccessTemplate.Name = "cbAccessTemplate";
      this.cbAccessTemplate.Properties.AccessibleDescription = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleDescription");
      this.cbAccessTemplate.Properties.AccessibleName = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleName");
      this.cbAccessTemplate.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Font");
      this.cbAccessTemplate.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontSizeDelta");
      this.cbAccessTemplate.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontStyleDelta");
      this.cbAccessTemplate.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.GradientMode");
      this.cbAccessTemplate.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Image");
      this.cbAccessTemplate.Properties.Appearance.Options.UseFont = true;
      this.cbAccessTemplate.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.AutoHeight");
      this.cbAccessTemplate.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbAccessTemplate.Properties.Buttons"))
      });
      this.cbAccessTemplate.Properties.NullValuePrompt = componentResourceManager.GetString("cbAccessTemplate.Properties.NullValuePrompt");
      this.cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue");
      this.cbAccessTemplate.Properties.ReadOnly = true;
      this.cbAccessTemplate.SelectedIndexChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbComment, "lbComment");
      this.lbComment.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.DisabledImage");
      this.lbComment.Appearance.Font = (Font) componentResourceManager.GetObject("lbComment.Appearance.Font");
      this.lbComment.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbComment.Appearance.FontSizeDelta");
      this.lbComment.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbComment.Appearance.FontStyleDelta");
      this.lbComment.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbComment.Appearance.ForeColor");
      this.lbComment.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbComment.Appearance.GradientMode");
      this.lbComment.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.HoverImage");
      this.lbComment.Appearance.Image = (Image) componentResourceManager.GetObject("lbComment.Appearance.Image");
      this.lbComment.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.PressedImage");
      this.lbComment.Name = "lbComment";
      componentResourceManager.ApplyResources((object) this.lbSex, "lbSex");
      this.lbSex.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.DisabledImage");
      this.lbSex.Appearance.Font = (Font) componentResourceManager.GetObject("lbSex.Appearance.Font");
      this.lbSex.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSex.Appearance.FontSizeDelta");
      this.lbSex.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSex.Appearance.FontStyleDelta");
      this.lbSex.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSex.Appearance.ForeColor");
      this.lbSex.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSex.Appearance.GradientMode");
      this.lbSex.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.HoverImage");
      this.lbSex.Appearance.Image = (Image) componentResourceManager.GetObject("lbSex.Appearance.Image");
      this.lbSex.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.PressedImage");
      this.lbSex.Name = "lbSex";
      componentResourceManager.ApplyResources((object) this.cbSEX, "cbSEX");
      this.cbSEX.Name = "cbSEX";
      this.cbSEX.Properties.AccessibleDescription = componentResourceManager.GetString("cbSEX.Properties.AccessibleDescription");
      this.cbSEX.Properties.AccessibleName = componentResourceManager.GetString("cbSEX.Properties.AccessibleName");
      this.cbSEX.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Font");
      this.cbSEX.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontSizeDelta");
      this.cbSEX.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontStyleDelta");
      this.cbSEX.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbSEX.Properties.Appearance.GradientMode");
      this.cbSEX.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Image");
      this.cbSEX.Properties.Appearance.Options.UseFont = true;
      this.cbSEX.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbSEX.Properties.AutoHeight");
      this.cbSEX.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbSEX.Properties.Buttons"))
      });
      this.cbSEX.Properties.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cbSEX.Properties.Items"),
        (object) componentResourceManager.GetString("cbSEX.Properties.Items1")
      });
      this.cbSEX.Properties.NullValuePrompt = componentResourceManager.GetString("cbSEX.Properties.NullValuePrompt");
      this.cbSEX.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbSEX.Properties.NullValuePromptShowForEmptyValue");
      this.cbSEX.Properties.ReadOnly = true;
      this.cbSEX.SelectedIndexChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.tbLastName, "tbLastName");
      this.tbLastName.Name = "tbLastName";
      this.tbLastName.Properties.AccessibleDescription = componentResourceManager.GetString("tbLastName.Properties.AccessibleDescription");
      this.tbLastName.Properties.AccessibleName = componentResourceManager.GetString("tbLastName.Properties.AccessibleName");
      this.tbLastName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Font");
      this.tbLastName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontSizeDelta");
      this.tbLastName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontStyleDelta");
      this.tbLastName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbLastName.Properties.Appearance.GradientMode");
      this.tbLastName.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Image");
      this.tbLastName.Properties.Appearance.Options.UseFont = true;
      this.tbLastName.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbLastName.Properties.AutoHeight");
      this.tbLastName.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbLastName.Properties.Mask.AutoComplete");
      this.tbLastName.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.BeepOnError");
      this.tbLastName.Properties.Mask.EditMask = componentResourceManager.GetString("tbLastName.Properties.Mask.EditMask");
      this.tbLastName.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.IgnoreMaskBlank");
      this.tbLastName.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbLastName.Properties.Mask.MaskType");
      this.tbLastName.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbLastName.Properties.Mask.PlaceHolder");
      this.tbLastName.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.SaveLiteral");
      this.tbLastName.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.ShowPlaceHolders");
      this.tbLastName.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbLastName.Properties.NullValuePrompt = componentResourceManager.GetString("tbLastName.Properties.NullValuePrompt");
      this.tbLastName.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbLastName.Properties.NullValuePromptShowForEmptyValue");
      this.tbLastName.Properties.ReadOnly = true;
      this.tbLastName.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbLastName, "lbLastName");
      this.lbLastName.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.DisabledImage");
      this.lbLastName.Appearance.Font = (Font) componentResourceManager.GetObject("lbLastName.Appearance.Font");
      this.lbLastName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbLastName.Appearance.FontSizeDelta");
      this.lbLastName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbLastName.Appearance.FontStyleDelta");
      this.lbLastName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbLastName.Appearance.ForeColor");
      this.lbLastName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbLastName.Appearance.GradientMode");
      this.lbLastName.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.HoverImage");
      this.lbLastName.Appearance.Image = (Image) componentResourceManager.GetObject("lbLastName.Appearance.Image");
      this.lbLastName.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.PressedImage");
      this.lbLastName.Name = "lbLastName";
      componentResourceManager.ApplyResources((object) this.tbFirstName, "tbFirstName");
      this.tbFirstName.Name = "tbFirstName";
      this.tbFirstName.Properties.AccessibleDescription = componentResourceManager.GetString("tbFirstName.Properties.AccessibleDescription");
      this.tbFirstName.Properties.AccessibleName = componentResourceManager.GetString("tbFirstName.Properties.AccessibleName");
      this.tbFirstName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Font");
      this.tbFirstName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontSizeDelta");
      this.tbFirstName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontStyleDelta");
      this.tbFirstName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.GradientMode");
      this.tbFirstName.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Image");
      this.tbFirstName.Properties.Appearance.Options.UseFont = true;
      this.tbFirstName.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbFirstName.Properties.AutoHeight");
      this.tbFirstName.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbFirstName.Properties.Mask.AutoComplete");
      this.tbFirstName.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.BeepOnError");
      this.tbFirstName.Properties.Mask.EditMask = componentResourceManager.GetString("tbFirstName.Properties.Mask.EditMask");
      this.tbFirstName.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.IgnoreMaskBlank");
      this.tbFirstName.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbFirstName.Properties.Mask.MaskType");
      this.tbFirstName.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbFirstName.Properties.Mask.PlaceHolder");
      this.tbFirstName.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.SaveLiteral");
      this.tbFirstName.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.ShowPlaceHolders");
      this.tbFirstName.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbFirstName.Properties.NullValuePrompt = componentResourceManager.GetString("tbFirstName.Properties.NullValuePrompt");
      this.tbFirstName.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbFirstName.Properties.NullValuePromptShowForEmptyValue");
      this.tbFirstName.Properties.ReadOnly = true;
      this.tbFirstName.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbFirstName, "lbFirstName");
      this.lbFirstName.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.DisabledImage");
      this.lbFirstName.Appearance.Font = (Font) componentResourceManager.GetObject("lbFirstName.Appearance.Font");
      this.lbFirstName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbFirstName.Appearance.FontSizeDelta");
      this.lbFirstName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbFirstName.Appearance.FontStyleDelta");
      this.lbFirstName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbFirstName.Appearance.ForeColor");
      this.lbFirstName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbFirstName.Appearance.GradientMode");
      this.lbFirstName.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.HoverImage");
      this.lbFirstName.Appearance.Image = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.Image");
      this.lbFirstName.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.PressedImage");
      this.lbFirstName.Name = "lbFirstName";
      componentResourceManager.ApplyResources((object) this.tbSurname, "tbSurname");
      this.tbSurname.Name = "tbSurname";
      this.tbSurname.Properties.AccessibleDescription = componentResourceManager.GetString("tbSurname.Properties.AccessibleDescription");
      this.tbSurname.Properties.AccessibleName = componentResourceManager.GetString("tbSurname.Properties.AccessibleName");
      this.tbSurname.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Font");
      this.tbSurname.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontSizeDelta");
      this.tbSurname.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontStyleDelta");
      this.tbSurname.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbSurname.Properties.Appearance.GradientMode");
      this.tbSurname.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Image");
      this.tbSurname.Properties.Appearance.Options.UseFont = true;
      this.tbSurname.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbSurname.Properties.AutoHeight");
      this.tbSurname.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbSurname.Properties.Mask.AutoComplete");
      this.tbSurname.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.BeepOnError");
      this.tbSurname.Properties.Mask.EditMask = componentResourceManager.GetString("tbSurname.Properties.Mask.EditMask");
      this.tbSurname.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.IgnoreMaskBlank");
      this.tbSurname.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbSurname.Properties.Mask.MaskType");
      this.tbSurname.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbSurname.Properties.Mask.PlaceHolder");
      this.tbSurname.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.SaveLiteral");
      this.tbSurname.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.ShowPlaceHolders");
      this.tbSurname.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbSurname.Properties.NullValuePrompt = componentResourceManager.GetString("tbSurname.Properties.NullValuePrompt");
      this.tbSurname.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbSurname.Properties.NullValuePromptShowForEmptyValue");
      this.tbSurname.Properties.ReadOnly = true;
      this.tbSurname.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbSurname, "lbSurname");
      this.lbSurname.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.DisabledImage");
      this.lbSurname.Appearance.Font = (Font) componentResourceManager.GetObject("lbSurname.Appearance.Font");
      this.lbSurname.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSurname.Appearance.FontSizeDelta");
      this.lbSurname.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSurname.Appearance.FontStyleDelta");
      this.lbSurname.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSurname.Appearance.ForeColor");
      this.lbSurname.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSurname.Appearance.GradientMode");
      this.lbSurname.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.HoverImage");
      this.lbSurname.Appearance.Image = (Image) componentResourceManager.GetObject("lbSurname.Appearance.Image");
      this.lbSurname.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.PressedImage");
      this.lbSurname.Name = "lbSurname";
      componentResourceManager.ApplyResources((object) this.gcImagesFullFace, "gcImagesFullFace");
      this.gcImagesFullFace.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleDescription");
      this.gcImagesFullFace.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleName");
      this.gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip");
      this.gcImagesFullFace.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.Anchor");
      this.gcImagesFullFace.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImage");
      this.gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout");
      this.gcImagesFullFace.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ImeMode");
      this.gcImagesFullFace.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.MaximumSize");
      this.gcImagesFullFace.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.TextLocation");
      this.gcImagesFullFace.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.ToolTip");
      this.gcImagesFullFace.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ToolTipIconType");
      this.gcImagesFullFace.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.ToolTipTitle");
      gridLevelNode.RelationName = "Level1";
      this.gcImagesFullFace.LevelTree.Nodes.AddRange(new GridLevelNode[1]
      {
        gridLevelNode
      });
      this.gcImagesFullFace.MainView = (BaseView) this.lvImagesFullFace;
      this.gcImagesFullFace.Name = "gcImagesFullFace";
      this.gcImagesFullFace.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.repositoryItemButtonEdit1
      });
      this.gcImagesFullFace.ViewCollection.AddRange(new BaseView[2]
      {
        (BaseView) this.lvImagesFullFace,
        (BaseView) this.gridView1
      });
      componentResourceManager.ApplyResources((object) this.lvImagesFullFace, "lvImagesFullFace");
      this.lvImagesFullFace.CardHorzInterval = 0;
      this.lvImagesFullFace.CardMinSize = new Size(261, 352);
      this.lvImagesFullFace.CardVertInterval = 0;
      this.lvImagesFullFace.Columns.AddRange(new LayoutViewColumn[4]
      {
        this.colImage,
        this.colName,
        this.colImageComment,
        this.colImageID
      });
      this.lvImagesFullFace.GridControl = this.gcImagesFullFace;
      this.lvImagesFullFace.HiddenItems.AddRange(new BaseLayoutItem[1]
      {
        (BaseLayoutItem) this.layoutViewField_layoutViewColumn1_1
      });
      this.lvImagesFullFace.Name = "lvImagesFullFace";
      this.lvImagesFullFace.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      this.lvImagesFullFace.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
      this.lvImagesFullFace.OptionsBehavior.AllowExpandCollapse = false;
      this.lvImagesFullFace.OptionsBehavior.AutoFocusCardOnScrolling = true;
      this.lvImagesFullFace.OptionsBehavior.AutoFocusNewCard = true;
      this.lvImagesFullFace.OptionsBehavior.AutoPopulateColumns = false;
      this.lvImagesFullFace.OptionsBehavior.AutoSelectAllInEditor = false;
      this.lvImagesFullFace.OptionsBehavior.FocusLeaveOnTab = true;
      this.lvImagesFullFace.OptionsBehavior.KeepFocusedRowOnUpdate = false;
      this.lvImagesFullFace.OptionsCustomization.AllowFilter = false;
      this.lvImagesFullFace.OptionsCustomization.AllowSort = false;
      this.lvImagesFullFace.OptionsHeaderPanel.ShowCustomizeButton = false;
      this.lvImagesFullFace.OptionsHeaderPanel.ShowPanButton = false;
      this.lvImagesFullFace.OptionsItemText.AlignMode = FieldTextAlignMode.AutoSize;
      this.lvImagesFullFace.OptionsItemText.TextToControlDistance = 0;
      this.lvImagesFullFace.OptionsLayout.Columns.AddNewColumns = false;
      this.lvImagesFullFace.OptionsLayout.Columns.RemoveOldColumns = false;
      this.lvImagesFullFace.OptionsLayout.Columns.StoreLayout = false;
      this.lvImagesFullFace.OptionsLayout.StoreDataSettings = false;
      this.lvImagesFullFace.OptionsLayout.StoreVisualOptions = false;
      this.lvImagesFullFace.OptionsSelection.MultiSelect = true;
      this.lvImagesFullFace.OptionsView.AllowHotTrackFields = false;
      this.lvImagesFullFace.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.lvImagesFullFace.OptionsView.ShowCardExpandButton = false;
      this.lvImagesFullFace.OptionsView.ShowCardFieldBorders = true;
      this.lvImagesFullFace.OptionsView.ShowFieldHints = false;
      this.lvImagesFullFace.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
      this.lvImagesFullFace.OptionsView.ShowViewCaption = true;
      this.lvImagesFullFace.OptionsView.ViewMode = LayoutViewMode.Column;
      this.lvImagesFullFace.TemplateCard = this.layoutViewCard1;
      this.lvImagesFullFace.CustomDrawCardCaption += new LayoutViewCustomDrawCardCaptionEventHandler(this.lvImagesFullFace_CustomDrawCardCaption);
      this.lvImagesFullFace.CellValueChanged += new CellValueChangedEventHandler(this.lvImagesFullFace_CellValueChanged);
      this.colImage.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceCell.Font");
      this.colImage.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceCell.FontSizeDelta");
      this.colImage.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceCell.FontStyleDelta");
      this.colImage.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceCell.GradientMode");
      this.colImage.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceCell.Image");
      this.colImage.AppearanceCell.Options.UseFont = true;
      this.colImage.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
      this.colImage.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceHeader.FontSizeDelta");
      this.colImage.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceHeader.FontStyleDelta");
      this.colImage.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceHeader.GradientMode");
      this.colImage.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceHeader.Image");
      this.colImage.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colImage, "colImage");
      this.colImage.ColumnEdit = (RepositoryItem) this.repositoryItemPictureEdit1;
      this.colImage.FieldName = "Image";
      this.colImage.LayoutViewField = this.layoutViewField_layoutViewColumn1;
      this.colImage.Name = "colImage";
      this.colImage.OptionsColumn.AllowMove = false;
      this.colImage.OptionsColumn.AllowShowHide = false;
      this.colImage.OptionsColumn.AllowSize = false;
      this.layoutViewField_layoutViewColumn1.EditorPreferredWidth = 252;
      this.layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
      this.layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
      this.layoutViewField_layoutViewColumn1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_layoutViewColumn1.Size = new Size(259, 22);
      this.layoutViewField_layoutViewColumn1.TextSize = new Size(7, 13);
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
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.FieldName = "Name";
      this.colName.LayoutViewField = this.layoutViewField_layoutViewColumn2_1;
      this.colName.Name = "colName";
      this.colName.OptionsColumn.AllowMove = false;
      this.colName.OptionsColumn.AllowSize = false;
      this.layoutViewField_layoutViewColumn2_1.EditorPreferredWidth = 182;
      this.layoutViewField_layoutViewColumn2_1.Location = new Point(0, 22);
      this.layoutViewField_layoutViewColumn2_1.Name = "layoutViewField_layoutViewColumn2_1";
      this.layoutViewField_layoutViewColumn2_1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_layoutViewColumn2_1.Size = new Size(259, 16);
      this.layoutViewField_layoutViewColumn2_1.TextSize = new Size(31, 13);
      this.colImageComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceCell.Font");
      this.colImageComment.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontSizeDelta");
      this.colImageComment.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontStyleDelta");
      this.colImageComment.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceCell.GradientMode");
      this.colImageComment.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImageComment.AppearanceCell.Image");
      this.colImageComment.AppearanceCell.Options.UseFont = true;
      this.colImageComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Font");
      this.colImageComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontSizeDelta");
      this.colImageComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontStyleDelta");
      this.colImageComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceHeader.GradientMode");
      this.colImageComment.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Image");
      this.colImageComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colImageComment, "colImageComment");
      this.colImageComment.FieldName = "Comment";
      this.colImageComment.LayoutViewField = this.layoutViewField_layoutViewColumn2;
      this.colImageComment.Name = "colImageComment";
      this.colImageComment.OptionsColumn.AllowMove = false;
      this.colImageComment.OptionsColumn.AllowSize = false;
      this.layoutViewField_layoutViewColumn2.EditorPreferredWidth = 194;
      this.layoutViewField_layoutViewColumn2.Location = new Point(0, 38);
      this.layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
      this.layoutViewField_layoutViewColumn2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_layoutViewColumn2.Size = new Size(259, 16);
      this.layoutViewField_layoutViewColumn2.TextSize = new Size(59, 13);
      componentResourceManager.ApplyResources((object) this.colImageID, "colImageID");
      this.colImageID.FieldName = "ImageID";
      this.colImageID.LayoutViewField = this.layoutViewField_layoutViewColumn1_1;
      this.colImageID.Name = "colImageID";
      this.colImageID.OptionsColumn.AllowMove = false;
      this.colImageID.OptionsColumn.AllowSize = false;
      this.layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 10;
      this.layoutViewField_layoutViewColumn1_1.Location = new Point(0, 0);
      this.layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
      this.layoutViewField_layoutViewColumn1_1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_layoutViewColumn1_1.Size = new Size(280, 54);
      this.layoutViewField_layoutViewColumn1_1.TextSize = new Size(77, 20);
      this.layoutViewField_layoutViewColumn1_1.TextToControlDistance = 0;
      componentResourceManager.ApplyResources((object) this.layoutViewCard1, "layoutViewCard1");
      this.layoutViewCard1.ExpandButtonLocation = GroupElementLocation.AfterText;
      this.layoutViewCard1.Items.AddRange(new BaseLayoutItem[3]
      {
        (BaseLayoutItem) this.layoutViewField_layoutViewColumn1,
        (BaseLayoutItem) this.layoutViewField_layoutViewColumn2_1,
        (BaseLayoutItem) this.layoutViewField_layoutViewColumn2
      });
      this.layoutViewCard1.Name = "layoutViewCard1";
      this.layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
      this.layoutViewCard1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      componentResourceManager.ApplyResources((object) this.repositoryItemButtonEdit1, "repositoryItemButtonEdit1");
      componentResourceManager.ApplyResources((object) appearanceObject, "serializableAppearanceObject1");
      this.repositoryItemButtonEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons"), componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons1"), (int) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons2"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons3"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons4"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons5"), (ImageLocation) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons6"), (Image) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons7"), new KeyShortcut(System.Windows.Forms.Keys.None), (AppearanceObject) appearanceObject, componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons8"), componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons9"), (SuperToolTip) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons10"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons11"))
      });
      this.repositoryItemButtonEdit1.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.AutoComplete");
      this.repositoryItemButtonEdit1.Mask.BeepOnError = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.BeepOnError");
      this.repositoryItemButtonEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemButtonEdit1.Mask.EditMask");
      this.repositoryItemButtonEdit1.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.IgnoreMaskBlank");
      this.repositoryItemButtonEdit1.Mask.MaskType = (MaskType) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.MaskType");
      this.repositoryItemButtonEdit1.Mask.PlaceHolder = (char) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.PlaceHolder");
      this.repositoryItemButtonEdit1.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.SaveLiteral");
      this.repositoryItemButtonEdit1.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.ShowPlaceHolders");
      this.repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat");
      this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
      this.repositoryItemButtonEdit1.TextEditStyle = TextEditStyles.DisableTextEditor;
      componentResourceManager.ApplyResources((object) this.gridView1, "gridView1");
      this.gridView1.GridControl = this.gcImagesFullFace;
      this.gridView1.Name = "gridView1";
      componentResourceManager.ApplyResources((object) this.pictureEdit1, "pictureEdit1");
      this.pictureEdit1.Name = "pictureEdit1";
      this.pictureEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleDescription");
      this.pictureEdit1.Properties.AccessibleName = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleName");
      this.pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this.groupControl1, "groupControl1");
      this.groupControl1.Appearance.Font = (Font) componentResourceManager.GetObject("groupControl1.Appearance.Font");
      this.groupControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.Appearance.FontSizeDelta");
      this.groupControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.Appearance.FontStyleDelta");
      this.groupControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.Appearance.GradientMode");
      this.groupControl1.Appearance.Image = (Image) componentResourceManager.GetObject("groupControl1.Appearance.Image");
      this.groupControl1.Appearance.Options.UseFont = true;
      this.groupControl1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Font");
      this.groupControl1.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontSizeDelta");
      this.groupControl1.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontStyleDelta");
      this.groupControl1.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.AppearanceCaption.GradientMode");
      this.groupControl1.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Image");
      this.groupControl1.AppearanceCaption.Options.UseFont = true;
      this.groupControl1.Controls.Add((Control) this.dtDate);
      this.groupControl1.Controls.Add((Control) this.tbCam);
      this.groupControl1.Controls.Add((Control) this.labelControl2);
      this.groupControl1.Controls.Add((Control) this.labelControl3);
      this.groupControl1.Controls.Add((Control) this.pictureEdit1);
      this.groupControl1.Name = "groupControl1";
      componentResourceManager.ApplyResources((object) this.dtDate, "dtDate");
      this.dtDate.Name = "dtDate";
      this.dtDate.Properties.AccessibleDescription = componentResourceManager.GetString("dtDate.Properties.AccessibleDescription");
      this.dtDate.Properties.AccessibleName = componentResourceManager.GetString("dtDate.Properties.AccessibleName");
      this.dtDate.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtDate.Properties.Appearance.Font");
      this.dtDate.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtDate.Properties.Appearance.FontSizeDelta");
      this.dtDate.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtDate.Properties.Appearance.FontStyleDelta");
      this.dtDate.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtDate.Properties.Appearance.GradientMode");
      this.dtDate.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtDate.Properties.Appearance.Image");
      this.dtDate.Properties.Appearance.Options.UseFont = true;
      this.dtDate.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtDate.Properties.AutoHeight");
      this.dtDate.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtDate.Properties.Buttons"))
      });
      this.dtDate.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtDate.Properties.CalendarTimeProperties.AccessibleDescription");
      this.dtDate.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtDate.Properties.CalendarTimeProperties.AccessibleName");
      this.dtDate.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.AutoHeight");
      this.dtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.dtDate.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.AutoComplete");
      this.dtDate.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.BeepOnError");
      this.dtDate.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtDate.Properties.CalendarTimeProperties.Mask.EditMask");
      this.dtDate.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      this.dtDate.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.MaskType");
      this.dtDate.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      this.dtDate.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      this.dtDate.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      this.dtDate.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      this.dtDate.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtDate.Properties.CalendarTimeProperties.NullValuePrompt");
      this.dtDate.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtDate.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      this.dtDate.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      this.dtDate.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      this.dtDate.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtDate.Properties.Mask.AutoComplete");
      this.dtDate.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtDate.Properties.Mask.BeepOnError");
      this.dtDate.Properties.Mask.EditMask = componentResourceManager.GetString("dtDate.Properties.Mask.EditMask");
      this.dtDate.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtDate.Properties.Mask.IgnoreMaskBlank");
      this.dtDate.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtDate.Properties.Mask.MaskType");
      this.dtDate.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtDate.Properties.Mask.PlaceHolder");
      this.dtDate.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtDate.Properties.Mask.SaveLiteral");
      this.dtDate.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtDate.Properties.Mask.ShowPlaceHolders");
      this.dtDate.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtDate.Properties.Mask.UseMaskAsDisplayFormat");
      this.dtDate.Properties.NullValuePrompt = componentResourceManager.GetString("dtDate.Properties.NullValuePrompt");
      this.dtDate.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtDate.Properties.NullValuePromptShowForEmptyValue");
      this.dtDate.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.tbCam, "tbCam");
      this.tbCam.Name = "tbCam";
      this.tbCam.Properties.AccessibleDescription = componentResourceManager.GetString("tbCam.Properties.AccessibleDescription");
      this.tbCam.Properties.AccessibleName = componentResourceManager.GetString("tbCam.Properties.AccessibleName");
      this.tbCam.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbCam.Properties.Appearance.Font");
      this.tbCam.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbCam.Properties.Appearance.FontSizeDelta");
      this.tbCam.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbCam.Properties.Appearance.FontStyleDelta");
      this.tbCam.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbCam.Properties.Appearance.GradientMode");
      this.tbCam.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbCam.Properties.Appearance.Image");
      this.tbCam.Properties.Appearance.Options.UseFont = true;
      this.tbCam.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbCam.Properties.AutoHeight");
      this.tbCam.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbCam.Properties.Mask.AutoComplete");
      this.tbCam.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbCam.Properties.Mask.BeepOnError");
      this.tbCam.Properties.Mask.EditMask = componentResourceManager.GetString("tbCam.Properties.Mask.EditMask");
      this.tbCam.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbCam.Properties.Mask.IgnoreMaskBlank");
      this.tbCam.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbCam.Properties.Mask.MaskType");
      this.tbCam.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbCam.Properties.Mask.PlaceHolder");
      this.tbCam.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbCam.Properties.Mask.SaveLiteral");
      this.tbCam.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbCam.Properties.Mask.ShowPlaceHolders");
      this.tbCam.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbCam.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbCam.Properties.NullValuePrompt = componentResourceManager.GetString("tbCam.Properties.NullValuePrompt");
      this.tbCam.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbCam.Properties.NullValuePromptShowForEmptyValue");
      this.tbCam.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.labelControl2, "labelControl2");
      this.labelControl2.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.DisabledImage");
      this.labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      this.labelControl2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl2.Appearance.FontSizeDelta");
      this.labelControl2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl2.Appearance.FontStyleDelta");
      this.labelControl2.Appearance.ForeColor = (Color) componentResourceManager.GetObject("labelControl2.Appearance.ForeColor");
      this.labelControl2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl2.Appearance.GradientMode");
      this.labelControl2.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.HoverImage");
      this.labelControl2.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl2.Appearance.Image");
      this.labelControl2.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.PressedImage");
      this.labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources((object) this.labelControl3, "labelControl3");
      this.labelControl3.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.DisabledImage");
      this.labelControl3.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl3.Appearance.Font");
      this.labelControl3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl3.Appearance.FontSizeDelta");
      this.labelControl3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl3.Appearance.FontStyleDelta");
      this.labelControl3.Appearance.ForeColor = (Color) componentResourceManager.GetObject("labelControl3.Appearance.ForeColor");
      this.labelControl3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl3.Appearance.GradientMode");
      this.labelControl3.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.HoverImage");
      this.labelControl3.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl3.Appearance.Image");
      this.labelControl3.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.PressedImage");
      this.labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupControl1);
      this.Controls.Add((Control) this.gcImagesFullFace);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = "FrmCompareImages";
      this.WindowState = FormWindowState.Maximized;
      this.HelpButtonClicked += new CancelEventHandler(this.EditEmployerForm_HelpButtonClicked);
      this.FormClosing += new FormClosingEventHandler(this.EditEmployerForm_FormClosing);
      this.Load += new EventHandler(this.EditEmployerForm_Load);
      this.HelpRequested += new HelpEventHandler(this.EditEmployerForm_HelpRequested);
      this.repositoryItemPictureEdit1.EndInit();
      this.repositoryItemCheckEdit1.EndInit();
      this.groupBox1.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tbAddress.Properties.EndInit();
      this.tbComment.Properties.EndInit();
      this.dtpBithday.Properties.CalendarTimeProperties.EndInit();
      this.dtpBithday.Properties.EndInit();
      this.tbPassport.Properties.EndInit();
      this.cbAccessTemplate.Properties.EndInit();
      this.cbSEX.Properties.EndInit();
      this.tbLastName.Properties.EndInit();
      this.tbFirstName.Properties.EndInit();
      this.tbSurname.Properties.EndInit();
      this.gcImagesFullFace.EndInit();
      this.lvImagesFullFace.EndInit();
      this.layoutViewField_layoutViewColumn1.EndInit();
      this.layoutViewField_layoutViewColumn2_1.EndInit();
      this.layoutViewField_layoutViewColumn2.EndInit();
      this.layoutViewField_layoutViewColumn1_1.EndInit();
      this.layoutViewCard1.EndInit();
      this.repositoryItemButtonEdit1.EndInit();
      this.gridView1.EndInit();
      this.pictureEdit1.Properties.EndInit();
      this.groupControl1.EndInit();
      this.groupControl1.ResumeLayout(false);
      this.groupControl1.PerformLayout();
      this.dtDate.Properties.CalendarTimeProperties.EndInit();
      this.dtDate.Properties.EndInit();
      this.tbCam.Properties.EndInit();
      this.ResumeLayout(false);
    }
  }
}
