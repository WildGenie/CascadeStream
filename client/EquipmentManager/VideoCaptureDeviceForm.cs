// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.VideoCaptureDeviceForm
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using AForge.Video.DirectShow;
using BasicComponents;
using CascadeEquipment.Properties;
using CS.DAL;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class VideoCaptureDeviceForm : XtraForm
  {
    public static int SelectedCap = 0;
    public string Cap = "";
    public string WebCamName = "";
    private readonly List<BcDevices> _devices = new List<BcDevices>();
    private IContainer components = (IContainer) null;
    private FilterInfoCollection _videoDevices;
    private string _device;
    private SimpleButton _cancelButton;
    private SimpleButton _okButton;
    private LabelControl _label1;
    private LabelControl _label2;
    private ComboBoxEdit _devicesCombo;
    private ComboBoxEdit CbCapabilities;

    public string VideoDevice
    {
      get
      {
        return this._device;
      }
    }

    public VideoCaptureDeviceForm()
    {
      this.InitializeComponent();
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.TechServIcon;
      try
      {
        this._videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        foreach (FilterInfo filterInfo in (CollectionBase) this._videoDevices)
        {
          this._devicesCombo.Properties.Items.Add((object) filterInfo.Name);
          this._devices.Add(new BcDevices());
        }
        foreach (BcDevices bcDevices in BcDevicesStorageExtensions.LoadAll())
        {
          this._devices.Add(bcDevices);
          this._devicesCombo.Properties.Items.Add((object) bcDevices.Name);
        }
      }
      catch (ApplicationException ex)
      {
        this._devicesCombo.Properties.Items.Add((object) "No local capture devices");
        this._devicesCombo.Enabled = false;
        this._okButton.Enabled = false;
      }
      this._devicesCombo.SelectedIndex = 0;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (this._devicesCombo.SelectedIndex == -1 || this._devicesCombo.SelectedIndex == -1)
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
      this._device = this._videoDevices[this._devicesCombo.SelectedIndex].MonikerString;
      this.WebCamName = this._videoDevices[this._devicesCombo.SelectedIndex].Name;
      this.Cap = this.CbCapabilities.EditValue.ToString();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this._devicesCombo.SelectedIndex == -1)
        return;
      this.CbCapabilities.Properties.Items.Clear();
      if (this._videoDevices.Count > 0)
      {
        if (this._devices[this._devicesCombo.SelectedIndex].Id == Guid.Empty)
        {
          this.CbCapabilities.Enabled = true;
          VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice(this._videoDevices[this._devicesCombo.SelectedIndex].MonikerString);
          if (videoCaptureDevice.VideoCapabilities != null)
          {
            foreach (VideoCapabilities videoCapabilities in videoCaptureDevice.VideoCapabilities)
              this.CbCapabilities.Properties.Items.Add((object) string.Format("{0}x{1} {2}fps", (object) videoCapabilities.FrameSize.Width, (object) videoCapabilities.FrameSize.Height, (object) videoCapabilities.AverageFrameRate));
          }
        }
        else
          this.CbCapabilities.Enabled = false;
      }
    }

    private void _cancelButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VideoCaptureDeviceForm));
      this._cancelButton = new SimpleButton();
      this._okButton = new SimpleButton();
      this._label1 = new LabelControl();
      this._label2 = new LabelControl();
      this._devicesCombo = new ComboBoxEdit();
      this.CbCapabilities = new ComboBoxEdit();
      this._devicesCombo.Properties.BeginInit();
      this.CbCapabilities.Properties.BeginInit();
      this.SuspendLayout();
      this._cancelButton.Appearance.Options.UseFont = true;
      this._cancelButton.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this._cancelButton, "_cancelButton");
      this._cancelButton.Name = "_cancelButton";
      this._cancelButton.Click += new EventHandler(this._cancelButton_Click);
      this._okButton.Appearance.Options.UseFont = true;
      componentResourceManager.ApplyResources((object) this._okButton, "_okButton");
      this._okButton.Name = "_okButton";
      this._okButton.Click += new EventHandler(this.okButton_Click);
      componentResourceManager.ApplyResources((object) this._label1, "_label1");
      this._label1.Name = "_label1";
      componentResourceManager.ApplyResources((object) this._label2, "_label2");
      this._label2.Name = "_label2";
      componentResourceManager.ApplyResources((object) this._devicesCombo, "_devicesCombo");
      this._devicesCombo.Name = "_devicesCombo";
      this._devicesCombo.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("_devicesCombo.Properties.Buttons"))
      });
      this._devicesCombo.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
      this._devicesCombo.SelectedIndexChanged += new EventHandler(this.devicesCombo_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.CbCapabilities, "CbCapabilities");
      this.CbCapabilities.Name = "CbCapabilities";
      this.CbCapabilities.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("CbCapabilities.Properties.Buttons"))
      });
      this.CbCapabilities.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
      this.AcceptButton = (IButtonControl) this._okButton;
      this.CancelButton = (IButtonControl) this._cancelButton;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.CbCapabilities);
      this.Controls.Add((Control) this._devicesCombo);
      this.Controls.Add((Control) this._label2);
      this.Controls.Add((Control) this._cancelButton);
      this.Controls.Add((Control) this._okButton);
      this.Controls.Add((Control) this._label1);
      this.Name = "VideoCaptureDeviceForm";
      this._devicesCombo.Properties.EndInit();
      this.CbCapabilities.Properties.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
