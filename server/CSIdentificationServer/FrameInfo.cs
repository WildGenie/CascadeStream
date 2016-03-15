// Decompiled with JetBrains decompiler
// Type: FaceIdentification.FrameInfo
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using BasicComponents;
using System;

namespace FaceIdentification
{
  public class FrameInfo
  {
    public double MinScore = 0.53;
    public int RequestId = -1;

    public long Date { get; set; }

    public Guid DetectedFrameId { get; set; }

    public BcDevices Device { get; set; }

    public byte[] Template { get; set; }

    public byte[] ImageIcon { get; set; }

    public byte[] Image { get; set; }
  }
}
