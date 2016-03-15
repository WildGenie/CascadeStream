// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.Properties.Settings
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSDetectorServer.Properties
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
    [DefaultSettingValue("90")]
    [DebuggerNonUserCode]
    public int Compression
    {
      get
      {
        return (int) this["Compression"];
      }
    }

    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    [DefaultSettingValue("30")]
    public int MaxFrameCount
    {
      get
      {
        return (int) this["MaxFrameCount"];
      }
    }

    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    [DefaultSettingValue("180")]
    public double YawAngle
    {
      get
      {
        return (double) this["YawAngle"];
      }
    }

    [ApplicationScopedSetting]
    [DefaultSettingValue("180")]
    [DebuggerNonUserCode]
    public double InplaneAngle
    {
      get
      {
        return (double) this["InplaneAngle"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Data Source=.\\;Initial Catalog=CascadeStream;Persist Security Info=True;User ID=sa;Password=1;Asynchronous Processing=True")]
    public string ConnectionStringMain
    {
      get
      {
        return (string) this["ConnectionStringMain"];
      }
    }

    [ApplicationScopedSetting]
    [DefaultSettingValue("Data Source=.\\;Initial Catalog=CSLogInfo;Persist Security Info=True;User ID=sa;Password=1;Asynchronous Processing=True")]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    public string ConnectionStringLog
    {
      get
      {
        return (string) this["ConnectionStringLog"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Data Source=.\\;Initial Catalog=CSKeys;Persist Security Info=True;User ID=sa;Password=1;Asynchronous Processing=True")]
    public string ConnectionStringKeys
    {
      get
      {
        return (string) this["ConnectionStringKeys"];
      }
    }

    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    [DefaultSettingValue("Data Source=.\\;Initial Catalog=CSLogInfo;Persist Security Info=True;User ID=sa;Password=1;Asynchronous Processing=True")]
    public string ConnectionStringPhoto
    {
      get
      {
        return (string) this["ConnectionStringPhoto"];
      }
    }

    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    [DefaultSettingValue("Data Source=.\\;Initial Catalog=CSFrameImages;Persist Security Info=True;User ID=sa;Password=1;Asynchronous Processing=True")]
    [SpecialSetting(SpecialSetting.ConnectionString)]
    public string ConnectionStringFrames
    {
      get
      {
        return (string) this["ConnectionStringFrames"];
      }
    }
  }
}
