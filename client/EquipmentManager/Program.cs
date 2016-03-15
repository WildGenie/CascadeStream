// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.Program
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using CascadeEquipment.Properties;
using System;

namespace CascadeEquipment
{
  internal static class Program
  {
    public const string ProgramName = "CascadeEquipment";

    [STAThread]
    private static void Main()
    {
      EquipmentApp equipmentApp1 = new EquipmentApp("CascadeEquipment");
      equipmentApp1.Culture = Settings.Default.Language;
      equipmentApp1.SkinName = "Office 2010 Black";
      EquipmentApp equipmentApp2 = equipmentApp1;
      equipmentApp2.Run();
      equipmentApp2.ReleaseResources();
    }
  }
}
