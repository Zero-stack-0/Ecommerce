namespace Entities.Models
{
    public class Product
    {
        public Product()
        { }

        public Product(string name, string description, string sku, int quantity, decimal price, decimal discount, int maxOrderQuantity,
        long categoryId, long createdById, long sellerStoreInfoId, string imageUrl)
        {
            Name = name;
            Description = description;
            SKU = sku;
            Quantity = quantity;
            Price = price;
            Discount = discount;
            MaxOrderQuantity = maxOrderQuantity;
            CategoryId = categoryId;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
            Rating = 0;
            SellerStoreInfoId = sellerStoreInfoId;
            ImageUrl = imageUrl;
        }
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
        public long CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long SellerStoreInfoId { get; set; }
        public bool IsDeleted { get; set; }
        public Category Category { get; set; }
        public Users CreatedBy { get; set; }
        public SellerStoreInfo SellerStoreInfo { get; set; }
        public string ImageUrl { get; set; }
    }
}