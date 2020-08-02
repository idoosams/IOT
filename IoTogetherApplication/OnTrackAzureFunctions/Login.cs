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
using System.Net.Http;
using System.Net;
using System.Linq;
using System.Text;

namespace OnTrackAzureFunctions
{
    public static class Login
    {
        [FunctionName("Login")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("UsersInfo", Connection = "AzureWebJobsStorage")] CloudTable usersInfoTable,
            ILogger log)
        {
            string email = req.Query["email"];
            string password = req.Query["password"];

            var query = new TableQuery<ParticipantTableEntity>();
            query.Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)));
            var items = usersInfoTable.ExecuteQuery(query).ToList();
            if (items.Count != 1)
            {
                return null;
            }
            var jsonToReturn = JsonConvert.SerializeObject((ParticipantTableEntity)items.First());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };


        }
    }
}
