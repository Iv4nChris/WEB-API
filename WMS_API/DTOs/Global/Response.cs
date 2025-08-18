namespace WEB_API.DTOs.Global
{
    public class Response
    {
        public bool Success {  get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
