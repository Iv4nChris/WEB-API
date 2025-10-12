using System.ComponentModel.DataAnnotations;

namespace WEB_API.Models
{
    public class Foods
    {
        [Key]
        public int FoodsId { get; set; }
        public int FoodCategoriesId { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodDescription { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        public FoodCategories FoodCategories { get; set; }
    }
}
