// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.Properties.Resources
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace CascadeFlowClient.Properties
{
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Resources.resourceMan, (object) null))
          Resources.resourceMan = new ResourceManager("CascadeFlowClient.Properties.Resources", typeof (Resources).Assembly);
        return Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Resources.resourceCulture;
      }
      set
      {
        Resources.resourceCulture = value;
      }
    }

    internal static Icon bitmap16x16
    {
      get
      {
        return (Icon) Resources.ResourceManager.GetObject("bitmap16x16", Resources.resourceCulture);
      }
    }

    internal static Icon bitmap64x64
    {
      get
      {
        return (Icon) Resources.ResourceManager.GetObject("bitmap64x64", Resources.resourceCulture);
      }
    }

    internal static Icon glaz
    {
      get
      {
        return (Icon) Resources.ResourceManager.GetObject("glaz", Resources.resourceCulture);
      }
    }

    internal static Icon zamena32x32
    {
      get
      {
        return (Icon) Resources.ResourceManager.GetObject("zamena32x32", Resources.resourceCulture);
      }
    }

    internal Resources()
    {
    }
  }
}
