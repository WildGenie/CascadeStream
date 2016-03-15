// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmCommonCategories
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using CellValueChangedEventArgs = DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs;
using FindMode = DevExpress.XtraEditors.FindMode;

namespace CascadeManager
{
	public class FrmCommonCategories : XtraForm
	{
		private List<BcAccessCategory> _categories = new List<BcAccessCategory>();
		private List<BcObjects> _objects = new List<BcObjects>();
		private DataTable _dtCategories = new DataTable();
		private DataTable _dtInCategories = new DataTable();
		private BcAccessCategory _unCategory = new BcAccessCategory();
		private IContainer components = null;
		private bool _categoriesLoading;
		private GroupControl groupControl1;
		private GroupControl groupControl2;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private GridControl gcInCategory;
		private GridView gvInCategory;
		private GridColumn colCategoryID;
		private GridColumn colCategory;
		private RepositoryItemCheckEdit repositoryItemCheckEdit1;
		private GridColumn colnList;
		private GridColumn colWarning;
		private GridColumn colSound;
		private SimpleButton btDeleteCategory;
		private SimpleButton btAddCategory;
		private GridControl gcCategory;
		private GridView gvCategory;
		private GridColumn gridColumn9;
		private RepositoryItemCheckEdit repositoryItemCheckEdit3;
		private GridColumn gridColumn11;
		private SimpleButton btSaveCategory;
		private GridColumn colObjectID;
		private GridColumn colTableID;
		private GridColumn colObjectData;
		private SimpleButton btAccept;
		private CheckEdit chbAcceptSubObjects;
		private GridColumn gridColumn8;
		private TreeList tvObjects;
		private TreeListColumn treeListColumn1;
		private RepositoryItemCheckEdit repositoryItemCheckEdit4;
		private CheckEdit chbAcceptAll;
		private SimpleButton btAcceptAll;
		private GridColumn colDevice;

		public FrmCommonCategories()
		{
			InitializeComponent();
		}

		private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void btAccept_Click(object sender, EventArgs e)
		{
			if (gvInCategory.SelectedRowsCount > 0)
			{
				foreach (int rowHandle in gvInCategory.GetSelectedRows())
				{
					DataRow dataRow = gvInCategory.GetDataRow(rowHandle);
					foreach (BcAccessCategory bcAccessCategory in _categories)
					{
						if (Convert.ToInt32(dataRow["ID"]) == bcAccessCategory.Id)
						{
							using (List<BcObjects>.Enumerator enumerator = bcAccessCategory.Data.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									BcObjects current = enumerator.Current;
									if (current.Id == Convert.ToInt32(dataRow["ObjectID"]))
									{
										if (chbAcceptAll.Checked)
										{
											foreach (BcObjects bcObjects in bcAccessCategory.Data)
												BcObjectsData.SetData(bcObjects.Data, current.InList, current.Warning, current.Sound);
											bcAccessCategory.SaveData();
											bcAccessCategory.Save();
											break;
										}
										if (dataRow["TableID"].ToString() == Guid.Empty.ToString())
										{
											if (chbAcceptSubObjects.Checked)
												BcObjectsData.SetData(current.Data, current.InList, current.Warning, current.Sound);
											bcAccessCategory.SaveData();
											bcAccessCategory.Save();
											break;
										}
										BcObjectsData objectById = BcObjectsData.GetObjectById(current.Data, (Guid)new GuidConverter().ConvertFromString(dataRow["TableID"].ToString()));
										BcObjectsData.SetData(objectById.Data, objectById.InList, objectById.Warning, objectById.Sound);
										bcAccessCategory.SaveData();
										bcAccessCategory.Save();
										break;
									}
								}
								break;
							}
						}
					}
				}
			}
			else
			{
				int num = (int)XtraMessageBox.Show(Messages.SelectCategoriesToChange, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void frmCommonCategories_Load(object sender, EventArgs e)
		{
			_categoriesLoading = true;
			_objects = BcObjects.LoadAll();
			ControlBox = false;
			_categoriesLoading = true;
			_dtCategories.Columns.AddRange(new DataColumn[4]
			{
		new DataColumn("ID", typeof (int)),
		new DataColumn("Name"),
		new DataColumn("ObjectData", typeof (byte[])),
		new DataColumn("Changed", typeof (bool))
			});
			_dtInCategories.Columns.AddRange(new DataColumn[8]
			{
		new DataColumn("ID", typeof (int)),
		new DataColumn("Name"),
		new DataColumn("InList", typeof (bool)),
		new DataColumn("Warning", typeof (bool)),
		new DataColumn("Sound", typeof (bool)),
		new DataColumn("Device", typeof (bool)),
		new DataColumn("ObjectID"),
		new DataColumn("TableID")
			});
			_categories = BcAccessCategory.LoadAll();
			gcCategory.DataSource = _dtCategories;
			foreach (BcAccessCategory bcAccessCategory in _categories)
			{
				if (bcAccessCategory.InCategory)
				{
					bcAccessCategory.GetData();
					_dtCategories.Rows.Add((object)bcAccessCategory.Id, (object)bcAccessCategory.Name, (object)bcAccessCategory.ObjectData, (object)false);
				}
				else
				{
					_unCategory = bcAccessCategory;
					_unCategory.Name = Messages.NonCategory;
				}
			}
			if (!_categories.Contains(_unCategory))
				_categories.Add(_unCategory);
			_unCategory.Name = Messages.NonCategory;
			_unCategory.InCategory = false;
			_categoriesLoading = true;
			gcInCategory.DataSource = _dtInCategories;
			tvObjects.BeginUnboundLoad();
			tvObjects.Nodes.Clear();
			foreach (BcObjects bcObjects1 in _objects)
			{
				bcObjects1.GetData();
				if (bcObjects1.Name == "")
					bcObjects1.Name = "[" + Messages.ControlledObject + "]";
				TreeListNode parentNode = tvObjects.AppendNode(bcObjects1.Name, -1);
				parentNode.SetValue(0, bcObjects1.Name);
				parentNode.Tag = bcObjects1.Id;
				foreach (BcObjectsData data in bcObjects1.Data)
				{
					if (data.Name == "")
						data.Name = "[" + Messages.ControlledObject + "]";
					TreeListNode node = tvObjects.AppendNode(data.Name, parentNode);
					data.OwnerObject = bcObjects1;
					node.Tag = data;
					node.SetValue(0, data.Name);
					LoadTree(node, data, bcObjects1);
				}
				bool flag = false;
				foreach (BcAccessCategory bcAccessCategory in _categories)
				{
					foreach (BcObjects bcObjects2 in bcAccessCategory.Data)
					{
						if (bcObjects1.Id == bcObjects2.Id)
						{
							bcObjects2.Id = bcObjects1.Id;
							bcObjects2.Name = bcObjects1.Name;
							flag = true;
							bcObjects2.GetData();
							Clone(bcObjects1.Data, bcObjects2.Data);
							break;
						}
					}
					if (!flag)
					{
						BcObjects bcObjects2 = new BcObjects();
						bcObjects2.Id = bcObjects1.Id;
						bcObjects2.Name = bcObjects1.Name;
						bcObjects2.Comment = bcObjects1.Comment;
						Clone(bcObjects1.Data, bcObjects2.Data);
						bcAccessCategory.Data.Add(bcObjects2);
					}
					flag = false;
				}
			}
			tvObjects.EndUnboundLoad();
			tvObjects.CollapseAll();
			_categoriesLoading = false;
			RefreshGrid();
		}

		public void ReloadObjects()
		{
			try
			{
				tvObjects.BeginUnboundLoad();
				_objects = BcObjects.LoadAll();
				_categoriesLoading = true;
				tvObjects.Nodes.Clear();
				foreach (BcObjects bcObjects1 in _objects)
				{
					bcObjects1.GetData();
					if (bcObjects1.Name == "")
						bcObjects1.Name = "[" + Messages.ControlledObject + "]";
					TreeListNode parentNode = tvObjects.AppendNode(bcObjects1.Name, -1);
					parentNode.SetValue(0, bcObjects1.Name);
					parentNode.Tag = bcObjects1.Id;
					foreach (BcObjectsData data in bcObjects1.Data)
					{
						if (data.Name == "")
							data.Name = "[" + Messages.ControlledObject + "]";
						TreeListNode node = tvObjects.AppendNode(data.Name, parentNode);
						data.OwnerObject = bcObjects1;
						node.Tag = data;
						node.SetValue(0, data.Name);
						LoadTree(node, data, bcObjects1);
					}
					bool flag = false;
					foreach (BcAccessCategory bcAccessCategory in _categories)
					{
						foreach (BcObjects bcObjects2 in bcAccessCategory.Data)
						{
							if (bcObjects1.Id == bcObjects2.Id)
							{
								bcObjects2.Id = bcObjects1.Id;
								bcObjects2.Name = bcObjects1.Name;
								flag = true;
								bcObjects2.GetData();
								Clone(bcObjects1.Data, bcObjects2.Data);
								break;
							}
						}
						if (!flag)
							bcAccessCategory.Data.Add(new BcObjects
							{
								Id = bcObjects1.Id,
								Name = bcObjects1.Name,
								Comment = bcObjects1.Comment,
								Data = bcObjects1.Data,
								ObjectData = bcObjects1.ObjectData
							});
						flag = false;
					}
				}
				tvObjects.EndUnboundLoad();
				tvObjects.ExpandAll();
				_categoriesLoading = false;
				RefreshGrid();
			}
			catch
			{
			}
		}

		private void LoadTree(TreeListNode node, BcObjectsData data, BcObjects obj)
		{
			if (node == null)
				return;
			foreach (BcObjectsData data1 in data.Data)
			{
				data1.OwnerObject = obj;
				if (data1.Name == "")
					data1.Name = "[" + Messages.ControlledObject + "]";
				TreeListNode node1 = tvObjects.AppendNode(data1.Name, node);
				node1.SetValue(0, data1.Name);
				node1.Tag = data1;
				LoadTree(node1, data1, obj);
			}
		}

		public static void Clone(List<BcObjectsData> sourceObj, List<BcObjectsData> detsObj)
		{
			foreach (BcObjectsData sourceObj1 in sourceObj)
			{
				bool flag = false;
				foreach (BcObjectsData bcObjectsData in detsObj)
				{
					if (sourceObj1.Id == bcObjectsData.Id)
					{
						bcObjectsData.Comment = sourceObj1.Comment;
						bcObjectsData.Name = sourceObj1.Name;
						bcObjectsData.FullAccess = sourceObj1.FullAccess;
						Clone(sourceObj1.Data, bcObjectsData.Data);
						flag = true;
					}
				}
				if (!flag)
				{
					BcObjectsData resultObj = new BcObjectsData();
					Clone(sourceObj1, resultObj);
					detsObj.Add(resultObj);
				}
			}
		}

		public static void Clone(BcObjectsData sourceObj, BcObjectsData resultObj)
		{
			resultObj.Id = sourceObj.Id;
			resultObj.Data = new List<BcObjectsData>();
			resultObj.Name = sourceObj.Name;
			resultObj.Comment = sourceObj.Comment;
			resultObj.FullAccess = sourceObj.FullAccess;
			resultObj.IsParent = false;
			for (int index = 0; index < sourceObj.Data.Count; ++index)
			{
				resultObj.Data.Add(new BcObjectsData());
				Clone(sourceObj.Data[index], resultObj.Data[index]);
			}
		}

		private void RefreshGrid()
		{
			if (tvObjects.FocusedNode != null)
			{
				if (tvObjects.Nodes.IndexOf(tvObjects.FocusedNode) == -1)
				{
					BcObjectsData bcObjectsData1 = (BcObjectsData)tvObjects.FocusedNode.Tag;
					BcObjects bcObjects1 = bcObjectsData1.OwnerObject;
					_dtInCategories.Rows.Clear();
					foreach (BcAccessCategory bcAccessCategory in _categories)
					{
						BcObjectsData bcObjectsData2 = new BcObjectsData();
						if (bcAccessCategory.InCategory)
						{
							BcObjects bcObjects2 = new BcObjects();
							foreach (BcObjects bcObjects3 in bcAccessCategory.Data)
							{
								if (bcObjects3.Id == bcObjects1.Id)
								{
									bcObjectsData2 = BcObjectsData.GetObjectById(bcObjects3.Data, bcObjectsData1.Id);
									break;
								}
							}
							_dtInCategories.Rows.Add(
								bcAccessCategory.Id, 
								bcAccessCategory.Name, 
								bcObjectsData2.InList, 
								bcObjectsData2.Warning, 
								bcObjectsData2.Sound, 
								bcObjectsData2.Device,
								bcObjects1.Id, 
								bcObjectsData2.Id);
						}
					}
					BcObjects bcObjects4 = new BcObjects();
					foreach (BcObjects bcObjects2 in _unCategory.Data)
					{
						if (bcObjects2.Id == bcObjects1.Id)
						{
							bcObjects4 = bcObjects2;
							break;
						}
					}
					BcObjectsData objectById = BcObjectsData.GetObjectById(bcObjects4.Data, bcObjectsData1.Id);
					_dtInCategories.Rows.Add(
						_unCategory.Id, 
						_unCategory.Name, 
						objectById.InList, 
						objectById.Warning, 
						objectById.Sound,
						objectById.Device,
						bcObjects4.Id,
						bcObjectsData1.Id);
				}
				else
				{
					if (tvObjects.Nodes.IndexOf(tvObjects.FocusedNode) == -1)
						return;
					int num = (int)tvObjects.FocusedNode.Tag;
					_dtInCategories.Rows.Clear();
					foreach (BcAccessCategory bcAccessCategory in _categories)
					{
						if (bcAccessCategory.InCategory)
						{
							bool flag = false;
							foreach (BcObjects bcObjects in bcAccessCategory.Data)
							{
								if (bcObjects.Id == num)
								{
									flag = true;
									_dtInCategories.Rows.Add(
										bcAccessCategory.Id, 
										bcAccessCategory.Name, 
										bcObjects.InList, 
										bcObjects.Warning, 
										bcObjects.Sound,
										bcObjects.Device,
										bcObjects.Id,
										Guid.Empty.ToString());
									break;
								}
							}
							if (!flag)
							{
								foreach (BcObjects bcObjects in _objects)
								{
									if (bcObjects.Id == num)
										_dtInCategories.Rows.Add(
											bcAccessCategory.Id, 
											bcObjects.Name,
											bcObjects.InList, 
											bcObjects.Warning,
											bcObjects.Sound,
											bcObjects.Device, 
											bcObjects.Id, 
											Guid.Empty.ToString());
								}
							}
						}
					}
					BcObjects bcObjects1 = new BcObjects();
					foreach (BcObjects bcObjects2 in _unCategory.Data)
					{
						if (bcObjects2.Id == num)
						{
							bcObjects1 = bcObjects2;
							break;
						}
					}
					_dtInCategories.Rows.Add(
						_unCategory.Id, 
						_unCategory.Name, 
						bcObjects1.InList,
						bcObjects1.Warning,
						bcObjects1.Sound,
						bcObjects1.Device,
						bcObjects1.Id,
						Guid.Empty.ToString());
				}
			}
			else
				_dtInCategories.Rows.Clear();
		}

		private void tvObjects_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
		{
			if (_categoriesLoading || e.Node == null)
				return;
			RefreshGrid();
		}

		private void gvInCategory_CellValueChanging(object sender, CellValueChangedEventArgs e)
		{
			DataRow dataRow = gvInCategory.GetDataRow(e.RowHandle);
			foreach (BcAccessCategory bcAccessCategory in _categories)
			{
				if (Convert.ToInt32(dataRow["ID"]) == bcAccessCategory.Id)
				{
					foreach (BcObjects bcObjects in bcAccessCategory.Data)
					{
						if (bcObjects.Id == Convert.ToInt32(dataRow["ObjectID"]))
						{
							if (dataRow["TableID"].ToString() == Guid.Empty.ToString())
							{
								if (e.Column == colSound)
								{
									bcObjects.Sound = (bool)e.Value;
									break;
								}
								if (e.Column == colWarning)
								{
									bcObjects.Warning = (bool)e.Value;
									break;
								}
								if (e.Column == colnList)
								{
									bcObjects.InList = (bool)e.Value;
									break;
								}
								if (e.Column == colDevice)
								{
									bcObjects.Device = (bool)e.Value;
								}
								break;
							}
							BcObjectsData objectById = BcObjectsData.GetObjectById(bcObjects.Data, (Guid)new GuidConverter().ConvertFromString(dataRow["TableID"].ToString()));
							if (e.Column == colSound)
								objectById.Sound = (bool)e.Value;
							else if (e.Column == colWarning)
								objectById.Warning = (bool)e.Value;
							else if (e.Column == colnList)
								objectById.InList = (bool)e.Value;
							else if (e.Column == colDevice)
								objectById.Device = (bool)e.Value;
							break;
						}
					}
				}
			}
		}

		private void btAddCategory_Click(object sender, EventArgs e)
		{
			_dtCategories.Rows.Add((object)-1, (object)"", (object)new byte[0], (object)false);
		}

		private void btDeleteCategory_Click(object sender, EventArgs e)
		{
			if (gvCategory.SelectedRowsCount <= 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
				return;
			int[] selectedRows = gvCategory.GetSelectedRows();
			BcAccessCategory bcAccessCategory1 = new BcAccessCategory();
			foreach (BcAccessCategory bcAccessCategory2 in _categories)
			{
				if (bcAccessCategory2.Id == Convert.ToInt32(gvCategory.GetDataRow(selectedRows[0])["ID"]))
				{
					bcAccessCategory2.Delete();
					bcAccessCategory1 = bcAccessCategory2;
					break;
				}
			}
			_categories.Remove(bcAccessCategory1);
			_dtCategories.Rows.Remove(gvCategory.GetDataRow(selectedRows[0]));
			RefreshGrid();
		}

		private void btSaveCategory_Click(object sender, EventArgs e)
		{
			foreach (DataRow dataRow in (InternalDataCollectionBase)_dtCategories.Rows)
			{
				if ((bool)dataRow["Changed"])
				{
					BcAccessCategory bcAccessCategory = new BcAccessCategory();
					bcAccessCategory.Id = Convert.ToInt32(dataRow["ID"]);
					bcAccessCategory.Name = dataRow["Name"].ToString();
					bcAccessCategory.ObjectData = (byte[])dataRow["ObjectData"];
					bcAccessCategory.Data = bcAccessCategory.GetData();
					bool flag1 = false;
					if (bcAccessCategory.Id == -1)
					{
						foreach (BcObjects bcObjects1 in _objects)
						{
							bool flag2 = false;
							foreach (BcObjects bcObjects2 in bcAccessCategory.Data)
							{
								if (bcObjects1.Id == bcObjects2.Id)
								{
									bcObjects2.Id = bcObjects1.Id;
									bcObjects2.Name = bcObjects1.Name;
									flag2 = true;
									bcObjects2.GetData();
									Clone(bcObjects1.Data, bcObjects2.Data);
									break;
								}
							}
							if (!flag2)
							{
								BcObjects bcObjects2 = new BcObjects();
								bcObjects2.Id = bcObjects1.Id;
								bcObjects2.Name = bcObjects1.Name;
								bcObjects2.Comment = bcObjects1.Comment;
								Clone(bcObjects1.Data, bcObjects2.Data);
								bcAccessCategory.Data.Add(bcObjects2);
							}
						}
						bcAccessCategory.SaveData();
						flag1 = true;
					}
					bcAccessCategory.Save();
					if (flag1)
					{
						dataRow["ID"] = bcAccessCategory.Id;
						_categories.Add(bcAccessCategory);
					}
					dataRow["Changed"] = false;
				}
			}
			RefreshGrid();
		}

		private void gvCategory_CellValueChanging(object sender, CellValueChangedEventArgs e)
		{
			if (_categoriesLoading)
				return;
			gvCategory.GetDataRow(e.RowHandle)["Changed"] = true;
		}

		private void frmCommonCategories_Resize(object sender, EventArgs e)
		{
			ControlBox = false;
		}

		private void btAcceptAll_Click(object sender, EventArgs e)
		{
		}

		private void gvInCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void btAcceptAll_Click_1(object sender, EventArgs e)
		{
			_unCategory.Name = Messages.NonCategory;
			foreach (DataRow dataRow in (InternalDataCollectionBase)_dtInCategories.Rows)
			{
				foreach (BcAccessCategory bcAccessCategory in _categories)
				{
					if (Convert.ToInt32(dataRow["ID"]) == bcAccessCategory.Id)
					{
						bcAccessCategory.SaveData();
						bcAccessCategory.Save();
						break;
					}
				}
			}
		}

		private void groupControl2_Paint(object sender, PaintEventArgs e)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmCommonCategories));
			groupControl1 = new GroupControl();
			btSaveCategory = new SimpleButton();
			btDeleteCategory = new SimpleButton();
			btAddCategory = new SimpleButton();
			gcCategory = new GridControl();
			gvCategory = new GridView();
			gridColumn9 = new GridColumn();
			gridColumn11 = new GridColumn();
			colObjectData = new GridColumn();
			gridColumn8 = new GridColumn();
			repositoryItemCheckEdit3 = new RepositoryItemCheckEdit();
			groupControl2 = new GroupControl();
			btAcceptAll = new SimpleButton();
			chbAcceptAll = new CheckEdit();
			chbAcceptSubObjects = new CheckEdit();
			btAccept = new SimpleButton();
			groupBox2 = new GroupBox();
			gcInCategory = new GridControl();
			gvInCategory = new GridView();
			colCategoryID = new GridColumn();
			colCategory = new GridColumn();
			colnList = new GridColumn();
			repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
			colWarning = new GridColumn();
			colSound = new GridColumn();
			colObjectID = new GridColumn();
			colTableID = new GridColumn();
			colDevice = new GridColumn();
			groupBox1 = new GroupBox();
			tvObjects = new TreeList();
			treeListColumn1 = new TreeListColumn();
			repositoryItemCheckEdit4 = new RepositoryItemCheckEdit();
			groupControl1.BeginInit();
			groupControl1.SuspendLayout();
			gcCategory.BeginInit();
			gvCategory.BeginInit();
			repositoryItemCheckEdit3.BeginInit();
			groupControl2.BeginInit();
			groupControl2.SuspendLayout();
			chbAcceptAll.Properties.BeginInit();
			chbAcceptSubObjects.Properties.BeginInit();
			groupBox2.SuspendLayout();
			gcInCategory.BeginInit();
			gvInCategory.BeginInit();
			repositoryItemCheckEdit1.BeginInit();
			groupBox1.SuspendLayout();
			tvObjects.BeginInit();
			repositoryItemCheckEdit4.BeginInit();
			SuspendLayout();
			componentResourceManager.ApplyResources(groupControl1, "groupControl1");
			groupControl1.Appearance.Font = (Font)componentResourceManager.GetObject("groupControl1.Appearance.Font");
			groupControl1.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("groupControl1.Appearance.FontSizeDelta");
			groupControl1.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupControl1.Appearance.FontStyleDelta");
			groupControl1.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupControl1.Appearance.GradientMode");
			groupControl1.Appearance.Image = (Image)componentResourceManager.GetObject("groupControl1.Appearance.Image");
			groupControl1.Appearance.Options.UseFont = true;
			groupControl1.AppearanceCaption.Font = (Font)componentResourceManager.GetObject("groupControl1.AppearanceCaption.Font");
			groupControl1.AppearanceCaption.FontSizeDelta = (int)componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontSizeDelta");
			groupControl1.AppearanceCaption.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontStyleDelta");
			groupControl1.AppearanceCaption.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupControl1.AppearanceCaption.GradientMode");
			groupControl1.AppearanceCaption.Image = (Image)componentResourceManager.GetObject("groupControl1.AppearanceCaption.Image");
			groupControl1.AppearanceCaption.Options.UseFont = true;
			groupControl1.Controls.Add(btSaveCategory);
			groupControl1.Controls.Add(btDeleteCategory);
			groupControl1.Controls.Add(btAddCategory);
			groupControl1.Controls.Add(gcCategory);
			groupControl1.Name = "groupControl1";
			componentResourceManager.ApplyResources(btSaveCategory, "btSaveCategory");
			btSaveCategory.Appearance.Font = (Font)componentResourceManager.GetObject("btSaveCategory.Appearance.Font");
			btSaveCategory.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btSaveCategory.Appearance.FontSizeDelta");
			btSaveCategory.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btSaveCategory.Appearance.FontStyleDelta");
			btSaveCategory.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btSaveCategory.Appearance.GradientMode");
			btSaveCategory.Appearance.Image = (Image)componentResourceManager.GetObject("btSaveCategory.Appearance.Image");
			btSaveCategory.Appearance.Options.UseFont = true;
			btSaveCategory.Name = "btSaveCategory";
			btSaveCategory.Click += btSaveCategory_Click;
			componentResourceManager.ApplyResources(btDeleteCategory, "btDeleteCategory");
			btDeleteCategory.Appearance.Font = (Font)componentResourceManager.GetObject("btDeleteCategory.Appearance.Font");
			btDeleteCategory.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btDeleteCategory.Appearance.FontSizeDelta");
			btDeleteCategory.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btDeleteCategory.Appearance.FontStyleDelta");
			btDeleteCategory.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btDeleteCategory.Appearance.GradientMode");
			btDeleteCategory.Appearance.Image = (Image)componentResourceManager.GetObject("btDeleteCategory.Appearance.Image");
			btDeleteCategory.Appearance.Options.UseFont = true;
			btDeleteCategory.Name = "btDeleteCategory";
			btDeleteCategory.Click += btDeleteCategory_Click;
			componentResourceManager.ApplyResources(btAddCategory, "btAddCategory");
			btAddCategory.Appearance.Font = (Font)componentResourceManager.GetObject("btAddCategory.Appearance.Font");
			btAddCategory.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btAddCategory.Appearance.FontSizeDelta");
			btAddCategory.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btAddCategory.Appearance.FontStyleDelta");
			btAddCategory.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btAddCategory.Appearance.GradientMode");
			btAddCategory.Appearance.Image = (Image)componentResourceManager.GetObject("btAddCategory.Appearance.Image");
			btAddCategory.Appearance.Options.UseFont = true;
			btAddCategory.Name = "btAddCategory";
			btAddCategory.Click += btAddCategory_Click;
			componentResourceManager.ApplyResources(gcCategory, "gcCategory");
			gcCategory.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcCategory.EmbeddedNavigator.AccessibleDescription");
			gcCategory.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcCategory.EmbeddedNavigator.AccessibleName");
			gcCategory.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.AllowHtmlTextInToolTip");
			gcCategory.EmbeddedNavigator.Anchor = (AnchorStyles)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.Anchor");
			gcCategory.EmbeddedNavigator.BackgroundImage = (Image)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.BackgroundImage");
			gcCategory.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.BackgroundImageLayout");
			gcCategory.EmbeddedNavigator.ImeMode = (ImeMode)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.ImeMode");
			gcCategory.EmbeddedNavigator.MaximumSize = (Size)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.MaximumSize");
			gcCategory.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.TextLocation");
			gcCategory.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcCategory.EmbeddedNavigator.ToolTip");
			gcCategory.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType)componentResourceManager.GetObject("gcCategory.EmbeddedNavigator.ToolTipIconType");
			gcCategory.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcCategory.EmbeddedNavigator.ToolTipTitle");
			gcCategory.LookAndFeel.SkinName = "Office 2007 Blue";
			gcCategory.MainView = gvCategory;
			gcCategory.Name = "gcCategory";
			gcCategory.RepositoryItems.AddRange(new RepositoryItem[1]
			{
		repositoryItemCheckEdit3
			});
			gcCategory.ViewCollection.AddRange(new BaseView[1]
			{
		gvCategory
			});
			componentResourceManager.ApplyResources(gvCategory, "gvCategory");
			gvCategory.Columns.AddRange(new GridColumn[4]
			{
		gridColumn9,
		gridColumn11,
		colObjectData,
		gridColumn8
			});
			gvCategory.GridControl = gcCategory;
			gvCategory.Name = "gvCategory";
			gvCategory.OptionsCustomization.AllowFilter = false;
			gvCategory.OptionsFind.ClearFindOnClose = false;
			gvCategory.OptionsFind.FindDelay = 10000;
			gvCategory.OptionsFind.FindMode = FindMode.Always;
			gvCategory.OptionsFind.ShowCloseButton = false;
			gvCategory.OptionsSelection.MultiSelect = true;
			gvCategory.OptionsView.ShowGroupPanel = false;
			gvCategory.CellValueChanging += gvCategory_CellValueChanging;
			gridColumn9.AppearanceCell.Font = (Font)componentResourceManager.GetObject("gridColumn9.AppearanceCell.Font");
			gridColumn9.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn9.AppearanceCell.FontSizeDelta");
			gridColumn9.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn9.AppearanceCell.FontStyleDelta");
			gridColumn9.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn9.AppearanceCell.GradientMode");
			gridColumn9.AppearanceCell.Image = (Image)componentResourceManager.GetObject("gridColumn9.AppearanceCell.Image");
			gridColumn9.AppearanceCell.Options.UseFont = true;
			gridColumn9.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("gridColumn9.AppearanceHeader.Font");
			gridColumn9.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn9.AppearanceHeader.FontSizeDelta");
			gridColumn9.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn9.AppearanceHeader.FontStyleDelta");
			gridColumn9.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn9.AppearanceHeader.GradientMode");
			gridColumn9.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("gridColumn9.AppearanceHeader.Image");
			gridColumn9.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(gridColumn9, "gridColumn9");
			gridColumn9.FieldName = "ID";
			gridColumn9.Name = "gridColumn9";
			gridColumn9.OptionsColumn.AllowEdit = false;
			gridColumn9.OptionsColumn.ReadOnly = true;
			gridColumn11.AppearanceCell.Font = (Font)componentResourceManager.GetObject("gridColumn11.AppearanceCell.Font");
			gridColumn11.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn11.AppearanceCell.FontSizeDelta");
			gridColumn11.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn11.AppearanceCell.FontStyleDelta");
			gridColumn11.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn11.AppearanceCell.GradientMode");
			gridColumn11.AppearanceCell.Image = (Image)componentResourceManager.GetObject("gridColumn11.AppearanceCell.Image");
			gridColumn11.AppearanceCell.Options.UseFont = true;
			gridColumn11.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("gridColumn11.AppearanceHeader.Font");
			gridColumn11.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn11.AppearanceHeader.FontSizeDelta");
			gridColumn11.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn11.AppearanceHeader.FontStyleDelta");
			gridColumn11.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn11.AppearanceHeader.GradientMode");
			gridColumn11.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("gridColumn11.AppearanceHeader.Image");
			gridColumn11.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(gridColumn11, "gridColumn11");
			gridColumn11.FieldName = "Name";
			gridColumn11.Name = "gridColumn11";
			colObjectData.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colObjectData.AppearanceCell.Font");
			colObjectData.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colObjectData.AppearanceCell.FontSizeDelta");
			colObjectData.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colObjectData.AppearanceCell.FontStyleDelta");
			colObjectData.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colObjectData.AppearanceCell.GradientMode");
			colObjectData.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colObjectData.AppearanceCell.Image");
			colObjectData.AppearanceCell.Options.UseFont = true;
			colObjectData.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colObjectData.AppearanceHeader.Font");
			colObjectData.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colObjectData.AppearanceHeader.FontSizeDelta");
			colObjectData.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colObjectData.AppearanceHeader.FontStyleDelta");
			colObjectData.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colObjectData.AppearanceHeader.GradientMode");
			colObjectData.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colObjectData.AppearanceHeader.Image");
			colObjectData.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colObjectData, "colObjectData");
			colObjectData.FieldName = "ObjectData";
			colObjectData.Name = "colObjectData";
			gridColumn8.AppearanceCell.Font = (Font)componentResourceManager.GetObject("gridColumn8.AppearanceCell.Font");
			gridColumn8.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn8.AppearanceCell.FontSizeDelta");
			gridColumn8.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn8.AppearanceCell.FontStyleDelta");
			gridColumn8.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn8.AppearanceCell.GradientMode");
			gridColumn8.AppearanceCell.Image = (Image)componentResourceManager.GetObject("gridColumn8.AppearanceCell.Image");
			gridColumn8.AppearanceCell.Options.UseFont = true;
			gridColumn8.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("gridColumn8.AppearanceHeader.Font");
			gridColumn8.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("gridColumn8.AppearanceHeader.FontSizeDelta");
			gridColumn8.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("gridColumn8.AppearanceHeader.FontStyleDelta");
			gridColumn8.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("gridColumn8.AppearanceHeader.GradientMode");
			gridColumn8.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("gridColumn8.AppearanceHeader.Image");
			gridColumn8.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(gridColumn8, "gridColumn8");
			gridColumn8.FieldName = "Changed";
			gridColumn8.Name = "gridColumn8";
			componentResourceManager.ApplyResources(repositoryItemCheckEdit3, "repositoryItemCheckEdit3");
			repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
			componentResourceManager.ApplyResources(groupControl2, "groupControl2");
			groupControl2.Appearance.Font = (Font)componentResourceManager.GetObject("groupControl2.Appearance.Font");
			groupControl2.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("groupControl2.Appearance.FontSizeDelta");
			groupControl2.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupControl2.Appearance.FontStyleDelta");
			groupControl2.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupControl2.Appearance.GradientMode");
			groupControl2.Appearance.Image = (Image)componentResourceManager.GetObject("groupControl2.Appearance.Image");
			groupControl2.Appearance.Options.UseFont = true;
			groupControl2.AppearanceCaption.Font = (Font)componentResourceManager.GetObject("groupControl2.AppearanceCaption.Font");
			groupControl2.AppearanceCaption.FontSizeDelta = (int)componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontSizeDelta");
			groupControl2.AppearanceCaption.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontStyleDelta");
			groupControl2.AppearanceCaption.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("groupControl2.AppearanceCaption.GradientMode");
			groupControl2.AppearanceCaption.Image = (Image)componentResourceManager.GetObject("groupControl2.AppearanceCaption.Image");
			groupControl2.AppearanceCaption.Options.UseFont = true;
			groupControl2.Controls.Add(btAcceptAll);
			groupControl2.Controls.Add(chbAcceptAll);
			groupControl2.Controls.Add(chbAcceptSubObjects);
			groupControl2.Controls.Add(btAccept);
			groupControl2.Controls.Add(groupBox2);
			groupControl2.Controls.Add(groupBox1);
			groupControl2.Name = "groupControl2";
			groupControl2.Paint += groupControl2_Paint;
			componentResourceManager.ApplyResources(btAcceptAll, "btAcceptAll");
			btAcceptAll.Appearance.Font = (Font)componentResourceManager.GetObject("btAcceptAll.Appearance.Font");
			btAcceptAll.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btAcceptAll.Appearance.FontSizeDelta");
			btAcceptAll.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btAcceptAll.Appearance.FontStyleDelta");
			btAcceptAll.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btAcceptAll.Appearance.GradientMode");
			btAcceptAll.Appearance.Image = (Image)componentResourceManager.GetObject("btAcceptAll.Appearance.Image");
			btAcceptAll.Appearance.Options.UseFont = true;
			btAcceptAll.Name = "btAcceptAll";
			btAcceptAll.Click += btAcceptAll_Click_1;
			componentResourceManager.ApplyResources(chbAcceptAll, "chbAcceptAll");
			chbAcceptAll.Name = "chbAcceptAll";
			chbAcceptAll.Properties.AccessibleDescription = componentResourceManager.GetString("chbAcceptAll.Properties.AccessibleDescription");
			chbAcceptAll.Properties.AccessibleName = componentResourceManager.GetString("chbAcceptAll.Properties.AccessibleName");
			chbAcceptAll.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("chbAcceptAll.Properties.Appearance.Font");
			chbAcceptAll.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("chbAcceptAll.Properties.Appearance.FontSizeDelta");
			chbAcceptAll.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("chbAcceptAll.Properties.Appearance.FontStyleDelta");
			chbAcceptAll.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("chbAcceptAll.Properties.Appearance.GradientMode");
			chbAcceptAll.Properties.Appearance.Image = (Image)componentResourceManager.GetObject("chbAcceptAll.Properties.Appearance.Image");
			chbAcceptAll.Properties.Appearance.Options.UseFont = true;
			chbAcceptAll.Properties.AutoHeight = (bool)componentResourceManager.GetObject("chbAcceptAll.Properties.AutoHeight");
			chbAcceptAll.Properties.Caption = componentResourceManager.GetString("chbAcceptAll.Properties.Caption");
			chbAcceptAll.Properties.DisplayValueChecked = componentResourceManager.GetString("chbAcceptAll.Properties.DisplayValueChecked");
			chbAcceptAll.Properties.DisplayValueGrayed = componentResourceManager.GetString("chbAcceptAll.Properties.DisplayValueGrayed");
			chbAcceptAll.Properties.DisplayValueUnchecked = componentResourceManager.GetString("chbAcceptAll.Properties.DisplayValueUnchecked");
			componentResourceManager.ApplyResources(chbAcceptSubObjects, "chbAcceptSubObjects");
			chbAcceptSubObjects.Name = "chbAcceptSubObjects";
			chbAcceptSubObjects.Properties.AccessibleDescription = componentResourceManager.GetString("chbAcceptSubObjects.Properties.AccessibleDescription");
			chbAcceptSubObjects.Properties.AccessibleName = componentResourceManager.GetString("chbAcceptSubObjects.Properties.AccessibleName");
			chbAcceptSubObjects.Properties.Appearance.Font = (Font)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.Appearance.Font");
			chbAcceptSubObjects.Properties.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.Appearance.FontSizeDelta");
			chbAcceptSubObjects.Properties.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.Appearance.FontStyleDelta");
			chbAcceptSubObjects.Properties.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.Appearance.GradientMode");
			chbAcceptSubObjects.Properties.Appearance.Image = (Image)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.Appearance.Image");
			chbAcceptSubObjects.Properties.Appearance.Options.UseFont = true;
			chbAcceptSubObjects.Properties.AutoHeight = (bool)componentResourceManager.GetObject("chbAcceptSubObjects.Properties.AutoHeight");
			chbAcceptSubObjects.Properties.Caption = componentResourceManager.GetString("chbAcceptSubObjects.Properties.Caption");
			chbAcceptSubObjects.Properties.DisplayValueChecked = componentResourceManager.GetString("chbAcceptSubObjects.Properties.DisplayValueChecked");
			chbAcceptSubObjects.Properties.DisplayValueGrayed = componentResourceManager.GetString("chbAcceptSubObjects.Properties.DisplayValueGrayed");
			chbAcceptSubObjects.Properties.DisplayValueUnchecked = componentResourceManager.GetString("chbAcceptSubObjects.Properties.DisplayValueUnchecked");
			componentResourceManager.ApplyResources(btAccept, "btAccept");
			btAccept.Appearance.Font = (Font)componentResourceManager.GetObject("btAccept.Appearance.Font");
			btAccept.Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
			btAccept.Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
			btAccept.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
			btAccept.Appearance.Image = (Image)componentResourceManager.GetObject("btAccept.Appearance.Image");
			btAccept.Appearance.Options.UseFont = true;
			btAccept.Name = "btAccept";
			btAccept.Click += btAccept_Click;
			componentResourceManager.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Controls.Add(gcInCategory);
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(gcInCategory, "gcInCategory");
			gcInCategory.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcInCategory.EmbeddedNavigator.AccessibleDescription");
			gcInCategory.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcInCategory.EmbeddedNavigator.AccessibleName");
			gcInCategory.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.AllowHtmlTextInToolTip");
			gcInCategory.EmbeddedNavigator.Anchor = (AnchorStyles)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.Anchor");
			gcInCategory.EmbeddedNavigator.BackgroundImage = (Image)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.BackgroundImage");
			gcInCategory.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.BackgroundImageLayout");
			gcInCategory.EmbeddedNavigator.ImeMode = (ImeMode)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.ImeMode");
			gcInCategory.EmbeddedNavigator.MaximumSize = (Size)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.MaximumSize");
			gcInCategory.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.TextLocation");
			gcInCategory.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcInCategory.EmbeddedNavigator.ToolTip");
			gcInCategory.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType)componentResourceManager.GetObject("gcInCategory.EmbeddedNavigator.ToolTipIconType");
			gcInCategory.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcInCategory.EmbeddedNavigator.ToolTipTitle");
			gcInCategory.LookAndFeel.SkinName = "Office 2007 Blue";
			gcInCategory.MainView = gvInCategory;
			gcInCategory.Name = "gcInCategory";
			gcInCategory.RepositoryItems.AddRange(new RepositoryItem[1]
			{
		repositoryItemCheckEdit1
			});
			gcInCategory.ViewCollection.AddRange(new BaseView[1]
			{
		gvInCategory
			});
			componentResourceManager.ApplyResources(gvInCategory, "gvInCategory");
			gvInCategory.ColumnPanelRowHeight = 40;
			gvInCategory.Columns.AddRange(new GridColumn[8]
			{
		colCategoryID,
		colCategory,
		colnList,
		colWarning,
		colSound,
		colObjectID,
		colTableID,
		colDevice
			});
			gvInCategory.GridControl = gcInCategory;
			gvInCategory.Name = "gvInCategory";
			gvInCategory.OptionsCustomization.AllowFilter = false;
			gvInCategory.OptionsFind.ClearFindOnClose = false;
			gvInCategory.OptionsFind.FindDelay = 10000;
			gvInCategory.OptionsFind.ShowCloseButton = false;
			gvInCategory.OptionsSelection.MultiSelect = true;
			gvInCategory.OptionsView.ColumnAutoWidth = false;
			gvInCategory.OptionsView.ShowGroupPanel = false;
			gvInCategory.SelectionChanged += gvInCategory_SelectionChanged;
			gvInCategory.CellValueChanging += gvInCategory_CellValueChanging;
			colCategoryID.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colCategoryID.AppearanceCell.Font");
			colCategoryID.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colCategoryID.AppearanceCell.FontSizeDelta");
			colCategoryID.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colCategoryID.AppearanceCell.FontStyleDelta");
			colCategoryID.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colCategoryID.AppearanceCell.GradientMode");
			colCategoryID.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colCategoryID.AppearanceCell.Image");
			colCategoryID.AppearanceCell.Options.UseFont = true;
			colCategoryID.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colCategoryID.AppearanceHeader.Font");
			colCategoryID.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colCategoryID.AppearanceHeader.FontSizeDelta");
			colCategoryID.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colCategoryID.AppearanceHeader.FontStyleDelta");
			colCategoryID.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colCategoryID.AppearanceHeader.GradientMode");
			colCategoryID.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colCategoryID.AppearanceHeader.Image");
			colCategoryID.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colCategoryID, "colCategoryID");
			colCategoryID.FieldName = "ID";
			colCategoryID.Name = "colCategoryID";
			colCategoryID.OptionsColumn.AllowEdit = false;
			colCategoryID.OptionsColumn.ReadOnly = true;
			colCategory.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colCategory.AppearanceCell.Font");
			colCategory.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colCategory.AppearanceCell.FontSizeDelta");
			colCategory.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colCategory.AppearanceCell.FontStyleDelta");
			colCategory.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colCategory.AppearanceCell.GradientMode");
			colCategory.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colCategory.AppearanceCell.Image");
			colCategory.AppearanceCell.Options.UseFont = true;
			colCategory.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colCategory.AppearanceHeader.Font");
			colCategory.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colCategory.AppearanceHeader.FontSizeDelta");
			colCategory.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colCategory.AppearanceHeader.FontStyleDelta");
			colCategory.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colCategory.AppearanceHeader.GradientMode");
			colCategory.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colCategory.AppearanceHeader.Image");
			colCategory.AppearanceHeader.Options.UseFont = true;
			colCategory.AppearanceHeader.Options.UseTextOptions = true;
			colCategory.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(colCategory, "colCategory");
			colCategory.FieldName = "Name";
			colCategory.Name = "colCategory";
			colCategory.OptionsColumn.AllowEdit = false;
			colCategory.OptionsColumn.AllowSort = DefaultBoolean.False;
			colCategory.OptionsColumn.ReadOnly = true;
			colnList.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colnList.AppearanceCell.Font");
			colnList.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colnList.AppearanceCell.FontSizeDelta");
			colnList.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colnList.AppearanceCell.FontStyleDelta");
			colnList.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colnList.AppearanceCell.GradientMode");
			colnList.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colnList.AppearanceCell.Image");
			colnList.AppearanceCell.Options.UseFont = true;
			colnList.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colnList.AppearanceHeader.Font");
			colnList.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colnList.AppearanceHeader.FontSizeDelta");
			colnList.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colnList.AppearanceHeader.FontStyleDelta");
			colnList.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colnList.AppearanceHeader.GradientMode");
			colnList.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colnList.AppearanceHeader.Image");
			colnList.AppearanceHeader.Options.UseFont = true;
			colnList.AppearanceHeader.Options.UseTextOptions = true;
			colnList.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(colnList, "colnList");
			colnList.ColumnEdit = repositoryItemCheckEdit1;
			colnList.FieldName = "InList";
			colnList.Name = "colnList";
			colnList.OptionsColumn.AllowSort = DefaultBoolean.False;
			componentResourceManager.ApplyResources(repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
			repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			repositoryItemCheckEdit1.CheckedChanged += repositoryItemCheckEdit1_CheckedChanged;
			colWarning.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colWarning.AppearanceCell.Font");
			colWarning.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colWarning.AppearanceCell.FontSizeDelta");
			colWarning.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colWarning.AppearanceCell.FontStyleDelta");
			colWarning.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colWarning.AppearanceCell.GradientMode");
			colWarning.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colWarning.AppearanceCell.Image");
			colWarning.AppearanceCell.Options.UseFont = true;
			colWarning.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colWarning.AppearanceHeader.Font");
			colWarning.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colWarning.AppearanceHeader.FontSizeDelta");
			colWarning.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colWarning.AppearanceHeader.FontStyleDelta");
			colWarning.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colWarning.AppearanceHeader.GradientMode");
			colWarning.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colWarning.AppearanceHeader.Image");
			colWarning.AppearanceHeader.Options.UseFont = true;
			colWarning.AppearanceHeader.Options.UseTextOptions = true;
			colWarning.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(colWarning, "colWarning");
			colWarning.ColumnEdit = repositoryItemCheckEdit1;
			colWarning.FieldName = "Warning";
			colWarning.Name = "colWarning";
			colWarning.OptionsColumn.AllowSort = DefaultBoolean.False;
			colSound.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colSound.AppearanceCell.Font");
			colSound.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colSound.AppearanceCell.FontSizeDelta");
			colSound.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colSound.AppearanceCell.FontStyleDelta");
			colSound.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colSound.AppearanceCell.GradientMode");
			colSound.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colSound.AppearanceCell.Image");
			colSound.AppearanceCell.Options.UseFont = true;
			colSound.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colSound.AppearanceHeader.Font");
			colSound.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colSound.AppearanceHeader.FontSizeDelta");
			colSound.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colSound.AppearanceHeader.FontStyleDelta");
			colSound.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colSound.AppearanceHeader.GradientMode");
			colSound.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colSound.AppearanceHeader.Image");
			colSound.AppearanceHeader.Options.UseFont = true;
			colSound.AppearanceHeader.Options.UseTextOptions = true;
			colSound.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(colSound, "colSound");
			colSound.ColumnEdit = repositoryItemCheckEdit1;
			colSound.FieldName = "Sound";
			colSound.Name = "colSound";
			colSound.OptionsColumn.AllowSort = DefaultBoolean.False;
			colObjectID.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colObjectID.AppearanceCell.Font");
			colObjectID.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colObjectID.AppearanceCell.FontSizeDelta");
			colObjectID.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colObjectID.AppearanceCell.FontStyleDelta");
			colObjectID.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colObjectID.AppearanceCell.GradientMode");
			colObjectID.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colObjectID.AppearanceCell.Image");
			colObjectID.AppearanceCell.Options.UseFont = true;
			colObjectID.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colObjectID.AppearanceHeader.Font");
			colObjectID.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colObjectID.AppearanceHeader.FontSizeDelta");
			colObjectID.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colObjectID.AppearanceHeader.FontStyleDelta");
			colObjectID.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colObjectID.AppearanceHeader.GradientMode");
			colObjectID.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colObjectID.AppearanceHeader.Image");
			colObjectID.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colObjectID, "colObjectID");
			colObjectID.FieldName = "ObjectID";
			colObjectID.Name = "colObjectID";
			colTableID.AppearanceCell.Font = (Font)componentResourceManager.GetObject("colTableID.AppearanceCell.Font");
			colTableID.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("colTableID.AppearanceCell.FontSizeDelta");
			colTableID.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colTableID.AppearanceCell.FontStyleDelta");
			colTableID.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colTableID.AppearanceCell.GradientMode");
			colTableID.AppearanceCell.Image = (Image)componentResourceManager.GetObject("colTableID.AppearanceCell.Image");
			colTableID.AppearanceCell.Options.UseFont = true;
			colTableID.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colTableID.AppearanceHeader.Font");
			colTableID.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colTableID.AppearanceHeader.FontSizeDelta");
			colTableID.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colTableID.AppearanceHeader.FontStyleDelta");
			colTableID.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colTableID.AppearanceHeader.GradientMode");
			colTableID.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colTableID.AppearanceHeader.Image");
			colTableID.AppearanceHeader.Options.UseFont = true;
			componentResourceManager.ApplyResources(colTableID, "colTableID");
			colTableID.FieldName = "TableID";
			colTableID.Name = "colTableID";
			colDevice.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("colDevice.AppearanceHeader.Font");
			colDevice.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("colDevice.AppearanceHeader.FontSizeDelta");
			colDevice.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("colDevice.AppearanceHeader.FontStyleDelta");
			colDevice.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("colDevice.AppearanceHeader.GradientMode");
			colDevice.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("colDevice.AppearanceHeader.Image");
			colDevice.AppearanceHeader.Options.UseFont = true;
			colDevice.AppearanceHeader.Options.UseTextOptions = true;
			colDevice.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			colDevice.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
			componentResourceManager.ApplyResources(colDevice, "colDevice");
			colDevice.ColumnEdit = repositoryItemCheckEdit1;
			colDevice.FieldName = "Device";
			colDevice.Name = "colDevice";
			componentResourceManager.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(tvObjects);
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(tvObjects, "tvObjects");
			tvObjects.Columns.AddRange(new TreeListColumn[1]
			{
		treeListColumn1
			});
			tvObjects.Name = "tvObjects";
			tvObjects.OptionsBehavior.Editable = false;
			tvObjects.OptionsSelection.InvertSelection = true;
			tvObjects.OptionsSelection.MultiSelect = true;
			tvObjects.OptionsSelection.UseIndicatorForSelection = true;
			tvObjects.RepositoryItems.AddRange(new RepositoryItem[1]
			{
		repositoryItemCheckEdit4
			});
			tvObjects.FocusedNodeChanged += tvObjects_FocusedNodeChanged;
			componentResourceManager.ApplyResources(treeListColumn1, "treeListColumn1");
			treeListColumn1.AppearanceCell.Font = (Font)componentResourceManager.GetObject("treeListColumn1.AppearanceCell.Font");
			treeListColumn1.AppearanceCell.FontSizeDelta = (int)componentResourceManager.GetObject("treeListColumn1.AppearanceCell.FontSizeDelta");
			treeListColumn1.AppearanceCell.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("treeListColumn1.AppearanceCell.FontStyleDelta");
			treeListColumn1.AppearanceCell.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("treeListColumn1.AppearanceCell.GradientMode");
			treeListColumn1.AppearanceCell.Image = (Image)componentResourceManager.GetObject("treeListColumn1.AppearanceCell.Image");
			treeListColumn1.AppearanceCell.Options.UseFont = true;
			treeListColumn1.AppearanceHeader.Font = (Font)componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Font");
			treeListColumn1.AppearanceHeader.FontSizeDelta = (int)componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontSizeDelta");
			treeListColumn1.AppearanceHeader.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontStyleDelta");
			treeListColumn1.AppearanceHeader.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.GradientMode");
			treeListColumn1.AppearanceHeader.Image = (Image)componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Image");
			treeListColumn1.AppearanceHeader.Options.UseFont = true;
			treeListColumn1.Name = "treeListColumn1";
			treeListColumn1.OptionsColumn.AllowSort = false;
			componentResourceManager.ApplyResources(repositoryItemCheckEdit4, "repositoryItemCheckEdit4");
			repositoryItemCheckEdit4.Name = "repositoryItemCheckEdit4";
			componentResourceManager.ApplyResources(this, "$this");
			Appearance.FontSizeDelta = (int)componentResourceManager.GetObject("frmCommonCategories.Appearance.FontSizeDelta");
			Appearance.FontStyleDelta = (FontStyle)componentResourceManager.GetObject("frmCommonCategories.Appearance.FontStyleDelta");
			Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("frmCommonCategories.Appearance.GradientMode");
			Appearance.Image = (Image)componentResourceManager.GetObject("frmCommonCategories.Appearance.Image");
			Appearance.Options.UseFont = true;
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(groupControl2);
			Controls.Add(groupControl1);
			FormBorderStyle = FormBorderStyle.None;
			Name = "FrmCommonCategories";
			ShowIcon = false;
			WindowState = FormWindowState.Maximized;
			Load += frmCommonCategories_Load;
			Resize += frmCommonCategories_Resize;
			groupControl1.EndInit();
			groupControl1.ResumeLayout(false);
			gcCategory.EndInit();
			gvCategory.EndInit();
			repositoryItemCheckEdit3.EndInit();
			groupControl2.EndInit();
			groupControl2.ResumeLayout(false);
			chbAcceptAll.Properties.EndInit();
			chbAcceptSubObjects.Properties.EndInit();
			groupBox2.ResumeLayout(false);
			gcInCategory.EndInit();
			gvInCategory.EndInit();
			repositoryItemCheckEdit1.EndInit();
			groupBox1.ResumeLayout(false);
			tvObjects.EndInit();
			repositoryItemCheckEdit4.EndInit();
			ResumeLayout(false);
		}
	}
}
