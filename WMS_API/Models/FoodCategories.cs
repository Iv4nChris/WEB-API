using System.ComponentModel.DataAnnotations;

namespace WEB_API.Models
{
    public class FoodCategories
    {
        [Key]
        public int FoodCategoriesId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
