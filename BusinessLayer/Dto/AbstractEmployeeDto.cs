using BusinessLayer.Constants;

namespace BusinessLayer.Dto;

public record AbstractEmployeeDto
{
    public string Name { get; init; }
    public Guid Id { get; init; }
    public int AccessLevel { get; init; }
    public string Login { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
    
    public AbstractEmployeeDto(Guid id,
        string name,
        int accessLevel,
        string login,
        string password)
    {
        Id = id;
        Name = name;
        AccessLevel = accessLevel;
        Login = login;
        Password = password;
        Role = accessLevel == 0 ? AccountRole.Leader.ToString("G") : AccountRole.OrdinaryEmployee.ToString("G");
    }
}

