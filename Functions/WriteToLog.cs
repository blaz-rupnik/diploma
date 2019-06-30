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
    public static class WriteToLog
    {
        [FunctionName("WriteToLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CustomLog customLog = JsonConvert.DeserializeObject<CustomLog>(requestBody);

            using(SqlConnection conn = new SqlConnection(Environment.GetEnvironmentVariable("MyConnectionString")))
            {
                conn.Open();

                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "insert into ErrorLogs values (@message, @timestamp)"
                };

                sqlCommand.Parameters.Add("@message", System.Data.SqlDbType.NVarChar);
                sqlCommand.Parameters.Add("@timestamp", System.Data.SqlDbType.DateTime);

                sqlCommand.Parameters["@message"].Value = customLog.Message;
                sqlCommand.Parameters["@timestamp"].Value = customLog.TimeStamp;

                sqlCommand.ExecuteNonQuery();
                conn.Close();
            }
            return (ActionResult)new OkObjectResult("");
        }
    }
}
