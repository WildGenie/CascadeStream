// Decompiled with JetBrains decompiler
// Type: CascadeFlowImport.FrmImport
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BasicComponents;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Mask;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Image = System.Drawing.Image;
using Rectangle = TS.Core.Model.Rectangle;

namespace CascadeManager.CascadeFlowClient
{
  public class FrmImport : XtraForm
  {
    public List<string> Errors = new List<string>();
    public List<string> ErrorFiles = new List<string>();
    public List<Bitmap> ErrorPictures = new List<Bitmap>();
    public List<string> ErrorComments = new List<string>();
    private List<BcAccessCategory> _categories = new List<BcAccessCategory>();
    public List<FaceRecognition> Detectors = new List<FaceRecognition>();
    public bool StopImport = false;
    private List<string> _commonFiles = new List<string>();
    public string CommonComment = "";
    private string _selectedDb = "";
    private int _selectedCategoryId = -1;
    private string _currentDir = "";
    private IContainer components = null;
    public bool StopImportFile;
    private int _progVal;
    private int _mainIndex;
    public bool Sleepfile;
    public int CommonSex;
    private bool _runningFile;
    private Thread _mainThreadFile;
    private TextEdit tbDirectory;
    private LabelControl lbDirectory;
    private SimpleButton btOpenDir;
    private LabelControl label3;
    private ComboBoxEdit cbAccessTemplate;
    private LabelControl lbComment;
    private LabelControl lbSex;
    private ComboBoxEdit cbSEX;
    private GroupBox groupBox1;
    private PictureBox pbImage;
    private LabelControl lbAdditional;
    private SimpleButton btStart;
    private SimpleButton btPause;
    private SimpleButton btStop;
    private LabelControl lbState;
    private LabelControl lbCount;
    private ProgressBarControl progressBarControl1;
    private MemoEdit tbComment;

    public FrmImport()
    {
      InitializeComponent();
      _categories = BcAccessCategory.LoadAll();
      foreach (BcAccessCategory bcAccessCategory in _categories)
        cbAccessTemplate.Properties.Items.Add(bcAccessCategory.Name);
    }

    public FaceRecognition GetFreeHandle()
    {
      foreach (FaceRecognition faceRecognition in Detectors)
      {
        if (!faceRecognition.IsWork)
        {
          faceRecognition.IsWork = true;
          return faceRecognition;
        }
      }
      return null;
    }

    public void EndSaveFace(IAsyncResult res)
    {
    }

    public void SaveFace(string str, FaceRecognition rec)
    {
      try
      {
        BcFace bcFace = new BcFace();
        string[] strArray = str.Replace(_currentDir + "\\", "").ToUpper().Replace(".JPEG", "").Replace(".JPG", "").Replace(".BMP", "").Replace(".GIF", "").Replace(".PNG", "").Replace(".TIFF", "").Split(new string[1]
        {
          " "
        }, StringSplitOptions.RemoveEmptyEntries);
        bcFace.Id = Guid.Empty;
        if (strArray.Length > 1)
          bcFace.FirstName = strArray[1];
        if (strArray.Length > 0)
          bcFace.Surname = strArray[0];
        if (strArray.Length > 2)
          bcFace.LastName = strArray[2];
        Bitmap source = (Bitmap) Image.FromFile(str);
        TS.Sdk.StaticFace.Model.Image image = source.ConvertFrom();
        BcKey bcKey = new BcKey();
        FaceInfo face = rec.Engine.DetectMaxFace(image, null);
        if (face != null)
        {
          bcFace.Comment = CommonComment;
          bcFace.Sex = CommonSex;
          bcFace.AccessId = _selectedCategoryId;
          bcFace.EditUserId = MainForm.CurrentUser.Id;
          Rectangle faceRectangle = face.FaceRectangle;
          int width1 = (int) faceRectangle.Width * 2;
          faceRectangle = face.FaceRectangle;
          int height1 = (int) faceRectangle.Height * 2;
          Bitmap bitmap = new Bitmap(width1, height1);
          using (Graphics graphics = Graphics.FromImage(bitmap))
          {
            graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
            faceRectangle = face.FaceRectangle;
            double x1 = faceRectangle.X;
            faceRectangle = face.FaceRectangle;
            double num1 = faceRectangle.Width / 2.0;
            int x2 = (int) (x1 - num1);
            double num2 = source.Height;
            faceRectangle = face.FaceRectangle;
            double y1 = faceRectangle.Y;
            double num3 = num2 - y1;
            faceRectangle = face.FaceRectangle;
            double height2 = faceRectangle.Height;
            double num4 = num3 - height2;
            faceRectangle = face.FaceRectangle;
            double num5 = faceRectangle.Width / 2.0;
            int y2 = (int) (num4 - num5);
            int width2 = bitmap.Width;
            int height3 = bitmap.Height;
            graphics.DrawImage(source, new System.Drawing.Rectangle(0, 0, width2, height3), new System.Drawing.Rectangle(x2, y2, width2, height3), GraphicsUnit.Pixel);
          }
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          bitmap.Save(memoryStream1, ImageFormat.Jpeg);
          BcImage bcImage = new BcImage();
          if (bitmap.Width >= 300)
          {
            bitmap = new Bitmap(bitmap, new Size(bitmap.Width / 2, bitmap.Height / 2));
            bitmap.Save(memoryStream2, ImageFormat.Jpeg);
            bcFace.ImageIcon = memoryStream2.GetBuffer();
            bcImage.ImageIcon = memoryStream2.GetBuffer();
          }
          else
          {
            bcFace.ImageIcon = memoryStream1.GetBuffer();
            bcImage.ImageIcon = memoryStream1.GetBuffer();
          }
          bcFace.Save();
          bcImage.FaceId = bcFace.Id;
          bcImage.Image = memoryStream1.GetBuffer();
          bcImage.FaceId = bcFace.Id;
          bcImage.Save();
          bitmap.Dispose();
          memoryStream1.Close();
          memoryStream1.Dispose();
          memoryStream2.Close();
          memoryStream2.Dispose();
          bcKey.ImageKey = rec.Engine.ExtractTemplate(image, face);
          if (bcKey.ImageKey != null)
          {
            bcKey.Ksid = -1;
            bcKey.ImageId = bcImage.Id;
            bcKey.FaceId = bcFace.Id;
            bcKey.Save();
          }
          else
          {
            Errors.Add("Error to create template");
            ErrorFiles.Add(str);
          }
        }
        else
        {
          Errors.Add(Messages.NoFaceWasFound);
          ErrorFiles.Add(str);
        }
        source.Dispose();
      }
      catch (Exception ex)
      {
        Errors.Add(ex.Message);
        ErrorFiles.Add(str);
      }
      rec.IsWork = false;
      try
      {
        Invoke(new NewValueFileFunc(NewValueFile), (object) _progVal, (object) _mainIndex, (object) _commonFiles.Count);
      }
      catch
      {
      }
    }

    public void GetFiles(string path)
    {
      _commonFiles.AddRange(Directory.GetFiles(path));
      foreach (string path1 in Directory.GetDirectories(path))
        GetFiles(path1);
    }

    public bool WaitAll()
    {
      bool flag;
      do
      {
        flag = false;
        for (int index = 0; index < Detectors.Count; ++index)
        {
          if (Detectors[index].IsWork)
            flag = true;
        }
      }
      while (flag);
      return false;
    }

    public void Saving()
    {
      try
      {
        StopImportFile = false;
        _currentDir = tbDirectory.Text;
        _commonFiles.Clear();
        _commonFiles = new List<string>();
        _runningFile = true;
        _progVal = 0;
        _mainIndex = 0;
        GetFiles(tbDirectory.Text);
        Invoke(new StartFileFunc(StartFile), (object) _commonFiles.Count);
        for (int index = 0; index < Environment.ProcessorCount - 1; ++index)
          Detectors.Add(new FaceRecognition());
        foreach (string str in _commonFiles)
        {
          if (!StopImportFile)
          {
            if (Sleepfile)
            {
              while (Sleepfile)
                Thread.Sleep(1500);
            }
            try
            {
              Invoke(new SetImageFileFunc(SetImageFile), (object) new Bitmap(str));
            }
            catch
            {
            }
            FaceRecognition freeHandle;
            while ((freeHandle = GetFreeHandle()) == null)
              Thread.Sleep(10);
            SaveFaceFunc saveFaceFunc = SaveFace;
            saveFaceFunc.BeginInvoke(str, freeHandle, EndSaveFace, saveFaceFunc);
            Invoke(new NewValueFileFunc(NewValueFile), (object) _progVal, (object) _mainIndex, (object) _commonFiles.Count);
            ++_mainIndex;
            ++_progVal;
          }
          else
            break;
        }
      }
      catch (Exception ex)
      {
        ErrorFiles.Add("");
        Errors.Add(ex.Message);
        try
        {
          if (!IsDisposed)
            Invoke(new ErrorFunc(ErrorMessage), (object) ex.Message);
        }
        catch
        {
        }
      }
      WaitAll();
      for (int index = 0; index < Detectors.Count; ++index)
        Detectors[index].Destroy();
      Detectors.Clear();
      if (!IsDisposed)
      {
        try
        {
          if (Errors.Count > 0)
            Invoke(new EndFunc(CloseImportFile), (object) Messages.ErrorImport);
          else
            Invoke(new EndFunc(CloseImportFile), (object) Messages.ImportComplete);
        }
        catch
        {
        }
      }
      _runningFile = false;
    }

    private void btStartFile_Click(object sender, EventArgs e)
    {
      if (!_runningFile)
      {
        if (cbAccessTemplate.SelectedIndex != -1)
        {
          _selectedDb = cbAccessTemplate.Text;
          _selectedCategoryId = _categories[cbAccessTemplate.SelectedIndex].Id;
          _mainThreadFile = new Thread(Saving)
          {
            IsBackground = true
          };
          _mainThreadFile.Start();
          btStart.Text = Messages.Start;
          btStart.Enabled = false;
          btStop.Enabled = true;
          btPause.Enabled = true;
          CommonComment = tbComment.Text;
          if (cbSEX.SelectedIndex == -1)
            return;
          CommonSex = cbSEX.SelectedIndex;
        }
        else
        {
          int num = (int) XtraMessageBox.Show(Messages.SetCategory, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      else
      {
        Sleepfile = false;
        _runningFile = true;
        btStart.Enabled = false;
        btStop.Enabled = true;
        btPause.Enabled = true;
      }
    }

    private void btPauseFile_Click(object sender, EventArgs e)
    {
      if (!_runningFile)
        return;
      btStart.Enabled = true;
      btPause.Enabled = false;
      Sleepfile = true;
      btStart.Text = Messages.Continue;
    }

    private void btStopFile_Click(object sender, EventArgs e)
    {
      if (!_runningFile || XtraMessageBox.Show(Messages.DouYouWantToStopProcess, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      _runningFile = false;
      Sleepfile = false;
      btStart.Text = "Начать";
      btStart.Enabled = false;
      btStop.Enabled = true;
      btOpenDir.Enabled = true;
      btPause.Enabled = true;
      StopImportFile = true;
      CloseImportFile(Messages.OperationComplete);
    }

    private void btOpenDir_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      tbDirectory.Text = folderBrowserDialog.SelectedPath;
      _currentDir = folderBrowserDialog.SelectedPath;
    }

    private void SetImageFile(Image img)
    {
      try
      {
        pbImage.Image.Dispose();
      }
      catch
      {
      }
      try
      {
        pbImage.Image = img;
      }
      catch
      {
      }
    }

    private void StartFile(int count)
    {
      lbCount.Visible = true;
      progressBarControl1.Properties.Maximum = count;
      progressBarControl1.EditValue = 0;
    }

    private void CloseImportFile(string message)
    {
      lbCount.Visible = true;
      btStart.Enabled = true;
      btPause.Enabled = false;
      btStop.Enabled = false;
      if (Errors.Count > 0)
      {
        int num1 = (int) XtraMessageBox.Show(message, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        int num2 = (int) new FrmErrorList(this).ShowDialog();
        Errors.Clear();
        ErrorComments.Clear();
        ErrorFiles.Clear();
      }
      else
      {
        int num = (int) XtraMessageBox.Show(message, Messages.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void NewValueFile(int progressValue, int count, int max)
    {
      try
      {
        lbCount.Text = (string) (object) count + (object) " из " + (string) (object) max;
        progressBarControl1.EditValue = progressValue;
      }
      catch
      {
      }
    }

    private void ErrorMessage(string error)
    {
      int num = (int) XtraMessageBox.Show(error + "\r\n" + Messages.OperationComplete, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private void frmImport_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!_runningFile || XtraMessageBox.Show(Messages.DouYouWantToStopProcess, Messages.Message, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      StopImportFile = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmImport));
      tbDirectory = new TextEdit();
      lbDirectory = new LabelControl();
      btOpenDir = new SimpleButton();
      label3 = new LabelControl();
      cbAccessTemplate = new ComboBoxEdit();
      lbComment = new LabelControl();
      lbSex = new LabelControl();
      cbSEX = new ComboBoxEdit();
      groupBox1 = new GroupBox();
      tbComment = new MemoEdit();
      pbImage = new PictureBox();
      lbAdditional = new LabelControl();
      btStart = new SimpleButton();
      btPause = new SimpleButton();
      btStop = new SimpleButton();
      lbState = new LabelControl();
      lbCount = new LabelControl();
      progressBarControl1 = new ProgressBarControl();
      tbDirectory.Properties.BeginInit();
      cbAccessTemplate.Properties.BeginInit();
      cbSEX.Properties.BeginInit();
      groupBox1.SuspendLayout();
      tbComment.Properties.BeginInit();
      ((ISupportInitialize) pbImage).BeginInit();
      progressBarControl1.Properties.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources(tbDirectory, "tbDirectory");
      tbDirectory.Name = "tbDirectory";
      tbDirectory.Properties.AccessibleDescription = componentResourceManager.GetString("tbDirectory.Properties.AccessibleDescription");
      tbDirectory.Properties.AccessibleName = componentResourceManager.GetString("tbDirectory.Properties.AccessibleName");
      tbDirectory.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("tbDirectory.Properties.Appearance.Font");
      tbDirectory.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("tbDirectory.Properties.Appearance.FontSizeDelta");
      tbDirectory.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("tbDirectory.Properties.Appearance.FontStyleDelta");
      tbDirectory.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("tbDirectory.Properties.Appearance.GradientMode");
      tbDirectory.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("tbDirectory.Properties.Appearance.Image");
      tbDirectory.Properties.Appearance.Options.UseFont = true;
      tbDirectory.Properties.AutoHeight = (bool) componentResourceManager.GetObject("tbDirectory.Properties.AutoHeight");
      tbDirectory.Properties.Mask.AutoComplete = (AutoCompleteType) componentResourceManager.GetObject("tbDirectory.Properties.Mask.AutoComplete");
      tbDirectory.Properties.Mask.BeepOnError = (bool) componentResourceManager.GetObject("tbDirectory.Properties.Mask.BeepOnError");
      tbDirectory.Properties.Mask.EditMask = componentResourceManager.GetString("tbDirectory.Properties.Mask.EditMask");
      tbDirectory.Properties.Mask.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("tbDirectory.Properties.Mask.IgnoreMaskBlank");
      tbDirectory.Properties.Mask.MaskType = (MaskType) componentResourceManager.GetObject("tbDirectory.Properties.Mask.MaskType");
      tbDirectory.Properties.Mask.PlaceHolder = (char) componentResourceManager.GetObject("tbDirectory.Properties.Mask.PlaceHolder");
      tbDirectory.Properties.Mask.SaveLiteral = (bool) componentResourceManager.GetObject("tbDirectory.Properties.Mask.SaveLiteral");
      tbDirectory.Properties.Mask.ShowPlaceHolders = (bool) componentResourceManager.GetObject("tbDirectory.Properties.Mask.ShowPlaceHolders");
      tbDirectory.Properties.Mask.UseMaskAsDisplayFormat = (bool) componentResourceManager.GetObject("tbDirectory.Properties.Mask.UseMaskAsDisplayFormat");
      tbDirectory.Properties.NullValuePrompt = componentResourceManager.GetString("tbDirectory.Properties.NullValuePrompt");
      tbDirectory.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbDirectory.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(lbDirectory, "lbDirectory");
      lbDirectory.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbDirectory.Appearance.DisabledImage");
      lbDirectory.Appearance.Font = (Font) componentResourceManager.GetObject("lbDirectory.Appearance.Font");
      lbDirectory.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbDirectory.Appearance.FontSizeDelta");
      lbDirectory.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbDirectory.Appearance.FontStyleDelta");
      lbDirectory.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbDirectory.Appearance.GradientMode");
      lbDirectory.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbDirectory.Appearance.HoverImage");
      lbDirectory.Appearance.Image = (Image) componentResourceManager.GetObject("lbDirectory.Appearance.Image");
      lbDirectory.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbDirectory.Appearance.PressedImage");
      lbDirectory.Name = "lbDirectory";
      componentResourceManager.ApplyResources(btOpenDir, "btOpenDir");
      btOpenDir.Appearance.Font = (Font) componentResourceManager.GetObject("btOpenDir.Appearance.Font");
      btOpenDir.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btOpenDir.Appearance.FontSizeDelta");
      btOpenDir.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btOpenDir.Appearance.FontStyleDelta");
      btOpenDir.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btOpenDir.Appearance.GradientMode");
      btOpenDir.Appearance.Image = (Image) componentResourceManager.GetObject("btOpenDir.Appearance.Image");
      btOpenDir.Appearance.Options.UseFont = true;
      btOpenDir.Name = "btOpenDir";
      btOpenDir.Click += btOpenDir_Click;
      componentResourceManager.ApplyResources(label3, "label3");
      label3.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("label3.Appearance.DisabledImage");
      label3.Appearance.Font = (Font) componentResourceManager.GetObject("label3.Appearance.Font");
      label3.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("label3.Appearance.FontSizeDelta");
      label3.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("label3.Appearance.FontStyleDelta");
      label3.Appearance.ForeColor = (Color) componentResourceManager.GetObject("label3.Appearance.ForeColor");
      label3.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("label3.Appearance.GradientMode");
      label3.Appearance.HoverImage = (Image) componentResourceManager.GetObject("label3.Appearance.HoverImage");
      label3.Appearance.Image = (Image) componentResourceManager.GetObject("label3.Appearance.Image");
      label3.Appearance.PressedImage = (Image) componentResourceManager.GetObject("label3.Appearance.PressedImage");
      label3.Name = "label3";
      componentResourceManager.ApplyResources(cbAccessTemplate, "cbAccessTemplate");
      cbAccessTemplate.Name = "cbAccessTemplate";
      cbAccessTemplate.Properties.AccessibleDescription = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleDescription");
      cbAccessTemplate.Properties.AccessibleName = componentResourceManager.GetString("cbAccessTemplate.Properties.AccessibleName");
      cbAccessTemplate.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Font");
      cbAccessTemplate.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontSizeDelta");
      cbAccessTemplate.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.FontStyleDelta");
      cbAccessTemplate.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.GradientMode");
      cbAccessTemplate.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbAccessTemplate.Properties.Appearance.Image");
      cbAccessTemplate.Properties.Appearance.Options.UseFont = true;
      cbAccessTemplate.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.AutoHeight");
      cbAccessTemplate.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbAccessTemplate.Properties.Buttons"))
      });
      cbAccessTemplate.Properties.NullValuePrompt = componentResourceManager.GetString("cbAccessTemplate.Properties.NullValuePrompt");
      cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbAccessTemplate.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(lbComment, "lbComment");
      lbComment.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.DisabledImage");
      lbComment.Appearance.Font = (Font) componentResourceManager.GetObject("lbComment.Appearance.Font");
      lbComment.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbComment.Appearance.FontSizeDelta");
      lbComment.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbComment.Appearance.FontStyleDelta");
      lbComment.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbComment.Appearance.ForeColor");
      lbComment.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbComment.Appearance.GradientMode");
      lbComment.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.HoverImage");
      lbComment.Appearance.Image = (Image) componentResourceManager.GetObject("lbComment.Appearance.Image");
      lbComment.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbComment.Appearance.PressedImage");
      lbComment.Name = "lbComment";
      componentResourceManager.ApplyResources(lbSex, "lbSex");
      lbSex.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.DisabledImage");
      lbSex.Appearance.Font = (Font) componentResourceManager.GetObject("lbSex.Appearance.Font");
      lbSex.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbSex.Appearance.FontSizeDelta");
      lbSex.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbSex.Appearance.FontStyleDelta");
      lbSex.Appearance.ForeColor = (Color) componentResourceManager.GetObject("lbSex.Appearance.ForeColor");
      lbSex.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbSex.Appearance.GradientMode");
      lbSex.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.HoverImage");
      lbSex.Appearance.Image = (Image) componentResourceManager.GetObject("lbSex.Appearance.Image");
      lbSex.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbSex.Appearance.PressedImage");
      lbSex.Name = "lbSex";
      componentResourceManager.ApplyResources(cbSEX, "cbSEX");
      cbSEX.Name = "cbSEX";
      cbSEX.Properties.AccessibleDescription = componentResourceManager.GetString("cbSEX.Properties.AccessibleDescription");
      cbSEX.Properties.AccessibleName = componentResourceManager.GetString("cbSEX.Properties.AccessibleName");
      cbSEX.Properties.Appearance.Font = (Font) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Font");
      cbSEX.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontSizeDelta");
      cbSEX.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("cbSEX.Properties.Appearance.FontStyleDelta");
      cbSEX.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("cbSEX.Properties.Appearance.GradientMode");
      cbSEX.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("cbSEX.Properties.Appearance.Image");
      cbSEX.Properties.Appearance.Options.UseFont = true;
      cbSEX.Properties.AutoHeight = (bool) componentResourceManager.GetObject("cbSEX.Properties.AutoHeight");
      cbSEX.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton((ButtonPredefines) componentResourceManager.GetObject("cbSEX.Properties.Buttons"))
      });
      cbSEX.Properties.Items.AddRange(new object[2]
      {
        componentResourceManager.GetString("cbSEX.Properties.Items"),
        componentResourceManager.GetString("cbSEX.Properties.Items1")
      });
      cbSEX.Properties.NullValuePrompt = componentResourceManager.GetString("cbSEX.Properties.NullValuePrompt");
      cbSEX.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("cbSEX.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(groupBox1, "groupBox1");
      groupBox1.Controls.Add(tbComment);
      groupBox1.Controls.Add(cbSEX);
      groupBox1.Controls.Add(label3);
      groupBox1.Controls.Add(lbSex);
      groupBox1.Controls.Add(cbAccessTemplate);
      groupBox1.Controls.Add(lbComment);
      groupBox1.Name = "groupBox1";
      groupBox1.TabStop = false;
      componentResourceManager.ApplyResources(tbComment, "tbComment");
      tbComment.Name = "tbComment";
      tbComment.Properties.AccessibleDescription = componentResourceManager.GetString("tbComment.Properties.AccessibleDescription");
      tbComment.Properties.AccessibleName = componentResourceManager.GetString("tbComment.Properties.AccessibleName");
      tbComment.Properties.NullValuePrompt = componentResourceManager.GetString("tbComment.Properties.NullValuePrompt");
      tbComment.Properties.NullValuePromptShowForEmptyValue = (bool) componentResourceManager.GetObject("tbComment.Properties.NullValuePromptShowForEmptyValue");
      componentResourceManager.ApplyResources(pbImage, "pbImage");
      pbImage.BorderStyle = BorderStyle.FixedSingle;
      pbImage.Name = "pbImage";
      pbImage.TabStop = false;
      componentResourceManager.ApplyResources(lbAdditional, "lbAdditional");
      lbAdditional.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbAdditional.Appearance.DisabledImage");
      lbAdditional.Appearance.Font = (Font) componentResourceManager.GetObject("lbAdditional.Appearance.Font");
      lbAdditional.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbAdditional.Appearance.FontSizeDelta");
      lbAdditional.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbAdditional.Appearance.FontStyleDelta");
      lbAdditional.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbAdditional.Appearance.GradientMode");
      lbAdditional.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbAdditional.Appearance.HoverImage");
      lbAdditional.Appearance.Image = (Image) componentResourceManager.GetObject("lbAdditional.Appearance.Image");
      lbAdditional.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbAdditional.Appearance.PressedImage");
      lbAdditional.Name = "lbAdditional";
      componentResourceManager.ApplyResources(btStart, "btStart");
      btStart.Appearance.Font = (Font) componentResourceManager.GetObject("btStart.Appearance.Font");
      btStart.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btStart.Appearance.FontSizeDelta");
      btStart.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btStart.Appearance.FontStyleDelta");
      btStart.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btStart.Appearance.GradientMode");
      btStart.Appearance.Image = (Image) componentResourceManager.GetObject("btStart.Appearance.Image");
      btStart.Appearance.Options.UseFont = true;
      btStart.Name = "btStart";
      btStart.Click += btStartFile_Click;
      componentResourceManager.ApplyResources(btPause, "btPause");
      btPause.Appearance.Font = (Font) componentResourceManager.GetObject("btPause.Appearance.Font");
      btPause.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btPause.Appearance.FontSizeDelta");
      btPause.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btPause.Appearance.FontStyleDelta");
      btPause.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btPause.Appearance.GradientMode");
      btPause.Appearance.Image = (Image) componentResourceManager.GetObject("btPause.Appearance.Image");
      btPause.Appearance.Options.UseFont = true;
      btPause.Name = "btPause";
      btPause.Click += btPauseFile_Click;
      componentResourceManager.ApplyResources(btStop, "btStop");
      btStop.Appearance.Font = (Font) componentResourceManager.GetObject("btStop.Appearance.Font");
      btStop.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("btStop.Appearance.FontSizeDelta");
      btStop.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("btStop.Appearance.FontStyleDelta");
      btStop.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("btStop.Appearance.GradientMode");
      btStop.Appearance.Image = (Image) componentResourceManager.GetObject("btStop.Appearance.Image");
      btStop.Appearance.Options.UseFont = true;
      btStop.Name = "btStop";
      btStop.Click += btStopFile_Click;
      componentResourceManager.ApplyResources(lbState, "lbState");
      lbState.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbState.Appearance.DisabledImage");
      lbState.Appearance.Font = (Font) componentResourceManager.GetObject("lbState.Appearance.Font");
      lbState.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbState.Appearance.FontSizeDelta");
      lbState.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbState.Appearance.FontStyleDelta");
      lbState.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbState.Appearance.GradientMode");
      lbState.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbState.Appearance.HoverImage");
      lbState.Appearance.Image = (Image) componentResourceManager.GetObject("lbState.Appearance.Image");
      lbState.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbState.Appearance.PressedImage");
      lbState.Name = "lbState";
      componentResourceManager.ApplyResources(lbCount, "lbCount");
      lbCount.Appearance.DisabledImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.DisabledImage");
      lbCount.Appearance.Font = (Font) componentResourceManager.GetObject("lbCount.Appearance.Font");
      lbCount.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("lbCount.Appearance.FontSizeDelta");
      lbCount.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("lbCount.Appearance.FontStyleDelta");
      lbCount.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("lbCount.Appearance.GradientMode");
      lbCount.Appearance.HoverImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.HoverImage");
      lbCount.Appearance.Image = (Image) componentResourceManager.GetObject("lbCount.Appearance.Image");
      lbCount.Appearance.PressedImage = (Image) componentResourceManager.GetObject("lbCount.Appearance.PressedImage");
      lbCount.Name = "lbCount";
      componentResourceManager.ApplyResources(progressBarControl1, "progressBarControl1");
      progressBarControl1.Name = "progressBarControl1";
      progressBarControl1.Properties.AccessibleDescription = componentResourceManager.GetString("progressBarControl1.Properties.AccessibleDescription");
      progressBarControl1.Properties.AccessibleName = componentResourceManager.GetString("progressBarControl1.Properties.AccessibleName");
      progressBarControl1.Properties.Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("progressBarControl1.Properties.Appearance.FontSizeDelta");
      progressBarControl1.Properties.Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("progressBarControl1.Properties.Appearance.FontStyleDelta");
      progressBarControl1.Properties.Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBarControl1.Properties.Appearance.GradientMode");
      progressBarControl1.Properties.Appearance.Image = (Image) componentResourceManager.GetObject("progressBarControl1.Properties.Appearance.Image");
      progressBarControl1.Properties.AppearanceDisabled.FontSizeDelta = (int) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceDisabled.FontSizeDelta");
      progressBarControl1.Properties.AppearanceDisabled.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceDisabled.FontStyleDelta");
      progressBarControl1.Properties.AppearanceDisabled.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceDisabled.GradientMode");
      progressBarControl1.Properties.AppearanceDisabled.Image = (Image) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceDisabled.Image");
      progressBarControl1.Properties.AppearanceFocused.FontSizeDelta = (int) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceFocused.FontSizeDelta");
      progressBarControl1.Properties.AppearanceFocused.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceFocused.FontStyleDelta");
      progressBarControl1.Properties.AppearanceFocused.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceFocused.GradientMode");
      progressBarControl1.Properties.AppearanceFocused.Image = (Image) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceFocused.Image");
      progressBarControl1.Properties.AppearanceReadOnly.FontSizeDelta = (int) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceReadOnly.FontSizeDelta");
      progressBarControl1.Properties.AppearanceReadOnly.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceReadOnly.FontStyleDelta");
      progressBarControl1.Properties.AppearanceReadOnly.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceReadOnly.GradientMode");
      progressBarControl1.Properties.AppearanceReadOnly.Image = (Image) componentResourceManager.GetObject("progressBarControl1.Properties.AppearanceReadOnly.Image");
      progressBarControl1.Properties.AutoHeight = (bool) componentResourceManager.GetObject("progressBarControl1.Properties.AutoHeight");
      componentResourceManager.ApplyResources(this, "$this");
      Appearance.FontSizeDelta = (int) componentResourceManager.GetObject("frmImport.Appearance.FontSizeDelta");
      Appearance.FontStyleDelta = (FontStyle) componentResourceManager.GetObject("frmImport.Appearance.FontStyleDelta");
      Appearance.GradientMode = (LinearGradientMode) componentResourceManager.GetObject("frmImport.Appearance.GradientMode");
      Appearance.Image = (Image) componentResourceManager.GetObject("frmImport.Appearance.Image");
      Appearance.Options.UseFont = true;
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(progressBarControl1);
      Controls.Add(lbCount);
      Controls.Add(lbState);
      Controls.Add(btStop);
      Controls.Add(btPause);
      Controls.Add(btStart);
      Controls.Add(lbAdditional);
      Controls.Add(pbImage);
      Controls.Add(groupBox1);
      Controls.Add(btOpenDir);
      Controls.Add(lbDirectory);
      Controls.Add(tbDirectory);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "FrmImport";
      FormClosing += frmImport_FormClosing;
      tbDirectory.Properties.EndInit();
      cbAccessTemplate.Properties.EndInit();
      cbSEX.Properties.EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      tbComment.Properties.EndInit();
      ((ISupportInitialize) pbImage).EndInit();
      progressBarControl1.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    public class FaceRecognition
    {
      private IEngine _engine = new Engine();

      public IEngine Engine
      {
        get
        {
          return _engine;
        }
      }

      public bool IsWork { get; set; }

      public void Destroy()
      {
        if (_engine == null)
          return;
        _engine.Dispose();
        _engine = null;
      }
    }

    public delegate void SaveFaceFunc(string str, FaceRecognition rec);

    private delegate void EndFunc(string message);

    private delegate void SetImageFileFunc(Image img);

    private delegate void StartFileFunc(int count);

    private delegate void EndFuncFile(string message);

    private delegate void NewValueFileFunc(int progressValue, int count, int max);

    private delegate void ErrorFunc(string error);
  }
}
