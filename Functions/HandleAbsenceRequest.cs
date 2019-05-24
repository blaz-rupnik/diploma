using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Functions
{
    public static class HandleAbsenceRequest
    {
        [FunctionName("HandleAbsenceRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int isAccepted = int.Parse(req.Query["isAccepted"]);
            Guid absenceId = Guid.Parse(req.Query["absenceId"]);
            Guid statusAccepted = Guid.Parse("AF4D3860-2CAA-4188-BBFC-94BB8F62DD4F");
            Guid statusRejected = Guid.Parse("315BEF5A-FCCD-4C73-83DB-AD2EA755FD3B");
            var str = Environment.GetEnvironmentVariable("MyConnectionString");

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Update VacationLeaves SET StatusId = @statusId "
                        + " WHERE Id = @absenceId";

                cmd.Parameters.Add("@statusId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters.Add("@absenceId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@absenceId"].Value = absenceId;
                cmd.Parameters["@statusId"].Value = isAccepted == 1 ? statusAccepted : statusRejected;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            log.LogInformation("Function processed a request HandleAbsenceRequest.");
            return (ActionResult)new OkObjectResult(isAccepted);
        }
    }
}
