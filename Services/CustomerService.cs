﻿using System.Net.Http;
using System.Threading.Tasks;
using Proyecto.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace Proyecto.Services
{
    public class CustomerService

    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(HttpClient httpClient, ILogger<CustomerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }



        public async Task<Customer> GetCustomerAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://examentecnico.azurewebsites.net/v3/api/Test/Customer");
            request.Headers.Add("Device", "POSTMAN");
            request.Headers.Add("Version", "2.0.6.0");
            request.Headers.Add("Authorization", "Basic Y2hyaXN0b3BoZXJAZGV2ZWxvcC5teDpUZXN0aW5nRGV2ZWxvcDEyM0AuLi4=");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();


            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"El tipo de contenido es: {response}{responseBody} ");


            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,

            };
            responseBody = CleanResponseBody(responseBody);
            _logger.LogInformation($"Longitud después de limpiar: {responseBody.Length}");
            _logger.LogInformation($"Contenido limpio: {responseBody}");


            try
            {
                if (string.IsNullOrWhiteSpace(responseBody) || !IsJson(responseBody))
                {
                    _logger.LogWarning("La respuesta no es un JSON válido.");
                    return null; // Manejar el caso según tus necesidades
                }

                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

                if (customer == null)
                {
                    _logger.LogWarning("El objeto Customer fue null después de la deserialización.");
                    return null; // Manejar el caso según tus necesidades
                }

                _logger.LogInformation($"Customer deserializado: {customer}");
                return customer;

            }
            catch (JsonSerializationException ex)
            {
                _logger.LogInformation($"Error al deserializar el JSON: {ex.Message}");
                Console.WriteLine($"Ruta del error: {ex.Path}");
                Console.WriteLine($"JSON: {response}");

                throw;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw; // Vuelve a lanzar la excepción
            }


        }
        private string CleanResponseBody(string responseBody)
        {
            if (string.IsNullOrEmpty(responseBody))
                return responseBody;

            if (responseBody.StartsWith("\"") && responseBody.EndsWith("\""))
            {
                responseBody = responseBody.Substring(1, responseBody.Length - 2);
            }


            // Eliminar emojis y caracteres no deseados
            responseBody = Regex.Replace(responseBody, @"[^\u0000-\u007F]+", ""); 
            responseBody = Regex.Replace(responseBody, @"\s+", " ");
            responseBody = Regex.Replace(responseBody, @"[^\{\}\[\]\"":,0-9a-zA-Z ]", "");
            responseBody = responseBody.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("rn", "");
            return responseBody;
        }

        private bool IsJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            try
            {
                var obj = JObject.Parse(input);
                return true; // Si no se lanza una excepción, es un JSON válido
            }
            catch (JsonReaderException)
            {
                return false; // No es un JSON válido
            }
            catch (Exception)
            {
                return false; // Manejo de errores generales
            }
        }
    }
}
