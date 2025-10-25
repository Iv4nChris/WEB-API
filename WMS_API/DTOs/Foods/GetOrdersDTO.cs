using WMS_API.Models;

namespace WEB_API.DTOs.Foods
{
    public class GetOrdersDTO
    {
        public int OrderId { get; set; }
        public ICollection<GetPackagesDTO>? Packages { get; set; }
        public ICollection<FoodsDTO>? Foods { get; set; }
        public DateTime DateOrder { get; set; }
        public bool Status { get; set; }

    }
}
