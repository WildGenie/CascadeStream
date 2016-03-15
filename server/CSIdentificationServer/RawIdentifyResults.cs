// Decompiled with JetBrains decompiler
// Type: FaceIdentification.RawIdentifyResults
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System;

namespace FaceIdentification
{
  internal class RawIdentifyResults
  {
    private FrameInfo _lastUnKnown;

    public CompareResult[] Known { get; set; }

    public int UnKnownCount { get; private set; }

    public FrameInfo LastUnKnown
    {
      get
      {
        return this._lastUnKnown;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        ++this.UnKnownCount;
        this._lastUnKnown = value;
      }
    }
  }
}
