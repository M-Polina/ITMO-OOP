using Banks.Exceptions;

namespace Banks.Transactions;

public class TransactionConditions
{
    public const decimal MinMoneyLimit = 0;

    public TransactionConditions(decimal withrawMoney, decimal commission)
    {
        if (withrawMoney < MinMoneyLimit)
            throw new BanksException("withrawMoney is incorrect while creating TransactionConditions.");
        if (commission < MinMoneyLimit)
            throw new BanksException("Commission is incorrect while creating TransactionConditions.");

        AmountOfMoney = withrawMoney;
        Commission = commission;
    }

    public decimal AmountOfMoney { get; }
    public decimal Commission { get; }
}