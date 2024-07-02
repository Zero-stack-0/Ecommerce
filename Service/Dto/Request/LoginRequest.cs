namespace Service.Dto
{
    public class LoginRequest
    {
        public string EmailIdOrUserName { get; set; }
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}