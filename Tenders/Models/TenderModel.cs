using System.ComponentModel.DataAnnotations;

namespace Tenders.Models;

public class TenderModel
{
    public int CompanyId { get; set; }
    public DateTime EndDate { get; set; }
    [MinLength(5,ErrorMessage="Too short title")]
    public string Title { get; set; } = null!;
    [Range(3000,Int32.MaxValue,ErrorMessage = "The field {0} must be greater than {1}")]
    public uint Cost { get; set; }
}