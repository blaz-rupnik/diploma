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
    public static class CreateUser
    {
        [FunctionName("CreateUser")]
        public static async Task<User> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).
                ReadToEndAsync();
            User data = JsonConvert.DeserializeObject<User>(requestBody);
            data.Id = Guid.NewGuid();
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            using(SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "insert into Users values(@userId," +
                    "@userName,@userDateOfBirth)"
                };
                cmd.Parameters.Add("@userId", 
                    System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@userId"].Value = data.Id;
                cmd.Parameters.Add("@userName", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@userName"].Value = data.Name;
                cmd.Parameters.Add("@userDateOfBirth", 
                    System.Data.SqlDbType.DateTime);
                cmd.Parameters["@userDateOfBirth"].Value = data.DateOfBirth;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return data;
        }
    }
}
