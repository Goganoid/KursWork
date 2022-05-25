using System.ComponentModel.DataAnnotations.Schema;

namespace Tenders.Models;

public class Company : Base
{
    public Company(string name, string location)
    {
        Name = name;
        Location = location;
    }

    public int UserOwnerId { get; set; }
    [ForeignKey("UserOwnerId")] public User Owner { get; set; } = null!;
    public string Name { get; set; }
    public string Location { get; set; }
    public List<Tender> Tenders { get; set; } = new();
    public List<Proposition> Propositions { get; set; } = new();
    public List<Tender> WonTenders { get; set; } = new();
}


public class CompanyDTO : Base
{
    public int UserOwnerId { get; set; }
    public UserDTO Owner { get; set; } = null!;
    public string Name { get; set; }
    public string Location { get; set; }

    public CompanyDTO(Company company, bool includeOwner=true)
    {
        Id = company.Id;
        UserOwnerId = company.UserOwnerId;
        if(includeOwner) Owner = new UserDTO(company.Owner,false);
        Name = company.Name;
        Location = company.Location;
    }
}

public record ShortCompanyInfo(int Id,string Name);
