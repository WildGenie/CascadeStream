// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.IVideoServerCache
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using System;

namespace CSDetectorServer
{
  internal interface IVideoServerCache
  {
    ServerConnectionParameters GetVideoServerConnectionParameters(Guid vsid);
  }
}
