using Newtonsoft.Json;
namespace Proyecto.Models
{
    public class Address
    {
        public string address1 { get; set; }
        public string? address2 { get; set; }
        public string addressId { get; set; }
        public string city { get; set; }
        public string? companyName { get; set; }
        public string countryCode { get; set; }
        public DateTime? CreationDate { get; set; }
        public string firstName { get; set; }
        public string? FullName { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string postalCode { get; set; }
        public string? PostBox { get; set; }
        public bool Preferred { get; set; }
        public string? Salutation { get; set; }
        public string? SecondName { get; set; }
        public string stateCode { get; set; }
        public string? Suite { get; set; }
        public string? Title { get; set; }
        public bool? C_EsFiscal { get; set; }
        public string? C_RazonSocial { get; set; }
        public string? C_RFC { get; set; }
        public string? C_UsoCFDI { get; set; }
        public string? C_Colonia { get; set; }
        public string? C_StreetNumber { get; set; }
        public string? C_BuildingNumber { get; set; }
        public double? c_latitude { get; set; }
        public double? c_longitude { get; set; }
        public string? C_Reference { get; set; }
    }
}
