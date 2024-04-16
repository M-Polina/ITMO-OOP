using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class WithdrawTransaction : ISimpleTransaction
{
    private const int MinId = 0;
    private const int _subscribersList = 0;

    public WithdrawTransaction(decimal amount, IAccount account)
    {
        if (account is null)
            throw new BanksException("Null account while creating transaction.");

        Id = Guid.NewGuid();
        Account = account;
        TransactionConditions = new TransactionConditions(amount, _subscribersList);
    }

    public bool TransactionIsDone { get; protected set; } = false;
    public IAccount Account { get; }
    public Guid Id { get; }
    public TransactionConditions TransactionConditions { get; private set; }

    public void MarkTransactionMade()
    {
        TransactionIsDone = true;
    }

    public void CreateTransaction()
    {
        if (!Account.CanWithdrawMoney(TransactionConditions.AmountOfMoney))
            throw new BanksException("Wrong ammount while creating withdrawTransaction.");

        TransactionConditions = Account.WithdrawMoney(TransactionConditions.AmountOfMoney);
        TransactionIsDone = true;
    }
}