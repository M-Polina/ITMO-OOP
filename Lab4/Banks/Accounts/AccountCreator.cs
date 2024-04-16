using Banks.Banks;
using Banks.Client;

namespace Banks.Accounts;

public abstract class AccountCreator
{
    private const decimal MinAmountOfMoney = 0;
    private const int DefaultMonths = 12;

    public abstract Bank Bank { get; }

    public abstract IAccount CreateAccount(int id, ClientAccount client, decimal amountOfMoney = MinAmountOfMoney, int months = DefaultMonths);
}