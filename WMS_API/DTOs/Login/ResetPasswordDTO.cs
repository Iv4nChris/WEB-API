namespace WEB_API.DTOs.Login
{
    public class ResetPasswordDTO
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
