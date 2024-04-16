using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;
using Banks.Time;
using Banks.Transactions;

namespace Banks.Accounts;

public class CreditAccount : IAccount
{
    private const int CoefToCountCommission = 1;
    private const int CoefToNotCountCommission = 0;
    private const decimal MaxCreditLimit = 0;
    private const decimal MinMoneyLimit = 0;
    private const int MinId = 0;
    private const decimal MinAmountOfOwnMoney = 0;
    private const decimal MinCommission = 0;

    public CreditAccount(int id, ClientAccount client)
    {
        if (client is null)
            throw new BanksException("Client is null while creating CreditAccount.");
        if (id < MinId)

            Money = MinMoneyLimit;
        Id = id;
        ClientAccount = client;
        Commission = client.Bank.Conditions.Comission;
        CreditLimit = client.Bank.Conditions.CreditLimit;
        CreationTime = OwnTime.GetInstance().TimeNow;
    }

    public DateTime CreationTime { get; }
    public int Id { get; }
    public ClientAccount ClientAccount { get; }
    public decimal CreditLimit { get; private set; }
    public decimal Commission { get; private set; }
    public decimal Money { get; private set; }

    public bool CanWithdrawMoney(decimal amount)
    {
        if (amount < CreditLimit || (Money - amount) < CreditLimit)
            return false;
        if (amount > ClientAccount.Bank.Conditions.SuspiciousClientMoneyLimit && !ClientAccount.IsNotSuspicious())
            return false;

        return true;
    }

    public TransactionConditions AddMoney(decimal amount)
    {
        if (amount <= MinMoneyLimit)
            throw new BanksException("Amount of money to add can't be <0 in DepositAccount.");

        Money += amount;

        return new TransactionConditions(amount, MinMoneyLimit);
    }

    public TransactionConditions WithdrawMoney(decimal amount)
    {
        decimal withrawMoney = 0;
        decimal commission = 0;

        if (!CanWithdrawMoney(amount))
            throw new BanksException("Money can't be withdrawed from Credit Account, wrong ammount.");

        if ((Money - amount) >= MinAmountOfOwnMoney)
        {
            withrawMoney = amount;
            Money -= amount;
        }
        else if ((Money - amount) >= CreditLimit)
        {
            withrawMoney = amount;
            commission = Commission;
            Money = Money - amount - Commission;
        }
        else
        {
            throw new BanksException("Amount of money to withdraw is too big in Credit Account.");
        }

        return new TransactionConditions(withrawMoney, commission);
    }

    public TransactionConditions RollBack(TransactionConditions conditions, RollBackType rollBackType)
    {
        if (conditions is null)
            throw new BanksException("Transaction can't be rollbacked in Credit Account, null conditions.");

        int coefficient = rollBackType == RollBackType.WithdrawRollback
            ? CoefToCountCommission
            : CoefToNotCountCommission;

        Money = Money + (((int)rollBackType) * conditions.AmountOfMoney) +
                        (conditions.Commission * coefficient);

        return conditions;
    }

    public void ChangeCommission(decimal newCommission)
    {
        if (newCommission <= MinCommission)
            throw new BanksException("newCommission is incorrect so it can't be changed.");

        Commission = newCommission;
    }

    public void AccumulateInterestRate() { }

    public void ChangeCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit >= MaxCreditLimit)
            throw new BanksException("newCreditLimit is incorrect so it can't be changed.");

        CreditLimit = newCreditLimit;
    }

    public void ChangeConditions()
    {
        if (ClientAccount.Bank.Conditions is null)
            throw new BanksException("Null conditions while chnging them in CreditAccount.");

        ChangeCreditLimit(ClientAccount.Bank.Conditions.CreditLimit);
        ChangeCommission(ClientAccount.Bank.Conditions.Comission);
    }
}