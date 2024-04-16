namespace DataAccessLayer.Models.Implementations;

public class Leader : AbstractEmployee
{
    public const int MaxAccessLevel = 0;

    public Leader(Guid id, string name, string login, string password) : base()
    {
        Id = id;
        Name = name;
        AccessLevel = MaxAccessLevel;
        Login = login;
        Password = password;
        Employees = new List<OrdinaryEmployee>();
    }

    public Leader() : base() { }

    public virtual ICollection<OrdinaryEmployee> Employees { get; set; }
}