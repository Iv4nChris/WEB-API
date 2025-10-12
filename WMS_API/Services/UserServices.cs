using Azure.Core;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using WEB_API.DTOs;
using WEB_API.DTOs.Login;
using WEB_API.Models;
using WMS_API.Data;
using WMS_API.Models;

namespace WEB_API.Services
{
    public class UserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Accounts> _passwordHasher;
        public UserServices(ApplicationDbContext context, IPasswordHasher<Accounts> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        #region -- add role --
        public async Task<RoleDTO> AddRole(RoleDTO roleDto)
        {
            var role = new Roles
            {
                Name = roleDto.RoleName
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            roleDto.Id = role.Id; // Assign the generated Id back to the DTO
            return roleDto;
        }
        #endregion

        #region -- Get All Users --
        public async Task<List<UserDTO>> GetAllUsers()
        {
            return await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    RoleId = u.RoleId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Address = u.Address
                })
                .ToListAsync();
        }
        #endregion

        #region -- Add Users / Registration --

        public async Task<AddUserDTO> AddUser(AddUserDTO userDto)
        {
            var isExist = await isEmailAndUserNameExist(userDto.UserName, userDto.Email);
            if (isExist) return null;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new Users
                {
                    RoleId = userDto.RoleId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Address = userDto.Address,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var account = new Accounts
                {
                    UsersId = user.Id,
                    UserName = userDto.UserName,
                };

                account.PasswordHash = _passwordHasher.HashPassword(account, userDto.Password);

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return userDto; 
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        #endregion

        #region -- Check email and username --
        public async Task<bool> isEmailAndUserNameExist(string username, string email)
        {
            return await _context.Users.GroupJoin(_context.Accounts,
                            u => u.Id,
                            ua => ua.UsersId,
                            (u, ua) => new { 
                                username = ua.Select(x => x.UserName),
                                email = u.Email }
                            )
                .AnyAsync(x => x.username.Any(a => a == username) || x.email == email);
        }

        #endregion

        #region -- get user by Id -- 
        public async Task<UserDTO> GetUser(int id)
        {
            return await _context.Users.Select(x => new UserDTO
            {
                Id = x.Id,
                RoleId = x.RoleId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Address = x.Address
            }).FirstAsync(x => x.Id == id);

        }
        #endregion

        #region -- get user account --
        public async Task<AccountsDTO> GetAccount(int id)
        {
            return await _context.Accounts.Select(x => new AccountsDTO
            {
                UserId = x.UsersId,
                UserName = x.UserName
            }).FirstAsync(i => i.UserId == id);
        }
        #endregion

        #region -- Verify Password --
        public Task<bool> VerifyPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Verify password (during login)
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
            
        }
        #endregion

        #region -- Get Role by Id--
        public async Task<RoleDTO> GetRoleById(int id)
        {
            return await _context.Roles.Select(x => new RoleDTO
            {
                Id = x.Id,
                RoleName = x.Name
            }).FirstAsync(role  => role.Id == id);
        }
        #endregion

        #region -- Get Roles--
        public async Task<List<RoleDTO>> GetRoles(int id)
        {
            return await _context.Roles.Select(x => new RoleDTO
            {
                Id = x.Id,
                RoleName = x.Name
            }).ToListAsync();
        }
        #endregion

        #region -- Get User by Email --
        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user =  await _context.Users.Select(x => new UserDTO
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Address,
            }).FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }
        #endregion

        
    }
}
