namespace DataAccessLayer.Models.Implementations;

public class MobilePhone : AbstractMessenger
{
    public MobilePhone(Guid id, Account account, int phoneNumber) : base()
    {
        Id = id;
        Account = account;
        PhoneNumber = phoneNumber;
    }

    public MobilePhone() : base() { }
    public int PhoneNumber { get; set; }
}