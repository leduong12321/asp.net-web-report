using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReportAPI.Controllers
{
	public class DepartmentController : ApiController
	{
		[HttpGet]
		public HttpResponseMessage Get()
		{
			string query = @"
                            SELECT F.ROLLING_STOP,
									A.PIECE_ID,
									A.TARGET_THICK,

								(SELECT DSC1_RAMP1
								FROM SC_MEASURE_MILL
								WHERE PIECE_ID = A.PIECE_ID
									AND STAND_ID = 'H1') AS DSC1_RAMP1,

								(SELECT AVG_VALUE
								FROM [dbo].[SC_MEASURE_DEVICE]
								WHERE PIECE_ID = A.PIECE_ID
									AND DEVICE_ID = 'THICK') AS MEAS_THIK,

								(SELECT AVG_VALUE
								FROM [dbo].[SC_MEASURE_DEVICE]
								WHERE PIECE_ID = A.PIECE_ID
									AND DEVICE_ID = 'CROWN') AS AVG_CROWN,

								(SELECT AVG_VALUE
								FROM [dbo].[SC_MEASURE_DEVICE]
								WHERE PIECE_ID = A.PIECE_ID
									AND DEVICE_ID = 'WEDGE') AS AVG_WEDGE,
								CAST(ROUND(E.AVG_VALUE, 2) AS FLOAT) AS FLATNEES,
								CAST(ROUND(E.AVG_HEAD, 2) AS FLOAT) AS SymHeadFlatness,
								CAST(ROUND(E.AVG_BODY, 2) AS FLOAT) AS SymBodyFlatness,
								CAST(ROUND(E.AVG_TAIL, 2) AS FLOAT) AS SymTailFlatness,
								CAST(ROUND(E.AVG_HEAD_ASIM, 2) AS FLOAT) AS ASymHeadFlatness,
								CAST(ROUND(E.AVG_BODY_ASIM, 2) AS FLOAT) AS ASymBodyFlatness,
								CAST(ROUND(E.AVG_TAIL_ASIM, 2) AS FLOAT) AS ASymTailFlatness
							FROM [dbo].[PM_PIECE_OUTPUT] A
							LEFT OUTER JOIN (SELECT * FROM [dbo].[SC_MEASURE_MILL] WHERE STAND_ID = 'F5') B ON A.PIECE_ID = B.PIECE_ID
							LEFT OUTER JOIN (SELECT * FROM [dbo].[SC_MEASURE_DEVICE] WHERE DEVICE_ID = 'WIDTH1') C ON A.PIECE_ID = C.PIECE_ID
							JOIN [dbo].[SC_PROFILE_MEASURE] D ON A.PIECE_ID = D.PIECE_ID
							JOIN [dbo].[SC_FLAT_MEASURE] E ON A.PIECE_ID = E.PIECE_ID
							JOIN View_PM_PIECE F ON A.PIECE_ID = F.PIECE_ID
							WHERE A.archive_date >= '2022-09-12 08:00:00'
								AND A.archive_date <= '2022-09-13 08:00:00'
								AND (A.PIECE_ID not like 'GHOST%'
									OR A.PIECE_ID = 'GHOST82')
							ORDER BY F.ROLLING_STOP
                            ";

			DataTable table = new DataTable();
			string connectionString = ConfigurationManager.ConnectionStrings["HSM_HOAPHATConnectionString"].ConnectionString;
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
