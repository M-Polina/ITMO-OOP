using Banks.Banks;
using Banks.Exceptions;

namespace Banks.Client;

public class ClientBuilder : IBuilder
{
    public ClientBuilder(Bank bank, string name, string surname)
    {
        if (bank is null)
            throw new BanksException("Bank is null while creating ClientBuilder.");

        Bank = bank;
        Name = new FullName(name, surname);
    }

    public FullName Name { get; }
    public Bank Bank { get; }
    public Address? Address { get; private set; }
    public Passport? Passport { get; private set; }

    public void SetAddress(string address)
    {
        Address = new Address(address);
    }

    public void SetPassport(int id)
    {
        Passport = new Passport(id);
    }

    public ClientAccount CreateAndGetClient()
    {
        ClientAccount clientAccount = new ClientAccount(Bank, Name, Address, Passport);
        return clientAccount;
    }
}