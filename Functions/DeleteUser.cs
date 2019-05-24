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
    public static class DeleteUser
    {
        [FunctionName("DeleteUser")]
        public static async Task<Guid> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            Guid userId = Guid.Parse(req.Query["userId"]);
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlDataReader reader;
                //check if user has related data
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "select * from VacationLeaves where UserId = @userId"
                };
                cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@userId"].Value = userId;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    return userId;
                }
                reader.Close();
                cmd.CommandText = "delete from Users where Id = @userId";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return userId;
        }
    }
}
