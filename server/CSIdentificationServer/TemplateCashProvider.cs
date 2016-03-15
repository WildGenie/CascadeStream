// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.TemplateCashProvider
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using FaceIdentification;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSIdentificationServer
{
  internal class TemplateCashProvider : ITemplateCashProvider
  {
    private readonly ILog _log;

    public TemplateCashProvider(ILog log)
    {
      this._log = log;
    }

    public IEnumerable<KeysDataRow> EnumerateTemplates()
    {
      string templatesFolder = TemplateCashProvider.TemplatesFolder();
      foreach (string path in Directory.EnumerateFiles(templatesFolder, "*.dbo"))
      {
        Guid id;
        if (!Guid.TryParse(Path.GetFileNameWithoutExtension(path), out id))
          this._log.Warn((object) ("Unable to parse id from file name" + Path.GetFileName(path)));
        KeysDataRow template = this.TryReadTemplateFromCache(id);
        if (template != null)
          yield return template;
      }
    }

    public bool TryAdd(KeysDataRow templateData)
    {
      try
      {
        using (FileStream fileStream = new FileStream(TemplateCashProvider.GetCachedTemplateFileName(templateData.Id), FileMode.Create, FileAccess.Write))
          new BinaryFormatter().Serialize((Stream) fileStream, (object) templateData);
        return true;
      }
      catch (Exception ex)
      {
        this._log.Warn((object) ex.Message);
        return false;
      }
    }

    public bool TryUpdate(KeysDataRow templateData)
    {
      return this.TryRemove(templateData.Id) && this.TryAdd(templateData);
    }

    public bool TryRemove(Guid id)
    {
      try
      {
        string templateFileName = TemplateCashProvider.GetCachedTemplateFileName(id);
        if (File.Exists(templateFileName))
          File.Delete(templateFileName);
        return true;
      }
      catch (Exception ex)
      {
        this._log.Warn((object) ex.Message);
        return false;
      }
    }

    private void TryDeleteFile(string file)
    {
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        this._log.Error((object) ("Failed to delete invalid template file" + Path.GetFileName(file)), ex);
      }
    }

    private static string TemplatesFolder()
    {
      return Path.Combine(IdentificationServer.ApplicationFolder, "Keys");
    }

    private KeysDataRow TryReadTemplateFromCache(Guid id)
    {
      string templateFileName = TemplateCashProvider.GetCachedTemplateFileName(id);
      try
      {
        using (FileStream fileStream = new FileStream(templateFileName, FileMode.Open, FileAccess.Read))
          return new BinaryFormatter().Deserialize((Stream) fileStream) as KeysDataRow;
      }
      catch (SerializationException ex)
      {
        this._log.Warn((object) ("Delete invalid template file " + Path.GetFileName(templateFileName)));
        this.TryDeleteFile(templateFileName);
        return (KeysDataRow) null;
      }
    }

    private static string GetCachedTemplateFileName(Guid id)
    {
      return Path.Combine(IdentificationServer.ApplicationFolder, "Keys", (string) (object) id + (object) ".dbo");
    }
  }
}
