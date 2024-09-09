namespace Service.Dto.Request.Admin.Category
{
    public class DeleteRequest
    {
        public UserResponse Requestor { get; set; }
        public long CategoryId { get; set; }
    }
}