// Decompiled with JetBrains decompiler
// Type: FaceIdentification.CompareRequest
// Assembly: CSIdentificationServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 008E8FAA-B893-454B-B679-DD35DA4D8B15
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\identifier\CSIdentificationServer.exe

using BasicComponents;
using CSIdentificationServer;
using CSIdentificationServer.Properties;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIdentification
{
  public class CompareRequest
  {
    private readonly ConcurrentQueue<FrameInfo> _frames = new ConcurrentQueue<FrameInfo>();
    private readonly IEngineWorker _engineWorker;
    private readonly ILog _logger;
    private readonly BcDevices _device;

    public RequestStatus RequestStatus { get; private set; }

    public Guid DeviceId
    {
      get
      {
        return this._device.Id;
      }
    }

    public CompareRequest(BcDevices bcDevice, ILog logger, IEngineWorker engineWorker)
    {
      this._logger = logger;
      this._engineWorker = engineWorker;
      this._device = bcDevice;
      this.RequestStatus = RequestStatus.Wait;
    }

    public void Add(FrameInfo frame)
    {
      if (this.RequestStatus != RequestStatus.Wait)
        return;
      this._frames.Enqueue(frame);
    }

    public async void ProcessAfter(int trackingPeriod)
    {
      try
      {
        await Task.Delay(trackingPeriod).ConfigureAwait(false);
        this.RequestStatus = RequestStatus.Search;
        RawIdentifyResults results = this.IdentifyFaces();
        this.Save(results);
      }
      catch (Exception ex)
      {
        this._logger.Error((object) "Identify Task process error", ex);
      }
      finally
      {
        this.RequestStatus = RequestStatus.Complete;
      }
    }

    private void Save(RawIdentifyResults results)
    {
      this.SaveResults((IEnumerable<CompareResult>) results.Known);
      if (!this._device.SaveUnidentified || results.UnKnownCount < Settings.Default.MinFaceCount)
        return;
      this.SaveLog(CompareRequest.FrameToLogBcLog(results.LastUnKnown));
    }

    private RawIdentifyResults IdentifyFaces()
    {
      RawIdentifyResults rawIdentifyResults = new RawIdentifyResults();
      List<CompareResult> list = new List<CompareResult>();
      FrameInfo result;
      while (this._frames.TryDequeue(out result))
      {
        try
        {
          CompareResult[] recommendedList = this._engineWorker.GetRecommendedList(result);
          if (recommendedList.Length == 0)
          {
            if (this._device.SaveUnidentified)
              rawIdentifyResults.LastUnKnown = result;
          }
          else
          {
            list.AddRange((IEnumerable<CompareResult>) recommendedList);
            result.Device.IdentifierCount += recommendedList.Length;
          }
        }
        catch (Exception ex)
        {
          this._logger.Error((object) " IdentifyFaces Error ", ex);
          ++result.Device.VerificationErrors;
        }
      }
      rawIdentifyResults.Known = CompareRequest.GroupKnown((IEnumerable<CompareResult>) list);
      return rawIdentifyResults;
    }

    public bool HasSimilar(float minScore, FrameInfo frame)
    {
      return Enumerable.Any<FrameInfo>((IEnumerable<FrameInfo>) this._frames, (Func<FrameInfo, bool>) (f => this._engineWorker.Verify(frame.Template, f.Template) >= (double) minScore));
    }

    private void SaveResults(IEnumerable<CompareResult> analiticResults)
    {
      foreach (CompareResult compareResult in analiticResults)
      {
        try
        {
          if (compareResult.AccessId != -1 || this._device.SaveNonCategory)
          {
            BcLog log = CompareRequest.FrameToLogBcLog(compareResult.SourceFrame);
            log.FaceId = compareResult.FaceId;
            log.ImageId = compareResult.ImageId;
            log.Score = Convert.ToSingle(compareResult.Score);
            this.SaveLog(log);
            ++this._device.ResultCount;
          }
        }
        catch (Exception ex)
        {
          this._logger.Error((object) "Error saving - ", ex);
        }
      }
    }

    private static CompareResult[] GroupKnown(IEnumerable<CompareResult> results)
    {
      return Enumerable.ToArray<CompareResult>(Enumerable.Select(Enumerable.Where(Enumerable.Select(Enumerable.GroupBy<CompareResult, Guid>(results, (Func<CompareResult, Guid>) (result => result.FaceId)), g =>
      {
        var fAnonymousType0 = new
        {
          FaceId = g.Key,
          Count = Enumerable.Count<CompareResult>((IEnumerable<CompareResult>) g),
          ResultWithMaxScore = Enumerable.First<CompareResult>((IEnumerable<CompareResult>) Enumerable.OrderByDescending<CompareResult, double>((IEnumerable<CompareResult>) g, (Func<CompareResult, double>) (result => result.Score)))
        };
        return fAnonymousType0;
      }), result => !Settings.Default.UseMinScoreCondition || result.Count >= Settings.Default.MinFaceCount), arg => arg.ResultWithMaxScore));
    }

    private void SaveLog(BcLog log)
    {
      log.Save();
      foreach (BcNotificationSystem sys in IdentificationServer.NotificationSystems)
      {
        IdentificationServer.SendNotifyFunc sendNotifyFunc = new IdentificationServer.SendNotifyFunc(IdentificationServer.SendNotify);
        sendNotifyFunc.BeginInvoke(this._device, log, true, sys, (AsyncCallback) null, (object) sendNotifyFunc);
      }
    }

    private static BcLog FrameToLogBcLog(FrameInfo frame)
    {
      return new BcLog()
      {
        Id = -1,
        FaceId = Guid.Empty,
        TableId = frame.Device.TableId,
        DeviceId = frame.Device.Id,
        ObjectId = frame.Device.ObjectId,
        Date = new DateTime(frame.Date),
        Category = "Не в категории",
        Image = frame.Image,
        ImageKey = frame.Template,
        ImageIcon = frame.ImageIcon,
        FrameId = frame.DetectedFrameId
      };
    }
  }
}
