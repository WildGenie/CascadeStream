// Decompiled with JetBrains decompiler
// Type: CSIdentificationServer.EngineWorker
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using FaceIdentification;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TS.Core.Model;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding;

namespace CSIdentificationServer
{
  internal class EngineWorker : IEngineWorker
  {
    private readonly ConcurrentDictionary<Guid, TemplateInfo> _templateDatas = new ConcurrentDictionary<Guid, TemplateInfo>();
    private const string SsName = "MainStream";
    private const int RecommendedListLength = 10;
    private readonly IEngine _engine;
    private readonly ILog _log;
    private readonly ITemplateCashProvider _templateCashProvider;

    public int TemplatesCount
    {
      get
      {
        return this._templateDatas.Count;
      }
    }

    public EngineWorker(IEngine engine, ILog log, ITemplateCashProvider templateCashProvider)
    {
      this._engine = engine;
      this._log = log;
      this._templateCashProvider = templateCashProvider;
    }

    public void UpdateTemplateInfo(KeysDataRow keysDataRow)
    {
      try
      {
        this._engine.UploadTemplate("MainStream", EngineWorker.ToTemplate(keysDataRow));
      }
      catch (SdkException ex)
      {
        if ((int) ex.ErrorCode != 518)
          this.LogSdkError(ex, "UpdateTemplateInfo");
      }
      this._templateDatas[keysDataRow.Id] = EngineWorker.ToTemplateData(keysDataRow);
      this._templateCashProvider.TryAdd(keysDataRow);
    }

    public void RemoveTemplate(Guid id)
    {
      try
      {
        this._engine.RemoveTemplate("MainStream", id);
      }
      catch (SdkException ex)
      {
        this.LogSdkError(ex, "RemoveTemplate");
      }
      TemplateInfo templateInfo;
      this._templateDatas.TryRemove(id, out templateInfo);
      this._templateCashProvider.TryRemove(id);
    }

    public void ClearTemplates()
    {
      try
      {
        if (Enumerable.Contains<string>((IEnumerable<string>) this._engine.GetSearchSetNames(), "MainStream"))
          this._engine.ClearTemplates("MainStream");
      }
      catch (SdkException ex)
      {
        this.LogSdkError(ex, "ClearTemplates");
      }
      this._templateDatas.Clear();
    }

    public void ReloadTemplates()
    {
      this.ClearTemplates();
      foreach (KeysDataRow keysDataRow in this._templateCashProvider.EnumerateTemplates())
        this.UpdateTemplateInfo(keysDataRow);
    }

    public double Verify(byte[] t1, byte[] t2)
    {
      return this._engine.Verify(t1, t2);
    }

    public CompareResult[] GetRecommendedList(FrameInfo frame)
    {
      try
      {
        return Enumerable.ToArray<CompareResult>(Enumerable.Select<SimilarityListRecord, CompareResult>(Enumerable.Where<SimilarityListRecord>((IEnumerable<SimilarityListRecord>) this._engine.Identify(frame.Template, "MainStream", new IdentificationConfiguration()
        {
          MaxListLength = 10
        }), (Func<SimilarityListRecord, bool>) (record => record.Score >= frame.MinScore)), (Func<SimilarityListRecord, CompareResult>) (record =>
        {
          CompareResult compareResult = this.ToCompareResult(record);
          compareResult.SourceFrame = frame;
          return compareResult;
        })));
      }
      catch (SdkException ex)
      {
        this.LogSdkError(ex, "GetRecommendedList");
        return new CompareResult[0];
      }
    }

    public List<Guid> GetExistingTemplateIds()
    {
      return Enumerable.ToList<Guid>((IEnumerable<Guid>) this._templateDatas.Keys);
    }

    private static Template ToTemplate(KeysDataRow keysDataRow)
    {
      return new Template()
      {
        TemplateId = keysDataRow.Id,
        Data = keysDataRow.FaceKey
      };
    }

    private static TemplateInfo ToTemplateData(KeysDataRow keysDataRow)
    {
      return new TemplateInfo()
      {
        Id = keysDataRow.Id,
        ImageId = keysDataRow.ImageId,
        FaceId = keysDataRow.FaceId,
        AccessId = keysDataRow.AccessId,
        Ksid = keysDataRow.Ksid,
        Sex = keysDataRow.Sex,
        Surname = keysDataRow.Surname,
        Birthday = keysDataRow.Birthday,
        Score = keysDataRow.Score
      };
    }

    private CompareResult ToCompareResult(SimilarityListRecord similarity)
    {
      CompareResult compareResult = new CompareResult()
      {
        KeyId = similarity.TemplateId,
        Score = similarity.Score
      };
      TemplateInfo templateInfo;
      if (this._templateDatas.TryGetValue(similarity.TemplateId, out templateInfo))
      {
        compareResult.AccessId = templateInfo.AccessId;
        compareResult.Birthday = templateInfo.Birthday;
        compareResult.FaceId = templateInfo.FaceId;
        compareResult.ImageId = templateInfo.ImageId;
        compareResult.Ksid = templateInfo.Ksid;
        compareResult.Sex = templateInfo.Sex;
      }
      return compareResult;
    }

    private void LogSdkError(SdkException e, [CallerMemberName] string memberName = "")
    {
      IEnumerable<FieldInfo> enumerable = Enumerable.Where<FieldInfo>((IEnumerable<FieldInfo>) typeof (FaceSdkErrors).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy), (Func<FieldInfo, bool>) (fi => fi.IsLiteral && !fi.IsInitOnly));
      string str = e.ErrorCode.ToString();
      foreach (FieldInfo fieldInfo in enumerable)
      {
        if ((int) e.ErrorCode == (int) (uint) fieldInfo.GetValue((object) null))
        {
          str = fieldInfo.Name;
          break;
        }
      }
      this._log.ErrorFormat("Unable to {0} template Error={1}", (object) memberName, (object) str);
    }
  }
}
