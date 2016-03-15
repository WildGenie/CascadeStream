// Decompiled with JetBrains decompiler
// Type: CSVideoServer.Program
// Assembly: CSVideoServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: 28813BD9-90C6-4DB5-ADB3-76EB1EEB4BDD
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\video\CSVideoServer.exe

using BasicComponents;
using CS.ServiceHost;
using CS.Utils;
using CSVideoServer.Properties;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using VideoStreamServer;

namespace CSVideoServer
{
  public class Program
  {
    private const string ServiceName = "CSVideoServer";

    public static void Main(params string[] args)
    {
      XmlConfigurator.Configure();
      ILog log = LogManager.GetLogger(typeof (Program));
      new Application(new ApplicationOptions<CmdLineOptions>()
      {
        CommandLineOptionsFactory = (Func<CmdLineOptions>) (() => new CmdLineOptions("CSVideoServer")),
        InitializeAction = (Action<string[]>) (a =>
        {
          BcDevices.MaxFrameCount = Settings.Default.MaxframeCount;
          BcDevices.VideoWidth = Convert.ToInt32(ConfigurationManager.AppSettings["VideoWidth"]);
          BcDevices.VideoHeight = Convert.ToInt32(ConfigurationManager.AppSettings["VideoHeight"]);
          Program.PrepareDirectories("CSVideoServer");
          SetupUtils.SetupService<BcVideoServer, VideoServerContractImpl>(a, "CSVideoServer", log, (Func<IPEndPoint, BcVideoServer>) (addressParts => ServiceFinder.GetInstance<BcVideoServer>(new Func<IEnumerable<BcVideoServer>>(BcVideoServer.LoadAll), (Func<BcVideoServer, bool>) (ds => ds.Ip == addressParts.Address.ToString() && ds.Port == addressParts.Port))));
        }),
        MainWorkerAction = (Action) null,
        StopAction = new Action(Program.OnStop),
        ServiceName = "CSVideoServer"
      }, VideoServerContractImpl.Instance.Logger).Run(args);
    }

    private static void PrepareDirectories(string serviceName)
    {
      VideoServer instance = VideoServerContractImpl.Instance;
      instance.ApplicationFolder = PathExtensions.GetProductAppDataFolder(serviceName);
      instance.ImageFolder = Path.Combine(instance.ApplicationFolder, "Images");
      try
      {
        PathExtensions.PrepareDirectory(instance.ApplicationFolder);
        PathExtensions.PrepareDirectory(Path.Combine(instance.ApplicationFolder, "Faces"));
        PathExtensions.PrepareDirectory(instance.ImageFolder);
        PathExtensions.PrepareDirectory(Path.Combine(instance.ApplicationFolder, "Data"));
      }
      catch (Exception ex)
      {
        instance.Logger.Fatal((object) "Unable to create directory. Service stopped", ex);
        throw;
      }
    }

    private static void OnStop()
    {
      VideoServerContractImpl.Instance.StopServer();
    }
  }
}
