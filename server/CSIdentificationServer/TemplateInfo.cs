// Decompiled with JetBrains decompiler
// Type: FaceIdentification.TemplateInfo
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System;

namespace FaceIdentification
{
  [Serializable]
  public class TemplateInfo
  {
    public Guid Id { get; set; }

    public Guid ImageId { get; set; }

    public Guid FaceId { get; set; }

    public int AccessId { get; set; }

    public int Ksid { get; set; }

    public int Sex { get; set; }

    public string Surname { get; set; }

    public long Birthday { get; set; }

    public double Score { get; set; }
  }
}
