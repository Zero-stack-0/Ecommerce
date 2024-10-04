namespace Entities.Models;

public class LoggingException
{
    public LoggingException()
    { }
    public LoggingException(string message)
    {
        Message = message;
        CreatedAt = DateTime.UtcNow;
        IsResolved = false;
    }
    public long Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; }
}