using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eauction_Seller_API.Models
{
    public class SellerInfo
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "pin")]
        public string Pin { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }

    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty(PropertyName = "detailedDescription")]
        public string DetailedDescription { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "startingPrice")]
        public double StartingPrice { get; set; }

        [JsonProperty(PropertyName = "bidEndDate")]
        public DateTime BidEndDate { get; set; }

        [JsonProperty(PropertyName = "sellerInfo")]
        public SellerInfo SellerInfo { get; set; }
    }
}
