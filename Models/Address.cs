using Newtonsoft.Json;
using System.Globalization;
namespace Proyecto.Models
{
    public class Address
    {
        [JsonProperty("address1")]
        public string address1 { get; set; }

        [JsonProperty("creationDate")]
        public string creationDate { get; set; }

        [JsonProperty("address2")]
        public string address2 { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("countryCode")]
        public string countryCode { get; set; }

        [JsonProperty("postalCode")]
        public string postalCode { get; set; }

        [JsonProperty("preferred")]
        public bool preferred { get; set; }
    }
}
