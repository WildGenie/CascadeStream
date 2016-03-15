// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmObjects
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
using CascadeManager.Properties;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using FindMode = DevExpress.XtraEditors.FindMode;

namespace CascadeManager
{
  public class FrmObjects : XtraForm
  {
    public static int SelectedId = -1;
    private List<BcObjects> _objects = new List<BcObjects>();
    private DataTable _dtmain = new DataTable();
    private BcObjects _objectData = new BcObjects();
    private IContainer components = null;
    private bool _editFlag;
    private bool _isEdit;
    private TreeListNode _selectionNode;
    private SimpleButton btDelete;
    private SimpleButton btAdd;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colName;
    private GridColumn colPosition;
    private GridColumn colComment;
    private TreeList tvObjects;
    private TreeListColumn treeListColumn1;
    private RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private SimpleButton btCancel;
    private SimpleButton btCopy;
    private SimpleButton btSave;
    private LabelControl label2;
    private SimpleButton btDeleteItem;
    private TextEdit tbName2;
    private LabelControl label1;
    private SimpleButton btAddItem;
    private GroupControl groupControl1;
    private MemoEdit tbComment2;

    public FrmObjects()
    {
      InitializeComponent();
    }

    private void RefreshForm()
    {
      _dtmain.Dispose();
      _dtmain = new DataTable();
      _dtmain.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("ID"),
        new DataColumn("Name"),
        new DataColumn("Comment"),
        new DataColumn("Data")
      });
      _objects = BcObjects.LoadAll();
      foreach (BcObjects bcObjects in _objects)
        _dtmain.Rows.Add((object) bcObjects.Id, (object) bcObjects.Name, (object) bcObjects.Comment, (object) bcObjects.Data);
      gridControl1.DataSource = _dtmain;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
      ControlBox = false;
      if (_isEdit && XtraMessageBox.Show(Messages.DoYouWantToSaveChanges, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btSave_Click(sender, e);
      _isEdit = false;
      btDelete.Enabled = false;
      btAdd.Enabled = false;
      gridControl1.Enabled = false;
      _editFlag = false;
      LoadData();
      _editFlag = true;
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (_isEdit && XtraMessageBox.Show(Messages.DoYouWantToSaveChanges, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btSave_Click(sender, e);
      _isEdit = false;
      if (gridView1.GetSelectedRows().Length <= 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      new BcObjects
      {
        Id = SelectedId
      }.Delete();
      _objects.RemoveAt(gridView1.GetSelectedRows()[0]);
      _dtmain.Rows.RemoveAt(gridView1.GetSelectedRows()[0]);
      tbComment2.Text = "";
      tbName2.Text = "";
      tvObjects.Nodes.Clear();
    }

    private void btMaps_Click(object sender, EventArgs e)
    {
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (_isEdit && XtraMessageBox.Show(Messages.DoYouWantToSaveChanges, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btSave_Click(sender, e);
      _isEdit = false;
      if (gridView1.GetSelectedRows().Length > 0)
      {
        SelectedId = Convert.ToInt32(gridView1.GetDataRow(gridView1.GetSelectedRows()[0])[0]);
        _editFlag = true;
        LoadData();
        _editFlag = false;
      }
      else
        SelectedId = -1;
    }

    private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    private void btCopy_Click(object sender, EventArgs e)
    {
      if (tvObjects.Selection[0] == null || tvObjects.Selection[0] == tvObjects.Nodes[0])
        return;
      _isEdit = true;
      BcObjectsData objectByObjectId = BcObjectsData.GetParentObjectByObjectId(_objectData.Data, null, (Guid) tvObjects.Selection[0].Tag);
      tvObjects.BeginUnboundLoad();
      if (objectByObjectId.IsParent)
      {
        BcObjectsData objectById = BcObjectsData.GetObjectById(_objectData.Data, (Guid) tvObjects.Selection[0].Tag);
        BcObjectsData bcObjectsData = new BcObjectsData();
        BcObjectsData.Clone(objectById, bcObjectsData);
        bcObjectsData.Id = Guid.NewGuid();
        _objectData.Data.Add(bcObjectsData);
        string str = bcObjectsData.Name;
        TreeListNode node = tvObjects.AppendNode(str, tvObjects.Selection[0].ParentNode);
        node.SetValue(0, str);
        node.Tag = bcObjectsData.Id;
        LoadTree(node, bcObjectsData);
      }
      else
      {
        BcObjectsData objectById = BcObjectsData.GetObjectById(_objectData.Data, (Guid) tvObjects.Selection[0].Tag);
        BcObjectsData bcObjectsData = new BcObjectsData();
        BcObjectsData.Clone(objectById, bcObjectsData);
        bcObjectsData.Id = Guid.NewGuid();
        objectByObjectId.Data.Add(bcObjectsData);
        TreeListNode node = tvObjects.AppendNode(bcObjectsData.Name, tvObjects.Selection[0].ParentNode);
        node.Tag = bcObjectsData.Id;
        node.SetValue(0, bcObjectsData.Name);
        LoadTree(node, bcObjectsData);
      }
      tvObjects.EndUnboundLoad();
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      if (tvObjects.Selection[0] != null)
        tvObjects_SelectionChanged(new object(), new EventArgs());
      _objectData.SaveData();
      _objectData.Save();
      int dataSourceRowIndex = gridView1.GetFocusedDataSourceRowIndex();
      int rowHandle = -1;
      _isEdit = false;
      if (gridView1.GetSelectedRows().Length > 0)
        rowHandle = gridView1.GetSelectedRows()[0];
      RefreshForm();
      if (rowHandle != -1)
        gridView1.SelectRow(rowHandle);
      if (dataSourceRowIndex != -1)
        gridView1.MoveBy(dataSourceRowIndex);
      btDelete.Enabled = true;
      btAdd.Enabled = true;
      gridControl1.Enabled = true;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      if (_isEdit && XtraMessageBox.Show(Messages.DoYouWantToSaveChanges, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        btSave_Click(sender, e);
      _isEdit = false;
      btDelete.Enabled = true;
      btAdd.Enabled = true;
      gridControl1.Enabled = true;
      int dataSourceRowIndex = gridView1.GetFocusedDataSourceRowIndex();
      int rowHandle = -1;
      if (gridView1.GetSelectedRows().Length > 0)
        rowHandle = gridView1.GetSelectedRows()[0];
      RefreshForm();
      if (rowHandle != -1)
        gridView1.SelectRow(rowHandle);
      if (dataSourceRowIndex == -1)
        return;
      gridView1.MoveBy(dataSourceRowIndex);
    }

    private void LoadData()
    {
      tvObjects.BeginUnboundLoad();
      if (_editFlag)
      {
        tvObjects.ClearNodes();
        _objectData = BcObjects.LoadById(SelectedId);
        _objectData.GetData();
        LoadTree(null, null);
      }
      else
      {
        tvObjects.ClearNodes();
        tbComment2.Text = "";
        tbName2.Text = "";
        _objectData = new BcObjects();
        _objectData.Name = "";
        tvObjects.AppendNode("[" + Messages.ControlledObject + "]", -1);
        tvObjects.Nodes[0].Tag = -1;
      }
      tvObjects.EndUnboundLoad();
      tvObjects.Selection.Set(tvObjects.Nodes[0]);
      tvObjects.Selection[0].ExpandAll();
    }

    private void LoadTree(TreeListNode node, BcObjectsData data)
    {
      if (tvObjects.Nodes.Count == 0)
      {
        tvObjects.BeginUnboundLoad();
        if (_objectData.Name == "")
          _objectData.Name = "[" + Messages.ControlledObject + "]";
        TreeListNode treeListNode = tvObjects.AppendNode(_objectData.Name, node);
        treeListNode.SetValue(0, _objectData.Name);
        treeListNode.Tag = _objectData.Id;
        tvObjects.Nodes.Add(treeListNode);
        foreach (BcObjectsData data1 in _objectData.Data)
        {
          if (data1.Name == "")
            data1.Name = "[" + Messages.ControlledObject + "]";
          TreeListNode node1 = tvObjects.AppendNode(data1.Name, treeListNode);
          node1.Tag = data1.Id;
          node1.SetValue(0, data1.Name);
          LoadTree(node1, data1);
        }
        tvObjects.EndUnboundLoad();
      }
      else
      {
        if (node == null)
          return;
        tvObjects.BeginUnboundLoad();
        foreach (BcObjectsData data1 in data.Data)
        {
          if (data1.Name == "")
            data1.Name = "[" + Messages.ControlledObject + "]";
          TreeListNode node1 = tvObjects.AppendNode(data1.Name, node);
          node1.SetValue(0, data1.Name);
          node1.Tag = data1.Id;
          LoadTree(node1, data1);
        }
        tvObjects.EndUnboundLoad();
      }
    }

    private void tvObjects_SelectionChanged(object sender, EventArgs e)
    {
      if (tvObjects.Nodes.Count > 0)
      {
        if (_selectionNode != null && _selectionNode.Tag.ToString() != "-1")
        {
          if (_selectionNode != null && _selectionNode != tvObjects.Nodes[0])
          {
            BcObjectsData objectById = BcObjectsData.GetObjectById(_objectData.Data, (Guid) _selectionNode.Tag);
            _selectionNode.SetValue(0, tbName2.Text);
            if (tbName2.Text == "")
              _selectionNode.SetValue(0, "[" + Messages.ControlledObject + "]");
            objectById.Name = tbName2.Text;
            objectById.Comment = tbComment2.Text;
          }
          else if (_selectionNode == tvObjects.Nodes[0])
          {
            _selectionNode.SetValue(0, tbName2.Text);
            if (tbName2.Text == "")
              _selectionNode.SetValue(0, "[" + Messages.ControlledObject + "]");
            _objectData.Name = tbName2.Text;
            _objectData.Comment = tbComment2.Text;
          }
        }
        else if (_selectionNode != null && _selectionNode.Tag.ToString() == "-1")
        {
          _selectionNode.SetValue(0, tbName2.Text);
          if (tbName2.Text == "")
            _selectionNode.SetValue(0, "[" + Messages.ControlledObject + "]");
          _objectData.Name = tbName2.Text;
          _objectData.Comment = tbComment2.Text;
        }
        if (tvObjects.Selection[0] == null)
          return;
        if (tvObjects.Selection[0] != tvObjects.Nodes[0])
        {
          BcObjectsData objectById = BcObjectsData.GetObjectById(_objectData.Data, (Guid) tvObjects.Selection[0].Tag);
          tbName2.Text = objectById.Name;
          tbComment2.Text = objectById.Comment;
        }
        else if (tvObjects.Selection[0] == tvObjects.Nodes[0])
        {
          tbName2.Text = _objectData.Name;
          tbComment2.Text = _objectData.Comment;
        }
        _selectionNode = tvObjects.Selection[0];
      }
      else
        _selectionNode = null;
    }

    private void btAddItem_Click(object sender, EventArgs e)
    {
      if (tvObjects.Selection[0] == null)
        return;
      _isEdit = true;
      tvObjects.BeginUnboundLoad();
      BcObjectsData bcObjectsData = new BcObjectsData();
      bcObjectsData.Id = Guid.NewGuid();
      TreeListNode treeListNode = tvObjects.AppendNode("[" + Messages.ControlledObject + "]", tvObjects.Selection[0]);
      treeListNode.SetValue(0, "[" + Messages.ControlledObject + "]");
      treeListNode.Tag = bcObjectsData.Id;
      if (tvObjects.Selection[0] == tvObjects.Nodes[0])
        _objectData.Data.Add(bcObjectsData);
      else
        BcObjectsData.GetObjectById(_objectData.Data, (Guid) tvObjects.Selection[0].Tag).Data.Add(bcObjectsData);
      tvObjects.Selection[0].ExpandAll();
      tvObjects.EndUnboundLoad();
    }

    private void btDeleteItem_Click(object sender, EventArgs e)
    {
      if (tvObjects.Selection[0] == null || tvObjects.Selection[0] == tvObjects.Nodes[0])
        return;
      _isEdit = true;
      BcObjectsData.RemoveObjectById(_objectData.Data, (Guid) tvObjects.Selection[0].Tag);
      tvObjects.Nodes.Remove(tvObjects.Selection[0]);
    }

    private void frmObjects_Load(object sender, EventArgs e)
    {
      RefreshForm();
    }

    private void tbComment2_TextChanged(object sender, EventArgs e)
    {
      if (_editFlag)
        return;
      _isEdit = true;
    }

    private void chbFree_CheckedChanged(object sender, EventArgs e)
    {
      if (_editFlag)
        return;
      _isEdit = true;
    }

    private void frmObjects_Resize(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmObjects));
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colName = new GridColumn();
      colPosition = new GridColumn();
      colComment = new GridColumn();
      btDelete = new SimpleButton();
      btAdd = new SimpleButton();
      tvObjects = new TreeList();
      treeListColumn1 = new TreeListColumn();
      repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
      btCancel = new SimpleButton();
      btCopy = new SimpleButton();
      btSave = new SimpleButton();
      label2 = new LabelControl();
      btDeleteItem = new SimpleButton();
      tbName2 = new TextEdit();
      label1 = new LabelControl();
      btAddItem = new SimpleButton();
      groupControl1 = new GroupControl();
      tbComment2 = new MemoEdit();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      tvObjects.BeginInit();
      repositoryItemCheckEdit1.BeginInit();
      tbName2.Properties.BeginInit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      tbComment2.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(gridControl1, "gridControl1");
      gridControl1.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleDescription");
      gridControl1.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.AccessibleName");
      gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip");
      gridControl1.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.Anchor");
      gridControl1.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImage");
      gridControl1.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.BackgroundImageLayout");
      gridControl1.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ImeMode");
      gridControl1.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.MaximumSize");
      gridControl1.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.TextLocation");
      gridControl1.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTip");
      gridControl1.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gridControl1.EmbeddedNavigator.ToolTipIconType");
      gridControl1.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gridControl1.EmbeddedNavigator.ToolTipTitle");
      gridControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      gridControl1.MainView = gridView1;
      gridControl1.Name = "gridControl1";
      gridControl1.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      gridControl1.MouseDoubleClick += gridControl1_MouseDoubleClick;
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.Columns.AddRange(new GridColumn[4]
      {
        colID,
        colName,
        colPosition,
        colComment
      });
      gridView1.GridControl = gridControl1;
      gridView1.Name = "gridView1";
      gridView1.OptionsBehavior.Editable = false;
      gridView1.OptionsCustomization.AllowFilter = false;
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.FindMode = FindMode.Always;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.SelectionChanged += gridView1_SelectionChanged;
      colID.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colID.AppearanceCell.Font");
      colID.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceCell.FontSizeDelta");
      colID.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceCell.FontStyleDelta");
      colID.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceCell.GradientMode");
      colID.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colID.AppearanceCell.Image");
      colID.AppearanceCell.Options.UseFont = true;
      colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      colID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceHeader.FontSizeDelta");
      colID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceHeader.FontStyleDelta");
      colID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceHeader.GradientMode");
      colID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colID.AppearanceHeader.Image");
      colID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colID, "colID");
      colID.FieldName = "ID";
      colID.Name = "colID";
      colID.OptionsColumn.ReadOnly = true;
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
      colName.Name = "colName";
      colName.OptionsColumn.AllowEdit = false;
      colName.OptionsColumn.ReadOnly = true;
      colPosition.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceCell.Font");
      colPosition.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colPosition.AppearanceCell.FontSizeDelta");
      colPosition.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPosition.AppearanceCell.FontStyleDelta");
      colPosition.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPosition.AppearanceCell.GradientMode");
      colPosition.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colPosition.AppearanceCell.Image");
      colPosition.AppearanceCell.Options.UseFont = true;
      colPosition.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colPosition.AppearanceHeader.Font");
      colPosition.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontSizeDelta");
      colPosition.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colPosition.AppearanceHeader.FontStyleDelta");
      colPosition.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colPosition.AppearanceHeader.GradientMode");
      colPosition.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colPosition.AppearanceHeader.Image");
      colPosition.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colPosition, "colPosition");
      colPosition.FieldName = "Comment";
      colPosition.Name = "colPosition";
      colPosition.OptionsColumn.AllowEdit = false;
      colComment.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceCell.Font");
      colComment.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colComment.AppearanceCell.FontSizeDelta");
      colComment.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colComment.AppearanceCell.FontStyleDelta");
      colComment.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colComment.AppearanceCell.GradientMode");
      colComment.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colComment.AppearanceCell.Image");
      colComment.AppearanceCell.Options.UseFont = true;
      colComment.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colComment.AppearanceHeader.Font");
      colComment.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colComment.AppearanceHeader.FontSizeDelta");
      colComment.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colComment.AppearanceHeader.FontStyleDelta");
      colComment.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colComment.AppearanceHeader.GradientMode");
      colComment.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colComment.AppearanceHeader.Image");
      colComment.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colComment, "colComment");
      colComment.FieldName = "Data";
      colComment.Name = "colComment";
      colComment.OptionsColumn.AllowEdit = false;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btDelete.Appearance.FontSizeDelta");
      btDelete.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btDelete.Appearance.FontStyleDelta");
      btDelete.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btDelete.Appearance.GradientMode");
      btDelete.Appearance.Image = (Image) componentResourceManager.GetObject("btDelete.Appearance.Image");
      btDelete.Appearance.Options.UseFont = true;
      btDelete.Image = Resources.document_delete_4_;
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      componentResourceManager.ApplyResources(btAdd, "btAdd");
      btAdd.Appearance.Font = (Font) componentResourceManager.GetObject("btAdd.Appearance.Font");
      btAdd.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAdd.Appearance.FontSizeDelta");
      btAdd.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAdd.Appearance.FontStyleDelta");
      btAdd.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAdd.Appearance.GradientMode");
      btAdd.Appearance.Image = (Image) componentResourceManager.GetObject("btAdd.Appearance.Image");
      btAdd.Appearance.Options.UseFont = true;
      btAdd.Image = Resources.document_new_4_;
      btAdd.Name = "btAdd";
      btAdd.Click += btAdd_Click;
      componentResourceManager.ApplyResources(tvObjects, "tvObjects");
      tvObjects.Columns.AddRange(new TreeListColumn[1]
      {
        treeListColumn1
      });
      tvObjects.IndicatorWidth = 10;
      tvObjects.Name = "tvObjects";
      tvObjects.OptionsBehavior.Editable = false;
      tvObjects.OptionsSelection.InvertSelection = true;
      tvObjects.OptionsSelection.MultiSelect = true;
      tvObjects.OptionsSelection.UseIndicatorForSelection = true;
      tvObjects.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        repositoryItemCheckEdit1
      });
      tvObjects.SelectionChanged += tvObjects_SelectionChanged;
      componentResourceManager.ApplyResources(treeListColumn1, "treeListColumn1");
      treeListColumn1.AppearanceCell.Font = (Font) componentResourceManager.GetObject("treeListColumn1.AppearanceCell.Font");
      treeListColumn1.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("treeListColumn1.AppearanceCell.FontSizeDelta");
      treeListColumn1.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("treeListColumn1.AppearanceCell.FontStyleDelta");
      treeListColumn1.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("treeListColumn1.AppearanceCell.GradientMode");
      treeListColumn1.AppearanceCell.Image = (Image) componentResourceManager.GetObject("treeListColumn1.AppearanceCell.Image");
      treeListColumn1.AppearanceCell.Options.UseFont = true;
      treeListColumn1.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Font");
      treeListColumn1.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontSizeDelta");
      treeListColumn1.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.FontStyleDelta");
      treeListColumn1.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.GradientMode");
      treeListColumn1.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("treeListColumn1.AppearanceHeader.Image");
      treeListColumn1.AppearanceHeader.Options.UseFont = true;
      treeListColumn1.Name = "treeListColumn1";
      treeListColumn1.OptionsColumn.AllowSort = false;
      componentResourceManager.ApplyResources(repositoryItemCheckEdit1, "repositoryItemCheckEdit1");
      repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      btCancel.Appearance.Options.UseFont = true;
      btCancel.Image = Resources.dialog_cancel;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(btCopy, "btCopy");
      btCopy.Appearance.Font = (Font) componentResourceManager.GetObject("btCopy.Appearance.Font");
      btCopy.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCopy.Appearance.FontSizeDelta");
      btCopy.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCopy.Appearance.FontStyleDelta");
      btCopy.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCopy.Appearance.GradientMode");
      btCopy.Appearance.Image = (Image) componentResourceManager.GetObject("btCopy.Appearance.Image");
      btCopy.Appearance.Options.UseFont = true;
      btCopy.Image = Resources.edit_copy_2_;
      btCopy.Name = "btCopy";
      btCopy.Click += btCopy_Click;
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      btSave.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.Appearance.FontSizeDelta");
      btSave.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.Appearance.FontStyleDelta");
      btSave.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btSave.Appearance.GradientMode");
      btSave.Appearance.Image = (Image) componentResourceManager.GetObject("btSave.Appearance.Image");
      btSave.Appearance.Options.UseFont = true;
      btSave.Image = Resources.document_save_4_;
      btSave.Name = "btSave";
      btSave.Click += btSave_Click;
      componentResourceManager.ApplyResources(label2, "label2");
      label2.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label2.Appearance.DisabledImage");
      label2.Appearance.Font = (Font) componentResourceManager.GetObject("label2.Appearance.Font");
      label2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label2.Appearance.FontSizeDelta");
      label2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label2.Appearance.FontStyleDelta");
      label2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label2.Appearance.GradientMode");
      label2.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label2.Appearance.HoverImage");
      label2.Appearance.Image = (Image) componentResourceManager.GetObject("label2.Appearance.Image");
      label2.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label2.Appearance.PressedImage");
      label2.Name = "label2";
      componentResourceManager.ApplyResources(btDeleteItem, "btDeleteItem");
      btDeleteItem.Appearance.Font = (Font) componentResourceManager.GetObject("btDeleteItem.Appearance.Font");
      btDeleteItem.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btDeleteItem.Appearance.FontSizeDelta");
      btDeleteItem.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btDeleteItem.Appearance.FontStyleDelta");
      btDeleteItem.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btDeleteItem.Appearance.GradientMode");
      btDeleteItem.Appearance.Image = (Image) componentResourceManager.GetObject("btDeleteItem.Appearance.Image");
      btDeleteItem.Appearance.Options.UseFont = true;
      btDeleteItem.Image = Resources.delete_3_;
      btDeleteItem.Name = "btDeleteItem";
      btDeleteItem.Click += btDeleteItem_Click;
      componentResourceManager.ApplyResources(tbName2, "tbName2");
      tbName2.Name = "tbName2";
      tbName2.Properties.AccessibleDescription = componentResourceManager.GetString("tbName2.Properties.AccessibleDescription");
      tbName2.Properties.AccessibleName = componentResourceManager.GetString("tbName2.Properties.AccessibleName");
      tbName2.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbName2.Properties.Appearance.Font");
      tbName2.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbName2.Properties.Appearance.FontSizeDelta");
      tbName2.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbName2.Properties.Appearance.FontStyleDelta");
      tbName2.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbName2.Properties.Appearance.GradientMode");
      tbName2.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbName2.Properties.Appearance.Image");
      tbName2.Properties.Appearance.Options.UseFont = true;
      tbName2.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbName2.Properties.AutoHeight");
      tbName2.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbName2.Properties.Mask.AutoComplete");
      tbName2.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbName2.Properties.Mask.BeepOnError");
      tbName2.Properties.Mask.EditMask = componentResourceManager.GetString("tbName2.Properties.Mask.EditMask");
      tbName2.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbName2.Properties.Mask.IgnoreMaskBlank");
      tbName2.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbName2.Properties.Mask.MaskType");
      tbName2.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbName2.Properties.Mask.PlaceHolder");
      tbName2.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbName2.Properties.Mask.SaveLiteral");
      tbName2.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbName2.Properties.Mask.ShowPlaceHolders");
      tbName2.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbName2.Properties.Mask.UseMaskAsDisplayFormat");
      tbName2.Properties.NullValuePrompt = componentResourceManager.GetString("tbName2.Properties.NullValuePrompt");
      tbName2.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbName2.Properties.NullValuePromptShowForEmptyValue");
      tbName2.TextChanged += tbComment2_TextChanged;
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label1.Appearance.DisabledImage");
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      label1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label1.Appearance.FontSizeDelta");
      label1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label1.Appearance.FontStyleDelta");
      label1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label1.Appearance.GradientMode");
      label1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label1.Appearance.HoverImage");
      label1.Appearance.Image = (Image) componentResourceManager.GetObject("label1.Appearance.Image");
      label1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label1.Appearance.PressedImage");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(btAddItem, "btAddItem");
      btAddItem.Appearance.Font = (Font) componentResourceManager.GetObject("btAddItem.Appearance.Font");
      btAddItem.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAddItem.Appearance.FontSizeDelta");
      btAddItem.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAddItem.Appearance.FontStyleDelta");
      btAddItem.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAddItem.Appearance.GradientMode");
      btAddItem.Appearance.Image = (Image) componentResourceManager.GetObject("btAddItem.Appearance.Image");
      btAddItem.Appearance.Options.UseFont = true;
      btAddItem.Image = Resources.edit_add;
      btAddItem.Name = "btAddItem";
      btAddItem.Click += btAddItem_Click;
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Appearance.Font = (Font) componentResourceManager.GetObject("groupControl1.Appearance.Font");
      groupControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.Appearance.FontSizeDelta");
      groupControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.Appearance.FontStyleDelta");
      groupControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.Appearance.GradientMode");
      groupControl1.Appearance.Image = (Image) componentResourceManager.GetObject("groupControl1.Appearance.Image");
      groupControl1.Appearance.Options.UseFont = true;
      groupControl1.Controls.Add(tbComment2);
      groupControl1.Controls.Add(tvObjects);
      groupControl1.Controls.Add(btCopy);
      groupControl1.Controls.Add(tbName2);
      groupControl1.Controls.Add(label1);
      groupControl1.Controls.Add(btDeleteItem);
      groupControl1.Controls.Add(label2);
      groupControl1.Controls.Add(btAddItem);
      groupControl1.Name = "groupControl1";
      groupControl1.ShowCaption = false;
      componentResourceManager.ApplyResources(tbComment2, "tbComment2");
      tbComment2.Name = "tbComment2";
      tbComment2.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment2.Properties.AccessibleDescription");
      tbComment2.Properties.AccessibleName = componentResourceManager.GetString("tbComment2.Properties.AccessibleName");
      tbComment2.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment2.Properties.NullValuePrompt");
      tbComment2.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment2.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(this, "$this");
      Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("frmObjects.Appearance.FontSizeDelta");
      Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("frmObjects.Appearance.FontStyleDelta");
      Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("frmObjects.Appearance.GradientMode");
      Appearance.Image = (Image) componentResourceManager.GetObject("frmObjects.Appearance.Image");
      Appearance.Options.UseFont = true;
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(groupControl1);
      Controls.Add(btCancel);
      Controls.Add(btDelete);
      Controls.Add(btSave);
      Controls.Add(btAdd);
      Controls.Add(gridControl1);
      FormBorderStyle = FormBorderStyle.None;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmObjects";
      ShowIcon = false;
      WindowState = FormWindowState.Maximized;
      Load += frmObjects_Load;
      Resize += frmObjects_Resize;
      gridControl1.EndInit();
      gridView1.EndInit();
      tvObjects.EndInit();
      repositoryItemCheckEdit1.EndInit();
      tbName2.Properties.EndInit();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      groupControl1.PerformLayout();
      tbComment2.Properties.EndInit();
      ResumeLayout(false);
    }
  }
}
