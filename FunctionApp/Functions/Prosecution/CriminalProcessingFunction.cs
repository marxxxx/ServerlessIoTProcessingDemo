using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp.Functions.Prosecution
{
    public static class CriminalProcessingFunction
    {
        [FunctionName("CriminalProcessingFunction")]
        public static void Run([QueueTrigger("criminals", Connection = "StorageConnection")]string criminalQueueItem, TraceWriter log)
        {
            log.Warning($"Processing criminal: {criminalQueueItem}");
        }
    }
}
