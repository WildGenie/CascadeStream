// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.FrmEditDevice
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using BasicComponents;
using CS.DAL;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CascadeEquipment
{
  public class FrmEditDevice : XtraForm
  {
    private readonly List<string> _supportedCameraTypes = new List<string>();
    private List<BcObjects> _objects = new List<BcObjects>();
    public BcDevices Device = new BcDevices();
    private IContainer components = (IContainer) null;
    private SimpleButton btLoadFile;
    private LabelControl labelControl4;
    private LabelControl labelControl3;
    private LabelControl labelControl2;
    private SimpleButton btLoadDevice;
    private LabelControl label6;
    private ComboBoxEdit cbCamState;
    private TextEdit tbName;
    private TextEdit tbPassword;
    private GroupControl groupBox2;
    private TreeList tvObjects;
    public ComboBoxEdit cbObjects;
    private LabelControl label1;
    private TextEdit tbLogin;
    private GroupControl groupControl2;
    private LabelControl labelControl8;
    private ButtonEdit btES;
    private LabelControl labelControl7;
    private ButtonEdit btDS;
    private LabelControl labelControl6;
    private ButtonEdit btVS;
    private SpinEdit tbMinScore;
    private LabelControl labelControl11;
    private SpinEdit tbExtractorCount;
    private LabelControl labelControl12;
    private SpinEdit tbDetectorCount;
    private LabelControl labelControl13;
    private LabelControl labelControl14;
    private SpinEdit tbDetectionScore;
    private LabelControl labelControl16;
    private ComboBoxEdit cbMinFace;
    private CheckEdit chbSaveFaces;
    private CheckEdit chbSaveImage;
    private CheckEdit chbSaveUnidentified;
    private SimpleButton btSave;
    private SimpleButton btCancel;
    private CheckEdit chbSaveNonCategory;
    private TreeListColumn treeListColumn1;
    private ComboBoxEdit cbNames;
    private TextEdit tbConnectionString;
    private LabelControl lbConnectionString;
    private LabelControl labelControl18;
    private ButtonEdit btIS;
    private GroupControl groupControl3;
    private LookUpEdit cbCameraType;

    public FrmEditDevice()
    {
      this.InitializeComponent();
      this.FillSupportedCameraTypes();
      this.cbCameraType.Properties.DataSource = (object) Enumerable.OrderBy<string, string>((IEnumerable<string>) this._supportedCameraTypes, (Func<string, string>) (s => s));
      this.btSave.Enabled = false;
    }

    private void FillSupportedCameraTypes()
    {
      this._supportedCameraTypes.AddRange((IEnumerable<string>) new string[5]
      {
        "Image",
        "File",
        "Basler Aviator Series",
        "HTTP MJPEG",
        "Web"
      });
    }

    private void ReloadObjects()
    {
      this._objects = BcObjects.LoadAll();
      int selectedIndex = this.cbObjects.SelectedIndex;
      this.cbObjects.Properties.Items.Clear();
      foreach (BcObjects bcObjects in this._objects)
        this.cbObjects.Properties.Items.Add((object) bcObjects.Name);
      this.cbObjects.SelectedIndex = selectedIndex;
    }

    private void LoadData()
    {
      this.cbObjects.Properties.Items.Clear();
      this.cbObjects.SelectedIndex = -1;
      this.tvObjects.ClearNodes();
      this._objects = BcObjects.LoadAll();
      foreach (BcObjects bcObjects in this._objects)
      {
        this.cbObjects.Properties.Items.Add((object) bcObjects.Name);
        bcObjects.GetData();
        if (this.Device.ObjectId == bcObjects.Id)
          this.cbObjects.SelectedIndex = this.cbObjects.Properties.Items.Count - 1;
      }
      this.btVS.Tag = (object) Guid.Empty;
      this.btDS.Tag = (object) Guid.Empty;
      this.btES.Tag = (object) Guid.Empty;
      this.btIS.Tag = (object) Guid.Empty;
      this.cbCameraType.EditValue = (object) null;
      if (this.Device.Id != Guid.Empty)
      {
        this.cbCameraType.EditValue = (object) this.Device.Type;
        if (this.Device.Type == "GeVi Scope")
        {
          this.cbNames.Visible = true;
          this.tbName.Visible = false;
        }
        this.cbCamState.SelectedIndex = this.Device.IsActive ? 0 : 1;
        this.tbExtractorCount.Text = this.Device.ExtractorCount.ToString();
        this.tbDetectionScore.EditValue = (object) this.Device.DetectorScore;
        SpinEdit spinEdit = this.tbDetectorCount;
        int num = this.Device.DetectorCount;
        string str1 = num.ToString();
        spinEdit.Text = str1;
        this.tbMinScore.EditValue = (object) this.Device.MinScore;
        BcIdentificationServer identificationServer = BcIdentificationServer.LoadById(this.Device.Isid);
        this.btIS.Tag = (object) identificationServer.Id;
        this.btIS.Text = identificationServer.Ip;
        BcVideoServer bcVideoServer = BcVideoServer.LoadById(this.Device.Vsid);
        this.btVS.Tag = (object) bcVideoServer.Id;
        this.btVS.Text = bcVideoServer.Ip;
        BcDetectorServer bcDetectorServer = BcDetectorServer.LoadById(this.Device.Dsid);
        this.btDS.Tag = (object) bcDetectorServer.Id;
        this.btDS.Text = bcDetectorServer.Ip;
        BcExtractorServer bcExtractorServer = BcExtractorServer.LoadById(this.Device.Esid);
        this.btES.Tag = (object) bcExtractorServer.Id;
        this.btES.Text = bcExtractorServer.Ip;
        ComboBoxEdit comboBoxEdit = this.cbMinFace;
        num = this.Device.MinFace;
        string str2 = num.ToString();
        comboBoxEdit.Text = str2;
        this.chbSaveFaces.Checked = this.Device.SaveFace;
        this.chbSaveImage.Checked = this.Device.SaveImage;
        this.chbSaveUnidentified.Checked = this.Device.SaveUnidentified;
        this.chbSaveNonCategory.Checked = this.Device.SaveNonCategory;
        this.tbName.Text = this.Device.Name;
        this.cbNames.Text = this.Device.Name;
        this.tbConnectionString.Text = this.Device.ConnectionString;
        this.tbLogin.Text = this.Device.Login;
        this.tbPassword.Text = this.Device.Password;
        List<TreeListNode> nodes = new List<TreeListNode>();
        if (this.cbObjects.SelectedIndex <= -1)
          return;
        nodes.Add(this.tvObjects.Nodes[0]);
        this.GetAllNodes(nodes, this.tvObjects.Nodes[0]);
        if (this.Device.TableId != Guid.Empty)
        {
          foreach (TreeListNode treeListNode in nodes)
          {
            if (treeListNode != this.tvObjects.Nodes[0] && (Guid) treeListNode.Tag == this.Device.TableId)
              treeListNode.Checked = true;
          }
        }
        else
          this.tvObjects.Nodes[0].Checked = true;
      }
      else
      {
        this.Device = new BcDevices();
        this.tbName.EditValue = (object) null;
        this.tbLogin.EditValue = (object) null;
        this.tbPassword.EditValue = (object) null;
        this.tbConnectionString.EditValue = (object) null;
        this.cbCamState.SelectedIndex = 0;
        this.cbCameraType.EditValue = (object) null;
      }
    }

    private void LoadTree(TreeListNode node, BcObjectsData data)
    {
      if (this.tvObjects.Nodes.Count == 0)
      {
        if (this._objects[this.cbObjects.SelectedIndex].Name == "")
          this._objects[this.cbObjects.SelectedIndex].Name = "[" + Messages.ControlledObject + "]";
        TreeListNode treeListNode = this.tvObjects.AppendNode((object) this._objects[this.cbObjects.SelectedIndex].Name, (TreeListNode) null);
        treeListNode.SetValue((object) 0, (object) this._objects[this.cbObjects.SelectedIndex].Name);
        treeListNode.Tag = (object) this._objects[this.cbObjects.SelectedIndex].Id;
        this.tvObjects.Nodes.Add(treeListNode);
        foreach (BcObjectsData data1 in this._objects[this.cbObjects.SelectedIndex].Data)
        {
          if (data1.Name == "")
            data1.Name = "[" + Messages.ControlledObject + "]";
          TreeListNode node1 = this.tvObjects.AppendNode((object) data1.Name, treeListNode);
          node1.Tag = (object) data1.Id;
          node1.SetValue((object) 0, (object) data1.Name);
          this.LoadTree(node1, data1);
        }
      }
      else
      {
        if (node == null)
          return;
        foreach (BcObjectsData data1 in data.Data)
        {
          if (data1.Name == "")
            data1.Name = "[" + Messages.ControlledObject + "]";
          TreeListNode node1 = this.tvObjects.AppendNode((object) data1.Name, node);
          node1.Tag = (object) data1.Id;
          node1.SetValue((object) 0, (object) data1.Name);
          this.LoadTree(node1, data1);
        }
      }
    }

    private void cbObjects_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.tvObjects.Nodes.Clear();
      if (this.cbObjects.SelectedIndex <= -1)
        return;
      this.LoadTree((TreeListNode) null, (BcObjectsData) null);
      this.tvObjects.ExpandAll();
    }

    private void btSave_Click(object sender, EventArgs e)
    {
      this.Device.IsActive = this.cbCamState.SelectedIndex == 0;
      this.Device.MinFace = Convert.ToInt32(this.cbMinFace.Text);
      this.Device.SaveNonCategory = this.chbSaveNonCategory.Checked;
      this.Device.SaveFace = this.chbSaveFaces.Checked;
      this.Device.SaveImage = this.chbSaveImage.Checked;
      this.Device.SaveUnidentified = this.chbSaveUnidentified.Checked;
      this.Device.ExtractorCount = Convert.ToInt32(this.tbExtractorCount.Text);
      this.Device.DetectorScore = this.tbDetectionScore.Value;
      this.Device.DetectorCount = Convert.ToInt32(this.tbDetectorCount.Text);
      this.Device.Vsid = !(this.btVS.Text != "") ? Guid.Empty : (Guid) this.btVS.Tag;
      this.Device.Isid = !(this.btIS.Text != "") ? Guid.Empty : (Guid) this.btIS.Tag;
      this.Device.Dsid = !(this.btDS.Text != "") ? Guid.Empty : (Guid) this.btDS.Tag;
      this.Device.Esid = !(this.btES.Text != "") ? Guid.Empty : (Guid) this.btES.Tag;
      this.Device.MinScore = this.tbMinScore.Value;
      this.Device.Password = this.tbPassword.Text;
      this.Device.Login = this.tbLogin.Text;
      this.Device.Name = this.tbName.Text;
      this.Device.Type = (string) this.cbCameraType.EditValue;
      this.Device.ConnectionString = this.tbConnectionString.EditValue.ToString();
      BcDevicesStorageExtensions.Save(this.Device, true);
    }

    private void GetAllNodes(List<TreeListNode> nodes, TreeListNode node)
    {
      foreach (TreeListNode node1 in node.Nodes)
      {
        nodes.Add(node1);
        this.GetAllNodes(nodes, node1);
      }
    }

    private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
    {
      if (e.Node.Checked && e.Node != this.tvObjects.Nodes[0])
      {
        this.Device.TableId = (Guid) e.Node.Tag;
        this.Device.ObjectId = this._objects[this.cbObjects.SelectedIndex].Id;
      }
      else
      {
        if (!e.Node.Checked || e.Node != this.tvObjects.Nodes[0])
          return;
        this.Device.TableId = Guid.Empty;
        this.Device.ObjectId = this._objects[this.cbObjects.SelectedIndex].Id;
      }
    }

    private void tvObjects_BeforeCheckNode(object sender, CheckNodeEventArgs e)
    {
      if (e.Node.Checked)
        return;
      List<TreeListNode> nodes = new List<TreeListNode>()
      {
        e.Node,
        this.tvObjects.Nodes[0]
      };
      this.GetAllNodes(nodes, this.tvObjects.Nodes[0]);
      foreach (TreeListNode treeListNode in Enumerable.Where<TreeListNode>((IEnumerable<TreeListNode>) nodes, (Func<TreeListNode, bool>) (tn => tn.Checked)))
        treeListNode.Checked = false;
    }

    private void cbCameraType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.lbConnectionString.Visible = false;
      string str = (string) this.cbCameraType.EditValue;
      if (str == "RTSP MJPEG" || str == "HTTP MJPEG")
        this.btLoadDevice.Visible = false;
      else
        this.btLoadDevice.Visible = true;
      if (str == "GeVi Scope")
      {
        this.cbNames.Visible = true;
        this.tbName.Visible = false;
      }
      else
      {
        this.tbName.Visible = true;
        this.cbNames.Visible = false;
      }
      this.btLoadDevice.Visible = str == "Web";
      if (str == "Image" || str == "File")
      {
        this.btLoadFile.Visible = true;
        this.btLoadFile.Enabled = true;
      }
      else
        this.btLoadFile.Visible = false;
    }

    private void btWebCamProperties_Click(object sender, EventArgs e)
    {
      using (VideoCaptureDeviceForm captureDeviceForm = new VideoCaptureDeviceForm())
      {
        if (captureDeviceForm.ShowDialog() != DialogResult.OK)
          return;
        this.Device.ConnectionString = captureDeviceForm.VideoDevice;
        this.Device.WebCameraName = captureDeviceForm.WebCamName;
        this.Device.Capability = captureDeviceForm.Cap;
        this.tbConnectionString.EditValue = (object) this.Device.ConnectionString;
      }
    }

    private void frmEditDevice_Load(object sender, EventArgs e)
    {
      this.ReloadObjects();
      this.LoadData();
    }

    private void btLoadFile_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.tbConnectionString.EditValue = (object) openFileDialog.FileName;
    }

    private void btVS_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      using (FrmVideoServers frmVideoServers = new FrmVideoServers()
      {
        ListFlag = true
      })
      {
        if (frmVideoServers.ShowDialog() != DialogResult.OK)
          return;
        this.btVS.Tag = (object) frmVideoServers.MainServer.Id;
        this.btVS.Text = frmVideoServers.MainServer.Ip;
      }
    }

    private void btDS_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      using (FrmDetectorServers frmDetectorServers = new FrmDetectorServers()
      {
        ListFlag = true
      })
      {
        if (frmDetectorServers.ShowDialog() != DialogResult.OK)
          return;
        this.btDS.Tag = (object) frmDetectorServers.MainServer.Id;
        this.btDS.Text = frmDetectorServers.MainServer.Ip;
      }
    }

    private void btES_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      using (FrmExtractorServers extractorServers = new FrmExtractorServers()
      {
        ListFlag = true
      })
      {
        if (extractorServers.ShowDialog() != DialogResult.OK)
          return;
        this.btES.Tag = (object) extractorServers.MainServer.Id;
        this.btES.Text = extractorServers.MainServer.Ip;
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
    }

    private void btIS_ButtonClick(object sender, ButtonPressedEventArgs e)
    {
      using (FrmIdentificationcServers identificationcServers = new FrmIdentificationcServers()
      {
        ListFlag = true
      })
      {
        if (identificationcServers.ShowDialog() != DialogResult.OK)
          return;
        this.btIS.Tag = (object) identificationcServers.MainServer.Id;
        this.btIS.Text = identificationcServers.MainServer.Ip;
      }
    }

    private void EditControl_EditValueChanged(object sender, EventArgs e)
    {
      string str = (string) this.cbCameraType.EditValue;
      this.tbConnectionString.ReadOnly = str == "Web";
      this.btLoadDevice.Visible = str == "Web";
      this.btLoadFile.Visible = str == "File" || str == "Image";
      if (sender == this.cbCameraType)
        this.tbConnectionString.EditValue = (object) null;
      this.btSave.Enabled = FrmEditDevice.HasNotNullValue((BaseEdit) this.tbName) && FrmEditDevice.HasNotNullValue((BaseEdit) this.tbConnectionString) && FrmEditDevice.HasNotNullValue((BaseEdit) this.cbCameraType);
    }

    private static bool HasNotNullValue(BaseEdit editor)
    {
      return editor != null && editor.EditValue != null && !string.IsNullOrEmpty(editor.EditValue.ToString());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmEditDevice));
      this.groupControl3 = new GroupControl();
      this.btLoadFile = new SimpleButton();
      this.tbName = new TextEdit();
      this.tbLogin = new TextEdit();
      this.tbPassword = new TextEdit();
      this.cbCamState = new ComboBoxEdit();
      this.label6 = new LabelControl();
      this.labelControl2 = new LabelControl();
      this.labelControl3 = new LabelControl();
      this.labelControl4 = new LabelControl();
      this.tbConnectionString = new TextEdit();
      this.label1 = new LabelControl();
      this.lbConnectionString = new LabelControl();
      this.btLoadDevice = new SimpleButton();
      this.cbNames = new ComboBoxEdit();
      this.cbCameraType = new LookUpEdit();
      this.groupControl2 = new GroupControl();
      this.labelControl18 = new LabelControl();
      this.btIS = new ButtonEdit();
      this.chbSaveNonCategory = new CheckEdit();
      this.labelControl16 = new LabelControl();
      this.cbMinFace = new ComboBoxEdit();
      this.chbSaveImage = new CheckEdit();
      this.chbSaveUnidentified = new CheckEdit();
      this.labelControl14 = new LabelControl();
      this.tbDetectionScore = new SpinEdit();
      this.labelControl13 = new LabelControl();
      this.tbMinScore = new SpinEdit();
      this.labelControl11 = new LabelControl();
      this.tbExtractorCount = new SpinEdit();
      this.labelControl12 = new LabelControl();
      this.tbDetectorCount = new SpinEdit();
      this.labelControl8 = new LabelControl();
      this.btES = new ButtonEdit();
      this.labelControl7 = new LabelControl();
      this.btDS = new ButtonEdit();
      this.labelControl6 = new LabelControl();
      this.btVS = new ButtonEdit();
      this.chbSaveFaces = new CheckEdit();
      this.groupBox2 = new GroupControl();
      this.cbObjects = new ComboBoxEdit();
      this.tvObjects = new TreeList();
      this.treeListColumn1 = new TreeListColumn();
      this.btSave = new SimpleButton();
      this.btCancel = new SimpleButton();
      this.groupControl3.BeginInit();
      this.groupControl3.SuspendLayout();
      this.tbName.Properties.BeginInit();
      this.tbLogin.Properties.BeginInit();
      this.tbPassword.Properties.BeginInit();
      this.cbCamState.Properties.BeginInit();
      this.tbConnectionString.Properties.BeginInit();
      this.cbNames.Properties.BeginInit();
      this.cbCameraType.Properties.BeginInit();
      this.groupControl2.BeginInit();
      this.groupControl2.SuspendLayout();
      this.btIS.Properties.BeginInit();
      this.chbSaveNonCategory.Properties.BeginInit();
      this.cbMinFace.Properties.BeginInit();
      this.chbSaveImage.Properties.BeginInit();
      this.chbSaveUnidentified.Properties.BeginInit();
      this.tbDetectionScore.Properties.BeginInit();
      this.tbMinScore.Properties.BeginInit();
      this.tbExtractorCount.Properties.BeginInit();
      this.tbDetectorCount.Properties.BeginInit();
      this.btES.Properties.BeginInit();
      this.btDS.Properties.BeginInit();
      this.btVS.Properties.BeginInit();
      this.chbSaveFaces.Properties.BeginInit();
      this.groupBox2.BeginInit();
      this.groupBox2.SuspendLayout();
      this.cbObjects.Properties.BeginInit();
      this.tvObjects.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.groupControl3, "groupControl3");
      this.groupControl3.Controls.Add((Control) this.btLoadFile);
      this.groupControl3.Controls.Add((Control) this.tbName);
      this.groupControl3.Controls.Add((Control) this.tbLogin);
      this.groupControl3.Controls.Add((Control) this.tbPassword);
      this.groupControl3.Controls.Add((Control) this.cbCamState);
      this.groupControl3.Controls.Add((Control) this.label6);
      this.groupControl3.Controls.Add((Control) this.labelControl2);
      this.groupControl3.Controls.Add((Control) this.labelControl3);
      this.groupControl3.Controls.Add((Control) this.labelControl4);
      this.groupControl3.Controls.Add((Control) this.tbConnectionString);
      this.groupControl3.Controls.Add((Control) this.label1);
      this.groupControl3.Controls.Add((Control) this.lbConnectionString);
      this.groupControl3.Controls.Add((Control) this.btLoadDevice);
      this.groupControl3.Controls.Add((Control) this.cbNames);
      this.groupControl3.Controls.Add((Control) this.cbCameraType);
      this.groupControl3.Name = "groupControl3";
      componentResourceManager.ApplyResources((object) this.btLoadFile, "btLoadFile");
      this.btLoadFile.Appearance.Options.UseTextOptions = true;
      this.btLoadFile.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
      this.btLoadFile.Appearance.TextOptions.VAlignment = VertAlignment.Center;
      this.btLoadFile.Name = "btLoadFile";
      this.btLoadFile.Click += new EventHandler(this.btLoadFile_Click);
      componentResourceManager.ApplyResources((object) this.tbName, "tbName");
      this.tbName.Name = "tbName";
      this.tbName.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbName.Properties.AutoHeight");
      this.tbName.Properties.Mask.EditMask = componentResourceManager.GetString("tbName.Properties.Mask.EditMask");
      this.tbName.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbName.Properties.Mask.IgnoreMaskBlank");
      this.tbName.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbName.Properties.Mask.SaveLiteral");
      this.tbName.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbName.Properties.Mask.ShowPlaceHolders");
      this.tbName.Properties.NullValuePrompt = componentResourceManager.GetString("tbName.Properties.NullValuePrompt");
      this.tbName.EditValueChanged += new EventHandler(this.EditControl_EditValueChanged);
      componentResourceManager.ApplyResources((object) this.tbLogin, "tbLogin");
      this.tbLogin.Name = "tbLogin";
      this.tbLogin.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbLogin.Properties.AutoHeight");
      this.tbLogin.Properties.Mask.EditMask = componentResourceManager.GetString("tbLogin.Properties.Mask.EditMask");
      this.tbLogin.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.IgnoreMaskBlank");
      this.tbLogin.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.SaveLiteral");
      this.tbLogin.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbLogin.Properties.Mask.ShowPlaceHolders");
      this.tbLogin.Properties.NullValuePrompt = componentResourceManager.GetString("tbLogin.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.tbPassword, "tbPassword");
      this.tbPassword.Name = "tbPassword";
      this.tbPassword.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbPassword.Properties.AutoHeight");
      this.tbPassword.Properties.Mask.EditMask = componentResourceManager.GetString("tbPassword.Properties.Mask.EditMask");
      this.tbPassword.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.IgnoreMaskBlank");
      this.tbPassword.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.SaveLiteral");
      this.tbPassword.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbPassword.Properties.Mask.ShowPlaceHolders");
      this.tbPassword.Properties.NullValuePrompt = componentResourceManager.GetString("tbPassword.Properties.NullValuePrompt");
      this.tbPassword.Properties.UseSystemPasswordChar = true;
      componentResourceManager.ApplyResources((object) this.cbCamState, "cbCamState");
      this.cbCamState.Name = "cbCamState";
      this.cbCamState.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbCamState.Properties.AutoHeight");
      this.cbCamState.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbCamState.Properties.Buttons"))
      });
      this.cbCamState.Properties.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cbCamState.Properties.Items"),
        (object) componentResourceManager.GetString("cbCamState.Properties.Items1")
      });
      this.cbCamState.Properties.NullValuePrompt = componentResourceManager.GetString("cbCamState.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.labelControl2, "labelControl2");
      this.labelControl2.Name = "labelControl2";
      componentResourceManager.ApplyResources((object) this.labelControl3, "labelControl3");
      this.labelControl3.Name = "labelControl3";
      componentResourceManager.ApplyResources((object) this.labelControl4, "labelControl4");
      this.labelControl4.Name = "labelControl4";
      componentResourceManager.ApplyResources((object) this.tbConnectionString, "tbConnectionString");
      this.tbConnectionString.Name = "tbConnectionString";
      this.tbConnectionString.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbConnectionString.Properties.AutoHeight");
      this.tbConnectionString.Properties.Mask.EditMask = componentResourceManager.GetString("tbConnectionString.Properties.Mask.EditMask");
      this.tbConnectionString.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbConnectionString.Properties.Mask.IgnoreMaskBlank");
      this.tbConnectionString.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbConnectionString.Properties.Mask.SaveLiteral");
      this.tbConnectionString.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbConnectionString.Properties.Mask.ShowPlaceHolders");
      this.tbConnectionString.Properties.NullValuePrompt = componentResourceManager.GetString("tbConnectionString.Properties.NullValuePrompt");
      this.tbConnectionString.EditValueChanged += new EventHandler(this.EditControl_EditValueChanged);
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.lbConnectionString, "lbConnectionString");
      this.lbConnectionString.Name = "lbConnectionString";
      componentResourceManager.ApplyResources((object) this.btLoadDevice, "btLoadDevice");
      this.btLoadDevice.Name = "btLoadDevice";
      this.btLoadDevice.Click += new EventHandler(this.btWebCamProperties_Click);
      componentResourceManager.ApplyResources((object) this.cbNames, "cbNames");
      this.cbNames.Name = "cbNames";
      this.cbNames.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbNames.Properties.AutoHeight");
      this.cbNames.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbNames.Properties.Buttons"))
      });
      this.cbNames.Properties.NullValuePrompt = componentResourceManager.GetString("cbNames.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.cbCameraType, "cbCameraType");
      this.cbCameraType.Name = "cbCameraType";
      this.cbCameraType.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbCameraType.Properties.AutoHeight");
      this.cbCameraType.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbCameraType.Properties.Buttons"))
      });
      this.cbCameraType.Properties.NullText = componentResourceManager.GetString("cbCameraType.Properties.NullText");
      this.cbCameraType.Properties.NullValuePrompt = componentResourceManager.GetString("cbCameraType.Properties.NullValuePrompt");
      this.cbCameraType.Properties.PopupSizeable = false;
      this.cbCameraType.Properties.ShowFooter = false;
      this.cbCameraType.Properties.ShowHeader = false;
      this.cbCameraType.Properties.TextEditStyle = TextEditStyles.Standard;
      this.cbCameraType.EditValueChanged += new EventHandler(this.EditControl_EditValueChanged);
      componentResourceManager.ApplyResources((object) this.groupControl2, "groupControl2");
      this.groupControl2.Controls.Add((Control) this.labelControl18);
      this.groupControl2.Controls.Add((Control) this.btIS);
      this.groupControl2.Controls.Add((Control) this.chbSaveNonCategory);
      this.groupControl2.Controls.Add((Control) this.labelControl16);
      this.groupControl2.Controls.Add((Control) this.cbMinFace);
      this.groupControl2.Controls.Add((Control) this.chbSaveImage);
      this.groupControl2.Controls.Add((Control) this.chbSaveUnidentified);
      this.groupControl2.Controls.Add((Control) this.labelControl14);
      this.groupControl2.Controls.Add((Control) this.tbDetectionScore);
      this.groupControl2.Controls.Add((Control) this.labelControl13);
      this.groupControl2.Controls.Add((Control) this.tbMinScore);
      this.groupControl2.Controls.Add((Control) this.labelControl11);
      this.groupControl2.Controls.Add((Control) this.tbExtractorCount);
      this.groupControl2.Controls.Add((Control) this.labelControl12);
      this.groupControl2.Controls.Add((Control) this.tbDetectorCount);
      this.groupControl2.Controls.Add((Control) this.labelControl8);
      this.groupControl2.Controls.Add((Control) this.btES);
      this.groupControl2.Controls.Add((Control) this.labelControl7);
      this.groupControl2.Controls.Add((Control) this.btDS);
      this.groupControl2.Controls.Add((Control) this.labelControl6);
      this.groupControl2.Controls.Add((Control) this.btVS);
      this.groupControl2.Controls.Add((Control) this.chbSaveFaces);
      this.groupControl2.Name = "groupControl2";
      componentResourceManager.ApplyResources((object) this.labelControl18, "labelControl18");
      this.labelControl18.Name = "labelControl18";
      componentResourceManager.ApplyResources((object) this.btIS, "btIS");
      this.btIS.Name = "btIS";
      this.btIS.Properties.AutoHeight = (bool) componentResourceManager.GetObject("btIS.Properties.AutoHeight");
      this.btIS.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btIS.Properties.Mask.EditMask = componentResourceManager.GetString("btIS.Properties.Mask.EditMask");
      this.btIS.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("btIS.Properties.Mask.IgnoreMaskBlank");
      this.btIS.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("btIS.Properties.Mask.SaveLiteral");
      this.btIS.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("btIS.Properties.Mask.ShowPlaceHolders");
      this.btIS.Properties.NullValuePrompt = componentResourceManager.GetString("btIS.Properties.NullValuePrompt");
      this.btIS.ButtonClick += new ButtonPressedEventHandler(this.btIS_ButtonClick);
      componentResourceManager.ApplyResources((object) this.chbSaveNonCategory, "chbSaveNonCategory");
      this.chbSaveNonCategory.Name = "chbSaveNonCategory";
      this.chbSaveNonCategory.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbSaveNonCategory.Properties.AutoHeight");
      this.chbSaveNonCategory.Properties.Caption = componentResourceManager.GetString("chbSaveNonCategory.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.labelControl16, "labelControl16");
      this.labelControl16.Name = "labelControl16";
      componentResourceManager.ApplyResources((object) this.cbMinFace, "cbMinFace");
      this.cbMinFace.Name = "cbMinFace";
      this.cbMinFace.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbMinFace.Properties.AutoHeight");
      this.cbMinFace.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbMinFace.Properties.Buttons"))
      });
      this.cbMinFace.Properties.Items.AddRange(new object[4]
      {
        (object) componentResourceManager.GetString("cbMinFace.Properties.Items"),
        (object) componentResourceManager.GetString("cbMinFace.Properties.Items1"),
        (object) componentResourceManager.GetString("cbMinFace.Properties.Items2"),
        (object) componentResourceManager.GetString("cbMinFace.Properties.Items3")
      });
      this.cbMinFace.Properties.NullValuePrompt = componentResourceManager.GetString("cbMinFace.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.chbSaveImage, "chbSaveImage");
      this.chbSaveImage.Name = "chbSaveImage";
      this.chbSaveImage.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbSaveImage.Properties.AutoHeight");
      this.chbSaveImage.Properties.Caption = componentResourceManager.GetString("chbSaveImage.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.chbSaveUnidentified, "chbSaveUnidentified");
      this.chbSaveUnidentified.Name = "chbSaveUnidentified";
      this.chbSaveUnidentified.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbSaveUnidentified.Properties.AutoHeight");
      this.chbSaveUnidentified.Properties.Caption = componentResourceManager.GetString("chbSaveUnidentified.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.labelControl14, "labelControl14");
      this.labelControl14.Name = "labelControl14";
      componentResourceManager.ApplyResources((object) this.tbDetectionScore, "tbDetectionScore");
      this.tbDetectionScore.Name = "tbDetectionScore";
      this.tbDetectionScore.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbDetectionScore.Properties.AutoHeight");
      this.tbDetectionScore.Properties.Mask.EditMask = componentResourceManager.GetString("tbDetectionScore.Properties.Mask.EditMask");
      this.tbDetectionScore.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbDetectionScore.Properties.Mask.IgnoreMaskBlank");
      this.tbDetectionScore.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbDetectionScore.Properties.Mask.MaskType");
      this.tbDetectionScore.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbDetectionScore.Properties.Mask.SaveLiteral");
      this.tbDetectionScore.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbDetectionScore.Properties.Mask.ShowPlaceHolders");
      this.tbDetectionScore.Properties.NullValuePrompt = componentResourceManager.GetString("tbDetectionScore.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.labelControl13, "labelControl13");
      this.labelControl13.Name = "labelControl13";
      componentResourceManager.ApplyResources((object) this.tbMinScore, "tbMinScore");
      this.tbMinScore.Name = "tbMinScore";
      this.tbMinScore.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbMinScore.Properties.AutoHeight");
      this.tbMinScore.Properties.Mask.EditMask = componentResourceManager.GetString("tbMinScore.Properties.Mask.EditMask");
      this.tbMinScore.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbMinScore.Properties.Mask.IgnoreMaskBlank");
      this.tbMinScore.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbMinScore.Properties.Mask.MaskType");
      this.tbMinScore.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbMinScore.Properties.Mask.SaveLiteral");
      this.tbMinScore.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbMinScore.Properties.Mask.ShowPlaceHolders");
      this.tbMinScore.Properties.NullValuePrompt = componentResourceManager.GetString("tbMinScore.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.labelControl11, "labelControl11");
      this.labelControl11.Name = "labelControl11";
      componentResourceManager.ApplyResources((object) this.tbExtractorCount, "tbExtractorCount");
      this.tbExtractorCount.Name = "tbExtractorCount";
      this.tbExtractorCount.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbExtractorCount.Properties.AutoHeight");
      this.tbExtractorCount.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.tbExtractorCount.Properties.Mask.EditMask = componentResourceManager.GetString("tbExtractorCount.Properties.Mask.EditMask");
      this.tbExtractorCount.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbExtractorCount.Properties.Mask.IgnoreMaskBlank");
      this.tbExtractorCount.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbExtractorCount.Properties.Mask.MaskType");
      this.tbExtractorCount.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbExtractorCount.Properties.Mask.SaveLiteral");
      this.tbExtractorCount.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbExtractorCount.Properties.Mask.ShowPlaceHolders");
      this.tbExtractorCount.Properties.NullValuePrompt = componentResourceManager.GetString("tbExtractorCount.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.labelControl12, "labelControl12");
      this.labelControl12.Name = "labelControl12";
      componentResourceManager.ApplyResources((object) this.tbDetectorCount, "tbDetectorCount");
      this.tbDetectorCount.Name = "tbDetectorCount";
      this.tbDetectorCount.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbDetectorCount.Properties.AutoHeight");
      this.tbDetectorCount.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.tbDetectorCount.Properties.Mask.EditMask = componentResourceManager.GetString("tbDetectorCount.Properties.Mask.EditMask");
      this.tbDetectorCount.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbDetectorCount.Properties.Mask.IgnoreMaskBlank");
      this.tbDetectorCount.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbDetectorCount.Properties.Mask.MaskType");
      this.tbDetectorCount.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbDetectorCount.Properties.Mask.SaveLiteral");
      this.tbDetectorCount.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbDetectorCount.Properties.Mask.ShowPlaceHolders");
      this.tbDetectorCount.Properties.NullValuePrompt = componentResourceManager.GetString("tbDetectorCount.Properties.NullValuePrompt");
      componentResourceManager.ApplyResources((object) this.labelControl8, "labelControl8");
      this.labelControl8.Name = "labelControl8";
      componentResourceManager.ApplyResources((object) this.btES, "btES");
      this.btES.Name = "btES";
      this.btES.Properties.AutoHeight = (bool) componentResourceManager.GetObject("btES.Properties.AutoHeight");
      this.btES.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btES.Properties.Mask.EditMask = componentResourceManager.GetString("btES.Properties.Mask.EditMask");
      this.btES.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("btES.Properties.Mask.IgnoreMaskBlank");
      this.btES.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("btES.Properties.Mask.SaveLiteral");
      this.btES.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("btES.Properties.Mask.ShowPlaceHolders");
      this.btES.Properties.NullValuePrompt = componentResourceManager.GetString("btES.Properties.NullValuePrompt");
      this.btES.ButtonClick += new ButtonPressedEventHandler(this.btES_ButtonClick);
      componentResourceManager.ApplyResources((object) this.labelControl7, "labelControl7");
      this.labelControl7.Name = "labelControl7";
      componentResourceManager.ApplyResources((object) this.btDS, "btDS");
      this.btDS.Name = "btDS";
      this.btDS.Properties.AutoHeight = (bool) componentResourceManager.GetObject("btDS.Properties.AutoHeight");
      this.btDS.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btDS.Properties.Mask.EditMask = componentResourceManager.GetString("btDS.Properties.Mask.EditMask");
      this.btDS.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("btDS.Properties.Mask.IgnoreMaskBlank");
      this.btDS.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("btDS.Properties.Mask.SaveLiteral");
      this.btDS.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("btDS.Properties.Mask.ShowPlaceHolders");
      this.btDS.Properties.NullValuePrompt = componentResourceManager.GetString("btDS.Properties.NullValuePrompt");
      this.btDS.ButtonClick += new ButtonPressedEventHandler(this.btDS_ButtonClick);
      componentResourceManager.ApplyResources((object) this.labelControl6, "labelControl6");
      this.labelControl6.Name = "labelControl6";
      componentResourceManager.ApplyResources((object) this.btVS, "btVS");
      this.btVS.Name = "btVS";
      this.btVS.Properties.AutoHeight = (bool) componentResourceManager.GetObject("btVS.Properties.AutoHeight");
      this.btVS.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.btVS.Properties.Mask.EditMask = componentResourceManager.GetString("btVS.Properties.Mask.EditMask");
      this.btVS.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("btVS.Properties.Mask.IgnoreMaskBlank");
      this.btVS.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("btVS.Properties.Mask.SaveLiteral");
      this.btVS.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("btVS.Properties.Mask.ShowPlaceHolders");
      this.btVS.Properties.NullValuePrompt = componentResourceManager.GetString("btVS.Properties.NullValuePrompt");
      this.btVS.ButtonClick += new ButtonPressedEventHandler(this.btVS_ButtonClick);
      componentResourceManager.ApplyResources((object) this.chbSaveFaces, "chbSaveFaces");
      this.chbSaveFaces.Name = "chbSaveFaces";
      this.chbSaveFaces.Properties.AutoHeight = (bool) componentResourceManager.GetObject("chbSaveFaces.Properties.AutoHeight");
      this.chbSaveFaces.Properties.Caption = componentResourceManager.GetString("chbSaveFaces.Properties.Caption");
      componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
      this.groupBox2.Controls.Add((Control) this.cbObjects);
      this.groupBox2.Controls.Add((Control) this.tvObjects);
      this.groupBox2.LookAndFeel.UseDefaultLookAndFeel = false;
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.TabStop = true;
      componentResourceManager.ApplyResources((object) this.cbObjects, "cbObjects");
      this.cbObjects.Name = "cbObjects";
      this.cbObjects.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbObjects.Properties.AutoHeight");
      this.cbObjects.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbObjects.Properties.Buttons"))
      });
      this.cbObjects.Properties.NullValuePrompt = componentResourceManager.GetString("cbObjects.Properties.NullValuePrompt");
      this.cbObjects.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
      this.cbObjects.SelectedIndexChanged += new EventHandler(this.cbObjects_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.tvObjects, "tvObjects");
      this.tvObjects.Columns.AddRange(new TreeListColumn[1]
      {
        this.treeListColumn1
      });
      this.tvObjects.Name = "tvObjects";
      this.tvObjects.OptionsBehavior.Editable = false;
      this.tvObjects.OptionsSelection.EnableAppearanceFocusedCell = false;
      this.tvObjects.OptionsSelection.MultiSelect = true;
      this.tvObjects.OptionsView.ShowCheckBoxes = true;
      this.tvObjects.BeforeCheckNode += new CheckNodeEventHandler(this.tvObjects_BeforeCheckNode);
      this.tvObjects.AfterCheckNode += new NodeEventHandler(this.treeList1_AfterCheckNode);
      componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
      this.treeListColumn1.FieldName = "treeListColumn1";
      this.treeListColumn1.Name = "treeListColumn1";
      componentResourceManager.ApplyResources((object) this.btSave, "btSave");
      this.btSave.DialogResult = DialogResult.OK;
      this.btSave.Name = "btSave";
      this.btSave.Click += new EventHandler(this.btSave_Click);
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupControl2);
      this.Controls.Add((Control) this.groupControl3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btSave);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = "FrmEditDevice";
      this.Load += new EventHandler(this.frmEditDevice_Load);
      this.groupControl3.EndInit();
      this.groupControl3.ResumeLayout(false);
      this.groupControl3.PerformLayout();
      this.tbName.Properties.EndInit();
      this.tbLogin.Properties.EndInit();
      this.tbPassword.Properties.EndInit();
      this.cbCamState.Properties.EndInit();
      this.tbConnectionString.Properties.EndInit();
      this.cbNames.Properties.EndInit();
      this.cbCameraType.Properties.EndInit();
      this.groupControl2.EndInit();
      this.groupControl2.ResumeLayout(false);
      this.groupControl2.PerformLayout();
      this.btIS.Properties.EndInit();
      this.chbSaveNonCategory.Properties.EndInit();
      this.cbMinFace.Properties.EndInit();
      this.chbSaveImage.Properties.EndInit();
      this.chbSaveUnidentified.Properties.EndInit();
      this.tbDetectionScore.Properties.EndInit();
      this.tbMinScore.Properties.EndInit();
      this.tbExtractorCount.Properties.EndInit();
      this.tbDetectorCount.Properties.EndInit();
      this.btES.Properties.EndInit();
      this.btDS.Properties.EndInit();
      this.btVS.Properties.EndInit();
      this.chbSaveFaces.Properties.EndInit();
      this.groupBox2.EndInit();
      this.groupBox2.ResumeLayout(false);
      this.cbObjects.Properties.EndInit();
      this.tvObjects.EndInit();
      this.ResumeLayout(false);
    }
  }
}
