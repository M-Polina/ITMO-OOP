using Banks.Accounts;
using Banks.Banks;
using Banks.Exceptions;
using Banks.MessageHandlers;

namespace Banks.Client;

public class ClientAccount : ISubscriber
{
    private List<IAccount> _accountsList = new List<IAccount>();
    private List<IMessageHandler> _handlersList = new List<IMessageHandler>();

    public ClientAccount(Bank bank, FullName name, Address? address, Passport? passport)
    {
        if (name is null)
            throw new BanksException("FullName is null while creating ClientAccount.");

        if (bank is null)
            throw new BanksException("Bank is null while creating ClientBuilder.");

        Bank = bank;
        FullName = name;
        Address = address;
        Passport = passport;
        Id = Guid.NewGuid();
    }

    public IReadOnlyCollection<IAccount> AccountsList => _accountsList;
    public IReadOnlyCollection<IMessageHandler> HandlersList => _handlersList;

    public Guid Id { get; }
    public Bank Bank { get; }
    public FullName FullName { get; }
    public Address? Address { get; private set; }
    public Passport? Passport { get; private set; }

    public IAccount? FindAccount(int accountId) => AccountsList.SingleOrDefault(acc => acc.Id == accountId);

    public IAccount GetAccount(int accountId) => AccountsList.Single(acc => acc.Id == accountId);

    public bool IsNotSuspicious() => !(FullName is null) && !(Address is null) && !(Passport is null);

    public void AddAccount(IAccount account)
    {
        if (account is null)
            throw new BanksException("Account is null while adding it to account");
        if (_accountsList.Contains(account))
            throw new BanksException("Client already has this account.");
        if (account.ClientAccount.Id != Id || account.ClientAccount.Bank.Id != Bank.Id)
            throw new BanksException("Incorrect account while adding it to client.");

        _accountsList.Add(account);
    }

    public void SetAddress(Address address)
    {
        if (address is null)
            throw new BanksException("Address is null while creating ClientAccount.");

        Address = address;
    }

    public void SetPassport(Passport passport)
    {
        if (passport is null)
            throw new BanksException("Passport is null while creating ClientAccount.");

        Passport = passport;
    }

    public void AddHandler(IMessageHandler handler)
    {
        if (handler is null)
            throw new BanksException("Null handler while adding it");
        if (!_handlersList.Contains(handler))
            _handlersList.Add(handler);
    }

    public void Update(IObservable observable)
    {
        ObservableMessage message = new ObservableMessage(observable);
        foreach (var handler in _handlersList)
        {
            handler.SendMessage(message);
        }
    }
}