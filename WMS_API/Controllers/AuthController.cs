using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WEB_API.DTOs.Login;
using WEB_API.Services;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AuthServices _authServices;
        private readonly UserServices _userServices;
        private readonly EmailServices _emailServices;
        public AuthController(AuthServices authServices, UserServices userServices, EmailServices emailServices )
        {
            _authServices = authServices;
            _userServices = userServices;
            _emailServices = emailServices;
        }


        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {

            /*
             ⚡ Important:
             - If the token is expired, this method will NOT run.
             - ASP.NET Core checks the JWT before it gets here.
             - If expired or invalid → the framework automatically returns 401 Unauthorized.
             */

            // userId is a custom claim, so it works fine
            var userId = User.FindFirst("userId")?.Value;

            // username is stored in "sub" (subject).
            // Depending on claim mapping, try "sub" or NameIdentifier
            var username = User.FindFirst("sub")?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(new { UserId = userId, Username = username });
        }




        #region -- Login --
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser([FromBody] RequestLoginDTO dto)
        {
            var response = await _authServices.Login(dto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        #endregion

        #region -- Forget Password --
        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDTO request)
        {
            try
            {
                var user = await _userServices.GetUserByEmail(request.Email);
                if (user == null) return BadRequest(user);

                // Generate token (e.g., a GUID or JWT with expiration)
                var token = Guid.NewGuid().ToString();

                //Save Token and expiration to the database link to user
                var saveResetLink = await _authServices.SaveResetPasswordLink(user.Id, token, DateTime.UtcNow.AddHours(1));
                if (saveResetLink == null)
                    return BadRequest(saveResetLink);

                //send Email with the reset including token
                await _emailServices.SendPasswordResetEmail(user.Email, token);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }


        }
        #endregion

        #region -- Reset Password --
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO request)
        {
            var resetLink = await _authServices.ResetPasswordByToken(request.Token, request.NewPassword);
            if (!resetLink) return BadRequest("Invalid or expired token.");

            return Ok();
        }
        #endregion
    }


}
