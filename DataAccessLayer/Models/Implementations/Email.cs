namespace DataAccessLayer.Models.Implementations;

public class Email : AbstractMessenger
{
    public Email(Guid id, Account account, string name) : base()
    {
        Id = id;
        Account = account;
        Name = name;
    }

    public Email() : base() { }
    public string Name { get; set; }
}