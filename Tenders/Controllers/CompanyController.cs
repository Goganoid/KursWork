using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenders.Data;
using Tenders.Models;

namespace Tenders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : Controller
{
    private ApplicationContext _context;
    public CompanyController(ApplicationContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpPost]
    [Route("add/")]
    public async Task<IActionResult> CreateCompany(CompanyModel model)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var company = user!.Companies.FirstOrDefault(c => c.Name == model.Name);
        if (company != null) return Conflict(new TextMessage("Компанія з таким ім'ям вже існує"));
        var newCompany = new Company(model.Name, model.Location);
        user.Companies.Add(newCompany);
        await _context.SaveChangesAsync();
        return Ok(new IdMessage(newCompany.Id));
    }
    [Authorize]
    [HttpPut]
    [Route("edit/{id}")]
    public async Task<IActionResult> EditCompany(CompanyModel model,int id)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var company = _context.Companies.FirstOrDefault(c => c.Id == id && c.UserOwnerId==user!.Id);
        
        if (company == null) return NotFound(new TextMessage("Така компанія не знайдена"));
        if(await _context.Companies.FirstOrDefaultAsync(c=>c.Name==model.Name && c.Id!=id)!=null) return Conflict(new TextMessage("Компанія з таким ім'ям вже існує"));

        company.Name = model.Name;
        company.Location = model.Location;
        await _context.SaveChangesAsync();
        return Ok(new IdMessage(company.Id));
    }
    [Authorize]
    [HttpDelete]
    [Route("remove/{companyId:int}")]
    public async Task<IActionResult> RemoveCompany(int companyId)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var company = await _context.Companies
            .Include(c => c.Owner)
            .Include(c => c.Tenders)
            .Include(c=>c.WonTenders)
            .FirstOrDefaultAsync(c => c.Id == companyId);
        if (company == null || user == null) return NotFound();
        if (company.WonTenders.Count != 0) return BadRequest(new TextMessage("Ви не можете видалити цю компанію поки вона виконує замовлення на тендер"));
        if (company.Owner.Id == user.Id)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return Ok(new { });
        }

        return Unauthorized(new TextMessage("You don't have access to that company!"));
    }

    [HttpGet]
    [Route("info/{companyId:int}")]
    public async Task<IActionResult> GetCompanyInfo(int companyId)
    {
        var company = await _context.Companies
            .Include(c => c.Owner)
            .FirstOrDefaultAsync(c => c.Id == companyId);
        if (company != null) return Ok(new CompanyDTO(company));
        return NotFound(new TextMessage("Company not found"));
    }

    [HttpGet]
    [Route("tenders-info/{companyId:int}")]
    public async Task<IActionResult> GetCompanyTenders(int companyId)
    {
        var company = await _context.Companies
            .Include(c => c.Tenders)
                .ThenInclude(t=>t.Propositions)
            .Include(c => c.Propositions)
                .ThenInclude(p => p.Tender)
                    .ThenInclude(t=>t.CompanyOrganizer)
            .Include(c => c.WonTenders)
            .FirstOrDefaultAsync(c => c.Id == companyId);
        if (company == null) return NotFound(new TextMessage("Company not found"));
        {
            var tenders = company.Tenders
                .OrderByDescending(t => t.PubDate)
                .Select(t=>new TenderDTO(t,true,false,true));
            var tendersWithParticipation = company.Propositions
                .Select(p => p.Tender)
                .OrderByDescending(t => t.PubDate)
                .Select(t=>new TenderDTO(t));
            var wonTendersId = company.WonTenders.Select(t => t.Id).ToList();
            return Ok(new
            {
                tenders, tendersWithParticipation, wonTendersId
            });
        }


    }
}