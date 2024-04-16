using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;

namespace Banks.Accounts;

public class DebitAccountCreator : AccountCreator
{
    private const decimal MinAmountOfMoney = 0;
    private const int DefaultMonths = 12;

    public DebitAccountCreator(Bank bank)
    {
        if (bank is null)
            throw new BanksException("bank is null while creating DebitAccountCreator");

        Bank = bank;
    }

    public override Bank Bank { get; }

    public override IAccount CreateAccount(int id, ClientAccount client, decimal amountOfMoney = MinAmountOfMoney, int months = DefaultMonths)
    {
        return new DebitAccount(id, client);
    }
}