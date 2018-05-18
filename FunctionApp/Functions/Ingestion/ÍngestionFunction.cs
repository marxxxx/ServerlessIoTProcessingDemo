using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Shared;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionApp.Functions.Ingestion
{    
    public static class IngestionFunction
    {
        /// <summary>
        /// Stores incoming telemetry data.
        /// </summary>
        /// <param name="req">Trigger: Incoming telemetry data request</param>
        /// <param name="documents">Output: Documents in event store</param>
        /// <param name="log">Logger</param>
        /// <returns></returns>
        [FunctionName("IngestionFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]TelemetryMessageModel req,
            [DocumentDB("testdb", "telemetry", 
            ConnectionStringSetting="CosmosDbConnection")]IAsyncCollector<TelemetryMessageModel>documents,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            await documents.AddAsync(req);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
