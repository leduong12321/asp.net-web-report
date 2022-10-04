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
    public partial class WRReport : System.Web.UI.Page
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
            string query = @"	with data as (select 
		                        a.STAND_ID,
		                        a.MOUNT_DATE,
		                        case (datediff(hour, '2014-06-27 08:00:00', a.MOUNT_DATE) / 12) % 6
			                        when 0 then '1A'
			                        when 1 then '2B'
			                        when 2 then '1C'
			                        when 3 then '2A'
			                        when 4 then '1B'
		                        else '2C'
		                        end as mount_shift,
		                        DISMOUNT_DATE,
		                        case (datediff(hour, '2014-06-27 08:00:00', a.DISMOUNT_DATE) / 12) % 6
			                        when 0 then '1A'
			                        when 1 then '2B'
			                        when 2 then '1C'
			                        when 3 then '2A'
			                        when 4 then '1B'
		                        else '2C'
		                        end as dismount_shift,
		                        a.POSITION,
		                        a.ROLL_ID,
		                        a.MOUNT_NUM,
		                        a.DIAMETER,
		                        b.CROWN as crown,
		                        a.ROLLED_WEIGHT,
		                        a.ROLLED_LENGTH
	                        from ROLL_WR_HIST a
		                        join (select MOUNT_DATE,
					                        [STAND_ID]
					                          ,[POSITION]
					                          ,[ROLL_ID]
					                          ,[CROWN]
				                        from PM_USED_WR_DATA where LAST_UPD = ''
				                        union
				                        SELECT
					                           MOUNT_DATE
					                          ,[STAND_ID]
					                          ,[POSITION]
					                          ,[ROLL_ID]
					                          ,[CROWN]
				                          FROM [HSM_HOAPHAT].[dbo].[PM_USED_WR_DATA]
				                          where LAST_UPD >= @FromDay and LAST_UPD <= @ToDay) b on a.ROLL_ID = b.ROLL_ID and a.MOUNT_DATE = b.MOUNT_DATE
	                        where a.LAST_UPD >= @FromDay and a.LAST_UPD <= @ToDay)

                        select * from data order by DISMOUNT_DATE desc, STAND_ID, POSITION;
                        ";
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
                    RdlcWRReportViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/WR.rdlc");
                    RdlcWRReportViewer.LocalReport.DataSources.Clear();

                    RdlcWRReportViewer.LocalReport.DataSources.Add(new ReportDataSource("WRDataSet", dataSet.Tables[0]));
                    RdlcWRReportViewer.LocalReport.Refresh();

                    RdlcWRReportViewer.LocalReport.DisplayName = "BaocaothaytrucWR " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", fromValue.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", toValue.ToString());
                    fromValue = null;
                    toValue = null;
                    RdlcWRReportViewer.LocalReport.SetParameters(fromDateValue);
                    RdlcWRReportViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
        private void GenerateReport()
        {
            RdlcWRReportViewer.LocalReport.DataSources.Clear();
            RdlcWRReportViewer.LocalReport.DataSources.Add(new ReportDataSource("BaoCaoSanXuatDataset", dataSet.Tables[0]));
            RdlcWRReportViewer.LocalReport.Refresh();
            //RdlcBurRollReportViewer.AsyncRendering = false;
            //RdlcBurRollReportViewer.SizeToReportContent = true;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}