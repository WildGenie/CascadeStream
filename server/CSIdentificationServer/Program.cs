// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.Program
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using BasicComponents;
using CS.ServiceHost;
using CS.Utils;
using CSIdentificationServer.Properties;
using FaceIdentification;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using TS.Sdk.StaticFace.NetBinding;

namespace CSIdentificationServer
{
  public class Program
  {
    private static string _lastData = "";
    private const string ServiceName = "CSIdentificationServer";
    private static bool _lastConnection;

    public static void Main(params string[] args)
    {
      XmlConfigurator.Configure();
      ILog log = LogManager.GetLogger(typeof (Program));
      new Application(new ApplicationOptions<CmdLineOptions>()
      {
        CommandLineOptionsFactory = (Func<CmdLineOptions>) (() => new CmdLineOptions("CSIdentificationServer")),
        InitializeAction = (Action<string[]>) (a =>
        {
          BcDevices.MaxFrameCount = Settings.Default.MaxFrameCount;
          Program.PrepareDirectories("CSIdentificationServer", log);
          Program.InitializeBiometricEngines();
          SetupUtils.SetupService<BcIdentificationServer, IdentificationServer>(a, "CSIdentificationServer", log, (Func<IPEndPoint, BcIdentificationServer>) (addressParts => ServiceFinder.GetInstance<BcIdentificationServer>(new Func<IEnumerable<BcIdentificationServer>>(BcIdentificationServer.LoadAll), (Func<BcIdentificationServer, bool>) (ds => ds.Ip == addressParts.Address.ToString() && ds.Port == addressParts.Port))));
        }),
        MainWorkerAction = new Action(IdentificationServer.WorkerThread),
        StopAction = new Action(Program.OnStop),
        ServiceName = "CSIdentificationServer"
      }, IdentificationServer.Logger).Run(args);
    }

    private static void InitializeBiometricEngines()
    {
      Engine engine = new Engine();
      Engine.Initialize(0U);
      IdentificationServer.EngineWorker = (IEngineWorker) new EngineWorker((IEngine) engine, IdentificationServer.Logger, (ITemplateCashProvider) new TemplateCashProvider(IdentificationServer.Logger));
      IdentificationServer.EngineWorker.ReloadTemplates();
      IdentificationServer.IniFileWorker = (IIniFileWorker) new IniFileWorker(IdentificationServer.Logger);
    }

    private static void PrepareDirectories(string serviceName, ILog log)
    {
      IdentificationServer.ApplicationFolder = PathExtensions.GetProductAppDataFolder(serviceName);
      try
      {
        PathExtensions.PrepareDirectory(IdentificationServer.ApplicationFolder);
        PathExtensions.PrepareDirectory(Path.Combine(IdentificationServer.ApplicationFolder, "Data"));
        PathExtensions.PrepareDirectory(Path.Combine(IdentificationServer.ApplicationFolder, "Keys"));
      }
      catch (Exception ex)
      {
        log.Fatal((object) "Unable to create directory. Service stopped", ex);
        throw;
      }
    }

    private static void OnStop()
    {
      Engine.ReleaseResources();
      IdentificationServer.StopFlag = true;
    }

    private static void ConfigureSfinksInterop()
    {
      try
      {
        string ipString = ConfigurationManager.AppSettings["SfinksIP"];
        IdentificationServer.LoginCom = ConfigurationManager.AppSettings["LoginCom"] + " " + ConfigurationManager.AppSettings["Version"] + " \"" + ConfigurationManager.AppSettings["User"] + "\" \"" + ConfigurationManager.AppSettings["Password"] + "\"";
        IdentificationServer.GetZoneInfo = ConfigurationManager.AppSettings["GetupinfoCom"] + " " + ConfigurationManager.AppSettings["ObjectID"];
        IdentificationServer.AllowPass = ConfigurationManager.AppSettings["AllopassCom"] + " " + ConfigurationManager.AppSettings["apid"] + " \"" + ConfigurationManager.AppSettings["Obj"] + "\" \"" + ConfigurationManager.AppSettings["Direction"] + "\"";
        IPAddress address;
        IPAddress.TryParse(ipString, out address);
        IdentificationServer.DeviceClient = new SfinksClient(address, Convert.ToInt32(ConfigurationManager.AppSettings["SfinksPort"]));
        IdentificationServer.DeviceClient.Receive += new SfinksClient.ReceiveDataEventHandler(Program.DeviceClient_Receive);
        IdentificationServer.DeviceClient.ConnectionState += new SfinksClient.ConnectionStateEventHandler(Program.DeviceClient_ConnectionState);
        IdentificationServer.DeviceClient.Connect();
        Thread.Sleep(1000);
        IdentificationServer.DeviceClient.SendCommand(IdentificationServer.LoginCom);
        IdentificationServer.DeviceClient.SendCommand(IdentificationServer.GetZoneInfo);
        IdentificationServer.Logger.Info((object) ipString);
        IdentificationServer.Logger.Info((object) ConfigurationManager.AppSettings["SfinksPort"]);
        IdentificationServer.Logger.Info((object) IdentificationServer.GetZoneInfo);
        IdentificationServer.Logger.Info((object) IdentificationServer.LoginCom);
        IdentificationServer.Logger.Info((object) IdentificationServer.AllowPass);
      }
      catch (Exception ex)
      {
        IdentificationServer.Logger.Error((object) ("Load Sfinks" + ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace));
      }
    }

    private static void DeviceClient_ConnectionState(object sender, SfinksClient.ConnectionStateEventArgs eventArgs)
    {
      if (eventArgs.ConnectionState != Program._lastConnection)
        IdentificationServer.Logger.Info((object) ("Sfinks Connection State: " + (object) (bool) (eventArgs.ConnectionState ? 1 : 0)));
      Program._lastConnection = eventArgs.ConnectionState;
    }

    private static void DeviceClient_Receive(object sender, SfinksClient.ReceiveDataEventArgs eventArgs)
    {
      if (eventArgs.Data != Program._lastData)
        IdentificationServer.Logger.Info((object) ("Sfinks receive data: " + eventArgs.Data));
      Program._lastData = eventArgs.Data;
    }
  }
}
