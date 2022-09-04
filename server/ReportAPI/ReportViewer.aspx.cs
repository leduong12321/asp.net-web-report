using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ReportAPI
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        private List<SqlParameter> sqlParameters;
        private DataSet dataSet; 
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    SetSqlParameter();
                    GetDataTable();
                    GenerateReport();
                }
                catch
                {
                    Console.WriteLine("Error has occurred while processing the report");
                }
            }
        }

        private void SetSqlParameter()
        {
            sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("DOWNCOILER", Request.QueryString["downcoiler"]));
        }

        private void GetDataTable()
        {
            string query = "SELECT A.TASK_SLAB," +
                                     "A.SLAB_ID," +
                                     "A.STEEL_GRADE," +
                                     "A.ENTRY_THICK," +
                                     "A.ENTRY_WIDTH," +
                                     "A.TARGET_THICK," +
                                     "A.TARGET_WIDTH," +
                                     "(SELECT MAX_VALUE FROM SC_MEASURE_DEVICE AS SC_MEASURE_DEVICE_4 WHERE (PIECE_ID = A.PIECE_ID) AND (DEVICE_ID = 'THICK')) AS MAX_THIK," +
                                     "(SELECT AVG_VALUE FROM SC_MEASURE_DEVICE WHERE (PIECE_ID = A.PIECE_ID) AND (DEVICE_ID = 'THICK')) AS MEAS_THIK," +
                                     "(SELECT MIN_VALUE FROM SC_MEASURE_DEVICE AS SC_MEASURE_DEVICE_3 WHERE (PIECE_ID = A.PIECE_ID) AND (DEVICE_ID = 'THICK')) AS MIN_THIK," +
                                     "C.MAX_VALUE," +
                                     "C.AVG_VALUE," +
                                     "C.MIN_VALUE," +
                                     "(SELECT AVG_VALUE FROM SC_MEASURE_DEVICE AS SC_MEASURE_DEVICE_2 WHERE (PIECE_ID = A.PIECE_ID) AND (DEVICE_ID = 'CROWN')) AS AVG_CROWN," +
                                     "(SELECT AVG_VALUE FROM SC_MEASURE_DEVICE AS SC_MEASURE_DEVICE_1 WHERE (PIECE_ID = A.PIECE_ID) AND (DEVICE_ID = 'WEDGE')) AS AVG_WEDGE," +
                                     "CAST(ROUND(E.AVG_VALUE, 2) AS FLOAT) AS FLATNESS," +
                                     "CAST(ROUND(E.AVG_HEAD_ASIM, 2) AS FLOAT) AS ASymHeadFlatness," +
                                     "CAST(ROUND(E.AVG_BODY_ASIM, 2) AS FLOAT) AS ASymBodyFlatness," +
                                     "CAST(ROUND(E.AVG_TAIL_ASIM, 2) AS FLOAT) AS ASymTailFlatness," +
                                     "CAST(ROUND(E.AVG_HEAD, 2) AS FLOAT) AS SymHeadFlatness," +
                                     "CAST(ROUND(E.AVG_BODY, 2) AS FLOAT) AS SymBodyFlatness," +
                                     "CAST(ROUND(E.AVG_TAIL, 2) AS FLOAT) AS SymTailFlatness," +
                                     "(SELECT DSC1_RAMP1 FROM SC_MEASURE_MILL WHERE (PIECE_ID = A.PIECE_ID) AND (STAND_ID = 'H1')) AS DSC1_RAMP1," +
                                     "B.DSC2_RAMP1," +
                                     "A.STATUS," +
                                     "A.DOWNCOILER," +
                                     "A.ARCHIVE_DATE " +
                                     "FROM PM_PIECE_OUTPUT AS A INNER JOIN SC_MEASURE_MILL AS B ON A.PIECE_ID = B.PIECE_ID INNER JOIN SC_MEASURE_DEVICE AS C ON A.PIECE_ID = C.PIECE_ID INNER JOIN SC_PROFILE_MEASURE AS D ON A.PIECE_ID = D.PIECE_ID INNER JOIN SC_FLAT_MEASURE AS E ON A.PIECE_ID = E.PIECE_ID " +
                                     "WHERE A.archive_date >='2021/12/01 00:00:00 AM' AND A.archive_date <='2022/08/29 23:59:00 PM' AND (B.STAND_ID = 'F4') AND (C.DEVICE_ID = 'WIDTH1') AND (A.PIECE_ID NOT LIKE 'GHOST%' OR A.PIECE_ID = 'GHOST82') AND A.DOWNCOILER = @downcoiler";
            string connectionString = ConfigurationManager.ConnectionStrings["HSM_HOAPHATConnectionString"].ConnectionString;
            dataSet = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    dynamic abc = sqlParameters.ToArray();
                    if (abc[0].Value == null)
                    {
                        abc[0].Value = 1;
                    }
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                }
            }
        }
        private void GenerateReport()
        {
            RdlcReportViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/ProductHSMReport.rdlc");
            RdlcReportViewer.LocalReport.DataSources.Clear();
            RdlcReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BaoCaoSanXuatDataset", dataSet.Tables[0]));
            RdlcReportViewer.LocalReport.Refresh();
        }
    }
}