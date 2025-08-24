using System.ComponentModel.DataAnnotations;
using WEB_API.Models;

namespace WMS_API.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;


        public Roles Role { get; set; }
        
    }
}
