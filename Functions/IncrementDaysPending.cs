using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public static class IncrementDaysPending
    {
        [FunctionName("IncrementDaysPending")]
        public static void Run([TimerTrigger("*/15 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE VacationLeaves SET DaysPending = DaysPending + 1 WHERE DaysPending is not null";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            log.LogInformation($"C# Timer trigger function for updatin Days Pending executed at: {DateTime.Now}");
        }
    }
}
