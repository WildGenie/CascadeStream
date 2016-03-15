// Decompiled with JetBrains decompiler
// Type: CSManagmentServer.Program
// Assembly: CSManagmentServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: C5B7D3C1-7999-4FC6-B40F-178E2CEECAE4
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\manager\CSManagmentServer.exe

using CS.ServiceHost;
using CS.Utils;
using log4net;
using log4net.Config;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace CSManagmentServer
{
  public class Program
  {
    private const string ServiceName = "CSManagmentServer";
    private static ServiceHost _serviceHost;

    public static void Main(params string[] args)
    {
      XmlConfigurator.Configure();
      ILog log = LogManager.GetLogger(typeof (Program));
      new Application(new ApplicationOptions<CmdLineOptions>()
      {
        CommandLineOptionsFactory = (Func<CmdLineOptions>) (() => new CmdLineOptions("CSManagmentServer")),
        InitializeAction = (Action<string[]>) (a =>
        {
          Program.PrepareDirectories("CSManagmentServer", log);
          ManagmentServer.KeyLength = Convert.ToInt32(ConfigurationManager.AppSettings["KeyLength"]);
          ConnectionStringsSetter.SetConnectionStrings();
          Program._serviceHost = SetupUtils.SetupAndStartServiceHost<ManagmentServer>(ServiceFinder.GetAddressParts(ConfigurationManager.AppSettings["ServiceEndpointAddress"]), log);
        }),
        MainWorkerAction = (Action) (() =>
        {
          ManagmentServer.StatisticThread = new Thread(new ThreadStart(ManagmentServer.SaveStatistic))
          {
            IsBackground = true
          };
          ManagmentServer.StatisticThread.Start();
          ManagmentServer.RefreshThread = new Thread(new ThreadStart(ManagmentServer.RefreshAll))
          {
            IsBackground = true
          };
          ManagmentServer.RefreshThread.Start();
          ManagmentServer.CheckOperatorsThread = new Thread(new ThreadStart(ManagmentServer.CheckOperators))
          {
            IsBackground = true
          };
          ManagmentServer.CheckOperatorsThread.Start();
        }),
        StopAction = new Action(Program.OnStop),
        ServiceName = "CSManagmentServer"
      }, ManagmentServer.Logger).Run(args);
    }

    private static void PrepareDirectories(string serviceName, ILog log)
    {
      string productAppDataFolder = PathExtensions.GetProductAppDataFolder(serviceName);
      ManagmentServer.KarsFilename = Path.Combine(productAppDataFolder, "CNAPIKarsConfig.xml");
      ManagmentServer.ApplicationFolder = productAppDataFolder;
      try
      {
        PathExtensions.PrepareDirectory(productAppDataFolder);
      }
      catch (Exception ex)
      {
        log.Fatal((object) "Unable to create directory. Service stopped", ex);
        throw;
      }
    }

    private static void OnStop()
    {
      if (Program._serviceHost != null)
      {
        Program._serviceHost.Close();
        Program._serviceHost = (ServiceHost) null;
      }
      try
      {
        ManagmentServer.StatisticThread.Abort();
      }
      catch
      {
      }
      ManagmentServer.StopFlag = true;
      Environment.Exit(0);
    }
  }
}
