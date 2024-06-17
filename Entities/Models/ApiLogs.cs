namespace Entities.Models
{
    public class ApiLogs
    {
        public ApiLogs()
        { }

        public ApiLogs(string request, string response, string url)
        {
            Request = request;
            Response = response;
            Url = url;
            CreatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}