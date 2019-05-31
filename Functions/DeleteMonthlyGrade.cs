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
    public static class DeleteMonthlyGrade
    {
        [FunctionName("DeleteMonthlyGrade")]
        public static async Task<Guid> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            Guid gradeId = Guid.Parse(req.Query["gradeId"]);
            var str = Environment.GetEnvironmentVariable("MyConnectionString");

            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "delete from MonthlyRating where Id = @gradeId"
                };
                cmd.Parameters.Add("@gradeId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@gradeId"].Value = gradeId;

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
            return gradeId;
        }
    }
}
