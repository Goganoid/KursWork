using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tenders.Data;
using Tenders.Models;

namespace Tenders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticateController : ControllerBase
{
    private IConfiguration _config;
    private ApplicationContext _context;

    public AuthenticateController(IConfiguration config, ApplicationContext context)
    {
        _config = config;
        _context = context;
    }

    /// <remarks>
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///        "email": "testmail1@gmail.com",
    ///        "password": "Passw0rd123"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user != null &&
            user.PasswordHash.Equals(CalculateSha256(model.Password!, _config.GetValue<string>("Auth:salt"))))
        {
            List<Claim> claims = new()
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                _config.GetValue<string>("JWT:ValidIssuer"),
                _config.GetValue<string>("JWT:ValidAudience"),
                claims,
                expires: DateTime.UtcNow.AddDays(20),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:Secret"))),
                    SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                token = tokenString,
                validTo = token.ValidTo,
                userId = user.Id
            });
        }

        return Unauthorized(new TextMessage("Неправильний логін чи пароль"));
    }

    /// <remarks>
    /// Sample request:
    ///
    ///     POST /login
    ///     {
    ///        {
    ///             "firstname":"John",
    ///             "secondname":"Doe",
    ///             "email": "testmail5@gmail.com",
    ///             "password": "Passw0rd123"
    ///         }
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user != null) return Conflict(new TextMessage("Користувач вже існує"));
        User newUser = new(
            model.FirstName,
            model.SecondName,
            model.Email,
            CalculateSha256(model.Password, _config.GetValue<string>("Auth:salt")));
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return await Login(new LoginModel() {Email = model.Email, Password = model.Password});
    }
    
    public static string CalculateSha256(string str, string salt)
    {
        var sha256 = SHA256.Create();
        byte[] hashValue;
        var objUtf8 = new UTF8Encoding();
        hashValue = sha256.ComputeHash(objUtf8.GetBytes(str + salt));

        return Convert.ToBase64String(hashValue);
    }
}