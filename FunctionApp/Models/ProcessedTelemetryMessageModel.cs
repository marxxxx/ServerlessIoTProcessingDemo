using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class ProcessedTelemetryMessageModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public TelemetryMessageModel OriginalTelemetry { get; set; }
        public string OriginalTelemetryId { get; set; }

        public string Driver { get; set; }
        public bool IsCriminal { get; set; }
        public bool IsSpeeding { get; set; }

        public ProcessedTelemetryMessageModel()
        {

        }

        public ProcessedTelemetryMessageModel(TelemetryMessageModel originalTelemetry, string originalTelemetryId)
        {
            this.Id = originalTelemetry.DeviceId;
            this.OriginalTelemetry = originalTelemetry;
            this.OriginalTelemetryId = originalTelemetryId;
        }

        public ProcessedTelemetryMessageModel(TelemetryMessageModel originalTelemetry, string originalTelemetryId, string driver) 
            : this(originalTelemetry, originalTelemetryId)
        {
            this.Driver = driver;
        }
    }
}
