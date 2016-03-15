// Decompiled with JetBrains decompiler
// Type: CSDetectorServer.Detector
// Assembly: CSDetectorServer, Version=2.0.5674.31273, Culture=neutral, PublicKeyToken=null
// MVID: 23828A2D-8674-48C1-9EA6-06BF9D96086D
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\detector\CSDetectorServer.exe

using AForge.Imaging.Filters;
using BasicComponents;
using CommonContracts;
using CS.DAL;
using CSDetectorServer.Properties;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using TS.Sdk.StaticFace.Model;
using TS.Sdk.StaticFace.NetBinding;
using TS.Sdk.StaticFace.NetBinding.Diagnostics;
using TS.Sdk.StaticFace.NetBinding.Utils;

namespace CSDetectorServer
{
  internal class Detector : IDisposable
  {
    private readonly List<Worker> _processingWorkers = new List<Worker>();
    private readonly LimitedConcurrentQueue<FaceFrame> _faceframes = new LimitedConcurrentQueue<FaceFrame>(5);
    private readonly ILog _log;
    private readonly IBcFrameManager _bcFrameManager;
    private readonly IVideoServerInteraction _videoServerInteraction;
    private readonly IEngine _engine;
    private BcDevices _device;
    private DetectionConfiguration _detectionConfiguration;

    public int Count
    {
      get
      {
        return this._processingWorkers.Count;
      }
    }

    public BcDevices Device
    {
      get
      {
        return this._device;
      }
    }

    public Detector(ILog log, IBcFrameManager bcFrameManager, IVideoServerInteraction videoServerInteraction, IEngine engine)
    {
      this._log = log;
      this._bcFrameManager = bcFrameManager;
      this._videoServerInteraction = videoServerInteraction;
      this._engine = engine;
    }

    public void UpdateDevice(BcDevices bcDevices)
    {
      if (bcDevices == null)
        throw new ArgumentNullException();
      if (this._device != null && bcDevices.Id != this._device.Id)
        throw new InvalidOperationException("bcDevices.Id != _device.Id");
      if (!this.HasDifferProperties(bcDevices))
        return;
      lock (this._processingWorkers)
      {
        ParallelEnumerable.ForAll<Worker>(ParallelEnumerable.AsParallel<Worker>((IEnumerable<Worker>) this._processingWorkers), (Action<Worker>) (worker => worker.Pause()));
        this._device = bcDevices;
        this._detectionConfiguration = new DetectionConfiguration()
        {
          MinFace = (uint) this._device.MinFace,
          DetectionThreshold = (double) this._device.DetectorScore
        };
        this.SyncProcessorCount();
        ParallelEnumerable.ForAll<Worker>(ParallelEnumerable.AsParallel<Worker>((IEnumerable<Worker>) this._processingWorkers), (Action<Worker>) (worker => worker.Resume()));
      }
    }

    private void SyncProcessorCount()
    {
      if (this._device.DetectorCount == this._processingWorkers.Count)
        return;
      this.AddMoreWorkers(this._device.DetectorCount - this._processingWorkers.Count);
      this.RemoveNotNeededWorkers(this._processingWorkers.Count - this._device.DetectorCount);
    }

    protected bool HasDifferProperties(BcDevices d2)
    {
      return this._device == null || this._device.DetectorCount != d2.DetectorCount || (this._device.SaveUnidentified != d2.SaveUnidentified || this._device.SaveImage != d2.SaveImage) || this._device.SaveFace != d2.SaveFace || this._device.Vsid != d2.Vsid;
    }

    public void Dispose()
    {
      lock (this._processingWorkers)
        this.RemoveNotNeededWorkers(this._processingWorkers.Count);
    }

    public FaceFrame GetFaceFrame()
    {
      return LimitedConcurrentQueueExtensions.GetOrDefault<FaceFrame>(this._faceframes);
    }

    private void AddFaceFrame(FaceFrame face)
    {
      this._faceframes.Enqueue(face);
    }

    public void ResetStats()
    {
      if (this._device == null)
        return;
      this._device.FrameCount = 0;
      this._device.DetectionCount = 0;
      this._device.DetectionFaces = 0;
    }

    private void RemoveNotNeededWorkers(int count)
    {
      for (int index = 0; index < count; ++index)
      {
        Worker worker = Enumerable.Last<Worker>((IEnumerable<Worker>) this._processingWorkers);
        this._processingWorkers.Remove(worker);
        worker.Dispose();
      }
    }

    private void AddMoreWorkers(int count)
    {
      for (int index = 0; index < count; ++index)
        this._processingWorkers.Add(new Worker(this._log, new Action(this.LoadAndDetectFrame)));
    }

    private void LoadAndDetectFrame()
    {
      if (!this._device.IsActive)
        return;
      VideoFrame frame = this._videoServerInteraction.GetFrame(this._device.Id, this._device.Vsid);
      if (frame == null)
        return;
      this._log.Debug((object) string.Format("Response from VideoServer DevId={0} FrameIndex={1}", (object) this._device.Id, (object) frame.FrameIndex));
      this._device.IsAccesable = true;
      this.TryDetectFace(frame);
    }

    private void TryDetectFace(VideoFrame frame)
    {
      try
      {
        ++this._device.FrameCount;
        using (Bitmap imageFromArray = BasicComponents.ImageConverter.GetImageFromArray(frame.Frame))
        {
          FaceInfo[] faceInfoArray = this._engine.DetectAllFaces(ModelImageConverter.ConvertFrom(imageFromArray), this._detectionConfiguration);
          ++this._device.DetectionCount;
          this._device.DetectionFaces += faceInfoArray.Length;
          StoredFace[] storedFaceArray = this.TrySaveFullFrameImage(this.FilterFaces((IEnumerable<FaceInfo>) faceInfoArray), frame, imageFromArray);
          if (storedFaceArray.Length == 0)
            return;
          foreach (StoredFace face1 in storedFaceArray)
          {
            FaceFrame face2 = Detector.CropFaceToExtractor(face1, (System.Drawing.Image) imageFromArray);
            face2.FrameIndex = frame.FrameIndex;
            face2.Date = frame.Date;
            this.AddFaceFrame(face2);
          }
        }
      }
      catch (Exception ex)
      {
        this._log.Error((object) ("DetectFace Error Device: " + this._device.Name), ex);
        ++this._device.DetectionErrors;
      }
    }

    private static void SaveDiagnosticFaceToDisc(FaceFrame cf)
    {
      FaceDrawing.SaveToFile("G:\\Test\\Temp" + (object) Guid.NewGuid() + ".jpeg", (System.Drawing.Image) BasicComponents.ImageConverter.GetImageFromArray(cf.Frame), new FaceInfo()
      {
        FaceRectangle = new TS.Core.Model.Rectangle()
        {
          X = (double) cf.Face.LeftX,
          Y = (double) cf.Face.LeftY,
          Width = (double) cf.Face.Width,
          Height = (double) cf.Face.Height
        },
        LeftEye = new TS.Core.Model.Point((double) cf.Face.LeftEyeX, (double) cf.Face.LeftEyeY),
        RightEye = new TS.Core.Model.Point((double) cf.Face.RightEyeX, (double) cf.Face.RightEyeY)
      });
    }

    private IEnumerable<FaceInfo> FilterFaces(IEnumerable<FaceInfo> detectedFaces)
    {
      double eyesDist = (double) this._detectionConfiguration.MinFace / 2.0;
      return Enumerable.Where<FaceInfo>(detectedFaces, (Func<FaceInfo, bool>) (f => Detector.FaceHasValidAngles(f) && Detector.FaceHasValidEyesDistance(f, eyesDist)));
    }

    private StoredFace[] TrySaveFullFrameImage(IEnumerable<FaceInfo> detected, VideoFrame frame, Bitmap image)
    {
      StoredFace[] storedFaceArray = Enumerable.ToArray<StoredFace>(Enumerable.Select<FaceInfo, StoredFace>(detected, new Func<FaceInfo, StoredFace>(Detector.StoredFace)));
      if (!this._device.SaveImage)
        return storedFaceArray;
      try
      {
        byte[] bytes = frame.Frame;
        Guid guid = this._bcFrameManager.SaveFrame(this._device.Id, frame.Date, (Func<byte[]>) (() => Detector.ConvertIfNeeded(image, bytes)), (IReadOnlyCollection<StoredFace>) storedFaceArray);
        foreach (StoredFace storedFace in storedFaceArray)
          storedFace.FrameId = guid;
        return storedFaceArray;
      }
      catch (Exception ex)
      {
        this._log.Error((object) "Failed to save the full frame", ex);
        return storedFaceArray;
      }
    }

    private static byte[] ConvertIfNeeded(Bitmap image, byte[] bytes)
    {
      return object.Equals((object) ImageFormatHelper.TryGetFormatBySignature(bytes), (object) ImageFormat.Jpeg) ? bytes : JpegCompression.ConvertToJpeg(image, 70);
    }

    private static StoredFace StoredFace(FaceInfo dface)
    {
      TS.Core.Model.Rectangle faceRectangle = dface.FaceRectangle;
      return new StoredFace()
      {
        FaceInfo = dface,
        XPos = Math.Max((int) (faceRectangle.X - faceRectangle.Width / 2.0), 0),
        YPos = Math.Max((int) (faceRectangle.Y - faceRectangle.Height / 2.0), 0),
        Width = (int) (faceRectangle.Width * 2.0),
        Height = (int) (faceRectangle.Height * 2.0)
      };
    }

    private static bool FaceHasValidEyesDistance(FaceInfo f, double minEyesDistance)
    {
      return FaceExtensions.EyeDistance(f) >= minEyesDistance;
    }

    private static bool FaceHasValidAngles(FaceInfo f)
    {
      double yawAngle = Settings.Default.YawAngle;
      double inplaneAngle = Settings.Default.InplaneAngle;
      return (double.IsNaN(yawAngle) || Math.Abs(f.YawAngle) <= yawAngle * Math.PI / 180.0) && (double.IsNaN(inplaneAngle) || Math.Abs(f.PitchAngle) <= inplaneAngle * Math.PI / 180.0);
    }

    private static FaceFrame CropFaceToExtractor(StoredFace face, System.Drawing.Image frameImage)
    {
      StoredFace storedFace = face;
      FaceInfo faceInfo = face.FaceInfo;
      using (Bitmap image = new Bitmap(storedFace.Width, storedFace.Height))
      {
        using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) image))
        {
          graphics.InterpolationMode = InterpolationMode.High;
          graphics.DrawImage(frameImage, new RectangleF(0.0f, 0.0f, (float) storedFace.Width, (float) storedFace.Height), new RectangleF((float) storedFace.XPos, (float) storedFace.YPos, (float) storedFace.Width, (float) storedFace.Height), GraphicsUnit.Pixel);
        }
        using (Bitmap bmp = Grayscale.CommonAlgorithms.Y.Apply(image))
        {
          TS.Core.Model.Rectangle faceRectangle = faceInfo.FaceRectangle;
          FaceFrame faceFrame1 = new FaceFrame();
          faceFrame1.Frame = JpegCompression.ConvertToJpeg(bmp, 70);
          faceFrame1.FrameId = storedFace.FrameId;
          FaceFrame faceFrame2 = faceFrame1;
          FaceData faceData1 = new FaceData();
          faceData1.DetectionProb = (float) faceInfo.DetectionProbability;
          faceData1.Width = (int) faceRectangle.Width;
          faceData1.Height = (int) faceRectangle.Height;
          faceData1.LeftX = (int) faceRectangle.X - storedFace.XPos;
          faceData1.LeftY = (int) faceRectangle.Y - storedFace.YPos;
          FaceData faceData2 = faceData1;
          TS.Core.Model.Point point = faceInfo.LeftEye;
          int num1 = (int) point.X - storedFace.XPos;
          faceData2.LeftEyeX = num1;
          FaceData faceData3 = faceData1;
          point = faceInfo.RightEye;
          int num2 = (int) point.X - storedFace.XPos;
          faceData3.RightEyeX = num2;
          FaceData faceData4 = faceData1;
          point = faceInfo.LeftEye;
          int num3 = (int) point.Y - storedFace.YPos;
          faceData4.LeftEyeY = num3;
          FaceData faceData5 = faceData1;
          point = faceInfo.RightEye;
          int num4 = (int) point.Y - storedFace.YPos;
          faceData5.RightEyeY = num4;
          FaceData faceData6 = faceData1;
          faceFrame2.Face = faceData6;
          return faceFrame1;
        }
      }
    }
  }
}
