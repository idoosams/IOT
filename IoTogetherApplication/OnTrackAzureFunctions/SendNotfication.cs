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
    public static class SendNotfication
    {
        [FunctionName("SendNotfication")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<NotficationInfo>(requestBody);

            try
            {
                    await CreateNotification($"Alert from {data.UserName}: {data.Message}");
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

