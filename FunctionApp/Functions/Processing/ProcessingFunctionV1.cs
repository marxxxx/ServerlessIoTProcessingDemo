using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

namespace FunctionApp.Functions.Processing
{
    public static class ProcessingFunctionV1
    {
        /// <summary>
        /// Processes incoming telemetry data.
        /// </summary>
        /// <param name="telemetryDocuments">Trigger: Incoming telemetry data from change feed</param>
        /// <param name="log">Log</param>
        /// <returns>Processed data record</returns>
        [FunctionName("ProcessingFunctionV1")]
        [return: DocumentDB("testdb", "analytics", ConnectionStringSetting = "CosmosDbConnection")]
        public static ProcessedTelemetryMessageModel Run(
            [CosmosDBTrigger("testdb", "telemetry", ConnectionStringSetting = "CosmosDbConnection", CreateLeaseCollectionIfNotExists = true)]
                IReadOnlyList<Document> telemetryDocuments,
            ILogger log)
        {
            foreach (var document in telemetryDocuments)
            {
                try
                {
                    var telemetryModel = JsonConvert.DeserializeObject<TelemetryMessageModel>(document.ToString());

                    log.LogInformation($"Processing Telemetry document for {telemetryModel.DeviceId} ...");

                    // return processed data record for storage
                    var processedTelemetry =  new ProcessedTelemetryMessageModel(telemetryModel, document.Id);

                    return processedTelemetry;
                }
                catch (Exception ex)
                {
                    log.LogError($"Exception occurred processing document {document.Id}: [{ex.Message}]");
                }
            }

            return null;
        }
    }
}
