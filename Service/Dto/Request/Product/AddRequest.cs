using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Service.Dto.Request.Product
{
    public class AddRequest
    {
        public UserResponse Requestor { get; set; }

        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string SKU { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be greater than 0")]
        public int Quantity { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Price should be greater than 0")]
        public decimal Price { get; set; }

        [Range(0.00, 99)]
        public decimal Discount { get; set; }

        [Range(1, 100000, ErrorMessage = "MaxOrderQuantity should be greater than 0")]
        public int MaxOrderQuantity { get; set; }
        public long CategoryId { get; set; }

        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}