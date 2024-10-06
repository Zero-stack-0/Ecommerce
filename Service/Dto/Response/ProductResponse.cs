using Entities.Models;

namespace Service.Dto.Response
{
    public class ProductResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int MaxOrderQuantity { get; set; }
        public long CategoryId { get; set; }
        public decimal Rating { get; set; } = 0;
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
    }
}