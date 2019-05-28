using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Functions
{
    public static class GetMonthlyGrades
    {
        [FunctionName("GetMonthlyGrades")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            Guid userId = Guid.Parse(req.Query["userId"]);
            var result = new List<MonthlyGrade>();
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "select mg.Id as MonthlyGradeId, mg.Year, mg.Month, mg.Grade, u.Id as UserId, u.Name from MonthlyRating mg "
                    + "inner join Users u on u.Id = mg.UserId where u.Id = @userId order by mg.Month"
                };
                cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@userId"].Value = userId;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new MonthlyGrade
                        {
                            Id = reader.GetGuid(0),
                            Year = reader.GetInt32(1),
                            Month = reader.GetInt32(2),
                            Grade = reader.GetInt32(3),
                            UserId = reader.GetGuid(4),
                            User = new User
                            {
                                Id = reader.GetGuid(4),
                                Name = reader.GetString(5)
                            }
                        });
                    }
                }
                reader.Close();
                conn.Close();
            }
      
            return result;
        }
    }
}
