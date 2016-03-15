// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.IEngineWorker
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using FaceIdentification;
using System;
using System.Collections.Generic;

namespace CSIdentificationServer
{
  public interface IEngineWorker
  {
    int TemplatesCount { get; }

    void UpdateTemplateInfo(KeysDataRow template);

    void RemoveTemplate(Guid id);

    void ReloadTemplates();

    void ClearTemplates();

    double Verify(byte[] template, byte[] template1);

    CompareResult[] GetRecommendedList(FrameInfo frame);

    List<Guid> GetExistingTemplateIds();
  }
}
