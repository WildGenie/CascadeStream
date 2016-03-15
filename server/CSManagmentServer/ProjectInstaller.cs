// Decompiled with JetBrains decompiler
// Type: CSManagmentServer.ProjectInstaller
// Assembly: CSManagmentServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: C5B7D3C1-7999-4FC6-B40F-178E2CEECAE4
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\manager\CSManagmentServer.exe

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CSManagmentServer
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
      this.serviceInstaller1.Description = "Сервер управления";
      this.serviceInstaller1.DisplayName = "CSManagmentServer";
      this.serviceInstaller1.ServiceName = "CSManagmentServer";
      this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
      this.Installers.AddRange(new Installer[2]
      {
        (Installer) this.serviceProcessInstaller1,
        (Installer) this.serviceInstaller1
      });
    }
  }
}
