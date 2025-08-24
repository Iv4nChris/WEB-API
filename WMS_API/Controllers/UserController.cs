using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.DTOs;
using WEB_API.DTOs.Login;
using WEB_API.Services;
using WMS_API.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
        {
            var users = await _userServices.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var userDto = await _userServices.GetUser(id);
            if (userDto == null)  return NotFound();
            return Ok(userDto);
        }

        [HttpGet("Account/{id}")]
        public async Task<ActionResult<AccountsDTO>> GetUserAccount(int id)
        {
            var account = await _userServices.GetAccount(id);
            if (account == null) return NotFound();
            
            return Ok(account);
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] AddUserDTO dto)
        {
            var newUser = await _userServices.AddUser(dto);
            if (newUser == null)
            {
                return BadRequest("Username or Email already Exist");
            }

            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("Role")]
        public async Task<ActionResult> AddRole([FromBody] RoleDTO dto)
        {
            var newRole = await _userServices.AddRole(dto);
            return Ok(newRole);
        }





    }
}
