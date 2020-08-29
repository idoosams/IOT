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

namespace OnTrackAzureFunctions
{
    public static class SignUp
    {
        [FunctionName("SignUp")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("UsersInfo", Connection = "AzureWebJobsStorage")] CloudTable usersInfoTable,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Participant>(requestBody);

            var entity = new ParticipantTableEntity(data);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await usersInfoTable.ExecuteAsync(replace);

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}
