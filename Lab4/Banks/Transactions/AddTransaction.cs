using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions;

public class AddTransaction : ISimpleTransaction
{
    private const decimal InitialComission = 0;
    private const int MinId = 0;

    public AddTransaction(decimal amount, IAccount account)
    {
        if (account is null)
            throw new BanksException("Null account while creating transaction.");

        Id = Guid.NewGuid();
        Account = account;
        TransactionConditions = new TransactionConditions(amount, InitialComission);
    }

    public bool TransactionIsDone { get; private set; } = false;
    public IAccount Account { get; }
    public Guid Id { get; }
    public TransactionConditions TransactionConditions { get; private set; }

    public void MarkTransactionMade()
    {
        TransactionIsDone = true;
    }

    public void CreateTransaction()
    {
        TransactionConditions = Account.AddMoney(TransactionConditions.AmountOfMoney);
        TransactionIsDone = true;
    }
}