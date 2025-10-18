using System.ComponentModel.DataAnnotations;

namespace WEB_API.Models
{
    public class FoodPackage
    {
        [Key]
        public int FoodPackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageDescription { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
        public bool isAvailable { get; set; } = true;
    }
}
