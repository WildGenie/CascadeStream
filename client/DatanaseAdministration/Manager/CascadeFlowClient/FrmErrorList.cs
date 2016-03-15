using System;
using System.Data;
using DevExpress.XtraEditors;

namespace CascadeManager.CascadeFlowClient
{
	public partial class FrmErrorList : XtraForm
	{
		private readonly FrmImport _mainform;
		public FrmErrorList()
		{
			InitializeComponent();
		}

		public FrmErrorList(FrmImport frm)
		{
			InitializeComponent();
			_mainform = frm;
		}

		private void frmErrorList_Load(object sender, EventArgs e)
		{
			var dataTable = new DataTable();
			dataTable.Columns.Add("FileName");
			dataTable.Columns.Add("Error");
			dataTable.Columns.Add("Comment");
			for (int index = 0; index < _mainform.Errors.Count; ++index)
				dataTable.Rows.Add(_mainform.ErrorFiles[index], _mainform.Errors[index], "");
			gcErrors.DataSource = dataTable;
		}
	}
}
