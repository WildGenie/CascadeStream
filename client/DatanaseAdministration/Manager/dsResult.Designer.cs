// Decompiled with JetBrains decompiler
// Type: CascadeManager.DsResult
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

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
	[XmlRoot("dsResult")]
	[HelpKeyword("vs.data.DataSet")]
	[DesignerCategory("code")]
	[ToolboxItem(true)]
	[XmlSchemaProvider("GetTypedDataSetSchema")]
	[Serializable]
	public class DsResult : DataSet
	{
		private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
		private dtResultsDataTable tabledtResults;
		private DtImageTypeDataTable tabledtImageType;
		private dtDevicesDataTable tabledtDevices;
		private dtCategoriesDataTable tabledtCategories;
		private DataRelation relationdtImageType_dtResults;
		private DataRelation relationdtCategories_dtResults;
		private DataRelation relationdtDevices_dtResults;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public dtResultsDataTable dtResults
		{
			get
			{
				return this.tabledtResults;
			}
		}

		[DebuggerNonUserCode]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[Browsable(false)]
		public DtImageTypeDataTable dtImageType
		{
			get
			{
				return this.tabledtImageType;
			}
		}

		[DebuggerNonUserCode]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public dtDevicesDataTable dtDevices
		{
			get
			{
				return this.tabledtDevices;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[Browsable(false)]
		[DebuggerNonUserCode]
		public dtCategoriesDataTable dtCategories
		{
			get
			{
				return this.tabledtCategories;
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
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
				string s = (string)info.GetValue("XmlSchema", typeof(string));
				if (this.DetermineSchemaSerializationMode(info, context) == SchemaSerializationMode.IncludeSchema)
				{
					DataSet dataSet = new DataSet();
					dataSet.ReadXmlSchema((XmlReader)new XmlTextReader((TextReader)new StringReader(s)));
					if (dataSet.Tables["dtResults"] != null)
						base.Tables.Add((DataTable)new dtResultsDataTable(dataSet.Tables["dtResults"]));
					if (dataSet.Tables["dtImageType"] != null)
						base.Tables.Add((DataTable)new DtImageTypeDataTable(dataSet.Tables["dtImageType"]));
					if (dataSet.Tables["dtDevices"] != null)
						base.Tables.Add((DataTable)new dtDevicesDataTable(dataSet.Tables["dtDevices"]));
					if (dataSet.Tables["dtCategories"] != null)
						base.Tables.Add((DataTable)new dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
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
					this.ReadXmlSchema((XmlReader)new XmlTextReader((TextReader)new StringReader(s)));
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

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
		public override DataSet Clone()
		{
			DsResult dsResult = (DsResult)base.Clone();
			dsResult.InitVars();
			dsResult.SchemaSerializationMode = this.SchemaSerializationMode;
			return (DataSet)dsResult;
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
		protected override bool ShouldSerializeTables()
		{
			return false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		protected override bool ShouldSerializeRelations()
		{
			return false;
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
		protected override void ReadXmlSerializable(XmlReader reader)
		{
			if (this.DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
			{
				this.Reset();
				DataSet dataSet = new DataSet();
				int num = (int)dataSet.ReadXml(reader);
				if (dataSet.Tables["dtResults"] != null)
					base.Tables.Add((DataTable)new dtResultsDataTable(dataSet.Tables["dtResults"]));
				if (dataSet.Tables["dtImageType"] != null)
					base.Tables.Add((DataTable)new DtImageTypeDataTable(dataSet.Tables["dtImageType"]));
				if (dataSet.Tables["dtDevices"] != null)
					base.Tables.Add((DataTable)new dtDevicesDataTable(dataSet.Tables["dtDevices"]));
				if (dataSet.Tables["dtCategories"] != null)
					base.Tables.Add((DataTable)new dtCategoriesDataTable(dataSet.Tables["dtCategories"]));
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
				int num = (int)this.ReadXml(reader);
				this.InitVars();
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
		protected override XmlSchema GetSchemaSerializable()
		{
			MemoryStream memoryStream = new MemoryStream();
			this.WriteXmlSchema((XmlWriter)new XmlTextWriter((Stream)memoryStream, (Encoding)null));
			memoryStream.Position = 0L;
			return XmlSchema.Read((XmlReader)new XmlTextReader((Stream)memoryStream), (ValidationEventHandler)null);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		internal void InitVars()
		{
			this.InitVars(true);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		internal void InitVars(bool initTable)
		{
			this.tabledtResults = (dtResultsDataTable)base.Tables["dtResults"];
			if (initTable && this.tabledtResults != null)
				this.tabledtResults.InitVars();
			this.tabledtImageType = (DtImageTypeDataTable)base.Tables["dtImageType"];
			if (initTable && this.tabledtImageType != null)
				this.tabledtImageType.InitVars();
			this.tabledtDevices = (dtDevicesDataTable)base.Tables["dtDevices"];
			if (initTable && this.tabledtDevices != null)
				this.tabledtDevices.InitVars();
			this.tabledtCategories = (dtCategoriesDataTable)base.Tables["dtCategories"];
			if (initTable && this.tabledtCategories != null)
				this.tabledtCategories.InitVars();
			this.relationdtImageType_dtResults = this.Relations["dtImageType_dtResults"];
			this.relationdtCategories_dtResults = this.Relations["dtCategories_dtResults"];
			this.relationdtDevices_dtResults = this.Relations["dtDevices_dtResults"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		private void InitClass()
		{
			this.DataSetName = "dsResult";
			this.Prefix = "";
			this.Namespace = "http://tempuri.org/dsResult.xsd";
			this.EnforceConstraints = true;
			this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
			this.tabledtResults = new dtResultsDataTable();
			base.Tables.Add((DataTable)this.tabledtResults);
			this.tabledtImageType = new DtImageTypeDataTable();
			base.Tables.Add((DataTable)this.tabledtImageType);
			this.tabledtDevices = new dtDevicesDataTable();
			base.Tables.Add((DataTable)this.tabledtDevices);
			this.tabledtCategories = new dtCategoriesDataTable();
			base.Tables.Add((DataTable)this.tabledtCategories);
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

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		[DebuggerNonUserCode]
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
			xmlSchemaSequence.Items.Add((XmlSchemaObject)new XmlSchemaAny()
			{
				Namespace = dsResult.Namespace
			});
			schemaComplexType.Particle = (XmlSchemaParticle)xmlSchemaSequence;
			XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream1 = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write((Stream)memoryStream1);
					foreach (XmlSchema xmlSchema in (IEnumerable)xs.Schemas(schemaSerializable.TargetNamespace))
					{
						memoryStream2.SetLength(0L);
						xmlSchema.Write((Stream)memoryStream2);
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
		public class DtImageTypeDataTable : TypedTableBase<dtImageTypeRow>
		{
			private DataColumn columntypeID;
			private DataColumn columnimgType;

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public DataColumn typeIDColumn
			{
				get
				{
					return this.columntypeID;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public DataColumn imgTypeColumn
			{
				get
				{
					return this.columnimgType;
				}
			}

			[DebuggerNonUserCode]
			[Browsable(false)]
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
			public dtImageTypeRow this[int index]
			{
				get
				{
					return (dtImageTypeRow)this.Rows[index];
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtImageTypeRowChangeEventHandler dtImageTypeRowChanging;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtImageTypeRowChangeEventHandler dtImageTypeRowChanged;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtImageTypeRowChangeEventHandler dtImageTypeRowDeleting;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtImageTypeRowChangeEventHandler dtImageTypeRowDeleted;

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public DtImageTypeDataTable()
			{
				this.TableName = "dtImageType";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected DtImageTypeDataTable(SerializationInfo info, StreamingContext context)
			  : base(info, context)
			{
				this.InitVars();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void AdddtImageTypeRow(dtImageTypeRow row)
			{
				this.Rows.Add((DataRow)row);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtImageTypeRow AdddtImageTypeRow(int typeID, Bitmap imgType)
			{
				dtImageTypeRow dtImageTypeRow = (dtImageTypeRow)this.NewRow();
				object[] objArray = new object[2]
				{
		  (object) typeID,
		  (object) imgType
				};
				dtImageTypeRow.ItemArray = objArray;
				this.Rows.Add((DataRow)dtImageTypeRow);
				return dtImageTypeRow;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtImageTypeRow FindBytypeID(int typeID)
			{
				return (dtImageTypeRow)this.Rows.Find(new object[1]
				{
		  (object) typeID
				});
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public override DataTable Clone()
			{
				DtImageTypeDataTable imageTypeDataTable = (DtImageTypeDataTable)base.Clone();
				imageTypeDataTable.InitVars();
				return (DataTable)imageTypeDataTable;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override DataTable CreateInstance()
			{
				return (DataTable)new DtImageTypeDataTable();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			internal void InitVars()
			{
				this.columntypeID = this.Columns["typeID"];
				this.columnimgType = this.Columns["imgType"];
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			private void InitClass()
			{
				this.columntypeID = new DataColumn("typeID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columntypeID);
				this.columnimgType = new DataColumn("imgType", typeof(Bitmap), (string)null, MappingType.Element);
				this.Columns.Add(this.columnimgType);
				this.Constraints.Add((Constraint)new UniqueConstraint("Constraint1", new DataColumn[1]
				{
		  this.columntypeID
				}, 1 != 0));
				this.columntypeID.AllowDBNull = false;
				this.columntypeID.Unique = true;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtImageTypeRow NewdtImageTypeRow()
			{
				return (dtImageTypeRow)this.NewRow();
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
			{
				return (DataRow)new dtImageTypeRow(builder);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override Type GetRowType()
			{
				return typeof(dtImageTypeRow);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowChanged(DataRowChangeEventArgs e)
			{
				base.OnRowChanged(e);
				if (this.dtImageTypeRowChanged == null)
					return;
				this.dtImageTypeRowChanged((object)this, new dtImageTypeRowChangeEvent((dtImageTypeRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowChanging(DataRowChangeEventArgs e)
			{
				base.OnRowChanging(e);
				if (this.dtImageTypeRowChanging == null)
					return;
				this.dtImageTypeRowChanging((object)this, new dtImageTypeRowChangeEvent((dtImageTypeRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override void OnRowDeleted(DataRowChangeEventArgs e)
			{
				base.OnRowDeleted(e);
				if (this.dtImageTypeRowDeleted == null)
					return;
				this.dtImageTypeRowDeleted((object)this, new dtImageTypeRowChangeEvent((dtImageTypeRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleting(DataRowChangeEventArgs e)
			{
				base.OnRowDeleting(e);
				if (this.dtImageTypeRowDeleting == null)
					return;
				this.dtImageTypeRowDeleting((object)this, new dtImageTypeRowChangeEvent((dtImageTypeRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void RemovedtImageTypeRow(dtImageTypeRow row)
			{
				this.Rows.Remove((DataRow)row);
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
				xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte)0);
				xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny1);
				XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
				xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
				xmlSchemaAny2.MinOccurs = new Decimal(1);
				xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny2);
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "namespace",
					FixedValue = dsResult.Namespace
				});
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "tableTypeName",
					FixedValue = "dtImageTypeDataTable"
				});
				schemaComplexType.Particle = (XmlSchemaParticle)xmlSchemaSequence;
				XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
				if (xs.Contains(schemaSerializable.TargetNamespace))
				{
					MemoryStream memoryStream1 = new MemoryStream();
					MemoryStream memoryStream2 = new MemoryStream();
					try
					{
						schemaSerializable.Write((Stream)memoryStream1);
						foreach (XmlSchema xmlSchema in (IEnumerable)xs.Schemas(schemaSerializable.TargetNamespace))
						{
							memoryStream2.SetLength(0L);
							xmlSchema.Write((Stream)memoryStream2);
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
		public delegate void dtResultsRowChangeEventHandler(object sender, dtResultsRowChangeEvent e);

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public delegate void dtImageTypeRowChangeEventHandler(object sender, dtImageTypeRowChangeEvent e);

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public delegate void dtDevicesRowChangeEventHandler(object sender, dtDevicesRowChangeEvent e);

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public delegate void dtCategoriesRowChangeEventHandler(object sender, dtCategoriesRowChangeEvent e);

		[XmlSchemaProvider("GetTypedTableSchema")]
		[Serializable]
		public class dtResultsDataTable : TypedTableBase<dtResultsRow>
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public DataColumn IDColumn
			{
				get
				{
					return this.columnID;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public DataColumn ScoreColumn
			{
				get
				{
					return this.columnScore;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public DataColumn DBNameColumn
			{
				get
				{
					return this.columnDBName;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtResultsRow this[int index]
			{
				get
				{
					return (dtResultsRow)this.Rows[index];
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtResultsRowChangeEventHandler dtResultsRowChanging;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtResultsRowChangeEventHandler dtResultsRowChanged;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtResultsRowChangeEventHandler dtResultsRowDeleting;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtResultsRowChangeEventHandler dtResultsRowDeleted;

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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected dtResultsDataTable(SerializationInfo info, StreamingContext context)
			  : base(info, context)
			{
				this.InitVars();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void AdddtResultsRow(dtResultsRow row)
			{
				this.Rows.Add((DataRow)row);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtResultsRow AdddtResultsRow(Guid ID, Guid FaceID, dtDevicesRow parentdtDevicesRowBydtDevices_dtResults, dtCategoriesRow parentdtCategoriesRowBydtCategories_dtResults, int ObjID, dtImageTypeRow parentdtImageTypeRowBydtImageType_dtResults, DateTime Date, float Score, Bitmap ImageIcon, string Name, bool Status, string DBName)
			{
				dtResultsRow dtResultsRow = (dtResultsRow)this.NewRow();
				object[] objArray = new object[12]
				{
					ID,
					FaceID,
					null,
					null,
					ObjID,
					null,
					Date,
					Score,
					ImageIcon,
					Name,
					Status,
					DBName
				};
				if (parentdtDevicesRowBydtDevices_dtResults != null)
					objArray[2] = parentdtDevicesRowBydtDevices_dtResults[0];
				if (parentdtCategoriesRowBydtCategories_dtResults != null)
					objArray[3] = parentdtCategoriesRowBydtCategories_dtResults[0];
				if (parentdtImageTypeRowBydtImageType_dtResults != null)
					objArray[5] = parentdtImageTypeRowBydtImageType_dtResults[0];
				dtResultsRow.ItemArray = objArray;
				this.Rows.Add((DataRow)dtResultsRow);
				return dtResultsRow;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtResultsRow FindByID(Guid ID)
			{
				return (dtResultsRow)this.Rows.Find(new object[1]
				{
		  (object) ID
				});
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public override DataTable Clone()
			{
				dtResultsDataTable resultsDataTable = (dtResultsDataTable)base.Clone();
				resultsDataTable.InitVars();
				return (DataTable)resultsDataTable;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override DataTable CreateInstance()
			{
				return (DataTable)new dtResultsDataTable();
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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
				this.columnID = new DataColumn("ID", typeof(Guid), (string)null, MappingType.Element);
				this.Columns.Add(this.columnID);
				this.columnFaceID = new DataColumn("FaceID", typeof(Guid), (string)null, MappingType.Element);
				this.Columns.Add(this.columnFaceID);
				this.columnDevID = new DataColumn("DevID", typeof(Guid), (string)null, MappingType.Element);
				this.Columns.Add(this.columnDevID);
				this.columnCatID = new DataColumn("CatID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columnCatID);
				this.columnObjID = new DataColumn("ObjID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columnObjID);
				this.columnimgTypeID = new DataColumn("imgTypeID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columnimgTypeID);
				this.columnDate = new DataColumn("Date", typeof(DateTime), (string)null, MappingType.Element);
				this.Columns.Add(this.columnDate);
				this.columnScore = new DataColumn("Score", typeof(float), (string)null, MappingType.Element);
				this.Columns.Add(this.columnScore);
				this.columnImageIcon = new DataColumn("ImageIcon", typeof(Bitmap), (string)null, MappingType.Element);
				this.Columns.Add(this.columnImageIcon);
				this.columnName = new DataColumn("Name", typeof(string), (string)null, MappingType.Element);
				this.Columns.Add(this.columnName);
				this.columnStatus = new DataColumn("Status", typeof(bool), (string)null, MappingType.Element);
				this.Columns.Add(this.columnStatus);
				this.columnDBName = new DataColumn("DBName", typeof(string), (string)null, MappingType.Element);
				this.Columns.Add(this.columnDBName);
				this.Constraints.Add((Constraint)new UniqueConstraint("Constraint1", new DataColumn[1]
				{
		  this.columnID
				}, 1 != 0));
				this.columnID.AllowDBNull = false;
				this.columnID.Unique = true;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtResultsRow NewdtResultsRow()
			{
				return (dtResultsRow)this.NewRow();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
			{
				return (DataRow)new dtResultsRow(builder);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override Type GetRowType()
			{
				return typeof(dtResultsRow);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override void OnRowChanged(DataRowChangeEventArgs e)
			{
				base.OnRowChanged(e);
				if (this.dtResultsRowChanged == null)
					return;
				this.dtResultsRowChanged((object)this, new dtResultsRowChangeEvent((dtResultsRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowChanging(DataRowChangeEventArgs e)
			{
				base.OnRowChanging(e);
				if (this.dtResultsRowChanging == null)
					return;
				this.dtResultsRowChanging((object)this, new dtResultsRowChangeEvent((dtResultsRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleted(DataRowChangeEventArgs e)
			{
				base.OnRowDeleted(e);
				if (this.dtResultsRowDeleted == null)
					return;
				this.dtResultsRowDeleted((object)this, new dtResultsRowChangeEvent((dtResultsRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleting(DataRowChangeEventArgs e)
			{
				base.OnRowDeleting(e);
				if (this.dtResultsRowDeleting == null)
					return;
				this.dtResultsRowDeleting((object)this, new dtResultsRowChangeEvent((dtResultsRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public void RemovedtResultsRow(dtResultsRow row)
			{
				this.Rows.Remove((DataRow)row);
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
				xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte)0);
				xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny1);
				XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
				xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
				xmlSchemaAny2.MinOccurs = new Decimal(1);
				xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny2);
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "namespace",
					FixedValue = dsResult.Namespace
				});
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "tableTypeName",
					FixedValue = "dtResultsDataTable"
				});
				schemaComplexType.Particle = (XmlSchemaParticle)xmlSchemaSequence;
				XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
				if (xs.Contains(schemaSerializable.TargetNamespace))
				{
					MemoryStream memoryStream1 = new MemoryStream();
					MemoryStream memoryStream2 = new MemoryStream();
					try
					{
						schemaSerializable.Write((Stream)memoryStream1);
						foreach (XmlSchema xmlSchema in (IEnumerable)xs.Schemas(schemaSerializable.TargetNamespace))
						{
							memoryStream2.SetLength(0L);
							xmlSchema.Write((Stream)memoryStream2);
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
		public class dtDevicesDataTable : TypedTableBase<dtDevicesRow>
		{
			private DataColumn columnDeviceID;
			private DataColumn columnDeviceName;
			private DataColumn columnObjectID;
			private DataColumn columnTableID;
			private DataColumn columnPosition;

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public DataColumn DeviceIDColumn
			{
				get
				{
					return this.columnDeviceID;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtDevicesRow this[int index]
			{
				get
				{
					return (dtDevicesRow)this.Rows[index];
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtDevicesRowChangeEventHandler dtDevicesRowChanging;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtDevicesRowChangeEventHandler dtDevicesRowChanged;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtDevicesRowChangeEventHandler dtDevicesRowDeleting;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtDevicesRowChangeEventHandler dtDevicesRowDeleted;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtDevicesDataTable()
			{
				this.TableName = "dtDevices";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public void AdddtDevicesRow(dtDevicesRow row)
			{
				this.Rows.Add((DataRow)row);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtDevicesRow AdddtDevicesRow(Guid DeviceID, string DeviceName, int ObjectID, Guid TableID, string Position)
			{
				dtDevicesRow dtDevicesRow = (dtDevicesRow)this.NewRow();
				object[] objArray = new object[5]
				{
		  (object) DeviceID,
		  (object) DeviceName,
		  (object) ObjectID,
		  (object) TableID,
		  (object) Position
				};
				dtDevicesRow.ItemArray = objArray;
				this.Rows.Add((DataRow)dtDevicesRow);
				return dtDevicesRow;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtDevicesRow FindByDeviceID(Guid DeviceID)
			{
				return (dtDevicesRow)this.Rows.Find(new object[1]
				{
		  (object) DeviceID
				});
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public override DataTable Clone()
			{
				dtDevicesDataTable devicesDataTable = (dtDevicesDataTable)base.Clone();
				devicesDataTable.InitVars();
				return (DataTable)devicesDataTable;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override DataTable CreateInstance()
			{
				return (DataTable)new dtDevicesDataTable();
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			private void InitClass()
			{
				this.columnDeviceID = new DataColumn("DeviceID", typeof(Guid), (string)null, MappingType.Element);
				this.Columns.Add(this.columnDeviceID);
				this.columnDeviceName = new DataColumn("DeviceName", typeof(string), (string)null, MappingType.Element);
				this.Columns.Add(this.columnDeviceName);
				this.columnObjectID = new DataColumn("ObjectID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columnObjectID);
				this.columnTableID = new DataColumn("TableID", typeof(Guid), (string)null, MappingType.Element);
				this.Columns.Add(this.columnTableID);
				this.columnPosition = new DataColumn("Position", typeof(string), (string)null, MappingType.Element);
				this.Columns.Add(this.columnPosition);
				this.Constraints.Add((Constraint)new UniqueConstraint("Constraint1", new DataColumn[1]
				{
		  this.columnDeviceID
				}, 1 != 0));
				this.columnDeviceID.AllowDBNull = false;
				this.columnDeviceID.Unique = true;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtDevicesRow NewdtDevicesRow()
			{
				return (dtDevicesRow)this.NewRow();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
			{
				return (DataRow)new dtDevicesRow(builder);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override Type GetRowType()
			{
				return typeof(dtDevicesRow);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowChanged(DataRowChangeEventArgs e)
			{
				base.OnRowChanged(e);
				if (this.dtDevicesRowChanged == null)
					return;
				this.dtDevicesRowChanged((object)this, new dtDevicesRowChangeEvent((dtDevicesRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override void OnRowChanging(DataRowChangeEventArgs e)
			{
				base.OnRowChanging(e);
				if (this.dtDevicesRowChanging == null)
					return;
				this.dtDevicesRowChanging((object)this, new dtDevicesRowChangeEvent((dtDevicesRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleted(DataRowChangeEventArgs e)
			{
				base.OnRowDeleted(e);
				if (this.dtDevicesRowDeleted == null)
					return;
				this.dtDevicesRowDeleted((object)this, new dtDevicesRowChangeEvent((dtDevicesRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleting(DataRowChangeEventArgs e)
			{
				base.OnRowDeleting(e);
				if (this.dtDevicesRowDeleting == null)
					return;
				this.dtDevicesRowDeleting((object)this, new dtDevicesRowChangeEvent((dtDevicesRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void RemovedtDevicesRow(dtDevicesRow row)
			{
				this.Rows.Remove((DataRow)row);
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
				xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte)0);
				xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny1);
				XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
				xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
				xmlSchemaAny2.MinOccurs = new Decimal(1);
				xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny2);
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "namespace",
					FixedValue = dsResult.Namespace
				});
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "tableTypeName",
					FixedValue = "dtDevicesDataTable"
				});
				schemaComplexType.Particle = (XmlSchemaParticle)xmlSchemaSequence;
				XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
				if (xs.Contains(schemaSerializable.TargetNamespace))
				{
					MemoryStream memoryStream1 = new MemoryStream();
					MemoryStream memoryStream2 = new MemoryStream();
					try
					{
						schemaSerializable.Write((Stream)memoryStream1);
						foreach (XmlSchema xmlSchema in (IEnumerable)xs.Schemas(schemaSerializable.TargetNamespace))
						{
							memoryStream2.SetLength(0L);
							xmlSchema.Write((Stream)memoryStream2);
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
		public class dtCategoriesDataTable : TypedTableBase<dtCategoriesRow>
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtCategoriesRow this[int index]
			{
				get
				{
					return (dtCategoriesRow)this.Rows[index];
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtCategoriesRowChangeEventHandler dtCategoriesRowChanging;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtCategoriesRowChangeEventHandler dtCategoriesRowChanged;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtCategoriesRowChangeEventHandler dtCategoriesRowDeleting;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public event dtCategoriesRowChangeEventHandler dtCategoriesRowDeleted;

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtCategoriesDataTable()
			{
				this.TableName = "dtCategories";
				this.BeginInit();
				this.InitClass();
				this.EndInit();
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected dtCategoriesDataTable(SerializationInfo info, StreamingContext context)
			  : base(info, context)
			{
				this.InitVars();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void AdddtCategoriesRow(dtCategoriesRow row)
			{
				this.Rows.Add((DataRow)row);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtCategoriesRow AdddtCategoriesRow(int CategoryID, string Category)
			{
				dtCategoriesRow dtCategoriesRow = (dtCategoriesRow)this.NewRow();
				object[] objArray = new object[2]
				{
		  (object) CategoryID,
		  (object) Category
				};
				dtCategoriesRow.ItemArray = objArray;
				this.Rows.Add((DataRow)dtCategoriesRow);
				return dtCategoriesRow;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtCategoriesRow FindByCategoryID(int CategoryID)
			{
				return (dtCategoriesRow)this.Rows.Find(new object[1]
				{
		  (object) CategoryID
				});
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public override DataTable Clone()
			{
				dtCategoriesDataTable categoriesDataTable = (dtCategoriesDataTable)base.Clone();
				categoriesDataTable.InitVars();
				return (DataTable)categoriesDataTable;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override DataTable CreateInstance()
			{
				return (DataTable)new dtCategoriesDataTable();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			internal void InitVars()
			{
				this.columnCategoryID = this.Columns["CategoryID"];
				this.columnCategory = this.Columns["Category"];
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			private void InitClass()
			{
				this.columnCategoryID = new DataColumn("CategoryID", typeof(int), (string)null, MappingType.Element);
				this.Columns.Add(this.columnCategoryID);
				this.columnCategory = new DataColumn("Category", typeof(string), (string)null, MappingType.Element);
				this.Columns.Add(this.columnCategory);
				this.Constraints.Add((Constraint)new UniqueConstraint("Constraint1", new DataColumn[1]
				{
		  this.columnCategoryID
				}, 1 != 0));
				this.columnCategoryID.AllowDBNull = false;
				this.columnCategoryID.Unique = true;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtCategoriesRow NewdtCategoriesRow()
			{
				return (dtCategoriesRow)this.NewRow();
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
			{
				return (DataRow)new dtCategoriesRow(builder);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override Type GetRowType()
			{
				return typeof(dtCategoriesRow);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowChanged(DataRowChangeEventArgs e)
			{
				base.OnRowChanged(e);
				if (this.dtCategoriesRowChanged == null)
					return;
				this.dtCategoriesRowChanged((object)this, new dtCategoriesRowChangeEvent((dtCategoriesRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override void OnRowChanging(DataRowChangeEventArgs e)
			{
				base.OnRowChanging(e);
				if (this.dtCategoriesRowChanging == null)
					return;
				this.dtCategoriesRowChanging((object)this, new dtCategoriesRowChangeEvent((dtCategoriesRow)e.Row, e.Action));
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			protected override void OnRowDeleted(DataRowChangeEventArgs e)
			{
				base.OnRowDeleted(e);
				if (this.dtCategoriesRowDeleted == null)
					return;
				this.dtCategoriesRowDeleted((object)this, new dtCategoriesRowChangeEvent((dtCategoriesRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			protected override void OnRowDeleting(DataRowChangeEventArgs e)
			{
				base.OnRowDeleting(e);
				if (this.dtCategoriesRowDeleting == null)
					return;
				this.dtCategoriesRowDeleting((object)this, new dtCategoriesRowChangeEvent((dtCategoriesRow)e.Row, e.Action));
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public void RemovedtCategoriesRow(dtCategoriesRow row)
			{
				this.Rows.Remove((DataRow)row);
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
				xmlSchemaAny1.MaxOccurs = new Decimal(-1, -1, -1, false, (byte)0);
				xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny1);
				XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
				xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
				xmlSchemaAny2.MinOccurs = new Decimal(1);
				xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
				xmlSchemaSequence.Items.Add((XmlSchemaObject)xmlSchemaAny2);
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "namespace",
					FixedValue = dsResult.Namespace
				});
				schemaComplexType.Attributes.Add((XmlSchemaObject)new XmlSchemaAttribute()
				{
					Name = "tableTypeName",
					FixedValue = "dtCategoriesDataTable"
				});
				schemaComplexType.Particle = (XmlSchemaParticle)xmlSchemaSequence;
				XmlSchema schemaSerializable = dsResult.GetSchemaSerializable();
				if (xs.Contains(schemaSerializable.TargetNamespace))
				{
					MemoryStream memoryStream1 = new MemoryStream();
					MemoryStream memoryStream2 = new MemoryStream();
					try
					{
						schemaSerializable.Write((Stream)memoryStream1);
						foreach (XmlSchema xmlSchema in (IEnumerable)xs.Schemas(schemaSerializable.TargetNamespace))
						{
							memoryStream2.SetLength(0L);
							xmlSchema.Write((Stream)memoryStream2);
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
			private dtResultsDataTable tabledtResults;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public Guid ID
			{
				get
				{
					return (Guid)this[this.tabledtResults.IDColumn];
				}
				set
				{
					this[this.tabledtResults.IDColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public Guid FaceID
			{
				get
				{
					try
					{
						return (Guid)this[this.tabledtResults.FaceIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'FaceID' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.FaceIDColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public Guid DevID
			{
				get
				{
					try
					{
						return (Guid)this[this.tabledtResults.DevIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'DevID' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.DevIDColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public int CatID
			{
				get
				{
					try
					{
						return (int)this[this.tabledtResults.CatIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'CatID' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.CatIDColumn] = (object)value;
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
						return (int)this[this.tabledtResults.ObjIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'ObjID' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.ObjIDColumn] = (object)value;
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
						return (int)this[this.tabledtResults.imgTypeIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'imgTypeID' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.imgTypeIDColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public DateTime Date
			{
				get
				{
					try
					{
						return (DateTime)this[this.tabledtResults.DateColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Date' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.DateColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public float Score
			{
				get
				{
					try
					{
						return (float)this[this.tabledtResults.ScoreColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Score' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.ScoreColumn] = (object)value;
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
						return (Bitmap)this[this.tabledtResults.ImageIconColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'ImageIcon' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.ImageIconColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public string Name
			{
				get
				{
					try
					{
						return (string)this[this.tabledtResults.NameColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Name' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.NameColumn] = (object)value;
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
						return (bool)this[this.tabledtResults.StatusColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Status' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.StatusColumn] = value;
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
						return (string)this[this.tabledtResults.DBNameColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'DBName' in table 'dtResults' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtResults.DBNameColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtImageTypeRow dtImageTypeRow
			{
				get
				{
					return (dtImageTypeRow)this.GetParentRow(this.Table.ParentRelations["dtImageType_dtResults"]);
				}
				set
				{
					this.SetParentRow((DataRow)value, this.Table.ParentRelations["dtImageType_dtResults"]);
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtCategoriesRow dtCategoriesRow
			{
				get
				{
					return (dtCategoriesRow)this.GetParentRow(this.Table.ParentRelations["dtCategories_dtResults"]);
				}
				set
				{
					this.SetParentRow((DataRow)value, this.Table.ParentRelations["dtCategories_dtResults"]);
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtDevicesRow dtDevicesRow
			{
				get
				{
					return (dtDevicesRow)this.GetParentRow(this.Table.ParentRelations["dtDevices_dtResults"]);
				}
				set
				{
					this.SetParentRow((DataRow)value, this.Table.ParentRelations["dtDevices_dtResults"]);
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			internal dtResultsRow(DataRowBuilder rb)
			  : base(rb)
			{
				this.tabledtResults = (dtResultsDataTable)this.Table;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public bool IsFaceIDNull()
			{
				return this.IsNull(this.tabledtResults.FaceIDColumn);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public bool IsObjIDNull()
			{
				return this.IsNull(this.tabledtResults.ObjIDColumn);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public void SetImageIconNull()
			{
				this[this.tabledtResults.ImageIconColumn] = Convert.DBNull;
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public bool IsDBNameNull()
			{
				return this.IsNull(this.tabledtResults.DBNameColumn);
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public void SetDBNameNull()
			{
				this[this.tabledtResults.DBNameColumn] = Convert.DBNull;
			}
		}

		public class dtImageTypeRow : DataRow
		{
			private DtImageTypeDataTable tabledtImageType;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public int typeID
			{
				get
				{
					return (int)this[this.tabledtImageType.typeIDColumn];
				}
				set
				{
					this[this.tabledtImageType.typeIDColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public Bitmap imgType
			{
				get
				{
					try
					{
						return (Bitmap)this[this.tabledtImageType.imgTypeColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'imgType' in table 'dtImageType' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtImageType.imgTypeColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			internal dtImageTypeRow(DataRowBuilder rb)
			  : base(rb)
			{
				this.tabledtImageType = (DtImageTypeDataTable)this.Table;
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtResultsRow[] GetdtResultsRows()
			{
				if (this.Table.ChildRelations["dtImageType_dtResults"] == null)
					return new dtResultsRow[0];
				return (dtResultsRow[])this.GetChildRows(this.Table.ChildRelations["dtImageType_dtResults"]);
			}
		}

		public class dtDevicesRow : DataRow
		{
			private dtDevicesDataTable tabledtDevices;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public Guid DeviceID
			{
				get
				{
					return (Guid)this[this.tabledtDevices.DeviceIDColumn];
				}
				set
				{
					this[this.tabledtDevices.DeviceIDColumn] = (object)value;
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
						return (string)this[this.tabledtDevices.DeviceNameColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'DeviceName' in table 'dtDevices' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtDevices.DeviceNameColumn] = (object)value;
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
						return (int)this[this.tabledtDevices.ObjectIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'ObjectID' in table 'dtDevices' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtDevices.ObjectIDColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public Guid TableID
			{
				get
				{
					try
					{
						return (Guid)this[this.tabledtDevices.TableIDColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'TableID' in table 'dtDevices' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtDevices.TableIDColumn] = (object)value;
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
						return (string)this[this.tabledtDevices.PositionColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Position' in table 'dtDevices' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtDevices.PositionColumn] = (object)value;
				}
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			internal dtDevicesRow(DataRowBuilder rb)
			  : base(rb)
			{
				this.tabledtDevices = (dtDevicesDataTable)this.Table;
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public bool IsObjectIDNull()
			{
				return this.IsNull(this.tabledtDevices.ObjectIDColumn);
			}

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
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
			public dtResultsRow[] GetdtResultsRows()
			{
				if (this.Table.ChildRelations["dtDevices_dtResults"] == null)
					return new dtResultsRow[0];
				return (dtResultsRow[])this.GetChildRows(this.Table.ChildRelations["dtDevices_dtResults"]);
			}
		}

		public class dtCategoriesRow : DataRow
		{
			private dtCategoriesDataTable tabledtCategories;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public int CategoryID
			{
				get
				{
					return (int)this[this.tabledtCategories.CategoryIDColumn];
				}
				set
				{
					this[this.tabledtCategories.CategoryIDColumn] = (object)value;
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
						return (string)this[this.tabledtCategories.CategoryColumn];
					}
					catch (InvalidCastException ex)
					{
						throw new StrongTypingException("The value for column 'Category' in table 'dtCategories' is DBNull.", (Exception)ex);
					}
				}
				set
				{
					this[this.tabledtCategories.CategoryColumn] = (object)value;
				}
			}

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			internal dtCategoriesRow(DataRowBuilder rb)
			  : base(rb)
			{
				this.tabledtCategories = (dtCategoriesDataTable)this.Table;
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
			public dtResultsRow[] GetdtResultsRows()
			{
				if (this.Table.ChildRelations["dtCategories_dtResults"] == null)
					return new dtResultsRow[0];
				return (dtResultsRow[])this.GetChildRows(this.Table.ChildRelations["dtCategories_dtResults"]);
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public class dtResultsRowChangeEvent : EventArgs
		{
			private dtResultsRow eventRow;
			private DataRowAction eventAction;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtResultsRow Row
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

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtResultsRowChangeEvent(dtResultsRow row, DataRowAction action)
			{
				this.eventRow = row;
				this.eventAction = action;
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public class dtImageTypeRowChangeEvent : EventArgs
		{
			private dtImageTypeRow eventRow;
			private DataRowAction eventAction;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtImageTypeRow Row
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
			public dtImageTypeRowChangeEvent(dtImageTypeRow row, DataRowAction action)
			{
				this.eventRow = row;
				this.eventAction = action;
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public class dtDevicesRowChangeEvent : EventArgs
		{
			private dtDevicesRow eventRow;
			private DataRowAction eventAction;

			[DebuggerNonUserCode]
			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			public dtDevicesRow Row
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
			public dtDevicesRowChangeEvent(dtDevicesRow row, DataRowAction action)
			{
				this.eventRow = row;
				this.eventAction = action;
			}
		}

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
		public class dtCategoriesRowChangeEvent : EventArgs
		{
			private dtCategoriesRow eventRow;
			private DataRowAction eventAction;

			[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
			[DebuggerNonUserCode]
			public dtCategoriesRow Row
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
			public dtCategoriesRowChangeEvent(dtCategoriesRow row, DataRowAction action)
			{
				this.eventRow = row;
				this.eventAction = action;
			}
		}
	}
}
