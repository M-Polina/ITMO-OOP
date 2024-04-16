namespace DataAccessLayer.Models.Implementations;

public class OrdinaryEmployee : AbstractEmployee
{
    public OrdinaryEmployee(Guid id, string name, int accessLevel, string login, string password, Leader leader) :
        base()
    {
        Id = id;
        Name = name;
        AccessLevel = accessLevel;
        Login = login;
        Password = password;
        Leader = leader;
        Tasks = new List<EmployeeTask>();
    }

    public OrdinaryEmployee() : base() { }

    public virtual Leader Leader { get; set; }
    public virtual ICollection<EmployeeTask> Tasks { get; set; }
}