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
    public static class GetUsers
    {
        [FunctionName("GetUsers")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            if(Guid.TryParse(req.Query["userId"], out Guid userId))
            {
                var result = new User();
                using(SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    SqlDataReader reader;
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = "select Id, Name, DateOfBirth from Users where Id = @userId"
                    };
                    cmd.Parameters.Add("@userId", System.Data.SqlDbType.UniqueIdentifier);
                    cmd.Parameters["@userId"].Value = userId;

                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = new User
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2)
                            };
                        }
                    }
                    reader.Close();
                    conn.Close();
                }
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                var result = new List<User>();
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    SqlDataReader reader;
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = "select Id, Name, DateOfBirth from Users"
                    };
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new User()
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2)
                            });
                        }
                    }
                    reader.Close();
                    conn.Close();
                }
                return JsonConvert.SerializeObject(result);
            }
        }
    }
}
