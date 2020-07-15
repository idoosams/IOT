using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OnTrackAzureFunctions
{
    public static class Users
    {
        [FunctionName("newUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "talk")]
            HttpRequest req,
            [SignalR(HubName = "simplechat")]
            IAsyncCollector<SignalRMessage> questionR,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string json = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic obj = JsonConvert.DeserializeObject(json);
                var jObject = new JObject(obj);

                await questionR.AddAsync(
                    new SignalRMessage
                    {
                        Target = Constants.UserTarget,
                        Arguments = new[] { jObject }
                    });

                var name = obj.name.ToString();
                var text = obj.text.ToString();

                // NOTE: returning values is helpful for testing requests in a browser
                // or with a program such as Postman
                return new OkObjectResult($"Hello {name}, your message was '{text}'");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("There was an error: " + ex.Message);
            }
        }
    }
}
