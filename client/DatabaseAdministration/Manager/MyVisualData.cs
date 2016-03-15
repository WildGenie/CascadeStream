// Decompiled with JetBrains decompiler
// Type: CascadeManager.MyVisualData
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.Windows;

namespace CascadeManager
{
  [Serializable]
  public class MyVisualData
  {
    public double LineWidth = 1.0;
    public byte[] LineColor = new byte[4];
    public byte[] ForeColor = new byte[4];
    public double FontSize = 12.0;
    public string FontName = "Times New Romman";
    public int FormNumber = 1;
    public MyVisualType Type = MyVisualType.Image;
    public List<Point> Points = new List<Point>();
    public string BlocText = "";
    public bool? _showText = false;
    public Point _Position;
    public Point NextPoint;
    public Point StartPoint;
    public bool InArea;
    public Point StartLinePoint;

    public Point Position
    {
      get
      {
        return _Position;
      }
      set
      {
        _Position = value;
      }
    }

    public bool ShowText
    {
      get
      {
        if (_showText.HasValue)
          return _showText.Value;
        return false;
      }
      set
      {
        _showText = value;
      }
    }

    public Rect GetRect()
    {
      return new Rect(StartPoint, NextPoint);
    }

    public MyVisual GetVisual()
    {
      MyVisual myVisual = new MyVisual();
      myVisual.ShowText = ShowText;
      myVisual.LineColor = LineColor;
      myVisual.ForeColor = ForeColor;
      myVisual.LineWidth = LineWidth;
      myVisual.FontName = FontName;
      myVisual.FontSize = FontSize;
      myVisual.FormNumber = FormNumber;
      myVisual.Position = _Position;
      myVisual.Type = Type;
      myVisual.Points.AddRange((Point[]) Points.ToArray().Clone());
      myVisual.NextPoint = NextPoint;
      myVisual.StartPoint = StartPoint;
      myVisual.InArea = InArea;
      myVisual.BlocText = BlocText;
      myVisual.StartLinePoint = StartLinePoint;
      return myVisual;
    }
  }
}
