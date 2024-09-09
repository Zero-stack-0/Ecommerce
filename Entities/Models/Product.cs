namespace Entities.Models
{
    public class Product
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
        public int Rating { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Category Category { get; set; }
        public Users CreatedBy { get; set; }
    }
}