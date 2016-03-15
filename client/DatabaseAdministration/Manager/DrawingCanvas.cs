// Decompiled with JetBrains decompiler
// Type: CascadeManager.DrawingCanvas
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CascadeManager
{
  public class DrawingCanvas : Panel
  {
    public List<Visual> Visuals = new List<Visual>();
    public List<Visual> Eyes = new List<Visual>();
    private List<DrawingVisual> _hits = new List<DrawingVisual>();

    protected override int VisualChildrenCount
    {
      get
      {
        return Visuals.Count;
      }
    }

    public bool Contains(Visual vs)
    {
      return Visuals.Contains(vs);
    }

    protected override Visual GetVisualChild(int index)
    {
      return Visuals[index];
    }

    public Visual GetByIndex(int index)
    {
      if (Visuals.Count > index)
        return Visuals[index];
      return null;
    }

    public int GetIndexByVisual(Visual vs)
    {
      return Visuals.IndexOf(vs);
    }

    public void AddVisual(Visual visual)
    {
      Visuals.Add(visual);
      AddVisualChild(visual);
      AddLogicalChild(visual);
    }

    public void AddEye(Visual visual)
    {
      Visuals.Add(visual);
      AddVisualChild(visual);
      AddLogicalChild(visual);
    }

    public void DeleteVisual(Visual visual)
    {
      try
      {
        Visuals.Remove(visual);
        RemoveVisualChild(visual);
        RemoveLogicalChild(visual);
      }
      catch
      {
      }
    }

    public void DeleteEye(Visual visual)
    {
      Visuals.Remove(visual);
      RemoveVisualChild(visual);
      RemoveLogicalChild(visual);
    }

    public void Clear()
    {
      for (int index = 0; index < Visuals.Count; index = index - 1 + 1)
        DeleteVisual(Visuals[index]);
      Visuals.Clear();
    }

    public DrawingVisual GetVisual(Point point)
    {
      return VisualTreeHelper.HitTest(this, point).VisualHit as DrawingVisual;
    }

    public List<DrawingVisual> GetVisuals(Geometry region)
    {
      _hits.Clear();
      VisualTreeHelper.HitTest(this, null, HitTestCallback, new GeometryHitTestParameters(region));
      return _hits;
    }

    public List<Visual> GetAllVisuals()
    {
      return Visuals;
    }

    private HitTestResultBehavior HitTestCallback(HitTestResult result)
    {
      GeometryHitTestResult geometryHitTestResult = (GeometryHitTestResult) result;
      DrawingVisual drawingVisual = result.VisualHit as DrawingVisual;
      if (drawingVisual != null && geometryHitTestResult.IntersectionDetail == IntersectionDetail.FullyInside)
        _hits.Add(drawingVisual);
      return HitTestResultBehavior.Continue;
    }
  }
}
