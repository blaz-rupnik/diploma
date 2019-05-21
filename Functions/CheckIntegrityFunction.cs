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
using System.Collections.Generic;

namespace Functions
{
    public static class CheckIntegrityFunction
    {
        [FunctionName("CheckIntegrity")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            var result = new List<string>();
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlDataReader reader;
                cmd.CommandText = "SELECT * FROM VacationLeaves WHERE DateFrom > DateTo;";

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    result.Add("1");
                }

                conn.Close();
            }
            log.LogInformation("C# HTTP trigger function processed a request.");

            return (ActionResult)new OkObjectResult(result);
               
        }
    }
}
