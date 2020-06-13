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

namespace OnTrackAzureFunctions
{
    public static class UserFuncs
    {
        [FunctionName("UpdateUserStatus")]
        public static async Task<IActionResult> Run(
            [Queue("usersqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "simplechat")]
            SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (!string.IsNullOrEmpty(name))
            {
                // Add a message to the output collection.
                msg.Add(string.Format("Name passed to the function: {0}", name));
                msg.Add(string.Format("connection is: {0}", connectionInfo));
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
