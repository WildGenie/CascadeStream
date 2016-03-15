// Decompiled with JetBrains decompiler
// Type: CascadeManager.TemplateFieldsControl
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace CascadeManager
{
  public class TemplateFieldsControl : XtraUserControl
  {
    private IContainer components = null;
    public XtraScrollableControl xtraScrollableControl1;

    public int Upper { get; set; }

    public int ChildHeight { get; set; }

    public int LableHeight { get; set; }

    public int LeftCorner { get; set; }

    public Dictionary<string, string> Templates { get; set; }

    public TemplateFieldsControl()
    {
      InitializeComponent();
    }

    public void Reinitialize()
    {
      xtraScrollableControl1.Controls.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in Templates)
      {
        int count = xtraScrollableControl1.Controls.Count;
        switch (keyValuePair.Value)
        {
          case "DateTime":
            LabelControl labelControl1 = new LabelControl();
            labelControl1.Text = keyValuePair.Key;
            labelControl1.Location = new Point(LeftCorner, count / 2 * LableHeight + count / 2 * ChildHeight + count * Upper);
            labelControl1.Height = LableHeight;
            xtraScrollableControl1.Controls.Add(labelControl1);
            DateEdit dateEdit = new DateEdit();
            dateEdit.Tag = keyValuePair.Key;
            dateEdit.Location = new Point(LeftCorner, (count / 2 + 1) * LableHeight + count / 2 * ChildHeight + (count + 1) * Upper);
            dateEdit.Width = xtraScrollableControl1.Width - 2 * LeftCorner;
            dateEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            xtraScrollableControl1.Controls.Add(dateEdit);
            break;
          case "String":
            LabelControl labelControl2 = new LabelControl();
            labelControl2.Text = keyValuePair.Key;
            labelControl2.Location = new Point(LeftCorner, count / 2 * LableHeight + count / 2 * ChildHeight + count * Upper);
            labelControl2.Height = LableHeight;
            xtraScrollableControl1.Controls.Add(labelControl2);
            TextEdit textEdit = new TextEdit();
            textEdit.Tag = keyValuePair.Key;
            textEdit.Location = new Point(LeftCorner, (count / 2 + 1) * LableHeight + count / 2 * ChildHeight + (count + 1) * Upper);
            textEdit.Width = xtraScrollableControl1.Width - 2 * LeftCorner;
            textEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            xtraScrollableControl1.Controls.Add(textEdit);
            break;
          case "Double":
            LabelControl labelControl3 = new LabelControl();
            labelControl3.Text = keyValuePair.Key;
            labelControl3.Location = new Point(LeftCorner, count / 2 * LableHeight + count / 2 * ChildHeight + count * Upper);
            labelControl3.Height = LableHeight;
            xtraScrollableControl1.Controls.Add(labelControl3);
            SpinEdit spinEdit1 = new SpinEdit();
            spinEdit1.Tag = keyValuePair.Key;
            spinEdit1.Location = new Point(LeftCorner, (count / 2 + 1) * LableHeight + count / 2 * ChildHeight + (count + 1) * Upper);
            spinEdit1.Width = xtraScrollableControl1.Width - 2 * LeftCorner;
            spinEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            xtraScrollableControl1.Controls.Add(spinEdit1);
            break;
          case "Int":
            LabelControl labelControl4 = new LabelControl();
            labelControl4.Text = keyValuePair.Key;
            labelControl4.Location = new Point(LeftCorner, count / 2 * LableHeight + count / 2 * ChildHeight + count * Upper);
            labelControl4.Height = LableHeight;
            xtraScrollableControl1.Controls.Add(labelControl4);
            SpinEdit spinEdit2 = new SpinEdit();
            spinEdit2.Tag = keyValuePair.Key;
            spinEdit2.Location = new Point(LeftCorner, (count / 2 + 1) * LableHeight + count / 2 * ChildHeight + (count + 1) * Upper);
            spinEdit2.Width = xtraScrollableControl1.Width - 2 * LeftCorner;
            spinEdit2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            spinEdit2.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            spinEdit2.Properties.DisplayFormat.FormatString = "n0";
            spinEdit2.Properties.EditFormat.FormatType = FormatType.Numeric;
            spinEdit2.Properties.EditFormat.FormatString = "n0";
            xtraScrollableControl1.Controls.Add(spinEdit2);
            break;
        }
      }
    }

    public List<SqlParameter> GetParameters()
    {
      List<SqlParameter> list = new List<SqlParameter>();
      foreach (Control control in (ArrangedElementCollection) xtraScrollableControl1.Controls)
      {
        Type type = control.GetType();
        if (type == typeof (TextEdit))
        {
          string parameterName = ((string) control.Tag).Replace("$", "@");
          if (!parameterName.Contains("@"))
            parameterName = "@" + parameterName;
          list.Add(new SqlParameter(parameterName, control.Text));
        }
        if (type == typeof (DateEdit))
        {
          string parameterName = ((string) control.Tag).Replace("$", "@");
          if (!parameterName.Contains("@"))
            parameterName = "@" + parameterName;
          list.Add(new SqlParameter(parameterName, ((DateEdit) control).DateTime));
        }
        if (type == typeof (SpinEdit))
        {
          string parameterName = ((string) control.Tag).Replace("$", "@");
          if (!parameterName.Contains("@"))
            parameterName = "@" + parameterName;
          list.Add(new SqlParameter(parameterName, ((SpinEdit) control).Value));
        }
      }
      return list;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      xtraScrollableControl1 = new XtraScrollableControl();
      SuspendLayout();
      xtraScrollableControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      xtraScrollableControl1.AutoScroll = false;
      xtraScrollableControl1.Location = new Point(3, 3);
      xtraScrollableControl1.Name = "xtraScrollableControl1";
      xtraScrollableControl1.Size = new Size(144, 144);
      xtraScrollableControl1.TabIndex = 0;
      AutoScaleDimensions = new SizeF(6f, 13f);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(xtraScrollableControl1);
      Name = "TemplateFieldsControl";
      ResumeLayout(false);
    }
  }
}
