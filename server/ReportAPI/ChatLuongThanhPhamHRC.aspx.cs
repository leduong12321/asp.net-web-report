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
    public partial class ChatLuongThanhPhamHRC : System.Web.UI.Page
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
            string query = @"SELECT ARCHIVE_DATE,
                                    case (datediff(hour, '2014-07-17 08:00:00', A.ARCHIVE_DATE) / 12) % 6
			                                                    when 0 then '1A'
			                                                    when 1 then '2B'
			                                                    when 2 then '1C'
			                                                    when 3 then '2A'
			                                                    when 4 then '1B'
		                                                    else '2C'
		                                                    end as CA,
                                    concat(A.TARGET_THICK, 'x', FORMAT(CAST(TARGET_WIDTH AS DECIMAL(18, 6)), 'g15')) AS SanPham,
                                    (ROW_NUMBER() OVER(
                                                        ORDER BY A.LAST_UPD)) AS Sothutu,
                                    A.TARGET_THICK,
                                    TASK_SLAB,
                                    SLAB_ID,
                                    STEEL_GRADE,
                                    B.DSC2_RAMP1 AS DSC2_RAMP1,

                                (SELECT DSC1_RAMP1
                                FROM SC_MEASURE_MILL
                                WHERE PIECE_ID = A.PIECE_ID
                                    AND STAND_ID = 'H1') AS DSC1_RAMP1,
                                    (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'H1') AS [ROLL_WEIGHT_H1],
		                            (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'H2') AS [ROLL_WEIGHT_H2],
                                    (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'F1') AS [ROLL_WEIGHT_F1],
                                    (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'F2') AS [ROLL_WEIGHT_F2],
                                    (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'F3') AS [ROLL_WEIGHT_F3],
                                    (Select ROLLED_WEIGHT FROM PM_USED_WR_DATA WHERE PIECE_ID = A.SLAB_ID AND POSITION = 'TOP' AND STAND_ID = 'F4') AS [ROLL_WEIGHT_F4]
                            FROM [dbo].[PM_PIECE_OUTPUT] A
                            LEFT OUTER JOIN (SELECT * FROM [dbo].[SC_MEASURE_MILL] WHERE STAND_ID = 'F4') B ON A.PIECE_ID = B.PIECE_ID
                            WHERE A.archive_date >= @FromDay
                                AND A.archive_date <= @ToDay
                                AND (A.PIECE_ID not like 'GHOST%' OR B.PIECE_ID = 'GHOST82')
	                            AND A.STATUS = 'PRODUCED'
                            ORDER BY ARCHIVE_DATE DESC";
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
                    ChatLuongThanhPhamViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/ChatLuongThanhPhamHRC.rdlc");
                    ChatLuongThanhPhamViewer.LocalReport.DataSources.Clear();
                    ChatLuongThanhPhamViewer.LocalReport.DataSources.Add(new ReportDataSource("ChatLuongThanhPhamHRCDataSet", dataSet.Tables[0]));
                    ChatLuongThanhPhamViewer.LocalReport.Refresh();

                    ChatLuongThanhPhamViewer.LocalReport.DisplayName = "Chatluongsanpham " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", fromValue.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", toValue.ToString());
                    fromValue = null;
                    toValue = null;
                    ChatLuongThanhPhamViewer.LocalReport.SetParameters(fromDateValue);
                    ChatLuongThanhPhamViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
    }
}