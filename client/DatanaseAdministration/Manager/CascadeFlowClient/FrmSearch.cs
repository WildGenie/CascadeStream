// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.FrmSearch
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.XtraEditors;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = TS.Sdk.StaticFace.Model.Image;

namespace CascadeManager.CascadeFlowClient
{
  public class FrmSearch : XtraForm
  {
    private IContainer components = null;
    private GroupControl groupBox1;
    private PictureBox pictureBox1;
    private GroupControl groupBox2;
    private PictureBox pictureBox2;
    private SimpleButton button1;
    private SimpleButton btSave1;
    private SimpleButton btSave2;
    private SimpleButton button3;
    private LabelControl label1;

    public FrmSearch()
    {
      InitializeComponent();
    }

    public FrmSearch(Bitmap bmpSource, Guid faceId, DateTime dt, Guid imageId)
    {
      InitializeComponent();
      BcFace bcFace = BcFace.LoadById(faceId);
      List<BcImage> list = BcImage.LoadByFaceId(faceId);
      foreach (BcImage bcImage in list)
      {
        if (bcImage.IsMain)
        {
          if (bcImage.Image.Length > 0)
          {
            pictureBox1.Image = bmpSource;
            pictureBox2.Image = new Bitmap(new MemoryStream(bcImage.Image));
          }
        }
        else if (list.Count > 0 && bcImage.Image.Length > 0)
        {
          pictureBox1.Image = bmpSource;
          pictureBox2.Image = new Bitmap(new MemoryStream(bcImage.Image));
        }
      }
      groupBox1.Text = groupBox1.Text + (object) " (" + Messages.PassDateAndTime + ": " + (string) (object) dt + ")";
      groupBox2.Text = groupBox2.Text + " (" + bcFace.Surname + " " + bcFace.FirstName + " " + bcFace.LastName + ")";
    }

    private void frmSerach_Load(object sender, EventArgs e)
    {
    }

    private void btSave1_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Bitmap image|*.bmp";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      pictureBox1.Image.Save(saveFileDialog.FileName);
    }

    private void btSave2_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Bitmap image|*.bmp";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      new Bitmap(pictureBox2.Image).Save(saveFileDialog.FileName);
    }

    private void button3_Click(object sender, EventArgs e)
    {
      Image image1 = ((Bitmap) pictureBox1.Image).ConvertFrom();
      Image image2 = ((Bitmap) pictureBox2.Image).ConvertFrom();
      FaceInfo face1 = MainForm.Engine.DetectMaxFace(image1, null);
      FaceInfo face2 = MainForm.Engine.DetectMaxFace(image2, null);
      byte[] t1 = face1 != null ? MainForm.Engine.ExtractTemplate(image1, face1) : null;
      byte[] t2 = face2 != null ? MainForm.Engine.ExtractTemplate(image2, face2) : null;
      label1.Text = t1 == null || t2 == null ? double.NaN.ToString(CultureInfo.CurrentCulture) : MainForm.Engine.Verify(t1, t2).ToString(CultureInfo.CurrentCulture);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmSearch));
      groupBox1 = new GroupControl();
      btSave1 = new SimpleButton();
      pictureBox1 = new PictureBox();
      groupBox2 = new GroupControl();
      btSave2 = new SimpleButton();
      pictureBox2 = new PictureBox();
      button1 = new SimpleButton();
      button3 = new SimpleButton();
      label1 = new LabelControl();
      groupBox1.BeginInit();
      groupBox1.SuspendLayout();
      ((ISupportInitialize) pictureBox1).BeginInit();
      groupBox2.BeginInit();
      groupBox2.SuspendLayout();
      ((ISupportInitialize) pictureBox2).BeginInit();
      SuspendLayout();
      groupBox1.Appearance.Font = (Font) componentResourceManager.GetObject("groupBox1.Appearance.Font");
      groupBox1.Appearance.Options.UseFont = true;
      groupBox1.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox1.AppearanceCaption.Font");
      groupBox1.AppearanceCaption.Options.UseFont = true;
      groupBox1.Controls.Add(btSave1);
      groupBox1.Controls.Add(pictureBox1);
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Name = "groupBox1";
      btSave1.Appearance.Font = (Font) componentResourceManager.GetObject("btSave1.Appearance.Font");
      btSave1.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btSave1, "btSave1");
      btSave1.Name = "btSave1";
      btSave1.Click += btSave1_Click;
      componentResourceManager.ApplyResources(pictureBox1, "pictureBox1");
      pictureBox1.Name = "pictureBox1";
      pictureBox1.TabStop = false;
      groupBox2.Appearance.Font = (Font) componentResourceManager.GetObject("groupBox2.Appearance.Font");
      groupBox2.Appearance.Options.UseFont = true;
      groupBox2.AppearanceCaption.Font = (Font) componentResourceManager.GetObject("groupBox2.AppearanceCaption.Font");
      groupBox2.AppearanceCaption.Options.UseFont = true;
      groupBox2.Controls.Add(btSave2);
      groupBox2.Controls.Add(pictureBox2);
      componentResourceManager.ApplyResources(groupBox2, "groupBox2");
      groupBox2.Name = "groupBox2";
      btSave2.Appearance.Font = (Font) componentResourceManager.GetObject("btSave2.Appearance.Font");
      btSave2.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(btSave2, "btSave2");
      btSave2.Name = "btSave2";
      btSave2.Click += btSave2_Click;
      componentResourceManager.ApplyResources(pictureBox2, "pictureBox2");
      pictureBox2.Name = "pictureBox2";
      pictureBox2.TabStop = false;
      button1.Appearance.Font = (Font) componentResourceManager.GetObject("button1.Appearance.Font");
      button1.Appearance.Options.UseFont = true;
      button1.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources(button1, "button1");
      button1.Name = "button1";
      button1.Click += button1_Click;
      button3.Appearance.Font = (Font) componentResourceManager.GetObject("button3.Appearance.Font");
      button3.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources(button3, "button3");
      button3.Name = "button3";
      button3.Click += button3_Click;
      label1.Appearance.Font = (Font) componentResourceManager.GetObject("label1.Appearance.Font");
      componentResourceManager.ApplyResources(label1, "label1");
      label1.Name = "label1";
      AcceptButton = button1;
      componentResourceManager.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = button1;
      Controls.Add(label1);
      Controls.Add(button3);
      Controls.Add(button1);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmSearch";
      Load += frmSerach_Load;
      groupBox1.EndInit();
      groupBox1.ResumeLayout(false);
      ((ISupportInitialize) pictureBox1).EndInit();
      groupBox2.EndInit();
      groupBox2.ResumeLayout(false);
      ((ISupportInitialize) pictureBox2).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
