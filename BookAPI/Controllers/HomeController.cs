using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookAPI.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BookAPIContext _context;

        public HomeController(IConfiguration configuration, BookAPIContext dbContext)
        {
            _configuration = configuration;
            _context = dbContext;
        }

        // This method handles user authentication and generates JWT token
        [HttpPost("LogIn")]
        public async Task<IActionResult> Login(UserViewModel user)
        {
            IActionResult result = Unauthorized();
            var listOfUsers = _context.Users.SingleOrDefault(m => m.emailOrUsername == user.emailOrUsername);

            if (listOfUsers == null)
            {
                ModelState.AddModelError("", Message.NO_USER);
                result = Unauthorized();
            }
            else
            {
                bool isPasswordValid = BC.Verify(user.password, listOfUsers.password);
                if (isPasswordValid)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.emailOrUsername) };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    // Generate the token
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(1),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                    result = Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }
            return result;

        }

        // POST: api/Home

        [HttpPost("SignUp")]
        public async Task<ActionResult<UserModel>> SignUp(UserViewModel user)
        {
            var newUser = new UserModel()
            {
                userId = Guid.NewGuid(),
                emailOrUsername = user.emailOrUsername,
                password = BC.HashPassword(user.password),
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
