namespace Entities.Models
{
    public class UserPasswordResetRequest
    {
        public UserPasswordResetRequest()
        { }

        public UserPasswordResetRequest(long userdId)
        {
            UserId = userdId;
            IsUsed = false;
            ResetToken = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString();
            RequestedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public string ResetToken { get; set; }
        public DateTime RequestedAt { get; set; }
        public long UserId { get; set; }
        public Users User { get; set; }
        public bool IsDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void LinkUsed()
        {
            IsUsed = true;
        }
    }
}