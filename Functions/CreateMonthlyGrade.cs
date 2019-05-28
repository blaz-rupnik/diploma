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
    public static class CreateMonthlyGrade
    {
        [FunctionName("CreateMonthlyGrade")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MonthlyGrade data = JsonConvert.DeserializeObject<MonthlyGrade>(requestBody);
            data.Id = Guid.NewGuid();
            var str = Environment.GetEnvironmentVariable("MyConnectionString");

            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "insert into MonthlyRating values(@monthlyGradeId,@year,@month,@grade,@userId)"
                };
                cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@userId"].Value = data.UserId;
                cmd.Parameters.Add("@monthlyGradeId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@monthlyGradeId"].Value = data.Id;
                cmd.Parameters.Add("@year", System.Data.SqlDbType.Int);
                cmd.Parameters["@year"].Value = data.Year;
                cmd.Parameters.Add("@month", System.Data.SqlDbType.Int);
                cmd.Parameters["@month"].Value = data.Month;
                cmd.Parameters.Add("@grade", System.Data.SqlDbType.Int);
                cmd.Parameters["@grade"].Value = data.Grade;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }
    }
}
