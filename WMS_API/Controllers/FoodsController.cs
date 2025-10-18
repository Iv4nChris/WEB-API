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

        [AllowAnonymous]
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddFoodCategory([FromBody] AddCategoryDTO dto)
        {
            var newCategory = await _foodsServices.AddCategory(dto);
            if (newCategory == null)
            {
                return BadRequest("Failed to add food category");
            }
            return Ok(newCategory);
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("GetPackages")]
        public async Task<ActionResult<List<GetPackagesDTO>>> GetPackages()
        {
            var PackageList = await _foodsServices.GetPackages();
            if(PackageList == null)
            {
                return BadRequest("Empty Package");
            }

            return Ok(PackageList);

        }
    }
}
