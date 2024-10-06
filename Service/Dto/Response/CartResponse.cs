namespace Service.Dto.Response
{
    public class CartResponse
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long AddedById { get; set; }
        public decimal PriceAtCartTime { get; set; }
        public decimal DiscountAtCartTime { get; set; }
        public bool IsPurchased { get; set; }
        public int Quantity { get; set; }
        public ProductResponse Product { get; set; }
        public decimal FinalPriceAfterDiscount { get; set; }
    }
}