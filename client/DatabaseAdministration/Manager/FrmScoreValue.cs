// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmScoreValue
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;

namespace CascadeManager
{
  public class FrmScoreValue : XtraForm
  {
    public string ScoreVal = "0";
    private IContainer components = null;
    private SimpleButton btAccept;
    private SimpleButton btCancel;
    private TextEdit textEdit1;

    public FrmScoreValue()
    {
      InitializeComponent();
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void btAccept_Click(object sender, EventArgs e)
    {
      ScoreVal = textEdit1.Text.Replace(".", ",");
    }

    private void textEdit1_Validating(object sender, CancelEventArgs e)
    {
      textEdit1.Text = textEdit1.Text.Replace(".", ",");
      float result;
      if (float.TryParse(textEdit1.Text, out result))
        return;
      int num = (int) XtraMessageBox.Show(Messages.IncorrectInputFormat, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmScoreValue));
      btAccept = new SimpleButton();
      btCancel = new SimpleButton();
      textEdit1 = new TextEdit();
      textEdit1.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(btAccept, "btAccept");
      btAccept.DialogResult = DialogResult.OK;
      btAccept.Name = "btAccept";
      btAccept.Click += btAccept_Click;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.DialogResult = DialogResult.Cancel;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      componentResourceManager.ApplyResources(textEdit1, "textEdit1");
      textEdit1.Name = "textEdit1";
      textEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("textEdit1.Properties.AccessibleDescription");
      textEdit1.Properties.AccessibleName = componentResourceManager.GetString("textEdit1.Properties.AccessibleName");
      textEdit1.Properties.AutoHeight = (bool) componentResourceManager.GetObject("textEdit1.Properties.AutoHeight");
      textEdit1.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("textEdit1.Properties.Mask.AutoComplete");
      textEdit1.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("textEdit1.Properties.Mask.BeepOnError");
      textEdit1.Properties.Mask.EditMask = componentResourceManager.GetString("textEdit1.Properties.Mask.EditMask");
      textEdit1.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("textEdit1.Properties.Mask.IgnoreMaskBlank");
      textEdit1.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("textEdit1.Properties.Mask.MaskType");
      textEdit1.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("textEdit1.Properties.Mask.PlaceHolder");
      textEdit1.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("textEdit1.Properties.Mask.SaveLiteral");
      textEdit1.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("textEdit1.Properties.Mask.ShowPlaceHolders");
      textEdit1.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("textEdit1.Properties.Mask.UseMaskAsDisplayFormat");
      textEdit1.Properties.NullValuePrompt = componentResourceManager.GetString("textEdit1.Properties.NullValuePrompt");
      textEdit1.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("textEdit1.Properties.NullValuePromptShowForEmptyValue");
      textEdit1.Validating += textEdit1_Validating;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(textEdit1);
      Controls.Add(btCancel);
      Controls.Add(btAccept);
      Name = "FrmScoreValue";
      ShowIcon = false;
      textEdit1.Properties.EndInit();
      ResumeLayout(false);
    }
  }
}
