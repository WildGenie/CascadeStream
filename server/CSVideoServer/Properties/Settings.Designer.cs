// Decompiled with JetBrains decompiler
// Type: CSVideoServer.Properties.Settings
// Assembly: CSVideoServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: 28813BD9-90C6-4DB5-ADB3-76EB1EEB4BDD
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\video\CSVideoServer.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSVideoServer.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings settings = Settings.defaultInstance;
        return settings;
      }
    }

    [DefaultSettingValue("10")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public int MaxframeCount
    {
      get
      {
        return (int) this["MaxframeCount"];
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("64")]
    [ApplicationScopedSetting]
    public int MinEyesDisnatce
    {
      get
      {
        return (int) this["MinEyesDisnatce"];
      }
    }

    [DefaultSettingValue("90")]
    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    public int Compression
    {
      get
      {
        return (int) this["Compression"];
      }
    }

    [DefaultSettingValue("True")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public bool UseHalfSizeDetection
    {
      get
      {
        return (bool) this["UseHalfSizeDetection"];
      }
    }
  }
}
