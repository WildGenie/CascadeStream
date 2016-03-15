// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.VideoServerInteraction
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using BasicComponents.VideoServer;
using CommonContracts;
using log4net;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;

namespace CSDetectorServer
{
  internal class VideoServerInteraction : IVideoServerInteraction
  {
    private readonly ConcurrentDictionary<Guid, VideoServerClient> _clients = new ConcurrentDictionary<Guid, VideoServerClient>();
    private readonly IVideoServerCache _videoServerCache;
    private readonly ILog _log;

    public VideoServerInteraction(IVideoServerCache videoServerCache, ILog log)
    {
      this._videoServerCache = videoServerCache;
      this._log = log;
    }

    public VideoFrame GetFrame(Guid deviceId, Guid vsid)
    {
      ServerConnectionParameters connectionParameters = this._videoServerCache.GetVideoServerConnectionParameters(vsid);
      if (connectionParameters == null)
        return (VideoFrame) null;
      this._log.Debug((object) string.Format("Request to VideoServer DevId={0}", (object) deviceId));
      return this.GetOrCreateVideoServerClient(vsid, connectionParameters).GetDetectorFrame(deviceId);
    }

    private static VideoServerClient CreateAndOpen(string ip, int port)
    {
      VideoServerClient videoServerClient = new VideoServerClient();
      videoServerClient.Endpoint.Address = new EndpointAddress("net.tcp://" + (object) ip + ":" + (string) (object) port + "/VideoStreamServer/VideoServer");
      videoServerClient.Open();
      return videoServerClient;
    }

    private void TryCloseClient(ICommunicationObject client)
    {
      try
      {
        client.Close();
      }
      catch (CommunicationException ex)
      {
        client.Abort();
      }
      catch (TimeoutException ex)
      {
        client.Abort();
      }
      catch (Exception ex)
      {
        client.Abort();
        this._log.Error((object) "WCF client dispose error:", ex);
      }
    }

    private VideoServerClient GetOrCreateVideoServerClient(Guid vsId, ServerConnectionParameters cp)
    {
      VideoServerClient videoServerClient = this._clients.GetOrAdd(vsId, (Func<Guid, VideoServerClient>) (guid => VideoServerInteraction.CreateAndOpen(cp.Ip, cp.Port)));
      if ((videoServerClient.State == CommunicationState.Faulted || videoServerClient.Endpoint.Address.Uri.Host != cp.Ip || videoServerClient.Endpoint.Address.Uri.Port != cp.Port) && this._clients.TryRemove(vsId, out videoServerClient))
      {
        this.TryCloseClient((ICommunicationObject) videoServerClient);
        videoServerClient = VideoServerInteraction.CreateAndOpen(cp.Ip, cp.Port);
        this._clients.TryAdd(vsId, videoServerClient);
      }
      return videoServerClient;
    }
  }
}
