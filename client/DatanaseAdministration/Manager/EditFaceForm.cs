// Decompiled with JetBrains decompiler
// Type: CascadeManager.EditFaceForm
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BasicComponents;
using CascadeManager.Properties;
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
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = TS.Sdk.StaticFace.Model.Image;
using Padding = DevExpress.XtraLayout.Utils.Padding;
using Rectangle = TS.Core.Model.Rectangle;

namespace CascadeManager
{
	public class EditFaceForm : XtraForm
	{
		public BcFace CurrentEmployer = new BcFace();
		public List<BcKey> Keys = new List<BcKey>();
		public List<BcKey> CurrentKeys = new List<BcKey>();
		public List<BcKeySettings> KeySettings = new List<BcKeySettings>();
		private List<BcDepartment> _deps = new List<BcDepartment>();
		private List<BcPost> _posts = new List<BcPost>();
		private List<BcOrganization> _orgs = new List<BcOrganization>();
		private readonly List<string> _countryList = new List<string>();
		private readonly List<string> _regionList = new List<string>();
		private readonly List<string> _cityList = new List<string>();
		private readonly List<string> _districtList = new List<string>();
		private readonly List<string> _streetList = new List<string>();
		private DataTable _dtImages = new DataTable();
		private List<BcAccessCategory> _templateList = new List<BcAccessCategory>();
		private List<Guid> _deletedImages = new List<Guid>();
		private IContainer components = null;
		private bool _cancelFlag;
		private bool _flagAfterAdd;
		public bool AddNewValue;
		private bool _isloading;
		private bool _changedValues;
		private GroupControl groupBox1;
		private LabelControl lbSurname;
		private TextEdit tbSurname;
		private LabelControl lbBirthday;
		private TextEdit tbPassport;
		private LabelControl lbPassport;
		private TextEdit tbLastName;
		private LabelControl lbLastName;
		private TextEdit tbFirstName;
		private LabelControl lbFirstName;
		private GroupControl groupBox2;
		private TextEdit tbFlat;
		private LabelControl lbFlat;
		private TextEdit tbHome;
		private LabelControl lbHome;
		private LabelControl lbStreet;
		private LabelControl lbDistrict;
		private LabelControl lbCity;
		private LabelControl lbRegion;
		private LabelControl lbCountry;
		private TextEdit tbPhone;
		private TextEdit tbMobile;
		private LabelControl label2;
		private LabelControl label3;
		private LabelControl lbComment;
		private LabelControl lbSex;
		private SimpleButton btCancel;
		private SimpleButton btSave;
		private LabelControl label1;
		private PageSetupDialog pageSetupDialog1;
		private ComboBoxEdit cbStreet;
		private ComboBoxEdit cbDistrict;
		private ComboBoxEdit cbCity;
		private ComboBoxEdit cbRegion;
		private ComboBoxEdit cbCountry;
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
		private LayoutViewField layoutViewField_layoutViewColumn1;
		private LayoutViewField layoutViewField_layoutViewColumn2_1;
		private LayoutViewField layoutViewField_layoutViewColumn2;
		private LayoutViewField layoutViewField_layoutViewColumn1_1;
		private LayoutViewField layoutViewField_layoutViewColumn1_10;
		private LayoutViewCard layoutViewCard1;
		private SimpleButton btFile;
		private SimpleButton btEditPicture;
		private SimpleButton btCamera;
		private SimpleButton btPlayer;
		private SimpleButton btAdd;
		private SimpleButton btDelete;
		private RepositoryItemButtonEdit repositoryItemButtonEdit1;
		private RepositoryItemPictureEdit repositoryItemPictureEdit1;
		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		public EditFaceForm()
		{
			InitializeComponent();
		}

		private void LoadImages()
		{
			_dtImages = new DataTable();
			_dtImages.Columns.Add("ID", typeof(Guid));
			_dtImages.Columns.Add("ImageIcon", typeof(Bitmap));
			_dtImages.Columns.Add("Image", typeof(Bitmap));
			_dtImages.Columns.Add("Name", typeof(string));
			_dtImages.Columns.Add("Comment", typeof(string));
			_dtImages.Columns.Add("IsMain", typeof(bool));
			_dtImages.Columns.Add("Changed", typeof(bool));
			_dtImages.Columns.Add("ImageChanged", typeof(bool));
			if (!(CurrentEmployer.Id != Guid.Empty))
				return;
			foreach (BcImage bcImage in BcImage.LoadByFaceId(CurrentEmployer.Id))
			{
				Bitmap bitmap1 = new Bitmap(new MemoryStream(bcImage.ImageIcon));
				Bitmap bitmap2 = new Bitmap(new MemoryStream(bcImage.Image));
				_dtImages.Rows.Add(
					bcImage.Id, 
					bitmap1, 
					bitmap2, 
					bcImage.Name, 
					bcImage.Comment, 
					bcImage.IsMain, 
					false, 
					false);
			}
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
				if (XtraMessageBox.Show(Messages.RecordHasBeenChanged, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					return;
				btSave_Click(new object(), new EventArgs());
				_changedValues = false;
			}
			else
				_cancelFlag = false;
		}

		private byte[] GetNewKey(Bitmap bmp, FaceInfo item, int size)
		{
			double num = item.EyeDistance();
			using (Bitmap source = new Bitmap(bmp, new Size((int)(bmp.Width * size / num), (int)(bmp.Height * size / num))))
			{
				Image image = source.ConvertFrom();
				FaceInfo face = MainForm.Engine.DetectMaxFace(image, null);
				if (item.FaceRectangle.Width > 0.0)
					return MainForm.Engine.ExtractTemplate(image, face);
				return null;
			}
		}

		private void btSave_Click(object sender, EventArgs e)
		{
			_cancelFlag = false;
			KeySettings = BcKeySettings.LoadAll();
			if (CurrentEmployer.Id == Guid.Empty)
				_flagAfterAdd = true;
			_changedValues = false;
			try
			{
				if (dtpBithday.DateTime == DateTime.MinValue)
					dtpBithday.DateTime = new DateTime(1900, 1, 1);
				CurrentEmployer.Birthday = dtpBithday.DateTime;
			}
			catch
			{
			}
			CurrentEmployer.FirstName = tbFirstName.Text;
			CurrentEmployer.Surname = tbSurname.Text;
			CurrentEmployer.LastName = tbLastName.Text;
			CurrentEmployer.Passport = tbPassport.Text;
			CurrentEmployer.Comment = tbComment.Text;
			CurrentEmployer.Sex = cbSEX.SelectedIndex;
			CurrentEmployer.EditUserId = MainForm.CurrentUser.Id;
			if (cbAccessTemplate.SelectedIndex != -1)
				CurrentEmployer.AccessId = _templateList[cbAccessTemplate.SelectedIndex].Id;
			CurrentEmployer.Country = cbCountry.Text;
			CurrentEmployer.City = cbCity.Text;
			CurrentEmployer.District = cbDistrict.Text;
			CurrentEmployer.Region = cbRegion.Text;
			CurrentEmployer.Street = cbStreet.Text;
			CurrentEmployer.Flat = tbFlat.Text;
			CurrentEmployer.Home = tbHome.Text;
			CurrentEmployer.Mobile = tbMobile.Text;
			CurrentEmployer.Phone = tbPhone.Text;
			bool flag = false;
			foreach (DataRow dataRow in (InternalDataCollectionBase)_dtImages.Rows)
			{
				if ((bool)dataRow["IsMain"])
				{
					flag = true;
					Bitmap bitmap = new Bitmap((System.Drawing.Image)dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image)dataRow["Image"]).Height / ((System.Drawing.Image)dataRow["Image"]).Width));
					dataRow["IsMain"] = true;
					FaceInfo faceInfo = MainForm.Engine.DetectMaxFace(((Bitmap)dataRow["Image"]).ConvertFrom(), null);
					if (faceInfo != null)
					{
						bitmap = new Bitmap((int)faceInfo.FaceRectangle.Width * 2, (int)faceInfo.FaceRectangle.Height * 2);
						Rectangle faceRectangle = faceInfo.FaceRectangle;
						double x = faceRectangle.X;
						faceRectangle = faceInfo.FaceRectangle;
						double num1 = faceRectangle.Width / 2.0;
						double num2 = x - num1;
						double num3 = ((System.Drawing.Image)dataRow["Image"]).Height;
						faceRectangle = faceInfo.FaceRectangle;
						double num4 = Math.Abs(faceRectangle.Y);
						double num5 = num3 - num4;
						faceRectangle = faceInfo.FaceRectangle;
						double height1 = faceRectangle.Height;
						double num6 = num5 - height1 - faceInfo.FaceRectangle.Width / 2.0;
						int width = bitmap.Width;
						int height2 = bitmap.Height;
						using (Graphics graphics = Graphics.FromImage(bitmap))
						{
							graphics.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, width, height2));
							graphics.DrawImage((System.Drawing.Image)dataRow["Image"], new System.Drawing.Rectangle(0, 0, width, height2), new System.Drawing.Rectangle((int)num2, (int)num6, width, height2), GraphicsUnit.Pixel);
						}
						if (bitmap.Width > 240)
							bitmap = new Bitmap(bitmap, new Size(240, 240));
					}
					MemoryStream memoryStream = new MemoryStream();
					bitmap.Save(memoryStream, ImageFormat.Jpeg);
					CurrentEmployer.ImageIcon = (byte[])memoryStream.GetBuffer().Clone();
					break;
				}
			}
			if (!flag)
			{
				if (_dtImages.Rows.Count == 1)
				{
					IEnumerator enumerator = _dtImages.Rows.GetEnumerator();
					try
					{
						if (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							Bitmap source = new Bitmap((System.Drawing.Image)dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image)dataRow["Image"]).Height / ((System.Drawing.Image)dataRow["Image"]).Width));
							dataRow["IsMain"] = true;
							Image image = source.ConvertFrom();
							FaceInfo faceInfo = MainForm.Engine.DetectMaxFace(image, null);
							if (faceInfo != null)
							{
								source = new Bitmap((int)faceInfo.FaceRectangle.Width * 2, (int)faceInfo.FaceRectangle.Height * 2);
								Rectangle faceRectangle = faceInfo.FaceRectangle;
								double x = faceRectangle.X;
								faceRectangle = faceInfo.FaceRectangle;
								double num1 = faceRectangle.Width / 2.0;
								double num2 = x - num1;
								double num3 = ((System.Drawing.Image)dataRow["Image"]).Height;
								faceRectangle = faceInfo.FaceRectangle;
								double num4 = Math.Abs(faceRectangle.Y);
								double num5 = num3 - num4;
								faceRectangle = faceInfo.FaceRectangle;
								double height1 = faceRectangle.Height;
								double num6 = num5 - height1 - faceInfo.FaceRectangle.Width / 2.0;
								int width = source.Width;
								int height2 = source.Height;
								using (Graphics graphics = Graphics.FromImage(source))
								{
									graphics.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, width, height2));
									graphics.DrawImage((System.Drawing.Image)dataRow["Image"], new System.Drawing.Rectangle(0, 0, width, height2), new System.Drawing.Rectangle((int)num2, (int)num6, width, height2), GraphicsUnit.Pixel);
								}
								if (source.Width > 240)
									source = new Bitmap(source, new Size(240, 240));
							}
							MemoryStream memoryStream = new MemoryStream();
							source.Save(memoryStream, ImageFormat.Jpeg);
							CurrentEmployer.ImageIcon = (byte[])memoryStream.GetBuffer().Clone();
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
					int num = (int)XtraMessageBox.Show(Messages.SetMainPhoto, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					_cancelFlag = true;
					return;
				}
			}
			CurrentEmployer.Save();
			foreach (DataRow dataRow in (InternalDataCollectionBase)_dtImages.Rows)
			{
				if ((bool)dataRow["Changed"])
				{
					BcImage bcImage = new BcImage
					{
						Id = (Guid)dataRow["ID"]
					};
					MemoryStream memoryStream1 = new MemoryStream();
					Bitmap bmp = (Bitmap)dataRow["Image"];
					bmp.Save(memoryStream1, ImageFormat.Jpeg);
					bcImage.Image = memoryStream1.GetBuffer();
					bcImage.IsMain = (bool)dataRow["IsMain"];
					bcImage.Name = dataRow["Name"].ToString();
					bcImage.Comment = dataRow["Comment"].ToString();
					bcImage.FaceId = CurrentEmployer.Id;
					Bitmap source = (Bitmap)dataRow["Image"];
					if ((bool)dataRow["ImageChanged"])
					{
						Bitmap bitmap1 = new Bitmap((System.Drawing.Image)dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image)dataRow["Image"]).Height / ((System.Drawing.Image)dataRow["Image"]).Width));
						MemoryStream memoryStream2 = new MemoryStream();
						bitmap1.Save(memoryStream2, ImageFormat.Jpeg);
						bcImage.ImageIcon = memoryStream2.GetBuffer();
						Image image = source.ConvertFrom();
						FaceInfo face = MainForm.Engine.DetectMaxFace(image, null);
						BcKey bcKey1 = new BcKey
						{
							ImageId = bcImage.Id
						};
						bcKey1.DeleteByImageId();
						if (face != null)
						{
							Rectangle faceRectangle = face.FaceRectangle;
							int width1 = (int)faceRectangle.Width * 2;
							faceRectangle = face.FaceRectangle;
							int height1 = (int)faceRectangle.Height * 2;
							Bitmap bitmap2 = new Bitmap(width1, height1);
							faceRectangle = face.FaceRectangle;
							double x = faceRectangle.X;
							faceRectangle = face.FaceRectangle;
							double num1 = faceRectangle.Width / 2.0;
							double num2 = x - num1;
							double num3 = ((System.Drawing.Image)dataRow["Image"]).Height;
							faceRectangle = face.FaceRectangle;
							double num4 = Math.Abs(faceRectangle.Y);
							double num5 = num3 - num4;
							faceRectangle = face.FaceRectangle;
							double height2 = faceRectangle.Height;
							double num6 = num5 - height2 - face.FaceRectangle.Width / 2.0;
							int width2 = bitmap2.Width;
							int height3 = bitmap2.Height;
							using (Graphics graphics = Graphics.FromImage(bitmap2))
							{
								graphics.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, width2, height3));
								graphics.DrawImage((System.Drawing.Image)dataRow["Image"], new System.Drawing.Rectangle(0, 0, width2, height3), new System.Drawing.Rectangle((int)num2, (int)num6, width2, height3), GraphicsUnit.Pixel);
							}
							if (bitmap2.Width > 240)
								bitmap2 = new Bitmap(bitmap2, new Size(240, 240));
							MemoryStream memoryStream3 = new MemoryStream();
							bitmap2.Save(memoryStream3, ImageFormat.Jpeg);
							bcImage.ImageIcon = memoryStream3.GetBuffer();
							bcImage.Save();
							byte[] template = MainForm.Engine.ExtractTemplate(image, face);
							if (template != null)
							{
								bcKey1.FaceId = CurrentEmployer.Id;
								bcKey1.ImageKey = template;
								bcKey1.Ksid = -1;
								bcKey1.ImageId = bcImage.Id;
								bcKey1.Save();
							}
							BcKey bcKey2 = new BcKey
							{
								FaceId = CurrentEmployer.Id,
								ImageId = bcImage.Id,
								Ksid = -3
							};
							byte[] newKey1 = GetNewKey(bmp, face, 50);
							bcKey2.ImageKey = newKey1;
							if (newKey1 != null)
								bcKey2.Save();
							BcKey bcKey3 = new BcKey();
							bcKey3.FaceId = CurrentEmployer.Id;
							bcKey3.ImageId = bcImage.Id;
							bcKey3.Ksid = -4;
							byte[] newKey2 = GetNewKey(bmp, face, 100);
							bcKey3.ImageKey = newKey2;
							if (newKey2 != null)
								bcKey3.Save();
							BcKey bcKey4 = new BcKey();
							bcKey4.FaceId = CurrentEmployer.Id;
							bcKey4.ImageId = bcImage.Id;
							bcKey4.Ksid = -5;
							byte[] newKey3 = GetNewKey(bmp, face, 150);
							bcKey4.ImageKey = newKey3;
							if (newKey3 != null)
								bcKey4.Save();
						}
					}
					else
						bcImage.Save();
				}
				else
				{
					BcImage bcImage = new BcImage();
					bcImage.Id = (Guid)dataRow["ID"];
					MemoryStream memoryStream1 = new MemoryStream();
					((System.Drawing.Image)dataRow["Image"]).Save(memoryStream1, ImageFormat.Jpeg);
					bcImage.Image = memoryStream1.GetBuffer();
					Bitmap bitmap = new Bitmap((System.Drawing.Image)dataRow["Image"], new Size(128, 128 * ((System.Drawing.Image)dataRow["Image"]).Height / ((System.Drawing.Image)dataRow["Image"]).Width));
					MemoryStream memoryStream2 = new MemoryStream();
					bitmap.Save(memoryStream2, ImageFormat.Jpeg);
					bcImage.ImageIcon = memoryStream2.GetBuffer();
					bcImage.IsMain = (bool)dataRow["IsMain"];
					bcImage.Name = dataRow["Name"].ToString();
					bcImage.Comment = dataRow["Comment"].ToString();
					bcImage.FaceId = CurrentEmployer.Id;
					bcImage.Save();
				}
			}
			foreach (Guid guid in _deletedImages)
				new BcImage
				{
					Id = guid
				}.DeleteById();
		}

		private void btCancel_Click(object sender, EventArgs e)
		{
			FindChanges();
			Close();
		}

		private void EditEmployerForm_Load(object sender, EventArgs e)
		{
			dtpBithday.DateTime = DateTime.Now;
			cbSEX.SelectedIndex = 0;
			_isloading = true;
			_changedValues = false;
			SqlCommand sqlCommand = new SqlCommand(" \r\nSelect \r\ndistinct\r\nCountry, \r\nRegion ,\r\nCity, \r\nDistrict ,\r\nStreet\r\nFrom Faces\r\norder by\r\nCountry,\r\nRegion,\r\nCity,\r\nDistrict,\r\nStreet\r\n", new SqlConnection(CommonSettings.ConnectionString));
			sqlCommand.Connection.Open();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				if (!_countryList.Contains(sqlDataReader[0].ToString()))
				{
					_countryList.Add(sqlDataReader[0].ToString());
					cbCountry.Properties.Items.Add(sqlDataReader[0].ToString());
				}
				if (!_regionList.Contains(sqlDataReader[1].ToString()))
				{
					_regionList.Add(sqlDataReader[1].ToString());
					cbRegion.Properties.Items.Add(sqlDataReader[1].ToString());
				}
				if (!_cityList.Contains(sqlDataReader[2].ToString()))
				{
					_cityList.Add(sqlDataReader[2].ToString());
					cbCity.Properties.Items.Add(sqlDataReader[2].ToString());
				}
				if (!_districtList.Contains(sqlDataReader[3].ToString()))
				{
					_districtList.Add(sqlDataReader[3].ToString());
					cbDistrict.Properties.Items.Add(sqlDataReader[3].ToString());
				}
				if (!_streetList.Contains(sqlDataReader[4].ToString()))
				{
					_streetList.Add(sqlDataReader[4].ToString());
					cbStreet.Properties.Items.Add(sqlDataReader[4].ToString());
				}
			}
			sqlCommand.Connection.Close();
			cbCountry.SelectedIndex = _countryList.IndexOf("Россия");
			cbCity.SelectedIndex = _cityList.IndexOf("Москва");
			_deps = BcDepartment.LoadAll();
			foreach (BcDepartment bcDepartment in _deps)
				;
			_orgs = BcOrganization.LoadAll();
			foreach (BcOrganization bcOrganization in _orgs)
				;
			_posts = BcPost.LoadAll();
			foreach (BcPost bcPost in _posts)
				;
			BcTiming.LoadAll();
			_templateList = BcAccessCategory.LoadAll();
			foreach (BcAccessCategory bcAccessCategory in _templateList)
				cbAccessTemplate.Properties.Items.Add(bcAccessCategory.Name);
			LoadImages();
			gcImagesFullFace.DataSource = _dtImages;
			if (CurrentEmployer.Id != Guid.Empty)
			{
				Keys = new List<BcKey>();
				CurrentKeys = BcKey.LoadKyesByFaceId(CurrentEmployer.Id);
				cbCountry.Text = CurrentEmployer.Country;
				cbCity.Text = CurrentEmployer.City;
				cbDistrict.Text = CurrentEmployer.District;
				cbRegion.Text = CurrentEmployer.Region;
				cbStreet.Text = CurrentEmployer.Street;
				tbFlat.Text = CurrentEmployer.Flat;
				tbHome.Text = CurrentEmployer.Home;
				tbMobile.Text = CurrentEmployer.Mobile;
				tbPhone.Text = CurrentEmployer.Phone;
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
				cbCountry.Text = CurrentEmployer.Country;
				cbCity.Text = CurrentEmployer.City;
				cbRegion.Text = CurrentEmployer.Region;
				cbDistrict.Text = CurrentEmployer.District;
				cbStreet.Text = CurrentEmployer.Street;
				tbFlat.Text = CurrentEmployer.Flat;
				tbHome.Text = CurrentEmployer.Home;
				tbPhone.Text = CurrentEmployer.Phone;
				tbMobile.Text = CurrentEmployer.Mobile;
			}
			else
				btAdd_Click(sender, e);
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
				if (XtraMessageBox.Show(Messages.NewRecordWasCreated, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
			if (lvImagesFullFace.FocusedRowHandle < 0)
				return;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Images| *.bmp; *.jpeg; *.png; *.tiff; *.gif; *.jpg";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Bitmap bitmap = new Bitmap(openFileDialog.FileName);
				DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
				dataRow["Image"] = bitmap;
				dataRow["Changed"] = true;
				dataRow["ImageChanged"] = true;
			}
		}

		private void btPlayer_Click(object sender, EventArgs e)
		{
			if (lvImagesFullFace.FocusedRowHandle < 0)
				return;
			FrmVideo frmVideo = new FrmVideo();
			if (frmVideo.ShowDialog() == DialogResult.OK)
			{
				DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
				dataRow["Changed"] = true;
				dataRow["ImageChanged"] = true;
				dataRow["Image"] = frmVideo.ImgResult;
			}
		}

		private void btCamera_Click(object sender, EventArgs e)
		{
			if (lvImagesFullFace.FocusedRowHandle < 0)
				return;
			FrmWebCam frmWebCam = new FrmWebCam();
			if (frmWebCam.ShowDialog() == DialogResult.OK)
			{
				DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
				dataRow["Image"] = frmWebCam.ImgResult;
				dataRow["Changed"] = true;
				dataRow["ImageChanged"] = true;
			}
		}

		private void btEditPicture_Click(object sender, EventArgs e)
		{
			if (lvImagesFullFace.FocusedRowHandle < 0)
				return;
			DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
			Bitmap bitmap = (Bitmap)dataRow["Image"];
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Bmp);
			FrmPictureEdit frmPictureEdit = new FrmPictureEdit();
			frmPictureEdit.WpfControl.LoadImage((byte[])memoryStream.GetBuffer().Clone());
			if (frmPictureEdit.ShowDialog() == DialogResult.OK)
			{
				dataRow["Image"] = new Bitmap(new MemoryStream(frmPictureEdit.WpfControl.ImgDefaultPath1));
				dataRow["Changed"] = true;
				dataRow["ImageChanged"] = true;
			}
		}

		private void btAdd_Click(object sender, EventArgs e)
		{
			_dtImages.Rows.Add((object)Guid.Empty, (object)new Bitmap(128, 128), (object)new Bitmap(128, 128), (object)"", (object)"", (object)false, (object)false, (object)false);
		}

		private void btDelete_Click(object sender, EventArgs e)
		{
			if (lvImagesFullFace.FocusedRowHandle < 0)
				return;
			DataRow dataRow = lvImagesFullFace.GetDataRow(lvImagesFullFace.FocusedRowHandle);
			_deletedImages.Add((Guid)dataRow["ID"]);
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
			if (_isloading || e.Column != colIsMain || !(bool)e.Value)
				return;
			DataRow dataRow1 = lvImagesFullFace.GetDataRow(e.RowHandle);
			foreach (DataRow dataRow2 in (InternalDataCollectionBase)_dtImages.Rows)
			{
				if (dataRow1 != dataRow2)
					dataRow2["IsMain"] = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EditFaceForm));
			GridLevelNode gridLevelNode = new GridLevelNode();
			SerializableAppearanceObject appearanceObject = new SerializableAppearanceObject();
			repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
			repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			groupBox1 = new GroupControl();
			tbComment = new MemoEdit();
			dtpBithday = new DateEdit();
			tbPassport = new TextEdit();
			btCancel = new SimpleButton();
			lbBirthday = new LabelControl();
			btSave = new SimpleButton();
			lbPassport = new LabelControl();
			label3 = new LabelControl();
			cbAccessTemplate = new ComboBoxEdit();
			lbComment = new LabelControl();
			lbSex = new LabelControl();
			cbSEX = new ComboBoxEdit();
			groupBox2 = new GroupControl();
			tbMobile = new TextEdit();
			label2 = new LabelControl();
			tbPhone = new TextEdit();
			label1 = new LabelControl();
			tbFlat = new TextEdit();
			lbFlat = new LabelControl();
			tbHome = new TextEdit();
			lbHome = new LabelControl();
			lbStreet = new LabelControl();
			cbStreet = new ComboBoxEdit();
			lbDistrict = new LabelControl();
			cbDistrict = new ComboBoxEdit();
			lbCity = new LabelControl();
			cbCity = new ComboBoxEdit();
			lbRegion = new LabelControl();
			cbRegion = new ComboBoxEdit();
			lbCountry = new LabelControl();
			cbCountry = new ComboBoxEdit();
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
			colIsMain = new LayoutViewColumn();
			layoutViewField_layoutViewColumn1_10 = new LayoutViewField();
			layoutViewCard1 = new LayoutViewCard();
			repositoryItemButtonEdit1 = new RepositoryItemButtonEdit();
			gridView1 = new GridView();
			btFile = new SimpleButton();
			btEditPicture = new SimpleButton();
			btCamera = new SimpleButton();
			btPlayer = new SimpleButton();
			btAdd = new SimpleButton();
			btDelete = new SimpleButton();
			repositoryItemPictureEdit1.BeginInit();
			repositoryItemCheckEdit1.BeginInit();
			groupBox1.BeginInit();
			groupBox1.SuspendLayout();
			tbComment.Properties.BeginInit();
			dtpBithday.Properties.CalendarTimeProperties.BeginInit();
			dtpBithday.Properties.BeginInit();
			tbPassport.Properties.BeginInit();
			cbAccessTemplate.Properties.BeginInit();
			cbSEX.Properties.BeginInit();
			groupBox2.BeginInit();
			groupBox2.SuspendLayout();
			tbMobile.Properties.BeginInit();
			tbPhone.Properties.BeginInit();
			tbFlat.Properties.BeginInit();
			tbHome.Properties.BeginInit();
			cbStreet.Properties.BeginInit();
			cbDistrict.Properties.BeginInit();
			cbCity.Properties.BeginInit();
			cbRegion.Properties.BeginInit();
			cbCountry.Properties.BeginInit();
			tbLastName.Properties.BeginInit();
			tbFirstName.Properties.BeginInit();
			tbSurname.Properties.BeginInit();
			gcImagesFullFace.BeginInit();
			lvImagesFullFace.BeginInit();
			layoutViewField_layoutViewColumn1.BeginInit();
			layoutViewField_layoutViewColumn2_1.BeginInit();
			layoutViewField_layoutViewColumn2.BeginInit();
			layoutViewField_layoutViewColumn1_1.BeginInit();
			layoutViewField_layoutViewColumn1_10.BeginInit();
			layoutViewCard1.BeginInit();
			repositoryItemButtonEdit1.BeginInit();
			gridView1.BeginInit();
			SuspendLayout();
			componentResourceManager.ApplyResources(repositoryItemPictureEdit1, "repositoryItemPictureEdit1");
			repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
			repositoryItemPictureEdit1.PictureStoreMode = PictureStoreMode.Image;
			repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
			componentResourceManager.ApplyResources(repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
			repositoryItemCheckEdit1.AutoWidth = true;
			repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			componentResourceManager.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(tbComment);
			groupBox1.Controls.Add(dtpBithday);
			groupBox1.Controls.Add(tbPassport);
			groupBox1.Controls.Add(btCancel);
			groupBox1.Controls.Add(lbBirthday);
			groupBox1.Controls.Add(btSave);
			groupBox1.Controls.Add(lbPassport);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(cbAccessTemplate);
			groupBox1.Controls.Add(lbComment);
			groupBox1.Controls.Add(lbSex);
			groupBox1.Controls.Add(cbSEX);
			groupBox1.Controls.Add(groupBox2);
			groupBox1.Controls.Add(tbLastName);
			groupBox1.Controls.Add(lbLastName);
			groupBox1.Controls.Add(tbFirstName);
			groupBox1.Controls.Add(lbFirstName);
			groupBox1.Controls.Add(tbSurname);
			groupBox1.Controls.Add(lbSurname);
			groupBox1.Name = "groupBox1";
			groupBox1.ShowCaption = false;
			componentResourceManager.ApplyResources(tbComment, "tbComment");
			tbComment.Name = "tbComment";
			tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
			tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
			tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
			tbComment.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
			componentResourceManager.ApplyResources(dtpBithday, "dtpBithday");
			dtpBithday.Name = "dtpBithday";
			dtpBithday.Properties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.AccessibleDescription");
			dtpBithday.Properties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.AccessibleName");
			dtpBithday.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Font");
			dtpBithday.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontSizeDelta");
			dtpBithday.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("dtpBithday.Properties.Appearance.FontStyleDelta");
			dtpBithday.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("dtpBithday.Properties.Appearance.GradientMode");
			dtpBithday.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("dtpBithday.Properties.Appearance.Image");
			dtpBithday.Properties.Appearance.Options.UseFont = true;
			dtpBithday.Properties.AutoHeight = (bool)componentResourceManager.GetObject("dtpBithday.Properties.AutoHeight");
			dtpBithday.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("dtpBithday.Properties.Buttons"))
			});
			dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleDescription");
			dtpBithday.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.AccessibleName");
			dtpBithday.Properties.CalendarTimeProperties.AutoHeight = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.AutoHeight");
			dtpBithday.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton()
			});
			dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.AutoComplete");
			dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.BeepOnError");
			dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.Mask.EditMask");
			dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
			dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.MaskType");
			dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.PlaceHolder");
			dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.SaveLiteral");
			dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
			dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
			dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.CalendarTimeProperties.NullValuePrompt");
			dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("dtpBithday.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
			dtpBithday.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("dtpBithday.Properties.Mask.AutoComplete");
			dtpBithday.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("dtpBithday.Properties.Mask.BeepOnError");
			dtpBithday.Properties.Mask.EditMask = componentResourceManager.GetString("dtpBithday.Properties.Mask.EditMask");
			dtpBithday.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("dtpBithday.Properties.Mask.IgnoreMaskBlank");
			dtpBithday.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("dtpBithday.Properties.Mask.MaskType");
			dtpBithday.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("dtpBithday.Properties.Mask.PlaceHolder");
			dtpBithday.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("dtpBithday.Properties.Mask.SaveLiteral");
			dtpBithday.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("dtpBithday.Properties.Mask.ShowPlaceHolders");
			dtpBithday.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("dtpBithday.Properties.Mask.UseMaskAsDisplayFormat");
			dtpBithday.Properties.NullValuePrompt = componentResourceManager.GetString("dtpBithday.Properties.NullValuePrompt");
			dtpBithday.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("dtpBithday.Properties.NullValuePromptShowForEmptyValue");
			componentResourceManager.ApplyResources(tbPassport, "tbPassport");
			tbPassport.Name = "tbPassport";
			tbPassport.Properties.AccessibleDescription = componentResourceManager.GetString("tbPassport.Properties.AccessibleDescription");
			tbPassport.Properties.AccessibleName = componentResourceManager.GetString("tbPassport.Properties.AccessibleName");
			tbPassport.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbPassport.Properties.Appearance.Font");
			tbPassport.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontSizeDelta");
			tbPassport.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbPassport.Properties.Appearance.FontStyleDelta");
			tbPassport.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbPassport.Properties.Appearance.GradientMode");
			tbPassport.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbPassport.Properties.Appearance.Image");
			tbPassport.Properties.Appearance.Options.UseFont = true;
			tbPassport.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbPassport.Properties.AutoHeight");
			tbPassport.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbPassport.Properties.Mask.AutoComplete");
			tbPassport.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbPassport.Properties.Mask.BeepOnError");
			tbPassport.Properties.Mask.EditMask = componentResourceManager.GetString("tbPassport.Properties.Mask.EditMask");
			tbPassport.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbPassport.Properties.Mask.IgnoreMaskBlank");
			tbPassport.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbPassport.Properties.Mask.MaskType");
			tbPassport.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbPassport.Properties.Mask.PlaceHolder");
			tbPassport.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbPassport.Properties.Mask.SaveLiteral");
			tbPassport.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbPassport.Properties.Mask.ShowPlaceHolders");
			tbPassport.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbPassport.Properties.Mask.UseMaskAsDisplayFormat");
			tbPassport.Properties.NullValuePrompt = componentResourceManager.GetString("tbPassport.Properties.NullValuePrompt");
			tbPassport.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbPassport.Properties.NullValuePromptShowForEmptyValue");
			tbPassport.EditValueChanged += tbPassport_EditValueChanged;
			tbPassport.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(btCancel, "btCancel");
			btCancel.Appearance.Font = (Font)componentResourceManager.GetObject("btCancel.Appearance.Font");
			btCancel.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
			btCancel.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
			btCancel.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
			btCancel.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btCancel.Appearance.Image");
			btCancel.Appearance.Options.UseFont = true;
			btCancel.DialogResult = DialogResult.Cancel;
			btCancel.Name = "btCancel";
			btCancel.Click += btCancel_Click;
			componentResourceManager.ApplyResources(lbBirthday, "lbBirthday");
			lbBirthday.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbBirthday.Appearance.DisabledImage");
			lbBirthday.Appearance.Font = (Font)componentResourceManager.GetObject("lbBirthday.Appearance.Font");
			lbBirthday.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbBirthday.Appearance.FontSizeDelta");
			lbBirthday.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbBirthday.Appearance.FontStyleDelta");
			lbBirthday.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbBirthday.Appearance.ForeColor");
			lbBirthday.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbBirthday.Appearance.GradientMode");
			lbBirthday.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbBirthday.Appearance.HoverImage");
			lbBirthday.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbBirthday.Appearance.Image");
			lbBirthday.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbBirthday.Appearance.PressedImage");
			lbBirthday.Name = "lbBirthday";
			componentResourceManager.ApplyResources(btSave, "btSave");
			btSave.Appearance.Font = (Font)componentResourceManager.GetObject("btSave.Appearance.Font");
			btSave.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btSave.Appearance.FontSizeDelta");
			btSave.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btSave.Appearance.FontStyleDelta");
			btSave.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btSave.Appearance.GradientMode");
			btSave.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btSave.Appearance.Image");
			btSave.Appearance.Options.UseFont = true;
			btSave.DialogResult = DialogResult.OK;
			btSave.Name = "btSave";
			btSave.Click += btSave_Click;
			componentResourceManager.ApplyResources(lbPassport, "lbPassport");
			lbPassport.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbPassport.Appearance.DisabledImage");
			lbPassport.Appearance.Font = (Font)componentResourceManager.GetObject("lbPassport.Appearance.Font");
			lbPassport.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbPassport.Appearance.FontSizeDelta");
			lbPassport.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbPassport.Appearance.FontStyleDelta");
			lbPassport.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbPassport.Appearance.ForeColor");
			lbPassport.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbPassport.Appearance.GradientMode");
			lbPassport.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbPassport.Appearance.HoverImage");
			lbPassport.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbPassport.Appearance.Image");
			lbPassport.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbPassport.Appearance.PressedImage");
			lbPassport.Name = "lbPassport";
			componentResourceManager.ApplyResources(label3, "label3");
			label3.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("label3.Appearance.DisabledImage");
			label3.Appearance.Font = (Font)componentResourceManager.GetObject("label3.Appearance.Font");
			label3.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("label3.Appearance.FontSizeDelta");
			label3.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("label3.Appearance.FontStyleDelta");
			label3.Appearance.ForeColor = (Color)componentResourceManager.GetObject("label3.Appearance.ForeColor");
			label3.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("label3.Appearance.GradientMode");
			label3.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("label3.Appearance.HoverImage");
			label3.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("label3.Appearance.Image");
			label3.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("label3.Appearance.PressedImage");
			label3.Name = "label3";
			componentResourceManager.ApplyResources(cbAccessTemplate, "cbAccessTemplate");
			cbAccessTemplate.Name = "cbAccessTemplate";
			cbAccessTemplate.Properties.AccessibleDescription = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleDescription");
			cbAccessTemplate.Properties.AccessibleName = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleName");
			cbAccessTemplate.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Font");
			cbAccessTemplate.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontSizeDelta");
			cbAccessTemplate.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontStyleDelta");
			cbAccessTemplate.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.GradientMode");
			cbAccessTemplate.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Image");
			cbAccessTemplate.Properties.Appearance.Options.UseFont = true;
			cbAccessTemplate.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbAccessTemplate.Properties.AutoHeight");
			cbAccessTemplate.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbAccessTemplate.Properties.Buttons"))
			});
			cbAccessTemplate.Properties.NullValuePrompt = componentResourceManager.GetString("cbAccessTemplate.Properties.NullValuePrompt");
			cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue");
			cbAccessTemplate.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbComment, "lbComment");
			lbComment.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbComment.Appearance.DisabledImage");
			lbComment.Appearance.Font = (Font)componentResourceManager.GetObject("lbComment.Appearance.Font");
			lbComment.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbComment.Appearance.FontSizeDelta");
			lbComment.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbComment.Appearance.FontStyleDelta");
			lbComment.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbComment.Appearance.ForeColor");
			lbComment.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbComment.Appearance.GradientMode");
			lbComment.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbComment.Appearance.HoverImage");
			lbComment.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbComment.Appearance.Image");
			lbComment.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbComment.Appearance.PressedImage");
			lbComment.Name = "lbComment";
			componentResourceManager.ApplyResources(lbSex, "lbSex");
			lbSex.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSex.Appearance.DisabledImage");
			lbSex.Appearance.Font = (Font)componentResourceManager.GetObject("lbSex.Appearance.Font");
			lbSex.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbSex.Appearance.FontSizeDelta");
			lbSex.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbSex.Appearance.FontStyleDelta");
			lbSex.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbSex.Appearance.ForeColor");
			lbSex.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbSex.Appearance.GradientMode");
			lbSex.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSex.Appearance.HoverImage");
			lbSex.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbSex.Appearance.Image");
			lbSex.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSex.Appearance.PressedImage");
			lbSex.Name = "lbSex";
			componentResourceManager.ApplyResources(cbSEX, "cbSEX");
			cbSEX.Name = "cbSEX";
			cbSEX.Properties.AccessibleDescription = componentResourceManager.GetString("cbSEX.Properties.AccessibleDescription");
			cbSEX.Properties.AccessibleName = componentResourceManager.GetString("cbSEX.Properties.AccessibleName");
			cbSEX.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbSEX.Properties.Appearance.Font");
			cbSEX.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontSizeDelta");
			cbSEX.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontStyleDelta");
			cbSEX.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbSEX.Properties.Appearance.GradientMode");
			cbSEX.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbSEX.Properties.Appearance.Image");
			cbSEX.Properties.Appearance.Options.UseFont = true;
			cbSEX.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbSEX.Properties.AutoHeight");
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
			cbSEX.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbSEX.Properties.NullValuePromptShowForEmptyValue");
			cbSEX.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("groupBox2.Appearance.FontSizeDelta");
			groupBox2.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupBox2.Appearance.FontStyleDelta");
			groupBox2.Appearance.ForeColor = (Color)componentResourceManager.GetObject("groupBox2.Appearance.ForeColor");
			groupBox2.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupBox2.Appearance.GradientMode");
			groupBox2.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("groupBox2.Appearance.Image");
			groupBox2.Appearance.Options.UseForeColor = true;
			groupBox2.AppearanceCaption.Font = (Font)componentResourceManager.GetObject("groupBox2.AppearanceCaption.Font");
			groupBox2.AppearanceCaption.FontSizeDelta = (int)componentResourceManager.GetObject("groupBox2.AppearanceCaption.FontSizeDelta");
			groupBox2.AppearanceCaption.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupBox2.AppearanceCaption.FontStyleDelta");
			groupBox2.AppearanceCaption.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupBox2.AppearanceCaption.GradientMode");
			groupBox2.AppearanceCaption.Image = (System.Drawing.Image)componentResourceManager.GetObject("groupBox2.AppearanceCaption.Image");
			groupBox2.AppearanceCaption.Options.UseFont = true;
			groupBox2.Controls.Add(tbMobile);
			groupBox2.Controls.Add(label2);
			groupBox2.Controls.Add(tbPhone);
			groupBox2.Controls.Add(label1);
			groupBox2.Controls.Add(tbFlat);
			groupBox2.Controls.Add(lbFlat);
			groupBox2.Controls.Add(tbHome);
			groupBox2.Controls.Add(lbHome);
			groupBox2.Controls.Add(lbStreet);
			groupBox2.Controls.Add(cbStreet);
			groupBox2.Controls.Add(lbDistrict);
			groupBox2.Controls.Add(cbDistrict);
			groupBox2.Controls.Add(lbCity);
			groupBox2.Controls.Add(cbCity);
			groupBox2.Controls.Add(lbRegion);
			groupBox2.Controls.Add(cbRegion);
			groupBox2.Controls.Add(lbCountry);
			groupBox2.Controls.Add(cbCountry);
			groupBox2.Name = "groupBox2";
			componentResourceManager.ApplyResources(tbMobile, "tbMobile");
			tbMobile.Name = "tbMobile";
			tbMobile.Properties.AccessibleDescription = componentResourceManager.GetString("tbMobile.Properties.AccessibleDescription");
			tbMobile.Properties.AccessibleName = componentResourceManager.GetString("tbMobile.Properties.AccessibleName");
			tbMobile.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbMobile.Properties.Appearance.Font");
			tbMobile.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbMobile.Properties.Appearance.FontSizeDelta");
			tbMobile.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbMobile.Properties.Appearance.FontStyleDelta");
			tbMobile.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbMobile.Properties.Appearance.GradientMode");
			tbMobile.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbMobile.Properties.Appearance.Image");
			tbMobile.Properties.Appearance.Options.UseFont = true;
			tbMobile.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbMobile.Properties.AutoHeight");
			tbMobile.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbMobile.Properties.Mask.AutoComplete");
			tbMobile.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbMobile.Properties.Mask.BeepOnError");
			tbMobile.Properties.Mask.EditMask = componentResourceManager.GetString("tbMobile.Properties.Mask.EditMask");
			tbMobile.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbMobile.Properties.Mask.IgnoreMaskBlank");
			tbMobile.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbMobile.Properties.Mask.MaskType");
			tbMobile.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbMobile.Properties.Mask.PlaceHolder");
			tbMobile.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbMobile.Properties.Mask.SaveLiteral");
			tbMobile.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbMobile.Properties.Mask.ShowPlaceHolders");
			tbMobile.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbMobile.Properties.Mask.UseMaskAsDisplayFormat");
			tbMobile.Properties.NullValuePrompt = componentResourceManager.GetString("tbMobile.Properties.NullValuePrompt");
			tbMobile.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbMobile.Properties.NullValuePromptShowForEmptyValue");
			tbMobile.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(label2, "label2");
			label2.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("label2.Appearance.DisabledImage");
			label2.Appearance.Font = (Font)componentResourceManager.GetObject("label2.Appearance.Font");
			label2.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("label2.Appearance.FontSizeDelta");
			label2.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("label2.Appearance.FontStyleDelta");
			label2.Appearance.ForeColor = (Color)componentResourceManager.GetObject("label2.Appearance.ForeColor");
			label2.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("label2.Appearance.GradientMode");
			label2.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("label2.Appearance.HoverImage");
			label2.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("label2.Appearance.Image");
			label2.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("label2.Appearance.PressedImage");
			label2.Name = "label2";
			componentResourceManager.ApplyResources(tbPhone, "tbPhone");
			tbPhone.Name = "tbPhone";
			tbPhone.Properties.AccessibleDescription = componentResourceManager.GetString("tbPhone.Properties.AccessibleDescription");
			tbPhone.Properties.AccessibleName = componentResourceManager.GetString("tbPhone.Properties.AccessibleName");
			tbPhone.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbPhone.Properties.Appearance.Font");
			tbPhone.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbPhone.Properties.Appearance.FontSizeDelta");
			tbPhone.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbPhone.Properties.Appearance.FontStyleDelta");
			tbPhone.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbPhone.Properties.Appearance.GradientMode");
			tbPhone.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbPhone.Properties.Appearance.Image");
			tbPhone.Properties.Appearance.Options.UseFont = true;
			tbPhone.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbPhone.Properties.AutoHeight");
			tbPhone.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbPhone.Properties.Mask.AutoComplete");
			tbPhone.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbPhone.Properties.Mask.BeepOnError");
			tbPhone.Properties.Mask.EditMask = componentResourceManager.GetString("tbPhone.Properties.Mask.EditMask");
			tbPhone.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbPhone.Properties.Mask.IgnoreMaskBlank");
			tbPhone.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbPhone.Properties.Mask.MaskType");
			tbPhone.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbPhone.Properties.Mask.PlaceHolder");
			tbPhone.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbPhone.Properties.Mask.SaveLiteral");
			tbPhone.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbPhone.Properties.Mask.ShowPlaceHolders");
			tbPhone.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbPhone.Properties.Mask.UseMaskAsDisplayFormat");
			tbPhone.Properties.NullValuePrompt = componentResourceManager.GetString("tbPhone.Properties.NullValuePrompt");
			tbPhone.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbPhone.Properties.NullValuePromptShowForEmptyValue");
			tbPhone.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(label1, "label1");
			label1.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("label1.Appearance.DisabledImage");
			label1.Appearance.Font = (Font)componentResourceManager.GetObject("label1.Appearance.Font");
			label1.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("label1.Appearance.FontSizeDelta");
			label1.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("label1.Appearance.FontStyleDelta");
			label1.Appearance.ForeColor = (Color)componentResourceManager.GetObject("label1.Appearance.ForeColor");
			label1.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("label1.Appearance.GradientMode");
			label1.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("label1.Appearance.HoverImage");
			label1.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("label1.Appearance.Image");
			label1.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("label1.Appearance.PressedImage");
			label1.Name = "label1";
			componentResourceManager.ApplyResources(tbFlat, "tbFlat");
			tbFlat.Name = "tbFlat";
			tbFlat.Properties.AccessibleDescription = componentResourceManager.GetString("tbFlat.Properties.AccessibleDescription");
			tbFlat.Properties.AccessibleName = componentResourceManager.GetString("tbFlat.Properties.AccessibleName");
			tbFlat.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbFlat.Properties.Appearance.Font");
			tbFlat.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbFlat.Properties.Appearance.FontSizeDelta");
			tbFlat.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbFlat.Properties.Appearance.FontStyleDelta");
			tbFlat.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbFlat.Properties.Appearance.GradientMode");
			tbFlat.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbFlat.Properties.Appearance.Image");
			tbFlat.Properties.Appearance.Options.UseFont = true;
			tbFlat.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbFlat.Properties.AutoHeight");
			tbFlat.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbFlat.Properties.Mask.AutoComplete");
			tbFlat.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbFlat.Properties.Mask.BeepOnError");
			tbFlat.Properties.Mask.EditMask = componentResourceManager.GetString("tbFlat.Properties.Mask.EditMask");
			tbFlat.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbFlat.Properties.Mask.IgnoreMaskBlank");
			tbFlat.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbFlat.Properties.Mask.MaskType");
			tbFlat.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbFlat.Properties.Mask.PlaceHolder");
			tbFlat.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbFlat.Properties.Mask.SaveLiteral");
			tbFlat.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbFlat.Properties.Mask.ShowPlaceHolders");
			tbFlat.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbFlat.Properties.Mask.UseMaskAsDisplayFormat");
			tbFlat.Properties.NullValuePrompt = componentResourceManager.GetString("tbFlat.Properties.NullValuePrompt");
			tbFlat.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbFlat.Properties.NullValuePromptShowForEmptyValue");
			tbFlat.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbFlat, "lbFlat");
			lbFlat.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFlat.Appearance.DisabledImage");
			lbFlat.Appearance.Font = (Font)componentResourceManager.GetObject("lbFlat.Appearance.Font");
			lbFlat.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbFlat.Appearance.FontSizeDelta");
			lbFlat.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbFlat.Appearance.FontStyleDelta");
			lbFlat.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbFlat.Appearance.ForeColor");
			lbFlat.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbFlat.Appearance.GradientMode");
			lbFlat.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFlat.Appearance.HoverImage");
			lbFlat.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbFlat.Appearance.Image");
			lbFlat.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFlat.Appearance.PressedImage");
			lbFlat.Name = "lbFlat";
			componentResourceManager.ApplyResources(tbHome, "tbHome");
			tbHome.Name = "tbHome";
			tbHome.Properties.AccessibleDescription = componentResourceManager.GetString("tbHome.Properties.AccessibleDescription");
			tbHome.Properties.AccessibleName = componentResourceManager.GetString("tbHome.Properties.AccessibleName");
			tbHome.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbHome.Properties.Appearance.Font");
			tbHome.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbHome.Properties.Appearance.FontSizeDelta");
			tbHome.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbHome.Properties.Appearance.FontStyleDelta");
			tbHome.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbHome.Properties.Appearance.GradientMode");
			tbHome.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbHome.Properties.Appearance.Image");
			tbHome.Properties.Appearance.Options.UseFont = true;
			tbHome.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbHome.Properties.AutoHeight");
			tbHome.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbHome.Properties.Mask.AutoComplete");
			tbHome.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbHome.Properties.Mask.BeepOnError");
			tbHome.Properties.Mask.EditMask = componentResourceManager.GetString("tbHome.Properties.Mask.EditMask");
			tbHome.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbHome.Properties.Mask.IgnoreMaskBlank");
			tbHome.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbHome.Properties.Mask.MaskType");
			tbHome.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbHome.Properties.Mask.PlaceHolder");
			tbHome.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbHome.Properties.Mask.SaveLiteral");
			tbHome.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbHome.Properties.Mask.ShowPlaceHolders");
			tbHome.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbHome.Properties.Mask.UseMaskAsDisplayFormat");
			tbHome.Properties.NullValuePrompt = componentResourceManager.GetString("tbHome.Properties.NullValuePrompt");
			tbHome.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbHome.Properties.NullValuePromptShowForEmptyValue");
			tbHome.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbHome, "lbHome");
			lbHome.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbHome.Appearance.DisabledImage");
			lbHome.Appearance.Font = (Font)componentResourceManager.GetObject("lbHome.Appearance.Font");
			lbHome.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbHome.Appearance.FontSizeDelta");
			lbHome.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbHome.Appearance.FontStyleDelta");
			lbHome.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbHome.Appearance.ForeColor");
			lbHome.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbHome.Appearance.GradientMode");
			lbHome.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbHome.Appearance.HoverImage");
			lbHome.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbHome.Appearance.Image");
			lbHome.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbHome.Appearance.PressedImage");
			lbHome.Name = "lbHome";
			componentResourceManager.ApplyResources(lbStreet, "lbStreet");
			lbStreet.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbStreet.Appearance.DisabledImage");
			lbStreet.Appearance.Font = (Font)componentResourceManager.GetObject("lbStreet.Appearance.Font");
			lbStreet.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbStreet.Appearance.FontSizeDelta");
			lbStreet.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbStreet.Appearance.FontStyleDelta");
			lbStreet.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbStreet.Appearance.ForeColor");
			lbStreet.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbStreet.Appearance.GradientMode");
			lbStreet.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbStreet.Appearance.HoverImage");
			lbStreet.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbStreet.Appearance.Image");
			lbStreet.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbStreet.Appearance.PressedImage");
			lbStreet.Name = "lbStreet";
			lbStreet.Tag = "";
			componentResourceManager.ApplyResources(cbStreet, "cbStreet");
			cbStreet.Name = "cbStreet";
			cbStreet.Properties.AccessibleDescription = componentResourceManager.GetString("cbStreet.Properties.AccessibleDescription");
			cbStreet.Properties.AccessibleName = componentResourceManager.GetString("cbStreet.Properties.AccessibleName");
			cbStreet.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbStreet.Properties.Appearance.Font");
			cbStreet.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbStreet.Properties.Appearance.FontSizeDelta");
			cbStreet.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbStreet.Properties.Appearance.FontStyleDelta");
			cbStreet.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbStreet.Properties.Appearance.GradientMode");
			cbStreet.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbStreet.Properties.Appearance.Image");
			cbStreet.Properties.Appearance.Options.UseFont = true;
			cbStreet.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbStreet.Properties.AutoHeight");
			cbStreet.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbStreet.Properties.Buttons"))
			});
			cbStreet.Properties.NullValuePrompt = componentResourceManager.GetString("cbStreet.Properties.NullValuePrompt");
			cbStreet.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbStreet.Properties.NullValuePromptShowForEmptyValue");
			cbStreet.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbDistrict, "lbDistrict");
			lbDistrict.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbDistrict.Appearance.DisabledImage");
			lbDistrict.Appearance.Font = (Font)componentResourceManager.GetObject("lbDistrict.Appearance.Font");
			lbDistrict.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbDistrict.Appearance.FontSizeDelta");
			lbDistrict.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbDistrict.Appearance.FontStyleDelta");
			lbDistrict.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbDistrict.Appearance.ForeColor");
			lbDistrict.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbDistrict.Appearance.GradientMode");
			lbDistrict.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbDistrict.Appearance.HoverImage");
			lbDistrict.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbDistrict.Appearance.Image");
			lbDistrict.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbDistrict.Appearance.PressedImage");
			lbDistrict.Name = "lbDistrict";
			componentResourceManager.ApplyResources(cbDistrict, "cbDistrict");
			cbDistrict.Name = "cbDistrict";
			cbDistrict.Properties.AccessibleDescription = componentResourceManager.GetString("cbDistrict.Properties.AccessibleDescription");
			cbDistrict.Properties.AccessibleName = componentResourceManager.GetString("cbDistrict.Properties.AccessibleName");
			cbDistrict.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbDistrict.Properties.Appearance.Font");
			cbDistrict.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbDistrict.Properties.Appearance.FontSizeDelta");
			cbDistrict.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbDistrict.Properties.Appearance.FontStyleDelta");
			cbDistrict.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbDistrict.Properties.Appearance.GradientMode");
			cbDistrict.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbDistrict.Properties.Appearance.Image");
			cbDistrict.Properties.Appearance.Options.UseFont = true;
			cbDistrict.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbDistrict.Properties.AutoHeight");
			cbDistrict.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbDistrict.Properties.Buttons"))
			});
			cbDistrict.Properties.NullValuePrompt = componentResourceManager.GetString("cbDistrict.Properties.NullValuePrompt");
			cbDistrict.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbDistrict.Properties.NullValuePromptShowForEmptyValue");
			cbDistrict.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbCity, "lbCity");
			lbCity.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCity.Appearance.DisabledImage");
			lbCity.Appearance.Font = (Font)componentResourceManager.GetObject("lbCity.Appearance.Font");
			lbCity.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbCity.Appearance.FontSizeDelta");
			lbCity.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbCity.Appearance.FontStyleDelta");
			lbCity.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbCity.Appearance.ForeColor");
			lbCity.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbCity.Appearance.GradientMode");
			lbCity.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCity.Appearance.HoverImage");
			lbCity.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbCity.Appearance.Image");
			lbCity.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCity.Appearance.PressedImage");
			lbCity.Name = "lbCity";
			componentResourceManager.ApplyResources(cbCity, "cbCity");
			cbCity.Name = "cbCity";
			cbCity.Properties.AccessibleDescription = componentResourceManager.GetString("cbCity.Properties.AccessibleDescription");
			cbCity.Properties.AccessibleName = componentResourceManager.GetString("cbCity.Properties.AccessibleName");
			cbCity.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbCity.Properties.Appearance.Font");
			cbCity.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbCity.Properties.Appearance.FontSizeDelta");
			cbCity.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbCity.Properties.Appearance.FontStyleDelta");
			cbCity.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbCity.Properties.Appearance.GradientMode");
			cbCity.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbCity.Properties.Appearance.Image");
			cbCity.Properties.Appearance.Options.UseFont = true;
			cbCity.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbCity.Properties.AutoHeight");
			cbCity.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbCity.Properties.Buttons"))
			});
			cbCity.Properties.NullValuePrompt = componentResourceManager.GetString("cbCity.Properties.NullValuePrompt");
			cbCity.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbCity.Properties.NullValuePromptShowForEmptyValue");
			cbCity.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbRegion, "lbRegion");
			lbRegion.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbRegion.Appearance.DisabledImage");
			lbRegion.Appearance.Font = (Font)componentResourceManager.GetObject("lbRegion.Appearance.Font");
			lbRegion.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbRegion.Appearance.FontSizeDelta");
			lbRegion.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbRegion.Appearance.FontStyleDelta");
			lbRegion.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbRegion.Appearance.ForeColor");
			lbRegion.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbRegion.Appearance.GradientMode");
			lbRegion.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbRegion.Appearance.HoverImage");
			lbRegion.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbRegion.Appearance.Image");
			lbRegion.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbRegion.Appearance.PressedImage");
			lbRegion.Name = "lbRegion";
			componentResourceManager.ApplyResources(cbRegion, "cbRegion");
			cbRegion.Name = "cbRegion";
			cbRegion.Properties.AccessibleDescription = componentResourceManager.GetString("cbRegion.Properties.AccessibleDescription");
			cbRegion.Properties.AccessibleName = componentResourceManager.GetString("cbRegion.Properties.AccessibleName");
			cbRegion.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbRegion.Properties.Appearance.Font");
			cbRegion.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbRegion.Properties.Appearance.FontSizeDelta");
			cbRegion.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbRegion.Properties.Appearance.FontStyleDelta");
			cbRegion.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbRegion.Properties.Appearance.GradientMode");
			cbRegion.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbRegion.Properties.Appearance.Image");
			cbRegion.Properties.Appearance.Options.UseFont = true;
			cbRegion.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbRegion.Properties.AutoHeight");
			cbRegion.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbRegion.Properties.Buttons"))
			});
			cbRegion.Properties.NullValuePrompt = componentResourceManager.GetString("cbRegion.Properties.NullValuePrompt");
			cbRegion.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbRegion.Properties.NullValuePromptShowForEmptyValue");
			cbRegion.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbCountry, "lbCountry");
			lbCountry.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCountry.Appearance.DisabledImage");
			lbCountry.Appearance.Font = (Font)componentResourceManager.GetObject("lbCountry.Appearance.Font");
			lbCountry.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbCountry.Appearance.FontSizeDelta");
			lbCountry.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbCountry.Appearance.FontStyleDelta");
			lbCountry.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbCountry.Appearance.ForeColor");
			lbCountry.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbCountry.Appearance.GradientMode");
			lbCountry.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCountry.Appearance.HoverImage");
			lbCountry.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbCountry.Appearance.Image");
			lbCountry.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbCountry.Appearance.PressedImage");
			lbCountry.Name = "lbCountry";
			componentResourceManager.ApplyResources(cbCountry, "cbCountry");
			cbCountry.Name = "cbCountry";
			cbCountry.Properties.AccessibleDescription = componentResourceManager.GetString("cbCountry.Properties.AccessibleDescription");
			cbCountry.Properties.AccessibleName = componentResourceManager.GetString("cbCountry.Properties.AccessibleName");
			cbCountry.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("cbCountry.Properties.Appearance.Font");
			cbCountry.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("cbCountry.Properties.Appearance.FontSizeDelta");
			cbCountry.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("cbCountry.Properties.Appearance.FontStyleDelta");
			cbCountry.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("cbCountry.Properties.Appearance.GradientMode");
			cbCountry.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("cbCountry.Properties.Appearance.Image");
			cbCountry.Properties.Appearance.Options.UseFont = true;
			cbCountry.Properties.AutoHeight = (bool)componentResourceManager.GetObject("cbCountry.Properties.AutoHeight");
			cbCountry.Properties.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbCountry.Properties.Buttons"))
			});
			cbCountry.Properties.NullValuePrompt = componentResourceManager.GetString("cbCountry.Properties.NullValuePrompt");
			cbCountry.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("cbCountry.Properties.NullValuePromptShowForEmptyValue");
			cbCountry.SelectedIndexChanged += TbValueChanged;
			componentResourceManager.ApplyResources(tbLastName, "tbLastName");
			tbLastName.Name = "tbLastName";
			tbLastName.Properties.AccessibleDescription = componentResourceManager.GetString("tbLastName.Properties.AccessibleDescription");
			tbLastName.Properties.AccessibleName = componentResourceManager.GetString("tbLastName.Properties.AccessibleName");
			tbLastName.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbLastName.Properties.Appearance.Font");
			tbLastName.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontSizeDelta");
			tbLastName.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbLastName.Properties.Appearance.FontStyleDelta");
			tbLastName.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbLastName.Properties.Appearance.GradientMode");
			tbLastName.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbLastName.Properties.Appearance.Image");
			tbLastName.Properties.Appearance.Options.UseFont = true;
			tbLastName.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbLastName.Properties.AutoHeight");
			tbLastName.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbLastName.Properties.Mask.AutoComplete");
			tbLastName.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbLastName.Properties.Mask.BeepOnError");
			tbLastName.Properties.Mask.EditMask = componentResourceManager.GetString("tbLastName.Properties.Mask.EditMask");
			tbLastName.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbLastName.Properties.Mask.IgnoreMaskBlank");
			tbLastName.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbLastName.Properties.Mask.MaskType");
			tbLastName.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbLastName.Properties.Mask.PlaceHolder");
			tbLastName.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbLastName.Properties.Mask.SaveLiteral");
			tbLastName.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbLastName.Properties.Mask.ShowPlaceHolders");
			tbLastName.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbLastName.Properties.Mask.UseMaskAsDisplayFormat");
			tbLastName.Properties.NullValuePrompt = componentResourceManager.GetString("tbLastName.Properties.NullValuePrompt");
			tbLastName.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbLastName.Properties.NullValuePromptShowForEmptyValue");
			tbLastName.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbLastName, "lbLastName");
			lbLastName.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbLastName.Appearance.DisabledImage");
			lbLastName.Appearance.Font = (Font)componentResourceManager.GetObject("lbLastName.Appearance.Font");
			lbLastName.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbLastName.Appearance.FontSizeDelta");
			lbLastName.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbLastName.Appearance.FontStyleDelta");
			lbLastName.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbLastName.Appearance.ForeColor");
			lbLastName.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbLastName.Appearance.GradientMode");
			lbLastName.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbLastName.Appearance.HoverImage");
			lbLastName.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbLastName.Appearance.Image");
			lbLastName.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbLastName.Appearance.PressedImage");
			lbLastName.Name = "lbLastName";
			componentResourceManager.ApplyResources(tbFirstName, "tbFirstName");
			tbFirstName.Name = "tbFirstName";
			tbFirstName.Properties.AccessibleDescription = componentResourceManager.GetString("tbFirstName.Properties.AccessibleDescription");
			tbFirstName.Properties.AccessibleName = componentResourceManager.GetString("tbFirstName.Properties.AccessibleName");
			tbFirstName.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Font");
			tbFirstName.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontSizeDelta");
			tbFirstName.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbFirstName.Properties.Appearance.FontStyleDelta");
			tbFirstName.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbFirstName.Properties.Appearance.GradientMode");
			tbFirstName.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbFirstName.Properties.Appearance.Image");
			tbFirstName.Properties.Appearance.Options.UseFont = true;
			tbFirstName.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbFirstName.Properties.AutoHeight");
			tbFirstName.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbFirstName.Properties.Mask.AutoComplete");
			tbFirstName.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbFirstName.Properties.Mask.BeepOnError");
			tbFirstName.Properties.Mask.EditMask = componentResourceManager.GetString("tbFirstName.Properties.Mask.EditMask");
			tbFirstName.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbFirstName.Properties.Mask.IgnoreMaskBlank");
			tbFirstName.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbFirstName.Properties.Mask.MaskType");
			tbFirstName.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbFirstName.Properties.Mask.PlaceHolder");
			tbFirstName.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbFirstName.Properties.Mask.SaveLiteral");
			tbFirstName.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbFirstName.Properties.Mask.ShowPlaceHolders");
			tbFirstName.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbFirstName.Properties.Mask.UseMaskAsDisplayFormat");
			tbFirstName.Properties.NullValuePrompt = componentResourceManager.GetString("tbFirstName.Properties.NullValuePrompt");
			tbFirstName.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbFirstName.Properties.NullValuePromptShowForEmptyValue");
			tbFirstName.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbFirstName, "lbFirstName");
			lbFirstName.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFirstName.Appearance.DisabledImage");
			lbFirstName.Appearance.Font = (Font)componentResourceManager.GetObject("lbFirstName.Appearance.Font");
			lbFirstName.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbFirstName.Appearance.FontSizeDelta");
			lbFirstName.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbFirstName.Appearance.FontStyleDelta");
			lbFirstName.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbFirstName.Appearance.ForeColor");
			lbFirstName.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbFirstName.Appearance.GradientMode");
			lbFirstName.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFirstName.Appearance.HoverImage");
			lbFirstName.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbFirstName.Appearance.Image");
			lbFirstName.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbFirstName.Appearance.PressedImage");
			lbFirstName.Name = "lbFirstName";
			componentResourceManager.ApplyResources(tbSurname, "tbSurname");
			tbSurname.Name = "tbSurname";
			tbSurname.Properties.AccessibleDescription = componentResourceManager.GetString("tbSurname.Properties.AccessibleDescription");
			tbSurname.Properties.AccessibleName = componentResourceManager.GetString("tbSurname.Properties.AccessibleName");
			tbSurname.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("tbSurname.Properties.Appearance.Font");
			tbSurname.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontSizeDelta");
			tbSurname.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("tbSurname.Properties.Appearance.FontStyleDelta");
			tbSurname.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("tbSurname.Properties.Appearance.GradientMode");
			tbSurname.Properties.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("tbSurname.Properties.Appearance.Image");
			tbSurname.Properties.Appearance.Options.UseFont = true;
			tbSurname.Properties.AutoHeight = (bool)componentResourceManager.GetObject("tbSurname.Properties.AutoHeight");
			tbSurname.Properties.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("tbSurname.Properties.Mask.AutoComplete");
			tbSurname.Properties.Mask.BeepOnError = (bool)componentResourceManager.GetObject("tbSurname.Properties.Mask.BeepOnError");
			tbSurname.Properties.Mask.EditMask = componentResourceManager.GetString("tbSurname.Properties.Mask.EditMask");
			tbSurname.Properties.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("tbSurname.Properties.Mask.IgnoreMaskBlank");
			tbSurname.Properties.Mask.MaskType = (MaskType)componentResourceManager.GetObject("tbSurname.Properties.Mask.MaskType");
			tbSurname.Properties.Mask.PlaceHolder = (char)componentResourceManager.GetObject("tbSurname.Properties.Mask.PlaceHolder");
			tbSurname.Properties.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("tbSurname.Properties.Mask.SaveLiteral");
			tbSurname.Properties.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("tbSurname.Properties.Mask.ShowPlaceHolders");
			tbSurname.Properties.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("tbSurname.Properties.Mask.UseMaskAsDisplayFormat");
			tbSurname.Properties.NullValuePrompt = componentResourceManager.GetString("tbSurname.Properties.NullValuePrompt");
			tbSurname.Properties.NullValuePromptShowForEmptyValue = (bool)componentResourceManager.GetObject("tbSurname.Properties.NullValuePromptShowForEmptyValue");
			tbSurname.TextChanged += TbValueChanged;
			componentResourceManager.ApplyResources(lbSurname, "lbSurname");
			lbSurname.Appearance.DisabledImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSurname.Appearance.DisabledImage");
			lbSurname.Appearance.Font = (Font)componentResourceManager.GetObject("lbSurname.Appearance.Font");
			lbSurname.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("lbSurname.Appearance.FontSizeDelta");
			lbSurname.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("lbSurname.Appearance.FontStyleDelta");
			lbSurname.Appearance.ForeColor = (Color)componentResourceManager.GetObject("lbSurname.Appearance.ForeColor");
			lbSurname.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("lbSurname.Appearance.GradientMode");
			lbSurname.Appearance.HoverImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSurname.Appearance.HoverImage");
			lbSurname.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("lbSurname.Appearance.Image");
			lbSurname.Appearance.PressedImage = (System.Drawing.Image)componentResourceManager.GetObject("lbSurname.Appearance.PressedImage");
			lbSurname.Name = "lbSurname";
			componentResourceManager.ApplyResources(gcImagesFullFace, "gcImagesFullFace");
			gcImagesFullFace.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleDescription");
			gcImagesFullFace.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.AccessibleName");
			gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.AllowHtmlTextInToolTip");
			gcImagesFullFace.EmbeddedNavigator.Anchor = (AnchorStyles)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.Anchor");
			gcImagesFullFace.EmbeddedNavigator.BackgroundImage = (System.Drawing.Image)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImage");
			gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.BackgroundImageLayout");
			gcImagesFullFace.EmbeddedNavigator.ImeMode = (ImeMode)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ImeMode");
			gcImagesFullFace.EmbeddedNavigator.MaximumSize = (Size)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.MaximumSize");
			gcImagesFullFace.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.TextLocation");
			gcImagesFullFace.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcImagesFullFace.EmbeddedNavigator.ToolTip");
			gcImagesFullFace.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType)componentResourceManager.GetObject("gcImagesFullFace.EmbeddedNavigator.ToolTipIconType");
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
			lvImagesFullFace.CardMinSize = new Size(316, 378);
			lvImagesFullFace.CardVertInterval = 0;
			lvImagesFullFace.Columns.AddRange(new LayoutViewColumn[5]
			{
		colImage,
		colName,
		colImageComment,
		colImageID,
		colIsMain
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
			lvImagesFullFace.OptionsView.ViewMode = LayoutViewMode.Carousel;
			lvImagesFullFace.SortInfo.AddRange(new GridColumnSortInfo[1]
			{
		new GridColumnSortInfo(colIsMain, ColumnSortOrder.Descending)
			});
			lvImagesFullFace.TemplateCard = layoutViewCard1;
			lvImagesFullFace.CustomDrawCardCaption += lvImagesFullFace_CustomDrawCardCaption;
			lvImagesFullFace.CellValueChanged += lvImagesFullFace_CellValueChanged;
			colImage.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colImage.AppearanceCell.Font");
			colImage.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colImage.AppearanceCell.FontSizeDelta");
			colImage.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colImage.AppearanceCell.FontStyleDelta");
			colImage.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colImage.AppearanceCell.GradientMode");
			colImage.AppearanceCell.Image = (System.Drawing.Image)componentResourceManager.GetObject("colImage.AppearanceCell.Image");
			colImage.AppearanceCell.Options.UseFont = true;
			colImage.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colImage.AppearanceHeader.Font");
			colImage.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colImage.AppearanceHeader.FontSizeDelta");
			colImage.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colImage.AppearanceHeader.FontStyleDelta");
			colImage.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colImage.AppearanceHeader.GradientMode");
			colImage.AppearanceHeader.Image = (System.Drawing.Image)componentResourceManager.GetObject("colImage.AppearanceHeader.Image");
			colImage.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colImage, "colImage");
			colImage.ColumnEdit = repositoryItemPictureEdit1;
			colImage.FieldName = "Image";
			colImage.LayoutViewField = layoutViewField_layoutViewColumn1;
			colImage.Name = "colImage";
			colImage.OptionsColumn.AllowMove = false;
			colImage.OptionsColumn.AllowShowHide = false;
			colImage.OptionsColumn.AllowSize = false;
			layoutViewField_layoutViewColumn1.EditorPreferredWidth = 309;
			layoutViewField_layoutViewColumn1.Location = new Point(0, 0);
			layoutViewField_layoutViewColumn1.Name = "layoutViewField_layoutViewColumn1";
			layoutViewField_layoutViewColumn1.Padding = new Padding(0, 0, 0, 0);
			layoutViewField_layoutViewColumn1.Size = new Size(316, 22);
			layoutViewField_layoutViewColumn1.TextSize = new Size(7, 13);
			colName.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colName.AppearanceCell.Font");
			colName.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colName.AppearanceCell.FontSizeDelta");
			colName.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colName.AppearanceCell.FontStyleDelta");
			colName.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colName.AppearanceCell.GradientMode");
			colName.AppearanceCell.Image = (System.Drawing.Image)componentResourceManager.GetObject("colName.AppearanceCell.Image");
			colName.AppearanceCell.Options.UseFont = true;
			colName.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colName.AppearanceHeader.Font");
			colName.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colName.AppearanceHeader.FontSizeDelta");
			colName.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colName.AppearanceHeader.FontStyleDelta");
			colName.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colName.AppearanceHeader.GradientMode");
			colName.AppearanceHeader.Image = (System.Drawing.Image)componentResourceManager.GetObject("colName.AppearanceHeader.Image");
			colName.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colName, "colName");
			colName.FieldName = "Name";
			colName.LayoutViewField = layoutViewField_layoutViewColumn2_1;
			colName.Name = "colName";
			colName.OptionsColumn.AllowMove = false;
			colName.OptionsColumn.AllowSize = false;
			layoutViewField_layoutViewColumn2_1.EditorPreferredWidth = 239;
			layoutViewField_layoutViewColumn2_1.Location = new Point(0, 22);
			layoutViewField_layoutViewColumn2_1.Name = "layoutViewField_layoutViewColumn2_1";
			layoutViewField_layoutViewColumn2_1.Padding = new Padding(0, 0, 0, 0);
			layoutViewField_layoutViewColumn2_1.Size = new Size(316, 16);
			layoutViewField_layoutViewColumn2_1.TextSize = new Size(31, 13);
			colImageComment.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colImageComment.AppearanceCell.Font");
			colImageComment.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colImageComment.AppearanceCell.FontSizeDelta");
			colImageComment.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colImageComment.AppearanceCell.FontStyleDelta");
			colImageComment.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colImageComment.AppearanceCell.GradientMode");
			colImageComment.AppearanceCell.Image = (System.Drawing.Image)componentResourceManager.GetObject("colImageComment.AppearanceCell.Image");
			colImageComment.AppearanceCell.Options.UseFont = true;
			colImageComment.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colImageComment.AppearanceHeader.Font");
			colImageComment.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontSizeDelta");
			colImageComment.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colImageComment.AppearanceHeader.FontStyleDelta");
			colImageComment.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colImageComment.AppearanceHeader.GradientMode");
			colImageComment.AppearanceHeader.Image = (System.Drawing.Image)componentResourceManager.GetObject("colImageComment.AppearanceHeader.Image");
			colImageComment.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colImageComment, "colImageComment");
			colImageComment.FieldName = "Comment";
			colImageComment.LayoutViewField = layoutViewField_layoutViewColumn2;
			colImageComment.Name = "colImageComment";
			colImageComment.OptionsColumn.AllowMove = false;
			colImageComment.OptionsColumn.AllowSize = false;
			layoutViewField_layoutViewColumn2.EditorPreferredWidth = 251;
			layoutViewField_layoutViewColumn2.Location = new Point(0, 38);
			layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
			layoutViewField_layoutViewColumn2.Padding = new Padding(0, 0, 0, 0);
			layoutViewField_layoutViewColumn2.Size = new Size(316, 16);
			layoutViewField_layoutViewColumn2.TextSize = new Size(49, 13);
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
			layoutViewField_layoutViewColumn1_1.Size = new Size(427, 100);
			layoutViewField_layoutViewColumn1_1.TextSize = new Size(77, 20);
			layoutViewField_layoutViewColumn1_1.TextToControlDistance = 0;
			colIsMain.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colIsMain.AppearanceCell.Font");
			colIsMain.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colIsMain.AppearanceCell.FontSizeDelta");
			colIsMain.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colIsMain.AppearanceCell.FontStyleDelta");
			colIsMain.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colIsMain.AppearanceCell.GradientMode");
			colIsMain.AppearanceCell.Image = (System.Drawing.Image)componentResourceManager.GetObject("colIsMain.AppearanceCell.Image");
			colIsMain.AppearanceCell.Options.UseFont = true;
			colIsMain.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colIsMain.AppearanceHeader.Font");
			colIsMain.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colIsMain.AppearanceHeader.FontSizeDelta");
			colIsMain.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colIsMain.AppearanceHeader.FontStyleDelta");
			colIsMain.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colIsMain.AppearanceHeader.GradientMode");
			colIsMain.AppearanceHeader.Image = (System.Drawing.Image)componentResourceManager.GetObject("colIsMain.AppearanceHeader.Image");
			colIsMain.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colIsMain, "colIsMain");
			colIsMain.ColumnEdit = repositoryItemCheckEdit1;
			colIsMain.FieldName = "IsMain";
			colIsMain.LayoutViewField = layoutViewField_layoutViewColumn1_10;
			colIsMain.Name = "colIsMain";
			colIsMain.OptionsColumn.AllowMove = false;
			colIsMain.OptionsColumn.AllowSize = false;
			colIsMain.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			layoutViewField_layoutViewColumn1_10.EditorPreferredWidth = 237;
			layoutViewField_layoutViewColumn1_10.Location = new Point(0, 54);
			layoutViewField_layoutViewColumn1_10.Name = "layoutViewField_layoutViewColumn1_10";
			layoutViewField_layoutViewColumn1_10.Size = new Size(316, 20);
			layoutViewField_layoutViewColumn1_10.TextSize = new Size(57, 13);
			layoutViewField_layoutViewColumn1_10.TextToControlDistance = 0;
			componentResourceManager.ApplyResources(layoutViewCard1, "layoutViewCard1");
			layoutViewCard1.ExpandButtonLocation = GroupElementLocation.AfterText;
			layoutViewCard1.Items.AddRange(new BaseLayoutItem[4]
			{
		layoutViewField_layoutViewColumn1,
		layoutViewField_layoutViewColumn2_1,
		layoutViewField_layoutViewColumn2,
		layoutViewField_layoutViewColumn1_10
			});
			layoutViewCard1.Name = "layoutViewTemplateCard";
			layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
			layoutViewCard1.Padding = new Padding(0, 0, 0, 0);
			componentResourceManager.ApplyResources(repositoryItemButtonEdit1, "repositoryItemButtonEdit1");
			componentResourceManager.ApplyResources(appearanceObject, "serializableAppearanceObject1");
			repositoryItemButtonEdit1.Buttons.AddRange(new EditorButton[1]
			{
		new EditorButton((ButtonPredefines) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons"), componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons1"), (int) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons2"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons3"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons4"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons5"), (ImageLocation) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons6"), (System.Drawing.Image) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons7"), new KeyShortcut(System.Windows.Forms.Keys.None), appearanceObject, componentResourceManager.GetString("repositoryItemButtonEdit1.Buttons8"), componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons9"), (SuperToolTip) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons10"), (bool) componentResourceManager.GetObject("repositoryItemButtonEdit1.Buttons11"))
			});
			repositoryItemButtonEdit1.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.AutoComplete");
			repositoryItemButtonEdit1.Mask.BeepOnError = (bool)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.BeepOnError");
			repositoryItemButtonEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemButtonEdit1.Mask.EditMask");
			repositoryItemButtonEdit1.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.IgnoreMaskBlank");
			repositoryItemButtonEdit1.Mask.MaskType = (MaskType)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.MaskType");
			repositoryItemButtonEdit1.Mask.PlaceHolder = (char)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.PlaceHolder");
			repositoryItemButtonEdit1.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.SaveLiteral");
			repositoryItemButtonEdit1.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.ShowPlaceHolders");
			repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("repositoryItemButtonEdit1.Mask.UseMaskAsDisplayFormat");
			repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
			repositoryItemButtonEdit1.TextEditStyle = TextEditStyles.DisableTextEditor;
			componentResourceManager.ApplyResources(gridView1, "gridView1");
			gridView1.GridControl = gcImagesFullFace;
			gridView1.Name = "gridView1";
			componentResourceManager.ApplyResources(btFile, "btFile");
			btFile.Appearance.Font = (Font)componentResourceManager.GetObject("btFile.Appearance.Font");
			btFile.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btFile.Appearance.FontSizeDelta");
			btFile.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btFile.Appearance.FontStyleDelta");
			btFile.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btFile.Appearance.GradientMode");
			btFile.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btFile.Appearance.Image");
			btFile.Appearance.Options.UseFont = true;
			btFile.Image = (System.Drawing.Image)componentResourceManager.GetObject("btFile.Image");
			btFile.Name = "btFile";
			btFile.Click += btFile_Click;
			componentResourceManager.ApplyResources(btEditPicture, "btEditPicture");
			btEditPicture.Appearance.Font = (Font)componentResourceManager.GetObject("btEditPicture.Appearance.Font");
			btEditPicture.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btEditPicture.Appearance.FontSizeDelta");
			btEditPicture.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btEditPicture.Appearance.FontStyleDelta");
			btEditPicture.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btEditPicture.Appearance.GradientMode");
			btEditPicture.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btEditPicture.Appearance.Image");
			btEditPicture.Appearance.Options.UseFont = true;
			btEditPicture.Image = (System.Drawing.Image)componentResourceManager.GetObject("btEditPicture.Image");
			btEditPicture.Name = "btEditPicture";
			btEditPicture.Click += btEditPicture_Click;
			componentResourceManager.ApplyResources(btCamera, "btCamera");
			btCamera.Appearance.Font = (Font)componentResourceManager.GetObject("btCamera.Appearance.Font");
			btCamera.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btCamera.Appearance.FontSizeDelta");
			btCamera.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btCamera.Appearance.FontStyleDelta");
			btCamera.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btCamera.Appearance.GradientMode");
			btCamera.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btCamera.Appearance.Image");
			btCamera.Appearance.Options.UseFont = true;
			btCamera.Image = (System.Drawing.Image)componentResourceManager.GetObject("btCamera.Image");
			btCamera.Name = "btCamera";
			btCamera.Click += btCamera_Click;
			componentResourceManager.ApplyResources(btPlayer, "btPlayer");
			btPlayer.Appearance.Font = (Font)componentResourceManager.GetObject("btPlayer.Appearance.Font");
			btPlayer.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btPlayer.Appearance.FontSizeDelta");
			btPlayer.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btPlayer.Appearance.FontStyleDelta");
			btPlayer.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btPlayer.Appearance.GradientMode");
			btPlayer.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btPlayer.Appearance.Image");
			btPlayer.Appearance.Options.UseFont = true;
			btPlayer.Image = (System.Drawing.Image)componentResourceManager.GetObject("btPlayer.Image");
			btPlayer.Name = "btPlayer";
			btPlayer.Click += btPlayer_Click;
			componentResourceManager.ApplyResources(btAdd, "btAdd");
			btAdd.Appearance.Font = (Font)componentResourceManager.GetObject("btAdd.Appearance.Font");
			btAdd.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btAdd.Appearance.FontSizeDelta");
			btAdd.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btAdd.Appearance.FontStyleDelta");
			btAdd.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btAdd.Appearance.GradientMode");
			btAdd.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btAdd.Appearance.Image");
			btAdd.Appearance.Options.UseFont = true;
			btAdd.Image = Resources.Actions_list_add_user_icon36;
			btAdd.Name = "btAdd";
			btAdd.Click += btAdd_Click;
			componentResourceManager.ApplyResources(btDelete, "btDelete");
			btDelete.Appearance.Font = (Font)componentResourceManager.GetObject("btDelete.Appearance.Font");
			btDelete.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btDelete.Appearance.FontSizeDelta");
			btDelete.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btDelete.Appearance.FontStyleDelta");
			btDelete.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btDelete.Appearance.GradientMode");
			btDelete.Appearance.Image = (System.Drawing.Image)componentResourceManager.GetObject("btDelete.Appearance.Image");
			btDelete.Appearance.Options.UseFont = true;
			btDelete.Image = Resources.Actions_list_remove_user_icon36;
			btDelete.Name = "btDelete";
			btDelete.Click += btDelete_Click;
			AcceptButton = btSave;
			componentResourceManager.ApplyResources(this, "$this");
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(btDelete);
			Controls.Add(btAdd);
			Controls.Add(btFile);
			Controls.Add(btEditPicture);
			Controls.Add(btCamera);
			Controls.Add(btPlayer);
			Controls.Add(gcImagesFullFace);
			Controls.Add(groupBox1);
			Name = "EditFaceForm";
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
			tbComment.Properties.EndInit();
			dtpBithday.Properties.CalendarTimeProperties.EndInit();
			dtpBithday.Properties.EndInit();
			tbPassport.Properties.EndInit();
			cbAccessTemplate.Properties.EndInit();
			cbSEX.Properties.EndInit();
			groupBox2.EndInit();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			tbMobile.Properties.EndInit();
			tbPhone.Properties.EndInit();
			tbFlat.Properties.EndInit();
			tbHome.Properties.EndInit();
			cbStreet.Properties.EndInit();
			cbDistrict.Properties.EndInit();
			cbCity.Properties.EndInit();
			cbRegion.Properties.EndInit();
			cbCountry.Properties.EndInit();
			tbLastName.Properties.EndInit();
			tbFirstName.Properties.EndInit();
			tbSurname.Properties.EndInit();
			gcImagesFullFace.EndInit();
			lvImagesFullFace.EndInit();
			layoutViewField_layoutViewColumn1.EndInit();
			layoutViewField_layoutViewColumn2_1.EndInit();
			layoutViewField_layoutViewColumn2.EndInit();
			layoutViewField_layoutViewColumn1_1.EndInit();
			layoutViewField_layoutViewColumn1_10.EndInit();
			layoutViewCard1.EndInit();
			repositoryItemButtonEdit1.EndInit();
			gridView1.EndInit();
			ResumeLayout(false);
		}
	}
}
