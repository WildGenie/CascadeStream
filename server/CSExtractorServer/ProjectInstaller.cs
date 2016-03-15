// Decompiled with JetBrains decompiler
// Type: FaceExtractorServer.ProjectInstaller
// Assembly: CSExtractorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 8644959D-DFA5-425A-8F71-823BB535F3D1
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\extractor\CSExtractorServer.exe

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FaceExtractorServer
{
  [RunInstaller(true)]
  public class ProjectInstaller : Installer
  {
    private IContainer components = (IContainer) null;
    private ServiceProcessInstaller serviceProcessInstaller1;
    private ServiceInstaller serviceInstaller1;

    public ProjectInstaller()
    {
      this.InitializeComponent();
    }

    private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
    {
      try
      {
        new ServiceController(this.serviceInstaller1.ServiceName).Start();
      }
      catch
      {
      }
    }

    private void serviceProcessInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
    {
      try
      {
        new ServiceController(this.serviceInstaller1.ServiceName).Stop();
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.serviceProcessInstaller1 = new ServiceProcessInstaller();
      this.serviceInstaller1 = new ServiceInstaller();
      this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
      this.serviceProcessInstaller1.Password = (string) null;
      this.serviceProcessInstaller1.Username = (string) null;
      this.serviceProcessInstaller1.AfterInstall += new InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
      this.serviceProcessInstaller1.BeforeUninstall += new InstallEventHandler(this.serviceProcessInstaller1_BeforeUninstall);
      this.serviceInstaller1.Description = "Сервер построения шаблонов";
      this.serviceInstaller1.DisplayName = "CSExtractorServer";
      this.serviceInstaller1.ServiceName = "CSExtractorServer";
      this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
      this.Installers.AddRange(new Installer[2]
      {
        (Installer) this.serviceProcessInstaller1,
        (Installer) this.serviceInstaller1
      });
    }
  }
}
