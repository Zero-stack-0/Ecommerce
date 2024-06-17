using Service.Dto.Response;

namespace Service.Dto
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public bool IsEmailVerified { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public RoleResponse Role { get; set; }
    }
}