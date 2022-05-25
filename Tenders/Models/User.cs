using System.Text.Json.Serialization;

namespace Tenders.Models;


public class UserInfo : Base
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}



public class User : UserInfo
{
    public User(){}
    public User(string firstName, string lastName, string email, 
        string passwordHash)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }
    public User(string firstName, string lastName, string email,
        string passwordHash, IEnumerable<Company> companies)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Companies = new List<Company>(companies);
        PasswordHash = passwordHash;
    }
    [JsonIgnore] public string PasswordHash { get; set; } = null!;
    [JsonIgnore] public List<Company> Companies { get; set; } = new();
}

public class UserDTO : Base
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<ShortCompanyInfo>? Companies { get; set; }
    public UserDTO(User user, bool includeCompaniesList=true)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        if (includeCompaniesList)
            Companies = user.Companies.Select(c => new ShortCompanyInfo(c.Id, c.Name)).ToList();
    }
}

