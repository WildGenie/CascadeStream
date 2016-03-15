// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.IniFileWorker
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using FaceIdentification;
using log4net;
using System;
using System.Data.SqlTypes;
using System.IO;

namespace CSIdentificationServer
{
  public class IniFileWorker : IIniFileWorker
  {
    private readonly ILog _log;
    private readonly string _lastUpdateFilePath;

    public IniFileWorker(ILog log)
    {
      this._log = log;
      this._lastUpdateFilePath = Path.Combine(IdentificationServer.ApplicationFolder, "update.ini");
    }

    public bool IsFileExists()
    {
      return File.Exists(this._lastUpdateFilePath);
    }

    public DateTime GetLastUpdateDate()
    {
      if (!this.IsFileExists())
        return SqlDateTime.MinValue.Value;
      try
      {
        using (StreamReader streamReader = new StreamReader(this._lastUpdateFilePath))
        {
          string str = streamReader.ReadLine();
          if (string.IsNullOrEmpty(str))
            return DateTime.MinValue;
          return new DateTime(Convert.ToInt64(str));
        }
      }
      catch (Exception ex)
      {
        this._log.Error((object) ("Read Last update date error " + ex.Message));
        return SqlDateTime.MinValue.Value;
      }
    }

    public void SetLastUpdateDate(DateTime dtLastUpDate)
    {
      string path = Path.Combine(IdentificationServer.ApplicationFolder, "update.ini");
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(path, false))
          streamWriter.WriteLine(dtLastUpDate.Ticks);
      }
      catch (Exception ex)
      {
        this._log.Error((object) "Error", ex);
      }
    }
  }
}
