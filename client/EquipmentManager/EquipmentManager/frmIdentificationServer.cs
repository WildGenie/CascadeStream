// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmIdentificationServer
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class FrmIdentificationServer : XtraForm
  {
    private IContainer components = (IContainer) null;
    private LabelControl label3;
    private TextEdit tbIP;
    private CheckEdit chbUsePBDCheck;
    private CheckEdit chbUseOwnScore;
    private SpinEdit tbThreadCount;
    private TextEdit tbPort;
    private LabelControl labelControl1;
    private LabelControl labelControl2;
    private CheckEdit chbUseFBDCheck;
    private CheckEdit chbUseErrorCheck;
    private CheckEdit chbSaveNewKeys;
    private LabelControl labelControl3;
    private SpinEdit tbMinus;
    private LabelControl labelControl4;
    private SpinEdit tbTreckingPeriod;
    private GroupControl groupControl2;
    private SimpleButton btCancel;
    private SimpleButton btAccept;
    private GroupControl groupControl1;
    private LabelControl labelControl9;
    private TextEdit tbQueueName;
    private LabelControl labelControl8;
    private TextEdit tbVirtualHost;
    private LabelControl labelControl7;
    private TextEdit tbPassword;
    private LabelControl labelControl6;
    private TextEdit tbLogin;
    private LabelControl labelControl5;
    private TextEdit tbHostName;

    public FrmIdentificationServer()
    {
      this.InitializeComponent();
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      new BcServerSettings()
      {
        Ip = this.tbIP.Text,
        Port = Convert.ToInt32(this.tbPort.Text),
        Minus = Convert.ToInt32(this.tbMinus.Text),
        SaveNewKeys = this.chbSaveNewKeys.Checked,
        ThreadCount = (int) this.tbThreadCount.Value,
        TrackingPeriod = (int) this.tbTreckingPeriod.Value,
        UseErrorCheck = this.chbUseErrorCheck.Checked,
        UseFbdCheck = this.chbUseFBDCheck.Checked,
        UsePbdCheck = this.chbUsePBDCheck.Checked,
        UseOwnScore = this.chbUseOwnScore.Checked,
        HostName = this.tbHostName.Text,
        UserName = this.tbLogin.Text,
        Password = this.tbPassword.Text,
        VirtualHost = this.tbVirtualHost.Text,
        QueueName = this.tbQueueName.Text
      }.Save();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void tbLogin_Validating(object sender, CancelEventArgs e)
    {
      TextEdit textEdit = sender as TextEdit;
      try
      {
        Convert.ToInt32(textEdit.Text);
      }
      catch
      {
        int num = (int) XtraMessageBox.Show("Введите целочисленное значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
      }
    }

    private void frmIdentificationServer_Load(object sender, EventArgs e)
    {
      BcServerSettings bcServerSettings = BcServerSettings.Load();
      this.tbIP.Text = bcServerSettings.Ip;
      this.tbPort.Text = bcServerSettings.Port.ToString();
      this.tbMinus.Text = bcServerSettings.Minus.ToString();
      this.chbSaveNewKeys.Checked = bcServerSettings.SaveNewKeys;
      this.tbThreadCount.Value = (Decimal) bcServerSettings.ThreadCount;
      this.tbTreckingPeriod.Value = (Decimal) bcServerSettings.TrackingPeriod;
      this.chbUseErrorCheck.Checked = bcServerSettings.UseErrorCheck;
      this.chbUseFBDCheck.Checked = bcServerSettings.UseFbdCheck;
      this.chbUsePBDCheck.Checked = bcServerSettings.UsePbdCheck;
      this.chbUseOwnScore.Checked = bcServerSettings.UseOwnScore;
      this.tbHostName.Text = bcServerSettings.HostName;
      this.tbLogin.Text = bcServerSettings.UserName;
      this.tbPassword.Text = bcServerSettings.Password;
      this.tbVirtualHost.Text = bcServerSettings.VirtualHost;
      this.tbQueueName.Text = bcServerSettings.QueueName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmIdentificationServer));
      this.label3 = new LabelControl();
      this.tbIP = new TextEdit();
      this.chbUsePBDCheck = new CheckEdit();
      this.chbUseOwnScore = new CheckEdit();
      this.tbThreadCount = new SpinEdit();
      this.tbPort = new TextEdit();
      this.labelControl1 = new LabelControl();
      this.labelControl2 = new LabelControl();
      this.chbUseFBDCheck = new CheckEdit();
      this.chbUseErrorCheck = new CheckEdit();
      this.chbSaveNewKeys = new CheckEdit();
      this.labelControl3 = new LabelControl();
      this.tbMinus = new SpinEdit();
      this.labelControl4 = new LabelControl();
      this.tbTreckingPeriod = new SpinEdit();
      this.groupControl2 = new GroupControl();
      this.groupControl1 = new GroupControl();
      this.labelControl9 = new LabelControl();
      this.tbQueueName = new TextEdit();
      this.labelControl8 = new LabelControl();
      this.tbVirtualHost = new TextEdit();
      this.labelControl7 = new LabelControl();
      this.tbPassword = new TextEdit();
      this.labelControl6 = new LabelControl();
      this.tbLogin = new TextEdit();
      this.labelControl5 = new LabelControl();
      this.tbHostName = new TextEdit();
      this.btCancel = new SimpleButton();
      this.btAccept = new SimpleButton();
      this.tbIP.Properties.BeginInit();
      this.chbUsePBDCheck.Properties.BeginInit();
      this.chbUseOwnScore.Properties.BeginInit();
      this.tbThreadCount.Properties.BeginInit();
      this.tbPort.Properties.BeginInit();
      this.chbUseFBDCheck.Properties.BeginInit();
      this.chbUseErrorCheck.Properties.BeginInit();
      this.chbSaveNewKeys.Properties.BeginInit();
      this.tbMinus.Properties.BeginInit();
      this.tbTreckingPeriod.Properties.BeginInit();
      this.groupControl2.BeginInit();
      this.groupControl2.SuspendLayout();
      this.groupControl1.BeginInit();
      this.groupControl1.SuspendLayout();
      this.tbQueueName.Properties.BeginInit();
      this.tbVirtualHost.Properties.BeginInit();
      this.tbPassword.Properties.BeginInit();
      this.tbLogin.Properties.BeginInit();
      this.tbHostName.Properties.BeginInit();
      this.SuspendLayout();
      this.label3.Appearance.Font = (Font) componentResourceManager.GetObject("label3.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.tbIP, "tbIP");
      this.tbIP.Name = "tbIP";
      this.tbIP.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbIP.Properties.Appearance.Font");
      this.tbIP.Properties.Appearance.Options.UseFont = true;
      this.tbIP.Properties.Mask.EditMask = componentResourceManager.GetString("tbIP.Properties.Mask.EditMask");
      this.tbIP.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbIP.Properties.Mask.MaskType");
      componentResourceManager.ApplyResources((object) this.chbUsePBDCheck, "chbUsePBDCheck");
      this.chbUsePBDCheck.Name = "chbUsePBDCheck";
      this.chbUsePBDCheck.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbUsePBDCheck.Properties.Appearance.Font");
      this.chbUsePBDCheck.Properties.Appearance.Options.UseFont = true;
      this.chbUsePBDCheck.Properties.Caption = componentResourceManager.GetString("chbUsePBDCheck.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.chbUseOwnScore, "chbUseOwnScore");
      this.chbUseOwnScore.Name = "chbUseOwnScore";
      this.chbUseOwnScore.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbUseOwnScore.Properties.Appearance.Font");
      this.chbUseOwnScore.Properties.Appearance.Options.UseFont = true;
      this.chbUseOwnScore.Properties.Caption = componentResourceManager.GetString("chbUseOwnScore.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.tbThreadCount, "tbThreadCount");
      this.tbThreadCount.Name = "tbThreadCount";
      this.tbThreadCount.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbThreadCount.Properties.Appearance.Font");
      this.tbThreadCount.Properties.Appearance.Options.UseFont = true;
      this.tbThreadCount.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      componentResourceManager.ApplyResources((object) this.tbPort, "tbPort");
      this.tbPort.Name = "tbPort";
      this.tbPort.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbPort.Properties.Appearance.Font");
      this.tbPort.Properties.Appearance.Options.UseFont = true;
      this.tbPort.Validating += new CancelEventHandler(this.tbLogin_Validating);
      this.labelControl1.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl1.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl1, "labelControl1");
      this.labelControl1.Name = "labelControl1";
      this.labelControl2.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl2.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl2, "labelControl2");
      this.labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources((object) this.chbUseFBDCheck, "chbUseFBDCheck");
      this.chbUseFBDCheck.Name = "chbUseFBDCheck";
      this.chbUseFBDCheck.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbUseFBDCheck.Properties.Appearance.Font");
      this.chbUseFBDCheck.Properties.Appearance.Options.UseFont = true;
      this.chbUseFBDCheck.Properties.Caption = componentResourceManager.GetString("chbUseFBDCheck.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.chbUseErrorCheck, "chbUseErrorCheck");
      this.chbUseErrorCheck.Name = "chbUseErrorCheck";
      this.chbUseErrorCheck.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbUseErrorCheck.Properties.Appearance.Font");
      this.chbUseErrorCheck.Properties.Appearance.Options.UseFont = true;
      this.chbUseErrorCheck.Properties.Caption = componentResourceManager.GetString("chbUseErrorCheck.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.chbSaveNewKeys, "chbSaveNewKeys");
      this.chbSaveNewKeys.Name = "chbSaveNewKeys";
      this.chbSaveNewKeys.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("chbSaveNewKeys.Properties.Appearance.Font");
      this.chbSaveNewKeys.Properties.Appearance.Options.UseFont = true;
      this.chbSaveNewKeys.Properties.Caption = componentResourceManager.GetString("chbSaveNewKeys.Properties.Caption");
      this.labelControl3.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl3.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl3, "labelControl3");
      this.labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources((object) this.tbMinus, "tbMinus");
      this.tbMinus.Name = "tbMinus";
      this.tbMinus.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbMinus.Properties.Appearance.Font");
      this.tbMinus.Properties.Appearance.Options.UseFont = true;
      this.tbMinus.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.labelControl4.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl4.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl4, "labelControl4");
      this.labelControl4.Name = "labelControl4";
      componentResourceManager.ApplyResources((object) this.tbTreckingPeriod, "tbTreckingPeriod");
      this.tbTreckingPeriod.Name = "tbTreckingPeriod";
      this.tbTreckingPeriod.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbTreckingPeriod.Properties.Appearance.Font");
      this.tbTreckingPeriod.Properties.Appearance.Options.UseFont = true;
      this.tbTreckingPeriod.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.groupControl2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Font");
      this.groupControl2.AppearanceCaption.Options.UseFont = true;
      this.groupControl2.Controls.Add((Control) this.label3);
      this.groupControl2.Controls.Add((Control) this.labelControl4);
      this.groupControl2.Controls.Add((Control) this.tbThreadCount);
      this.groupControl2.Controls.Add((Control) this.tbTreckingPeriod);
      this.groupControl2.Controls.Add((Control) this.chbUseOwnScore);
      this.groupControl2.Controls.Add((Control) this.labelControl3);
      this.groupControl2.Controls.Add((Control) this.chbUsePBDCheck);
      this.groupControl2.Controls.Add((Control) this.tbMinus);
      this.groupControl2.Controls.Add((Control) this.tbIP);
      this.groupControl2.Controls.Add((Control) this.tbPort);
      this.groupControl2.Controls.Add((Control) this.chbSaveNewKeys);
      this.groupControl2.Controls.Add((Control) this.labelControl1);
      this.groupControl2.Controls.Add((Control) this.chbUseErrorCheck);
      this.groupControl2.Controls.Add((Control) this.labelControl2);
      this.groupControl2.Controls.Add((Control) this.chbUseFBDCheck);
      this.groupControl2.Controls.Add((Control) this.groupControl1);
      componentResourceManager.ApplyResources((object) this.groupControl2, "groupControl2");
      this.groupControl2.Name = "groupControl2";
      this.groupControl2.ShowCaption = false;
      this.groupControl1.Controls.Add((Control) this.labelControl9);
      this.groupControl1.Controls.Add((Control) this.tbQueueName);
      this.groupControl1.Controls.Add((Control) this.labelControl8);
      this.groupControl1.Controls.Add((Control) this.tbVirtualHost);
      this.groupControl1.Controls.Add((Control) this.labelControl7);
      this.groupControl1.Controls.Add((Control) this.tbPassword);
      this.groupControl1.Controls.Add((Control) this.labelControl6);
      this.groupControl1.Controls.Add((Control) this.tbLogin);
      this.groupControl1.Controls.Add((Control) this.labelControl5);
      this.groupControl1.Controls.Add((Control) this.tbHostName);
      componentResourceManager.ApplyResources((object) this.groupControl1, "groupControl1");
      this.groupControl1.Name = "groupControl1";
      this.labelControl9.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl9.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl9, "labelControl9");
      this.labelControl9.Name = "labelControl9";
      componentResourceManager.ApplyResources((object) this.tbQueueName, "tbQueueName");
      this.tbQueueName.Name = "tbQueueName";
      this.labelControl8.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl8.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl8, "labelControl8");
      this.labelControl8.Name = "labelControl8";
      componentResourceManager.ApplyResources((object) this.tbVirtualHost, "tbVirtualHost");
      this.tbVirtualHost.Name = "tbVirtualHost";
      this.labelControl7.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl7.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl7, "labelControl7");
      this.labelControl7.Name = "labelControl7";
      componentResourceManager.ApplyResources((object) this.tbPassword, "tbPassword");
      this.tbPassword.Name = "tbPassword";
      this.tbPassword.Properties.UseSystemPasswordChar = true;
      this.labelControl6.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl6.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl6, "labelControl6");
      this.labelControl6.Name = "labelControl6";
      componentResourceManager.ApplyResources((object) this.tbLogin, "tbLogin");
      this.tbLogin.Name = "tbLogin";
      this.labelControl5.Appearance.Font = (Font) componentResourceManager.GetObject("labelControl5.Appearance.Font");
      componentResourceManager.ApplyResources((object) this.labelControl5, "labelControl5");
      this.labelControl5.Name = "labelControl5";
      componentResourceManager.ApplyResources((object) this.tbHostName, "tbHostName");
      this.tbHostName.Name = "tbHostName";
      this.btCancel.Appearance.Font = (Font) componentResourceManager.GetObject("btCancel.Appearance.Font");
      this.btCancel.Appearance.Options.UseFont = true;
      this.btCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.btAccept.Appearance.Font = (Font) componentResourceManager.GetObject("btAccept.Appearance.Font");
      this.btAccept.Appearance.Options.UseFont = true;
      this.btAccept.DialogResult = DialogResult.OK;
      componentResourceManager.ApplyResources((object) this.btAccept, "btAccept");
      this.btAccept.Name = "btAccept";
      this.btAccept.Click += new EventHandler(this.btAccept_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btAccept);
      this.Controls.Add((Control) this.groupControl2);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmIdentificationServer";
      this.Load += new EventHandler(this.frmIdentificationServer_Load);
      this.tbIP.Properties.EndInit();
      this.chbUsePBDCheck.Properties.EndInit();
      this.chbUseOwnScore.Properties.EndInit();
      this.tbThreadCount.Properties.EndInit();
      this.tbPort.Properties.EndInit();
      this.chbUseFBDCheck.Properties.EndInit();
      this.chbUseErrorCheck.Properties.EndInit();
      this.chbSaveNewKeys.Properties.EndInit();
      this.tbMinus.Properties.EndInit();
      this.tbTreckingPeriod.Properties.EndInit();
      this.groupControl2.EndInit();
      this.groupControl2.ResumeLayout(false);
      this.groupControl2.PerformLayout();
      this.groupControl1.EndInit();
      this.groupControl1.ResumeLayout(false);
      this.groupControl1.PerformLayout();
      this.tbQueueName.Properties.EndInit();
      this.tbVirtualHost.Properties.EndInit();
      this.tbPassword.Properties.EndInit();
      this.tbLogin.Properties.EndInit();
      this.tbHostName.Properties.EndInit();
      this.ResumeLayout(false);
    }
  }
}
