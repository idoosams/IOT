using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.NotificationHubs;

namespace OnTrackAzureFunctions
{
    public class UpdateUserStats
    {
        [FunctionName("UpdateUserStats")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Table("UserStats", Connection = "AzureWebJobsStorage")] CloudTable userStatsTable,
            [SignalR(HubName = "OnTrackHub")]IAsyncCollector<SignalRMessage> signalRUserStats)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserStats>(requestBody);
            string groupId = req.Query["groupId"];

            var entity = new UserStatsTableEntity(data.Id, data.Latitude, data.Longtitude, data.Speed, data.BaterryCharge, data.Connectivity);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await userStatsTable.ExecuteAsync(replace);

            await signalRUserStats.AddAsync(
            new SignalRMessage
            {
                Target = "userStatsUpdate",
                Arguments = new[] { data },
                GroupName = groupId,
            });

            return new OkObjectResult("OK");
        }
    }
}
