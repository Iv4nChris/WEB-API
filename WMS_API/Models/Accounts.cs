using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WMS_API.Models;

namespace WEB_API.Models
{
    public class Accounts
    {
        [Key]
        public int UsersId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        [ForeignKey(nameof(UsersId))]
        public Users Users { get; set; }
    }
}
