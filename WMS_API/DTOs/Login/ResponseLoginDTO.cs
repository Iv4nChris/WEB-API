namespace WEB_API.DTOs
{
    public class ResponseLoginDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }

    }
}
