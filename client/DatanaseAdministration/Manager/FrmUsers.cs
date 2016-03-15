// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmUsers
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace CascadeManager
{
  public class FrmUsers : XtraForm
  {
    private DataTable _dtMain = new DataTable();
    private BcUser _loadedUser = new BcUser();
    private IContainer components = null;
    private LabelControl label1;
    private GroupControl groupBox1;
    private TextEdit tbCheckPassword;
    private LabelControl label4;
    private TextEdit tbPassword;
    private LabelControl label3;
    private TextEdit tbLogin;
    private LabelControl label2;
    private TextEdit tbFIO;
    private LabelControl label6;
    private GroupControl groupBox2;
    private SimpleButton btDelete;
    private SimpleButton btCancel;
    private SimpleButton btSave;
    private SimpleButton btAdd;
    private SimpleButton btEdit;
    private CheckEdit chbChangePassword;
    private LabelControl label5;
    private LabelControl label7;
    private SimpleButton btPermissions;
    private ComboBoxEdit cbRole;
    private CheckedListBoxControl chlbActions;
    private GridControl gridControl1;
    private GridView gridView1;
    private GridColumn colID;
    private GridColumn colName;
    private GridColumn colIP;
    private GridColumn colPosition;
    private GridColumn colComment;
    private GridColumn colState;
    private GridColumn gridColumn1;
    private MemoEdit tbComment;

    public FrmUsers()
    {
      InitializeComponent();
    }

    private void LoadGrid()
    {
      groupBox1.Enabled = false;
      gridControl1.Enabled = true;
      btCancel.Enabled = false;
      btPermissions.Enabled = false;
      btEdit.Enabled = true;
      btSave.Enabled = false;
      btDelete.Enabled = true;
      _dtMain = BcUser.LoadAllUsers();
      gridControl1.DataSource = _dtMain;
    }

    private void UsersForm_Load(object sender, EventArgs e)
    {
      ControlBox = false;
      LoadGrid();
    }

    private void btAdd_Click(object sender, EventArgs e)
    {
      chbChangePassword.Checked = false;
      chbChangePassword.Visible = false;
      groupBox1.Enabled = true;
      gridControl1.Enabled = false;
      btCancel.Enabled = true;
      btEdit.Enabled = false;
      btDelete.Enabled = false;
      btSave.Enabled = true;
      btAdd.Enabled = true;
      tbPassword.Text = "";
      btPermissions.Enabled = true;
      tbCheckPassword.Text = "";
      tbFIO.Text = "";
      btAdd.Enabled = false;
      tbLogin.Text = "";
      cbRole.SelectedIndex = 0;
      tbComment.Text = "";
      tbPassword.Enabled = true;
      tbCheckPassword.Enabled = true;
      _loadedUser = new BcUser();
    }

    private void btEdit_Click(object sender, EventArgs e)
    {
      chbChangePassword.Checked = false;
      chbChangePassword.Visible = true;
      groupBox1.Enabled = true;
      btSave.Enabled = true;
      gridControl1.Enabled = false;
      btAdd.Enabled = false;
      btPermissions.Enabled = true;
      btCancel.Enabled = true;
      btDelete.Enabled = false;
      btEdit.Enabled = false;
      tbPassword.Enabled = false;
      tbCheckPassword.Enabled = false;
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      _loadedUser.Password = tbPassword.Text;
      _loadedUser.Password = tbCheckPassword.Text;
      _loadedUser.Login = tbLogin.Text;
      _loadedUser.UserName = tbFIO.Text;
      _loadedUser.Comment = tbComment.Text;
      _loadedUser.Role = cbRole.SelectedIndex;
      int[] array = new int[chlbActions.CheckedItems.Count];
      int index = 0;
      foreach (CheckedListBoxItem checkedListBoxItem in chlbActions.CheckedItems)
      {
        array[index] = chlbActions.Items.IndexOf(checkedListBoxItem);
        ++index;
      }
      _loadedUser.Actions = _loadedUser.SetActions(array);
      if (chbChangePassword.Checked)
        _loadedUser.Password = Encoding.ASCII.GetString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(tbPassword.Text)));
      else if (!chbChangePassword.Visible)
        _loadedUser.Password = Encoding.ASCII.GetString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(tbPassword.Text)));
      bool flag = true;
      foreach (DataRow dataRow in (InternalDataCollectionBase) _dtMain.Rows)
      {
        if (_loadedUser.Login == dataRow["Login"].ToString() && _loadedUser.Id != (Guid) dataRow["ID"])
        {
          flag = false;
          int num = (int) XtraMessageBox.Show(Messages.UserNameAlreadyExist, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        }
      }
      if (tbCheckPassword.Text == "" || tbCheckPassword.Text != tbPassword.Text)
      {
        flag = false;
        int num = (int) XtraMessageBox.Show(Messages.CheckPassword, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (tbFIO.Text == "" || tbLogin.Text == "" || cbRole.Text == "")
      {
        flag = false;
        int num = (int) XtraMessageBox.Show(Messages.CheckInputParameters, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (!flag)
        return;
      chbChangePassword.Checked = false;
      chbChangePassword.Visible = false;
      if (_loadedUser.Id == Guid.Empty)
        _loadedUser.Save();
      else
        _loadedUser.Save();
      btAdd.Enabled = true;
      int dataSourceRowIndex = gridView1.GetFocusedDataSourceRowIndex();
      int delta = -1;
      if (gridView1.GetSelectedRows().Length > 0)
        delta = gridView1.GetSelectedRows()[0];
      LoadGrid();
      if (delta != -1)
        gridView1.SelectRow(dataSourceRowIndex);
      if (dataSourceRowIndex != -1)
        gridView1.MoveBy(delta);
      if (_loadedUser.Id == Guid.Empty)
      {
        gridView1.SelectRow(0);
      }
      else
      {
        int dataSourceIndex = -1;
        gridView1.ClearSelection();
        foreach (DataRow dataRow in (InternalDataCollectionBase) _dtMain.Rows)
        {
          if (_loadedUser.Id == (Guid) dataRow["ID"])
          {
            gridView1.SelectRow(gridView1.GetRowHandle(dataSourceIndex));
            break;
          }
          ++dataSourceIndex;
        }
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      chbChangePassword.Checked = false;
      chbChangePassword.Visible = false;
      groupBox1.Enabled = false;
      gridControl1.Enabled = true;
      btCancel.Enabled = false;
      btAdd.Enabled = true;
      btDelete.Enabled = true;
      btEdit.Enabled = true;
      btSave.Enabled = false;
      btPermissions.Enabled = false;
      dgvUsers_SelectionChanged(new object(), new EventArgs());
    }

    private void btDelete_Click(object sender, EventArgs e)
    {
      if (gridView1.SelectedRowsCount <= 0 || XtraMessageBox.Show(Messages.DouYouWantToDelete, Messages.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      _loadedUser.Delete();
      LoadGrid();
    }

    private void dgvUsers_SelectionChanged(object sender, EventArgs e)
    {
    }

    private void chbChangePassword_CheckedChanged(object sender, EventArgs e)
    {
      if (chbChangePassword.Checked)
      {
        tbCheckPassword.Enabled = true;
        tbPassword.Enabled = true;
      }
      else
      {
        tbCheckPassword.Enabled = false;
        tbPassword.Enabled = false;
      }
    }

    private void UsersForm_HelpButtonClicked(object sender, CancelEventArgs e)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/9.htm");
    }

    private void UsersForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/9.htm");
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (gridView1.SelectedRowsCount <= 0)
        return;
      _loadedUser = BcUser.LoadUserById((Guid) _dtMain.Rows[gridView1.GetSelectedRows()[0]][0]);
      tbPassword.Text = _loadedUser.Password;
      tbCheckPassword.Text = _loadedUser.Password;
      tbLogin.Text = _loadedUser.Login;
      tbFIO.Text = _loadedUser.UserName;
      tbComment.Text = _loadedUser.Comment;
      cbRole.SelectedIndex = _loadedUser.Role;
      int[] actions = _loadedUser.GetActions();
      for (int index1 = 0; index1 < chlbActions.Items.Count; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < actions.Length; ++index2)
        {
          if (index1 == actions[index2])
          {
            chlbActions.SetItemCheckState(index1, CheckState.Checked);
            flag = true;
            break;
          }
        }
        if (!flag)
          chlbActions.SetItemCheckState(index1, CheckState.Unchecked);
      }
    }

    private void UsersForm_Resize(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmUsers));
      label1 = new LabelControl();
      groupBox1 = new GroupControl();
      tbComment = new MemoEdit();
      chlbActions = new CheckedListBoxControl();
      label7 = new LabelControl();
      label5 = new LabelControl();
      chbChangePassword = new CheckEdit();
      label6 = new LabelControl();
      cbRole = new ComboBoxEdit();
      tbCheckPassword = new TextEdit();
      label4 = new LabelControl();
      tbPassword = new TextEdit();
      label3 = new LabelControl();
      tbLogin = new TextEdit();
      label2 = new LabelControl();
      tbFIO = new TextEdit();
      groupBox2 = new GroupControl();
      btEdit = new SimpleButton();
      btDelete = new SimpleButton();
      btAdd = new SimpleButton();
      btSave = new SimpleButton();
      btCancel = new SimpleButton();
      btPermissions = new SimpleButton();
      gridControl1 = new GridControl();
      gridView1 = new GridView();
      colID = new GridColumn();
      colName = new GridColumn();
      colIP = new GridColumn();
      colPosition = new GridColumn();
      colComment = new GridColumn();
      colState = new GridColumn();
      gridColumn1 = new GridColumn();
      groupBox1.BeginInit();
      groupBox1.SuspendLayout();
      tbComment.Properties.BeginInit();
      chlbActions.BeginInit();
      chbChangePassword.Properties.BeginInit();
      cbRole.Properties.BeginInit();
      tbCheckPassword.Properties.BeginInit();
      tbPassword.Properties.BeginInit();
      tbLogin.Properties.BeginInit();
      tbFIO.Properties.BeginInit();
      groupBox2.BeginInit();
      groupBox2.SuspendLayout();
      gridControl1.BeginInit();
      gridView1.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label1.Appearance.DisabledImage");
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      label1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label1.Appearance.FontSizeDelta");
      label1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label1.Appearance.FontStyleDelta");
      label1.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label1.Appearance.ForeColor");
      label1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label1.Appearance.GradientMode");
      label1.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label1.Appearance.HoverImage");
      label1.Appearance.Image = (Image) componentResourceManager.GetObject("label1.Appearance.Image");
      label1.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label1.Appearance.PressedImage");
      label1.Name = "label1";
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("groupBox1.Appearance.FontSizeDelta");
      groupBox1.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupBox1.Appearance.FontStyleDelta");
      groupBox1.Appearance.ForeColor = (Color) componentResourceManager.GetObject("groupBox1.Appearance.ForeColor");
      groupBox1.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupBox1.Appearance.GradientMode");
      groupBox1.Appearance.Image = (Image) componentResourceManager.GetObject("groupBox1.Appearance.Image");
      groupBox1.Appearance.Options.UseForeColor = true;
      groupBox1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox1.AppearanceCaption.Font");
      groupBox1.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupBox1.AppearanceCaption.FontSizeDelta");
      groupBox1.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupBox1.AppearanceCaption.FontStyleDelta");
      groupBox1.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupBox1.AppearanceCaption.GradientMode");
      groupBox1.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupBox1.AppearanceCaption.Image");
      groupBox1.AppearanceCaption.Options.UseFont = true;
      groupBox1.Controls.Add(tbComment);
      groupBox1.Controls.Add(chlbActions);
      groupBox1.Controls.Add(label7);
      groupBox1.Controls.Add(label5);
      groupBox1.Controls.Add(chbChangePassword);
      groupBox1.Controls.Add(label6);
      groupBox1.Controls.Add(cbRole);
      groupBox1.Controls.Add(tbCheckPassword);
      groupBox1.Controls.Add(label4);
      groupBox1.Controls.Add(tbPassword);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(tbLogin);
      groupBox1.Controls.Add(label2);
      groupBox1.Controls.Add(tbFIO);
      groupBox1.Controls.Add(label1);
      groupBox1.Name = "groupBox1";
      componentResourceManager.ApplyResources(tbComment, "tbComment");
      tbComment.Name = "tbComment";
      tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(chlbActions, "chlbActions");
      chlbActions.Appearance.Font = (Font) componentResourceManager.GetObject("chlbActions.Appearance.Font");
      chlbActions.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("chlbActions.Appearance.FontSizeDelta");
      chlbActions.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("chlbActions.Appearance.FontStyleDelta");
      chlbActions.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("chlbActions.Appearance.GradientMode");
      chlbActions.Appearance.Image = (Image) componentResourceManager.GetObject("chlbActions.Appearance.Image");
      chlbActions.Appearance.Options.UseFont = true;
      chlbActions.Items.AddRange(new CheckedListBoxItem[18]
      {
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items"), componentResourceManager.GetString("chlbActions.Items1")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items2"), componentResourceManager.GetString("chlbActions.Items3")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items4"), componentResourceManager.GetString("chlbActions.Items5")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items6"), componentResourceManager.GetString("chlbActions.Items7")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items8"), componentResourceManager.GetString("chlbActions.Items9")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items10"), componentResourceManager.GetString("chlbActions.Items11")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items12"), componentResourceManager.GetString("chlbActions.Items13")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items14"), componentResourceManager.GetString("chlbActions.Items15")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items16"), componentResourceManager.GetString("chlbActions.Items17")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items18"), componentResourceManager.GetString("chlbActions.Items19")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items20"), componentResourceManager.GetString("chlbActions.Items21")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items22"), componentResourceManager.GetString("chlbActions.Items23")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items24"), componentResourceManager.GetString("chlbActions.Items25")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items26"), componentResourceManager.GetString("chlbActions.Items27")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items28"), componentResourceManager.GetString("chlbActions.Items29")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items30"), componentResourceManager.GetString("chlbActions.Items31")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items32"), componentResourceManager.GetString("chlbActions.Items33")),
        new CheckedListBoxItem(componentResourceManager.GetObject("chlbActions.Items34"), componentResourceManager.GetString("chlbActions.Items35"))
      });
      chlbActions.Name = "chlbActions";
      componentResourceManager.ApplyResources(label7, "label7");
      label7.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label7.Appearance.DisabledImage");
      label7.Appearance.Font = (Font) componentResourceManager.GetObject("label7.Appearance.Font");
      label7.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label7.Appearance.FontSizeDelta");
      label7.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label7.Appearance.FontStyleDelta");
      label7.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label7.Appearance.GradientMode");
      label7.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label7.Appearance.HoverImage");
      label7.Appearance.Image = (Image) componentResourceManager.GetObject("label7.Appearance.Image");
      label7.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label7.Appearance.PressedImage");
      label7.Name = "label7";
      componentResourceManager.ApplyResources(label5, "label5");
      label5.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label5.Appearance.DisabledImage");
      label5.Appearance.Font = (Font) componentResourceManager.GetObject("label5.Appearance.Font");
      label5.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label5.Appearance.FontSizeDelta");
      label5.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label5.Appearance.FontStyleDelta");
      label5.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label5.Appearance.ForeColor");
      label5.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label5.Appearance.GradientMode");
      label5.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label5.Appearance.HoverImage");
      label5.Appearance.Image = (Image) componentResourceManager.GetObject("label5.Appearance.Image");
      label5.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label5.Appearance.PressedImage");
      label5.Name = "label5";
      componentResourceManager.ApplyResources(chbChangePassword, "chbChangePassword");
      chbChangePassword.Name = "chbChangePassword";
      chbChangePassword.Properties.AccessibleDescription = componentResourceManager.GetString("chbChangePassword.Properties.AccessibleDescription");
      chbChangePassword.Properties.AccessibleName = componentResourceManager.GetString("chbChangePassword.Properties.AccessibleName");
      chbChangePassword.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbChangePassword.Properties.Appearance.Font");
      chbChangePassword.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("chbChangePassword.Properties.Appearance.FontSizeDelta");
      chbChangePassword.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("chbChangePassword.Properties.Appearance.FontStyleDelta");
      chbChangePassword.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("chbChangePassword.Properties.Appearance.GradientMode");
      chbChangePassword.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("chbChangePassword.Properties.Appearance.Image");
      chbChangePassword.Properties.Appearance.Options.UseFont = true;
      chbChangePassword.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbChangePassword.Properties.AutoHeight");
      chbChangePassword.Properties.Caption = componentResourceManager.GetString("chbChangePassword.Properties.Caption");
      chbChangePassword.Properties.DisplayValueChecked = componentResourceManager.GetString("chbChangePassword.Properties.DisplayValueChecked");
      chbChangePassword.Properties.DisplayValueGrayed = componentResourceManager.GetString("chbChangePassword.Properties.DisplayValueGrayed");
      chbChangePassword.Properties.DisplayValueUnchecked = componentResourceManager.GetString("chbChangePassword.Properties.DisplayValueUnchecked");
      chbChangePassword.CheckedChanged += chbChangePassword_CheckedChanged;
      componentResourceManager.ApplyResources(label6, "label6");
      label6.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label6.Appearance.DisabledImage");
      label6.Appearance.Font = (Font) componentResourceManager.GetObject("label6.Appearance.Font");
      label6.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label6.Appearance.FontSizeDelta");
      label6.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label6.Appearance.FontStyleDelta");
      label6.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label6.Appearance.ForeColor");
      label6.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label6.Appearance.GradientMode");
      label6.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label6.Appearance.HoverImage");
      label6.Appearance.Image = (Image) componentResourceManager.GetObject("label6.Appearance.Image");
      label6.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label6.Appearance.PressedImage");
      label6.Name = "label6";
      componentResourceManager.ApplyResources(cbRole, "cbRole");
      cbRole.Name = "cbRole";
      cbRole.Properties.AccessibleDescription = componentResourceManager.GetString("cbRole.Properties.AccessibleDescription");
      cbRole.Properties.AccessibleName = componentResourceManager.GetString("cbRole.Properties.AccessibleName");
      cbRole.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbRole.Properties.Appearance.Font");
      cbRole.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbRole.Properties.Appearance.FontSizeDelta");
      cbRole.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbRole.Properties.Appearance.FontStyleDelta");
      cbRole.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbRole.Properties.Appearance.GradientMode");
      cbRole.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbRole.Properties.Appearance.Image");
      cbRole.Properties.Appearance.Options.UseFont = true;
      cbRole.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbRole.Properties.AutoHeight");
      cbRole.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbRole.Properties.Buttons"))
      });
      cbRole.Properties.Items.AddRange(new object[3]
      {
        componentResourceManager.GetString("cbRole.Properties.Items"),
        componentResourceManager.GetString("cbRole.Properties.Items1"),
        componentResourceManager.GetString("cbRole.Properties.Items2")
      });
      cbRole.Properties.NullValuePrompt = componentResourceManager.GetString("cbRole.Properties.NullValuePrompt");
      cbRole.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbRole.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(tbCheckPassword, "tbCheckPassword");
      tbCheckPassword.Name = "tbCheckPassword";
      tbCheckPassword.Properties.AccessibleDescription = componentResourceManager.GetString("tbCheckPassword.Properties.AccessibleDescription");
      tbCheckPassword.Properties.AccessibleName = componentResourceManager.GetString("tbCheckPassword.Properties.AccessibleName");
      tbCheckPassword.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbCheckPassword.Properties.Appearance.Font");
      tbCheckPassword.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbCheckPassword.Properties.Appearance.FontSizeDelta");
      tbCheckPassword.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbCheckPassword.Properties.Appearance.FontStyleDelta");
      tbCheckPassword.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbCheckPassword.Properties.Appearance.GradientMode");
      tbCheckPassword.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbCheckPassword.Properties.Appearance.Image");
      tbCheckPassword.Properties.Appearance.Options.UseFont = true;
      tbCheckPassword.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.AutoHeight");
      tbCheckPassword.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.AutoComplete");
      tbCheckPassword.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.BeepOnError");
      tbCheckPassword.Properties.Mask.EditMask = componentResourceManager.GetString("tbCheckPassword.Properties.Mask.EditMask");
      tbCheckPassword.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.IgnoreMaskBlank");
      tbCheckPassword.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.MaskType");
      tbCheckPassword.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.PlaceHolder");
      tbCheckPassword.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.SaveLiteral");
      tbCheckPassword.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.ShowPlaceHolders");
      tbCheckPassword.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.Mask.UseMaskAsDisplayFormat");
      tbCheckPassword.Properties.NullValuePrompt = componentResourceManager.GetString("tbCheckPassword.Properties.NullValuePrompt");
      tbCheckPassword.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbCheckPassword.Properties.NullValuePromptShowForEmptyValue");
      tbCheckPassword.Properties.UseSystemPasswordChar = true;
      componentResourceManager.ApplyResources(label4, "label4");
      label4.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label4.Appearance.DisabledImage");
      label4.Appearance.Font = (Font) componentResourceManager.GetObject("label4.Appearance.Font");
      label4.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label4.Appearance.FontSizeDelta");
      label4.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label4.Appearance.FontStyleDelta");
      label4.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label4.Appearance.ForeColor");
      label4.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label4.Appearance.GradientMode");
      label4.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label4.Appearance.HoverImage");
      label4.Appearance.Image = (Image) componentResourceManager.GetObject("label4.Appearance.Image");
      label4.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label4.Appearance.PressedImage");
      label4.Name = "label4";
      componentResourceManager.ApplyResources(tbPassword, "tbPassword");
      tbPassword.Name = "tbPassword";
      tbPassword.Properties.AccessibleDescription = componentResourceManager.GetString("tbPassword.Properties.AccessibleDescription");
      tbPassword.Properties.AccessibleName = componentResourceManager.GetString("tbPassword.Properties.AccessibleName");
      tbPassword.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbPassword.Properties.Appearance.Font");
      tbPassword.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbPassword.Properties.Appearance.FontSizeDelta");
      tbPassword.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbPassword.Properties.Appearance.FontStyleDelta");
      tbPassword.Properties.Appearance.ForeColor = (Color) componentResourceManager.GetObject("tbPassword.Properties.Appearance.ForeColor");
      tbPassword.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbPassword.Properties.Appearance.GradientMode");
      tbPassword.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbPassword.Properties.Appearance.Image");
      tbPassword.Properties.Appearance.Options.UseFont = true;
      tbPassword.Properties.Appearance.Options.UseForeColor = true;
      tbPassword.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbPassword.Properties.AutoHeight");
      tbPassword.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbPassword.Properties.Mask.AutoComplete");
      tbPassword.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.BeepOnError");
      tbPassword.Properties.Mask.EditMask = componentResourceManager.GetString("tbPassword.Properties.Mask.EditMask");
      tbPassword.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.IgnoreMaskBlank");
      tbPassword.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbPassword.Properties.Mask.MaskType");
      tbPassword.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbPassword.Properties.Mask.PlaceHolder");
      tbPassword.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.SaveLiteral");
      tbPassword.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.ShowPlaceHolders");
      tbPassword.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.UseMaskAsDisplayFormat");
      tbPassword.Properties.NullValuePrompt = componentResourceManager.GetString("tbPassword.Properties.NullValuePrompt");
      tbPassword.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbPassword.Properties.NullValuePromptShowForEmptyValue");
      tbPassword.Properties.UseSystemPasswordChar = true;
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
      componentResourceManager.ApplyResources(tbLogin, "tbLogin");
      tbLogin.Name = "tbLogin";
      tbLogin.Properties.AccessibleDescription = componentResourceManager.GetString("tbLogin.Properties.AccessibleDescription");
      tbLogin.Properties.AccessibleName = componentResourceManager.GetString("tbLogin.Properties.AccessibleName");
      tbLogin.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbLogin.Properties.Appearance.Font");
      tbLogin.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbLogin.Properties.Appearance.FontSizeDelta");
      tbLogin.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbLogin.Properties.Appearance.FontStyleDelta");
      tbLogin.Properties.Appearance.ForeColor = (Color) componentResourceManager.GetObject("tbLogin.Properties.Appearance.ForeColor");
      tbLogin.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbLogin.Properties.Appearance.GradientMode");
      tbLogin.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbLogin.Properties.Appearance.Image");
      tbLogin.Properties.Appearance.Options.UseFont = true;
      tbLogin.Properties.Appearance.Options.UseForeColor = true;
      tbLogin.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbLogin.Properties.AutoHeight");
      tbLogin.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbLogin.Properties.Mask.AutoComplete");
      tbLogin.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.BeepOnError");
      tbLogin.Properties.Mask.EditMask = componentResourceManager.GetString("tbLogin.Properties.Mask.EditMask");
      tbLogin.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.IgnoreMaskBlank");
      tbLogin.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbLogin.Properties.Mask.MaskType");
      tbLogin.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbLogin.Properties.Mask.PlaceHolder");
      tbLogin.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.SaveLiteral");
      tbLogin.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.ShowPlaceHolders");
      tbLogin.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.UseMaskAsDisplayFormat");
      tbLogin.Properties.NullValuePrompt = componentResourceManager.GetString("tbLogin.Properties.NullValuePrompt");
      tbLogin.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbLogin.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(label2, "label2");
      label2.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label2.Appearance.DisabledImage");
      label2.Appearance.Font = (Font) componentResourceManager.GetObject("label2.Appearance.Font");
      label2.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label2.Appearance.FontSizeDelta");
      label2.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label2.Appearance.FontStyleDelta");
      label2.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label2.Appearance.ForeColor");
      label2.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label2.Appearance.GradientMode");
      label2.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label2.Appearance.HoverImage");
      label2.Appearance.Image = (Image) componentResourceManager.GetObject("label2.Appearance.Image");
      label2.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label2.Appearance.PressedImage");
      label2.Name = "label2";
      componentResourceManager.ApplyResources(tbFIO, "tbFIO");
      tbFIO.Name = "tbFIO";
      tbFIO.Properties.AccessibleDescription = componentResourceManager.GetString("tbFIO.Properties.AccessibleDescription");
      tbFIO.Properties.AccessibleName = componentResourceManager.GetString("tbFIO.Properties.AccessibleName");
      tbFIO.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbFIO.Properties.Appearance.Font");
      tbFIO.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbFIO.Properties.Appearance.FontSizeDelta");
      tbFIO.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbFIO.Properties.Appearance.FontStyleDelta");
      tbFIO.Properties.Appearance.ForeColor = (Color) componentResourceManager.GetObject("tbFIO.Properties.Appearance.ForeColor");
      tbFIO.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbFIO.Properties.Appearance.GradientMode");
      tbFIO.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbFIO.Properties.Appearance.Image");
      tbFIO.Properties.Appearance.Options.UseFont = true;
      tbFIO.Properties.Appearance.Options.UseForeColor = true;
      tbFIO.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbFIO.Properties.AutoHeight");
      tbFIO.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbFIO.Properties.Mask.AutoComplete");
      tbFIO.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbFIO.Properties.Mask.BeepOnError");
      tbFIO.Properties.Mask.EditMask = componentResourceManager.GetString("tbFIO.Properties.Mask.EditMask");
      tbFIO.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbFIO.Properties.Mask.IgnoreMaskBlank");
      tbFIO.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbFIO.Properties.Mask.MaskType");
      tbFIO.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbFIO.Properties.Mask.PlaceHolder");
      tbFIO.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbFIO.Properties.Mask.SaveLiteral");
      tbFIO.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbFIO.Properties.Mask.ShowPlaceHolders");
      tbFIO.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbFIO.Properties.Mask.UseMaskAsDisplayFormat");
      tbFIO.Properties.NullValuePrompt = componentResourceManager.GetString("tbFIO.Properties.NullValuePrompt");
      tbFIO.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbFIO.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(groupBox2, "groupBox2");
      groupBox2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox2.AppearanceCaption.Font");
      groupBox2.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupBox2.AppearanceCaption.FontSizeDelta");
      groupBox2.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupBox2.AppearanceCaption.FontStyleDelta");
      groupBox2.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupBox2.AppearanceCaption.GradientMode");
      groupBox2.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupBox2.AppearanceCaption.Image");
      groupBox2.AppearanceCaption.Options.UseFont = true;
      groupBox2.Controls.Add(btEdit);
      groupBox2.Controls.Add(btDelete);
      groupBox2.Controls.Add(btAdd);
      groupBox2.Controls.Add(btSave);
      groupBox2.Controls.Add(btCancel);
      groupBox2.Name = "groupBox2";
      groupBox2.ShowCaption = false;
      componentResourceManager.ApplyResources(btEdit, "btEdit");
      btEdit.Appearance.Font = (Font) componentResourceManager.GetObject("btEdit.Appearance.Font");
      btEdit.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btEdit.Appearance.FontSizeDelta");
      btEdit.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btEdit.Appearance.FontStyleDelta");
      btEdit.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btEdit.Appearance.GradientMode");
      btEdit.Appearance.Image = (Image) componentResourceManager.GetObject("btEdit.Appearance.Image");
      btEdit.Appearance.Options.UseFont = true;
      btEdit.Name = "btEdit";
      btEdit.Click += btEdit_Click;
      componentResourceManager.ApplyResources(btDelete, "btDelete");
      btDelete.Appearance.Font = (Font) componentResourceManager.GetObject("btDelete.Appearance.Font");
      btDelete.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btDelete.Appearance.FontSizeDelta");
      btDelete.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btDelete.Appearance.FontStyleDelta");
      btDelete.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btDelete.Appearance.GradientMode");
      btDelete.Appearance.Image = (Image) componentResourceManager.GetObject("btDelete.Appearance.Image");
      btDelete.Appearance.Options.UseFont = true;
      btDelete.Name = "btDelete";
      btDelete.Click += btDelete_Click;
      componentResourceManager.ApplyResources(btAdd, "btAdd");
      btAdd.Appearance.Font = (Font) componentResourceManager.GetObject("btAdd.Appearance.Font");
      btAdd.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAdd.Appearance.FontSizeDelta");
      btAdd.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAdd.Appearance.FontStyleDelta");
      btAdd.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAdd.Appearance.GradientMode");
      btAdd.Appearance.Image = (Image) componentResourceManager.GetObject("btAdd.Appearance.Image");
      btAdd.Appearance.Options.UseFont = true;
      btAdd.Name = "btAdd";
      btAdd.Click += btAdd_Click;
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.Appearance.Font = (Font) componentResourceManager.GetObject("btSave.Appearance.Font");
      btSave.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btSave.Appearance.FontSizeDelta");
      btSave.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btSave.Appearance.FontStyleDelta");
      btSave.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btSave.Appearance.GradientMode");
      btSave.Appearance.Image = (Image) componentResourceManager.GetObject("btSave.Appearance.Image");
      btSave.Appearance.Options.UseFont = true;
      btSave.Name = "btSave";
      btSave.Click += btSave_Click;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      btCancel.Appearance.Options.UseFont = true;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(btPermissions, "btPermissions");
      btPermissions.Appearance.Font = (Font) componentResourceManager.GetObject("btPermissions.Appearance.Font");
      btPermissions.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPermissions.Appearance.FontSizeDelta");
      btPermissions.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPermissions.Appearance.FontStyleDelta");
      btPermissions.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPermissions.Appearance.GradientMode");
      btPermissions.Appearance.Image = (Image) componentResourceManager.GetObject("btPermissions.Appearance.Image");
      btPermissions.Appearance.Options.UseFont = true;
      btPermissions.Name = "btPermissions";
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
      componentResourceManager.ApplyResources(gridView1, "gridView1");
      gridView1.Columns.AddRange(new GridColumn[7]
      {
        colID,
        colName,
        colIP,
        colPosition,
        colComment,
        colState,
        gridColumn1
      });
      gridView1.GridControl = gridControl1;
      gridView1.Name = "gridView1";
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
      colID.OptionsColumn.AllowEdit = false;
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
      colIP.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceCell.Font");
      colIP.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colIP.AppearanceCell.FontSizeDelta");
      colIP.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIP.AppearanceCell.FontStyleDelta");
      colIP.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIP.AppearanceCell.GradientMode");
      colIP.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colIP.AppearanceCell.Image");
      colIP.AppearanceCell.Options.UseFont = true;
      colIP.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colIP.AppearanceHeader.Font");
      colIP.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colIP.AppearanceHeader.FontSizeDelta");
      colIP.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colIP.AppearanceHeader.FontStyleDelta");
      colIP.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colIP.AppearanceHeader.GradientMode");
      colIP.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colIP.AppearanceHeader.Image");
      colIP.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colIP, "colIP");
      colIP.FieldName = "Login";
      colIP.Name = "colIP";
      colIP.OptionsColumn.AllowEdit = false;
      colIP.OptionsColumn.ReadOnly = true;
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
      colPosition.FieldName = "Password";
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
      colComment.FieldName = "Comment";
      colComment.Name = "colComment";
      colComment.OptionsColumn.AllowEdit = false;
      colState.AppearanceCell.Font = (Font) componentResourceManager.GetObject("colState.AppearanceCell.Font");
      colState.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("colState.AppearanceCell.FontSizeDelta");
      colState.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colState.AppearanceCell.FontStyleDelta");
      colState.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colState.AppearanceCell.GradientMode");
      colState.AppearanceCell.Image = (Image) componentResourceManager.GetObject("colState.AppearanceCell.Image");
      colState.AppearanceCell.Options.UseFont = true;
      colState.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("colState.AppearanceHeader.Font");
      colState.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("colState.AppearanceHeader.FontSizeDelta");
      colState.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("colState.AppearanceHeader.FontStyleDelta");
      colState.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("colState.AppearanceHeader.GradientMode");
      colState.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("colState.AppearanceHeader.Image");
      colState.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(colState, "colState");
      colState.FieldName = "Role";
      colState.Name = "colState";
      colState.OptionsColumn.AllowEdit = false;
      gridColumn1.AppearanceCell.Font = (Font) componentResourceManager.GetObject("gridColumn1.AppearanceCell.Font");
      gridColumn1.AppearanceCell.FontSizeDelta = (int) componentResourceManager.GetObject("gridColumn1.AppearanceCell.FontSizeDelta");
      gridColumn1.AppearanceCell.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridColumn1.AppearanceCell.FontStyleDelta");
      gridColumn1.AppearanceCell.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridColumn1.AppearanceCell.GradientMode");
      gridColumn1.AppearanceCell.Image = (Image) componentResourceManager.GetObject("gridColumn1.AppearanceCell.Image");
      gridColumn1.AppearanceCell.Options.UseFont = true;
      gridColumn1.AppearanceHeader.Font = (Font) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.Font");
      gridColumn1.AppearanceHeader.FontSizeDelta = (int) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.FontSizeDelta");
      gridColumn1.AppearanceHeader.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.FontStyleDelta");
      gridColumn1.AppearanceHeader.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.GradientMode");
      gridColumn1.AppearanceHeader.Image = (Image) componentResourceManager.GetObject("gridColumn1.AppearanceHeader.Image");
      gridColumn1.AppearanceHeader.Options.UseFont = true;
      componentResourceManager.ApplyResources(gridColumn1, "gridColumn1");
      gridColumn1.FieldName = "Actions";
      gridColumn1.Name = "gridColumn1";
      componentResourceManager.ApplyResources(this, "$this");
      Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("frmUsers.Appearance.FontSizeDelta");
      Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("frmUsers.Appearance.FontStyleDelta");
      Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("frmUsers.Appearance.GradientMode");
      Appearance.Image = (Image) componentResourceManager.GetObject("frmUsers.Appearance.Image");
      Appearance.Options.UseFont = true;
      ControlBox = false;
      Controls.Add(btPermissions);
      Controls.Add(gridControl1);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      FormBorderStyle = FormBorderStyle.None;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmUsers";
      WindowState = FormWindowState.Maximized;
      HelpButtonClicked += UsersForm_HelpButtonClicked;
      Load += UsersForm_Load;
      HelpRequested += UsersForm_HelpRequested;
      Resize += UsersForm_Resize;
      groupBox1.EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      tbComment.Properties.EndInit();
      chlbActions.EndInit();
      chbChangePassword.Properties.EndInit();
      cbRole.Properties.EndInit();
      tbCheckPassword.Properties.EndInit();
      tbPassword.Properties.EndInit();
      tbLogin.Properties.EndInit();
      tbFIO.Properties.EndInit();
      groupBox2.EndInit();
      groupBox2.ResumeLayout(false);
      gridControl1.EndInit();
      gridView1.EndInit();
      ResumeLayout(false);
    }
  }
}
