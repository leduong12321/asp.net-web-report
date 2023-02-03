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
	                              FROM [Q3IntelligenceDWH].[dbo].[REP_CCM] WHERE REPORT_COUNTER IN (
	                              SELECT REPORT_COUNTER FROM [Q3IntelligenceDWH].[dbo].[REPORTS] WHERE PRODUCTION_DATE >= @FromDay AND PRODUCTION_DATE <= @ToDay) 
                            ) A 
                            LEFT JOIN (SELECT * FROM REP_CCM) B ON B.SEQ_COUNTER = A.SEQ_COUNTER ORDER BY LADLE_OPENING_DATE
                            ";

            DataTable table = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["TSC_HIS"].ConnectionString;
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
