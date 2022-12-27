using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace ReportAPI
{
    public partial class Equipment : System.Web.UI.Page
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
                }
                catch
                {
                    Console.WriteLine("Error has occurred while processing the report");
                }
            }
        }

        private void SetSqlParameter()
        {
            sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("FromDay", Request.QueryString["from"]),
                new SqlParameter("ToDay", Request.QueryString["to"]),
                new SqlParameter("Type", Request.QueryString["type"])
            };
        }
        private void GetDataTable()
        {
            string query = @"	WITH
                                R_INFO AS (
                                    SELECT *
                                    FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_LIFE_ACCOUNTS]
                                    WHERE REPORT_COUNTER IN (SELECT REPORT_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REPORTS] WHERE STOP_DATE >= @FromDay AND STOP_DATE <= @ToDay)
                                ),
                                E_INFO AS (
                                    SELECT A.NAME,
                                        B.MOVEMENT_DATE,
                                        B.CODE_NAME,
                                        B.EQUIPMENT_NAME,
                                        B.VENDOR,
                                        B.NOTE,
                                        B.TSC_NO,
                                        B.WORK_TEAM,
                                        B.PARENT_NAME, C.ID
                                    FROM (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[MDB_EQUIPMENTS_CFG] WHERE ID IN (@Type)) A
                                        LEFT JOIN (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQUIPMENT_MOVEMENTS] WHERE MOVEMENT_DATE >= @FromDay AND MOVEMENT_DATE <= @ToDay) B ON A.NAME = B.CODE_NAME
                                        LEFT JOIN (SELECT ID, NAME FROM [HOAPHAT_TSC1_PRD].[dbo].[MDB_EQUIPMENTS]) C ON C.NAME = B.EQUIPMENT_NAME
                                ),
                                R_HAS_MOVEMENT_E AS (
                                    SELECT
                                        *,
                                        ROW_NUMBER() OVER (PARTITION BY R_INFO.EQUIPMENT_ID ORDER BY R_INFO.REPORT_COUNTER DESC) AS ROW
                                    FROM R_INFO
                                    WHERE
                                        EQUIPMENT_ID IN (SELECT E_INFO.ID FROM E_INFO)
                                ),
                                GET_EQ AS (
                                    SELECT *
                                    FROM R_HAS_MOVEMENT_E
                                        LEFT JOIN E_INFO ON E_INFO.ID = R_HAS_MOVEMENT_E.EQUIPMENT_ID
                                    WHERE R_HAS_MOVEMENT_E.ROW <= 2
                                ),
                                TABLEA AS (
                                    SELECT * FROM GET_EQ A WHERE LIFE_ACCOUNT_CODE = 1
                                ),
                                TABLEB AS (
                                    SELECT EQUIPMENT_ID, LIFE_FROM_NEW AS PRODUCTION, LIFE_FROM_RENEW AS T_PRODUCTION FROM GET_EQ A WHERE LIFE_ACCOUNT_CODE = 5
                                )
                                SELECT * FROM TABLEA LEFT JOIN TABLEB ON TABLEA.EQUIPMENT_ID = TABLEB.EQUIPMENT_ID
                        ";
            string connectionString = ConfigurationManager.ConnectionStrings["HSM_HOAPHATConnectionString2"].ConnectionString;
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
                    _ = sqlDataAdapter.Fill(dataSet);
                    EquipmentReportViewer.LocalReport.ReportPath = Server.MapPath("~/Views/Reports/Equipment.rdlc");
                    EquipmentReportViewer.LocalReport.DataSources.Clear();

                    EquipmentReportViewer.LocalReport.DataSources.Add(new ReportDataSource("EquipmentDataSet", dataSet.Tables[0]));
                    EquipmentReportViewer.LocalReport.Refresh();

                    EquipmentReportViewer.LocalReport.DisplayName = "Baocaotinhtrangthietbi " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ReportParameter fromDateValue = new ReportParameter("fromDate", fromValue.ToString());
                    ReportParameter toDateValue = new ReportParameter("toDate", toValue.ToString());
                    fromValue = null;
                    toValue = null;
                    EquipmentReportViewer.LocalReport.SetParameters(fromDateValue);
                    EquipmentReportViewer.LocalReport.SetParameters(toDateValue);
                }
            }
        }
        private void GenerateReport()
        {
            EquipmentReportViewer.LocalReport.DataSources.Clear();
            EquipmentReportViewer.LocalReport.DataSources.Add(new ReportDataSource("EquipmentDataset", dataSet.Tables[0]));
            EquipmentReportViewer.LocalReport.Refresh();
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}