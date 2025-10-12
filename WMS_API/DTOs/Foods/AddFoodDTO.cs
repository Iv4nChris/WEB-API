namespace WEB_API.DTOs.Foods
{
    public class AddFoodDTO
    {
        public int CategoryId { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodDescription { get; set; } = string.Empty;
    }
}
