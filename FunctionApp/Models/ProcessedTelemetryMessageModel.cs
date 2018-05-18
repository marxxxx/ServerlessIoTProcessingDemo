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
        public TelemetryMessageModel OriginalTelemetry { get; set; }
        public string OriginalTelemetryId { get; set; }

        public string Driver { get; set; }
        public bool IsCriminal { get; set; }

        public ProcessedTelemetryMessageModel()
        {

        }

        public ProcessedTelemetryMessageModel(TelemetryMessageModel originalTelemetry, string originalTelemetryId)
        {
            this.OriginalTelemetry = originalTelemetry;
            this.OriginalTelemetryId = originalTelemetryId;
        }

        public ProcessedTelemetryMessageModel(TelemetryMessageModel originalTelemetry, string originalTelemetryId, string driver, bool isCriminal) 
            : this(originalTelemetry, originalTelemetryId)
        {
            this.Driver = driver;
            this.IsCriminal = isCriminal;
        }
    }
}
