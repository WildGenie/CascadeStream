// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.IIniFileWorker
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System;

namespace CSIdentificationServer
{
  public interface IIniFileWorker
  {
    bool IsFileExists();

    DateTime GetLastUpdateDate();

    void SetLastUpdateDate(DateTime dtLastUpDate);
  }
}
