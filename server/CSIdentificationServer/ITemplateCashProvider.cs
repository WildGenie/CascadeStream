// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.ITemplateCashProvider
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using FaceIdentification;
using System;
using System.Collections.Generic;

namespace CSIdentificationServer
{
  internal interface ITemplateCashProvider
  {
    IEnumerable<KeysDataRow> EnumerateTemplates();

    bool TryAdd(KeysDataRow templateData);

    bool TryUpdate(KeysDataRow templateData);

    bool TryRemove(Guid id);
  }
}
