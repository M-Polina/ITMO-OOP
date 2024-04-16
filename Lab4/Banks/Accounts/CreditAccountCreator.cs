using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;

namespace Banks.Accounts;

public class CreditAccountCreator : AccountCreator
{
    private const decimal MinAmountOfMoney = 0;
    private const int DefaultMonths = 12;

    public CreditAccountCreator(Bank bank)
    {
        if (bank is null)
            throw new BanksException("Bank is null while creating CreditAccountCreator");

        Bank = bank;
    }

    public override Bank Bank { get; }

    public override IAccount CreateAccount(int id, ClientAccount client, decimal amountOfMoney = MinAmountOfMoney, int months = DefaultMonths)
    {
        return new CreditAccount(id, client);
    }
}