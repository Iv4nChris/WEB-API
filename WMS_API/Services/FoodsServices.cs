using WEB_API.DTOs.Foods;
using WEB_API.Models;
using WMS_API.Data;

namespace WEB_API.Services
{
    public class FoodsServices
    {
        private readonly ApplicationDbContext _context;
        public FoodsServices(ApplicationDbContext context) 
        {
            _context = context;
        }

        #region -- Add Food Category --
        public async Task<AddPackageDTO> AddFoodPackage(AddPackageDTO dto)
        {
            var foodPackage = new FoodPackage
            {
                PackageName = dto.PackageName,
                PackageDescription = dto.Description
            };

            _context.FoodPackages.Add(foodPackage);
            await _context.SaveChangesAsync();
            return dto;
        }
        #endregion

        #region -- Add Category --
        public async Task<AddCategoryDTO> AddCategory(AddCategoryDTO dto)
        {
            var category = new FoodCategories
            {
                CategoryName = dto.Title
            };
            _context.FoodCategories.Add(category);
            await _context.SaveChangesAsync();
            return dto;
        }
        #endregion
    }
}
