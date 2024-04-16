using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;

namespace Banks.Accounts;

public class DepositAccountCreator : AccountCreator
{
    private const decimal MinAmountOfMoney = 0;
    private const int DefaultMonths = 12;

    public DepositAccountCreator(Bank bank)
    {
        if (bank is null)
            throw new BanksException("bank is null while creating DebpositAccountCreator");

        Bank = bank;
    }

    public override Bank Bank { get; }

    public override IAccount CreateAccount(int id, ClientAccount client, decimal amountOfMoney = MinAmountOfMoney, int months = DefaultMonths)
    {
        return new DepositAccount(id, client, amountOfMoney, months);
    }
}