namespace Entities.Models
{
    public class Users
    {
        public Users()
        { }

        public Users(string firstName, string lastName, string emailId, string passWord, DateTime dateOfBirth, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailId = emailId;
            HashedPassword = passWord;
            DateOfBirth = dateOfBirth;
            Username = userName;
            CreatedOn = DateTime.UtcNow;
            IsEmailVerified = false;
            IsDeleted = false;
            LastLoginDate = DateTime.UtcNow;
            IsActive = true;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public bool IsEmailVerified { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public DateTime? AccountBlockedDueToWrongCredentialDate { get; set; }
        public Role Role { get; set; }

        public void ChangeLastLoginDate()
        {
            LastLoginDate = DateTime.UtcNow;
        }

        public void ChangeFailedLoginAttempts()
        {
            FailedLoginAttempts += 1;
            if (FailedLoginAttempts >= 5)
            {
                AccountBlockedDueToWrongCredentialDate = DateTime.UtcNow;
            }
        }

        public void UpdateFailedLoginAttemptAndDate()
        {
            FailedLoginAttempts = 0;
            AccountBlockedDueToWrongCredentialDate = null;
        }
    }
}