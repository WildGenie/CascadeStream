// Decompiled with JetBrains decompiler
// Type: CascadeManager.ManagerApp
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Resources;
using System.Windows.Forms;
using CS.Client.Common;
using CS.Utils;
using DevExpress.XtraEditors;
using TS.Sdk.StaticFace.NetBinding;

namespace CascadeManager
{
  internal class ManagerApp : BaseApplication
  {
    public ManagerApp(string programName)
      : base(programName)
    {
    }

    protected override Form CreateMainForm()
    {
      return new MainForm();
    }

    protected override void Initialize()
    {
      try
      {
        MainForm.Engine = new Engine();
        Engine.Initialize(0U);
      }
      catch (Exception ex)
      {
        int num = (int) XtraMessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Messages.Manager = new ResourceManager("CascadeManager.WordList", typeof (MainForm).Assembly);
    }

    protected override void PrepareDirectories()
    {
      PathExtensions.PrepareDirectory(AppDataFolder);
    }

    public override void ReleaseResources()
    {
      Engine.ReleaseResources();
    }
  }
}
