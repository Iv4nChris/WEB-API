using System.ComponentModel.DataAnnotations.Schema;
using WMS_API.Models;

namespace WEB_API.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? FoodPackageId { get; set; }
        public int? FoodId { get; set; }
        public int Order { get; set; } = 1;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDelivered { get; set; } = false;
        [ForeignKey("UserId")]
        public required Users User { get; set; }

    }
}
