using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API.Models
{
    public class FoodMenu
    {
        [Key]
        public int FoodMenuId { get; set; }
        public int FoodPackageId { get; set; }
        public int FoodsId { get; set; }
        public FoodPackage FoodPackage { get; set; }
        public Foods Foods { get; set; }
    }
}
