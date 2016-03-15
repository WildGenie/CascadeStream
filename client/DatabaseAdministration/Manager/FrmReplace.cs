// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmReplace
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmReplace : XtraForm
  {
    private DataTable _dtMain = new DataTable();
    private string _selectStr = "Select \r\nFaces.ID,\r\nSurname+' '+\r\nFirstName+' '+\r\nLastName as FIO from [dbo].[Faces] Order by Surname";
    public BcFace MainFace = new BcFace();
    private IContainer components = null;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colFIO;
    private PictureEdit pictureEdit1;
    private LabelControl labelControl1;
    private SimpleButton simpleButton1;
    private SimpleButton simpleButton2;

    public FrmReplace()
    {
      InitializeComponent();
      SqlCommand sqlCommand = new SqlCommand(_selectStr, new SqlConnection(CommonSettings.ConnectionString));
      sqlCommand.Connection.Open();
      SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
      _dtMain.Columns.Add("ID", typeof (Guid));
      _dtMain.Columns.Add("FIO", typeof (string));
      while (sqlDataReader.Read())
        _dtMain.Rows.Add(sqlDataReader[0], sqlDataReader[1]);
      sqlCommand.Connection.Close();
      gridControl1.DataSource = _dtMain;
    }

    private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
      try
      {
        pictureEdit1.Image.Dispose();
        pictureEdit1.Image = null;
      }
      catch
      {
      }
      if (e.FocusedRowHandle < 0)
        return;
      byte[] buffer = BcFace.LoadIcon((Guid) gridView1.GetDataRow(e.FocusedRowHandle)["ID"]);
      try
      {
        pictureEdit1.Image = buffer.Length <= 0 ? null : Image.FromStream(new MemoryStream(buffer));
      }
      catch
      {
      }
    }

    private void simpleButton1_Click(object sender, EventArgs e)
    {
      MainFace = BcFace.LoadById((Guid) gridView1.GetDataRow(gridView1.FocusedRowHandle)["ID"]);
    }

    private void simpleButton2_Click(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmReplace));
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colFIO = new GridColumn();
      pictureEdit1 = new PictureEdit();
      labelControl1 = new LabelControl();
      simpleButton1 = new SimpleButton();
      simpleButton2 = new SimpleButton();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      pictureEdit1.Properties.BeginInit();
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
      gridView1.Appearance.ColumnFilterButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BackColor");
      gridView1.Appearance.ColumnFilterButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BackColor2");
      gridView1.Appearance.ColumnFilterButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.BorderColor");
      gridView1.Appearance.ColumnFilterButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.Font");
      gridView1.Appearance.ColumnFilterButton.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.FontSizeDelta");
      gridView1.Appearance.ColumnFilterButton.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.FontStyleDelta");
      gridView1.Appearance.ColumnFilterButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.ForeColor");
      gridView1.Appearance.ColumnFilterButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.GradientMode");
      gridView1.Appearance.ColumnFilterButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButton.Image");
      gridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseFont = true;
      gridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor");
      gridView1.Appearance.ColumnFilterButtonActive.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BackColor2");
      gridView1.Appearance.ColumnFilterButtonActive.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.BorderColor");
      gridView1.Appearance.ColumnFilterButtonActive.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.Font");
      gridView1.Appearance.ColumnFilterButtonActive.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.FontSizeDelta");
      gridView1.Appearance.ColumnFilterButtonActive.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.FontStyleDelta");
      gridView1.Appearance.ColumnFilterButtonActive.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.ForeColor");
      gridView1.Appearance.ColumnFilterButtonActive.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.GradientMode");
      gridView1.Appearance.ColumnFilterButtonActive.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ColumnFilterButtonActive.Image");
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
      gridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
      gridView1.Appearance.CustomizationFormHint.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.Font");
      gridView1.Appearance.CustomizationFormHint.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.FontSizeDelta");
      gridView1.Appearance.CustomizationFormHint.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.FontStyleDelta");
      gridView1.Appearance.CustomizationFormHint.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.GradientMode");
      gridView1.Appearance.CustomizationFormHint.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.CustomizationFormHint.Image");
      gridView1.Appearance.CustomizationFormHint.Options.UseFont = true;
      gridView1.Appearance.DetailTip.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.Font");
      gridView1.Appearance.DetailTip.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.FontSizeDelta");
      gridView1.Appearance.DetailTip.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.FontStyleDelta");
      gridView1.Appearance.DetailTip.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.GradientMode");
      gridView1.Appearance.DetailTip.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.DetailTip.Image");
      gridView1.Appearance.DetailTip.Options.UseFont = true;
      gridView1.Appearance.Empty.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Empty.BackColor");
      gridView1.Appearance.Empty.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Empty.Font");
      gridView1.Appearance.Empty.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.Empty.FontSizeDelta");
      gridView1.Appearance.Empty.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.Empty.FontStyleDelta");
      gridView1.Appearance.Empty.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Empty.GradientMode");
      gridView1.Appearance.Empty.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Empty.Image");
      gridView1.Appearance.Empty.Options.UseBackColor = true;
      gridView1.Appearance.Empty.Options.UseFont = true;
      gridView1.Appearance.EvenRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.BackColor");
      gridView1.Appearance.EvenRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.Font");
      gridView1.Appearance.EvenRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.FontSizeDelta");
      gridView1.Appearance.EvenRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.FontStyleDelta");
      gridView1.Appearance.EvenRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.ForeColor");
      gridView1.Appearance.EvenRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.GradientMode");
      gridView1.Appearance.EvenRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.EvenRow.Image");
      gridView1.Appearance.EvenRow.Options.UseBackColor = true;
      gridView1.Appearance.EvenRow.Options.UseFont = true;
      gridView1.Appearance.EvenRow.Options.UseForeColor = true;
      gridView1.Appearance.FilterCloseButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor");
      gridView1.Appearance.FilterCloseButton.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BackColor2");
      gridView1.Appearance.FilterCloseButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.BorderColor");
      gridView1.Appearance.FilterCloseButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.Font");
      gridView1.Appearance.FilterCloseButton.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.FontSizeDelta");
      gridView1.Appearance.FilterCloseButton.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.FontStyleDelta");
      gridView1.Appearance.FilterCloseButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.ForeColor");
      gridView1.Appearance.FilterCloseButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.GradientMode");
      gridView1.Appearance.FilterCloseButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FilterCloseButton.Image");
      gridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
      gridView1.Appearance.FilterCloseButton.Options.UseFont = true;
      gridView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
      gridView1.Appearance.FilterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.BackColor");
      gridView1.Appearance.FilterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.Font");
      gridView1.Appearance.FilterPanel.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.FontSizeDelta");
      gridView1.Appearance.FilterPanel.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.FontStyleDelta");
      gridView1.Appearance.FilterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.ForeColor");
      gridView1.Appearance.FilterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.GradientMode");
      gridView1.Appearance.FilterPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FilterPanel.Image");
      gridView1.Appearance.FilterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FilterPanel.Options.UseFont = true;
      gridView1.Appearance.FilterPanel.Options.UseForeColor = true;
      gridView1.Appearance.FixedLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.BackColor");
      gridView1.Appearance.FixedLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.Font");
      gridView1.Appearance.FixedLine.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.FontSizeDelta");
      gridView1.Appearance.FixedLine.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.FontStyleDelta");
      gridView1.Appearance.FixedLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.GradientMode");
      gridView1.Appearance.FixedLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FixedLine.Image");
      gridView1.Appearance.FixedLine.Options.UseBackColor = true;
      gridView1.Appearance.FixedLine.Options.UseFont = true;
      gridView1.Appearance.FocusedCell.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.BackColor");
      gridView1.Appearance.FocusedCell.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.Font");
      gridView1.Appearance.FocusedCell.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.FontSizeDelta");
      gridView1.Appearance.FocusedCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.FontStyleDelta");
      gridView1.Appearance.FocusedCell.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.ForeColor");
      gridView1.Appearance.FocusedCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.GradientMode");
      gridView1.Appearance.FocusedCell.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FocusedCell.Image");
      gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
      gridView1.Appearance.FocusedCell.Options.UseFont = true;
      gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
      gridView1.Appearance.FocusedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.BackColor");
      gridView1.Appearance.FocusedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.Font");
      gridView1.Appearance.FocusedRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.FontSizeDelta");
      gridView1.Appearance.FocusedRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.FontStyleDelta");
      gridView1.Appearance.FocusedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.ForeColor");
      gridView1.Appearance.FocusedRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.GradientMode");
      gridView1.Appearance.FocusedRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FocusedRow.Image");
      gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
      gridView1.Appearance.FocusedRow.Options.UseFont = true;
      gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
      gridView1.Appearance.FooterPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor");
      gridView1.Appearance.FooterPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BackColor2");
      gridView1.Appearance.FooterPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.BorderColor");
      gridView1.Appearance.FooterPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.Font");
      gridView1.Appearance.FooterPanel.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.FontSizeDelta");
      gridView1.Appearance.FooterPanel.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.FontStyleDelta");
      gridView1.Appearance.FooterPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.ForeColor");
      gridView1.Appearance.FooterPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.GradientMode");
      gridView1.Appearance.FooterPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.FooterPanel.Image");
      gridView1.Appearance.FooterPanel.Options.UseBackColor = true;
      gridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
      gridView1.Appearance.FooterPanel.Options.UseFont = true;
      gridView1.Appearance.FooterPanel.Options.UseForeColor = true;
      gridView1.Appearance.GroupButton.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BackColor");
      gridView1.Appearance.GroupButton.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.BorderColor");
      gridView1.Appearance.GroupButton.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.Font");
      gridView1.Appearance.GroupButton.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.FontSizeDelta");
      gridView1.Appearance.GroupButton.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.FontStyleDelta");
      gridView1.Appearance.GroupButton.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.ForeColor");
      gridView1.Appearance.GroupButton.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.GradientMode");
      gridView1.Appearance.GroupButton.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupButton.Image");
      gridView1.Appearance.GroupButton.Options.UseBackColor = true;
      gridView1.Appearance.GroupButton.Options.UseBorderColor = true;
      gridView1.Appearance.GroupButton.Options.UseFont = true;
      gridView1.Appearance.GroupButton.Options.UseForeColor = true;
      gridView1.Appearance.GroupFooter.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BackColor");
      gridView1.Appearance.GroupFooter.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.BorderColor");
      gridView1.Appearance.GroupFooter.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.Font");
      gridView1.Appearance.GroupFooter.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.FontSizeDelta");
      gridView1.Appearance.GroupFooter.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.FontStyleDelta");
      gridView1.Appearance.GroupFooter.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.ForeColor");
      gridView1.Appearance.GroupFooter.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.GradientMode");
      gridView1.Appearance.GroupFooter.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupFooter.Image");
      gridView1.Appearance.GroupFooter.Options.UseBackColor = true;
      gridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
      gridView1.Appearance.GroupFooter.Options.UseFont = true;
      gridView1.Appearance.GroupFooter.Options.UseForeColor = true;
      gridView1.Appearance.GroupPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.BackColor");
      gridView1.Appearance.GroupPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.Font");
      gridView1.Appearance.GroupPanel.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.FontSizeDelta");
      gridView1.Appearance.GroupPanel.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.FontStyleDelta");
      gridView1.Appearance.GroupPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.ForeColor");
      gridView1.Appearance.GroupPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.GradientMode");
      gridView1.Appearance.GroupPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupPanel.Image");
      gridView1.Appearance.GroupPanel.Options.UseBackColor = true;
      gridView1.Appearance.GroupPanel.Options.UseFont = true;
      gridView1.Appearance.GroupPanel.Options.UseForeColor = true;
      gridView1.Appearance.GroupRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.BackColor");
      gridView1.Appearance.GroupRow.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.BorderColor");
      gridView1.Appearance.GroupRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.Font");
      gridView1.Appearance.GroupRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.FontSizeDelta");
      gridView1.Appearance.GroupRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.FontStyleDelta");
      gridView1.Appearance.GroupRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.ForeColor");
      gridView1.Appearance.GroupRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.GradientMode");
      gridView1.Appearance.GroupRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.GroupRow.Image");
      gridView1.Appearance.GroupRow.Options.UseBackColor = true;
      gridView1.Appearance.GroupRow.Options.UseBorderColor = true;
      gridView1.Appearance.GroupRow.Options.UseFont = true;
      gridView1.Appearance.GroupRow.Options.UseForeColor = true;
      gridView1.Appearance.HeaderPanel.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BackColor");
      gridView1.Appearance.HeaderPanel.BackColor2 = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BackColor2");
      gridView1.Appearance.HeaderPanel.BorderColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.BorderColor");
      gridView1.Appearance.HeaderPanel.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.Font");
      gridView1.Appearance.HeaderPanel.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.FontSizeDelta");
      gridView1.Appearance.HeaderPanel.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.FontStyleDelta");
      gridView1.Appearance.HeaderPanel.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.ForeColor");
      gridView1.Appearance.HeaderPanel.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.GradientMode");
      gridView1.Appearance.HeaderPanel.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HeaderPanel.Image");
      gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
      gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
      gridView1.Appearance.HeaderPanel.Options.UseFont = true;
      gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
      gridView1.Appearance.HideSelectionRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.BackColor");
      gridView1.Appearance.HideSelectionRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.Font");
      gridView1.Appearance.HideSelectionRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.FontSizeDelta");
      gridView1.Appearance.HideSelectionRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.FontStyleDelta");
      gridView1.Appearance.HideSelectionRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.ForeColor");
      gridView1.Appearance.HideSelectionRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.GradientMode");
      gridView1.Appearance.HideSelectionRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HideSelectionRow.Image");
      gridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
      gridView1.Appearance.HideSelectionRow.Options.UseFont = true;
      gridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
      gridView1.Appearance.HorzLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.BackColor");
      gridView1.Appearance.HorzLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.Font");
      gridView1.Appearance.HorzLine.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.FontSizeDelta");
      gridView1.Appearance.HorzLine.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.FontStyleDelta");
      gridView1.Appearance.HorzLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.GradientMode");
      gridView1.Appearance.HorzLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.HorzLine.Image");
      gridView1.Appearance.HorzLine.Options.UseBackColor = true;
      gridView1.Appearance.HorzLine.Options.UseFont = true;
      gridView1.Appearance.OddRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.BackColor");
      gridView1.Appearance.OddRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.OddRow.Font");
      gridView1.Appearance.OddRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.OddRow.FontSizeDelta");
      gridView1.Appearance.OddRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.OddRow.FontStyleDelta");
      gridView1.Appearance.OddRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.OddRow.ForeColor");
      gridView1.Appearance.OddRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.OddRow.GradientMode");
      gridView1.Appearance.OddRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.OddRow.Image");
      gridView1.Appearance.OddRow.Options.UseBackColor = true;
      gridView1.Appearance.OddRow.Options.UseFont = true;
      gridView1.Appearance.OddRow.Options.UseForeColor = true;
      gridView1.Appearance.Preview.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Preview.Font");
      gridView1.Appearance.Preview.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.Preview.FontSizeDelta");
      gridView1.Appearance.Preview.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.Preview.FontStyleDelta");
      gridView1.Appearance.Preview.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Preview.GradientMode");
      gridView1.Appearance.Preview.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Preview.Image");
      gridView1.Appearance.Preview.Options.UseFont = true;
      gridView1.Appearance.Row.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.BackColor");
      gridView1.Appearance.Row.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.Row.Font");
      gridView1.Appearance.Row.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.Row.FontSizeDelta");
      gridView1.Appearance.Row.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.Row.FontStyleDelta");
      gridView1.Appearance.Row.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.Row.ForeColor");
      gridView1.Appearance.Row.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.Row.GradientMode");
      gridView1.Appearance.Row.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.Row.Image");
      gridView1.Appearance.Row.Options.UseBackColor = true;
      gridView1.Appearance.Row.Options.UseFont = true;
      gridView1.Appearance.Row.Options.UseForeColor = true;
      gridView1.Appearance.RowSeparator.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.BackColor");
      gridView1.Appearance.RowSeparator.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.Font");
      gridView1.Appearance.RowSeparator.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.FontSizeDelta");
      gridView1.Appearance.RowSeparator.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.FontStyleDelta");
      gridView1.Appearance.RowSeparator.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.GradientMode");
      gridView1.Appearance.RowSeparator.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.RowSeparator.Image");
      gridView1.Appearance.RowSeparator.Options.UseBackColor = true;
      gridView1.Appearance.RowSeparator.Options.UseFont = true;
      gridView1.Appearance.SelectedRow.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.BackColor");
      gridView1.Appearance.SelectedRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.Font");
      gridView1.Appearance.SelectedRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.FontSizeDelta");
      gridView1.Appearance.SelectedRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.FontStyleDelta");
      gridView1.Appearance.SelectedRow.ForeColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.ForeColor");
      gridView1.Appearance.SelectedRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.GradientMode");
      gridView1.Appearance.SelectedRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.SelectedRow.Image");
      gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
      gridView1.Appearance.SelectedRow.Options.UseFont = true;
      gridView1.Appearance.SelectedRow.Options.UseForeColor = true;
      gridView1.Appearance.TopNewRow.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.Font");
      gridView1.Appearance.TopNewRow.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.FontSizeDelta");
      gridView1.Appearance.TopNewRow.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.FontStyleDelta");
      gridView1.Appearance.TopNewRow.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.GradientMode");
      gridView1.Appearance.TopNewRow.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.TopNewRow.Image");
      gridView1.Appearance.TopNewRow.Options.UseFont = true;
      gridView1.Appearance.VertLine.BackColor = (Color) componentResourceManager.GetObject("gridView1.Appearance.VertLine.BackColor");
      gridView1.Appearance.VertLine.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.VertLine.Font");
      gridView1.Appearance.VertLine.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.VertLine.FontSizeDelta");
      gridView1.Appearance.VertLine.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.VertLine.FontStyleDelta");
      gridView1.Appearance.VertLine.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.VertLine.GradientMode");
      gridView1.Appearance.VertLine.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.VertLine.Image");
      gridView1.Appearance.VertLine.Options.UseBackColor = true;
      gridView1.Appearance.VertLine.Options.UseFont = true;
      gridView1.Appearance.ViewCaption.Font = (Font) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.Font");
      gridView1.Appearance.ViewCaption.FontSizeDelta = (int) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.FontSizeDelta");
      gridView1.Appearance.ViewCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.FontStyleDelta");
      gridView1.Appearance.ViewCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.GradientMode");
      gridView1.Appearance.ViewCaption.Image = (Image) componentResourceManager.GetObject("gridView1.Appearance.ViewCaption.Image");
      gridView1.Appearance.ViewCaption.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.Columns.AddRange(new GridColumn[2]
      {
        colID,
        colFIO
      });
      gridView1.GridControl = gridControl1;
      gridView1.IndicatorWidth = 60;
      gridView1.Name = "gridView1";
      gridView1.OptionsFind.AlwaysVisible = true;
      gridView1.OptionsFind.ClearFindOnClose = false;
      gridView1.OptionsFind.FindDelay = 10000;
      gridView1.OptionsFind.FindMode = FindMode.Always;
      gridView1.OptionsFind.ShowCloseButton = false;
      gridView1.OptionsSelection.MultiSelect = true;
      gridView1.OptionsView.AutoCalcPreviewLineCount = true;
      gridView1.OptionsView.ColumnAutoWidth = false;
      gridView1.OptionsView.EnableAppearanceEvenRow = true;
      gridView1.OptionsView.EnableAppearanceOddRow = true;
      gridView1.OptionsView.ShowGroupPanel = false;
      gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;
      colID.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colID.AppearanceHeader.Font");
      colID.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colID.AppearanceHeader.FontSizeDelta");
      colID.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colID.AppearanceHeader.FontStyleDelta");
      colID.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colID.AppearanceHeader.GradientMode");
      colID.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colID.AppearanceHeader.Image");
      colID.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colID, "colID");
      colID.FieldName = "ID";
      colID.Name = "colID";
      colID.OptionsColumn.AllowEdit = false;
      colID.OptionsColumn.ReadOnly = true;
      colFIO.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colFIO.AppearanceHeader.Font");
      colFIO.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colFIO.AppearanceHeader.FontSizeDelta");
      colFIO.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colFIO.AppearanceHeader.FontStyleDelta");
      colFIO.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colFIO.AppearanceHeader.GradientMode");
      colFIO.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colFIO.AppearanceHeader.Image");
      colFIO.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colFIO, "colFIO");
      colFIO.FieldName = "FIO";
      colFIO.Name = "colFIO";
      colFIO.OptionsColumn.AllowEdit = false;
      colFIO.OptionsColumn.ReadOnly = true;
      componentResourceManager.ApplyResources(pictureEdit1, "pictureEdit1");
      pictureEdit1.Name = "pictureEdit1";
      pictureEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleDescription");
      pictureEdit1.Properties.AccessibleName = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleName");
      pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.DisabledImage");
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      labelControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl1.Appearance.FontSizeDelta");
      labelControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl1.Appearance.FontStyleDelta");
      labelControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl1.Appearance.GradientMode");
      labelControl1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.HoverImage");
      labelControl1.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl1.Appearance.Image");
      labelControl1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.PressedImage");
      labelControl1.LookAndFeel.SkinName = "Office 2007 Blue";
      labelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(simpleButton1, "simpleButton1");
      simpleButton1.Appearance.Font = (Font) componentResourceManager.GetObject("simpleButton1.Appearance.Font");
      simpleButton1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("simpleButton1.Appearance.FontSizeDelta");
      simpleButton1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("simpleButton1.Appearance.FontStyleDelta");
      simpleButton1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("simpleButton1.Appearance.GradientMode");
      simpleButton1.Appearance.Image = (Image) componentResourceManager.GetObject("simpleButton1.Appearance.Image");
      simpleButton1.Appearance.Options.UseFont = true;
      simpleButton1.DialogResult = DialogResult.OK;
      simpleButton1.Name = "simpleButton1";
      simpleButton1.Click += simpleButton1_Click;
      componentResourceManager.ApplyResources(simpleButton2, "simpleButton2");
      simpleButton2.Appearance.Font = (Font) componentResourceManager.GetObject("simpleButton2.Appearance.Font");
      simpleButton2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("simpleButton2.Appearance.FontSizeDelta");
      simpleButton2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("simpleButton2.Appearance.FontStyleDelta");
      simpleButton2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("simpleButton2.Appearance.GradientMode");
      simpleButton2.Appearance.Image = (Image) componentResourceManager.GetObject("simpleButton2.Appearance.Image");
      simpleButton2.Appearance.Options.UseFont = true;
      simpleButton2.DialogResult = DialogResult.Cancel;
      simpleButton2.Name = "simpleButton2";
      simpleButton2.Click += simpleButton2_Click;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(simpleButton2);
      Controls.Add(simpleButton1);
      Controls.Add(gridControl1);
      Controls.Add(pictureEdit1);
      Controls.Add(labelControl1);
      Name = "FrmReplace";
      ShowIcon = false;
      gridControl1.EndInit();
      gridView1.EndInit();
      pictureEdit1.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
