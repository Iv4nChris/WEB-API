using System.ComponentModel.DataAnnotations;

namespace WEB_API.Models
{
    public class FoodMenu
    {
        [Key]
        public int FoodMenuId { get; set; }
        public int FoodPackageId { get; set; }
        public int FoodId { get; set; }
        public string PackageDescription { get; set; } = string.Empty;
        public bool isAvailable { get; set; } = true;
        public FoodPackage FoodPackage { get; set; }
        public Foods Foods { get; set; }
    }
}
