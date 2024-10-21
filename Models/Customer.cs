using Newtonsoft.Json;
using System.Globalization;

namespace Proyecto.Models
{
    public class Customer
    {
        [JsonProperty("customerId")]
        public string customerId { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phoneHome")]
        public string phoneHome { get; set; }

        [JsonProperty("phoneMobile")]
        public string phoneMobile { get; set; }

        [JsonProperty("birthday")]
        public string birthday { get; set; }

        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonProperty("lastName")]
        public string lastName { get; set; }

        [JsonProperty("creationDate")]
        public DateTime creationDate { get; set; }

        [JsonProperty("addresses")]
        public List<Address> addresses { get; set; }

    }
}

