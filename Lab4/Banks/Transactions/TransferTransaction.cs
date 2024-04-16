using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class TransferTransaction : IMoneyTransaction
{
    private const int MinId = 0;
    private const int MinCommission = 0;

    public TransferTransaction(decimal amount, IAccount accountFrom, IAccount accountTo)
    {
        if (accountFrom is null)
            throw new BanksException("Null accountFrom while creating transaction.");

        if (accountTo is null)
            throw new BanksException("Null accountTo while creating transaction.");

        Id = Guid.NewGuid();
        AccountFrom = accountFrom;
        AccountTo = accountTo;
        TransactionConditions = new TransactionConditions(amount, MinCommission);
    }

    public bool TransactionIsDone { get; protected set; } = false;
    public IAccount AccountFrom { get; }
    public IAccount AccountTo { get; }
    public Guid Id { get; }
    public TransactionConditions TransactionConditions { get; private set; }

    public void MarkTransactionMade()
    {
        TransactionIsDone = true;
    }

    public void CreateTransaction()
    {
        if (!AccountFrom.CanWithdrawMoney(TransactionConditions.AmountOfMoney))
            throw new BanksException("Can't withdraw money from account (Creating transaction).");

        TransactionConditions = AccountFrom.WithdrawMoney(TransactionConditions.AmountOfMoney);
        AccountTo.AddMoney(TransactionConditions.AmountOfMoney);
        TransactionIsDone = true;
    }
}