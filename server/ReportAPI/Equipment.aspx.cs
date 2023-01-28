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
        private dynamic textType = null;
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

            //string query v2 = @"
            //                 WITH 
            //                 E_INFO AS (
            //                  SELECT A.NAME,
            //                  B.MOVEMENT_DATE,
            //                  B.CODE_NAME,
            //                  B.EQUIPMENT_NAME,
            //                  B.VENDOR,
            //                  B.NOTE,
            //                  B.TSC_NO,
            //                  B.WORK_TEAM,
            //                  B.PARENT_NAME,
            //                  B.MOVEMENT_SEQ,
            //                  C.LIFE_ACCOUNT_CODE,
            //                  C.TOTAL_LIFE_FROM_NEW,
            //                  C.TOTAL_LIFE_FROM_RENEW,
            //                  C.TOTAL_WEIGHT_NEW,
            //                  C.TOTAL_WEIGHT_RENEW
            //                  FROM (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[MDB_EQUIPMENTS_CFG] WHERE ID = @Type) A
            //                  LEFT JOIN (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQUIPMENT_MOVEMENTS] WHERE MOVEMENT_DATE >= @FromDay AND MOVEMENT_DATE <= @ToDay) B ON A.NAME = B.CODE_NAME 
            //                  LEFT JOIN (

            //                  SELECT * FROM ( SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 1 ) F
            //                  LEFT JOIN (SELECT MOVEMENT_SEQ AS M1, TOTAL_LIFE_FROM_NEW AS TOTAL_WEIGHT_NEW, TOTAL_LIFE_FROM_RENEW AS TOTAL_WEIGHT_RENEW FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 5) G ON G.M1 = F.MOVEMENT_SEQ

            //                  ) C ON C.MOVEMENT_SEQ = B.MOVEMENT_SEQ 
            //                 )
            //                 SELECT * FROM E_INFO ORDER BY MOVEMENT_DATE DESC
            //                ";

            string query = @"
                            WITH 
	                        E_TSC1 AS (
		                        SELECT A.NAME,
		                        B.MOVEMENT_DATE,
		                        B.CODE_NAME,
		                        B.EQUIPMENT_NAME,
		                        B.VENDOR,
		                        B.NOTE,
		                        B.TSC_NO,
		                        B.WORK_TEAM,
		                        B.PARENT_NAME,
		                        B.MOVEMENT_SEQ,
		                        C.EQUIPMENT_ID,
		                        C.LAST_MOUNT_DATE,
		                        D.CHAR_VALUE,
		                        E.LIFE_ACCOUNT_CODE,
		                        E.TOTAL_LIFE_FROM_NEW,
		                        E.TOTAL_LIFE_FROM_RENEW,
                                E.TOTAL_WEIGHT_NEW,
		                        E.TOTAL_WEIGHT_RENEW,
		                        H.RENEW_CYCLE_COUNT
		                        FROM (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[MDB_EQUIPMENTS_CFG] WHERE ID = @Type) A
		                        LEFT JOIN (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQUIPMENT_MOVEMENTS] WHERE MOVEMENT_DATE >= @FromDay AND MOVEMENT_DATE <= @ToDay) B ON A.NAME = B.CODE_NAME 
		                        LEFT JOIN (SELECT ID AS EQUIPMENT_ID , STATUS_CODE, LAST_MOUNT_DATE, LAST_DISMOUNT_DATE, NAME FROM [HOAPHAT_TSC1_PRD].[dbo].[MDB_EQUIPMENTS]) C ON C.NAME = B.EQUIPMENT_NAME
		                        LEFT JOIN (SELECT VARIABLE_ID, INTEGER_VALUE, CHAR_VALUE FROM [HOAPHAT_TSC1_PRD].[dbo].[AUX_VALUES] WHERE VARIABLE_ID ='EQP_STATUS_CODES' ) D ON D.INTEGER_VALUE = C.STATUS_CODE 
		                        LEFT JOIN (SELECT EQUIPMENT_ID, RENEW_CYCLE_COUNT FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_LIFE_ACCOUNTS] ) H ON H.EQUIPMENT_ID = C.EQUIPMENT_ID
		                        LEFT JOIN (
			                        SELECT * FROM ( SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 1 ) F
			                        LEFT JOIN (SELECT MOVEMENT_SEQ AS M1, TOTAL_LIFE_FROM_NEW AS TOTAL_WEIGHT_NEW, TOTAL_LIFE_FROM_RENEW AS TOTAL_WEIGHT_RENEW FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 5) G ON G.M1 = F.MOVEMENT_SEQ
		                        ) E ON E.MOVEMENT_SEQ = B.MOVEMENT_SEQ
	                        ),
	                        E_TSC2 AS (
		                        SELECT A.NAME,
		                        B.MOVEMENT_DATE,
		                        B.CODE_NAME,
		                        B.EQUIPMENT_NAME,
		                        B.VENDOR,
		                        B.NOTE,
		                        B.TSC_NO,
		                        B.WORK_TEAM,
		                        B.PARENT_NAME,
		                        B.MOVEMENT_SEQ,
		                        C.EQUIPMENT_ID,
		                        C.LAST_MOUNT_DATE,
		                        D.CHAR_VALUE,
		                        E.LIFE_ACCOUNT_CODE,
		                        E.TOTAL_LIFE_FROM_NEW,
		                        E.TOTAL_LIFE_FROM_RENEW,
                                E.TOTAL_WEIGHT_NEW,
		                        E.TOTAL_WEIGHT_RENEW,
		                        H.RENEW_CYCLE_COUNT
		                        FROM (SELECT * FROM [HOAPHAT_TSC2_PRD].[dbo].[MDB_EQUIPMENTS_CFG] WHERE ID = @Type) A
		                        LEFT JOIN (SELECT * FROM [HOAPHAT_TSC2_PRD].[dbo].[REP_EQUIPMENT_MOVEMENTS] WHERE MOVEMENT_DATE >= @FromDay AND MOVEMENT_DATE <= @ToDay) B ON A.NAME = B.CODE_NAME 
		                        LEFT JOIN (SELECT ID AS EQUIPMENT_ID, STATUS_CODE, LAST_MOUNT_DATE, NAME FROM [HOAPHAT_TSC2_PRD].[dbo].[MDB_EQUIPMENTS]) C ON C.NAME = B.EQUIPMENT_NAME
		                        LEFT JOIN (SELECT VARIABLE_ID, INTEGER_VALUE, CHAR_VALUE FROM [HOAPHAT_TSC2_PRD].[dbo].[AUX_VALUES] WHERE VARIABLE_ID ='EQP_STATUS_CODES' ) D ON D.INTEGER_VALUE = C.STATUS_CODE 
		                        LEFT JOIN (SELECT EQUIPMENT_ID, RENEW_CYCLE_COUNT FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_EQP_LIFE_ACCOUNTS] ) H ON H.EQUIPMENT_ID = C.EQUIPMENT_ID
		                        LEFT JOIN (
			                        SELECT * FROM ( SELECT * FROM [HOAPHAT_TSC2_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 1 ) F
			                        LEFT JOIN (SELECT MOVEMENT_SEQ AS M1, TOTAL_LIFE_FROM_NEW AS TOTAL_WEIGHT_NEW, TOTAL_LIFE_FROM_RENEW AS TOTAL_WEIGHT_RENEW FROM [HOAPHAT_TSC2_PRD].[dbo].[REP_EQP_MOV_LIFE_ACCOUNTS] WHERE LIFE_ACCOUNT_CODE = 5) G ON G.M1 = F.MOVEMENT_SEQ
		                        ) E ON E.MOVEMENT_SEQ = B.MOVEMENT_SEQ
                        )
	                        SELECT * FROM E_TSC1 WHERE EQUIPMENT_NAME IS NOT NULL
	                        UNION
	                        SELECT * FROM E_TSC2 WHERE EQUIPMENT_NAME IS NOT NULL
                            ";

            string connectionString = ConfigurationManager.ConnectionStrings["TSC_PRODUCT"].ConnectionString;
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
                    
                    switch (time[2].Value)
                    {
                        case "4":
                            textType = "BROADPLATES";
                            break;
                        case "5":
                            textType = "NARROWPLATES";
                            break;
                        case "7":
                            textType = "TUNDISHES";
                            break;
                        case "8":
                            textType = "SEN";
                            break;
                        case "9":
                            textType = "SHEARKNIFE";
                            break;
                        case "10":
                            textType = "SEG0";
                            break;
                        case "11":
                            textType = "SEGA";
                            break;
                        case "12":
                            textType = "SEGB";
                            break;
                        case "13":
                            textType = "SEGC";
                            break;
                        case "14":
                            textType = "SEGD";
                            break;
                        case "15":
                            textType = "SEGE";
                            break;
                        case "16":
                            textType = "VUHZ";
                            break;
                        default:
                            textType = "MOULDS";
                            // code block
                            break;
                    }
                    ReportParameter textTypeView = new ReportParameter("textType", textType);

                    fromValue = null;
                    toValue = null;
                    EquipmentReportViewer.LocalReport.SetParameters(fromDateValue);
                    EquipmentReportViewer.LocalReport.SetParameters(toDateValue);
                    EquipmentReportViewer.LocalReport.SetParameters(textTypeView);
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