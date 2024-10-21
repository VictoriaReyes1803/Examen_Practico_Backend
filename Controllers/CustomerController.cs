using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Proyecto.Services;
using Newtonsoft.Json;
using Proyecto.Models;
using System.Globalization;

namespace Proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {

        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        

        [HttpGet("getCustomer")]
        public async Task<IActionResult> GetCustomerBasicInfo()
        {
            var customer = await _customerService.GetCustomerAsync();

            
            var Phone = customer.phoneHome ?? customer.phoneMobile;
            var result = new
            {
                customer.customerId,
                customer.email,
                phone = Phone,
                customer.birthday,
                customer.firstName,
                customer.lastName,
                addresses = customer.addresses ?? new List<Address>()
            };

            return Ok(result);
        }


        // 2.
        [HttpGet("addresses/sorted")]
        public async Task<IActionResult> GetSortedAddresses([FromQuery] string sortBy = "Address1", [FromQuery] bool ascending = true)
        {
            var customer = await _customerService.GetCustomerAsync();
            var addresses = customer.addresses;

            addresses = sortBy switch
            {
                "CreationDate" => ascending
                    ? addresses.OrderBy(a => DateTime.ParseExact(a.creationDate, "yyyyMMddTHHmmssfffffZ", CultureInfo.InvariantCulture)).ToList()
                    : addresses.OrderByDescending(a => DateTime.ParseExact(a.creationDate, "yyyyMMddTHHmmssfffffZ", CultureInfo.InvariantCulture)).ToList(),
                _ => ascending
                    ? addresses.OrderBy(a => a.address1).ToList()
                    : addresses.OrderByDescending(a => a.address1).ToList()
            };

            return Ok(addresses);
        }


        // 3. 
        [HttpGet("addresses/preferred")]
        public async Task<IActionResult> GetPreferredAddress()
        {
            var customer = await _customerService.GetCustomerAsync();
            var preferredAddress = customer.addresses.FirstOrDefault(a => a.preferred);

            if (preferredAddress == null)
                return NotFound("No preferred address found");

            return Ok(preferredAddress);
        }

        // 4. 
        [HttpGet("addresses/postalcode")]
        public async Task<IActionResult> GetAddressesByPostalCode([FromQuery] string postalCode)
        {
            var customer = await _customerService.GetCustomerAsync();
            var addresses = customer.addresses.Where(a => a.postalCode == postalCode).ToList();

            if (!addresses.Any())
                return NotFound("No addresses found for the given postal code");

            return Ok(addresses);
        }
    }
    }
