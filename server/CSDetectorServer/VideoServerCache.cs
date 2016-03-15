// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.VideoServerCache
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using BasicComponents;
using log4net;
using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace CSDetectorServer
{
  internal class VideoServerCache : IVideoServerCache
  {
    private readonly MemoryCache _videoServersCache = new MemoryCache("VideoServers", (NameValueCollection) null);
    private readonly ILog _log;

    public VideoServerCache(ILog log)
    {
      this._log = log;
    }

    public ServerConnectionParameters GetVideoServerConnectionParameters(Guid vsid)
    {
      string key = vsid.ToString();
      ServerConnectionParameters connectionParameters1 = this._videoServersCache.Get(key, (string) null) as ServerConnectionParameters;
      if (connectionParameters1 != null)
        return connectionParameters1;
      BcVideoServer bcVideoServer = this.TryLoadVideServer(vsid);
      if (bcVideoServer == null)
        return (ServerConnectionParameters) null;
      ServerConnectionParameters connectionParameters2 = new ServerConnectionParameters(bcVideoServer.Ip, bcVideoServer.Port);
      this._videoServersCache.Set(key, (object) connectionParameters2, (DateTimeOffset) DateTime.Now.AddSeconds(10.0), (string) null);
      return connectionParameters2;
    }

    private BcVideoServer TryLoadVideServer(Guid vsid)
    {
      try
      {
        BcVideoServer bcVideoServer = BcVideoServer.LoadById(vsid);
        if (!(bcVideoServer.Id == Guid.Empty))
          return bcVideoServer;
        this._log.WarnFormat("Information about the object id={0} not found in DB", (object) vsid);
        return (BcVideoServer) null;
      }
      catch (Exception ex)
      {
        this._log.Error((object) ex);
        return (BcVideoServer) null;
      }
    }
  }
}
