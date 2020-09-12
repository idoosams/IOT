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
    public static class JoinGroup
    {
        [FunctionName("JoinGroup")]
        public static async Task AddToGroup(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalR(HubName = "OnTrackHub")]
        IAsyncCollector<SignalRGroupAction> signalRGroupActions,
        [Table("GroupInfo", Connection = "AzureWebJobsStorage")] CloudTable groupInfoTable)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var sessionParticipant = JsonConvert.DeserializeObject<SessionParticipant>(requestBody);

            var entity = new SessionParticipantTableEntity(sessionParticipant);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await groupInfoTable.ExecuteAsync(replace);
            //return ((SessionParticipantTableEntity)result.Result); TODO: check fail.

            await signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = sessionParticipant.participantId,
                    GroupName = sessionParticipant.groupId,
                    Action = GroupAction.Add
                });

        }

    }
}
