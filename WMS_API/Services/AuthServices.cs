using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WEB_API.DTOs;
using WEB_API.DTOs.Global;
using WEB_API.DTOs.Login;
using WEB_API.Models;
using WMS_API.Data;

namespace WEB_API.Services
{
    public class AuthServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Accounts> _passwordHasher;
        public AuthServices(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Accounts>();
        }

        #region -- Login Process --
        public async Task<ResponseLoginDTO> Login(RequestLoginDTO loginDto)
        {
            try
            {


                var user = await _context.Accounts.FirstOrDefaultAsync(account => account.UserName == loginDto.Username);
                if (user == null)
                {
                    //User not found
                    return new ResponseLoginDTO
                    {
                        Success = false,
                        Message = "Username not found!"
                    };
                }

                //found
                // Hash password (during registration)
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);

                // Verify password (during login)
                bool isValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, hashedPassword);
                if (!isValid)
                {
                    //Incorrect Password
                    return new ResponseLoginDTO
                    {
                        Success = false,
                        Message = "Invalid Password!"
                    };
                }

                // Generate JWT
                var token = GenerateJwtToken(user);

                return new ResponseLoginDTO
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token
                };

            }
            catch (Exception ex)
            {
                return new ResponseLoginDTO
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        #endregion


        #region -- generate Token JWT --
        private string GenerateJwtToken(Accounts accounts)
        {
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key")
             ?? throw new InvalidOperationException("JWT key not found");

            /*
             Claims are pieces of information embedded inside the token about the user.
             -- JwtRegisteredClaimNames.Sub (subject) stores the user’s username.
             -- "userId" is a custom claim holding the user’s unique ID.
             -- ClaimTypes.Role defines the user’s role as "User" (used for authorization).
             */
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, accounts.UserName),
                new Claim("userId", accounts.UsersId.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };
            /*
             Reads your JWT secret key from configuration.
             -- Converts it into a symmetric security key.
             -- Creates signing credentials using HMAC-SHA256 algorithm — this means the token will be signed with your secret key to ensure it can’t be tampered with.
             */
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /*
             Constructs the JWT token with:
                issuer — who issues the token (your API).
                audience — who the token is meant for.
                claims — the user information inside the token.
                expires — when the token should expire (e.g., 60 minutes from now).
                signingCredentials — how the token is signed to ensure authenticity.
             */
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion


        #region -- Add ResetLink to the Database --
        public async Task<Response> SaveResetPasswordLink(int id, string token, DateTime expire)
        {
            var ResetLink = new ResetPasswordLink
            {
                UserId = id,
                Token = token,
                ExpirationDate = expire,
            };

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(ResetLink);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response
                {
                    Success = true,
                    Message = "Save Reset Link"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                return new Response
                {
                    Success = false,
                    Message = "Failed to reset password"
                };
            }
        }
        #endregion

        #region -- get Reset Password by Token --
        public async Task<bool> ResetPasswordByToken(string token, string password)
        {
            var result =  await _context.ResetPasswordLinks.FirstOrDefaultAsync(
                        u => u.Token == token
                        && u.ExpirationDate > DateTime.UtcNow
                    );
            if (result == null) return false;

            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.UsersId == result.UserId);
            if (account == null) return false;

            account.PasswordHash = _passwordHasher.HashPassword(account, password);
            _context.ResetPasswordLinks.Remove(result);

            await _context.SaveChangesAsync();
            return true;

        }
        #endregion
    }
}
