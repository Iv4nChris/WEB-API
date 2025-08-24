using System.ComponentModel.DataAnnotations;
using WMS_API.Models;

namespace WEB_API.Models
{
    public class ResetPasswordLink
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public Users User { get; set; }
    }
}
