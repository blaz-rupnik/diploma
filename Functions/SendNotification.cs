using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio.Rest.Api.V2010.Account;

namespace Functions
{
    public static class SendNotification
    {
        [FunctionName("SendNotification")]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [TwilioSms(From = "+17029048046", Body = "Testno sporocilo")] out CreateMessageOptions messageOptions)
        {
            messageOptions = new CreateMessageOptions(new Twilio.Types.PhoneNumber("+38631251302"));
        }
    }
}
