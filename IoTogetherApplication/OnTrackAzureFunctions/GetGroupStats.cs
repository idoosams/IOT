using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace OnTrackAzureFunctions
{
    public static class GetGroupStats
    {
        [FunctionName("GetGroupStats")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("UserStats", Connection = "AzureWebJobsStorage")] CloudTable userStatsTable,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var participantsIds = JsonConvert.DeserializeObject<List<string>>(requestBody);

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
            TableQuery<UserStatsTableEntity> finalQuery = new TableQuery<UserStatsTableEntity>().Where(activeUsersQuery);

            Dictionary<string, UserStatsTableEntity> dict = new Dictionary<string, UserStatsTableEntity>();
            userStatsTable.ExecuteQuery(finalQuery).ToList().ForEach(u => dict.Add(u.RowKey, u));

            var jsonToReturn = JsonConvert.SerializeObject(dict);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }
    }
}
