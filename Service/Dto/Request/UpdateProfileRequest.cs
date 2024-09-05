using Microsoft.AspNetCore.Http;

namespace Service.Dto.Request
{
    public class UpdateProfileRequest
    {
        public UserResponse Requestor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public IFormFile ProfilePic { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}