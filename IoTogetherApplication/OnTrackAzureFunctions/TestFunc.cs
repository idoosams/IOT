using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json.Linq;

namespace OnTrackAzureFunctions
{
    public class TestFunc
    {
        public class MyPoco
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string Text { get; set; }
        }

        [FunctionName("TableOutput")]
        public static async Task<IActionResult> TableOutputAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [Table("UserStats", Connection = "AzureWebJobsStorage")] CloudTable userStatsTable,
            [SignalR(HubName = "OnTrackHub")]IAsyncCollector<SignalRMessage> signalRUserStats)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserStats>(requestBody);
            dynamic x = JsonConvert.DeserializeObject(requestBody);
            var jObject = new JObject(x);

            var entity = new UserStatsTableEntity(data.Id, data.Latitude, data.Longtitude, data.Speed, data.BaterryCharge, data.Connectivity);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await userStatsTable.ExecuteAsync(replace);

            await signalRUserStats.AddAsync(
            new SignalRMessage
            {
                Target = "userStatsUpdate",
                Arguments = new[] { data }
            });

            return new OkObjectResult("OK");
        }
    }
}
