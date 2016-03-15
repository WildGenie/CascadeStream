// Decompiled with JetBrains decompiler
// Type: CascadeFlowClient.DsResult
// Assembly: АРМ Оператор, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: 8B9D82EA-6277-41F7-9CB6-00BBE5F9D023
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Workstation\АРМ Оператор.exe

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CascadeFlowClient
{
  [XmlRoot("dsResult")]
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [ToolboxItem(true)]
  [HelpKeyword("vs.data.DataSet")]
  [DesignerCategory("code")]
  [Serializable]
  public class DsResult : DataSet
  {
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
    private DsResult.dtResultsDataTable tabledtResults;
    private DsResult.DtImageTypeDataTable tabledtImageType;
    private DsResult.dtDevicesDataTable tabledtDevices;
    private DsResult.dtCategoriesDataTable tabledtCategories;
    private DataRelation relationdtImageType_dtResults;
    private DataRelation relationdtCategories_dtResults;
    private DataRelation relationdtDevices_dtResults;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    public DsResult.dtResultsDataTable dtResults
    {
      get
      {
        return this.tabledtResults;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DebuggerNonUserCode]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DsResult.DtImageTypeDataTable dtImageType
    {
      get
      {
        return this.tabledtImageType;
      }
    }

    [Browsable(false)]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DsResult.dtDevicesDataTable dtDevices
    {
      get
      {
        return this.tabledtDevices;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Browsable(false)]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public DsResult.dtCategoriesDataTable dtCategories
    {
      get
      {
        return this.tabledtCategories;
      }
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public override SchemaSerializationMode SchemaSerializationMode
    {
      get
      {
        return this._schemaSerializationMode;
      }
      set
      {
        this._schemaSerializationMode = value;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataTableCollection Tables
    {
      get
      {
        return base.Tables;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public new DataRelationCollection Relations
    {
      get
      {
        return base.Relations;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    public DsResult()
    {
      this.BeginInit();
      this.InitClass();
      CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
      base.Tables.CollectionChanged += changeEventHandler;
      base.Relations.CollectionChanged += changeEventHandler;
      this.EndInit();
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    protected DsResult(SerializationInfo info, StreamingContext context)
      : base(info, context, false)
    {
      if (this.IsBinarySerialized(info, context))
      {
        this.InitVars(false);
        CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
        this.Tables.CollectionChanged += changeEventHandler;
        this.Relations.CollectionChanged += changeEventHandler;
      }
      else
      {
        string s = (string) info.GetValue("XmlSchema", typeof (string));
        if (this.DetermineSchemaSerializationMode(info, context) == SchemaSerializationMode.IncludeSchema)
        {
          DataSet dataSet = new DataSet();
          dataSet.ReadXmlSchema((XmlReader) new XmlTextReader((TextReader) new StringReader(s)));
          if (dataSet.Tables["dtResults"] != null)
            base.Tables.Add((DataTable) new DsResult.dtResultsDataTable(dataSet.Tables["dtResults"]));
          if (dataSet.Tables["dtImageType"] != null)
            base.Tables.Add((DataTable) new DsResult.DtImageTypeDataTable(dataSet.Tables["dtImageType"]));
          if (dataSet.Tables["dtDevices"] != null)
            base.Tables.Add((DataTable) new DsResult.dtDevicesDataTable(dataSet.Tables["dtDevices"]));
          if (dataSet.Tables["dtCategories"] != null)
            base.Tables.Add((DataTable) new DsResult.dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
          this.DataSetName = dataSet.DataSetName;
          this.Prefix = dataSet.Prefix;
          this.Namespace = dataSet.Namespace;
          this.Locale = dataSet.Locale;
          this.CaseSensitive = dataSet.CaseSensitive;
          this.EnforceConstraints = dataSet.EnforceConstraints;
          this.Merge(dataSet, false, MissingSchemaAction.Add);
          this.InitVars();
        }
        else
          this.ReadXmlSchema((XmlReader) new XmlTextReader((TextReader) new StringReader(s)));
        this.GetSerializationData(info, context);
        CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
        base.Tables.CollectionChanged += changeEventHandler;
        this.Relations.CollectionChanged += changeEventHandler;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    protected override void InitializeDerivedDataSet()
    {
      this.BeginInit();
      this.InitClass();
      this.EndInit();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public override DataSet Clone()
    {
      DsResult dsResult = (DsResult) base.Clone();
      dsResult.InitVars();
      dsResult.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) dsResult;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override bool ShouldSerializeTables()
    {
      return false;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    protected override bool ShouldSerializeRelations()
    {
      return false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override void ReadXmlSerializable(XmlReader reader)
    {
      if (this.DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
      {
        this.Reset();
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(reader);
        if (dataSet.Tables["dtResults"] != null)
          base.Tables.Add((DataTable) new DsResult.dtResultsDataTable(dataSet.Tables["dtResults"]));
        if (dataSet.Tables["dtImageType"] != null)
          base.Tables.Add((DataTable) new DsResult.DtImageTypeDataTable(dataSet.Tables["dtImageType"]));
        if (dataSet.Tables["dtDevices"] != null)
          base.Tables.Add((DataTable) new DsResult.dtDevicesDataTable(dataSet.Tables["dtDevices"]));
        if (dataSet.Tables["dtCategories"] != null)
          base.Tables.Add((DataTable) new DsResult.dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
        this.DataSetName = dataSet.DataSetName;
        this.Prefix = dataSet.Prefix;
        this.Namespace = dataSet.Namespace;
        this.Locale = dataSet.Locale;
        this.CaseSensitive = dataSet.CaseSensitive;
        this.EnforceConstraints = dataSet.EnforceConstraints;
        this.Merge(dataSet, false, MissingSchemaAction.Add);
        this.InitVars();
      }
      else
      {
        int num = (int) this.ReadXml(reader);
        this.InitVars();
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    protected override XmlSchema GetSchemaSerializable()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.WriteXmlSchema((XmlWriter) new XmlTextWriter((Stream) memoryStream, (Encoding) null));
      memoryStream.Position = 0L;
      return XmlSchema.Read((XmlReader) new XmlTextReader((Stream) memoryStream), (ValidationEventHandler) null);
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    internal void InitVars()
    {
      this.InitVars(true);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    internal void InitVars(bool initTable)
    {
      this.tabledtResults = (DsResult.dtResultsDataTable) base.Tables["dtResults"];
      if (initTable && this.tabledtResults != null)
        this.tabledtResults.InitVars();
      this.tabledtImageType = (DsResult.DtImageTypeDataTable) base.Tables["dtImageType"];
      if (initTable && this.tabledtImageType != null)
        this.tabledtImageType.InitVars();
      this.tabledtDevices = (DsResult.dtDevicesDataTable) base.Tables["dtDevices"];
      if (initTable && this.tabledtDevices != null)
        this.tabledtDevices.InitVars();
      this.tabledtCategories = (DsResult.dtCategoriesDataTable) base.Tables["dtCategories"];
      if (initTable && this.tabledtCategories != null)
        this.tabledtCategories.InitVars();
      this.relationdtImageType_dtResults = this.Relations["dtImageType_dtResults"];
      this.relationdtCategories_dtResults = this.Relations["dtCategories_dtResults"];
      this.relationdtDevices_dtResults = this.Relations["dtDevices_dtResults"];
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    private void InitClass()
    {
      this.DataSetName = "dsResult";
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/dsResult.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tabledtResults = new DsResult.dtResultsDataTable();
      base.Tables.Add((DataTable) this.tabledtResults);
      this.tabledtImageType = new DsResult.DtImageTypeDataTable();
      base.Tables.Add((DataTable) this.tabledtImageType);
      this.tabledtDevices = new DsResult.dtDevicesDataTable();
      base.Tables.Add((DataTable) this.tabledtDevices);
      this.tabledtCategories = new DsResult.dtCategoriesDataTable();
      base.Tables.Add((DataTable) this.tabledtCategories);
      this.relationdtImageType_dtResults = new DataRelation("dtImageType_dtResults", new DataColumn[1]
      {
        this.tabledtImageType.typeIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.imgTypeIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtImageType_dtResults);
      this.relationdtCategories_dtResults = new DataRelation("dtCategories_dtResults", new DataColumn[1]
      {
        this.tabledtCategories.CategoryIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.CatIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtCategories_dtResults);
      this.relationdtDevices_dtResults = new DataRelation("dtDevices_dtResults", new DataColumn[1]
      {
        this.tabledtDevices.DeviceIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.DevIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtDevices_dtResults);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializedtResults()
    {
      return false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializedtImageType()
    {
      return false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializedtDevices()
    {
      return false;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DebuggerNonUserCode]
    private bool ShouldSerializedtCategories()
    {
      return false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void SchemaChanged(object sender, CollectionChangeEventArgs e)
    {
      if (e.Action != CollectionChangeAction.Remove)
        return;
      this.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
    {
      DsResult dsResult = new DsResult();
      XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = dsResult.Namespace
      });
      schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
      if (xs.Contains(schemaSerializable.TargetNamespace))
      {
        MemoryStream memoryStream1 = new MemoryStream();
        MemoryStream memoryStream2 = new MemoryStream();
        try
        {
          schemaSerializable.Write((Stream) memoryStream1);
          foreach (XmlSchema xmlSchema in (IEnumerable) xs.Schemas(schemaSerializable.TargetNamespace))
          {
            memoryStream2.SetLength(0L);
            xmlSchema.Write((Stream) memoryStream2);
            if (memoryStream1.Length == memoryStream2.Length)
            {
              memoryStream1.Position = 0L;
              memoryStream2.Position = 0L;
              do
                ;
              while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
              if (memoryStream1.Position == memoryStream1.Length)
                return schemaComplexType;
            }
          }
        }
        finally
        {
          if (memoryStream1 != null)
            memoryStream1.Close();
          if (memoryStream2 != null)
            memoryStream2.Close();
        }
      }
      xs.Add(schemaSerializable);
      return schemaComplexType;
    }

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class DtImageTypeDataTable : TypedTableBase<DsResult.dtImageTypeRow>
    {
      private DataColumn columntypeID;
      private DataColumn columnimgType;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn typeIDColumn
      {
        get
        {
          return this.columntypeID;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn imgTypeColumn
      {
        get
        {
          return this.columnimgType;
        }
      }

      [Browsable(false)]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtImageTypeRow this[int index]
      {
        get
        {
          return (DsResult.dtImageTypeRow) this.Rows[index];
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DtImageTypeDataTable()
      {
        this.TableName = "dtImageType";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal DtImageTypeDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected DtImageTypeDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AdddtImageTypeRow(DsResult.dtImageTypeRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtImageTypeRow AdddtImageTypeRow(int typeID, Bitmap imgType)
      {
        DsResult.dtImageTypeRow dtImageTypeRow = (DsResult.dtImageTypeRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) typeID,
          (object) imgType
        };
        dtImageTypeRow.ItemArray = objArray;
        this.Rows.Add((DataRow) dtImageTypeRow);
        return dtImageTypeRow;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtImageTypeRow FindBytypeID(int typeID)
      {
        return (DsResult.dtImageTypeRow) this.Rows.Find(new object[1]
        {
          (object) typeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DsResult.DtImageTypeDataTable imageTypeDataTable = (DsResult.DtImageTypeDataTable) base.Clone();
        imageTypeDataTable.InitVars();
        return (DataTable) imageTypeDataTable;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DsResult.DtImageTypeDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columntypeID = this.Columns["typeID"];
        this.columnimgType = this.Columns["imgType"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columntypeID = new DataColumn("typeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columntypeID);
        this.columnimgType = new DataColumn("imgType", typeof (Bitmap), (string) null, MappingType.Element);
        this.Columns.Add(this.columnimgType);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columntypeID
        }, 1 != 0));
        this.columntypeID.AllowDBNull = false;
        this.columntypeID.Unique = true;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtImageTypeRow NewdtImageTypeRow()
      {
        return (DsResult.dtImageTypeRow) this.NewRow();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DsResult.dtImageTypeRow(builder);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (DsResult.dtImageTypeRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtImageTypeRowChanged == null)
          return;
        this.dtImageTypeRowChanged((object) this, new DsResult.dtImageTypeRowChangeEvent((DsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtImageTypeRowChanging == null)
          return;
        this.dtImageTypeRowChanging((object) this, new DsResult.dtImageTypeRowChangeEvent((DsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtImageTypeRowDeleted == null)
          return;
        this.dtImageTypeRowDeleted((object) this, new DsResult.dtImageTypeRowChangeEvent((DsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtImageTypeRowDeleting == null)
          return;
        this.dtImageTypeRowDeleting((object) this, new DsResult.dtImageTypeRowChangeEvent((DsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemovedtImageTypeRow(DsResult.dtImageTypeRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DsResult dsResult = new DsResult();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = new Decimal(0);
        xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte) 0);
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = new Decimal(1);
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = dsResult.Namespace
        });
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = "dtImageTypeDataTable"
        });
        schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            foreach (XmlSchema xmlSchema in (IEnumerable) xs.Schemas(schemaSerializable.TargetNamespace))
            {
              memoryStream2.SetLength(0L);
              xmlSchema.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return schemaComplexType;
              }
            }
          }
          finally
          {
            if (memoryStream1 != null)
              memoryStream1.Close();
            if (memoryStream2 != null)
              memoryStream2.Close();
          }
        }
        xs.Add(schemaSerializable);
        return schemaComplexType;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void dtResultsRowChangeEventHandler(object sender, DsResult.dtResultsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void dtImageTypeRowChangeEventHandler(object sender, DsResult.dtImageTypeRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void dtDevicesRowChangeEventHandler(object sender, DsResult.dtDevicesRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void dtCategoriesRowChangeEventHandler(object sender, DsResult.dtCategoriesRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class dtResultsDataTable : TypedTableBase<DsResult.dtResultsRow>
    {
      private DataColumn columnID;
      private DataColumn columnFaceID;
      private DataColumn columnDevID;
      private DataColumn columnCatID;
      private DataColumn columnObjID;
      private DataColumn columnimgTypeID;
      private DataColumn columnDate;
      private DataColumn columnScore;
      private DataColumn columnImageIcon;
      private DataColumn columnName;
      private DataColumn columnStatus;
      private DataColumn columnDBName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn IDColumn
      {
        get
        {
          return this.columnID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FaceIDColumn
      {
        get
        {
          return this.columnFaceID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DevIDColumn
      {
        get
        {
          return this.columnDevID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CatIDColumn
      {
        get
        {
          return this.columnCatID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ObjIDColumn
      {
        get
        {
          return this.columnObjID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn imgTypeIDColumn
      {
        get
        {
          return this.columnimgTypeID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DateColumn
      {
        get
        {
          return this.columnDate;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn ScoreColumn
      {
        get
        {
          return this.columnScore;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn ImageIconColumn
      {
        get
        {
          return this.columnImageIcon;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn NameColumn
      {
        get
        {
          return this.columnName;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn StatusColumn
      {
        get
        {
          return this.columnStatus;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DBNameColumn
      {
        get
        {
          return this.columnDBName;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtResultsRow this[int index]
      {
        get
        {
          return (DsResult.dtResultsRow) this.Rows[index];
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtResultsRowChangeEventHandler dtResultsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtResultsRowChangeEventHandler dtResultsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtResultsRowChangeEventHandler dtResultsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtResultsRowChangeEventHandler dtResultsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public dtResultsDataTable()
      {
        this.TableName = "dtResults";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal dtResultsDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected dtResultsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AdddtResultsRow(DsResult.dtResultsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtResultsRow AdddtResultsRow(Guid ID, Guid FaceID, DsResult.dtDevicesRow parentdtDevicesRowBydtDevices_dtResults, DsResult.dtCategoriesRow parentdtCategoriesRowBydtCategories_dtResults, int ObjID, DsResult.dtImageTypeRow parentdtImageTypeRowBydtImageType_dtResults, DateTime Date, float Score, Bitmap ImageIcon, string Name, bool Status, string DBName)
      {
        DsResult.dtResultsRow dtResultsRow = (DsResult.dtResultsRow) this.NewRow();
        object[] objArray = new object[12]
        {
          (object) ID,
          (object) FaceID,
          null,
          null,
          (object) ObjID,
          null,
          (object) Date,
          (object) Score,
          (object) ImageIcon,
          (object) Name,
          (object) (bool) (Status ? 1 : 0),
          (object) DBName
        };
        if (parentdtDevicesRowBydtDevices_dtResults != null)
          objArray[2] = parentdtDevicesRowBydtDevices_dtResults[0];
        if (parentdtCategoriesRowBydtCategories_dtResults != null)
          objArray[3] = parentdtCategoriesRowBydtCategories_dtResults[0];
        if (parentdtImageTypeRowBydtImageType_dtResults != null)
          objArray[5] = parentdtImageTypeRowBydtImageType_dtResults[0];
        dtResultsRow.ItemArray = objArray;
        this.Rows.Add((DataRow) dtResultsRow);
        return dtResultsRow;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtResultsRow FindByID(Guid ID)
      {
        return (DsResult.dtResultsRow) this.Rows.Find(new object[1]
        {
          (object) ID
        });
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        DsResult.dtResultsDataTable resultsDataTable = (DsResult.dtResultsDataTable) base.Clone();
        resultsDataTable.InitVars();
        return (DataTable) resultsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DsResult.dtResultsDataTable();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal void InitVars()
      {
        this.columnID = this.Columns["ID"];
        this.columnFaceID = this.Columns["FaceID"];
        this.columnDevID = this.Columns["DevID"];
        this.columnCatID = this.Columns["CatID"];
        this.columnObjID = this.Columns["ObjID"];
        this.columnimgTypeID = this.Columns["imgTypeID"];
        this.columnDate = this.Columns["Date"];
        this.columnScore = this.Columns["Score"];
        this.columnImageIcon = this.Columns["ImageIcon"];
        this.columnName = this.Columns["Name"];
        this.columnStatus = this.Columns["Status"];
        this.columnDBName = this.Columns["DBName"];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      private void InitClass()
      {
        this.columnID = new DataColumn("ID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnID);
        this.columnFaceID = new DataColumn("FaceID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFaceID);
        this.columnDevID = new DataColumn("DevID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDevID);
        this.columnCatID = new DataColumn("CatID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCatID);
        this.columnObjID = new DataColumn("ObjID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnObjID);
        this.columnimgTypeID = new DataColumn("imgTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnimgTypeID);
        this.columnDate = new DataColumn("Date", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDate);
        this.columnScore = new DataColumn("Score", typeof (float), (string) null, MappingType.Element);
        this.Columns.Add(this.columnScore);
        this.columnImageIcon = new DataColumn("ImageIcon", typeof (Bitmap), (string) null, MappingType.Element);
        this.Columns.Add(this.columnImageIcon);
        this.columnName = new DataColumn("Name", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnName);
        this.columnStatus = new DataColumn("Status", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnStatus);
        this.columnDBName = new DataColumn("DBName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDBName);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnID
        }, 1 != 0));
        this.columnID.AllowDBNull = false;
        this.columnID.Unique = true;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtResultsRow NewdtResultsRow()
      {
        return (DsResult.dtResultsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DsResult.dtResultsRow(builder);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (DsResult.dtResultsRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtResultsRowChanged == null)
          return;
        this.dtResultsRowChanged((object) this, new DsResult.dtResultsRowChangeEvent((DsResult.dtResultsRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtResultsRowChanging == null)
          return;
        this.dtResultsRowChanging((object) this, new DsResult.dtResultsRowChangeEvent((DsResult.dtResultsRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtResultsRowDeleted == null)
          return;
        this.dtResultsRowDeleted((object) this, new DsResult.dtResultsRowChangeEvent((DsResult.dtResultsRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtResultsRowDeleting == null)
          return;
        this.dtResultsRowDeleting((object) this, new DsResult.dtResultsRowChangeEvent((DsResult.dtResultsRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void RemovedtResultsRow(DsResult.dtResultsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DsResult dsResult = new DsResult();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = new Decimal(0);
        xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte) 0);
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = new Decimal(1);
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = dsResult.Namespace
        });
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = "dtResultsDataTable"
        });
        schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            foreach (XmlSchema xmlSchema in (IEnumerable) xs.Schemas(schemaSerializable.TargetNamespace))
            {
              memoryStream2.SetLength(0L);
              xmlSchema.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return schemaComplexType;
              }
            }
          }
          finally
          {
            if (memoryStream1 != null)
              memoryStream1.Close();
            if (memoryStream2 != null)
              memoryStream2.Close();
          }
        }
        xs.Add(schemaSerializable);
        return schemaComplexType;
      }
    }

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class dtDevicesDataTable : TypedTableBase<DsResult.dtDevicesRow>
    {
      private DataColumn columnDeviceID;
      private DataColumn columnDeviceName;
      private DataColumn columnObjectID;
      private DataColumn columnTableID;
      private DataColumn columnPosition;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn DeviceIDColumn
      {
        get
        {
          return this.columnDeviceID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DeviceNameColumn
      {
        get
        {
          return this.columnDeviceName;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ObjectIDColumn
      {
        get
        {
          return this.columnObjectID;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn TableIDColumn
      {
        get
        {
          return this.columnTableID;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn PositionColumn
      {
        get
        {
          return this.columnPosition;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      [DebuggerNonUserCode]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtDevicesRow this[int index]
      {
        get
        {
          return (DsResult.dtDevicesRow) this.Rows[index];
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtDevicesRowChangeEventHandler dtDevicesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtDevicesRowChangeEventHandler dtDevicesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtDevicesRowChangeEventHandler dtDevicesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtDevicesRowChangeEventHandler dtDevicesRowDeleted;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public dtDevicesDataTable()
      {
        this.TableName = "dtDevices";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal dtDevicesDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected dtDevicesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void AdddtDevicesRow(DsResult.dtDevicesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtDevicesRow AdddtDevicesRow(Guid DeviceID, string DeviceName, int ObjectID, Guid TableID, string Position)
      {
        DsResult.dtDevicesRow dtDevicesRow = (DsResult.dtDevicesRow) this.NewRow();
        object[] objArray = new object[5]
        {
          (object) DeviceID,
          (object) DeviceName,
          (object) ObjectID,
          (object) TableID,
          (object) Position
        };
        dtDevicesRow.ItemArray = objArray;
        this.Rows.Add((DataRow) dtDevicesRow);
        return dtDevicesRow;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtDevicesRow FindByDeviceID(Guid DeviceID)
      {
        return (DsResult.dtDevicesRow) this.Rows.Find(new object[1]
        {
          (object) DeviceID
        });
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        DsResult.dtDevicesDataTable devicesDataTable = (DsResult.dtDevicesDataTable) base.Clone();
        devicesDataTable.InitVars();
        return (DataTable) devicesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DsResult.dtDevicesDataTable();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal void InitVars()
      {
        this.columnDeviceID = this.Columns["DeviceID"];
        this.columnDeviceName = this.Columns["DeviceName"];
        this.columnObjectID = this.Columns["ObjectID"];
        this.columnTableID = this.Columns["TableID"];
        this.columnPosition = this.Columns["Position"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnDeviceID = new DataColumn("DeviceID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDeviceID);
        this.columnDeviceName = new DataColumn("DeviceName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDeviceName);
        this.columnObjectID = new DataColumn("ObjectID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnObjectID);
        this.columnTableID = new DataColumn("TableID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTableID);
        this.columnPosition = new DataColumn("Position", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPosition);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnDeviceID
        }, 1 != 0));
        this.columnDeviceID.AllowDBNull = false;
        this.columnDeviceID.Unique = true;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtDevicesRow NewdtDevicesRow()
      {
        return (DsResult.dtDevicesRow) this.NewRow();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DsResult.dtDevicesRow(builder);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (DsResult.dtDevicesRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtDevicesRowChanged == null)
          return;
        this.dtDevicesRowChanged((object) this, new DsResult.dtDevicesRowChangeEvent((DsResult.dtDevicesRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtDevicesRowChanging == null)
          return;
        this.dtDevicesRowChanging((object) this, new DsResult.dtDevicesRowChangeEvent((DsResult.dtDevicesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtDevicesRowDeleted == null)
          return;
        this.dtDevicesRowDeleted((object) this, new DsResult.dtDevicesRowChangeEvent((DsResult.dtDevicesRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtDevicesRowDeleting == null)
          return;
        this.dtDevicesRowDeleting((object) this, new DsResult.dtDevicesRowChangeEvent((DsResult.dtDevicesRow) e.Row, e.Action));
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void RemovedtDevicesRow(DsResult.dtDevicesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DsResult dsResult = new DsResult();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = new Decimal(0);
        xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte) 0);
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = new Decimal(1);
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = dsResult.Namespace
        });
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = "dtDevicesDataTable"
        });
        schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            foreach (XmlSchema xmlSchema in (IEnumerable) xs.Schemas(schemaSerializable.TargetNamespace))
            {
              memoryStream2.SetLength(0L);
              xmlSchema.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return schemaComplexType;
              }
            }
          }
          finally
          {
            if (memoryStream1 != null)
              memoryStream1.Close();
            if (memoryStream2 != null)
              memoryStream2.Close();
          }
        }
        xs.Add(schemaSerializable);
        return schemaComplexType;
      }
    }

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class dtCategoriesDataTable : TypedTableBase<DsResult.dtCategoriesRow>
    {
      private DataColumn columnCategoryID;
      private DataColumn columnCategory;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataColumn CategoryIDColumn
      {
        get
        {
          return this.columnCategoryID;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CategoryColumn
      {
        get
        {
          return this.columnCategory;
        }
      }

      [Browsable(false)]
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtCategoriesRow this[int index]
      {
        get
        {
          return (DsResult.dtCategoriesRow) this.Rows[index];
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public dtCategoriesDataTable()
      {
        this.TableName = "dtCategories";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal dtCategoriesDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected dtCategoriesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AdddtCategoriesRow(DsResult.dtCategoriesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtCategoriesRow AdddtCategoriesRow(int CategoryID, string Category)
      {
        DsResult.dtCategoriesRow dtCategoriesRow = (DsResult.dtCategoriesRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) CategoryID,
          (object) Category
        };
        dtCategoriesRow.ItemArray = objArray;
        this.Rows.Add((DataRow) dtCategoriesRow);
        return dtCategoriesRow;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtCategoriesRow FindByCategoryID(int CategoryID)
      {
        return (DsResult.dtCategoriesRow) this.Rows.Find(new object[1]
        {
          (object) CategoryID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DsResult.dtCategoriesDataTable categoriesDataTable = (DsResult.dtCategoriesDataTable) base.Clone();
        categoriesDataTable.InitVars();
        return (DataTable) categoriesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DsResult.dtCategoriesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnCategoryID = this.Columns["CategoryID"];
        this.columnCategory = this.Columns["Category"];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      private void InitClass()
      {
        this.columnCategoryID = new DataColumn("CategoryID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCategoryID);
        this.columnCategory = new DataColumn("Category", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCategory);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnCategoryID
        }, 1 != 0));
        this.columnCategoryID.AllowDBNull = false;
        this.columnCategoryID.Unique = true;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtCategoriesRow NewdtCategoriesRow()
      {
        return (DsResult.dtCategoriesRow) this.NewRow();
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DsResult.dtCategoriesRow(builder);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (DsResult.dtCategoriesRow);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtCategoriesRowChanged == null)
          return;
        this.dtCategoriesRowChanged((object) this, new DsResult.dtCategoriesRowChangeEvent((DsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtCategoriesRowChanging == null)
          return;
        this.dtCategoriesRowChanging((object) this, new DsResult.dtCategoriesRowChangeEvent((DsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtCategoriesRowDeleted == null)
          return;
        this.dtCategoriesRowDeleted((object) this, new DsResult.dtCategoriesRowChangeEvent((DsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtCategoriesRowDeleting == null)
          return;
        this.dtCategoriesRowDeleting((object) this, new DsResult.dtCategoriesRowChangeEvent((DsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemovedtCategoriesRow(DsResult.dtCategoriesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DsResult dsResult = new DsResult();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = new Decimal(0);
        xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte) 0);
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = new Decimal(1);
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = dsResult.Namespace
        });
        schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = "dtCategoriesDataTable"
        });
        schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            foreach (XmlSchema xmlSchema in (IEnumerable) xs.Schemas(schemaSerializable.TargetNamespace))
            {
              memoryStream2.SetLength(0L);
              xmlSchema.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return schemaComplexType;
              }
            }
          }
          finally
          {
            if (memoryStream1 != null)
              memoryStream1.Close();
            if (memoryStream2 != null)
              memoryStream2.Close();
          }
        }
        xs.Add(schemaSerializable);
        return schemaComplexType;
      }
    }

    public class dtResultsRow : DataRow
    {
      private DsResult.dtResultsDataTable tabledtResults;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Guid ID
      {
        get
        {
          return (Guid) this[this.tabledtResults.IDColumn];
        }
        set
        {
          this[this.tabledtResults.IDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Guid FaceID
      {
        get
        {
          try
          {
            return (Guid) this[this.tabledtResults.FaceIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FaceID' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.FaceIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Guid DevID
      {
        get
        {
          try
          {
            return (Guid) this[this.tabledtResults.DevIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DevID' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.DevIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public int CatID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtResults.CatIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CatID' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.CatIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public int ObjID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtResults.ObjIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ObjID' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.ObjIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int imgTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtResults.imgTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'imgTypeID' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.imgTypeIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Date
      {
        get
        {
          try
          {
            return (DateTime) this[this.tabledtResults.DateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Date' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.DateColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public float Score
      {
        get
        {
          try
          {
            return (float) this[this.tabledtResults.ScoreColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Score' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.ScoreColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Bitmap ImageIcon
      {
        get
        {
          try
          {
            return (Bitmap) this[this.tabledtResults.ImageIconColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ImageIcon' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.ImageIconColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Name
      {
        get
        {
          try
          {
            return (string) this[this.tabledtResults.NameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Name' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.NameColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool Status
      {
        get
        {
          try
          {
            return (bool) this[this.tabledtResults.StatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Status' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.StatusColumn] = (object) (bool) (value ? 1 : 0);
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public string DBName
      {
        get
        {
          try
          {
            return (string) this[this.tabledtResults.DBNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DBName' in table 'dtResults' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtResults.DBNameColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtImageTypeRow dtImageTypeRow
      {
        get
        {
          return (DsResult.dtImageTypeRow) this.GetParentRow(this.Table.ParentRelations["dtImageType_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtImageType_dtResults"]);
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtCategoriesRow dtCategoriesRow
      {
        get
        {
          return (DsResult.dtCategoriesRow) this.GetParentRow(this.Table.ParentRelations["dtCategories_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtCategories_dtResults"]);
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtDevicesRow dtDevicesRow
      {
        get
        {
          return (DsResult.dtDevicesRow) this.GetParentRow(this.Table.ParentRelations["dtDevices_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtDevices_dtResults"]);
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      internal dtResultsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtResults = (DsResult.dtResultsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFaceIDNull()
      {
        return this.IsNull(this.tabledtResults.FaceIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFaceIDNull()
      {
        this[this.tabledtResults.FaceIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDevIDNull()
      {
        return this.IsNull(this.tabledtResults.DevIDColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetDevIDNull()
      {
        this[this.tabledtResults.DevIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCatIDNull()
      {
        return this.IsNull(this.tabledtResults.CatIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCatIDNull()
      {
        this[this.tabledtResults.CatIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsObjIDNull()
      {
        return this.IsNull(this.tabledtResults.ObjIDColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetObjIDNull()
      {
        this[this.tabledtResults.ObjIDColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsimgTypeIDNull()
      {
        return this.IsNull(this.tabledtResults.imgTypeIDColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetimgTypeIDNull()
      {
        this[this.tabledtResults.imgTypeIDColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsDateNull()
      {
        return this.IsNull(this.tabledtResults.DateColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetDateNull()
      {
        this[this.tabledtResults.DateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsScoreNull()
      {
        return this.IsNull(this.tabledtResults.ScoreColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetScoreNull()
      {
        this[this.tabledtResults.ScoreColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsImageIconNull()
      {
        return this.IsNull(this.tabledtResults.ImageIconColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetImageIconNull()
      {
        this[this.tabledtResults.ImageIconColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsNameNull()
      {
        return this.IsNull(this.tabledtResults.NameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNameNull()
      {
        this[this.tabledtResults.NameColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsStatusNull()
      {
        return this.IsNull(this.tabledtResults.StatusColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetStatusNull()
      {
        this[this.tabledtResults.StatusColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsDBNameNull()
      {
        return this.IsNull(this.tabledtResults.DBNameColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetDBNameNull()
      {
        this[this.tabledtResults.DBNameColumn] = Convert.DBNull;
      }
    }

    public class dtImageTypeRow : DataRow
    {
      private DsResult.DtImageTypeDataTable tabledtImageType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int typeID
      {
        get
        {
          return (int) this[this.tabledtImageType.typeIDColumn];
        }
        set
        {
          this[this.tabledtImageType.typeIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Bitmap imgType
      {
        get
        {
          try
          {
            return (Bitmap) this[this.tabledtImageType.imgTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'imgType' in table 'dtImageType' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtImageType.imgTypeColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal dtImageTypeRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtImageType = (DsResult.DtImageTypeDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsimgTypeNull()
      {
        return this.IsNull(this.tabledtImageType.imgTypeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetimgTypeNull()
      {
        this[this.tabledtImageType.imgTypeColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtImageType_dtResults"] == null)
          return new DsResult.dtResultsRow[0];
        return (DsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtImageType_dtResults"]);
      }
    }

    public class dtDevicesRow : DataRow
    {
      private DsResult.dtDevicesDataTable tabledtDevices;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Guid DeviceID
      {
        get
        {
          return (Guid) this[this.tabledtDevices.DeviceIDColumn];
        }
        set
        {
          this[this.tabledtDevices.DeviceIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public string DeviceName
      {
        get
        {
          try
          {
            return (string) this[this.tabledtDevices.DeviceNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DeviceName' in table 'dtDevices' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtDevices.DeviceNameColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public int ObjectID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtDevices.ObjectIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ObjectID' in table 'dtDevices' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtDevices.ObjectIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public Guid TableID
      {
        get
        {
          try
          {
            return (Guid) this[this.tabledtDevices.TableIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TableID' in table 'dtDevices' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtDevices.TableIDColumn] = (object) value;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public string Position
      {
        get
        {
          try
          {
            return (string) this[this.tabledtDevices.PositionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Position' in table 'dtDevices' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtDevices.PositionColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal dtDevicesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtDevices = (DsResult.dtDevicesDataTable) this.Table;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsDeviceNameNull()
      {
        return this.IsNull(this.tabledtDevices.DeviceNameColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetDeviceNameNull()
      {
        this[this.tabledtDevices.DeviceNameColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsObjectIDNull()
      {
        return this.IsNull(this.tabledtDevices.ObjectIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetObjectIDNull()
      {
        this[this.tabledtDevices.ObjectIDColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsTableIDNull()
      {
        return this.IsNull(this.tabledtDevices.TableIDColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetTableIDNull()
      {
        this[this.tabledtDevices.TableIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPositionNull()
      {
        return this.IsNull(this.tabledtDevices.PositionColumn);
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public void SetPositionNull()
      {
        this[this.tabledtDevices.PositionColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtDevices_dtResults"] == null)
          return new DsResult.dtResultsRow[0];
        return (DsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtDevices_dtResults"]);
      }
    }

    public class dtCategoriesRow : DataRow
    {
      private DsResult.dtCategoriesDataTable tabledtCategories;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int CategoryID
      {
        get
        {
          return (int) this[this.tabledtCategories.CategoryIDColumn];
        }
        set
        {
          this[this.tabledtCategories.CategoryIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Category
      {
        get
        {
          try
          {
            return (string) this[this.tabledtCategories.CategoryColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Category' in table 'dtCategories' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tabledtCategories.CategoryColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal dtCategoriesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtCategories = (DsResult.dtCategoriesDataTable) this.Table;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public bool IsCategoryNull()
      {
        return this.IsNull(this.tabledtCategories.CategoryColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCategoryNull()
      {
        this[this.tabledtCategories.CategoryColumn] = Convert.DBNull;
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtCategories_dtResults"] == null)
          return new DsResult.dtResultsRow[0];
        return (DsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtCategories_dtResults"]);
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class dtResultsRowChangeEvent : EventArgs
    {
      private DsResult.dtResultsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtResultsRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public dtResultsRowChangeEvent(DsResult.dtResultsRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class dtImageTypeRowChangeEvent : EventArgs
    {
      private DsResult.dtImageTypeRow eventRow;
      private DataRowAction eventAction;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtImageTypeRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public dtImageTypeRowChangeEvent(DsResult.dtImageTypeRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class dtDevicesRowChangeEvent : EventArgs
    {
      private DsResult.dtDevicesRow eventRow;
      private DataRowAction eventAction;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public DsResult.dtDevicesRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [DebuggerNonUserCode]
      public dtDevicesRowChangeEvent(DsResult.dtDevicesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class dtCategoriesRowChangeEvent : EventArgs
    {
      private DsResult.dtCategoriesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DsResult.dtCategoriesRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public dtCategoriesRowChangeEvent(DsResult.dtCategoriesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }
  }
}
