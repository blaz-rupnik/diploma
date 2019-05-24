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
    public static class GetTasks
    {
        [FunctionName("GetTasks")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get tasks function called");

            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            var result = new List<TaskNotification>();

            Guid statusPendingAbsence = new Guid("39CF86C0-0E7B-4080-A684-E7E081B8FE17"); 
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "select VacationLeaves.Id as absenceId, VacationLeaves.DateFrom as dateFrom, VacationLeaves.DateTo as dateTo, Users.Name as Name from VacationLeaves"
                    + " inner join Users on VacationLeaves.UserId = Users.Id where VacationLeaves.StatusId = @statusId"
                };

                cmd.Parameters.Add("@statusId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@statusId"].Value = statusPendingAbsence;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Guid absenceId = reader.GetGuid(0);
                        string taskName = reader.GetString(3) + ": " + reader.GetDateTime(1).ToShortDateString() + " - " + reader.GetDateTime(2).ToShortDateString();
                        result.Add(new TaskNotification()
                        {
                            TaskType = "Potrditev odsotnosti",
                            EntityInstanceId = absenceId,
                            TaskName = taskName,
                            TaskAction = 0
                        });
                    }
                }
                reader.Close();
                conn.Close();
            }
            return JsonConvert.SerializeObject(result);
        }

        internal class TaskNotification
        {
            public string TaskType { get; set; }
            public Guid EntityInstanceId { get; set; }
            public string TaskName { get; set; }
            public int TaskAction { get; set; }
        }
    }
}
