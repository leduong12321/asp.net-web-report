using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsApp.Controllers
{

    public class SanXuatTheoSequenceController : ApiController
    {
        private List<SqlParameter> sqlParameters;
        [HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    //string query = @" WITH 
        //    //             E_INFO AS (
        //    //              SELECT 
        //    //              A.[SEQ_COUNTER],
        //    //                   B.[SEQ_HEAT_COUNTER],
        //    //                   B.[SEQ_TOTAL_HEATS],
        //    //                   B.[LADLE_OPENING_DATE],
        //    //                   B.[LADLE_OPENING_WGT],
        //    //                   B.[LADLE_CLOSE_WGT],
        //    //                   B.[TUNDISH_AT_LADLE_OPEN_WGT],
        //    //                   B.[TUNDISH_AT_LADLE_CLOSE_WGT],
        //    //                   B.[LADLE_OPEN_GROSSWGT],
        //    //                   B.[LADLE_CLOSE_GROSSWGT]
        //    //              FROM (SELECT DISTINCT SEQ_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] WHERE LADLE_OPENING_DATE >= '2022-11-26 00:00:00' AND LADLE_OPENING_DATE <= '2022-11-30 00:00:00') A
        //    //              LEFT JOIN (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] ) B ON A.SEQ_COUNTER = B.SEQ_COUNTER 
        //    //             )
        //    //             SELECT * FROM E_INFO
        //    //                ";

        //    string query = @"
        //                    SELECT A.SEQ_COUNTER,
        //                            B.REPORT_COUNTER,
		      //                      B.SEQ_HEAT_COUNTER,
		      //                      B.LADLE_OPENING_DATE,
		      //                      B.LADLE_OPENING_WGT,
		      //                      B.LADLE_CLOSE_WGT,
		      //                      B.TUNDISH_SKULL_WGT
        //                    FROM (
	       //                     SELECT DISTINCT SEQ_COUNTER
	       //                       FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] WHERE REPORT_COUNTER IN (
	       //                       SELECT REPORT_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REPORTS] WHERE PRODUCTION_DATE >= '2022-11-16 00:00:00.000' AND PRODUCTION_DATE <= '2022-11-16 00:00:00.000') 
        //                    ) A 
        //                    LEFT JOIN (SELECT * FROM REP_CCM) B ON B.SEQ_COUNTER = A.SEQ_COUNTER ORDER BY LADLE_OPENING_DATE

        //                    ";

        //    DataTable table = new DataTable();
        //    string connectionString = ConfigurationManager.ConnectionStrings["TSC_HOAPHAT_PRD1"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand sqlCommand = new SqlCommand(query, con))
        //        {
        //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

        //            sqlCommand.CommandType = CommandType.Text;
        //            _ = sqlDataAdapter.Fill(table);
        //        }
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, table);
        //}


        public HttpResponseMessage Get([FromUri] dynamic from, [FromUri] dynamic to)
        {
            sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("FromDay", from));
            sqlParameters.Add(new SqlParameter("ToDay", to));

            string query = @"SELECT A.SEQ_COUNTER,
                                    B.REPORT_COUNTER,
		                            B.SEQ_HEAT_COUNTER,
		                            B.LADLE_OPENING_DATE,
		                            B.LADLE_OPENING_WGT,
		                            B.LADLE_CLOSE_WGT,
		                            B.TUNDISH_SKULL_WGT
                            FROM (
	                            SELECT DISTINCT SEQ_COUNTER
	                              FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] WHERE REPORT_COUNTER IN (
	                              SELECT REPORT_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REPORTS] WHERE PRODUCTION_DATE >= @FromDay AND PRODUCTION_DATE <= @ToDay) 
                            ) A 
                            LEFT JOIN (SELECT * FROM REP_CCM) B ON B.SEQ_COUNTER = A.SEQ_COUNTER ORDER BY LADLE_OPENING_DATE
                            ";

            DataTable table = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["TSC_HOAPHAT_PRD1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    dynamic time = sqlParameters.ToArray();

                    long fromValue = long.Parse(time[0].Value);
                    long toValue = long.Parse(time[1].Value);
                    time[0].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(fromValue + 25200000);
                    time[1].Value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(toValue + 25200000);
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlDataAdapter.Fill(table);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

    }
}
