namespace Entities.Models
{
    public class SellerRequest
    {
        public SellerRequest()
        { }

        public SellerRequest(long userId, string storeName, string storeAddress, string storeContactNumber, PaymentMethod paymentMethod)
        {
            IsApproved = false;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            StoreAddress = storeAddress;
            StoreName = storeName;
            StoreContactNumber = storeContactNumber;
            PaymentMethod = paymentMethod;
            Status = SellerReqeustStatus.Pending;
        }
        public long Id { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreContactNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public SellerReqeustStatus Status { get; set; }
        public bool IsApproved { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Users User { get; set; }

        public void UpdateStatus(SellerReqeustStatus status)
        {
            Status = status;
        }
    }

    public enum PaymentMethod
    {
        Online = 1,
        COD,
        BOTH
    }

    public enum SellerReqeustStatus
    {
        Pending = 1,
        Accepted,
        Rejected
    }
}