// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmEditFace
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using BasicComponents;
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
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Events;
using DevExpress.XtraLayout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding.Utils;

namespace CascadeFlowClient
{
  public class FrmEditFace : XtraForm
  {
    public string Category = Messages.NonCategory;
    public BcFace CurrentEmployer = new BcFace();
    public List<BcKey> Keys = new List<BcKey>();
    public List<BcKey> CurrentKeys = new List<BcKey>();
    public List<BcKeySettings> KeySettings = new List<BcKeySettings>();
    public List<BcImage> Images = new List<BcImage>();
    private List<BcDepartment> _deps = new List<BcDepartment>();
    private List<BcOrganization> _orgs = new List<BcOrganization>();
    private List<BcTiming> _timinList = new List<BcTiming>();
    private DataTable _dtImages = new DataTable();
    private List<BcAccessCategory> _templateList = new List<BcAccessCategory>();
    private IContainer components = (IContainer) null;
    private bool _cancelFlag;
    private bool _flagAfterAdd;
    public bool AddNewValue;
    private bool _isloading;
    private bool _changedValues;
    private GroupControl groupBox1;
    private LabelControl lbSurname;
    private TextEdit tbSurname;
    private LabelControl lbBirthday;
    private TextEdit tbLastName;
    private LabelControl lbLastName;
    private TextEdit tbFirstName;
    private LabelControl lbFirstName;
    private LabelControl label3;
    private LabelControl lbComment;
    private LabelControl lbSex;
    private SimpleButton btCancel;
    private SimpleButton btSave;
    private PageSetupDialog pageSetupDialog1;
    private ComboBoxEdit cbAccessTemplate;
    private ComboBoxEdit cbSEX;
    private DateEdit dtpBithday;
    private MemoEdit tbComment;
    private GridControl gcImagesFullFace;
    private LayoutView lvImagesFullFace;
    private LayoutViewColumn colImage;
    private LayoutViewColumn colName;
    private LayoutViewColumn colImageComment;
    private LayoutViewColumn colImageID;
    private LayoutViewColumn colIsMain;
    private GridView gridView1;
    private RepositoryItemButtonEdit repositoryItemButtonEdit1;
    private RepositoryItemPictureEdit repositoryItemPictureEdit1;
    private RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private RepositoryItemButtonEdit btDeleteImage;
    private LayoutViewField layoutViewField_colImage;
    private LayoutViewField layoutViewField_colName;
    private LayoutViewField layoutViewField_colImageComment;
    private LayoutViewField layoutViewField_colImageID;
    private LayoutViewField layoutViewField_colIsMain;
    private LayoutViewCard layoutViewCard1;
    private SimpleButton btDelete;

    public FrmEditFace()
    {
      this.InitializeComponent();
    }

    private void LoadImages(List<BcImage> images)
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
      foreach (BcImage bcImage in images)
      {
        Bitmap bitmap1 = new Bitmap((Stream) new MemoryStream(bcImage.ImageIcon));
        Bitmap bitmap2 = new Bitmap((Stream) new MemoryStream(bcImage.Image));
        this._dtImages.Rows.Add((object) Guid.Empty, (object) bitmap1, (object) bitmap2, (object) bcImage.Name, (object) bcImage.Comment, (object) (bool) (bcImage.IsMain ? 1 : 0), (object) true, (object) true);
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
      this._cancelFlag = false;
      this.KeySettings = BcKeySettings.LoadAll();
      if (this.CurrentEmployer.Id == Guid.Empty)
        this._flagAfterAdd = true;
      this._changedValues = false;
      try
      {
        if (this.dtpBithday.DateTime == DateTime.MinValue)
          this.dtpBithday.DateTime = new DateTime(1900, 1, 1);
        this.CurrentEmployer.Birthday = this.dtpBithday.DateTime;
      }
      catch
      {
      }
      this.CurrentEmployer.FirstName = this.tbFirstName.Text;
      this.CurrentEmployer.Surname = this.tbSurname.Text;
      this.CurrentEmployer.LastName = this.tbLastName.Text;
      this.CurrentEmployer.Comment = this.tbComment.Text;
      this.CurrentEmployer.Sex = this.cbSEX.SelectedIndex;
      this.CurrentEmployer.EditUserId = FrmImages.CurrentUser.Id;
      if (this.cbAccessTemplate.SelectedIndex != -1)
        this.CurrentEmployer.AccessId = this._templateList[this.cbAccessTemplate.SelectedIndex].Id;
      this.Category = this.cbAccessTemplate.Text;
      bool flag = false;
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._dtImages.Rows)
      {
        if ((bool) dataRow["IsMain"])
        {
          flag = true;
          Bitmap bitmap = new Bitmap((System.Drawing.Image) dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image) dataRow["Image"]).Height / ((System.Drawing.Image) dataRow["Image"]).Width));
          MemoryStream memoryStream = new MemoryStream();
          bitmap.Save((Stream) memoryStream, ImageFormat.Jpeg);
          this.CurrentEmployer.ImageIcon = (byte[]) memoryStream.GetBuffer().Clone();
          break;
        }
      }
      if (!flag)
      {
        if (this._dtImages.Rows.Count == 1)
        {
          IEnumerator enumerator = this._dtImages.Rows.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
            {
              DataRow dataRow = (DataRow) enumerator.Current;
              Bitmap bitmap = new Bitmap((System.Drawing.Image) dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image) dataRow["Image"]).Height / ((System.Drawing.Image) dataRow["Image"]).Width));
              dataRow["IsMain"] = (object) true;
              MemoryStream memoryStream = new MemoryStream();
              bitmap.Save((Stream) memoryStream, ImageFormat.Jpeg);
              this.CurrentEmployer.ImageIcon = (byte[]) memoryStream.GetBuffer().Clone();
            }
          }
          finally
          {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
              disposable.Dispose();
          }
        }
        else
        {
          int num = (int) XtraMessageBox.Show(Messages.SetMainPhoto, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this._cancelFlag = true;
          return;
        }
      }
      this.CurrentEmployer.Save();
      foreach (DataRow dataRow in (InternalDataCollectionBase) this._dtImages.Rows)
      {
        if ((bool) dataRow["Changed"])
        {
          BcImage bcImage = new BcImage();
          bcImage.Id = (Guid) dataRow["ID"];
          MemoryStream memoryStream1 = new MemoryStream();
          ((System.Drawing.Image) dataRow["Image"]).Save((Stream) memoryStream1, ImageFormat.Jpeg);
          bcImage.Image = memoryStream1.GetBuffer();
          Bitmap bitmap = new Bitmap((System.Drawing.Image) dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image) dataRow["Image"]).Height / ((System.Drawing.Image) dataRow["Image"]).Width));
          MemoryStream memoryStream2 = new MemoryStream();
          bitmap.Save((Stream) memoryStream2, ImageFormat.Jpeg);
          bcImage.ImageIcon = memoryStream2.GetBuffer();
          bcImage.IsMain = (bool) dataRow["IsMain"];
          bcImage.Name = dataRow["Name"].ToString();
          bcImage.Comment = dataRow["Comment"].ToString();
          bcImage.FaceId = this.CurrentEmployer.Id;
          bcImage.Save();
          Bitmap source = (Bitmap) dataRow["Image"];
          if ((bool) dataRow["ImageChanged"])
          {
            TS.Sdk.StaticFace.Model.Image image = ModelImageConverter.ConvertFrom(source);
            FaceInfo face = FrmImages.MainDetector.DetectMaxFace(image, (DetectionConfiguration) null);
            BcKey bcKey = new BcKey()
            {
              ImageId = bcImage.Id
            };
            bcKey.DeleteByImageId();
            if (face != null)
            {
              byte[] template = FrmImages.MainDetector.ExtractTemplate(image, face);
              bcKey.FaceId = this.CurrentEmployer.Id;
              bcKey.ImageKey = template;
              bcKey.Ksid = -1;
              bcKey.FaceId = this.CurrentEmployer.Id;
              bcKey.Save();
            }
          }
        }
        else
        {
          BcImage bcImage = new BcImage();
          bcImage.Id = (Guid) dataRow["ID"];
          MemoryStream memoryStream1 = new MemoryStream();
          ((System.Drawing.Image) dataRow["Image"]).Save((Stream) memoryStream1, ImageFormat.Jpeg);
          bcImage.Image = memoryStream1.GetBuffer();
          Bitmap bitmap = new Bitmap((System.Drawing.Image) dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image) dataRow["Image"]).Height / ((System.Drawing.Image) dataRow["Image"]).Width));
          MemoryStream memoryStream2 = new MemoryStream();
          bitmap.Save((Stream) memoryStream2, ImageFormat.Jpeg);
          bcImage.ImageIcon = memoryStream2.GetBuffer();
          bcImage.IsMain = (bool) dataRow["IsMain"];
          bcImage.Name = dataRow["Name"].ToString();
          bcImage.Comment = dataRow["Comment"].ToString();
          bcImage.FaceId = this.CurrentEmployer.Id;
          bcImage.Save();
        }
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      this.FindChanges();
      this.Close();
    }

    private void EditEmployerForm_Load(object sender, EventArgs e)
    {
      this.dtpBithday.DateTime = DateTime.Now;
      this.cbSEX.SelectedIndex = 0;
      this._isloading = true;
      this._changedValues = false;
      this._templateList = BcAccessCategory.LoadAll();
      foreach (BcAccessCategory bcAccessCategory in this._templateList)
        this.cbAccessTemplate.Properties.Items.Add((object) bcAccessCategory.Name);
      if (this.cbAccessTemplate.Properties.Items.Count > 0)
        this.cbAccessTemplate.SelectedIndex = 0;
      this.LoadImages(this.Images);
      this.gcImagesFullFace.DataSource = (object) this._dtImages;
      this._isloading = false;
    }

    private void EditEmployerForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
        this._cancelFlag = false;
      if (this._cancelFlag)
        e.Cancel = true;
      else if (this._flagAfterAdd)
        this.AddNewValue = false;
      else
        this.FindChanges();
    }

    private void TbValueChanged(object sender, EventArgs e)
    {
      if (this._isloading)
        return;
      this._changedValues = true;
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
      if (this._isloading || e.Column != this.colIsMain || !(bool) e.Value)
        return;
      DataRow dataRow1 = this.lvImagesFullFace.GetDataRow(e.RowHandle);
      foreach (DataRow dataRow2 in (InternalDataCollectionBase) this._dtImages.Rows)
      {
        if (dataRow1 != dataRow2)
          dataRow2["IsMain"] = (object) false;
      }
    }

    private void btDeleteImage_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      if (this.lvImagesFullFace.FocusedRowHandle < 0)
        return;
      this._dtImages.Rows.Remove(this.lvImagesFullFace.GetDataRow(this.lvImagesFullFace.FocusedRowHandle));
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      this.lvImagesFullFace.DeleteSelectedRows();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmEditFace));
      GridLevelNode gridLevelNode = new GridLevelNode();
      SerializableAppearanceObject appearanceObject1 = new SerializableAppearanceObject();
      SerializableAppearanceObject appearanceObject2 = new SerializableAppearanceObject();
      this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
      this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      this.groupBox1 = new GroupControl();
      this.tbComment = new MemoEdit();
      this.dtpBithday = new DateEdit();
      this.btCancel = new SimpleButton();
      this.lbBirthday = new LabelControl();
      this.btSave = new SimpleButton();
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
      this.layoutViewField_colImage = new LayoutViewField();
      this.colName = new LayoutViewColumn();
      this.layoutViewField_colName = new LayoutViewField();
      this.colImageComment = new LayoutViewColumn();
      this.layoutViewField_colImageComment = new LayoutViewField();
      this.colImageID = new LayoutViewColumn();
      this.layoutViewField_colImageID = new LayoutViewField();
      this.colIsMain = new LayoutViewColumn();
      this.layoutViewField_colIsMain = new LayoutViewField();
      this.layoutViewCard1 = new LayoutViewCard();
      this.repositoryItemButtonEdit1 = new RepositoryItemButtonEdit();
      this.btDeleteImage = new RepositoryItemButtonEdit();
      this.gridView1 = new GridView();
      this.btDelete = new SimpleButton();
      this.repositoryItemPictureEdit1.BeginInit();
      this.repositoryItemCheckEdit1.BeginInit();
      this.groupBox1.BeginInit();
      this.groupBox1.SuspendLayout();
      this.tbComment.Properties.BeginInit();
      this.dtpBithday.Properties.CalendarTimeProperties.BeginInit();
      this.dtpBithday.Properties.BeginInit();
      this.cbAccessTemplate.Properties.BeginInit();
      this.cbSEX.Properties.BeginInit();
      this.tbLastName.Properties.BeginInit();
      this.tbFirstName.Properties.BeginInit();
      this.tbSurname.Properties.BeginInit();
      this.gcImagesFullFace.BeginInit();
      this.lvImagesFullFace.BeginInit();
      this.layoutViewField_colImage.BeginInit();
      this.layoutViewField_colName.BeginInit();
      this.layoutViewField_colImageComment.BeginInit();
      this.layoutViewField_colImageID.BeginInit();
      this.layoutViewField_colIsMain.BeginInit();
      this.layoutViewCard1.BeginInit();
      this.repositoryItemButtonEdit1.BeginInit();
      this.btDeleteImage.BeginInit();
      this.gridView1.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
      this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
      this.repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.Image;
      this.repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this.repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      this.repositoryItemCheckEdit1.AutoWidth = true;
      this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
      this.groupBox1.Controls.Add((Control) this.tbComment);
      this.groupBox1.Controls.Add((Control) this.dtpBithday);
      this.groupBox1.Controls.Add((Control) this.btCancel);
      this.groupBox1.Controls.Add((Control) this.lbBirthday);
      this.groupBox1.Controls.Add((Control) this.btSave);
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
      componentResourceManager.ApplyResources((object) this.tbComment, "tbComment");
      this.tbComment.Name = "tbComment";
      this.tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      this.tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      this.tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      this.tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources((object) this.dtpBithday, "dtpBithday");
      this.dtpBithday.Name = "dtpBithday";
      this.dtpBithday.Properties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.AccessibleDescription");
      this.dtpBithday.Properties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.AccessibleName");
      this.dtpBithday.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Font");
      this.dtpBithday.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontSizeDelta");
      this.dtpBithday.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontStyleDelta");
      this.dtpBithday.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.GradientMode");
      this.dtpBithday.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Image");
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
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      this.btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      this.btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      this.btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      this.btCancel.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      this.btCancel.Appearance.Options.UseFont = true;
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      componentResourceManager.ApplyResources((object) this.lbBirthday, "lbBirthday");
      this.lbBirthday.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbBirthday.Appearance.DisabledImage");
      this.lbBirthday.Appearance.Font = (Font) componentResourceManager.GetObject("lbBirthday.Appearance.Font");
      this.lbBirthday.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbBirthday.Appearance.FontSizeDelta");
      this.lbBirthday.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbBirthday.Appearance.FontStyleDelta");
      this.lbBirthday.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbBirthday.Appearance.ForeColor");
      this.lbBirthday.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbBirthday.Appearance.GradientMode");
      this.lbBirthday.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbBirthday.Appearance.HoverImage");
      this.lbBirthday.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbBirthday.Appearance.Image");
      this.lbBirthday.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbBirthday.Appearance.PressedImage");
      this.lbBirthday.Name = "lbBirthday";
      componentResourceManager.ApplyResources((object) this.btSave, "btSave");
      this.btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      this.btSave.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.Appearance.FontSizeDelta");
      this.btSave.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.Appearance.FontStyleDelta");
      this.btSave.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btSave.Appearance.GradientMode");
      this.btSave.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("btSave.Appearance.Image");
      this.btSave.Appearance.Options.UseFont = true;
      this.btSave.DialogResult = DialogResult.OK;
      this.btSave.Name = "btSave";
      this.btSave.Click += new EventHandler(this.btSave_Click);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("label3.Appearance.DisabledImage");
      this.label3.Appearance.Font = (Font) componentResourceManager.GetObject("label3.Appearance.Font");
      this.label3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label3.Appearance.FontSizeDelta");
      this.label3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label3.Appearance.FontStyleDelta");
      this.label3.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label3.Appearance.ForeColor");
      this.label3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label3.Appearance.GradientMode");
      this.label3.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("label3.Appearance.HoverImage");
      this.label3.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("label3.Appearance.Image");
      this.label3.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("label3.Appearance.PressedImage");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.cbAccessTemplate, "cbAccessTemplate");
      this.cbAccessTemplate.Name = "cbAccessTemplate";
      this.cbAccessTemplate.Properties.AccessibleDescription = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleDescription");
      this.cbAccessTemplate.Properties.AccessibleName = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleName");
      this.cbAccessTemplate.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Font");
      this.cbAccessTemplate.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontSizeDelta");
      this.cbAccessTemplate.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontStyleDelta");
      this.cbAccessTemplate.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.GradientMode");
      this.cbAccessTemplate.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Image");
      this.cbAccessTemplate.Properties.Appearance.Options.UseFont = true;
      this.cbAccessTemplate.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.AutoHeight");
      this.cbAccessTemplate.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbAccessTemplate.Properties.Buttons"))
      });
      this.cbAccessTemplate.Properties.NullValuePrompt = componentResourceManager.GetString("cbAccessTemplate.Properties.NullValuePrompt");
      this.cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue");
      this.cbAccessTemplate.SelectedIndexChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbComment, "lbComment");
      this.lbComment.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbComment.Appearance.DisabledImage");
      this.lbComment.Appearance.Font = (Font) componentResourceManager.GetObject("lbComment.Appearance.Font");
      this.lbComment.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbComment.Appearance.FontSizeDelta");
      this.lbComment.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbComment.Appearance.FontStyleDelta");
      this.lbComment.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbComment.Appearance.ForeColor");
      this.lbComment.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbComment.Appearance.GradientMode");
      this.lbComment.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbComment.Appearance.HoverImage");
      this.lbComment.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbComment.Appearance.Image");
      this.lbComment.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbComment.Appearance.PressedImage");
      this.lbComment.Name = "lbComment";
      componentResourceManager.ApplyResources((object) this.lbSex, "lbSex");
      this.lbSex.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSex.Appearance.DisabledImage");
      this.lbSex.Appearance.Font = (Font) componentResourceManager.GetObject("lbSex.Appearance.Font");
      this.lbSex.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSex.Appearance.FontSizeDelta");
      this.lbSex.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSex.Appearance.FontStyleDelta");
      this.lbSex.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSex.Appearance.ForeColor");
      this.lbSex.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSex.Appearance.GradientMode");
      this.lbSex.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSex.Appearance.HoverImage");
      this.lbSex.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbSex.Appearance.Image");
      this.lbSex.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSex.Appearance.PressedImage");
      this.lbSex.Name = "lbSex";
      componentResourceManager.ApplyResources((object) this.cbSEX, "cbSEX");
      this.cbSEX.Name = "cbSEX";
      this.cbSEX.Properties.AccessibleDescription = componentResourceManager.GetString("cbSEX.Properties.AccessibleDescription");
      this.cbSEX.Properties.AccessibleName = componentResourceManager.GetString("cbSEX.Properties.AccessibleName");
      this.cbSEX.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Font");
      this.cbSEX.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontSizeDelta");
      this.cbSEX.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontStyleDelta");
      this.cbSEX.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbSEX.Properties.Appearance.GradientMode");
      this.cbSEX.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Image");
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
      this.cbSEX.SelectedIndexChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.tbLastName, "tbLastName");
      this.tbLastName.Name = "tbLastName";
      this.tbLastName.Properties.AccessibleDescription = componentResourceManager.GetString("tbLastName.Properties.AccessibleDescription");
      this.tbLastName.Properties.AccessibleName = componentResourceManager.GetString("tbLastName.Properties.AccessibleName");
      this.tbLastName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Font");
      this.tbLastName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontSizeDelta");
      this.tbLastName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontStyleDelta");
      this.tbLastName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbLastName.Properties.Appearance.GradientMode");
      this.tbLastName.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("tbLastName.Properties.Appearance.Image");
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
      this.tbLastName.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbLastName, "lbLastName");
      this.lbLastName.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbLastName.Appearance.DisabledImage");
      this.lbLastName.Appearance.Font = (Font) componentResourceManager.GetObject("lbLastName.Appearance.Font");
      this.lbLastName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbLastName.Appearance.FontSizeDelta");
      this.lbLastName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbLastName.Appearance.FontStyleDelta");
      this.lbLastName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbLastName.Appearance.ForeColor");
      this.lbLastName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbLastName.Appearance.GradientMode");
      this.lbLastName.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbLastName.Appearance.HoverImage");
      this.lbLastName.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbLastName.Appearance.Image");
      this.lbLastName.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbLastName.Appearance.PressedImage");
      this.lbLastName.Name = "lbLastName";
      componentResourceManager.ApplyResources((object) this.tbFirstName, "tbFirstName");
      this.tbFirstName.Name = "tbFirstName";
      this.tbFirstName.Properties.AccessibleDescription = componentResourceManager.GetString("tbFirstName.Properties.AccessibleDescription");
      this.tbFirstName.Properties.AccessibleName = componentResourceManager.GetString("tbFirstName.Properties.AccessibleName");
      this.tbFirstName.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Font");
      this.tbFirstName.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontSizeDelta");
      this.tbFirstName.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontStyleDelta");
      this.tbFirstName.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.GradientMode");
      this.tbFirstName.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Image");
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
      this.tbFirstName.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbFirstName, "lbFirstName");
      this.lbFirstName.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbFirstName.Appearance.DisabledImage");
      this.lbFirstName.Appearance.Font = (Font) componentResourceManager.GetObject("lbFirstName.Appearance.Font");
      this.lbFirstName.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbFirstName.Appearance.FontSizeDelta");
      this.lbFirstName.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbFirstName.Appearance.FontStyleDelta");
      this.lbFirstName.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbFirstName.Appearance.ForeColor");
      this.lbFirstName.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbFirstName.Appearance.GradientMode");
      this.lbFirstName.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbFirstName.Appearance.HoverImage");
      this.lbFirstName.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbFirstName.Appearance.Image");
      this.lbFirstName.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbFirstName.Appearance.PressedImage");
      this.lbFirstName.Name = "lbFirstName";
      componentResourceManager.ApplyResources((object) this.tbSurname, "tbSurname");
      this.tbSurname.Name = "tbSurname";
      this.tbSurname.Properties.AccessibleDescription = componentResourceManager.GetString("tbSurname.Properties.AccessibleDescription");
      this.tbSurname.Properties.AccessibleName = componentResourceManager.GetString("tbSurname.Properties.AccessibleName");
      this.tbSurname.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Font");
      this.tbSurname.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontSizeDelta");
      this.tbSurname.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontStyleDelta");
      this.tbSurname.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbSurname.Properties.Appearance.GradientMode");
      this.tbSurname.Properties.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("tbSurname.Properties.Appearance.Image");
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
      this.tbSurname.TextChanged += new EventHandler(this.TbValueChanged);
      componentResourceManager.ApplyResources((object) this.lbSurname, "lbSurname");
      this.lbSurname.Appearance.DisabledImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSurname.Appearance.DisabledImage");
      this.lbSurname.Appearance.Font = (Font) componentResourceManager.GetObject("lbSurname.Appearance.Font");
      this.lbSurname.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSurname.Appearance.FontSizeDelta");
      this.lbSurname.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSurname.Appearance.FontStyleDelta");
      this.lbSurname.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSurname.Appearance.ForeColor");
      this.lbSurname.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSurname.Appearance.GradientMode");
      this.lbSurname.Appearance.HoverImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSurname.Appearance.HoverImage");
      this.lbSurname.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("lbSurname.Appearance.Image");
      this.lbSurname.Appearance.PressedImage = (System.Drawing.Image) componentResourceManager.GetObject("lbSurname.Appearance.PressedImage");
      this.lbSurname.Name = "lbSurname";
      componentResourceManager.ApplyResources((object) this.gcImagesFullFace, "gcImagesFullFace");
      this.gcImagesFullFace.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleDescription");
      this.gcImagesFullFace.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleName");
      this.gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip");
      this.gcImagesFullFace.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.Anchor");
      this.gcImagesFullFace.EmbeddedNavigator.BackgroundImage = (System.Drawing.Image) componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImage");
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
      this.gcImagesFullFace.RepositoryItems.AddRange(new RepositoryItem[2]
      {
        (RepositoryItem) this.repositoryItemButtonEdit1,
        (RepositoryItem) this.btDeleteImage
      });
      this.gcImagesFullFace.ViewCollection.AddRange(new BaseView[2]
      {
        (BaseView) this.lvImagesFullFace,
        (BaseView) this.gridView1
      });
      componentResourceManager.ApplyResources((object) this.lvImagesFullFace, "lvImagesFullFace");
      this.lvImagesFullFace.CardHorzInterval = 0;
      this.lvImagesFullFace.CardMinSize = new Size(355, 370);
      this.lvImagesFullFace.CardVertInterval = 0;
      this.lvImagesFullFace.Columns.AddRange(new LayoutViewColumn[5]
      {
        this.colImage,
        this.colName,
        this.colImageComment,
        this.colImageID,
        this.colIsMain
      });
      this.lvImagesFullFace.GridControl = this.gcImagesFullFace;
      this.lvImagesFullFace.HiddenItems.AddRange(new BaseLayoutItem[1]
      {
        (BaseLayoutItem) this.layoutViewField_colImageID
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
      this.lvImagesFullFace.OptionsView.ViewMode = LayoutViewMode.Carousel;
      this.lvImagesFullFace.SortInfo.AddRange(new GridColumnSortInfo[1]
      {
        new GridColumnSortInfo((GridColumn) this.colIsMain, ColumnSortOrder.Descending)
      });
      this.lvImagesFullFace.TemplateCard = this.layoutViewCard1;
      this.lvImagesFullFace.CustomDrawCardCaption += new LayoutViewCustomDrawCardCaptionEventHandler(this.lvImagesFullFace_CustomDrawCardCaption);
      this.lvImagesFullFace.CellValueChanged += new CellValueChangedEventHandler(this.lvImagesFullFace_CellValueChanged);
      this.colImage.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceCell.Font");
      this.colImage.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceCell.FontSizeDelta");
      this.colImage.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceCell.FontStyleDelta");
      this.colImage.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceCell.GradientMode");
      this.colImage.AppearanceCell.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImage.AppearanceCell.Image");
      this.colImage.AppearanceCell.Options.UseFont = true;
      this.colImage.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
      this.colImage.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImage.AppearanceHeader.FontSizeDelta");
      this.colImage.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImage.AppearanceHeader.FontStyleDelta");
      this.colImage.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImage.AppearanceHeader.GradientMode");
      this.colImage.AppearanceHeader.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImage.AppearanceHeader.Image");
      this.colImage.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colImage, "colImage");
      this.colImage.ColumnEdit = (RepositoryItem) this.repositoryItemPictureEdit1;
      this.colImage.FieldName = "Image";
      this.colImage.LayoutViewField = this.layoutViewField_colImage;
      this.colImage.Name = "colImage";
      this.colImage.OptionsColumn.AllowShowHide = false;
      this.layoutViewField_colImage.EditorPreferredWidth = 353;
      this.layoutViewField_colImage.ImageAlignment = ContentAlignment.MiddleCenter;
      this.layoutViewField_colImage.Location = new Point(0, 0);
      this.layoutViewField_colImage.MaxSize = new Size(343, 284);
      this.layoutViewField_colImage.MinSize = new Size(343, 284);
      this.layoutViewField_colImage.Name = "layoutViewField_colImage";
      this.layoutViewField_colImage.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_colImage.Size = new Size(349, 284);
      this.layoutViewField_colImage.SizeConstraintsType = SizeConstraintsType.Custom;
      this.layoutViewField_colImage.TextSize = new Size(0, 0);
      this.layoutViewField_colImage.TextToControlDistance = 0;
      this.layoutViewField_colImage.TextVisible = false;
      this.colName.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colName.AppearanceCell.Font");
      this.colName.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceCell.FontSizeDelta");
      this.colName.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceCell.FontStyleDelta");
      this.colName.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceCell.GradientMode");
      this.colName.AppearanceCell.Image = (System.Drawing.Image) componentResourceManager.GetObject("colName.AppearanceCell.Image");
      this.colName.AppearanceCell.Options.UseFont = true;
      this.colName.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colName.AppearanceHeader.Font");
      this.colName.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
      this.colName.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
      this.colName.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
      this.colName.AppearanceHeader.Image = (System.Drawing.Image) componentResourceManager.GetObject("colName.AppearanceHeader.Image");
      this.colName.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colName, "colName");
      this.colName.FieldName = "Name";
      this.colName.LayoutViewField = this.layoutViewField_colName;
      this.colName.Name = "colName";
      this.colName.OptionsColumn.AllowMove = false;
      this.colName.OptionsColumn.AllowSize = false;
      this.layoutViewField_colName.EditorPreferredWidth = 276;
      this.layoutViewField_colName.Location = new Point(0, 284);
      this.layoutViewField_colName.Name = "layoutViewField_colName";
      this.layoutViewField_colName.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_colName.Size = new Size(349, 16);
      this.layoutViewField_colName.TextSize = new Size(31, 13);
      this.layoutViewField_colName.TextToControlDistance = 0;
      this.colImageComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceCell.Font");
      this.colImageComment.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontSizeDelta");
      this.colImageComment.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceCell.FontStyleDelta");
      this.colImageComment.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceCell.GradientMode");
      this.colImageComment.AppearanceCell.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImageComment.AppearanceCell.Image");
      this.colImageComment.AppearanceCell.Options.UseFont = true;
      this.colImageComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Font");
      this.colImageComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontSizeDelta");
      this.colImageComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontStyleDelta");
      this.colImageComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageComment.AppearanceHeader.GradientMode");
      this.colImageComment.AppearanceHeader.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImageComment.AppearanceHeader.Image");
      this.colImageComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colImageComment, "colImageComment");
      this.colImageComment.FieldName = "Comment";
      this.colImageComment.LayoutViewField = this.layoutViewField_colImageComment;
      this.colImageComment.Name = "colImageComment";
      this.colImageComment.OptionsColumn.AllowMove = false;
      this.colImageComment.OptionsColumn.AllowSize = false;
      this.layoutViewField_colImageComment.EditorPreferredWidth = 288;
      this.layoutViewField_colImageComment.Location = new Point(0, 300);
      this.layoutViewField_colImageComment.Name = "layoutViewField_colImageComment";
      this.layoutViewField_colImageComment.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_colImageComment.Size = new Size(349, 16);
      this.layoutViewField_colImageComment.TextSize = new Size(49, 13);
      this.layoutViewField_colImageComment.TextToControlDistance = 0;
      this.colImageID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colImageID.AppearanceCell.Font");
      this.colImageID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colImageID.AppearanceCell.FontSizeDelta");
      this.colImageID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageID.AppearanceCell.FontStyleDelta");
      this.colImageID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageID.AppearanceCell.GradientMode");
      this.colImageID.AppearanceCell.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImageID.AppearanceCell.Image");
      this.colImageID.AppearanceCell.Options.UseFont = true;
      this.colImageID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colImageID.AppearanceHeader.Font");
      this.colImageID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colImageID.AppearanceHeader.FontSizeDelta");
      this.colImageID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colImageID.AppearanceHeader.FontStyleDelta");
      this.colImageID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colImageID.AppearanceHeader.GradientMode");
      this.colImageID.AppearanceHeader.Image = (System.Drawing.Image) componentResourceManager.GetObject("colImageID.AppearanceHeader.Image");
      this.colImageID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colImageID, "colImageID");
      this.colImageID.FieldName = "ImageID";
      this.colImageID.LayoutViewField = this.layoutViewField_colImageID;
      this.colImageID.Name = "colImageID";
      this.colImageID.OptionsColumn.AllowMove = false;
      this.colImageID.OptionsColumn.AllowSize = false;
      this.layoutViewField_colImageID.EditorPreferredWidth = 20;
      this.layoutViewField_colImageID.Location = new Point(0, 0);
      this.layoutViewField_colImageID.Name = "layoutViewField_colImageID";
      this.layoutViewField_colImageID.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewField_colImageID.Size = new Size(353, 348);
      this.layoutViewField_colImageID.TextSize = new Size(77, 20);
      this.layoutViewField_colImageID.TextToControlDistance = 0;
      this.colIsMain.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colIsMain.AppearanceCell.Font");
      this.colIsMain.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colIsMain.AppearanceCell.FontSizeDelta");
      this.colIsMain.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIsMain.AppearanceCell.FontStyleDelta");
      this.colIsMain.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIsMain.AppearanceCell.GradientMode");
      this.colIsMain.AppearanceCell.Image = (System.Drawing.Image) componentResourceManager.GetObject("colIsMain.AppearanceCell.Image");
      this.colIsMain.AppearanceCell.Options.UseFont = true;
      this.colIsMain.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colIsMain.AppearanceHeader.Font");
      this.colIsMain.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colIsMain.AppearanceHeader.FontSizeDelta");
      this.colIsMain.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIsMain.AppearanceHeader.FontStyleDelta");
      this.colIsMain.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIsMain.AppearanceHeader.GradientMode");
      this.colIsMain.AppearanceHeader.Image = (System.Drawing.Image) componentResourceManager.GetObject("colIsMain.AppearanceHeader.Image");
      this.colIsMain.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this.colIsMain, "colIsMain");
      this.colIsMain.ColumnEdit = (RepositoryItem) this.repositoryItemCheckEdit1;
      this.colIsMain.FieldName = "IsMain";
      this.colIsMain.LayoutViewField = this.layoutViewField_colIsMain;
      this.colIsMain.Name = "colIsMain";
      this.colIsMain.OptionsColumn.AllowMove = false;
      this.colIsMain.OptionsColumn.AllowSize = false;
      this.colIsMain.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
      this.layoutViewField_colIsMain.EditorPreferredWidth = 274;
      this.layoutViewField_colIsMain.Location = new Point(0, 316);
      this.layoutViewField_colIsMain.Name = "layoutViewField_colIsMain";
      this.layoutViewField_colIsMain.Size = new Size(349, 20);
      this.layoutViewField_colIsMain.TextSize = new Size(71, 13);
      this.layoutViewField_colIsMain.TextToControlDistance = 0;
      componentResourceManager.ApplyResources((object) this.layoutViewCard1, "layoutViewCard1");
      this.layoutViewCard1.ExpandButtonLocation = GroupElementLocation.AfterText;
      this.layoutViewCard1.Items.AddRange(new BaseLayoutItem[4]
      {
        (BaseLayoutItem) this.layoutViewField_colImage,
        (BaseLayoutItem) this.layoutViewField_colName,
        (BaseLayoutItem) this.layoutViewField_colImageComment,
        (BaseLayoutItem) this.layoutViewField_colIsMain
      });
      this.layoutViewCard1.Name = "layoutViewCard1";
      this.layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
      this.layoutViewCard1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
      this.layoutViewCard1.TextLocation = Locations.Default;
      componentResourceManager.ApplyResources((object) this.repositoryItemButtonEdit1, "repositoryItemButtonEdit1");
      componentResourceManager.ApplyResources((object) appearanceObject1, "serializableAppearanceObject1");
      this.repositoryItemButtonEdit1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons"), componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons1"), (int) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons2"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons3"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons4"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons5"), (ImageLocation) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons6"), (System.Drawing.Image) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons7"), new KeyShortcut(System.Windows.Forms.Keys.None), (AppearanceObject) appearanceObject1, componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons8"), componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons9"), (SuperToolTip) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons10"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons11"))
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
      componentResourceManager.ApplyResources((object) this.btDeleteImage, "btDeleteImage");
      componentResourceManager.ApplyResources((object) appearanceObject2, "serializableAppearanceObject2");
      this.btDeleteImage.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("btDeleteImage.Buttons"), componentResourceManager.GetString("btDeleteImage.Buttons1"), (int) componentResourceManager.GetObject("btDeleteImage.Buttons2"), (bool) componentResourceManager.GetObject("btDeleteImage.Buttons3"), (bool) componentResourceManager.GetObject("btDeleteImage.Buttons4"), (bool) componentResourceManager.GetObject("btDeleteImage.Buttons5"), (ImageLocation) componentResourceManager.GetObject("btDeleteImage.Buttons6"), (System.Drawing.Image) componentResourceManager.GetObject("btDeleteImage.Buttons7"), new KeyShortcut(System.Windows.Forms.Keys.None), (AppearanceObject) appearanceObject2, componentResourceManager.GetString("btDeleteImage.Buttons8"), componentResourceManager.GetObject("btDeleteImage.Buttons9"), (SuperToolTip) componentResourceManager.GetObject("btDeleteImage.Buttons10"), (bool) componentResourceManager.GetObject("btDeleteImage.Buttons11"))
      });
      this.btDeleteImage.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("btDeleteImage.Mask.AutoComplete");
      this.btDeleteImage.Mask.BeepOnError = (bool) componentResourceManager.GetObject("btDeleteImage.Mask.BeepOnError");
      this.btDeleteImage.Mask.EditMask = componentResourceManager.GetString("btDeleteImage.Mask.EditMask");
      this.btDeleteImage.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("btDeleteImage.Mask.IgnoreMaskBlank");
      this.btDeleteImage.Mask.MaskType = (MaskType) componentResourceManager.GetObject("btDeleteImage.Mask.MaskType");
      this.btDeleteImage.Mask.PlaceHolder = (char) componentResourceManager.GetObject("btDeleteImage.Mask.PlaceHolder");
      this.btDeleteImage.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("btDeleteImage.Mask.SaveLiteral");
      this.btDeleteImage.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("btDeleteImage.Mask.ShowPlaceHolders");
      this.btDeleteImage.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("btDeleteImage.Mask.UseMaskAsDisplayFormat");
      this.btDeleteImage.Name = "btDeleteImage";
      this.btDeleteImage.ButtonClick += new ButtonPressedEventHandler(this.btDeleteImage_ButtonClick);
      componentResourceManager.ApplyResources((object) this.gridView1, "gridView1");
      this.gridView1.GridControl = this.gcImagesFullFace;
      this.gridView1.Name = "gridView1";
      componentResourceManager.ApplyResources((object) this.btDelete, "btDelete");
      this.btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      this.btDelete.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btDelete.Appearance.FontSizeDelta");
      this.btDelete.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btDelete.Appearance.FontStyleDelta");
      this.btDelete.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btDelete.Appearance.GradientMode");
      this.btDelete.Appearance.Image = (System.Drawing.Image) componentResourceManager.GetObject("btDelete.Appearance.Image");
      this.btDelete.Appearance.Options.UseFont = true;
      this.btDelete.DialogResult = DialogResult.OK;
      this.btDelete.Name = "btDelete";
      this.btDelete.Click += new EventHandler(this.btDelete_Click);
      this.AcceptButton = (IButtonControl) this.btSave;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btDelete);
      this.Controls.Add((Control) this.gcImagesFullFace);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = "FrmEditFace";
      this.WindowState = FormWindowState.Maximized;
      this.FormClosing += new FormClosingEventHandler(this.EditEmployerForm_FormClosing);
      this.Load += new EventHandler(this.EditEmployerForm_Load);
      this.repositoryItemPictureEdit1.EndInit();
      this.repositoryItemCheckEdit1.EndInit();
      this.groupBox1.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tbComment.Properties.EndInit();
      this.dtpBithday.Properties.CalendarTimeProperties.EndInit();
      this.dtpBithday.Properties.EndInit();
      this.cbAccessTemplate.Properties.EndInit();
      this.cbSEX.Properties.EndInit();
      this.tbLastName.Properties.EndInit();
      this.tbFirstName.Properties.EndInit();
      this.tbSurname.Properties.EndInit();
      this.gcImagesFullFace.EndInit();
      this.lvImagesFullFace.EndInit();
      this.layoutViewField_colImage.EndInit();
      this.layoutViewField_colName.EndInit();
      this.layoutViewField_colImageComment.EndInit();
      this.layoutViewField_colImageID.EndInit();
      this.layoutViewField_colIsMain.EndInit();
      this.layoutViewCard1.EndInit();
      this.repositoryItemButtonEdit1.EndInit();
      this.btDeleteImage.EndInit();
      this.gridView1.EndInit();
      this.ResumeLayout(false);
    }
  }
}
