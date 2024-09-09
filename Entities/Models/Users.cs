namespace Entities.Models
{
    public class Users
    {
        public Users()
        { }

        public Users(string firstName, string lastName, string emailId, string passWord, DateTime dateOfBirth, string userName, long countryId, long stateId, long cityId, string profilePicUrl)
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
            CountryId = countryId;
            StateId = stateId;
            CityId = cityId;
            ProfilePicUrl = profilePicUrl;
            AccountVerificationCode = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; } = 2;
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
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? AccountVerificationCode { get; set; }
        public int AccountVerificationResendCount { get; set; }
        public DateTime? AccountVerificationCodeSentAt { get; set; }
        public bool IsSeller { get; set; }
        public SellerRequest SellerRequest { get; set; }
        // public long SellerStoreInfoId { get; set; }
        // public SellerStoreInfo SellerStoreInfo { get; set; }

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

        public void VerifyAccount()
        {
            IsEmailVerified = true;
            AccountVerificationCodeSentAt = DateTime.UtcNow;
        }

        public void ResentVerifyEmail()
        {
            AccountVerificationResendCount += 1;
            AccountVerificationCodeSentAt = DateTime.UtcNow;
            AccountVerificationCode = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString();
        }

        public void UpdatePassword(string password)
        {
            HashedPassword = password;
            LastPasswordChangeDate = DateTime.UtcNow;
        }

        public void UpdateSeller()
        {
            IsSeller = true;
        }

        public void Update(string firstName, string lastName, string userName, string? profilePicUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = userName;
            ProfilePicUrl = profilePicUrl;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}