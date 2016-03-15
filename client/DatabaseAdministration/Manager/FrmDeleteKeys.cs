// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmDeleteKeys
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;

namespace CascadeManager
{
  public class FrmDeleteKeys : XtraForm
  {
    private IContainer components = null;
    private GroupControl groupBox3;
    private CheckEdit checkEditBefore;
    private CheckEdit checkEditFrom;
    private DateEdit dtpBefore;
    private DateEdit dtpFrom;
    private LabelControl labelControl1;
    private SimpleButton btAccept;
    private DateEdit dtValueFrom;
    private SimpleButton btSave;
    private GroupControl groupControl1;
    private LabelControl labelControl2;
    private SimpleButton simpleButton1;

    public FrmDeleteKeys()
    {
      InitializeComponent();
    }

    private void checkEditFrom_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditFrom.Checked)
        dtpFrom.Enabled = true;
      else
        dtpFrom.Enabled = false;
    }

    private void checkEditBefore_CheckedChanged(object sender, EventArgs e)
    {
      if (checkEditBefore.Checked)
        dtpBefore.Enabled = true;
      else
        dtpBefore.Enabled = false;
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand("if((Select count( Datefrom) from DateSettings)>0)\r\nbegin\r\nUpdate\r\nDateSettings\r\nSet\r\nDatefrom = @date\r\nend\r\nelse\r\nbegin\r\nInsert Into DateSettings\r\n(\r\nDatefrom\r\n)\r\nVALUES\r\n(\r\n@date\r\n)\r\nend\r\n\r\n", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Parameters.Add(new SqlParameter("@date", dtValueFrom.DateTime));
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
        sqlCommand.Connection.Close();
        int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      string str = "Select ID from CascadeStream.dbo.Faces WHERE";
      if (checkEditFrom.Checked && checkEditBefore.Checked)
        str += " (CascadeStream.dbo.Faces.ModifiedDate>=@datefrom \r\nand CascadeStream.dbo.Faces.ModifiedDate <= @datebefore)";
      else if (checkEditBefore.Checked)
        str += " (CascadeStream.dbo.Faces.ModifiedDate <= @datebefore) ";
      else if (checkEditFrom.Checked)
        str += " (CascadeStream.dbo.Faces.ModifiedDate>=@datefrom) ";
      string cmdText = "Delete CSKeys.dbo.Keys WHERE\r\n            FaceID in (" + str + ") and KSID>=0";
      try
      {
        if (checkEditFrom.Checked || checkEditBefore.Checked)
        {
          SqlCommand sqlCommand = new SqlCommand(cmdText, new SqlConnection(CommonSettings.ConnectionString));
          if (checkEditFrom.Checked)
            sqlCommand.Parameters.Add(new SqlParameter("@datefrom", dtpFrom.DateTime));
          if (checkEditBefore.Checked)
            sqlCommand.Parameters.Add(new SqlParameter("@datebefore", dtpBefore.DateTime));
          sqlCommand.Connection.Open();
          sqlCommand.ExecuteNonQuery();
          sqlCommand.Connection.Close();
          int num = (int) XtraMessageBox.Show(Messages.OperationComplete, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) XtraMessageBox.Show(Messages.EnterPeriod, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void frmDeleteKeys_Load(object sender, EventArgs e)
    {
      try
      {
        SqlCommand sqlCommand = new SqlCommand("Select * from DateSettings", new SqlConnection(CommonSettings.ConnectionString));
        sqlCommand.Connection.Open();
        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
        while (sqlDataReader.Read())
          dtValueFrom.DateTime = (DateTime) sqlDataReader[0];
        sqlDataReader.Close();
        sqlCommand.Connection.Close();
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void simpleButton1_Click(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmDeleteKeys));
      groupBox3 = new GroupControl();
      checkEditBefore = new CheckEdit();
      checkEditFrom = new CheckEdit();
      btAccept = new SimpleButton();
      dtpBefore = new DateEdit();
      dtpFrom = new DateEdit();
      labelControl1 = new LabelControl();
      dtValueFrom = new DateEdit();
      btSave = new SimpleButton();
      groupControl1 = new GroupControl();
      labelControl2 = new LabelControl();
      simpleButton1 = new SimpleButton();
      groupBox3.BeginInit();
      groupBox3.SuspendLayout();
      checkEditBefore.Properties.BeginInit();
      checkEditFrom.Properties.BeginInit();
      dtpBefore.Properties.CalendarTimeProperties.BeginInit();
      dtpBefore.Properties.BeginInit();
      dtpFrom.Properties.CalendarTimeProperties.BeginInit();
      dtpFrom.Properties.BeginInit();
      dtValueFrom.Properties.CalendarTimeProperties.BeginInit();
      dtValueFrom.Properties.BeginInit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      SuspendLayout();
      componentResourceManager.ApplyResources(groupBox3, "groupBox3");
      groupBox3.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox3.AppearanceCaption.Font");
      groupBox3.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupBox3.AppearanceCaption.FontSizeDelta");
      groupBox3.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupBox3.AppearanceCaption.FontStyleDelta");
      groupBox3.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupBox3.AppearanceCaption.GradientMode");
      groupBox3.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupBox3.AppearanceCaption.Image");
      groupBox3.AppearanceCaption.Options.UseFont = true;
      groupBox3.Controls.Add(checkEditBefore);
      groupBox3.Controls.Add(checkEditFrom);
      groupBox3.Controls.Add(btAccept);
      groupBox3.Controls.Add(dtpBefore);
      groupBox3.Controls.Add(dtpFrom);
      groupBox3.Name = "groupBox3";
      groupBox3.ShowCaption = false;
      groupBox3.TabStop = true;
      componentResourceManager.ApplyResources(checkEditBefore, "checkEditBefore");
      checkEditBefore.Name = "checkEditBefore";
      checkEditBefore.Properties.AccessibleDescription = componentResourceManager.GetString("checkEditBefore.Properties.AccessibleDescription");
      checkEditBefore.Properties.AccessibleName = componentResourceManager.GetString("checkEditBefore.Properties.AccessibleName");
      checkEditBefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.Font");
      checkEditBefore.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.FontSizeDelta");
      checkEditBefore.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.FontStyleDelta");
      checkEditBefore.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.GradientMode");
      checkEditBefore.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("checkEditBefore.Properties.Appearance.Image");
      checkEditBefore.Properties.Appearance.Options.UseFont = true;
      checkEditBefore.Properties.AutoHeight = (bool) componentResourceManager.GetObject("checkEditBefore.Properties.AutoHeight");
      checkEditBefore.Properties.Caption = componentResourceManager.GetString("checkEditBefore.Properties.Caption");
      checkEditBefore.Properties.DisplayValueChecked = componentResourceManager.GetString("checkEditBefore.Properties.DisplayValueChecked");
      checkEditBefore.Properties.DisplayValueGrayed = componentResourceManager.GetString("checkEditBefore.Properties.DisplayValueGrayed");
      checkEditBefore.Properties.DisplayValueUnchecked = componentResourceManager.GetString("checkEditBefore.Properties.DisplayValueUnchecked");
      checkEditBefore.CheckedChanged += checkEditBefore_CheckedChanged;
      componentResourceManager.ApplyResources(checkEditFrom, "checkEditFrom");
      checkEditFrom.Name = "checkEditFrom";
      checkEditFrom.Properties.AccessibleDescription = componentResourceManager.GetString("checkEditFrom.Properties.AccessibleDescription");
      checkEditFrom.Properties.AccessibleName = componentResourceManager.GetString("checkEditFrom.Properties.AccessibleName");
      checkEditFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.Font");
      checkEditFrom.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.FontSizeDelta");
      checkEditFrom.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.FontStyleDelta");
      checkEditFrom.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.GradientMode");
      checkEditFrom.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("checkEditFrom.Properties.Appearance.Image");
      checkEditFrom.Properties.Appearance.Options.UseFont = true;
      checkEditFrom.Properties.AutoHeight = (bool) componentResourceManager.GetObject("checkEditFrom.Properties.AutoHeight");
      checkEditFrom.Properties.Caption = componentResourceManager.GetString("checkEditFrom.Properties.Caption");
      checkEditFrom.Properties.DisplayValueChecked = componentResourceManager.GetString("checkEditFrom.Properties.DisplayValueChecked");
      checkEditFrom.Properties.DisplayValueGrayed = componentResourceManager.GetString("checkEditFrom.Properties.DisplayValueGrayed");
      checkEditFrom.Properties.DisplayValueUnchecked = componentResourceManager.GetString("checkEditFrom.Properties.DisplayValueUnchecked");
      checkEditFrom.CheckedChanged += checkEditFrom_CheckedChanged;
      componentResourceManager.ApplyResources(btAccept, "btAccept");
      btAccept.Appearance.Font = (Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      btAccept.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
      btAccept.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
      btAccept.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
      btAccept.Appearance.Image = (Image) componentResourceManager.GetObject("btAccept.Appearance.Image");
      btAccept.Appearance.Options.UseFont = true;
      btAccept.Name = "btAccept";
      btAccept.Click += btAccept_Click;
      componentResourceManager.ApplyResources(dtpBefore, "dtpBefore");
      dtpBefore.Name = "dtpBefore";
      dtpBefore.Properties.AccessibleDescription = componentResourceManager.GetString("dtpBefore.Properties.AccessibleDescription");
      dtpBefore.Properties.AccessibleName = componentResourceManager.GetString("dtpBefore.Properties.AccessibleName");
      dtpBefore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.Font");
      dtpBefore.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.FontSizeDelta");
      dtpBefore.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.FontStyleDelta");
      dtpBefore.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.GradientMode");
      dtpBefore.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtpBefore.Properties.Appearance.Image");
      dtpBefore.Properties.Appearance.Options.UseFont = true;
      dtpBefore.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBefore.Properties.AutoHeight");
      dtpBefore.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpBefore.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtpBefore.Properties.CalendarTimeProperties.AccessibleDescription");
      dtpBefore.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtpBefore.Properties.CalendarTimeProperties.AccessibleName");
      dtpBefore.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.AutoHeight");
      dtpBefore.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpBefore.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpBefore.Properties.CalendarTimeProperties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpBefore.Properties.CalendarTimeProperties.EditFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.AutoComplete");
      dtpBefore.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.BeepOnError");
      dtpBefore.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpBefore.Properties.CalendarTimeProperties.Mask.EditMask");
      dtpBefore.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      dtpBefore.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.MaskType");
      dtpBefore.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      dtpBefore.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      dtpBefore.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      dtpBefore.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      dtpBefore.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtpBefore.Properties.CalendarTimeProperties.NullValuePrompt");
      dtpBefore.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBefore.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      dtpBefore.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      dtpBefore.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtpBefore.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtpBefore.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpBefore.Properties.Mask.AutoComplete");
      dtpBefore.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpBefore.Properties.Mask.BeepOnError");
      dtpBefore.Properties.Mask.EditMask = componentResourceManager.GetString("dtpBefore.Properties.Mask.EditMask");
      dtpBefore.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpBefore.Properties.Mask.IgnoreMaskBlank");
      dtpBefore.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpBefore.Properties.Mask.MaskType");
      dtpBefore.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpBefore.Properties.Mask.PlaceHolder");
      dtpBefore.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpBefore.Properties.Mask.SaveLiteral");
      dtpBefore.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpBefore.Properties.Mask.ShowPlaceHolders");
      dtpBefore.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpBefore.Properties.Mask.UseMaskAsDisplayFormat");
      dtpBefore.Properties.NullValuePrompt = componentResourceManager.GetString("dtpBefore.Properties.NullValuePrompt");
      dtpBefore.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpBefore.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(dtpFrom, "dtpFrom");
      dtpFrom.Name = "dtpFrom";
      dtpFrom.Properties.AccessibleDescription = componentResourceManager.GetString("dtpFrom.Properties.AccessibleDescription");
      dtpFrom.Properties.AccessibleName = componentResourceManager.GetString("dtpFrom.Properties.AccessibleName");
      dtpFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.Font");
      dtpFrom.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.FontSizeDelta");
      dtpFrom.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.FontStyleDelta");
      dtpFrom.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.GradientMode");
      dtpFrom.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtpFrom.Properties.Appearance.Image");
      dtpFrom.Properties.Appearance.Options.UseFont = true;
      dtpFrom.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtpFrom.Properties.AutoHeight");
      dtpFrom.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpFrom.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtpFrom.Properties.CalendarTimeProperties.AccessibleDescription");
      dtpFrom.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtpFrom.Properties.CalendarTimeProperties.AccessibleName");
      dtpFrom.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.AutoHeight");
      dtpFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtpFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtpFrom.Properties.CalendarTimeProperties.EditFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.AutoComplete");
      dtpFrom.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.BeepOnError");
      dtpFrom.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtpFrom.Properties.CalendarTimeProperties.Mask.EditMask");
      dtpFrom.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      dtpFrom.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.MaskType");
      dtpFrom.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      dtpFrom.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      dtpFrom.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      dtpFrom.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      dtpFrom.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtpFrom.Properties.CalendarTimeProperties.NullValuePrompt");
      dtpFrom.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpFrom.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      dtpFrom.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      dtpFrom.Properties.DisplayFormat.FormatType = FormatType.Custom;
      dtpFrom.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtpFrom.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtpFrom.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtpFrom.Properties.Mask.AutoComplete");
      dtpFrom.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtpFrom.Properties.Mask.BeepOnError");
      dtpFrom.Properties.Mask.EditMask = componentResourceManager.GetString("dtpFrom.Properties.Mask.EditMask");
      dtpFrom.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtpFrom.Properties.Mask.IgnoreMaskBlank");
      dtpFrom.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtpFrom.Properties.Mask.MaskType");
      dtpFrom.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtpFrom.Properties.Mask.PlaceHolder");
      dtpFrom.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtpFrom.Properties.Mask.SaveLiteral");
      dtpFrom.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtpFrom.Properties.Mask.ShowPlaceHolders");
      dtpFrom.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtpFrom.Properties.Mask.UseMaskAsDisplayFormat");
      dtpFrom.Properties.NullValuePrompt = componentResourceManager.GetString("dtpFrom.Properties.NullValuePrompt");
      dtpFrom.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtpFrom.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(labelControl1, "labelControl1");
      labelControl1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.DisabledImage");
      labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      labelControl1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl1.Appearance.FontSizeDelta");
      labelControl1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl1.Appearance.FontStyleDelta");
      labelControl1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl1.Appearance.GradientMode");
      labelControl1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.HoverImage");
      labelControl1.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl1.Appearance.Image");
      labelControl1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl1.Appearance.PressedImage");
      labelControl1.Name = "labelControl1";
      componentResourceManager.ApplyResources(dtValueFrom, "dtValueFrom");
      dtValueFrom.Name = "dtValueFrom";
      dtValueFrom.Properties.AccessibleDescription = componentResourceManager.GetString("dtValueFrom.Properties.AccessibleDescription");
      dtValueFrom.Properties.AccessibleName = componentResourceManager.GetString("dtValueFrom.Properties.AccessibleName");
      dtValueFrom.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("dtValueFrom.Properties.Appearance.Font");
      dtValueFrom.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("dtValueFrom.Properties.Appearance.FontSizeDelta");
      dtValueFrom.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("dtValueFrom.Properties.Appearance.FontStyleDelta");
      dtValueFrom.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("dtValueFrom.Properties.Appearance.GradientMode");
      dtValueFrom.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("dtValueFrom.Properties.Appearance.Image");
      dtValueFrom.Properties.Appearance.Options.UseFont = true;
      dtValueFrom.Properties.AutoHeight = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.AutoHeight");
      dtValueFrom.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtValueFrom.Properties.CalendarTimeProperties.AccessibleDescription = componentResourceManager.GetString("dtValueFrom.Properties.CalendarTimeProperties.AccessibleDescription");
      dtValueFrom.Properties.CalendarTimeProperties.AccessibleName = componentResourceManager.GetString("dtValueFrom.Properties.CalendarTimeProperties.AccessibleName");
      dtValueFrom.Properties.CalendarTimeProperties.AutoHeight = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.AutoHeight");
      dtValueFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      dtValueFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtValueFrom.Properties.CalendarTimeProperties.DisplayFormat.FormatType = FormatType.DateTime;
      dtValueFrom.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd.MMMM.yyyy hh:mm:ss";
      dtValueFrom.Properties.CalendarTimeProperties.EditFormat.FormatType = FormatType.DateTime;
      dtValueFrom.Properties.CalendarTimeProperties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.AutoComplete");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.BeepOnError");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.EditMask = componentResourceManager.GetString("dtValueFrom.Properties.CalendarTimeProperties.Mask.EditMask");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.IgnoreMaskBlank");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.MaskType");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.PlaceHolder");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.SaveLiteral");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.ShowPlaceHolders");
      dtValueFrom.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.Mask.UseMaskAsDisplayFormat");
      dtValueFrom.Properties.CalendarTimeProperties.NullValuePrompt = componentResourceManager.GetString("dtValueFrom.Properties.CalendarTimeProperties.NullValuePrompt");
      dtValueFrom.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.CalendarTimeProperties.NullValuePromptShowForEmptyValue");
      dtValueFrom.Properties.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm:ss";
      dtValueFrom.Properties.DisplayFormat.FormatType = FormatType.DateTime;
      dtValueFrom.Properties.EditFormat.FormatString = "dd.MMMM.yyyy HH:mm:ss";
      dtValueFrom.Properties.EditFormat.FormatType = FormatType.DateTime;
      dtValueFrom.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.AutoComplete");
      dtValueFrom.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.BeepOnError");
      dtValueFrom.Properties.Mask.EditMask = componentResourceManager.GetString("dtValueFrom.Properties.Mask.EditMask");
      dtValueFrom.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.IgnoreMaskBlank");
      dtValueFrom.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.MaskType");
      dtValueFrom.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.PlaceHolder");
      dtValueFrom.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.SaveLiteral");
      dtValueFrom.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.ShowPlaceHolders");
      dtValueFrom.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.Mask.UseMaskAsDisplayFormat");
      dtValueFrom.Properties.NullValuePrompt = componentResourceManager.GetString("dtValueFrom.Properties.NullValuePrompt");
      dtValueFrom.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("dtValueFrom.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      btSave.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.Appearance.FontSizeDelta");
      btSave.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.Appearance.FontStyleDelta");
      btSave.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btSave.Appearance.GradientMode");
      btSave.Appearance.Image = (Image) componentResourceManager.GetObject("btSave.Appearance.Image");
      btSave.Appearance.Options.UseFont = true;
      btSave.Name = "btSave";
      btSave.Click += btSave_Click;
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.Controls.Add(btSave);
      groupControl1.Controls.Add(dtValueFrom);
      groupControl1.Name = "groupControl1";
      groupControl1.ShowCaption = false;
      componentResourceManager.ApplyResources(labelControl2, "labelControl2");
      labelControl2.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.DisabledImage");
      labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      labelControl2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl2.Appearance.FontSizeDelta");
      labelControl2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl2.Appearance.FontStyleDelta");
      labelControl2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl2.Appearance.GradientMode");
      labelControl2.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.HoverImage");
      labelControl2.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl2.Appearance.Image");
      labelControl2.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl2.Appearance.PressedImage");
      labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources(simpleButton1, "simpleButton1");
      simpleButton1.Appearance.Font = (Font) componentResourceManager.GetObject("simpleButton1.Appearance.Font");
      simpleButton1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("simpleButton1.Appearance.FontSizeDelta");
      simpleButton1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("simpleButton1.Appearance.FontStyleDelta");
      simpleButton1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("simpleButton1.Appearance.GradientMode");
      simpleButton1.Appearance.Image = (Image) componentResourceManager.GetObject("simpleButton1.Appearance.Image");
      simpleButton1.Appearance.Options.UseFont = true;
      simpleButton1.DialogResult = DialogResult.Cancel;
      simpleButton1.Name = "simpleButton1";
      simpleButton1.Click += simpleButton1_Click;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = simpleButton1;
      Controls.Add(simpleButton1);
      Controls.Add(labelControl2);
      Controls.Add(groupControl1);
      Controls.Add(labelControl1);
      Controls.Add(groupBox3);
      Name = "FrmDeleteKeys";
      ShowIcon = false;
      Load += frmDeleteKeys_Load;
      groupBox3.EndInit();
      groupBox3.ResumeLayout(false);
      checkEditBefore.Properties.EndInit();
      checkEditFrom.Properties.EndInit();
      dtpBefore.Properties.CalendarTimeProperties.EndInit();
      dtpBefore.Properties.EndInit();
      dtpFrom.Properties.CalendarTimeProperties.EndInit();
      dtpFrom.Properties.EndInit();
      dtValueFrom.Properties.CalendarTimeProperties.EndInit();
      dtValueFrom.Properties.EndInit();
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
