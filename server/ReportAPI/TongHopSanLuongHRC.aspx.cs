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

namespace ReportAPI
{
    public partial class TongHopSanLuongHRC : System.Web.UI.Page
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
            string query = "SELECT (ROW_NUMBER() OVER (ORDER BY STEEL_GRADE)) AS Sothutu, concat(TARGET_THICK,'x', FORMAT(CAST(TARGET_WIDTH AS DECIMAL(18,6)), 'g15')) as SanPham , STEEL_GRADE,COUNT(STEEL_GRADE) as SoCuon,FORMAT(CAST(SUM(WEIGHT_PDI) as DECIMAL(18,6)) , '#,###.###', 'en-US')  as TongKhoiLuongPDI,FORMAT(CAST(SUM(WEIGHT_MEAS) as DECIMAL(18,6)) , '#,###.###', 'en-US') as TongKhoiLuong from [dbo].[PM_PIECE_OUTPUT] where STATUS IN ('PRODUCED')  and (PIECE_ID not like 'GHOST%' or PIECE_ID = 'GHOST82') and ARCHIVE_DATE >=@FromDay and ARCHIVE_DATE <=@ToDay group by TARGET_THICK,TARGET_WIDTH,STEEL_GRADE";
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
                    }
                    else
                    {
                        time[0].Value = 1659312000000;
                        time[1].Value = 1662163200000;
                    }
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    TongHopSanLuongHRCViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/TongHopSanLuongHRC.rdlc");
                    TongHopSanLuongHRCViewer.LocalReport.DataSources.Clear();
                    TongHopSanLuongHRCViewer.LocalReport.DataSources.Add(new ReportDataSource("TongHopSanLuongDataSet", dataSet.Tables[0]));
                    TongHopSanLuongHRCViewer.LocalReport.Refresh();

                    ReportParameter fromDateValue = new ReportParameter("fromDate", time[0].Value.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", time[1].Value.ToString());
                    TongHopSanLuongHRCViewer.LocalReport.SetParameters(fromDateValue);
                    TongHopSanLuongHRCViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
    }
}