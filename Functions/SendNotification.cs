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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, [TwilioSms(From = "+17029048046")]
            out CreateMessageOptions messageOptions)
        {
            messageOptions = new CreateMessageOptions
                (new Twilio.Types.PhoneNumber("+38631251302"));
            var person = req.Query["person"].ToString();
            var dateFrom = req.Query["dateFrom"].ToString();
            var dateTo = req.Query["dateTo"].ToString();



            messageOptions.Body = "Uporabnik " + person + " je poslal zahtevo za letni dopust od: " + dateFrom + " do: " + dateTo 
                + ". Prosim da jo čimprej odobrite. Čakajoče odobritve daljše od 7 dni se avtomatično zavrnejo.";
        }
    }
}

               