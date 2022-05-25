using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tenders.Models;

public class Tender : Base
{
    public DateTime PubDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public string Title { get; set; } = null!;


    public int? CompanyExecutorId { get; set; }

    [ForeignKey("CompanyExecutorId")]
    [JsonIgnore]
    public Company? CompanyExecutor { get; set; }

    [JsonIgnore] public List<Proposition> Propositions { get; set; } = new();
    public uint Cost { get; set; }
    public int CompanyOrganizerId { get; set; }
    [ForeignKey("CompanyOrganizerId")] public Company CompanyOrganizer { get; set; } = null!;
}

public class TenderDTO : Base
{
    public DateTime PubDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public string Title { get; set; }
    
    public int? CompanyExecutorId { get; set; }

    public CompanyDTO? CompanyExecutor { get; set; }

    public List<PropositionDTO>? Propositions { get; set; } = new();
    public int PropositionsCount { get; set; }
    public uint Cost { get; set; }
    public int CompanyOrganizerId { get; set; }
    public CompanyDTO? CompanyOrganizer { get; set; }


    public TenderDTO(Tender tender,bool includeCompanyOrganizer=true, bool includePropositions=true, bool countPropositions=false)
    {
        Id = tender.Id;
        PubDate = tender.PubDate;
        EndDate = tender.EndDate;
        IsActive = tender.IsActive;
        Title = tender.Title;
        CompanyOrganizerId = tender.CompanyOrganizerId;
        CompanyExecutorId = tender.CompanyExecutorId;
        Cost = tender.Cost;

        CompanyExecutor = tender.CompanyExecutor==null ? null : new CompanyDTO(tender.CompanyExecutor,includeOwner:false);
        if (countPropositions) PropositionsCount = tender.Propositions.Count;
        if(includeCompanyOrganizer) CompanyOrganizer = new CompanyDTO(tender.CompanyOrganizer,false);
        if(includePropositions) Propositions = tender.Propositions.Select(p => new PropositionDTO(p)).ToList();
    }
}
