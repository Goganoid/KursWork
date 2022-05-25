namespace Tenders.Models;

public class Proposition : Base
{
    public Company Company { get; set; } = null!;
    public Tender Tender { get; set; } = null!;

    public uint Cost { get; set; }
}

public class PropositionDTO : Base
{
    public string CompanyName { get; set; }
    public int CompanyId { get; set; }
    public int UserId { get; set; }
    public uint Cost { get; set; }

    public PropositionDTO(Proposition proposition)
    {
        Id = proposition.Id;
        CompanyId = proposition.Company.Id;
        UserId = proposition.Company.UserOwnerId;
        Cost = proposition.Cost;
        CompanyName = proposition.Company.Name;
    }
}