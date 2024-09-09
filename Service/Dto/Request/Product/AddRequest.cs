using Microsoft.AspNetCore.Http;

namespace Service.Dto.Request.Product
{
    public class AddRequest
    {
        public UserResponse Requestor { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int MaxOrderQuantity { get; set; }
        public long CategoryId { get; set; }

        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}