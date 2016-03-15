// Decompiled with JetBrains decompiler
// Type: CascadeManager.ShowFaceForm
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BasicComponents;
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
using Padding = DevExpress.XtraLayout.Utils.Padding;

namespace CascadeManager
{
  public class ShowFaceForm : XtraForm
  {
    public BcFace CurrentEmployer = new BcFace();
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
    private IContainer components = null;
    public Bitmap MainImage;
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

    public ShowFaceForm()
    {
      InitializeComponent();
    }

    private void LoadImages()
    {
      _dtImages = new DataTable();
      _dtImages.Columns.Add("ID", typeof (Guid));
      _dtImages.Columns.Add("ImageIcon", typeof (Bitmap));
      _dtImages.Columns.Add("Image", typeof (Bitmap));
      _dtImages.Columns.Add("Name", typeof (string));
      _dtImages.Columns.Add("Comment", typeof (string));
      _dtImages.Columns.Add("IsMain", typeof (bool));
      _dtImages.Columns.Add("Changed", typeof (bool));
      _dtImages.Columns.Add("ImageChanged", typeof (bool));
      if (!(CurrentEmployer.Id != Guid.Empty))
        return;
      foreach (BcImage bcImage in BcImage.LoadByFaceId(CurrentEmployer.Id))
      {
        Bitmap bitmap1 = new Bitmap(new MemoryStream(bcImage.ImageIcon));
        Bitmap bitmap2 = new Bitmap(new MemoryStream(bcImage.Image));
        _dtImages.Rows.Add((object) bcImage.Id, (object) bitmap1, (object) bitmap2, (object) bcImage.Name, (object) bcImage.Comment, (object) (bool) (bcImage.IsMain ? 1 : 0), (object) false, (object) false);
      }
    }

    private int IndexOfDep(Guid id)
    {
      int num = -1;
      foreach (BcDepartment bcDepartment in _deps)
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
      foreach (BcOrganization bcOrganization in _orgs)
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
      foreach (BcTiming bcTiming in _timinList)
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
      foreach (BcAccessCategory bcAccessCategory in _templateList)
      {
        ++num;
        if (bcAccessCategory.Id == id)
          break;
      }
      return num;
    }

    private void FindChanges()
    {
      if (_changedValues)
      {
        if (XtraMessageBox.Show("В запись были внесены изменения.\r\nВы хотите сохранить их?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        btSave_Click(new object(), new EventArgs());
        _changedValues = false;
      }
      else
        _cancelFlag = false;
    }

    private void btSave_Click(object sender, EventArgs e)
    {
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      FindChanges();
      Close();
    }

    private void EditEmployerForm_Load(object sender, EventArgs e)
    {
      WindowState = FormWindowState.Normal;
      StartPosition = FormStartPosition.CenterParent;
      try
      {
        pictureEdit1.Image = MainImage;
      }
      catch
      {
      }
      cbSEX.SelectedIndex = 0;
      _isloading = true;
      _changedValues = false;
      SqlCommand sqlCommand = new SqlCommand(" \r\nSelect \r\ndistinct\r\nCountry, \r\nRegion ,\r\nCity, \r\nDistrict ,\r\nStreet\r\nFrom Faces\r\norder by\r\nCountry,\r\nRegion,\r\nCity,\r\nDistrict,\r\nStreet\r\n", new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      sqlCommand.ExecuteReader();
      sqlCommand.Connection.Close();
      _deps = BcDepartment.LoadAll();
      _timinList = BcTiming.LoadAll();
      _templateList = BcAccessCategory.LoadAll();
      foreach (BcAccessCategory bcAccessCategory in _templateList)
        cbAccessTemplate.Properties.Items.Add(bcAccessCategory.Name);
      LoadImages();
      gcImagesFullFace.DataSource = _dtImages;
      if (CurrentEmployer.Id != Guid.Empty)
      {
        Keys = new List<BcKey>();
        CurrentKeys = BcKey.LoadKyesByFaceId(CurrentEmployer.Id);
        tbAddress.Text = BcFace.FullAddress(CurrentEmployer);
        dtpBithday.DateTime = CurrentEmployer.Birthday;
        tbFirstName.Text = CurrentEmployer.FirstName;
        tbSurname.Text = CurrentEmployer.Surname;
        tbLastName.Text = CurrentEmployer.LastName;
        tbPassport.Text = CurrentEmployer.Passport;
        tbComment.Text = CurrentEmployer.Comment;
        cbSEX.SelectedIndex = 0;
        if (CurrentEmployer.Sex == 1)
          cbSEX.SelectedIndex = 1;
        cbAccessTemplate.SelectedIndex = IndexOfAccessTemplate(CurrentEmployer.AccessId);
      }
      _isloading = false;
    }

    private void EditEmployerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
        _cancelFlag = false;
      if (_cancelFlag)
        e.Cancel = true;
      else if (_flagAfterAdd)
      {
        if (XtraMessageBox.Show("Была создана новая запись.\r\nВы хотите продолжить добавление?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          AddNewValue = true;
        else
          AddNewValue = false;
      }
      else
        FindChanges();
    }

    private void TbValueChanged(object sender, EventArgs e)
    {
      if (_isloading)
        return;
      _changedValues = true;
    }

    private void chbHolidayState_CheckedChanged(object sender, EventArgs e)
    {
      if (_isloading)
        return;
      _changedValues = true;
    }

    private void EditEmployerForm_HelpButtonClicked(object sender, CancelEventArgs e)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/14.htm");
    }

    private void EditEmployerForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/14.htm");
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
      if (lvImagesFullFace.FocusedRowHandle < 0)
        return;
      DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
      _deletedImages.Add((Guid) dataRow["ID"]);
      _dtImages.Rows.Remove(dataRow);
    }

    private void lvImagesFullFace_CustomDrawCardCaption(object sender, LayoutViewCustomDrawCardCaptionEventArgs e)
    {
      if (e.RowHandle < 0)
        return;
      DataRow dataRow = lvImagesFullFace.GetDataRow(e.RowHandle);
      e.CardCaption = dataRow["Name"].ToString();
    }

    private void lvImagesFullFace_CellValueChanged(object sender, CellValueChangedEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ShowFaceForm));
      GridLevelNode gridLevelNode = new GridLevelNode();
      SerializableAppearanceObject appearanceObject = new SerializableAppearanceObject();
      repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      groupBox1 = new GroupControl();
      tbAddress = new MemoEdit();
      labelControl1 = new LabelControl();
      tbComment = new MemoEdit();
      dtpBithday = new DateEdit();
      tbPassport = new TextEdit();
      lbBirthday = new LabelControl();
      lbPassport = new LabelControl();
      label3 = new LabelControl();
      cbAccessTemplate = new ComboBoxEdit();
      lbComment = new LabelControl();
      lbSex = new LabelControl();
      cbSEX = new ComboBoxEdit();
      tbLastName = new TextEdit();
      lbLastName = new LabelControl();
      tbFirstName = new TextEdit();
      lbFirstName = new LabelControl();
      tbSurname = new TextEdit();
      lbSurname = new LabelControl();
      pageSetupDialog1 = new PageSetupDialog();
      gcImagesFullFace = new GridControl();
      lvImagesFullFace = new LayoutView();
      colImage = new LayoutViewColumn();
      layoutViewField_layoutViewColumn1 = new LayoutViewField();
      colName = new LayoutViewColumn();
      layoutViewField_layoutViewColumn2_1 = new LayoutViewField();
      colImageComment = new LayoutViewColumn();
      layoutViewField_layoutViewColumn2 = new LayoutViewField();
      colImageID = new LayoutViewColumn();
      layoutViewField_layoutViewColumn1_1 = new LayoutViewField();
      layoutViewCard1 = new LayoutViewCard();
      repositoryItemButtonEdit1 = new RepositoryItemButtonEdit();
      gridView1 = new GridView();
      pictureEdit1 = new PictureEdit();
      groupControl1 = new GroupControl();
      repositoryItemPictureEdit1.BeginInit();
      repositoryItemCheckEdit1.BeginInit();
      groupBox1.BeginInit();
      groupBox1.SuspendLayout();
      tbAddress.Properties.BeginInit();
      tbComment.Properties.BeginInit();
      dtpBithday.Properties.CalendarTimeProperties.BeginInit();
      dtpBithday.Properties.BeginInit();
      tbPassport.Properties.BeginInit();
      cbAccessTemplate.Properties.BeginInit();
      cbSEX.Properties.BeginInit();
      tbLastName.Properties.BeginInit();
      tbFirstName.Properties.BeginInit();
      tbSurname.Properties.BeginInit();
      gcImagesFullFace.BeginInit();
      lvImagesFullFace.BeginInit();
      layoutViewField_layoutViewColumn1.BeginInit();
      layoutViewField_layoutViewColumn2_1.BeginInit();
      layoutViewField_layoutViewColumn2.BeginInit();
      layoutViewField_layoutViewColumn1_1.BeginInit();
      layoutViewCard1.BeginInit();
      repositoryItemButtonEdit1.BeginInit();
      gridView1.BeginInit();
      pictureEdit1.Properties.BeginInit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      SuspendLayout();
      componentResourceManager.ApplyResources(repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
      repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.Image;
      repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      repositoryItemCheckEdit1.AutoWidth = true;
      repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Controls.Add(tbAddress);
      groupBox1.Controls.Add(labelControl1);
      groupBox1.Controls.Add(tbComment);
      groupBox1.Controls.Add(dtpBithday);
      groupBox1.Controls.Add(tbPassport);
      groupBox1.Controls.Add(lbBirthday);
      groupBox1.Controls.Add(lbPassport);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(cbAccessTemplate);
      groupBox1.Controls.Add(lbComment);
      groupBox1.Controls.Add(lbSex);
      groupBox1.Controls.Add(cbSEX);
      groupBox1.Controls.Add(tbLastName);
      groupBox1.Controls.Add(lbLastName);
      groupBox1.Controls.Add(tbFirstName);
      groupBox1.Controls.Add(lbFirstName);
      groupBox1.Controls.Add(tbSurname);
      groupBox1.Controls.Add(lbSurname);
      groupBox1.Name = "groupBox1";
      groupBox1.ShowCaption = false;
      componentResourceManager.ApplyResources(tbAddress, "tbAddress");
      tbAddress.Name = "tbAddress";
      tbAddress.Properties.AccessibleDescription = componentResourceManager.GetString("tbAddress.Properties.AccessibleDescription");
      tbAddress.Properties.AccessibleName = componentResourceManager.GetString("tbAddress.Properties.AccessibleName");
      tbAddress.Properties.NullValuePrompt = componentResourceManager.GetString("tbAddress.Properties.NullValuePrompt");
      tbAddress.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbAddress.Properties.NullValuePromptShowForEmptyValue");
      tbAddress.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.DisabledImage");
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      labelControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl1.Appearance.FontSizeDelta");
      labelControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl1.Appearance.FontStyleDelta");
      labelControl1.Appearance.ForeColor = (Color) componentResourceManager.GetObject("labelControl1.Appearance.ForeColor");
      labelControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl1.Appearance.GradientMode");
      labelControl1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.HoverImage");
      labelControl1.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl1.Appearance.Image");
      labelControl1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.PressedImage");
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(tbComment, "tbComment");
      tbComment.Name = "tbComment";
      tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      tbComment.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources(dtpBithday, "dtpBithday");
      dtpBithday.Name = "dtpBithday";
      dtpBithday.Properties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.AccessibleDescription");
      dtpBithday.Properties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.AccessibleName");
      dtpBithday.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Font");
      dtpBithday.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontSizeDelta");
      dtpBithday.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontStyleDelta");
      dtpBithday.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.GradientMode");
      dtpBithday.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Image");
      dtpBithday.Properties.Appearance.Options.UseFont = true;
      dtpBithday.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBithday.Properties.AutoHeight");
      dtpBithday.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtpBithday.Properties.Buttons"))
      });
      dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription");
      dtpBithday.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleName");
      dtpBithday.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.AutoHeight");
      dtpBithday.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete");
      dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError");
      dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask");
      dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType");
      dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt");
      dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      dtpBithday.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBithday.Properties.Mask.AutoComplete");
      dtpBithday.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.BeepOnError");
      dtpBithday.Properties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.Mask.EditMask");
      dtpBithday.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.IgnoreMaskBlank");
      dtpBithday.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBithday.Properties.Mask.MaskType");
      dtpBithday.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBithday.Properties.Mask.PlaceHolder");
      dtpBithday.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.SaveLiteral");
      dtpBithday.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.ShowPlaceHolders");
      dtpBithday.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBithday.Properties.Mask.UseMaskAsDisplayFormat");
      dtpBithday.Properties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.NullValuePrompt");
      dtpBithday.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBithday.Properties.NullValuePromptShowForEmptyValue");
      dtpBithday.Properties.ReadOnly = true;
      componentResourceManager.ApplyResources(tbPassport, "tbPassport");
      tbPassport.Name = "tbPassport";
      tbPassport.Properties.AccessibleDescription = componentResourceManager.GetString("tbPassport.Properties.AccessibleDescription");
      tbPassport.Properties.AccessibleName = componentResourceManager.GetString("tbPassport.Properties.AccessibleName");
      tbPassport.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbPassport.Properties.Appearance.Font");
      tbPassport.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontSizeDelta");
      tbPassport.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontStyleDelta");
      tbPassport.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbPassport.Properties.Appearance.GradientMode");
      tbPassport.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbPassport.Properties.Appearance.Image");
      tbPassport.Properties.Appearance.Options.UseFont = true;
      tbPassport.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbPassport.Properties.AutoHeight");
      tbPassport.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbPassport.Properties.Mask.AutoComplete");
      tbPassport.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.BeepOnError");
      tbPassport.Properties.Mask.EditMask = componentResourceManager.GetString("tbPassport.Properties.Mask.EditMask");
      tbPassport.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.IgnoreMaskBlank");
      tbPassport.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbPassport.Properties.Mask.MaskType");
      tbPassport.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbPassport.Properties.Mask.PlaceHolder");
      tbPassport.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.SaveLiteral");
      tbPassport.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.ShowPlaceHolders");
      tbPassport.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbPassport.Properties.Mask.UseMaskAsDisplayFormat");
      tbPassport.Properties.NullValuePrompt = componentResourceManager.GetString("tbPassport.Properties.NullValuePrompt");
      tbPassport.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbPassport.Properties.NullValuePromptShowForEmptyValue");
      tbPassport.Properties.ReadOnly = true;
      tbPassport.EditValueChanged += tbPassport_EditValueChanged;
      tbPassport.TextChanged += TbValueChanged;
      componentResourceManager.ApplyResources(lbBirthday, "lbBirthday");
      lbBirthday.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.DisabledImage");
      lbBirthday.Appearance.Font = (Font) componentResourceManager.GetObject("lbBirthday.Appearance.Font");
      lbBirthday.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbBirthday.Appearance.FontSizeDelta");
      lbBirthday.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbBirthday.Appearance.FontStyleDelta");
      lbBirthday.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbBirthday.Appearance.ForeColor");
      lbBirthday.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbBirthday.Appearance.GradientMode");
      lbBirthday.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.HoverImage");
      lbBirthday.Appearance.Image = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.Image");
      lbBirthday.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbBirthday.Appearance.PressedImage");
      lbBirthday.Name = "lbBirthday";
      componentResourceManager.ApplyResources(lbPassport, "lbPassport");
      lbPassport.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.DisabledImage");
      lbPassport.Appearance.Font = (Font) componentResourceManager.GetObject("lbPassport.Appearance.Font");
      lbPassport.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbPassport.Appearance.FontSizeDelta");
      lbPassport.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbPassport.Appearance.FontStyleDelta");
      lbPassport.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbPassport.Appearance.ForeColor");
      lbPassport.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbPassport.Appearance.GradientMode");
      lbPassport.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.HoverImage");
      lbPassport.Appearance.Image = (Image) componentResourceManager.GetObject("lbPassport.Appearance.Image");
      lbPassport.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbPassport.Appearance.PressedImage");
      lbPassport.Name = "lbPassport";
      componentResourceManager.ApplyResources(label3, "label3");
      label3.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label3.Appearance.DisabledImage");
      label3.Appearance.Font = (Font) componentResourceManager.GetObject("label3.Appearance.Font");
      label3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label3.Appearance.FontSizeDelta");
      label3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label3.Appearance.FontStyleDelta");
      label3.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label3.Appearance.ForeColor");
      label3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label3.Appearance.GradientMode");
      label3.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label3.Appearance.HoverImage");
      label3.Appearance.Image = (Image) componentResourceManager.GetObject("label3.Appearance.Image");
      label3.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label3.Appearance.PressedImage");
      label3.Name = "label3";
      componentResourceManager.ApplyResources(cbAccessTemplate, "cbAccessTemplate");
      cbAccessTemplate.Name = "cbAccessTemplate";
      cbAccessTemplate.Properties.AccessibleDescription = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleDescription");
      cbAccessTemplate.Properties.AccessibleName = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleName");
      cbAccessTemplate.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Font");
      cbAccessTemplate.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontSizeDelta");
      cbAccessTemplate.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontStyleDelta");
      cbAccessTemplate.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.GradientMode");
      cbAccessTemplate.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Image");
      cbAccessTemplate.Properties.Appearance.Options.UseFont = true;
      cbAccessTemplate.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.AutoHeight");
      cbAccessTemplate.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbAccessTemplate.Properties.Buttons"))
      });
      cbAccessTemplate.Properties.NullValuePrompt = componentResourceManager.GetString("cbAccessTemplate.Properties.NullValuePrompt");
      cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue");
      cbAccessTemplate.Properties.ReadOnly = true;
      cbAccessTemplate.SelectedIndexChanged += TbValueChanged;
      componentResourceManager.ApplyResources(lbComment, "lbComment");
      lbComment.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.DisabledImage");
      lbComment.Appearance.Font = (Font) componentResourceManager.GetObject("lbComment.Appearance.Font");
      lbComment.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbComment.Appearance.FontSizeDelta");
      lbComment.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbComment.Appearance.FontStyleDelta");
      lbComment.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbComment.Appearance.ForeColor");
      lbComment.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbComment.Appearance.GradientMode");
      lbComment.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.HoverImage");
      lbComment.Appearance.Image = (Image) componentResourceManager.GetObject("lbComment.Appearance.Image");
      lbComment.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.PressedImage");
      lbComment.Name = "lbComment";
      componentResourceManager.ApplyResources(lbSex, "lbSex");
      lbSex.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.DisabledImage");
      lbSex.Appearance.Font = (Font) componentResourceManager.GetObject("lbSex.Appearance.Font");
      lbSex.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSex.Appearance.FontSizeDelta");
      lbSex.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSex.Appearance.FontStyleDelta");
      lbSex.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSex.Appearance.ForeColor");
      lbSex.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSex.Appearance.GradientMode");
      lbSex.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.HoverImage");
      lbSex.Appearance.Image = (Image) componentResourceManager.GetObject("lbSex.Appearance.Image");
      lbSex.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.PressedImage");
      lbSex.Name = "lbSex";
      componentResourceManager.ApplyResources(cbSEX, "cbSEX");
      cbSEX.Name = "cbSEX";
      cbSEX.Properties.AccessibleDescription = componentResourceManager.GetString("cbSEX.Properties.AccessibleDescription");
      cbSEX.Properties.AccessibleName = componentResourceManager.GetString("cbSEX.Properties.AccessibleName");
      cbSEX.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Font");
      cbSEX.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontSizeDelta");
      cbSEX.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontStyleDelta");
      cbSEX.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbSEX.Properties.Appearance.GradientMode");
      cbSEX.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Image");
      cbSEX.Properties.Appearance.Options.UseFont = true;
      cbSEX.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbSEX.Properties.AutoHeight");
      cbSEX.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbSEX.Properties.Buttons"))
      });
      cbSEX.Properties.Items.AddRange(new object[2]
      {
        componentResourceManager.GetString("cbSEX.Properties.Items"),
        componentResourceManager.GetString("cbSEX.Properties.Items1")
      });
      cbSEX.Properties.NullValuePrompt = componentResourceManager.GetString("cbSEX.Properties.NullValuePrompt");
      cbSEX.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbSEX.Properties.NullValuePromptShowForEmptyValue");
      cbSEX.Properties.ReadOnly = true;
      cbSEX.SelectedIndexChanged += TbValueChanged;
      componentResourceManager.ApplyResources(tbLastName, "tbLastName");
      tbLastName.Name = "tbLastName";
      tbLastName.Properties.AccessibleDescription = componentResourceManager.GetString("tbLastName.Properties.AccessibleDescription");
      tbLastName.Properties.AccessibleName = componentResourceManager.GetString("tbLastName.Properties.AccessibleName");
      tbLastName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Font");
      tbLastName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontSizeDelta");
      tbLastName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontStyleDelta");
      tbLastName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbLastName.Properties.Appearance.GradientMode");
      tbLastName.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Image");
      tbLastName.Properties.Appearance.Options.UseFont = true;
      tbLastName.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbLastName.Properties.AutoHeight");
      tbLastName.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbLastName.Properties.Mask.AutoComplete");
      tbLastName.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.BeepOnError");
      tbLastName.Properties.Mask.EditMask = componentResourceManager.GetString("tbLastName.Properties.Mask.EditMask");
      tbLastName.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.IgnoreMaskBlank");
      tbLastName.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbLastName.Properties.Mask.MaskType");
      tbLastName.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbLastName.Properties.Mask.PlaceHolder");
      tbLastName.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.SaveLiteral");
      tbLastName.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.ShowPlaceHolders");
      tbLastName.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbLastName.Properties.Mask.UseMaskAsDisplayFormat");
      tbLastName.Properties.NullValuePrompt = componentResourceManager.GetString("tbLastName.Properties.NullValuePrompt");
      tbLastName.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbLastName.Properties.NullValuePromptShowForEmptyValue");
      tbLastName.Properties.ReadOnly = true;
      tbLastName.TextChanged += TbValueChanged;
      componentResourceManager.ApplyResources(lbLastName, "lbLastName");
      lbLastName.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.DisabledImage");
      lbLastName.Appearance.Font = (Font) componentResourceManager.GetObject("lbLastName.Appearance.Font");
      lbLastName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbLastName.Appearance.FontSizeDelta");
      lbLastName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbLastName.Appearance.FontStyleDelta");
      lbLastName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbLastName.Appearance.ForeColor");
      lbLastName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbLastName.Appearance.GradientMode");
      lbLastName.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.HoverImage");
      lbLastName.Appearance.Image = (Image) componentResourceManager.GetObject("lbLastName.Appearance.Image");
      lbLastName.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbLastName.Appearance.PressedImage");
      lbLastName.Name = "lbLastName";
      componentResourceManager.ApplyResources(tbFirstName, "tbFirstName");
      tbFirstName.Name = "tbFirstName";
      tbFirstName.Properties.AccessibleDescription = componentResourceManager.GetString("tbFirstName.Properties.AccessibleDescription");
      tbFirstName.Properties.AccessibleName = componentResourceManager.GetString("tbFirstName.Properties.AccessibleName");
      tbFirstName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Font");
      tbFirstName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontSizeDelta");
      tbFirstName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontStyleDelta");
      tbFirstName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.GradientMode");
      tbFirstName.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Image");
      tbFirstName.Properties.Appearance.Options.UseFont = true;
      tbFirstName.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbFirstName.Properties.AutoHeight");
      tbFirstName.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbFirstName.Properties.Mask.AutoComplete");
      tbFirstName.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.BeepOnError");
      tbFirstName.Properties.Mask.EditMask = componentResourceManager.GetString("tbFirstName.Properties.Mask.EditMask");
      tbFirstName.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.IgnoreMaskBlank");
      tbFirstName.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbFirstName.Properties.Mask.MaskType");
      tbFirstName.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbFirstName.Properties.Mask.PlaceHolder");
      tbFirstName.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.SaveLiteral");
      tbFirstName.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.ShowPlaceHolders");
      tbFirstName.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbFirstName.Properties.Mask.UseMaskAsDisplayFormat");
      tbFirstName.Properties.NullValuePrompt = componentResourceManager.GetString("tbFirstName.Properties.NullValuePrompt");
      tbFirstName.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbFirstName.Properties.NullValuePromptShowForEmptyValue");
      tbFirstName.Properties.ReadOnly = true;
      tbFirstName.TextChanged += TbValueChanged;
      componentResourceManager.ApplyResources(lbFirstName, "lbFirstName");
      lbFirstName.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.DisabledImage");
      lbFirstName.Appearance.Font = (Font) componentResourceManager.GetObject("lbFirstName.Appearance.Font");
      lbFirstName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbFirstName.Appearance.FontSizeDelta");
      lbFirstName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbFirstName.Appearance.FontStyleDelta");
      lbFirstName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbFirstName.Appearance.ForeColor");
      lbFirstName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbFirstName.Appearance.GradientMode");
      lbFirstName.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.HoverImage");
      lbFirstName.Appearance.Image = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.Image");
      lbFirstName.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbFirstName.Appearance.PressedImage");
      lbFirstName.Name = "lbFirstName";
      componentResourceManager.ApplyResources(tbSurname, "tbSurname");
      tbSurname.Name = "tbSurname";
      tbSurname.Properties.AccessibleDescription = componentResourceManager.GetString("tbSurname.Properties.AccessibleDescription");
      tbSurname.Properties.AccessibleName = componentResourceManager.GetString("tbSurname.Properties.AccessibleName");
      tbSurname.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Font");
      tbSurname.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontSizeDelta");
      tbSurname.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontStyleDelta");
      tbSurname.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbSurname.Properties.Appearance.GradientMode");
      tbSurname.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Image");
      tbSurname.Properties.Appearance.Options.UseFont = true;
      tbSurname.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbSurname.Properties.AutoHeight");
      tbSurname.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbSurname.Properties.Mask.AutoComplete");
      tbSurname.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.BeepOnError");
      tbSurname.Properties.Mask.EditMask = componentResourceManager.GetString("tbSurname.Properties.Mask.EditMask");
      tbSurname.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.IgnoreMaskBlank");
      tbSurname.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbSurname.Properties.Mask.MaskType");
      tbSurname.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbSurname.Properties.Mask.PlaceHolder");
      tbSurname.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.SaveLiteral");
      tbSurname.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.ShowPlaceHolders");
      tbSurname.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbSurname.Properties.Mask.UseMaskAsDisplayFormat");
      tbSurname.Properties.NullValuePrompt = componentResourceManager.GetString("tbSurname.Properties.NullValuePrompt");
      tbSurname.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbSurname.Properties.NullValuePromptShowForEmptyValue");
      tbSurname.Properties.ReadOnly = true;
      tbSurname.TextChanged += TbValueChanged;
      componentResourceManager.ApplyResources(lbSurname, "lbSurname");
      lbSurname.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.DisabledImage");
      lbSurname.Appearance.Font = (Font) componentResourceManager.GetObject("lbSurname.Appearance.Font");
      lbSurname.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSurname.Appearance.FontSizeDelta");
      lbSurname.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSurname.Appearance.FontStyleDelta");
      lbSurname.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSurname.Appearance.ForeColor");
      lbSurname.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSurname.Appearance.GradientMode");
      lbSurname.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.HoverImage");
      lbSurname.Appearance.Image = (Image) componentResourceManager.GetObject("lbSurname.Appearance.Image");
      lbSurname.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSurname.Appearance.PressedImage");
      lbSurname.Name = "lbSurname";
      componentResourceManager.ApplyResources(gcImagesFullFace, "gcImagesFullFace");
      gcImagesFullFace.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleDescription");
      gcImagesFullFace.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleName");
      gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip");
      gcImagesFullFace.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.Anchor");
      gcImagesFullFace.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImage");
      gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout");
      gcImagesFullFace.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ImeMode");
      gcImagesFullFace.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.MaximumSize");
      gcImagesFullFace.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.TextLocation");
      gcImagesFullFace.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.ToolTip");
      gcImagesFullFace.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ToolTipIconType");
      gcImagesFullFace.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.ToolTipTitle");
      gridLevelNode.RelationName = "Level1";
      gcImagesFullFace.LevelTree.Nodes.AddRange(new GridLevelNode[1]
      {
        gridLevelNode
      });
      gcImagesFullFace.MainView = lvImagesFullFace;
      gcImagesFullFace.Name = "gcImagesFullFace";
      gcImagesFullFace.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        repositoryItemButtonEdit1
      });
      gcImagesFullFace.ViewCollection.AddRange(new BaseView[2]
      {
        lvImagesFullFace,
        gridView1
      });
      componentResourceManager.ApplyResources(lvImagesFullFace, "lvImagesFullFace");
      lvImagesFullFace.CardHorzInterval = 0;
      lvImagesFullFace.CardMinSize = new Size(261, 352);
      lvImagesFullFace.CardVertInterval = 0;
      lvImagesFullFace.Columns.AddRange(new LayoutViewColumn[4]
      {
        colImage,
        colName,
        colImageComment,
        colImageID
      });
      lvImagesFullFace.GridControl = gcImagesFullFace;
      lvImagesFullFace.HiddenItems.AddRange(new BaseLayoutItem[1]
      {
        layoutViewField_layoutViewColumn1_1
      });
      lvImagesFullFace.Name = "lvImagesFullFace";
      lvImagesFullFace.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
      lvImagesFullFace.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
      lvImagesFullFace.OptionsBehavior.AllowExpandCollapse = false;
      lvImagesFullFace.OptionsBehavior.AutoFocusCardOnScrolling = true;
      lvImagesFullFace.OptionsBehavior.AutoFocusNewCard = true;
      lvImagesFullFace.OptionsBehavior.AutoPopulateColumns = false;
      lvImagesFullFace.OptionsBehavior.AutoSelectAllInEditor = false;
      lvImagesFullFace.OptionsBehavior.FocusLeaveOnTab = true;
      lvImagesFullFace.OptionsBehavior.KeepFocusedRowOnUpdate = false;
      lvImagesFullFace.OptionsCustomization.AllowFilter = false;
      lvImagesFullFace.OptionsCustomization.AllowSort = false;
      lvImagesFullFace.OptionsHeaderPanel.ShowCustomizeButton = false;
      lvImagesFullFace.OptionsHeaderPanel.ShowPanButton = false;
      lvImagesFullFace.OptionsItemText.AlignMode = FieldTextAlignMode.AutoSize;
      lvImagesFullFace.OptionsItemText.TextToControlDistance = 0;
      lvImagesFullFace.OptionsLayout.Columns.AddNewColumns = false;
      lvImagesFullFace.OptionsLayout.Columns.RemoveOldColumns = false;
      lvImagesFullFace.OptionsLayout.Columns.StoreLayout = false;
      lvImagesFullFace.OptionsLayout.StoreDataSettings = false;
      lvImagesFullFace.OptionsLayout.StoreVisualOptions = false;
      lvImagesFullFace.OptionsSelection.MultiSelect = true;
      lvImagesFullFace.OptionsView.AllowHotTrackFields = false;
      lvImagesFullFace.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      lvImagesFullFace.OptionsView.ShowCardExpandButton = false;
      lvImagesFullFace.OptionsView.ShowCardFieldBorders = true;
      lvImagesFullFace.OptionsView.ShowFieldHints = false;
      lvImagesFullFace.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
      lvImagesFullFace.OptionsView.ShowViewCaption = true;
      lvImagesFullFace.OptionsView.ViewMode = LayoutViewMode.Column;
      lvImagesFullFace.TemplateCard = layoutViewCard1;
      lvImagesFullFace.CustomDrawCardCaption += lvImagesFullFace_CustomDrawCardCaption;
      lvImagesFullFace.CellValueChanged += lvImagesFullFace_CellValueChanged;
      colImage.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceCell.Font");
      colImage.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceCell.FontSizeDelta");
      colImage.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceCell.FontStyleDelta");
      colImage.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceCell.GradientMode");
      colImage.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceCell.Image");
      colImage.AppearanceCell.Options.UseFont = true;
      colImage.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
      colImage.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceHeader.FontSizeDelta");
      colImage.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceHeader.FontStyleDelta");
      colImage.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceHeader.GradientMode");
      colImage.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImage.AppearanceHeader.Image");
      colImage.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colImage, "colImage");
      colImage.ColumnEdit = repositoryItemPictureEdit1;
      colImage.FieldName = "Image";
      colImage.LayoutViewField = layoutViewField_layoutViewColumn1;
      colImage.Name = "colImage";
      colImage.OptionsColumn.AllowMove = false;
      colImage.OptionsColumn.AllowShowHide = false;
      colImage.OptionsColumn.AllowSize = false;
      layoutViewField_layoutViewColumn1.EditorPreferredWidth = 252;
      layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
      layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
      layoutViewField_layoutViewColumn1.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn1.Size = new Size(259, 22);
      layoutViewField_layoutViewColumn1.TextSize = new Size(7, 13);
      colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      colName.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceCell.FontSizeDelta");
      colName.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceCell.FontStyleDelta");
      colName.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceCell.GradientMode");
      colName.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colName.AppearanceCell.Image");
      colName.AppearanceCell.Options.UseFont = true;
      colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      colName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
      colName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
      colName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
      colName.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colName.AppearanceHeader.Image");
      colName.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colName, "colName");
      colName.FieldName = "Name";
      colName.LayoutViewField = layoutViewField_layoutViewColumn2_1;
      colName.Name = "colName";
      colName.OptionsColumn.AllowMove = false;
      colName.OptionsColumn.AllowSize = false;
      layoutViewField_layoutViewColumn2_1.EditorPreferredWidth = 182;
      layoutViewField_layoutViewColumn2_1.Location = new Point(0, 22);
      layoutViewField_layoutViewColumn2_1.Name = "layoutViewField_layoutViewColumn2_1";
      layoutViewField_layoutViewColumn2_1.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn2_1.Size = new Size(259, 16);
      layoutViewField_layoutViewColumn2_1.TextSize = new Size(77, 13);
      colImageComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceCell.Font");
      colImageComment.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontSizeDelta");
      colImageComment.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontStyleDelta");
      colImageComment.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceCell.GradientMode");
      colImageComment.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colImageComment.AppearanceCell.Image");
      colImageComment.AppearanceCell.Options.UseFont = true;
      colImageComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Font");
      colImageComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontSizeDelta");
      colImageComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontStyleDelta");
      colImageComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceHeader.GradientMode");
      colImageComment.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Image");
      colImageComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colImageComment, "colImageComment");
      colImageComment.FieldName = "Comment";
      colImageComment.LayoutViewField = layoutViewField_layoutViewColumn2;
      colImageComment.Name = "colImageComment";
      colImageComment.OptionsColumn.AllowMove = false;
      colImageComment.OptionsColumn.AllowSize = false;
      layoutViewField_layoutViewColumn2.EditorPreferredWidth = 194;
      layoutViewField_layoutViewColumn2.Location = new Point(0, 38);
      layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
      layoutViewField_layoutViewColumn2.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn2.Size = new Size(259, 16);
      layoutViewField_layoutViewColumn2.TextSize = new Size(31, 13);
      componentResourceManager.ApplyResources(colImageID, "colImageID");
      colImageID.FieldName = "ImageID";
      colImageID.LayoutViewField = layoutViewField_layoutViewColumn1_1;
      colImageID.Name = "colImageID";
      colImageID.OptionsColumn.AllowMove = false;
      colImageID.OptionsColumn.AllowSize = false;
      layoutViewField_layoutViewColumn1_1.EditorPreferredWidth = 10;
      layoutViewField_layoutViewColumn1_1.Location = new Point(0, 0);
      layoutViewField_layoutViewColumn1_1.Name = "layoutViewField_layoutViewColumn1_1";
      layoutViewField_layoutViewColumn1_1.Padding = new Padding(0, 0, 0, 0);
      layoutViewField_layoutViewColumn1_1.Size = new Size(280, 54);
      layoutViewField_layoutViewColumn1_1.TextSize = new Size(77, 20);
      layoutViewField_layoutViewColumn1_1.TextToControlDistance = 0;
      componentResourceManager.ApplyResources(layoutViewCard1, "layoutViewCard1");
      layoutViewCard1.ExpandButtonLocation = GroupElementLocation.AfterText;
      layoutViewCard1.Items.AddRange(new BaseLayoutItem[3]
      {
        layoutViewField_layoutViewColumn1,
        layoutViewField_layoutViewColumn2_1,
        layoutViewField_layoutViewColumn2
      });
      layoutViewCard1.Name = "layoutViewCard1";
      layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
      layoutViewCard1.Padding = new Padding(0, 0, 0, 0);
      componentResourceManager.ApplyResources(repositoryItemButtonEdit1, "repositoryItemButtonEdit1");
      componentResourceManager.ApplyResources(appearanceObject, "serializableAppearanceObject1");
      repositoryItemButtonEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons"), componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons1"), (int) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons2"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons3"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons4"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons5"), (ImageLocation) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons6"), (Image) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons7"), new KeyShortcut(System.Windows.Forms.Keys.None), appearanceObject, componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons8"), componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons9"), (SuperToolTip) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons10"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons11"))
      });
      repositoryItemButtonEdit1.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.AutoComplete");
      repositoryItemButtonEdit1.Mask.BeepOnError = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.BeepOnError");
      repositoryItemButtonEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemButtonEdit1.Mask.EditMask");
      repositoryItemButtonEdit1.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.IgnoreMaskBlank");
      repositoryItemButtonEdit1.Mask.MaskType = (MaskType) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.MaskType");
      repositoryItemButtonEdit1.Mask.PlaceHolder = (char) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.PlaceHolder");
      repositoryItemButtonEdit1.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.SaveLiteral");
      repositoryItemButtonEdit1.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.ShowPlaceHolders");
      repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat");
      repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
      repositoryItemButtonEdit1.TextEditStyle = TextEditStyles.DisableTextEditor;
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.GridControl = gcImagesFullFace;
      gridView1.Name = "gridView1";
      componentResourceManager.ApplyResources(pictureEdit1, "pictureEdit1");
      pictureEdit1.Name = "pictureEdit1";
      pictureEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleDescription");
      pictureEdit1.Properties.AccessibleName = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleName");
      pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Appearance.Font = (Font) componentResourceManager.GetObject("groupControl1.Appearance.Font");
      groupControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.Appearance.FontSizeDelta");
      groupControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.Appearance.FontStyleDelta");
      groupControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.Appearance.GradientMode");
      groupControl1.Appearance.Image = (Image) componentResourceManager.GetObject("groupControl1.Appearance.Image");
      groupControl1.Appearance.Options.UseFont = true;
      groupControl1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Font");
      groupControl1.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontSizeDelta");
      groupControl1.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontStyleDelta");
      groupControl1.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.AppearanceCaption.GradientMode");
      groupControl1.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Image");
      groupControl1.AppearanceCaption.Options.UseFont = true;
      groupControl1.Controls.Add(pictureEdit1);
      groupControl1.Name = "groupControl1";
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(groupControl1);
      Controls.Add(gcImagesFullFace);
      Controls.Add(groupBox1);
      Name = "ShowFaceForm";
      WindowState = FormWindowState.Maximized;
      HelpButtonClicked += EditEmployerForm_HelpButtonClicked;
      FormClosing += EditEmployerForm_FormClosing;
      Load += EditEmployerForm_Load;
      HelpRequested += EditEmployerForm_HelpRequested;
      repositoryItemPictureEdit1.EndInit();
      repositoryItemCheckEdit1.EndInit();
      groupBox1.EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      tbAddress.Properties.EndInit();
      tbComment.Properties.EndInit();
      dtpBithday.Properties.CalendarTimeProperties.EndInit();
      dtpBithday.Properties.EndInit();
      tbPassport.Properties.EndInit();
      cbAccessTemplate.Properties.EndInit();
      cbSEX.Properties.EndInit();
      tbLastName.Properties.EndInit();
      tbFirstName.Properties.EndInit();
      tbSurname.Properties.EndInit();
      gcImagesFullFace.EndInit();
      lvImagesFullFace.EndInit();
      layoutViewField_layoutViewColumn1.EndInit();
      layoutViewField_layoutViewColumn2_1.EndInit();
      layoutViewField_layoutViewColumn2.EndInit();
      layoutViewField_layoutViewColumn1_1.EndInit();
      layoutViewCard1.EndInit();
      repositoryItemButtonEdit1.EndInit();
      gridView1.EndInit();
      pictureEdit1.Properties.EndInit();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      ResumeLayout(false);
    }
  }
}
