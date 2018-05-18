using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp.Functions.Processing
{
    public static class ProcessingFunctionV2
    {
        /// <summary>
        /// Processes incoming telemetry data.
        /// </summary>
        /// <param name="telemetryDocuments">Trigger: Incoming telemetry data from change feed</param>
        /// <param name="drivers">Input: Driver information</param>
        /// <param name="criminals">Output: Criminal processing</param>
        /// <param name="speeding">Output: Speeding violation processing</param>
        /// <param name="log">Log</param>
        /// <returns>Processed data record</returns>
        [FunctionName("ProcessingFunctionV2")]
        [return: DocumentDB("testdb", "analytics", ConnectionStringSetting = "CosmosDbConnection")]
        public static ProcessedTelemetryMessageModel Run(
            [CosmosDBTrigger("testdb", "telemetry", ConnectionStringSetting = "CosmosDbConnection", CreateLeaseCollectionIfNotExists =true)]
                IReadOnlyList<Document> telemetryDocuments,
            [DocumentDB("testdb", "drivers", ConnectionStringSetting = "CosmosDbConnection")]IEnumerable<DriverModel> drivers,
            ILogger log)
        {
            foreach (var document in telemetryDocuments)
            {
                try
                {
                    var telemetryModel = JsonConvert.DeserializeObject<TelemetryMessageModel>(document.ToString());

                    log.LogInformation($"Processing Telemetry document for {telemetryModel.DeviceId} ...");

                    // Assign driver information
                    var driver = drivers.FirstOrDefault(d => d.Id == telemetryModel.DeviceId);
                    if (driver == null)
                    {
                        throw new InvalidOperationException($"Driver for Vehicle {telemetryModel.DeviceId} not found!");
                    }

                    // Create processed data record
                    var processedTelemetry = new ProcessedTelemetryMessageModel(telemetryModel, document.Id, 
                        driver.Name, driver.IsCriminal);
                                        
                    // return processed data record for storage
                    return processedTelemetry;
                }
                catch (Exception ex)
                {
                    log.LogError($"Exception occurred processing document {document.Id}: [{ex.Message}]");
                }
            }

            return null;
        }

        private static bool IsCriminal(ProcessedTelemetryMessageModel message)
        {
            return message.IsCriminal;
        }

        private static bool IsSpeeding(ProcessedTelemetryMessageModel message)
        {
            bool isSpeeding = message.OriginalTelemetry.Velocity > 130;
            return isSpeeding;
        }
    }
}
