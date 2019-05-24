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
    public static class GetVacationLeaves
    {
        [FunctionName("GetVacationLeaves")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("MyConnectionString");
            if (Guid.TryParse(req.Query["absenceId"], out Guid absenceId))
            {
                var result = new VacationLeave();
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    SqlDataReader reader;
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = "select vl.Id as AbsenceId, vl.DateFrom, vl.DateTo, vl.DaysPending, vl.StatusId, vl.UserId, u.Name as UserName, u.DateOfBirth, vrs.Name from VacationLeaves vl"
                        + " inner join Users u on vl.UserId = u.Id inner join VacationRequestStatus vrs on vrs.Id = vl.StatusId where vl.Id = @absenceId"
                    };
                    cmd.Parameters.Add("@absenceId", System.Data.SqlDbType.UniqueIdentifier);
                    cmd.Parameters["@absenceId"].Value = absenceId;

                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = new VacationLeave
                            {
                                Id = reader.GetGuid(0),
                                DateFrom = reader.GetDateTime(1),
                                DateTo = reader.GetDateTime(2),
                                StatusId = reader.GetGuid(4),
                                UserId = reader.GetGuid(5),
                                User = new User
                                {
                                    Id = reader.GetGuid(5),
                                    Name = reader.GetString(6),
                                    DateOfBirth = reader.GetDateTime(7),
                                },
                                Status = new VacationRequestStatus {
                                    Id = reader.GetGuid(4),
                                    Name = reader.GetString(8)
                                }
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
                var result = new List<VacationLeave>();
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    SqlDataReader reader;
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = "select vl.Id as AbsenceId, vl.DateFrom, vl.DateTo, vl.DaysPending, vl.StatusId, vl.UserId, u.Name as UserName, u.DateOfBirth, vrs.Name from VacationLeaves vl"
                        + " inner join Users u on vl.UserId = u.Id inner join VacationRequestStatus vrs on vrs.Id = vl.StatusId"
                    };
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new VacationLeave()
                            {
                                Id = reader.GetGuid(0),
                                DateFrom = reader.GetDateTime(1),
                                DateTo = reader.GetDateTime(2),
                                StatusId = reader.GetGuid(4),
                                UserId = reader.GetGuid(5),
                                User = new User
                                {
                                    Id = reader.GetGuid(5),
                                    Name = reader.GetString(6),
                                    DateOfBirth = reader.GetDateTime(7),
                                },
                                Status = new VacationRequestStatus
                                {
                                    Id = reader.GetGuid(4),
                                    Name = reader.GetString(8)
                                }
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
