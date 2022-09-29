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
    public partial class BurRollReport : System.Web.UI.Page
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
            string query = @"Declare @FromDate datetime;
                            DECLARE @ToDate datetime;
                            SET @FromDate = @FromDay;
                            SET @ToDate = @ToDay;
                            WITH DATA AS
                                (SELECT a.STAND_ID,
                                        a.MOUNT_DATE,
                                        CASE (datediff(HOUR, '2014-06-27 08:00:00', a.MOUNT_DATE) / 12) % 6
                                            WHEN 0 THEN '1A'
                                            WHEN 1 THEN '2B'
                                            WHEN 2 THEN '1C'
                                            WHEN 3 THEN '2A'
                                            WHEN 4 THEN '1B'
                                            ELSE '2C'
                                        END AS mount_shift,
                                        DISMOUNT_DATE,
                                        CASE (datediff(HOUR, '2014-06-27 08:00:00', a.DISMOUNT_DATE) / 12) % 6
                                            WHEN 0 THEN '1A'
                                            WHEN 1 THEN '2B'
                                            WHEN 2 THEN '1C'
                                            WHEN 3 THEN '2A'
                                            WHEN 4 THEN '1B'
                                            ELSE '2C'
                                        END AS dismount_shift,
                                        a.POSITION,
                                        a.ROLL_ID,
                                        a.MOUNT_NUM,
                                        a.DIAMETER,
                                        b.CROWN AS crown,
                                        a.ROLLED_WEIGHT,
                                        a.ROLLED_LENGTH
                                FROM ROLL_BUR_HIST a
                                JOIN
                                    (SELECT MOUNT_DATE,
                                            [STAND_ID] ,
                                            [POSITION] ,
                                            [ROLL_ID] ,
                                            [CROWN]
                                    FROM PM_USED_BUR_DATA
                                    WHERE LAST_UPD = ''
                                    UNION SELECT MOUNT_DATE ,
                                                [STAND_ID] ,
                                                [POSITION] ,
                                                [ROLL_ID] ,
                                                [CROWN]
                                    FROM [HSM_HOAPHAT].[dbo].[PM_USED_BUR_DATA]
                                    WHERE LAST_UPD >= @FromDate
                                    AND LAST_UPD <= @ToDate) b ON a.ROLL_ID = b.ROLL_ID
                                AND a.MOUNT_DATE = b.MOUNT_DATE
                                WHERE a.LAST_UPD >= @FromDate
                                    AND a.LAST_UPD <= @ToDate)
                            SELECT *
                            FROM DATA
                            ORDER BY DISMOUNT_DATE DESC,
                                        STAND_ID,
                                        POSITION";
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
                    RdlcBurRollReportViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/BurRoll.rdlc");
                    RdlcBurRollReportViewer.LocalReport.DataSources.Clear();

                    RdlcBurRollReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BurRollDataSet", dataSet.Tables[0]));
                    RdlcBurRollReportViewer.LocalReport.Refresh();

                    RdlcBurRollReportViewer.LocalReport.DisplayName = "Baocaothaytruc " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", time[0].Value.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", time[1].Value.ToString());
                    RdlcBurRollReportViewer.LocalReport.SetParameters(fromDateValue);
                    RdlcBurRollReportViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
        private void GenerateReport()
        {
            RdlcBurRollReportViewer.LocalReport.DataSources.Clear();
            RdlcBurRollReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BaoCaoSanXuatDataset", dataSet.Tables[0]));
            RdlcBurRollReportViewer.LocalReport.Refresh();
            //RdlcBurRollReportViewer.AsyncRendering = false;
            //RdlcBurRollReportViewer.SizeToReportContent = true;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}