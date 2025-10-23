using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Services;
using WEB_API.DTOs;
using WEB_API.Models;
using WEB_API.DTOs.Foods;
using Org.BouncyCastle.Asn1.Crmf;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodsServices _foodsServices;

        public FoodsController(FoodsServices foodsServices)
        {
            _foodsServices = foodsServices;
        }

        #region -- Package Method --
        [AllowAnonymous]
        [HttpGet("GetPackages")]
        public async Task<ActionResult<List<GetPackagesDTO>>> GetPackages()
        {
            var packageList = await _foodsServices.GetPackages();
            if (packageList == null)
            {
                return NotFound();
            }
            return Ok(packageList);
        }

        [HttpPost("AddPackage")]
        public async Task<ActionResult> AddFoodPackage([FromBody] AddPackageDTO dto)
        {
            var newPackage = await _foodsServices.AddFoodPackage(dto);
            if (newPackage == null)
            {
                return BadRequest("Failed to add food package");
            }
            return Ok(newPackage);
        }

        #endregion

        #region -- Category Method --

        [AllowAnonymous]
        [HttpGet("GetCategory")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var CategoryList = await _foodsServices.GetCategories();
            if (CategoryList == null)
            {
                return NotFound();
            }
            return Ok(CategoryList);
        }

        [AllowAnonymous]
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddFoodCategory([FromBody] CategoryDTO dto)
        {
            var newCategory = await _foodsServices.AddCategory(dto);
            if (newCategory == null)
            {
                return BadRequest("Failed to add food category");
            }
            return Ok(newCategory);
        }

        #endregion

        #region -- Food Method --

        [AllowAnonymous]
        [HttpGet("GetFoods")]
        public async Task<ActionResult<List<FoodsDTO>>> GetFoods()
        {
            var FoodList = await _foodsServices.GetFoods();
            if (FoodList == null)
            {
                return NotFound();
            }

            return Ok(FoodList);
        }

        [HttpPost("AddFood")]
        public async Task<ActionResult> AddFood([FromBody] AddFoodDTO dto)
        {
            var newFood = await _foodsServices.AddFood(dto);
            if (newFood == null)
            {
                return BadRequest("Failed to add food");
            }
            return Ok(newFood);
        }

        #endregion

        #region -- Menu Method --
        [HttpPost("AssignFoodToPackage")]
        public async Task<ActionResult> AddMenu([FromBody] AssignFoodToPackageDTO dto)
        {
            var newMenu = await _foodsServices.AddMenu(dto);
            if (newMenu == null)
            {
                return BadRequest("Failed to assign food to package");
            }
            return Ok(newMenu);
        }

        [HttpGet("GetMenu")]
        public async Task<ActionResult<List<GetMenuDTO>>> GetPackageMenu()
        {
            var PackageList = await _foodsServices.GetMenu();
            if(PackageList == null)
            {
                return BadRequest("Empty Package");
            }

            return Ok(PackageList);

        }

        #endregion



    }
}
