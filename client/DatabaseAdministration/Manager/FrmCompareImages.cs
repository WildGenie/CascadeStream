// Decompiled with JetBrains decompiler
// Type: CascadeManager.FrmCompareImages
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace CascadeManager
{
  public class FrmCompareImages : XtraForm
  {
    private IContainer components = null;
    private GroupControl groupControl1;
    private GroupControl groupControl2;
    public PictureEdit pbMainImage;
    public PictureEdit pbCamImage;

    public FrmCompareImages()
    {
      InitializeComponent();
    }

    private void frmCompareImages_Load(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmCompareImages));
      groupControl1 = new GroupControl();
      pbMainImage = new PictureEdit();
      groupControl2 = new GroupControl();
      pbCamImage = new PictureEdit();
      groupControl1.BeginInit();
      groupControl1.SuspendLayout();
      pbMainImage.Properties.BeginInit();
      groupControl2.BeginInit();
      groupControl2.SuspendLayout();
      pbCamImage.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(groupControl1, "groupControl1");
      groupControl1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Font");
      groupControl1.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontSizeDelta");
      groupControl1.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl1.AppearanceCaption.FontStyleDelta");
      groupControl1.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl1.AppearanceCaption.GradientMode");
      groupControl1.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupControl1.AppearanceCaption.Image");
      groupControl1.AppearanceCaption.Options.UseFont = true;
      groupControl1.Controls.Add(pbMainImage);
      groupControl1.Name = "groupControl1";
      componentResourceManager.ApplyResources(pbMainImage, "pbMainImage");
      pbMainImage.Name = "pbMainImage";
      pbMainImage.Properties.AccessibleDescription = componentResourceManager.GetString("pbMainImage.Properties.AccessibleDescription");
      pbMainImage.Properties.AccessibleName = componentResourceManager.GetString("pbMainImage.Properties.AccessibleName");
      pbMainImage.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(groupControl2, "groupControl2");
      groupControl2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Font");
      groupControl2.AppearanceCaption.FontSizeDelta = (int) componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontSizeDelta");
      groupControl2.AppearanceCaption.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("groupControl2.AppearanceCaption.FontStyleDelta");
      groupControl2.AppearanceCaption.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("groupControl2.AppearanceCaption.GradientMode");
      groupControl2.AppearanceCaption.Image = (Image) componentResourceManager.GetObject("groupControl2.AppearanceCaption.Image");
      groupControl2.AppearanceCaption.Options.UseFont = true;
      groupControl2.Controls.Add(pbCamImage);
      groupControl2.Name = "groupControl2";
      componentResourceManager.ApplyResources(pbCamImage, "pbCamImage");
      pbCamImage.Name = "pbCamImage";
      pbCamImage.Properties.AccessibleDescription = componentResourceManager.GetString("pbCamImage.Properties.AccessibleDescription");
      pbCamImage.Properties.AccessibleName = componentResourceManager.GetString("pbCamImage.Properties.AccessibleName");
      pbCamImage.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(groupControl2);
      Controls.Add(groupControl1);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmCompareImages";
      Load += frmCompareImages_Load;
      groupControl1.EndInit();
      groupControl1.ResumeLayout(false);
      pbMainImage.Properties.EndInit();
      groupControl2.EndInit();
      groupControl2.ResumeLayout(false);
      pbCamImage.Properties.EndInit();
      ResumeLayout(false);
    }
  }
}
