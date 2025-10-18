namespace WEB_API.DTOs.Foods
{
    public class FoodsDTO
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string? FoodDescription { get; set;}
        public decimal? FoodPrice { get; set; }

    }
}
