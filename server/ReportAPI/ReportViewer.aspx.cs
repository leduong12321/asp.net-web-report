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
                    //GenerateReport();
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
            sqlParameters.Add(new SqlParameter("FromDay", Request.QueryString["from"]));
            sqlParameters.Add(new SqlParameter("ToDay", Request.QueryString["to"]));
        }
        private void GetDataTable()
        {
            string query = @"SELECT ARCHIVE_DATE,
                                    SHIFT,
                                    CREW_ID,
                                    (ROW_NUMBER() OVER (ORDER BY A.LAST_UPD)) AS Sothutu,
                                    A.PIECE_ID,
                                    A.STATUS,
                                    A.LAST_UPD,
                                    A.DOWNCOILER,
                                    A.TARGET_THICK,
                                    TASK_SLAB,
                                    SLAB_ID,
                                    STEEL_GRADE,
                                    ENTRY_THICK,
                                    ENTRY_WIDTH,
                                    TARGET_CROWN,
                                    TARGET_WIDTH,
                                    B.DSC2_RAMP1,

                                (SELECT DSC1_RAMP1
                                FROM SC_MEASURE_MILL
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND STAND_ID = 'H1') AS DSC1_RAMP1,

                                (SELECT AVG_VALUE
                                FROM [dbo].[SC_MEASURE_DEVICE]
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND DEVICE_ID = 'THICK') AS MEAS_THIK,

                                (SELECT MAX_VALUE
                                FROM [dbo].[SC_MEASURE_DEVICE]
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND DEVICE_ID = 'THICK') AS MAX_THIK,

                                (SELECT MIN_VALUE
                                FROM [dbo].[SC_MEASURE_DEVICE]
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND DEVICE_ID = 'THICK') AS MIN_THIK,
                                    C.AVG_VALUE,
                                    C.MAX_VALUE,
                                    C.MIN_VALUE,

                                (SELECT AVG_VALUE
                                FROM [dbo].[SC_MEASURE_DEVICE]
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND DEVICE_ID = 'CROWN') AS AVG_CROWN,

                                (SELECT AVG_VALUE
                                FROM [dbo].[SC_MEASURE_DEVICE]
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND DEVICE_ID = 'WEDGE') AS AVG_WEDGE,
                                    CAST(ROUND(E.AVG_VALUE, 2) AS FLOAT) AS FLATNEES,
                                    CAST(ROUND(E.AVG_HEAD, 2) AS FLOAT) AS SymHeadFlatness,
                                    CAST(ROUND(E.AVG_BODY, 2) AS FLOAT) AS SymBodyFlatness,
                                    CAST(ROUND(E.AVG_TAIL, 2) AS FLOAT) AS SymTailFlatness,
                                    CAST(ROUND(E.AVG_HEAD_ASIM, 2) AS FLOAT) AS ASymHeadFlatness,
                                    CAST(ROUND(E.AVG_BODY_ASIM, 2) AS FLOAT) AS ASymBodyFlatness,
                                    CAST(ROUND(E.AVG_TAIL_ASIM, 2) AS FLOAT) AS ASymTailFlatness
                            FROM [dbo].[PM_PIECE_OUTPUT] A
                            LEFT OUTER JOIN (SELECT * FROM [dbo].[SC_MEASURE_MILL] WHERE STAND_ID = 'F4') B ON A.PIECE_ID = B.PIECE_ID
                            LEFT OUTER JOIN (SELECT * FROM [dbo].[SC_MEASURE_DEVICE] WHERE DEVICE_ID = 'WIDTH1') C ON A.PIECE_ID = C.PIECE_ID
                            JOIN [dbo].[SC_PROFILE_MEASURE] D ON A.PIECE_ID = D.PIECE_ID
                            JOIN [dbo].[SC_FLAT_MEASURE] E ON A.PIECE_ID = E.PIECE_ID
                            WHERE A.archive_date >= @FromDay
                                AND A.archive_date <= @ToDay
                                AND (A.PIECE_ID not like 'GHOST%'
                                    OR A.PIECE_ID = 'GHOST82')
                            ORDER BY Sothutu";
            string connectionString = ConfigurationManager.ConnectionStrings["HSM_HOAPHATConnectionString"].ConnectionString;
            dataSet = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    dynamic time = sqlParameters.ToArray();
                    if (time[0].Value != null && time[1].Value != null)
                    {
                        long from = long.Parse(time[0].Value);
                        long to = long.Parse(time[1].Value);
                        time[0].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(from);
                        time[1].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(to);
                    } else
                    {
                        time[0].Value = 1659312000000;
                        time[1].Value = 1662163200000;
                    }
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    RdlcReportViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/ReportBaoCaoSanXuat.rdlc");
                    RdlcReportViewer.LocalReport.DataSources.Clear();
                    
                    RdlcReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BaoCaoSanXuatDataset", dataSet.Tables[0]));
                    RdlcReportViewer.LocalReport.Refresh();

                    RdlcReportViewer.LocalReport.DisplayName = "Baocaosanxuat " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", time[0].Value.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", time[1].Value.ToString());
                    RdlcReportViewer.LocalReport.SetParameters(fromDateValue);
                    RdlcReportViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
        private void GenerateReport()
        {
            RdlcReportViewer.LocalReport.DataSources.Clear();
            RdlcReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BaoCaoSanXuatDataset", dataSet.Tables[0]));
            RdlcReportViewer.LocalReport.Refresh();
            //RdlcReportViewer.AsyncRendering = false;
            //RdlcReportViewer.SizeToReportContent = true;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}