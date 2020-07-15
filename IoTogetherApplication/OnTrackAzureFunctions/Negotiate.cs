﻿using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace OnTrackAzureFunctions
{
    public static class Negotiate
    {
        [FunctionName("Negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous,"get",Route = "negotiate")]
            HttpRequest req,
            [SignalRConnectionInfo(HubName = "OnTrackHub")]
            SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}