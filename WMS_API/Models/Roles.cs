using System.ComponentModel.DataAnnotations;
using WMS_API.Models;

namespace WEB_API.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
