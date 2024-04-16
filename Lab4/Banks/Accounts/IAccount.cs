using Banks.Banks;
using Banks.Client;
using Banks.Transactions;

namespace Banks.Accounts;

public interface IAccount
{
    public int Id { get; }
    public decimal Money { get; }
    public ClientAccount ClientAccount { get; }
    TransactionConditions AddMoney(decimal amount);
    TransactionConditions WithdrawMoney(decimal amount);
    TransactionConditions RollBack(TransactionConditions conditions, RollBackType rollBackType);
    bool CanWithdrawMoney(decimal amount);
    public void AccumulateInterestRate();
    public void ChangeConditions();
}