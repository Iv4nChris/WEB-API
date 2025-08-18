using WMS_API.Models;

namespace WEB_API.Models
{
    public class ResetPasswordLink
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public Users User { get; set; }
    }
}
