namespace Banks.Transactions;

public interface IMoneyTransaction
{
    Guid Id { get; }
    public TransactionConditions TransactionConditions { get; }
    void CreateTransaction();
    void MarkTransactionMade();
}