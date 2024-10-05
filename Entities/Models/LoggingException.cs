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
    public string MethodName { get; set; }
    public string FileName { get; set; }
    public int LineNumber { get; set; }
    public string Request { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; }
}