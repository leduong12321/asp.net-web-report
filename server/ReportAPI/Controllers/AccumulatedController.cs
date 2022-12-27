using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsApp.Controllers
{

    public class AccumulatedController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            string query = @"
                            WITH 
	                            DATA AS
	                              (SELECT [ARCHIVED_DATE],
			                              RIGHT([SHIFT], 1) AS CREW,
			                              SUM([WEIGHT])/1000 AS PRODUCT
	                               FROM [PRODUCTION_ACCUMULATION].[DBO].[SOURCE]
	                               GROUP BY [ARCHIVED_DATE],
				                            RIGHT([SHIFT], 1)), 
	                            DAY_DATA AS
	                              (SELECT [ARCHIVED_DATE],
			                              SUM(PRODUCT) AS DAY_TOTAL
	                               FROM DATA
	                               GROUP BY [ARCHIVED_DATE]), 
	                            MONTH_DATA AS
	                              (SELECT ARCHIVED_DATE,
			                              SUM(DAY_TOTAL) OVER(PARTITION BY YEAR(ARCHIVED_DATE), MONTH(ARCHIVED_DATE)
								                              ORDER BY ARCHIVED_DATE) AS MONTH_TOTAL
	                               FROM DAY_DATA),
	                            CREW_DATA AS
	                              (SELECT [ARCHIVED_DATE],
                                          CONVERT(varchar,[ARCHIVED_DATE],103) AS ARCHIVED_DATE_FORMAT,
			                              [A],
			                              [B],
			                              [C]
	                               FROM DATA PIVOT (SUM([PRODUCT])
						                            FOR CREW IN ([A], [B], [C])) AS PV)
                            SELECT A.*,
                                   B.DAY_TOTAL,
                                   C.MONTH_TOTAL
                            FROM CREW_DATA A
                            JOIN DAY_DATA B ON A.ARCHIVED_DATE = B.ARCHIVED_DATE
                            JOIN MONTH_DATA C ON A.ARCHIVED_DATE = C.ARCHIVED_DATE
                            ";

            DataTable table = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["AccumulatedDataConnectionString"].ConnectionString;
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