// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.ClientApp
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using CS.Client.Common;
using System.Resources;
using System.Windows.Forms;
using TS.Sdk.StaticFace.NetBinding;

namespace CascadeFlowClient
{
  internal class ClientApp : BaseApplication
  {
    public ClientApp(string programName)
      : base(programName)
    {
    }

    protected override Form CreateMainForm()
    {
      return (Form) new FrmImages();
    }

    protected override void Initialize()
    {
      Messages.Manager = new ResourceManager("CascadeFlowClient.WordList", typeof (FrmDevices).Assembly);
      Engine.Initialize(0U);
    }

    public override void ReleaseResources()
    {
      base.ReleaseResources();
      Engine.ReleaseResources();
    }
  }
}
