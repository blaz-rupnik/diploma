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
    public static class GetSummaryData
    {
        [FunctionName("GetSummaryData")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var result = new List<MonthlyGradeSummary>();
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            int startMonth = 1;
            int currentYear = DateTime.Today.Year;
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "select avg(Cast([Grade] as float)) as avggrade from MonthlyRating where [Year] = @year group by [Month]"
                };
                cmd.Parameters.Add("@year", System.Data.SqlDbType.Int);
                cmd.Parameters["@year"].Value = currentYear;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new MonthlyGradeSummary
                        {
                            Month = startMonth,
                            AverageGrade = reader.GetDouble(0)
                        });
                    }
                    startMonth += 1;
                    
                }
                reader.Close();
                conn.Close();
            }
            return result;
        }
    }
}
