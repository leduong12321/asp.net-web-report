using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ReportAPI.Controllers
{
    public class SanLuongTheoNgayController : ApiController
    {
        private List<SqlParameter> sqlParameters;
        [HttpGet]
        public HttpResponseMessage Get([FromUri] dynamic from, [FromUri] dynamic to)
        {
            sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("FromDay", from));
            sqlParameters.Add(new SqlParameter("ToDay", to));

            string query = @"SELECT A.REPORT_COUNTER,
		                            A.PRODUCTION_DATE,
                                    B.SEQ_COUNTER,
                                    B.SEQ_HEAT_COUNTER,
                                    B.LADLE_OPENING_DATE,
                                    B.LADLE_OPENING_WGT,
                                    B.LADLE_CLOSE_WGT,
                                    B.TUNDISH_SKULL_WGT
                            FROM (
                                SELECT REPORT_COUNTER, PRODUCTION_DATE FROM [Q3IntelligenceDWH].[dbo].[REPORTS] WHERE PRODUCTION_DATE >= @FromDay AND PRODUCTION_DATE < @ToDay
                            ) A 
                            LEFT JOIN (SELECT * FROM REP_CCM) B ON B.REPORT_COUNTER = A.REPORT_COUNTER ORDER BY REPORT_COUNTER ASC
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
