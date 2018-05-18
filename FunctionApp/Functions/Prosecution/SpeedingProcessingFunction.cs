using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Functions.Prosecution
{
    public static class SpeedingProcessingFunction
    {
        [FunctionName("SpeedingProcessingFunction")]
        public static void Run([QueueTrigger("speeding", Connection = "StorageConnection")]string speedingQueueItem, ILogger log)
        {
            log.LogWarning($"Processing speeding violation: {speedingQueueItem}");
        }
    }
}
