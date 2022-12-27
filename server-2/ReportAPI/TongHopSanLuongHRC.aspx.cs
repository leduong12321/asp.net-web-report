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
        private dynamic fromValue = null;
        private dynamic toValue = null;
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
            string query = @"DECLARE @FromDate datetime;

                            DECLARE @ToDate datetime;


                            SET @FromDate = @FromDay;


                            SET @ToDate = @ToDay;


                            SELECT (ROW_NUMBER() OVER (
                                                       ORDER BY STEEL_GRADE)) AS Sothutu,
                                   concat(TARGET_THICK, 'x', FORMAT(CAST(TARGET_WIDTH AS DECIMAL(18, 6)), 'g15')) AS SanPham,
                                   STEEL_GRADE,
                                   COUNT(STEEL_GRADE) AS SoCuon,
                                   FORMAT(CAST(SUM(WEIGHT_PDI) AS DECIMAL(18, 6)), '#,###.###', 'en-US') AS TongKhoiLuongPDI,
                                   FORMAT(CAST(SUM(WEIGHT_MEAS) AS DECIMAL(18, 6)), '#,###.###', 'en-US') AS TongKhoiLuong,
                                   sum(WEIGHT_MEAS) * 100 /
                              (SELECT sum(WEIGHT_MEAS)
                               FROM PM_PIECE_OUTPUT
                               WHERE STATUS IN ('PRODUCED')
                                 AND (PIECE_ID not like 'GHOST%'
                                      OR PIECE_ID = 'GHOST82')
                                 AND ARCHIVE_DATE >= @FromDate
                                 AND ARCHIVE_DATE <= @ToDate) AS TiLePhanTram,

                              (SELECT FORMAT(CAST(SUM(WEIGHT_PDI) AS DECIMAL(18, 6)), '#,###.###', 'en-US')
                               FROM PM_PIECE_OUTPUT
                               WHERE STATUS IN ('PRODUCED')
                                 AND (PIECE_ID not like 'GHOST%'
                                      OR PIECE_ID = 'GHOST82')
                                 AND ARCHIVE_DATE >= @FromDate
                                 AND ARCHIVE_DATE <= @ToDate) AS TongKhoiLuongSanXuatPDI,

                              (SELECT FORMAT(CAST(SUM(WEIGHT_MEAS) AS DECIMAL(18, 6)), '#,###.###', 'en-US')
                               FROM PM_PIECE_OUTPUT
                               WHERE STATUS IN ('PRODUCED')
                                 AND (PIECE_ID not like 'GHOST%'
                                      OR PIECE_ID = 'GHOST82')
                                 AND ARCHIVE_DATE >= @FromDate
                                 AND ARCHIVE_DATE <= @ToDate) AS TongKhoiLuongSanXuat

                            FROM [dbo].[PM_PIECE_OUTPUT]
                            WHERE STATUS IN ('PRODUCED')
                              AND (PIECE_ID not like 'GHOST%'
                                   OR PIECE_ID = 'GHOST82')
                              AND ARCHIVE_DATE >= @FromDate
                              AND ARCHIVE_DATE <= @ToDate
                            GROUP BY TARGET_THICK,
                                     TARGET_WIDTH,
                                     STEEL_GRADE
                            ORDER BY STEEL_GRADE, SanPham desc";
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
                        time[0].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(from + 25200000);
                        time[1].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(to + 25200000);
                        fromValue = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(from);
                        toValue = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(to);
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

                    TongHopSanLuongHRCViewer.LocalReport.DisplayName = "Tonghopsanluong " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", fromValue.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", toValue.ToString());
                    TongHopSanLuongHRCViewer.LocalReport.SetParameters(fromDateValue);
                    TongHopSanLuongHRCViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
    }
}