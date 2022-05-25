using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tenders.Data;
using Tenders.Models;

namespace Tenders.Controllers;

[ApiController]
[Route("api/tender")]
public class TenderController : ControllerBase
{
    private ApplicationContext _context;

    public TenderController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("tenders/{n:int}")]
    public IActionResult GetNTenders(int n, string? searchTerm)
    {
        var tenders = _context.Tenders
            .Where(t => t.EndDate > DateTime.Now && t.IsActive )
            .Include(t => t.CompanyOrganizer)
            .AsEnumerable();
        searchTerm = searchTerm?.ToLower();
        
        var nTenders = searchTerm == null
            ? tenders.Take(n)
            : tenders.Where(t => t.Title.ToLower().Contains(searchTerm));
        
        var tendersInfo = nTenders
            .OrderBy(t => t.EndDate)
            .Select(t => new TenderDTO(t, true, false,false));
        return Ok(tendersInfo);
    }
    [Authorize]
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateTender(TenderModel model)
    {
        var userId = UserController.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
        var user = await _context.Users.Include(u => u.Companies).FirstAsync(u => u.Id == userId);
        var company = user.Companies.FirstOrDefault(c => c.Id == model.CompanyId);
        if (company != null)
        {
            company.Tenders.Add(new Tender
            {
                Cost = model.Cost,
                EndDate = model.EndDate,
                IsActive = true,
                PubDate = DateTime.Now,
                Title = model.Title
            });
            await _context.SaveChangesAsync();
            return Ok(new { });
        }

        return NotFound(new { });
    }
    [Authorize]
    [HttpPut]
    [Route("toggle/{tenderId:int}")]
    public async Task<IActionResult> ToggleStatus(int tenderId)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var tender = await _context.Tenders.Include(t => t.CompanyOrganizer).FirstOrDefaultAsync(t => t.Id == tenderId);

        if (tender == null || user == null) return NotFound();
        if (tender.EndDate < DateTime.Now) return Forbid();
        if (tender.CompanyOrganizer.UserOwnerId != user.Id)
            return Unauthorized(new TextMessage("You don't have access to that tender!"));

        tender.IsActive = !tender.IsActive;
        await _context.SaveChangesAsync();
        return Ok(new {tender.IsActive});
    }
    [Authorize]
    [HttpDelete]
    [Route("remove/{tenderId:int}")]
    public async Task<IActionResult> RemoveTender(int tenderId)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var tender = await _context.Tenders.Include(t => t.CompanyOrganizer).FirstOrDefaultAsync(t => t.Id == tenderId);

        if (tender == null || user == null) return NotFound();

        if (tender.CompanyOrganizer.UserOwnerId == user.Id)
        {
            _context.Tenders.Remove(tender);
            await _context.SaveChangesAsync();
            return Ok(new { });
        }

        return Unauthorized(new TextMessage("You don't have access to that tender!"));
    }
    [Authorize]
    [HttpDelete]
    [Route("unsubscribe/{tenderId:int}")]
    public async Task<IActionResult> Unsubscribe(int tenderId)
    {
        var userId = UserController.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
        var user = await _context.Users
            .Include(u => u.Companies)
            .ThenInclude(c => c.Propositions)
            .ThenInclude(p => p.Tender)
            .FirstAsync(u => u.Id == userId);
        var company = user.Companies
            .FirstOrDefault(c =>
                c.Propositions.Remove(c.Propositions.Find(t => t.Tender.Id == tenderId)!)
            );
        await _context.SaveChangesAsync();
        if (company == null) return NotFound(new TextMessage("Tender not found"));
        return Ok(new { });
    }

    [Authorize]
    [HttpPut]
    [Route("subscribe/{companyId:int}/{tenderId:int}/{cost:int}")]
    public async Task<IActionResult> Subscribe(int companyId, int tenderId, int cost)
    {
        var user = await UserController.GetUser(_context, HttpContext.User.Identity as ClaimsIdentity);
        var tender = await _context.Tenders
            .Include(t => t.CompanyOrganizer)
            .Include(t => t.Propositions)
            .ThenInclude(p=>p.Company)
            .FirstOrDefaultAsync(t => t.Id == tenderId);
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
        
        if (tender == null) return NotFound(new TextMessage("Tender not found"));
        if (user == null) return NotFound(new TextMessage("User not found"));
        if (company == null) return NotFound(new TextMessage("Company not found"));
        if (tender.EndDate < DateTime.Now) return Forbid();
        if (tender.CompanyOrganizer.Id == company.Id) return Forbid();
        
        var proposition = tender.Propositions.FirstOrDefault(p => p.Company.UserOwnerId == user.Id);
        if (proposition != null)
        {
            tender.Propositions.Remove(proposition);
        }
        else
        {
            Proposition newProposition = new()
            {
                Company = company,
                Tender = tender,
                Cost = (uint) cost
            };
            tender.Propositions.Add(newProposition);
        }

        await _context.SaveChangesAsync();
        return Ok(tender.Propositions.Select(p => new PropositionDTO(p)));
    }
    [Authorize]
    [HttpPut]
    [Route("set-executor/{tenderId:int}/{executorId:int}")]
    public async Task<IActionResult> SetExecutor(int tenderId, int executorId)
    {
        var userId = UserController.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
        var user = await _context.Users.Include(u => u.Companies).FirstOrDefaultAsync(u => u.Id == userId);
        var executor = await _context.Companies.FindAsync(executorId);
        var tender = await _context.Tenders
            .Include(t => t.Propositions)
            .ThenInclude(p=>p.Company)
            .FirstOrDefaultAsync(t => t.Id == tenderId);
        if (tender == null || user == null || executor == null) return NotFound();
        if (tender.EndDate < DateTime.Now) return Forbid();
        if (tender.Propositions.Exists(p => p.Company.Id == executorId))
        {
            tender.CompanyExecutor = executor;
            tender.IsActive = false;
            tender.EndDate = DateTime.Now;
        }
        else
        {
            return NotFound();
        }

        await _context.SaveChangesAsync();
        return Ok(new {});
    }
    [HttpGet]
    [Route("{tenderId:int}")]
    public async Task<IActionResult> GetTenderInfo(int tenderId)
    {
        var tender = await _context.Tenders
            .Include(t => t.CompanyOrganizer)
            .ThenInclude(c => c.Owner)
            .Include(t => t.Propositions)
            .ThenInclude(p => p.Company)
            .ThenInclude(c => c.Owner)
            .Include(t => t.CompanyExecutor)
            .FirstOrDefaultAsync(t => t.Id == tenderId);
        if (tender != null)
            return Ok(new TenderDTO(tender));
        
        return NotFound(new { });
    }
}