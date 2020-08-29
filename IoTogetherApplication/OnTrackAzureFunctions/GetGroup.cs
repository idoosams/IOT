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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;

namespace OnTrackAzureFunctions
{
    public static class GetGroup
    {
        [FunctionName("GetGroup")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("GroupInfo", Connection = "AzureWebJobsStorage")] CloudTable groupInfoTable,
            [Table("UsersInfo", Connection = "AzureWebJobsStorage")] CloudTable usersInfoTable,
            ILogger log)
        {
            string groupId = req.Query["groupId"];

            var query = new TableQuery<SessionParticipantTableEntity>();
            query.Where(TableQuery.GenerateFilterCondition("GroupId", QueryComparisons.Equal, groupId));
            query.Select(new List<string> { "ParticipantId", "IsAdmin" });
            var result = groupInfoTable.ExecuteQuery(query).ToList();
            var adminId = result.Find(u => u.IsAdmin == true).ParticipantId;
            List<string> participantsIds = result.Where(entry => !string.IsNullOrWhiteSpace(entry.ParticipantId)).Select(entry => entry.ParticipantId).ToList();

            int i = 0;
            string activeUsersQuery = string.Empty;
            foreach (string id in participantsIds)
            {
                i++;
                if (i == 1) { activeUsersQuery = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id); }
                else
                {
                    activeUsersQuery = TableQuery.CombineFilters(
                        activeUsersQuery,
                        TableOperators.Or,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id)
                        );
                }
            }
            TableQuery<ParticipantTableEntity> finalQuery = new TableQuery<ParticipantTableEntity>().Where(activeUsersQuery);
            List<ParticipantTableEntity> activeUsers = usersInfoTable.ExecuteQuery(finalQuery).ToList();

            var jsonToReturn = JsonConvert.SerializeObject(activeUsers);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }
}
