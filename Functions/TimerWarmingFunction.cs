using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public static class TimerWarmingFunction
    {
        [FunctionName("TimerWarmingFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            using(var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync("https://functions20190602101203.azurewebsites.net/api/DummyFunction");
            }
            log.LogInformation($"Called warming function at {DateTime.Now}");
        }
    }
}
