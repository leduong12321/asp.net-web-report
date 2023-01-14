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
        [HttpGet]
        public HttpResponseMessage Get()
        {
            //string query = @" WITH 
            //             E_INFO AS (
            //              SELECT 
            //              A.[SEQ_COUNTER],
            //                   B.[SEQ_HEAT_COUNTER],
            //                   B.[SEQ_TOTAL_HEATS],
            //                   B.[LADLE_OPENING_DATE],
            //                   B.[LADLE_OPENING_WGT],
            //                   B.[LADLE_CLOSE_WGT],
            //                   B.[TUNDISH_AT_LADLE_OPEN_WGT],
            //                   B.[TUNDISH_AT_LADLE_CLOSE_WGT],
            //                   B.[LADLE_OPEN_GROSSWGT],
            //                   B.[LADLE_CLOSE_GROSSWGT]
            //              FROM (SELECT DISTINCT SEQ_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] WHERE LADLE_OPENING_DATE >= '2022-11-26 00:00:00' AND LADLE_OPENING_DATE <= '2022-11-30 00:00:00') A
            //              LEFT JOIN (SELECT * FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] ) B ON A.SEQ_COUNTER = B.SEQ_COUNTER 
            //             )
            //             SELECT * FROM E_INFO
            //                ";

            string query = @"
                            SELECT A.SEQ_COUNTER,
                                    B.REPORT_COUNTER,
		                            B.SEQ_HEAT_COUNTER,
		                            B.LADLE_OPENING_DATE,
		                            B.LADLE_OPENING_WGT,
		                            B.LADLE_CLOSE_WGT,
		                            B.TUNDISH_SKULL_WGT
                            FROM (
	                            SELECT DISTINCT SEQ_COUNTER
	                              FROM [HOAPHAT_TSC1_PRD].[dbo].[REP_CCM] WHERE REPORT_COUNTER IN (
	                              SELECT REPORT_COUNTER FROM [HOAPHAT_TSC1_PRD].[dbo].[REPORTS] WHERE PRODUCTION_DATE >= '2022-11-16 00:00:00.000' AND PRODUCTION_DATE <= '2022-11-16 00:00:00.000') 
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
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    sqlCommand.CommandType = CommandType.Text;
                    _ = sqlDataAdapter.Fill(table);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

    }
}
