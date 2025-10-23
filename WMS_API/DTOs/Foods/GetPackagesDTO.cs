namespace WEB_API.DTOs.Foods
{
    public class GetPackagesDTO
    {
        public int Id  { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageDescription { get; set; } = string.Empty;
        public decimal PackagePrice { get; set; }
    }
}
