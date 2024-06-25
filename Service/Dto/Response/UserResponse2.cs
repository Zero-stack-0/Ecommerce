using Entities.Models;

namespace Service.Dto.Response
{
    public class UserResponse2
    {
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
        public Country Country { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}