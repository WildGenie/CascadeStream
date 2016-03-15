// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.Properties.Settings
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSIdentificationServer.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
  [CompilerGenerated]
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

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0.55")]
    public float MinScore
    {
      get
      {
        return (float) this["MinScore"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int MinFaceCount
    {
      get
      {
        return (int) this["MinFaceCount"];
      }
    }

    [DefaultSettingValue("True")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public bool UseMinScoreCondition
    {
      get
      {
        return (bool) this["UseMinScoreCondition"];
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("30")]
    [ApplicationScopedSetting]
    public int MaxFrameCount
    {
      get
      {
        return (int) this["MaxFrameCount"];
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("1000")]
    [ApplicationScopedSetting]
    public int MaxLogCount
    {
      get
      {
        return (int) this["MaxLogCount"];
      }
    }
  }
}
