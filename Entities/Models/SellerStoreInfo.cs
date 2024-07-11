namespace Entities.Models
{
    public class SellerStoreInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public Users User { get; set; }
    }
}