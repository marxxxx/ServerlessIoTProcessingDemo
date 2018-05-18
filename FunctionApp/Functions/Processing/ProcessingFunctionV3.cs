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
    public static class ProcessingFunctionV3
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
        [FunctionName("ProcessingFunctionV3")]
        [return: DocumentDB("testdb", "analytics", ConnectionStringSetting = "CosmosDbConnection")]
        public static ProcessedTelemetryMessageModel Run(
            [CosmosDBTrigger("testdb", "telemetry", ConnectionStringSetting = "CosmosDbConnection", CreateLeaseCollectionIfNotExists =true)]
                IReadOnlyList<Document> telemetryDocuments,
            [DocumentDB("testdb", "drivers", ConnectionStringSetting = "CosmosDbConnection")]IEnumerable<DriverModel> drivers,
            
            [Queue("criminals", Connection = "StorageConnection")]ICollector<ProcessedTelemetryMessageModel> criminals,
            [Queue("speeding", Connection = "StorageConnection")]ICollector<ProcessedTelemetryMessageModel> speeding,
            
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
                    var processedTelemetry = new ProcessedTelemetryMessageModel(telemetryModel, document.Id, driver.Name, driver.IsCriminal);
                    
                    // Criminal detection
                    if (IsCriminal(processedTelemetry))
                    {
                        log.LogWarning($"Driver {driver.Name} is a criminal!");
                        criminals.Add(processedTelemetry);
                    }

                    // Speeding detection
                    if (IsSpeeding(processedTelemetry))
                    {
                        log.LogWarning($"Driver {driver.Name} is speeding!");
                        speeding.Add(processedTelemetry);
                    }
                    

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
