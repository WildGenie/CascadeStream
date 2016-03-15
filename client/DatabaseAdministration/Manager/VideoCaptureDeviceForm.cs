// Decompiled with JetBrains decompiler
// Type: CascadeManager.VideoCaptureDeviceForm
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace CascadeManager
{
  public class VideoCaptureDeviceForm : XtraForm
  {
    public string Cap = "";
    public string WebCamName = "";
    private IContainer components = null;
    private FilterInfoCollection _videoDevices;
    private string _device;
    public static int SelectedCap;
    private SimpleButton _cancelButton;
    private SimpleButton _okButton;
    private ComboBoxEdit _devicesCombo;
    private LabelControl _label1;
    public ComboBoxEdit CbCapabilities;
    private LabelControl _label2;

    public string VideoDevice
    {
      get
      {
        return _device;
      }
    }

    public VideoCaptureDeviceForm()
    {
      InitializeComponent();
      FormBorderStyle = FormBorderStyle.FixedDialog;
      string str = "";
      try
      {
        StreamReader streamReader = new StreamReader("WebCamSettings.ini");
        try
        {
          str = streamReader.ReadLine();
        }
        catch
        {
        }
        streamReader.Close();
      }
      catch
      {
      }
      try
      {
        _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        foreach (FilterInfo filterInfo in _videoDevices)
        {
          _devicesCombo.Properties.Items.Add(filterInfo.Name);
          if (str != "" && filterInfo.Name == str)
            _devicesCombo.SelectedIndex = _devicesCombo.Properties.Items.Count - 1;
        }
      }
      catch (ApplicationException ex)
      {
        _devicesCombo.Properties.Items.Add("No local capture devices");
        _devicesCombo.Enabled = false;
        _okButton.Enabled = false;
      }
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      SelectedCap = CbCapabilities.SelectedIndex;
      _device = _videoDevices[_devicesCombo.SelectedIndex].MonikerString;
      WebCamName = _videoDevices[_devicesCombo.SelectedIndex].Name;
      Cap = CbCapabilities.Text;
      try
      {
        StreamWriter streamWriter = new StreamWriter("WebCamSettings.ini", false);
        try
        {
          streamWriter.WriteLine(_devicesCombo.Text);
          streamWriter.WriteLine(CbCapabilities.SelectedIndex);
        }
        catch
        {
        }
        streamWriter.Close();
      }
      catch
      {
      }
    }

    private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (_devicesCombo.SelectedIndex == -1)
        return;
      CbCapabilities.Properties.Items.Clear();
      if (_videoDevices.Count > 0)
      {
        CbCapabilities.Enabled = true;
        VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice(_videoDevices[_devicesCombo.SelectedIndex].MonikerString);
        if (videoCaptureDevice.VideoCapabilities != null)
        {
          foreach (VideoCapabilities videoCapabilities in videoCaptureDevice.VideoCapabilities)
            CbCapabilities.Properties.Items.Add(string.Format("{0}x{1}", videoCapabilities.FrameSize.Width, videoCapabilities.FrameSize.Height));
          if (FrmWebCam.SelectedCap != -1)
            CbCapabilities.SelectedIndex = FrmWebCam.SelectedCap;
          else if (CbCapabilities.Properties.Items.Count > 0)
            CbCapabilities.SelectedIndex = 0;
        }
      }
    }

    private void VideoCaptureDeviceForm_Load(object sender, EventArgs e)
    {
    }

    private void VideoCaptureDeviceForm_HelpButtonClicked(object sender, CancelEventArgs e)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/5.htm");
    }

    private void VideoCaptureDeviceForm_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      Help.ShowHelp(this, Application.StartupPath + "\\help.chm", Application.StartupPath + "\\help.chm::/5.htm");
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VideoCaptureDeviceForm));
      _cancelButton = new SimpleButton();
      _okButton = new SimpleButton();
      _devicesCombo = new ComboBoxEdit();
      _label1 = new LabelControl();
      CbCapabilities = new ComboBoxEdit();
      _label2 = new LabelControl();
      _devicesCombo.Properties.BeginInit();
      CbCapabilities.Properties.BeginInit();
      SuspendLayout();
      _cancelButton.Appearance.Font = new Font("Tahoma", 9f);
      _cancelButton.Appearance.Options.UseFont = true;
      _cancelButton.DialogResult = DialogResult.Cancel;
      _cancelButton.Location = new Point(265, 143);
      _cancelButton.Name = "_cancelButton";
      _cancelButton.Size = new Size(75, 23);
      _cancelButton.TabIndex = 11;
      _cancelButton.Text = "Отмена";
      _okButton.Appearance.Font = new Font("Tahoma", 9f);
      _okButton.Appearance.Options.UseFont = true;
      _okButton.DialogResult = DialogResult.OK;
      _okButton.Location = new Point(175, 143);
      _okButton.Name = "_okButton";
      _okButton.Size = new Size(75, 23);
      _okButton.TabIndex = 10;
      _okButton.Text = "Применить";
      _okButton.Click += okButton_Click;
      _devicesCombo.Location = new Point(10, 35);
      _devicesCombo.Name = "_devicesCombo";
      _devicesCombo.Properties.Appearance.Font = new Font("Tahoma", 9f);
      _devicesCombo.Properties.Appearance.Options.UseFont = true;
      _devicesCombo.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      _devicesCombo.Size = new Size(325, 20);
      _devicesCombo.TabIndex = 9;
      _devicesCombo.SelectedIndexChanged += devicesCombo_SelectedIndexChanged;
      _label1.Appearance.Font = new Font("Tahoma", 9f);
      _label1.Location = new Point(10, 10);
      _label1.Name = "_label1";
      _label1.Size = new Size(192, 14);
      _label1.TabIndex = 8;
      _label1.Text = "Выберите локальное устройство:";
      _label1.Click += label1_Click;
      CbCapabilities.Location = new Point(10, 102);
      CbCapabilities.Name = "CbCapabilities";
      CbCapabilities.Properties.Appearance.Font = new Font("Tahoma", 9f);
      CbCapabilities.Properties.Appearance.Options.UseFont = true;
      CbCapabilities.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      CbCapabilities.Size = new Size(325, 20);
      CbCapabilities.TabIndex = 12;
      CbCapabilities.SelectedIndexChanged += cbCapabilities_SelectedIndexChanged;
      _label2.Appearance.Font = new Font("Tahoma", 9f);
      _label2.Location = new Point(10, 86);
      _label2.Name = "_label2";
      _label2.Size = new Size(222, 14);
      _label2.TabIndex = 13;
      _label2.Text = "Установите необходимое разрешение:";
      _label2.Click += label2_Click;
      AcceptButton = _okButton;
      CancelButton = _cancelButton;
      ClientSize = new Size(351, 187);
      Controls.Add(_label2);
      Controls.Add(CbCapabilities);
      Controls.Add(_cancelButton);
      Controls.Add(_okButton);
      Controls.Add(_devicesCombo);
      Controls.Add(_label1);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      MaximizeBox = false;
      MaximumSize = new Size(367, 226);
      MinimizeBox = false;
      MinimumSize = new Size(357, 215);
      Name = "VideoCaptureDeviceForm";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Открыть видео устройство";
      HelpButtonClicked += VideoCaptureDeviceForm_HelpButtonClicked;
      Load += VideoCaptureDeviceForm_Load;
      HelpRequested += VideoCaptureDeviceForm_HelpRequested;
      _devicesCombo.Properties.EndInit();
      CbCapabilities.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    private void cbCapabilities_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }
  }
}
