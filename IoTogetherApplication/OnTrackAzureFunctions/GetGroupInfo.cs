using System.Threading.Tasks;
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
    public static class GetGroupInfo
    {
        [FunctionName("GetGroupInfo")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("GroupInfo", Connection = "AzureWebJobsStorage")] CloudTable groupInfoTable,
            [Table("UsersInfo", Connection = "AzureWebJobsStorage")] CloudTable usersInfoTable,
            ILogger log)
        {
            string groupId = req.Query["groupId"];

            var query = new TableQuery<SessionParticipantTableEntity>();
            query.Where(TableQuery.GenerateFilterCondition("GroupId", QueryComparisons.Equal, groupId));
            query.Select(new List<string> { "GroupId", "GroupName", "ParticipantId", "IsAdmin" });
            var result = groupInfoTable.ExecuteQuery(query).ToList();
            var adminRow = result.Find(u => u.IsAdmin == true);


            var groupName = adminRow != null ? adminRow.GroupName : null;
            var adminId = adminRow != null ? adminRow.ParticipantId : null;

            var jsonToReturn = JsonConvert.SerializeObject(new GroupInfo(adminId, groupName));

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }
}
