// Decompiled with JetBrains decompiler
// Type: CascadeManager.SetName
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;

namespace CascadeManager
{
  public class SetName : XtraForm
  {
    private IContainer components = null;
    public string ReturnName;
    private LabelControl label1;
    private SimpleButton btSave;
    private SimpleButton btCancel;
    private TextEdit textBox1;

    public SetName()
    {
      InitializeComponent();
    }

    public SetName(string textForm)
    {
      InitializeComponent();
      Text = textForm;
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      ReturnName = textBox1.Text;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void SetName_Load(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SetName));
      textBox1 = new TextEdit();
      label1 = new LabelControl();
      btSave = new SimpleButton();
      btCancel = new SimpleButton();
      textBox1.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(textBox1, "textBox1");
      textBox1.Name = "textBox1";
      textBox1.Properties.AccessibleDescription = componentResourceManager.GetString("textBox1.Properties.AccessibleDescription");
      textBox1.Properties.AccessibleName = componentResourceManager.GetString("textBox1.Properties.AccessibleName");
      textBox1.Properties.AutoHeight = (bool) componentResourceManager.GetObject("textBox1.Properties.AutoHeight");
      textBox1.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("textBox1.Properties.Mask.AutoComplete");
      textBox1.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("textBox1.Properties.Mask.BeepOnError");
      textBox1.Properties.Mask.EditMask = componentResourceManager.GetString("textBox1.Properties.Mask.EditMask");
      textBox1.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("textBox1.Properties.Mask.IgnoreMaskBlank");
      textBox1.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("textBox1.Properties.Mask.MaskType");
      textBox1.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("textBox1.Properties.Mask.PlaceHolder");
      textBox1.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("textBox1.Properties.Mask.SaveLiteral");
      textBox1.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("textBox1.Properties.Mask.ShowPlaceHolders");
      textBox1.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("textBox1.Properties.Mask.UseMaskAsDisplayFormat");
      textBox1.Properties.NullValuePrompt = componentResourceManager.GetString("textBox1.Properties.NullValuePrompt");
      textBox1.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("textBox1.Properties.NullValuePromptShowForEmptyValue");
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
      componentResourceManager.ApplyResources(btSave, "btSave");
      btSave.DialogResult = DialogResult.OK;
      btSave.Name = "btSave";
      btSave.Click += btSave_Click;
      componentResourceManager.ApplyResources(btCancel, "btCancel");
      btCancel.DialogResult = DialogResult.Cancel;
      btCancel.Name = "btCancel";
      btCancel.Click += btCancel_Click;
      AcceptButton = btSave;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = btCancel;
      Controls.Add(btCancel);
      Controls.Add(btSave);
      Controls.Add(label1);
      Controls.Add(textBox1);
      Name = "SetName";
      Load += SetName_Load;
      textBox1.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
