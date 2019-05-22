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

            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using(SqlConnection conn = new SqlConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if(isAccepted == 1)
                {
                    cmd.CommandText = "Update VacationLeaves SET StatusId = " + "AF4D3860-2CAA-4188-BBFC-94BB8F62DD4F"
                        + "WHERE Id = " + absenceId.ToString();
                }
                else
                {
                    //TODO
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return (ActionResult)new OkObjectResult(isAccepted);
        }
    }
}
