using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.Cosmos.Table;

namespace OnTrackAzureFunctions
{
    public static class LeaveGroup
    {
        [FunctionName("LeaveGroup")]
        public static async Task RemoveFromGroup(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalR(HubName = "OnTrackHub")]
        IAsyncCollector<SignalRGroupAction> signalRGroupActions,
        [Table("GroupInfo", Connection = "AzureWebJobsStorage")] CloudTable groupInfoTable)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var sessionParticipant = JsonConvert.DeserializeObject<SessionParticipant>(requestBody);

            TableOperation retrieve = TableOperation.Retrieve<SessionParticipantTableEntity>("", sessionParticipant.id);
            TableResult result = await groupInfoTable.ExecuteAsync(retrieve);
            SessionParticipantTableEntity entity = (SessionParticipantTableEntity)result.Result;
            TableOperation delete = TableOperation.Delete(entity);
            await groupInfoTable.ExecuteAsync(delete); // TODO: check fali

            await signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = sessionParticipant.id,
                    GroupName = sessionParticipant.groupId,
                    Action = GroupAction.Remove
                });

        }
    }
}
