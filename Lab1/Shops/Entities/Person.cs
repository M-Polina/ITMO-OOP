using Isu.Exceptions;
using Shops.Models;

namespace Shops.Entitis;

public class Person
{
    private Wallet _wallet;

    public Person(string name, uint id, int initialAmountOfMoney)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new PersonCanNotBeCreatedException("Name is incorrect, so person can't be created.");
        }

        Name = name;
        Id = id;
        _wallet = new Wallet(initialAmountOfMoney);
    }

    public string Name { get; }
    public uint Id { get; }

    public void WriteOffMoney(int debitedAmountOfMoney)
    {
        _wallet.WriteOffMoney(debitedAmountOfMoney);
    }

    public int GetAmountOfMoney() => _wallet.AmountOfMoney;
}