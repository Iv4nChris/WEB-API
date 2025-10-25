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

        #region -- Save the selected order || Add to Cart --
        public async Task<OrderDTO> SaveOrder(OrderDTO dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                throw new ArgumentException("Invalid UserId");
            }
            //TODO
            //check package and food is the same value in the existing order
            //Update the qty/order base on the input 
            var order = new Orders
            {
                UserId = dto.UserId,
                FoodPackageId = dto.FoodPackageId,
                FoodId = dto.FoodId,
                Order = dto.Order,
                User = user
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return dto;
        }
        #endregion

        #region -- Get the order of the customer --
        public async Task<List<GetOrdersDTO>> GetOrder(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new GetOrdersDTO
                {
                    OrderId = o.Id,
                    DateOrder = o.CreatedDate,
                    Status = o.IsDelivered,
                    Packages = _context.FoodPackages
                        .Where(fp => fp.FoodPackageId == o.FoodPackageId)
                        .Select(fp => new GetPackagesDTO
                        {
                            Id = fp.FoodPackageId,
                            PackageName = fp.PackageName,
                            PackageDescription = fp.PackageDescription,
                            PackagePrice = fp.PackagePrice
                        }).ToList(),
                    Foods = _context.Foods
                        .Where(f => f.FoodsId == o.FoodId)
                        .Select(f => new FoodsDTO
                        {
                            FoodID = f.FoodsId,
                            FoodName = f.FoodName,
                            FoodDescription = f.FoodDescription,
                            FoodPrice = f.FoodPrice,
                            CategoryId = f.FoodCategoriesId
                        }).ToList()
                }).ToListAsync();
            return orders;
        }
        #endregion

        #region -- Place Order --
        #endregion



    }
}
