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
    public static class CreateVacationLeave
    {
        [FunctionName("CreateVacationLeave")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            VacationLeave data = JsonConvert.DeserializeObject<VacationLeave>(requestBody);
            data.Id = Guid.NewGuid();
            data.DaysPending = 0;
            data.StatusId = Guid.Parse("39CF86C0-0E7B-4080-A684-E7E081B8FE17");
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "insert into VacationLeave values(@absenceId,@dateFrom,@dateTo,@userId,@statusId,@daysPending)"
                };
                cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@userId"].Value = data.UserId;
                cmd.Parameters.Add("@absenceId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@absenceId"].Value = data.Id;
                cmd.Parameters.Add("@dateFrom", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dateFrom"].Value = data.DateFrom;
                cmd.Parameters.Add("@dateTo", System.Data.SqlDbType.DateTime);
                cmd.Parameters["@dateTo"].Value = data.DateTo;
                cmd.Parameters.Add("@statusId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@statusId"].Value = data.StatusId;
                cmd.Parameters.Add("@daysPending", System.Data.SqlDbType.Int);
                cmd.Parameters["@daysPending"].Value = data.DaysPending;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }
    }
}
