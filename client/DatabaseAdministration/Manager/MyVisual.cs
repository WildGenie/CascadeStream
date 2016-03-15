// Decompiled with JetBrains decompiler
// Type: CascadeManager.MyVisual
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CascadeManager
{
  public class MyVisual : DrawingVisual
  {
    public byte[] _LineColor = new byte[4];
    public byte[] _ForeColor = new byte[4];
    public int FormNumber = 1;
    public bool IsSelected = false;
    public MyVisualType Type = MyVisualType.Image;
    public List<Point> Points = new List<Point>();
    public Rect BlocRect = new Rect();
    public string BlocText = "";
    public Point NextPoint;
    public Point StartPoint;
    public bool InArea;
    public Point StartLinePoint;

    public Point Position { get; set; }

    public double FontSize { get; set; }

    public double LineWidth { get; set; }

    public string FontName { get; set; }

    public bool ShowText { get; set; }

    public byte[] ForeColor
    {
      get
      {
        if (_ForeColor != null)
          return _ForeColor;
        return new byte[4];
      }
      set
      {
        _ForeColor = value;
      }
    }

    public byte[] LineColor
    {
      get
      {
        if (_LineColor != null)
          return _LineColor;
        return new byte[4];
      }
      set
      {
        _LineColor = value;
      }
    }

    public MyVisual()
    {
      LineWidth = 1.0;
      FontSize = 12.0;
      FontName = "Times New Roman";
      ShowText = false;
    }

    public Rect GetRect()
    {
      return new Rect(StartPoint, NextPoint);
    }

    public MyVisualData GetData()
    {
      return new MyVisualData
      {
        LineColor = LineColor,
        ForeColor = ForeColor,
        LineWidth = LineWidth,
        FontName = FontName,
        FontSize = FontSize,
        FormNumber = FormNumber,
        _Position = Position,
        ShowText = ShowText,
        Type = Type,
        Points = Points,
        NextPoint = NextPoint,
        StartPoint = StartPoint,
        InArea = InArea,
        BlocText = BlocText,
        StartLinePoint = StartLinePoint
      };
    }
  }
}
