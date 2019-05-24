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
    public static class DeleteVacationLeave
    {
        [FunctionName("DeleteVacationLeave")]
        public static async Task<Guid> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            Guid absenceId = Guid.Parse(req.Query["absenceId"]);
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "delete from VacationLeaves where Id = @absenceId"
                };
                cmd.Parameters.Add("@absenceId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@absenceId"].Value = absenceId;
              
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return absenceId;
        }
    }
}
