namespace Service.Dto.Request
{
    public class UpdatePasswordRequest
    {
        public string Requestor { get; set; }
        public string ResetToken { get; set; }
        public string PassWord { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}