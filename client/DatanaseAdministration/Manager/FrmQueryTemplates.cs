// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmQueryTemplates
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmQueryTemplates : XtraForm
  {
    private DataTable _dtmain = new DataTable();
    private List<BcQueryTemplate> _templates = new List<BcQueryTemplate>();
    private IContainer components = null;
    private GridControl gcMain;
    private GridView gridView1;
    private SplitContainerControl splitContainerControl1;
    private LabelControl labelControl3;
    private MemoEdit tbQuery;
    private LabelControl labelControl2;
    private MemoEdit tbComment;
    private ComboBoxEdit cbTemplates;
    private LabelControl labelControl1;
    private TemplateFieldsControl templateFields;
    private SimpleButton btOk;

    public FrmQueryTemplates()
    {
      InitializeComponent();
      WindowState = FormWindowState.Maximized;
    }

    private void frmQueryTemplates_Load(object sender, EventArgs e)
    {
      _templates = BcQueryTemplate.LoadAll();
      foreach (BcQueryTemplate bcQueryTemplate in _templates)
        cbTemplates.Properties.Items.Add(bcQueryTemplate.Name);
    }

    private void cbTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cbTemplates.SelectedIndex >= 0)
      {
        templateFields.Templates = _templates[cbTemplates.SelectedIndex].Templates;
        tbQuery.Text = _templates[cbTemplates.SelectedIndex].Query;
        tbComment.Text = _templates[cbTemplates.SelectedIndex].Comment;
        templateFields.Reinitialize();
      }
      else
      {
        tbComment.Text = "";
        tbQuery.Text = "";
        templateFields.xtraScrollableControl1.Controls.Clear();
      }
    }

    private void btOk_Click(object sender, EventArgs e)
    {
      SqlCommand sqlCommand = new SqlCommand(tbQuery.Text.Replace("$", "@"), new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Parameters.AddRange(templateFields.GetParameters().ToArray());
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      foreach (DataColumn dataColumn in _dtmain.Columns)
      {
        if (dataColumn.DataType == typeof (Bitmap))
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) _dtmain.Rows)
          {
            try
            {
              if (dataRow[dataColumn.ColumnName] != DBNull.Value)
              {
                Bitmap bitmap = (Bitmap) dataRow[dataColumn.ColumnName];
                if (bitmap != null)
                  bitmap.Dispose();
              }
            }
            catch
            {
            }
          }
        }
      }
      _dtmain.Rows.Clear();
      _dtmain.Dispose();
      _dtmain = new DataTable();
      gridView1.Columns.Clear();
      gridView1.OptionsView.RowAutoHeight = true;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
      gridView1.OptionsCustomization.AllowRowSizing = true;
      gridView1.OptionsView.ColumnAutoWidth = true;
      for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
      {
        GridColumn column = new GridColumn();
        column.FieldName = sqlDataReader.GetName(ordinal);
        column.Visible = true;
        column.VisibleIndex = ordinal;
        column.Caption = sqlDataReader.GetName(ordinal);
        if (sqlDataReader.GetFieldType(ordinal) == typeof (byte[]))
        {
          _dtmain.Columns.Add(sqlDataReader.GetName(ordinal), typeof (Bitmap));
          column.ColumnEdit = new RepositoryItemPictureEdit();
        }
        else
          _dtmain.Columns.Add(sqlDataReader.GetName(ordinal), sqlDataReader.GetFieldType(ordinal));
        _dtmain.Columns[_dtmain.Columns.Count - 1].Caption = sqlDataReader.GetName(ordinal);
        gridView1.Columns.Add(column);
      }
      try
      {
        while (sqlDataReader.Read())
        {
          object[] objArray = new object[sqlDataReader.FieldCount];
          for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
          {
            if (sqlDataReader.GetFieldType(ordinal) == typeof (byte[]))
            {
              try
              {
                if (sqlDataReader[ordinal].ToString() != "" && ((byte[]) sqlDataReader[ordinal]).Length != 10988)
                  objArray[ordinal] = new Bitmap(new MemoryStream((byte[]) sqlDataReader[ordinal]));
              }
              catch
              {
              }
            }
            else
              objArray[ordinal] = sqlDataReader[ordinal];
          }
          _dtmain.Rows.Add(objArray);
        }
        gcMain.DataSource = _dtmain;
        gridView1.OptionsBehavior.Editable = false;
        sqlDataReader.Close();
        sqlCommand.Connection.Close();
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void frmQueryTemplates_Resize(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmQueryTemplates));
      splitContainerControl1 = new SplitContainerControl();
      gcMain = new GridControl();
      gridView1 = new GridView();
      btOk = new SimpleButton();
      labelControl3 = new LabelControl();
      tbQuery = new MemoEdit();
      labelControl2 = new LabelControl();
      tbComment = new MemoEdit();
      cbTemplates = new ComboBoxEdit();
      labelControl1 = new LabelControl();
      splitContainerControl1.BeginInit();
      splitContainerControl1.SuspendLayout();
      gcMain.BeginInit();
      gridView1.BeginInit();
      tbQuery.Properties.BeginInit();
      tbComment.Properties.BeginInit();
      cbTemplates.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(splitContainerControl1, "splitContainerControl1");
      splitContainerControl1.Name = "splitContainerControl1";
      componentResourceManager.ApplyResources(splitContainerControl1.Panel1, "splitContainerControl1.Panel1");
      componentResourceManager.ApplyResources(splitContainerControl1.Panel2, "splitContainerControl1.Panel2");
      splitContainerControl1.Panel2.Controls.Add(gcMain);
      splitContainerControl1.SplitterPosition = 363;
      componentResourceManager.ApplyResources(gcMain, "gcMain");
      gcMain.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gcMain.EmbeddedNavigator.AccessibleDescription");
      gcMain.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gcMain.EmbeddedNavigator.AccessibleName");
      gcMain.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.AllowHtmlTextInToolTip");
      gcMain.EmbeddedNavigator.Anchor = (AnchorStyles) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.Anchor");
      gcMain.EmbeddedNavigator.BackgroundImage = (Image) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.BackgroundImage");
      gcMain.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.BackgroundImageLayout");
      gcMain.EmbeddedNavigator.ImeMode = (ImeMode) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.ImeMode");
      gcMain.EmbeddedNavigator.MaximumSize = (Size) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.MaximumSize");
      gcMain.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.TextLocation");
      gcMain.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gcMain.EmbeddedNavigator.ToolTip");
      gcMain.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType) componentResourceManager.GetObject("gcMain.EmbeddedNavigator.ToolTipIconType");
      gcMain.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gcMain.EmbeddedNavigator.ToolTipTitle");
      gcMain.MainView = gridView1;
      gcMain.Name = "gcMain";
      gcMain.ViewCollection.AddRange(new BaseView[1]
      {
        gridView1
      });
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.GridControl = gcMain;
      gridView1.Name = "gridView1";
      gridView1.OptionsView.ShowGroupPanel = false;
      componentResourceManager.ApplyResources(btOk, "btOk");
      btOk.Name = "btOk";
      btOk.Click += btOk_Click;
      componentResourceManager.ApplyResources(labelControl3, "labelControl3");
      labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources(tbQuery, "tbQuery");
      tbQuery.Name = "tbQuery";
      tbQuery.Properties.AccessibleDescription = componentResourceManager.GetString("tbQuery.Properties.AccessibleDescription");
      tbQuery.Properties.AccessibleName = componentResourceManager.GetString("tbQuery.Properties.AccessibleName");
      tbQuery.Properties.NullValuePrompt = componentResourceManager.GetString("tbQuery.Properties.NullValuePrompt");
      tbQuery.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbQuery.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(labelControl2, "labelControl2");
      labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources(tbComment, "tbComment");
      tbComment.Name = "tbComment";
      tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(cbTemplates, "cbTemplates");
      cbTemplates.Name = "cbTemplates";
      cbTemplates.Properties.AccessibleDescription = componentResourceManager.GetString("cbTemplates.Properties.AccessibleDescription");
      cbTemplates.Properties.AccessibleName = componentResourceManager.GetString("cbTemplates.Properties.AccessibleName");
      cbTemplates.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbTemplates.Properties.AutoHeight");
      cbTemplates.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbTemplates.Properties.Buttons"))
      });
      cbTemplates.Properties.NullValuePrompt = componentResourceManager.GetString("cbTemplates.Properties.NullValuePrompt");
      cbTemplates.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbTemplates.Properties.NullValuePromptShowForEmptyValue");
      cbTemplates.SelectedIndexChanged += cbTemplates_SelectedIndexChanged;
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      ControlBox = false;
      Controls.Add(splitContainerControl1);
      FormBorderStyle = FormBorderStyle.None;
      Name = "FrmQueryTemplates";
      ShowIcon = false;
      Load += frmQueryTemplates_Load;
      Resize += frmQueryTemplates_Resize;
      splitContainerControl1.EndInit();
      splitContainerControl1.ResumeLayout(false);
      gcMain.EndInit();
      gridView1.EndInit();
      tbQuery.Properties.EndInit();
      tbComment.Properties.EndInit();
      cbTemplates.Properties.EndInit();
      ResumeLayout(false);
    }
  }
}
