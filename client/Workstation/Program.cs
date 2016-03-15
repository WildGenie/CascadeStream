// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.Program
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using CascadeFlowClient.Properties;
using System;

namespace CascadeFlowClient
{
  internal static class Program
  {
    public const string ProgramName = "CascadeFlowClient";

    [STAThread]
    private static void Main()
    {
      ClientApp clientApp1 = new ClientApp("CascadeFlowClient");
      clientApp1.SkinName = "Office 2010 Black";
      clientApp1.Culture = Settings.Default.Language;
      ClientApp clientApp2 = clientApp1;
      clientApp2.Run();
      clientApp2.ReleaseResources();
    }
  }
}
