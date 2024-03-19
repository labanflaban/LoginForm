using Chatpoc.Data;
using Chatpoc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Google.Protobuf.Reflection;
using Microsoft.IdentityModel.Tokens;


namespace Chatpoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private const string SecretKey = "debugging_secret_key_for_a_chatpoc_that_I_made"; 
        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Authenicate(User user)
        {
            if (ModelState.IsValid)
            {
                //Hash the password before validating
                string hashedPassword = PasswordHasher.HashPassword(user.Password_Hash);

                // Check the hashed password against the database
                var isValid = _context.User.Any(u => u.Email == user.Email && u.Password_Hash == hashedPassword);

                if (isValid)
                {
                    //If user is valid, generate a JWT token
                    var token = GenerateToken(user.Email);
                    return new JsonResult(new { success = true, token});

                }

                else
                {
                    return new JsonResult(new { success = false });
                }
            }
            else
            {
                return new JsonResult(new { success = false, error = "Invalid model state" });
            }
        }

        //Method for generating a JWT token
        private string GenerateToken(string email)
        {
            //Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
                //Can add more claims here if needed
            };
            
            //Create symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            //Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), //Token expiry time
                SigningCredentials = creds
            };

            //Create JWT token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //Create JWT token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Serialize JWT token to string
            return tokenHandler.WriteToken(token);
        }
    }
}
