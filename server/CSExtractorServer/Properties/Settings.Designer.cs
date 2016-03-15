// Decompiled with JetBrains decompiler
// Type: CSExtractorServer.Properties.Settings
// Assembly: CSExtractorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 8644959D-DFA5-425A-8F71-823BB535F3D1
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\extractor\CSExtractorServer.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSExtractorServer.Properties
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

    [DebuggerNonUserCode]
    [DefaultSettingValue("100")]
    [ApplicationScopedSetting]
    public int Compression
    {
      get
      {
        return (int) this["Compression"];
      }
    }

    [DefaultSettingValue("30")]
    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    public int MaxFrameCount
    {
      get
      {
        return (int) this["MaxFrameCount"];
      }
    }
  }
}
