using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenders.Data;
using Tenders.Models;

namespace Tenders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private ApplicationContext _context;

    public UserController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("account-info/{id:int}")]
    public async Task<IActionResult> GetInfo(int id)
    {
        var user = await _context.Users.Include(u => u.Companies).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound(new TextMessage("User not found"));
        return Ok(new UserDTO(user));

    }

    [HttpGet]
    [Route("companies-list/{id:int}")]
    public async Task<IActionResult> GetUserCompanies(int id)
    {
        var user = await _context.Users.Include(u => u.Companies).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound(new TextMessage("user not found"));
        var userCompanies = user.Companies.Select(c => new ShortCompanyInfo(c.Id,c.Name));
        return Ok(userCompanies);
    }

    [Authorize]
    [HttpPut]
    [Route("edit/{id}")]
    public async Task<IActionResult> EditUser(UserModel model,int id)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        if (user!.Id != id) return Unauthorized(new TextMessage("You can't do this"));
        var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if(email!=null && model.Email!=user.Email) return Unauthorized(new TextMessage("You can't do this"));
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        await _context.SaveChangesAsync();
        return Ok(new {});
    }
   
    public static int? GetUserId(ClaimsIdentity? identity)
    {
        if (int.TryParse(GetClaim(identity, "userId"), out var id))
            return id;
        return null;
    }

    public static async Task<User?> GetUser(ApplicationContext context, ClaimsIdentity? identity)
    {
        var userId = GetUserId(identity);
        return await context.Users.FindAsync(userId);
    }
    
    private static string? GetClaim(ClaimsIdentity? identity, string claimType)
    {
        var userClaims = identity?.Claims;
        return userClaims?.FirstOrDefault(o => o.Type == claimType)?.Value;
    }
}