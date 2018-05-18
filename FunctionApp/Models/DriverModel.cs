using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class DriverModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsCriminal { get; set; }
    }
}
