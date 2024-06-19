namespace Service.Dto
{
    public class SignUpRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassWord { get; set; }
        public string ConfirmPassword { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
    }
}