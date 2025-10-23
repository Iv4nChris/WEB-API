using Microsoft.EntityFrameworkCore;
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
        public async Task<CategoryDTO> AddCategory(CategoryDTO dto)
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

        #region -- Add Food and Drinks --
        public async Task<AddFoodDTO> AddFood(AddFoodDTO dto)
        {
            var food = new Foods
            {
                FoodCategoriesId = dto.CategoryId,
                FoodName = dto.FoodName,
                FoodDescription = dto.FoodDescription
            };

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return dto;
        }
        #endregion

        #region -- Assign Food to Package --
        public async Task<AssignFoodToPackageDTO> AddMenu(AssignFoodToPackageDTO dto)
        {
            var newMenu = new FoodMenu
            {
                FoodPackageId = dto.PackageId,
                FoodsId = dto.FoodsId
            };
            _context.FoodMenus.Add(newMenu);
            await _context.SaveChangesAsync();
            return dto;
        }
        #endregion


        #region -- Get Category --
        public async Task<List<CategoryDTO>> GetCategories()
        {
            return await _context.FoodCategories.Select(cat => new CategoryDTO
            {
                Id = cat.FoodCategoriesId,
                Title = cat.CategoryName
            }).ToListAsync();
        }
        #endregion

        #region -- Get Foods --
        public async Task<List<FoodsDTO>> GetFoods()
        {
            return await _context.Foods.Select(food => new FoodsDTO
            {
                FoodID = food.FoodsId,
                CategoryId = food.FoodCategoriesId,
                FoodName = food.FoodName,
                FoodDescription = food.FoodDescription,
                FoodPrice = food.FoodPrice
            }).ToListAsync();
        }
        #endregion

        #region -- Get Packages --
        public async Task<List<GetPackagesDTO>> GetPackages()
        {
            return await _context.FoodPackages.Select(package => new GetPackagesDTO
            {
                Id = package.FoodPackageId,
                PackageName = package.PackageName,
                PackageDescription = package.PackageDescription,
                PackagePrice = package.PackagePrice

            }).ToListAsync();
        }
        #endregion

        #region -- Get All Package in Menu --
        public async Task<List<GetMenuDTO>> GetMenu()
        {
            var packages = await _context.FoodPackages.GroupJoin(
                _context.FoodMenus,
                package => package.FoodPackageId,
                menu => menu.FoodPackageId,
                (package, menu) => new GetMenuDTO
                {
                    FoodPackageId = package.FoodPackageId,
                    PackageName = package.PackageName,
                    PackageDescription = package.PackageDescription,
                    PackagePrice = package.PackagePrice,
                    Foods = menu.Join(_context.Foods,
                         menu => menu.FoodsId,
                         food => food.FoodsId,
                         (menu, food) => new FoodsDTO
                         {
                             FoodID = food.FoodsId,
                             FoodName = food.FoodName,
                             FoodDescription = food.FoodDescription,
                             CategoryId = food.FoodCategoriesId
                         }
                    ).ToList()
                }
            ).ToListAsync();

            return packages;
        }
        #endregion

        #region -- Save the selected order
        
        #endregion



        #region -- Get the order of the customer --
        #endregion



    }
}
