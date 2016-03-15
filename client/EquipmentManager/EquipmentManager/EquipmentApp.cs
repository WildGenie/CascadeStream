// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.EquipmentApp
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using CS.Client.Common;
using System.Resources;
using System.Windows.Forms;

namespace CascadeEquipment
{
  internal class EquipmentApp : BaseApplication
  {
    public EquipmentApp(string programName)
      : base(programName)
    {
    }

    protected override Form CreateMainForm()
    {
      return (Form) new FrmDevices();
    }

    protected override void Initialize()
    {
      Messages.Manager = new ResourceManager("CascadeEquipment.WordList", typeof (FrmDevices).Assembly);
    }
  }
}
