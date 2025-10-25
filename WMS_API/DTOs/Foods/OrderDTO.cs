namespace WEB_API.DTOs.Foods
{
    public class OrderDTO
    {
        public int UserId { get; set;  }
        public int? FoodPackageId { get; set;  }
        public int? FoodId { get; set;  }
        public int Order { get; set; } = 1;
    }
}
