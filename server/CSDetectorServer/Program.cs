// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.Program
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using BasicComponents;
using CS.ServiceHost;
using CS.Utils;
using CSDetectorServer.Properties;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TS.Sdk.StaticFace.NetBinding;

namespace CSDetectorServer
{
  public class Program
  {
    private const string ServiceName = "CSDetectorServer";

    public static void Main(params string[] args)
    {
      XmlConfigurator.Configure();
      ILog log = LogManager.GetLogger(typeof (DetectorServer));
      new Application(new ApplicationOptions<CmdLineOptions>()
      {
        CommandLineOptionsFactory = (Func<CmdLineOptions>) (() => new CmdLineOptions("CSDetectorServer")),
        InitializeAction = (Action<string[]>) (stg =>
        {
          Engine.Initialize(0U);
          log.Debug((object) "Engine initialized");
          BcDevices.MaxFrameCount = Settings.Default.MaxFrameCount;
          Program.PrepareDirectories("CSDetectorServer", log);
          SetupUtils.SetupService<BcDetectorServer, DetectorServer>(stg, "CSDetectorServer", log, (Func<IPEndPoint, BcDetectorServer>) (addressParts => ServiceFinder.GetInstance<BcDetectorServer>(new Func<IEnumerable<BcDetectorServer>>(BcDetectorServer.LoadAll), (Func<BcDetectorServer, bool>) (ds => ds.Ip == addressParts.Address.ToString() && ds.Port == addressParts.Port))));
        }),
        MainWorkerAction = new Action(DetectorServer.RefreshDevicesLoop),
        StopAction = new Action(Program.OnStop),
        ServiceName = "CSDetectorServer"
      }, DetectorServer.Logger).Run(args);
    }

    private static async void OnStop()
    {
      DetectorServer.StopFlag = true;
      await Task.Delay(50);
      Engine.ReleaseResources();
      Environment.Exit(0);
    }

    private static void PrepareDirectories(string serviceName, ILog log)
    {
      DetectorServer.ApplicationFolder = PathExtensions.GetProductAppDataFolder(serviceName);
      DetectorServer.ImageFolder = ConfigurationManager.AppSettings["ImagePath"];
      if (string.IsNullOrEmpty(DetectorServer.ImageFolder))
        DetectorServer.ImageFolder = Path.Combine(DetectorServer.ApplicationFolder, "Images");
      try
      {
        PathExtensions.PrepareDirectory(DetectorServer.ApplicationFolder);
        PathExtensions.PrepareDirectory(Path.Combine(DetectorServer.ApplicationFolder, "Data"));
        PathExtensions.PrepareDirectory(DetectorServer.ImageFolder);
        PathExtensions.PrepareDirectory(Path.Combine(DetectorServer.ImageFolder, "Faces"));
        PathExtensions.PrepareDirectory(Path.Combine(DetectorServer.ImageFolder, "Images"));
      }
      catch (Exception ex)
      {
        log.Fatal((object) "Unable to create directory. Service stopped", ex);
        throw;
      }
    }
  }
}
