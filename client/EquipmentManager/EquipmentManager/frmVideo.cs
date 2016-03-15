// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmVideo
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using BasicComponents.VideoServer;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class FrmVideo : XtraForm
  {
    public BcDevices MainDevice = new BcDevices();
    private IContainer components = (IContainer) null;
    public Thread MainThread;
    public bool CloseForm;
    private bool _nextPos;
    private PictureEdit pictureEdit1;
    private TrackBarControl trackBarControl1;
    private GroupControl groupControl1;
    private SimpleButton btStop;
    private SimpleButton btPause;
    private SimpleButton btPlay;
    private LabelControl lbPeriod;

    public FrmVideo()
    {
      this.InitializeComponent();
    }

    private void frmVideo_Load(object sender, EventArgs e)
    {
      this.Text = this.MainDevice.Name;
      this.FormClosing += new FormClosingEventHandler(this.frmVideo_FormClosing);
      this.MainThread = new Thread(new ThreadStart(this.WorkerThread))
      {
        IsBackground = true
      };
      this.MainThread.Start();
      if (this.MainDevice.Type == "File")
      {
        this.groupControl1.Visible = true;
        this.trackBarControl1.Visible = true;
        BcVideoServer vs = BcVideoServer.LoadById(this.MainDevice.Vsid);
        WcfExtensions.Using<VideoServerClient>(new VideoServerClient(), (Action<VideoServerClient>) (service =>
        {
          service.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) vs.Ip + ":" + (string) (object) vs.Port + "/VideoStreamServer/VideoServer");
          service.Open();
          Thread.Sleep(1000);
          try
          {
            this.trackBarControl1.Properties.Maximum = (int) service.GetDuration(this.MainDevice.Id);
          }
          catch (Exception ex)
          {
          }
        }));
      }
      else
        this.pictureEdit1.Size = new Size(this.pictureEdit1.Width, this.pictureEdit1.Height + this.groupControl1.Height);
    }

    private void frmVideo_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.CloseForm = true;
    }

    private void NewImage(Bitmap image)
    {
      if (this.CloseForm)
        return;
      try
      {
        this.pictureEdit1.Image.Dispose();
      }
      catch (Exception ex)
      {
      }
      this.pictureEdit1.Image = (Image) image;
    }

    public void WorkerThread()
    {
      while (!FrmDevices.ClosFlag)
      {
        if (this.CloseForm)
          break;
        try
        {
          BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
          if (bcVideoServer.Id != Guid.Empty)
          {
            int num = 0;
            VideoServerClient videoServerClient = new VideoServerClient();
            videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
            videoServerClient.Open();
            while (!FrmDevices.ClosFlag)
            {
              ++num;
              if (!this.CloseForm)
              {
                byte[] imageFromDevice = videoServerClient.GetImageFromDevice(this.MainDevice.Id);
                if (imageFromDevice != null && imageFromDevice.Length > 0)
                {
                  using (MemoryStream memoryStream = new MemoryStream(imageFromDevice))
                    this.Invoke((Delegate) new FrmVideo.NewImageFunc(this.NewImage), (object) new Bitmap((Stream) memoryStream));
                }
                Thread.Sleep(20);
                if (num >= 20 && this.MainDevice.Type == "File")
                {
                  try
                  {
                    this._nextPos = true;
                    this.Invoke((Delegate) new FrmVideo.SetNewPosFunc(this.SetNewPos), (object) (int) videoServerClient.GetCurrentPos(this.MainDevice.Id));
                    this._nextPos = false;
                    num = 0;
                  }
                  catch (Exception ex)
                  {
                  }
                }
              }
              else
                break;
            }
          }
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(1000);
      }
    }

    private string GetTime(double val)
    {
      return this.GetHours(val) + ":" + this.GetMinuts(val) + ":" + this.GetSeconds(val);
    }

    private string GetHours(double val)
    {
      int num = (int) val / 3600;
      if (num >= 10)
        return num.ToString();
      if (num > 0)
        return "0" + (object) num;
      return "00";
    }

    private string GetMinuts(double val)
    {
      int num1 = (int) val % 3600;
      if (num1 <= 0)
        return "00";
      int num2 = num1 / 60;
      if (num2 >= 10)
        return num2.ToString();
      if (num2 > 0)
        return "0" + (object) num2;
      return "00";
    }

    private string GetSeconds(double val)
    {
      int num1 = (int) val % 3600;
      if (num1 <= 0)
        return "00";
      int num2 = num1 % 60;
      if (num2 <= 0)
        return "00";
      if (num2 >= 10)
        return num2.ToString();
      if (num2 > 0)
        return "0" + (object) num2;
      return "00";
    }

    private void SetNewPos(int val)
    {
      this.trackBarControl1.Value = val;
    }

    private void btPlay_Click(object sender, EventArgs e)
    {
      try
      {
        BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
        VideoServerClient videoServerClient = new VideoServerClient();
        videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
        videoServerClient.Open();
        this.trackBarControl1.Visible = true;
        this.trackBarControl1.Value = 0;
        this.btPause.Visible = true;
        this.btPlay.Visible = true;
        this.btStop.Visible = true;
        videoServerClient.PlayStart(this.MainDevice.Id);
        try
        {
          Thread.Sleep(2000);
          this.trackBarControl1.Properties.Maximum = (int) videoServerClient.GetDuration(this.MainDevice.Id);
        }
        catch (Exception ex)
        {
        }
        this.btPause.Text = "Пауза";
        videoServerClient.Abort();
      }
      catch (Exception ex)
      {
      }
    }

    private void btStop_Click(object sender, EventArgs e)
    {
      try
      {
        BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
        VideoServerClient videoServerClient = new VideoServerClient();
        videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
        videoServerClient.Open();
        this.trackBarControl1.Value = 0;
        videoServerClient.Stop(this.MainDevice.Id);
        videoServerClient.Abort();
      }
      catch (Exception ex)
      {
      }
    }

    private void btPause_Click(object sender, EventArgs e)
    {
      if (this.btPause.Text == Messages.Pause)
      {
        try
        {
          BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
          VideoServerClient videoServerClient = new VideoServerClient();
          videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
          videoServerClient.Open();
          videoServerClient.Pause(this.MainDevice.Id);
          this.btPause.Text = Messages.Continue;
          videoServerClient.Abort();
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        try
        {
          BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
          VideoServerClient videoServerClient = new VideoServerClient();
          videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
          videoServerClient.Open();
          this.btPause.Text = Messages.Pause;
          videoServerClient.Run(this.MainDevice.Id);
          videoServerClient.Abort();
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
    {
      if (!this._nextPos)
      {
        try
        {
          BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.MainDevice.Vsid);
          VideoServerClient videoServerClient = new VideoServerClient();
          videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) bcVideoServer.Ip + ":" + (string) (object) bcVideoServer.Port + "/VideoStreamServer/VideoServer");
          videoServerClient.Open();
          videoServerClient.SetPos(this.MainDevice.Id, (double) this.trackBarControl1.Value);
          videoServerClient.Abort();
        }
        catch (Exception ex)
        {
        }
      }
      this.lbPeriod.Text = this.GetTime((double) this.trackBarControl1.Value) + "//" + this.GetTime((double) this.trackBarControl1.Properties.Maximum);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmVideo));
      this.pictureEdit1 = new PictureEdit();
      this.trackBarControl1 = new TrackBarControl();
      this.groupControl1 = new GroupControl();
      this.lbPeriod = new LabelControl();
      this.btStop = new SimpleButton();
      this.btPause = new SimpleButton();
      this.btPlay = new SimpleButton();
      this.pictureEdit1.Properties.BeginInit();
      ((ISupportInitialize) this.trackBarControl1).BeginInit();
      this.trackBarControl1.Properties.BeginInit();
      this.groupControl1.BeginInit();
      this.groupControl1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.pictureEdit1, "pictureEdit1");
      this.pictureEdit1.Name = "pictureEdit1";
      this.pictureEdit1.Properties.AccessibleDescription = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleDescription");
      this.pictureEdit1.Properties.AccessibleName = componentResourceManager.GetString("pictureEdit1.Properties.AccessibleName");
      this.pictureEdit1.Properties.PictureStoreMode = PictureStoreMode.Image;
      this.pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
      componentResourceManager.ApplyResources((object) this.trackBarControl1, "trackBarControl1");
      this.trackBarControl1.Name = "trackBarControl1";
      this.trackBarControl1.Properties.AccessibleDescription = componentResourceManager.GetString("trackBarControl1.Properties.AccessibleDescription");
      this.trackBarControl1.Properties.AccessibleName = componentResourceManager.GetString("trackBarControl1.Properties.AccessibleName");
      this.trackBarControl1.Properties.Orientation = (Orientation) componentResourceManager.GetObject("trackBarControl1.Properties.Orientation");
      this.trackBarControl1.EditValueChanged += new EventHandler(this.trackBarControl1_EditValueChanged);
      componentResourceManager.ApplyResources((object) this.groupControl1, "groupControl1");
      this.groupControl1.Controls.Add((Control) this.lbPeriod);
      this.groupControl1.Controls.Add((Control) this.btStop);
      this.groupControl1.Controls.Add((Control) this.btPause);
      this.groupControl1.Controls.Add((Control) this.btPlay);
      this.groupControl1.Controls.Add((Control) this.trackBarControl1);
      this.groupControl1.Name = "groupControl1";
      this.groupControl1.ShowCaption = false;
      componentResourceManager.ApplyResources((object) this.lbPeriod, "lbPeriod");
      this.lbPeriod.Name = "lbPeriod";
      componentResourceManager.ApplyResources((object) this.btStop, "btStop");
      this.btStop.Appearance.Font = (Font) componentResourceManager.GetObject("btStop.Appearance.Font");
      this.btStop.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btStop.Appearance.FontSizeDelta");
      this.btStop.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btStop.Appearance.FontStyleDelta");
      this.btStop.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btStop.Appearance.GradientMode");
      this.btStop.Appearance.Image = (Image) componentResourceManager.GetObject("btStop.Appearance.Image");
      this.btStop.Appearance.Options.UseFont = true;
      this.btStop.Name = "btStop";
      this.btStop.Click += new EventHandler(this.btStop_Click);
      componentResourceManager.ApplyResources((object) this.btPause, "btPause");
      this.btPause.Appearance.Font = (Font) componentResourceManager.GetObject("btPause.Appearance.Font");
      this.btPause.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPause.Appearance.FontSizeDelta");
      this.btPause.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPause.Appearance.FontStyleDelta");
      this.btPause.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPause.Appearance.GradientMode");
      this.btPause.Appearance.Image = (Image) componentResourceManager.GetObject("btPause.Appearance.Image");
      this.btPause.Appearance.Options.UseFont = true;
      this.btPause.Name = "btPause";
      this.btPause.Click += new EventHandler(this.btPause_Click);
      componentResourceManager.ApplyResources((object) this.btPlay, "btPlay");
      this.btPlay.Appearance.Font = (Font) componentResourceManager.GetObject("btPlay.Appearance.Font");
      this.btPlay.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPlay.Appearance.FontSizeDelta");
      this.btPlay.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPlay.Appearance.FontStyleDelta");
      this.btPlay.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPlay.Appearance.GradientMode");
      this.btPlay.Appearance.Image = (Image) componentResourceManager.GetObject("btPlay.Appearance.Image");
      this.btPlay.Appearance.Options.UseFont = true;
      this.btPlay.Name = "btPlay";
      this.btPlay.Click += new EventHandler(this.btPlay_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupControl1);
      this.Controls.Add((Control) this.pictureEdit1);
      this.Name = "FrmVideo";
      this.Load += new EventHandler(this.frmVideo_Load);
      this.pictureEdit1.Properties.EndInit();
      this.trackBarControl1.Properties.EndInit();
      ((ISupportInitialize) this.trackBarControl1).EndInit();
      this.groupControl1.EndInit();
      this.groupControl1.ResumeLayout(false);
      this.groupControl1.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void NewImageFunc(Bitmap image);

    private delegate void SetNewPosFunc(int val);
  }
}
