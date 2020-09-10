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
using Microsoft.Azure.NotificationHubs;
using Xamarin.Essentials;

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

            var entity = new UserStatsTableEntity(data.Id, data.Latitude, data.Longtitude, data.Speed, data.BaterryCharge, data.Connectivity);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await userStatsTable.ExecuteAsync(replace);

            await signalRUserStats.AddAsync(
            new SignalRMessage
            {
                Target = "userStatsUpdate",
                Arguments = new[] { data }
            });


            try
            {
                if (entity.BaterryCharge < 0.15)
                {
                    await CreateNotification(entity.RowKey + " has low battery!");
                }
                /*var location = await Geolocation.GetLocationAsync();
                var distance = Location.CalculateDistance(location, entity.Latitude, entity.Longtitude, DistanceUnits.Kilometers);
                if (distance > 1)
                {
                    await CreateNotification(entity.RowKey + " is far!");
                }*/
                var networkState = entity.Connectivity;
                if (!networkState)
                {
                    await CreateNotification(entity.RowKey + " is not connected!");
                }

            }
            catch (Exception ex)
            {
            }

            return new OkObjectResult("OK");
        }

        private async static Task CreateNotification(string message)
        {
            var connectionString = "Endpoint=sb://iotogethernotificationsns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=NW358Y1/qG1d7pE0X7xHeYA7LBGy20D/rVdW/GW0mZ0=";
            var hubName = "IOTogetherNotifications";
            var nhClient = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);

            var notificationPayload = "{\"data\":{\"body\":\"" + message + "\",\"title\":\"" + "OnTrack Notification" + "\"}}";
            var notification = new FcmNotification(notificationPayload);

            var outcomeFcmByTag = await nhClient.SendFcmNativeNotificationAsync(notificationPayload);

        }
    }
}
