// Decompiled with JetBrains decompiler
// Type: BasicComponents.GetColumnIndexs
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BasicComponents
{
  [Serializable]
  public class GetColumnIndexs
  {
    public string FileName = "";
    public List<int> ColumnDisplayIndexs = new List<int>();

    public static void Save(GetColumnIndexs gci)
    {
      FileStream fileStream = new FileStream(gci.FileName, FileMode.Create);
      new BinaryFormatter().Serialize(fileStream, new List<GetColumnIndexs>
      {
	      gci
      });
      fileStream.Close();
    }
  }
}
