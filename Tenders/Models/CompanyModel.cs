using System.ComponentModel.DataAnnotations;

namespace Tenders.Models;

public class CompanyModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = null!;
}