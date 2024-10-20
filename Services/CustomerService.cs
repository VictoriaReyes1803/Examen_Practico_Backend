using System.Net.Http;
using System.Threading.Tasks;
using Proyecto.Models;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Serialization;


namespace Proyecto.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
   

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Customer> GetCustomerAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://examentecnico.azurewebsites.net/v3/api/Test/Customer");
            request.Headers.Add("Device", "POSTMAN");
            request.Headers.Add("Version", "2.0.6.0");
            request.Headers.Add("Authorization", "Basic Y2hyaXN0b3BoZXJAZGV2ZWxvcC5teDpUZXN0aW5nRGV2ZWxvcDEyM0AuLi4=");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

           
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = true
                }
            };
            try
            {

                var customerJson = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(customerJson, settings);
                Console.WriteLine($"pasoo : ");
                return customer;
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error al deserializar el JSON: {ex.Message}");
                Console.WriteLine($"Ruta del error: {ex.Path}");
                Console.WriteLine($"JSON: {response}");
                var customerJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"JSON recibido: {customerJson}");
                throw;
            }
        }
       
    }
}
