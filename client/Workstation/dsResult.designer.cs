// Decompiled with JetBrains decompiler
// Type: CascadeManager.dsResult
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

namespace CascadeManager
{
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [HelpKeyword("vs.data.DataSet")]
  [XmlRoot("dsResult")]
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
  [Serializable]
  public class dsResult : DataSet
  {
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
    private dsResult.dtResultsDataTable tabledtResults;
    private dsResult.dtImageTypeDataTable tabledtImageType;
    private dsResult.dtDevicesDataTable tabledtDevices;
    private dsResult.dtCategoriesDataTable tabledtCategories;
    private DataRelation relationdtImageType_dtResults;
    private DataRelation relationdtDevices_dtResults;
    private DataRelation relationdtCategories_dtResults;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Browsable(false)]
    [DebuggerNonUserCode]
    public dsResult.dtResultsDataTable dtResults
    {
      get
      {
        return this.tabledtResults;
      }
    }

    [DebuggerNonUserCode]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Browsable(false)]
    public dsResult.dtImageTypeDataTable dtImageType
    {
      get
      {
        return this.tabledtImageType;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DebuggerNonUserCode]
    [Browsable(false)]
    public dsResult.dtDevicesDataTable dtDevices
    {
      get
      {
        return this.tabledtDevices;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DebuggerNonUserCode]
    [Browsable(false)]
    public dsResult.dtCategoriesDataTable dtCategories
    {
      get
      {
        return this.tabledtCategories;
      }
    }

    [DebuggerNonUserCode]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DebuggerNonUserCode]
    public new DataTableCollection Tables
    {
      get
      {
        return base.Tables;
      }
    }

    [DebuggerNonUserCode]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataRelationCollection Relations
    {
      get
      {
        return base.Relations;
      }
    }

    [DebuggerNonUserCode]
    public dsResult()
    {
      this.BeginInit();
      this.InitClass();
      CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
      base.Tables.CollectionChanged += changeEventHandler;
      base.Relations.CollectionChanged += changeEventHandler;
      this.EndInit();
    }

    [DebuggerNonUserCode]
    protected dsResult(SerializationInfo info, StreamingContext context)
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
            base.Tables.Add((DataTable) new dsResult.dtResultsDataTable(dataSet.Tables["dtResults"]));
          if (dataSet.Tables["dtImageType"] != null)
            base.Tables.Add((DataTable) new dsResult.dtImageTypeDataTable(dataSet.Tables["dtImageType"]));
          if (dataSet.Tables["dtDevices"] != null)
            base.Tables.Add((DataTable) new dsResult.dtDevicesDataTable(dataSet.Tables["dtDevices"]));
          if (dataSet.Tables["dtCategories"] != null)
            base.Tables.Add((DataTable) new dsResult.dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
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

    [DebuggerNonUserCode]
    protected override void InitializeDerivedDataSet()
    {
      this.BeginInit();
      this.InitClass();
      this.EndInit();
    }

    [DebuggerNonUserCode]
    public override DataSet Clone()
    {
      dsResult dsResult = (dsResult) base.Clone();
      dsResult.InitVars();
      dsResult.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) dsResult;
    }

    [DebuggerNonUserCode]
    protected override bool ShouldSerializeTables()
    {
      return false;
    }

    [DebuggerNonUserCode]
    protected override bool ShouldSerializeRelations()
    {
      return false;
    }

    [DebuggerNonUserCode]
    protected override void ReadXmlSerializable(XmlReader reader)
    {
      if (this.DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
      {
        this.Reset();
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(reader);
        if (dataSet.Tables["dtResults"] != null)
          base.Tables.Add((DataTable) new dsResult.dtResultsDataTable(dataSet.Tables["dtResults"]));
        if (dataSet.Tables["dtImageType"] != null)
          base.Tables.Add((DataTable) new dsResult.dtImageTypeDataTable(dataSet.Tables["dtImageType"]));
        if (dataSet.Tables["dtDevices"] != null)
          base.Tables.Add((DataTable) new dsResult.dtDevicesDataTable(dataSet.Tables["dtDevices"]));
        if (dataSet.Tables["dtCategories"] != null)
          base.Tables.Add((DataTable) new dsResult.dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
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

    [DebuggerNonUserCode]
    protected override XmlSchema GetSchemaSerializable()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.WriteXmlSchema((XmlWriter) new XmlTextWriter((Stream) memoryStream, (Encoding) null));
      memoryStream.Position = 0L;
      return XmlSchema.Read((XmlReader) new XmlTextReader((Stream) memoryStream), (ValidationEventHandler) null);
    }

    [DebuggerNonUserCode]
    internal void InitVars()
    {
      this.InitVars(true);
    }

    [DebuggerNonUserCode]
    internal void InitVars(bool initTable)
    {
      this.tabledtResults = (dsResult.dtResultsDataTable) base.Tables["dtResults"];
      if (initTable && this.tabledtResults != null)
        this.tabledtResults.InitVars();
      this.tabledtImageType = (dsResult.dtImageTypeDataTable) base.Tables["dtImageType"];
      if (initTable && this.tabledtImageType != null)
        this.tabledtImageType.InitVars();
      this.tabledtDevices = (dsResult.dtDevicesDataTable) base.Tables["dtDevices"];
      if (initTable && this.tabledtDevices != null)
        this.tabledtDevices.InitVars();
      this.tabledtCategories = (dsResult.dtCategoriesDataTable) base.Tables["dtCategories"];
      if (initTable && this.tabledtCategories != null)
        this.tabledtCategories.InitVars();
      this.relationdtImageType_dtResults = this.Relations["dtImageType_dtResults"];
      this.relationdtDevices_dtResults = this.Relations["dtDevices_dtResults"];
      this.relationdtCategories_dtResults = this.Relations["dtCategories_dtResults"];
    }

    [DebuggerNonUserCode]
    private void InitClass()
    {
      this.DataSetName = "dsResult";
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/dsResult.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tabledtResults = new dsResult.dtResultsDataTable();
      base.Tables.Add((DataTable) this.tabledtResults);
      this.tabledtImageType = new dsResult.dtImageTypeDataTable();
      base.Tables.Add((DataTable) this.tabledtImageType);
      this.tabledtDevices = new dsResult.dtDevicesDataTable();
      base.Tables.Add((DataTable) this.tabledtDevices);
      this.tabledtCategories = new dsResult.dtCategoriesDataTable();
      base.Tables.Add((DataTable) this.tabledtCategories);
      this.relationdtImageType_dtResults = new DataRelation("dtImageType_dtResults", new DataColumn[1]
      {
        this.tabledtImageType.typeIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.imgTypeIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtImageType_dtResults);
      this.relationdtDevices_dtResults = new DataRelation("dtDevices_dtResults", new DataColumn[1]
      {
        this.tabledtDevices.DeviceIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.DevIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtDevices_dtResults);
      this.relationdtCategories_dtResults = new DataRelation("dtCategories_dtResults", new DataColumn[1]
      {
        this.tabledtCategories.CategoryIDColumn
      }, new DataColumn[1]
      {
        this.tabledtResults.CatIDColumn
      }, 0 != 0);
      this.Relations.Add(this.relationdtCategories_dtResults);
    }

    [DebuggerNonUserCode]
    private bool ShouldSerializedtResults()
    {
      return false;
    }

    [DebuggerNonUserCode]
    private bool ShouldSerializedtImageType()
    {
      return false;
    }

    [DebuggerNonUserCode]
    private bool ShouldSerializedtDevices()
    {
      return false;
    }

    [DebuggerNonUserCode]
    private bool ShouldSerializedtCategories()
    {
      return false;
    }

    [DebuggerNonUserCode]
    private void SchemaChanged(object sender, CollectionChangeEventArgs e)
    {
      if (e.Action != CollectionChangeAction.Remove)
        return;
      this.InitVars();
    }

    [DebuggerNonUserCode]
    public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
    {
      dsResult dsResult = new dsResult();
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

    public delegate void dtResultsRowChangeEventHandler(object sender, dsResult.dtResultsRowChangeEvent e);

    public delegate void dtImageTypeRowChangeEventHandler(object sender, dsResult.dtImageTypeRowChangeEvent e);

    public delegate void dtDevicesRowChangeEventHandler(object sender, dsResult.dtDevicesRowChangeEvent e);

    public delegate void dtCategoriesRowChangeEventHandler(object sender, dsResult.dtCategoriesRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class dtResultsDataTable : TypedTableBase<dsResult.dtResultsRow>
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
      public DataColumn IDColumn
      {
        get
        {
          return this.columnID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn FaceIDColumn
      {
        get
        {
          return this.columnFaceID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn DevIDColumn
      {
        get
        {
          return this.columnDevID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn CatIDColumn
      {
        get
        {
          return this.columnCatID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn ObjIDColumn
      {
        get
        {
          return this.columnObjID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn imgTypeIDColumn
      {
        get
        {
          return this.columnimgTypeID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn DateColumn
      {
        get
        {
          return this.columnDate;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn ScoreColumn
      {
        get
        {
          return this.columnScore;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn ImageIconColumn
      {
        get
        {
          return this.columnImageIcon;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn NameColumn
      {
        get
        {
          return this.columnName;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn StatusColumn
      {
        get
        {
          return this.columnStatus;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn DBNameColumn
      {
        get
        {
          return this.columnDBName;
        }
      }

      [DebuggerNonUserCode]
      [Browsable(false)]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow this[int index]
      {
        get
        {
          return (dsResult.dtResultsRow) this.Rows[index];
        }
      }

      public event dsResult.dtResultsRowChangeEventHandler dtResultsRowChanging;

      public event dsResult.dtResultsRowChangeEventHandler dtResultsRowChanged;

      public event dsResult.dtResultsRowChangeEventHandler dtResultsRowDeleting;

      public event dsResult.dtResultsRowChangeEventHandler dtResultsRowDeleted;

      [DebuggerNonUserCode]
      public dtResultsDataTable()
      {
        this.TableName = "dtResults";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

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
      protected dtResultsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      public void AdddtResultsRow(dsResult.dtResultsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow AdddtResultsRow(Guid ID, int FaceID, dsResult.dtDevicesRow parentdtDevicesRowBydtDevices_dtResults, dsResult.dtCategoriesRow parentdtCategoriesRowBydtCategories_dtResults, int ObjID, dsResult.dtImageTypeRow parentdtImageTypeRowBydtImageType_dtResults, DateTime Date, float Score, Bitmap ImageIcon, string Name, bool Status, string DBName)
      {
        dsResult.dtResultsRow dtResultsRow = (dsResult.dtResultsRow) this.NewRow();
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
      public dsResult.dtResultsRow FindByID(Guid ID)
      {
        return (dsResult.dtResultsRow) this.Rows.Find(new object[1]
        {
          (object) ID
        });
      }

      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        dsResult.dtResultsDataTable resultsDataTable = (dsResult.dtResultsDataTable) base.Clone();
        resultsDataTable.InitVars();
        return (DataTable) resultsDataTable;
      }

      [DebuggerNonUserCode]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new dsResult.dtResultsDataTable();
      }

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

      [DebuggerNonUserCode]
      private void InitClass()
      {
        this.columnID = new DataColumn("ID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnID);
        this.columnFaceID = new DataColumn("FaceID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFaceID);
        this.columnDevID = new DataColumn("DevID", typeof (int), (string) null, MappingType.Element);
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
      public dsResult.dtResultsRow NewdtResultsRow()
      {
        return (dsResult.dtResultsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new dsResult.dtResultsRow(builder);
      }

      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (dsResult.dtResultsRow);
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtResultsRowChanged == null)
          return;
        this.dtResultsRowChanged((object) this, new dsResult.dtResultsRowChangeEvent((dsResult.dtResultsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtResultsRowChanging == null)
          return;
        this.dtResultsRowChanging((object) this, new dsResult.dtResultsRowChangeEvent((dsResult.dtResultsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtResultsRowDeleted == null)
          return;
        this.dtResultsRowDeleted((object) this, new dsResult.dtResultsRowChangeEvent((dsResult.dtResultsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtResultsRowDeleting == null)
          return;
        this.dtResultsRowDeleting((object) this, new dsResult.dtResultsRowChangeEvent((dsResult.dtResultsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      public void RemovedtResultsRow(dsResult.dtResultsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        dsResult dsResult = new dsResult();
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
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable]
    public class dtImageTypeDataTable : TypedTableBase<dsResult.dtImageTypeRow>
    {
      private DataColumn columntypeID;
      private DataColumn columnimgType;

      [DebuggerNonUserCode]
      public DataColumn typeIDColumn
      {
        get
        {
          return this.columntypeID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn imgTypeColumn
      {
        get
        {
          return this.columnimgType;
        }
      }

      [DebuggerNonUserCode]
      [Browsable(false)]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtImageTypeRow this[int index]
      {
        get
        {
          return (dsResult.dtImageTypeRow) this.Rows[index];
        }
      }

      public event dsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowChanging;

      public event dsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowChanged;

      public event dsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowDeleting;

      public event dsResult.dtImageTypeRowChangeEventHandler dtImageTypeRowDeleted;

      [DebuggerNonUserCode]
      public dtImageTypeDataTable()
      {
        this.TableName = "dtImageType";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      internal dtImageTypeDataTable(DataTable table)
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
      protected dtImageTypeDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      public void AdddtImageTypeRow(dsResult.dtImageTypeRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      public dsResult.dtImageTypeRow AdddtImageTypeRow(int typeID, Bitmap imgType)
      {
        dsResult.dtImageTypeRow dtImageTypeRow = (dsResult.dtImageTypeRow) this.NewRow();
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
      public dsResult.dtImageTypeRow FindBytypeID(int typeID)
      {
        return (dsResult.dtImageTypeRow) this.Rows.Find(new object[1]
        {
          (object) typeID
        });
      }

      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        dsResult.dtImageTypeDataTable imageTypeDataTable = (dsResult.dtImageTypeDataTable) base.Clone();
        imageTypeDataTable.InitVars();
        return (DataTable) imageTypeDataTable;
      }

      [DebuggerNonUserCode]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new dsResult.dtImageTypeDataTable();
      }

      [DebuggerNonUserCode]
      internal void InitVars()
      {
        this.columntypeID = this.Columns["typeID"];
        this.columnimgType = this.Columns["imgType"];
      }

      [DebuggerNonUserCode]
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

      [DebuggerNonUserCode]
      public dsResult.dtImageTypeRow NewdtImageTypeRow()
      {
        return (dsResult.dtImageTypeRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new dsResult.dtImageTypeRow(builder);
      }

      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (dsResult.dtImageTypeRow);
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtImageTypeRowChanged == null)
          return;
        this.dtImageTypeRowChanged((object) this, new dsResult.dtImageTypeRowChangeEvent((dsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtImageTypeRowChanging == null)
          return;
        this.dtImageTypeRowChanging((object) this, new dsResult.dtImageTypeRowChangeEvent((dsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtImageTypeRowDeleted == null)
          return;
        this.dtImageTypeRowDeleted((object) this, new dsResult.dtImageTypeRowChangeEvent((dsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtImageTypeRowDeleting == null)
          return;
        this.dtImageTypeRowDeleting((object) this, new dsResult.dtImageTypeRowChangeEvent((dsResult.dtImageTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      public void RemovedtImageTypeRow(dsResult.dtImageTypeRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        dsResult dsResult = new dsResult();
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

    [XmlSchemaProvider("GetTypedTableSchema")]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable]
    public class dtDevicesDataTable : TypedTableBase<dsResult.dtDevicesRow>
    {
      private DataColumn columnDeviceID;
      private DataColumn columnDeviceName;
      private DataColumn columnObjectID;
      private DataColumn columnTableID;
      private DataColumn columnPosition;

      [DebuggerNonUserCode]
      public DataColumn DeviceIDColumn
      {
        get
        {
          return this.columnDeviceID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn DeviceNameColumn
      {
        get
        {
          return this.columnDeviceName;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn ObjectIDColumn
      {
        get
        {
          return this.columnObjectID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn TableIDColumn
      {
        get
        {
          return this.columnTableID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn PositionColumn
      {
        get
        {
          return this.columnPosition;
        }
      }

      [DebuggerNonUserCode]
      [Browsable(false)]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtDevicesRow this[int index]
      {
        get
        {
          return (dsResult.dtDevicesRow) this.Rows[index];
        }
      }

      public event dsResult.dtDevicesRowChangeEventHandler dtDevicesRowChanging;

      public event dsResult.dtDevicesRowChangeEventHandler dtDevicesRowChanged;

      public event dsResult.dtDevicesRowChangeEventHandler dtDevicesRowDeleting;

      public event dsResult.dtDevicesRowChangeEventHandler dtDevicesRowDeleted;

      [DebuggerNonUserCode]
      public dtDevicesDataTable()
      {
        this.TableName = "dtDevices";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

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

      [DebuggerNonUserCode]
      protected dtDevicesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      public void AdddtDevicesRow(dsResult.dtDevicesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      public dsResult.dtDevicesRow AdddtDevicesRow(int DeviceID, string DeviceName, int ObjectID, Guid TableID, string Position)
      {
        dsResult.dtDevicesRow dtDevicesRow = (dsResult.dtDevicesRow) this.NewRow();
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
      public dsResult.dtDevicesRow FindByDeviceID(int DeviceID)
      {
        return (dsResult.dtDevicesRow) this.Rows.Find(new object[1]
        {
          (object) DeviceID
        });
      }

      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        dsResult.dtDevicesDataTable devicesDataTable = (dsResult.dtDevicesDataTable) base.Clone();
        devicesDataTable.InitVars();
        return (DataTable) devicesDataTable;
      }

      [DebuggerNonUserCode]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new dsResult.dtDevicesDataTable();
      }

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
      private void InitClass()
      {
        this.columnDeviceID = new DataColumn("DeviceID", typeof (int), (string) null, MappingType.Element);
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
      public dsResult.dtDevicesRow NewdtDevicesRow()
      {
        return (dsResult.dtDevicesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new dsResult.dtDevicesRow(builder);
      }

      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (dsResult.dtDevicesRow);
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtDevicesRowChanged == null)
          return;
        this.dtDevicesRowChanged((object) this, new dsResult.dtDevicesRowChangeEvent((dsResult.dtDevicesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtDevicesRowChanging == null)
          return;
        this.dtDevicesRowChanging((object) this, new dsResult.dtDevicesRowChangeEvent((dsResult.dtDevicesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtDevicesRowDeleted == null)
          return;
        this.dtDevicesRowDeleted((object) this, new dsResult.dtDevicesRowChangeEvent((dsResult.dtDevicesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtDevicesRowDeleting == null)
          return;
        this.dtDevicesRowDeleting((object) this, new dsResult.dtDevicesRowChangeEvent((dsResult.dtDevicesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      public void RemovedtDevicesRow(dsResult.dtDevicesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        dsResult dsResult = new dsResult();
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
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable]
    public class dtCategoriesDataTable : TypedTableBase<dsResult.dtCategoriesRow>
    {
      private DataColumn columnCategoryID;
      private DataColumn columnCategory;

      [DebuggerNonUserCode]
      public DataColumn CategoryIDColumn
      {
        get
        {
          return this.columnCategoryID;
        }
      }

      [DebuggerNonUserCode]
      public DataColumn CategoryColumn
      {
        get
        {
          return this.columnCategory;
        }
      }

      [Browsable(false)]
      [DebuggerNonUserCode]
      public int Count
      {
        get
        {
          return this.Rows.Count;
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtCategoriesRow this[int index]
      {
        get
        {
          return (dsResult.dtCategoriesRow) this.Rows[index];
        }
      }

      public event dsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowChanging;

      public event dsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowChanged;

      public event dsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowDeleting;

      public event dsResult.dtCategoriesRowChangeEventHandler dtCategoriesRowDeleted;

      [DebuggerNonUserCode]
      public dtCategoriesDataTable()
      {
        this.TableName = "dtCategories";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

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

      [DebuggerNonUserCode]
      protected dtCategoriesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      public void AdddtCategoriesRow(dsResult.dtCategoriesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      public dsResult.dtCategoriesRow AdddtCategoriesRow(int CategoryID, string Category)
      {
        dsResult.dtCategoriesRow dtCategoriesRow = (dsResult.dtCategoriesRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) CategoryID,
          (object) Category
        };
        dtCategoriesRow.ItemArray = objArray;
        this.Rows.Add((DataRow) dtCategoriesRow);
        return dtCategoriesRow;
      }

      [DebuggerNonUserCode]
      public dsResult.dtCategoriesRow FindByCategoryID(int CategoryID)
      {
        return (dsResult.dtCategoriesRow) this.Rows.Find(new object[1]
        {
          (object) CategoryID
        });
      }

      [DebuggerNonUserCode]
      public override DataTable Clone()
      {
        dsResult.dtCategoriesDataTable categoriesDataTable = (dsResult.dtCategoriesDataTable) base.Clone();
        categoriesDataTable.InitVars();
        return (DataTable) categoriesDataTable;
      }

      [DebuggerNonUserCode]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new dsResult.dtCategoriesDataTable();
      }

      [DebuggerNonUserCode]
      internal void InitVars()
      {
        this.columnCategoryID = this.Columns["CategoryID"];
        this.columnCategory = this.Columns["Category"];
      }

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
      public dsResult.dtCategoriesRow NewdtCategoriesRow()
      {
        return (dsResult.dtCategoriesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new dsResult.dtCategoriesRow(builder);
      }

      [DebuggerNonUserCode]
      protected override Type GetRowType()
      {
        return typeof (dsResult.dtCategoriesRow);
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.dtCategoriesRowChanged == null)
          return;
        this.dtCategoriesRowChanged((object) this, new dsResult.dtCategoriesRowChangeEvent((dsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.dtCategoriesRowChanging == null)
          return;
        this.dtCategoriesRowChanging((object) this, new dsResult.dtCategoriesRowChangeEvent((dsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.dtCategoriesRowDeleted == null)
          return;
        this.dtCategoriesRowDeleted((object) this, new dsResult.dtCategoriesRowChangeEvent((dsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.dtCategoriesRowDeleting == null)
          return;
        this.dtCategoriesRowDeleting((object) this, new dsResult.dtCategoriesRowChangeEvent((dsResult.dtCategoriesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      public void RemovedtCategoriesRow(dsResult.dtCategoriesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        dsResult dsResult = new dsResult();
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

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtResultsRow : DataRow
    {
      private dsResult.dtResultsDataTable tabledtResults;

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

      [DebuggerNonUserCode]
      public int FaceID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtResults.FaceIDColumn];
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

      [DebuggerNonUserCode]
      public int DevID
      {
        get
        {
          try
          {
            return (int) this[this.tabledtResults.DevIDColumn];
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

      [DebuggerNonUserCode]
      public dsResult.dtImageTypeRow dtImageTypeRow
      {
        get
        {
          return (dsResult.dtImageTypeRow) this.GetParentRow(this.Table.ParentRelations["dtImageType_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtImageType_dtResults"]);
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtDevicesRow dtDevicesRow
      {
        get
        {
          return (dsResult.dtDevicesRow) this.GetParentRow(this.Table.ParentRelations["dtDevices_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtDevices_dtResults"]);
        }
      }

      [DebuggerNonUserCode]
      public dsResult.dtCategoriesRow dtCategoriesRow
      {
        get
        {
          return (dsResult.dtCategoriesRow) this.GetParentRow(this.Table.ParentRelations["dtCategories_dtResults"]);
        }
        set
        {
          this.SetParentRow((DataRow) value, this.Table.ParentRelations["dtCategories_dtResults"]);
        }
      }

      [DebuggerNonUserCode]
      internal dtResultsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtResults = (dsResult.dtResultsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      public bool IsFaceIDNull()
      {
        return this.IsNull(this.tabledtResults.FaceIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetFaceIDNull()
      {
        this[this.tabledtResults.FaceIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsDevIDNull()
      {
        return this.IsNull(this.tabledtResults.DevIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetDevIDNull()
      {
        this[this.tabledtResults.DevIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsCatIDNull()
      {
        return this.IsNull(this.tabledtResults.CatIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetCatIDNull()
      {
        this[this.tabledtResults.CatIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsObjIDNull()
      {
        return this.IsNull(this.tabledtResults.ObjIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetObjIDNull()
      {
        this[this.tabledtResults.ObjIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsimgTypeIDNull()
      {
        return this.IsNull(this.tabledtResults.imgTypeIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetimgTypeIDNull()
      {
        this[this.tabledtResults.imgTypeIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsDateNull()
      {
        return this.IsNull(this.tabledtResults.DateColumn);
      }

      [DebuggerNonUserCode]
      public void SetDateNull()
      {
        this[this.tabledtResults.DateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsScoreNull()
      {
        return this.IsNull(this.tabledtResults.ScoreColumn);
      }

      [DebuggerNonUserCode]
      public void SetScoreNull()
      {
        this[this.tabledtResults.ScoreColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsImageIconNull()
      {
        return this.IsNull(this.tabledtResults.ImageIconColumn);
      }

      [DebuggerNonUserCode]
      public void SetImageIconNull()
      {
        this[this.tabledtResults.ImageIconColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsNameNull()
      {
        return this.IsNull(this.tabledtResults.NameColumn);
      }

      [DebuggerNonUserCode]
      public void SetNameNull()
      {
        this[this.tabledtResults.NameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsStatusNull()
      {
        return this.IsNull(this.tabledtResults.StatusColumn);
      }

      [DebuggerNonUserCode]
      public void SetStatusNull()
      {
        this[this.tabledtResults.StatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsDBNameNull()
      {
        return this.IsNull(this.tabledtResults.DBNameColumn);
      }

      [DebuggerNonUserCode]
      public void SetDBNameNull()
      {
        this[this.tabledtResults.DBNameColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtImageTypeRow : DataRow
    {
      private dsResult.dtImageTypeDataTable tabledtImageType;

      [DebuggerNonUserCode]
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
      internal dtImageTypeRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtImageType = (dsResult.dtImageTypeDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      public bool IsimgTypeNull()
      {
        return this.IsNull(this.tabledtImageType.imgTypeColumn);
      }

      [DebuggerNonUserCode]
      public void SetimgTypeNull()
      {
        this[this.tabledtImageType.imgTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtImageType_dtResults"] == null)
          return new dsResult.dtResultsRow[0];
        return (dsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtImageType_dtResults"]);
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtDevicesRow : DataRow
    {
      private dsResult.dtDevicesDataTable tabledtDevices;

      [DebuggerNonUserCode]
      public int DeviceID
      {
        get
        {
          return (int) this[this.tabledtDevices.DeviceIDColumn];
        }
        set
        {
          this[this.tabledtDevices.DeviceIDColumn] = (object) value;
        }
      }

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
      internal dtDevicesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtDevices = (dsResult.dtDevicesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      public bool IsDeviceNameNull()
      {
        return this.IsNull(this.tabledtDevices.DeviceNameColumn);
      }

      [DebuggerNonUserCode]
      public void SetDeviceNameNull()
      {
        this[this.tabledtDevices.DeviceNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsObjectIDNull()
      {
        return this.IsNull(this.tabledtDevices.ObjectIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetObjectIDNull()
      {
        this[this.tabledtDevices.ObjectIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsTableIDNull()
      {
        return this.IsNull(this.tabledtDevices.TableIDColumn);
      }

      [DebuggerNonUserCode]
      public void SetTableIDNull()
      {
        this[this.tabledtDevices.TableIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public bool IsPositionNull()
      {
        return this.IsNull(this.tabledtDevices.PositionColumn);
      }

      [DebuggerNonUserCode]
      public void SetPositionNull()
      {
        this[this.tabledtDevices.PositionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtDevices_dtResults"] == null)
          return new dsResult.dtResultsRow[0];
        return (dsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtDevices_dtResults"]);
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtCategoriesRow : DataRow
    {
      private dsResult.dtCategoriesDataTable tabledtCategories;

      [DebuggerNonUserCode]
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
      internal dtCategoriesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tabledtCategories = (dsResult.dtCategoriesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      public bool IsCategoryNull()
      {
        return this.IsNull(this.tabledtCategories.CategoryColumn);
      }

      [DebuggerNonUserCode]
      public void SetCategoryNull()
      {
        this[this.tabledtCategories.CategoryColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow[] GetdtResultsRows()
      {
        if (this.Table.ChildRelations["dtCategories_dtResults"] == null)
          return new dsResult.dtResultsRow[0];
        return (dsResult.dtResultsRow[]) this.GetChildRows(this.Table.ChildRelations["dtCategories_dtResults"]);
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtResultsRowChangeEvent : EventArgs
    {
      private dsResult.dtResultsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      public dsResult.dtResultsRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      public dtResultsRowChangeEvent(dsResult.dtResultsRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtImageTypeRowChangeEvent : EventArgs
    {
      private dsResult.dtImageTypeRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      public dsResult.dtImageTypeRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      public dtImageTypeRowChangeEvent(dsResult.dtImageTypeRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtDevicesRowChangeEvent : EventArgs
    {
      private dsResult.dtDevicesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      public dsResult.dtDevicesRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      public dtDevicesRowChangeEvent(dsResult.dtDevicesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    public class dtCategoriesRowChangeEvent : EventArgs
    {
      private dsResult.dtCategoriesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      public dsResult.dtCategoriesRow Row
      {
        get
        {
          return this.eventRow;
        }
      }

      [DebuggerNonUserCode]
      public DataRowAction Action
      {
        get
        {
          return this.eventAction;
        }
      }

      [DebuggerNonUserCode]
      public dtCategoriesRowChangeEvent(dsResult.dtCategoriesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }
    }
  }
}
