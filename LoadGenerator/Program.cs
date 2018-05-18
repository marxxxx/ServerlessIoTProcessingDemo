using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoadGenerator
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            tasks.Add(SendDriverTelemetryAsync("L-515-IL"));
            tasks.Add(SendDriverTelemetryAsync("L-834-LN"));
            tasks.Add(SendDriverTelemetryAsync("L-123-XY"));

            Task.WhenAll(tasks).Wait();
        }

        static async Task SendDriverTelemetryAsync(string deviceId)
        {
            string functionAppEndpoint = ConfigurationManager.AppSettings["FunctionAppEndpoint"];
            Random rand = new Random();
            while (true)
            {

                var message = new TelemetryMessageModel()
                {
                    DeviceId = deviceId,
                    Timestamp = DateTime.UtcNow,
                    Velocity = rand.Next(10, 250)
                };

                var payload = new StringContent(JsonConvert.SerializeObject(message));
                payload.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                await httpClient.PostAsync(functionAppEndpoint, payload);
                Console.WriteLine($"Sent telemetry information for {deviceId} at speed {message.Velocity}.");
            }
        }
    }
}
