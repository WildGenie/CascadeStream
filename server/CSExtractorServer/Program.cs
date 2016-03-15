// Decompiled with JetBrains decompiler
// Type: CSExtractorServer.Program
// Assembly: CSExtractorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 8644959D-DFA5-425A-8F71-823BB535F3D1
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\extractor\CSExtractorServer.exe

using BasicComponents;
using CS.ServiceHost;
using CS.Utils;
using CSExtractorServer.Properties;
using FaceExtractorServer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TS.Sdk.StaticFace.NetBinding;

namespace CSExtractorServer
{
  public class Program
  {
    private const string ServiceName = "CSExtractorServer";

    public static void Main(params string[] args)
    {
      XmlConfigurator.Configure();
      ILog log = LogManager.GetLogger(typeof (Program));
      new Application(new ApplicationOptions<CmdLineOptions>()
      {
        CommandLineOptionsFactory = (Func<CmdLineOptions>) (() => new CmdLineOptions("CSExtractorServer")),
        InitializeAction = (Action<string[]>) (a =>
        {
          BcDevices.MaxFrameCount = Settings.Default.MaxFrameCount;
          Program.PrepareDirectories("CSExtractorServer", log);
          SetupUtils.SetupService<BcExtractorServer, ExtractorServer>(a, "CSExtractorServer", log, (Func<IPEndPoint, BcExtractorServer>) (addressParts => ServiceFinder.GetInstance<BcExtractorServer>(new Func<IEnumerable<BcExtractorServer>>(BcExtractorServer.LoadAll), (Func<BcExtractorServer, bool>) (ds => ds.Ip == addressParts.Address.ToString() && ds.Port == addressParts.Port))));
        }),
        MainWorkerAction = new Action(ExtractorServer.RefreshDevicesLoop),
        StopAction = new Action(Program.OnStop),
        ServiceName = "CSExtractorServer"
      }, ExtractorServer.Logger).Run(args);
    }

    private static void PrepareDirectories(string serviceName, ILog log)
    {
      try
      {
        ExtractorServer.ApplicationFolder = PathExtensions.GetProductAppDataFolder(serviceName);
        PathExtensions.PrepareDirectory(ExtractorServer.ApplicationFolder);
        PathExtensions.PrepareDirectory(Path.Combine(ExtractorServer.ApplicationFolder, "Data"));
      }
      catch (Exception ex)
      {
        log.Fatal((object) "Unable to create directory. Service stopped", ex);
        throw;
      }
    }

    protected static void OnStop()
    {
      Engine.ReleaseResources();
      ExtractorServer.StopFlag = true;
      Environment.Exit(0);
    }
  }
}
