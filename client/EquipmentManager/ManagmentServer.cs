// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.ManagmentServer
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class ManagmentServer : XtraForm
  {
    private IContainer components = (IContainer) null;
    private SimpleButton btAccept;
    private SimpleButton btCancel;
    private TextEdit tbIP;
    private TextEdit tbPort;
    private LabelControl labelControl4;
    private LabelControl labelControl3;
    private GroupControl groupControl2;

    public ManagmentServer()
    {
      this.InitializeComponent();
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      new BcManagmentServer()
      {
        Ip = this.tbIP.Text,
        Port = Convert.ToInt32(this.tbPort.Text)
      }.Save();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void frmIdentificationServer_Load(object sender, EventArgs e)
    {
      BcManagmentServer bcManagmentServer = BcManagmentServer.Load();
      this.tbIP.Text = bcManagmentServer.Ip;
      this.tbPort.Text = bcManagmentServer.Port.ToString();
    }

    private void tbISPort_Validating(object sender, CancelEventArgs e)
    {
      TextEdit textEdit = sender as TextEdit;
      try
      {
        Convert.ToInt32(textEdit.Text);
      }
      catch
      {
        int num = (int) XtraMessageBox.Show(Messages.IncorrectInputFormat, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManagmentServer));
      this.btAccept = new SimpleButton();
      this.btCancel = new SimpleButton();
      this.tbIP = new TextEdit();
      this.tbPort = new TextEdit();
      this.labelControl4 = new LabelControl();
      this.labelControl3 = new LabelControl();
      this.groupControl2 = new GroupControl();
      this.tbIP.Properties.BeginInit();
      this.tbPort.Properties.BeginInit();
      this.groupControl2.BeginInit();
      this.groupControl2.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.btAccept, "btAccept");
      this.btAccept.Appearance.Font = (Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      this.btAccept.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btAccept.Appearance.FontSizeDelta");
      this.btAccept.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btAccept.Appearance.FontStyleDelta");
      this.btAccept.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btAccept.Appearance.GradientMode");
      this.btAccept.Appearance.Image = (Image) componentResourceManager.GetObject("btAccept.Appearance.Image");
      this.btAccept.Appearance.Options.UseFont = true;
      this.btAccept.DialogResult = DialogResult.OK;
      this.btAccept.Name = "btAccept";
      this.btAccept.Click += new EventHandler(this.btAccept_Click);
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      this.btCancel.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btCancel.Appearance.FontSizeDelta");
      this.btCancel.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btCancel.Appearance.FontStyleDelta");
      this.btCancel.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btCancel.Appearance.GradientMode");
      this.btCancel.Appearance.Image = (Image) componentResourceManager.GetObject("btCancel.Appearance.Image");
      this.btCancel.Appearance.Options.UseFont = true;
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      componentResourceManager.ApplyResources((object) this.tbIP, "tbIP");
      this.tbIP.Name = "tbIP";
      this.tbIP.Properties.AccessibleDescription = componentResourceManager.GetString("tbIP.Properties.AccessibleDescription");
      this.tbIP.Properties.AccessibleName = componentResourceManager.GetString("tbIP.Properties.AccessibleName");
      this.tbIP.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbIP.Properties.Appearance.Font");
      this.tbIP.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbIP.Properties.Appearance.FontSizeDelta");
      this.tbIP.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbIP.Properties.Appearance.FontStyleDelta");
      this.tbIP.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbIP.Properties.Appearance.GradientMode");
      this.tbIP.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbIP.Properties.Appearance.Image");
      this.tbIP.Properties.Appearance.Options.UseFont = true;
      this.tbIP.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbIP.Properties.AutoHeight");
      this.tbIP.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbIP.Properties.Mask.AutoComplete");
      this.tbIP.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbIP.Properties.Mask.BeepOnError");
      this.tbIP.Properties.Mask.EditMask = componentResourceManager.GetString("tbIP.Properties.Mask.EditMask");
      this.tbIP.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbIP.Properties.Mask.IgnoreMaskBlank");
      this.tbIP.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbIP.Properties.Mask.MaskType");
      this.tbIP.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbIP.Properties.Mask.PlaceHolder");
      this.tbIP.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbIP.Properties.Mask.SaveLiteral");
      this.tbIP.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbIP.Properties.Mask.ShowPlaceHolders");
      this.tbIP.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbIP.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbIP.Properties.NullValuePrompt = componentResourceManager.GetString("tbIP.Properties.NullValuePrompt");
      this.tbIP.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbIP.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources((object) this.tbPort, "tbPort");
      this.tbPort.Name = "tbPort";
      this.tbPort.Properties.AccessibleDescription = componentResourceManager.GetString("tbPort.Properties.AccessibleDescription");
      this.tbPort.Properties.AccessibleName = componentResourceManager.GetString("tbPort.Properties.AccessibleName");
      this.tbPort.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbPort.Properties.Appearance.Font");
      this.tbPort.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbPort.Properties.Appearance.FontSizeDelta");
      this.tbPort.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbPort.Properties.Appearance.FontStyleDelta");
      this.tbPort.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbPort.Properties.Appearance.GradientMode");
      this.tbPort.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbPort.Properties.Appearance.Image");
      this.tbPort.Properties.Appearance.Options.UseFont = true;
      this.tbPort.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbPort.Properties.AutoHeight");
      this.tbPort.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbPort.Properties.Mask.AutoComplete");
      this.tbPort.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbPort.Properties.Mask.BeepOnError");
      this.tbPort.Properties.Mask.EditMask = componentResourceManager.GetString("tbPort.Properties.Mask.EditMask");
      this.tbPort.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbPort.Properties.Mask.IgnoreMaskBlank");
      this.tbPort.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbPort.Properties.Mask.MaskType");
      this.tbPort.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbPort.Properties.Mask.PlaceHolder");
      this.tbPort.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbPort.Properties.Mask.SaveLiteral");
      this.tbPort.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbPort.Properties.Mask.ShowPlaceHolders");
      this.tbPort.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbPort.Properties.Mask.UseMaskAsDisplayFormat");
      this.tbPort.Properties.NullValuePrompt = componentResourceManager.GetString("tbPort.Properties.NullValuePrompt");
      this.tbPort.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbPort.Properties.NullValuePromptShowForEmptyValue");
      this.tbPort.Validating += new CancelEventHandler(this.tbISPort_Validating);
      componentResourceManager.ApplyResources((object) this.labelControl4, "labelControl4");
      this.labelControl4.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl4.Appearance.DisabledImage");
      this.labelControl4.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl4.Appearance.Font");
      this.labelControl4.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl4.Appearance.FontSizeDelta");
      this.labelControl4.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl4.Appearance.FontStyleDelta");
      this.labelControl4.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl4.Appearance.GradientMode");
      this.labelControl4.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl4.Appearance.HoverImage");
      this.labelControl4.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl4.Appearance.Image");
      this.labelControl4.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl4.Appearance.PressedImage");
      this.labelControl4.Name = "labelControl4";
      componentResourceManager.ApplyResources((object) this.labelControl3, "labelControl3");
      this.labelControl3.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.DisabledImage");
      this.labelControl3.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl3.Appearance.Font");
      this.labelControl3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("labelControl3.Appearance.FontSizeDelta");
      this.labelControl3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("labelControl3.Appearance.FontStyleDelta");
      this.labelControl3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("labelControl3.Appearance.GradientMode");
      this.labelControl3.Appearance.HoverImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.HoverImage");
      this.labelControl3.Appearance.Image = (Image) componentResourceManager.GetObject("labelControl3.Appearance.Image");
      this.labelControl3.Appearance.PressedImage = (Image) componentResourceManager.GetObject("labelControl3.Appearance.PressedImage");
      this.labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources((object) this.groupControl2, "groupControl2");
      this.groupControl2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Font");
      this.groupControl2.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontSizeDelta");
      this.groupControl2.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontStyleDelta");
      this.groupControl2.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl2.AppearanceCaption.GradientMode");
      this.groupControl2.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Image");
      this.groupControl2.AppearanceCaption.Options.UseFont = true;
      this.groupControl2.Controls.Add((Control) this.labelControl3);
      this.groupControl2.Controls.Add((Control) this.labelControl4);
      this.groupControl2.Controls.Add((Control) this.tbPort);
      this.groupControl2.Controls.Add((Control) this.tbIP);
      this.groupControl2.Name = "groupControl2";
      this.groupControl2.ShowCaption = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btAccept);
      this.Controls.Add((Control) this.groupControl2);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManagmentServer";
      this.Load += new EventHandler(this.frmIdentificationServer_Load);
      this.tbIP.Properties.EndInit();
      this.tbPort.Properties.EndInit();
      this.groupControl2.EndInit();
      this.groupControl2.ResumeLayout(false);
      this.groupControl2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
