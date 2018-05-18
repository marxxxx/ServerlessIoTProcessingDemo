using System;

namespace Shared
{
    public class TelemetryMessageModel
    {        
        public DateTime Timestamp { get; set; }
        public string DeviceId { get; set; }
        public int Velocity { get; set; }
    }
}
