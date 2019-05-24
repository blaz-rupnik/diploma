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
        public static void Run([TimerTrigger("0 0 5 * * *")]TimerInfo myTimer, ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            Guid statusId = new Guid("39CF86C0-0E7B-4080-A684-E7E081B8FE17");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "UPDATE VacationLeaves SET DaysPending = DaysPending + 1 WHERE DaysPending is not null and StatusId = @statusId"
                };
                //add parameter
                cmd.Parameters.Add("@statusId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@statusId"].Value = statusId;

                cmd.ExecuteNonQuery();

                //delete all that have too big days pending
                cmd.CommandText = "DELETE FROM VacationLeaves WHERE StatusId = @statusId and DaysPending > 7";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            log.LogInformation($"C# Timer trigger function for updating Days Pending executed at: {DateTime.Now}");
        }
    }
}
