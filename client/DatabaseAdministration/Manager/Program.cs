// Decompiled with JetBrains decompiler
// Type: CascadeManager.Program
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using CascadeManager.Properties;

namespace CascadeManager
{
  internal static class Program
  {
    public const string ProgramName = "CascadeManager";

    [STAThread]
    private static void Main()
    {
      ManagerApp managerApp1 = new ManagerApp("CascadeManager");
      managerApp1.Culture = Settings.Default.Language;
      managerApp1.SkinName = "Office 2010 Black";
      ManagerApp managerApp2 = managerApp1;
      managerApp2.Run();
      managerApp2.ReleaseResources();
    }
  }
}
