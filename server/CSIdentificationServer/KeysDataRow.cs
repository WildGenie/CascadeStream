// Decompiled with JetBrains decompiler
// Type: FaceIdentification.KeysDataRow
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using System;
using System.Data;

namespace FaceIdentification
{
  [Serializable]
  public class KeysDataRow : TemplateInfo
  {
    public byte[] FaceKey { get; set; }

    public KeysDataRow()
    {
    }

    public KeysDataRow(IDataRecord row)
    {
      this.Id = (Guid) row["ID"];
      this.ImageId = (Guid) row["ImageID"];
      this.FaceId = (Guid) row["FaceID"];
      this.AccessId = (int) row["AccessID"];
      this.Ksid = (int) row["KSID"];
      this.Score = (double) row["Score"];
      this.Sex = (int) row["Sex"];
      this.FaceKey = (byte[]) row["ImageKey"];
      this.Surname = row["Surname"].ToString();
    }
  }
}
