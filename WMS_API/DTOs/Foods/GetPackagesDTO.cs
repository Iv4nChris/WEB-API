namespace WEB_API.DTOs.Foods
{
    public class GetPackagesDTO
    {
        public int FoodPackageId { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageDescription { get; set; } = string.Empty ;
        public decimal PackagePrice { get; set; }
        public required List<FoodsDTO> Foods { get; set; }

    }
}
