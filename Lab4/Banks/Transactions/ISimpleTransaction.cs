using Banks.Accounts;

namespace Banks.Transactions;

public interface ISimpleTransaction : IMoneyTransaction
{
    IAccount Account { get; }
}