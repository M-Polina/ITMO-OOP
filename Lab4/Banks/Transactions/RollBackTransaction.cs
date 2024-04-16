using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class RollBackTransaction : IMoneyTransaction
{
    private const int MinId = 0;

    public RollBackTransaction(IMoneyTransaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Null transaction while creating RollbackTransaction");

        Id = Guid.NewGuid();
        Transaction = transaction;
        TransactionConditions = transaction.TransactionConditions;
    }

    public bool TransactionIsDone { get; private set; } = false;
    public TransactionConditions TransactionConditions { get; }
    public IMoneyTransaction Transaction { get; }
    public Guid Id { get; }

    public void MarkTransactionMade()
    {
        TransactionIsDone = true;
    }

    public void CreateSimpleTransaction()
    {
        if (Transaction is WithdrawTransaction)
            ((WithdrawTransaction)Transaction).Account.RollBack(Transaction.TransactionConditions, RollBackType.WithdrawRollback);
        else if (Transaction is AddTransaction)
            ((AddTransaction)Transaction).Account.RollBack(Transaction.TransactionConditions, RollBackType.AddRollback);
        else
            throw new BanksException("Wrong transaction type in CreateSimpleTransaction Rollback.");

        TransactionIsDone = true;
    }

    public void CreateTransaction()
    {
        if (Transaction is ISimpleTransaction)
        {
            CreateSimpleTransaction();
        }
        else
        {
            ((TransferTransaction)Transaction).AccountFrom.RollBack(Transaction.TransactionConditions, RollBackType.WithdrawRollback);
            ((TransferTransaction)Transaction).AccountTo.RollBack(Transaction.TransactionConditions, RollBackType.AddRollback);
        }
    }
}