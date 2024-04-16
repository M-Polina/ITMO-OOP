using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;
using Banks.Time;
using Banks.Transactions;

namespace Banks.Accounts;

public class DebitAccount : IAccount
{
    private const int CoefToCountCommission = 1;
    private const int CoefToNotCountCommission = 0;
    private const int MinId = 0;
    private const int MonthsInYear = 12;
    private const decimal MinMoneyLimit = 0;
    private const decimal MinInterestRate = 0;
    private const decimal MinCommission = 0;

    private decimal _interestRateStorage = MinMoneyLimit;

    public DebitAccount(int id, ClientAccount client)
    {
        if (client is null)
            throw new BanksException("Client is null while creating DebitAccount.");
        if (id < MinId)
            throw new BanksException("Id is <0 while creating CreditAccount");

        Id = id;
        Money = MinMoneyLimit;
        InterestRate = client.Bank.Conditions.DebitInterestRate;
        ClientAccount = client;
        CreationTime = OwnTime.GetInstance().TimeNow;
    }

    public decimal InterestRatePerMonth => InterestRate / MonthsInYear;
    public int Id { get; }
    public ClientAccount ClientAccount { get; }
    public decimal Money { get; private set; }
    public DateTime CreationTime { get; }
    public decimal InterestRate { get; private set; }

    public bool CanWithdrawMoney(decimal amount)
    {
        if (amount < MinMoneyLimit || (Money - amount) < MinMoneyLimit)
            return false;

        if (amount > ClientAccount.Bank.Conditions.SuspiciousClientMoneyLimit && !ClientAccount.IsNotSuspicious())
            return false;

        return true;
    }

    public TransactionConditions AddMoney(decimal amount)
    {
        if (amount <= MinMoneyLimit)
            throw new BanksException("Amount of money to add can't be <0 in Debit Account.");

        Money += amount;

        return new TransactionConditions(amount, MinCommission);
    }

    public TransactionConditions WithdrawMoney(decimal amount)
    {
        if (!CanWithdrawMoney(amount))
            throw new BanksException("Money can't be withdrawed from Debit Account.");

        Money -= amount;

        return new TransactionConditions(amount, MinCommission);
    }

    public TransactionConditions RollBack(TransactionConditions conditions, RollBackType rollBackType)
    {
        if (conditions is null)
            throw new BanksException("Transaction can't be rollbacked in Debit Account, null conditions.");

        int coefficient = rollBackType == RollBackType.WithdrawRollback
            ? CoefToCountCommission
            : CoefToNotCountCommission;
        Money = Money + (((int)rollBackType) * conditions.AmountOfMoney) +
                        (conditions.Commission * coefficient);
        return conditions;
    }

    public void ChangeInterestRate(decimal newInterestRate)
    {
        if (newInterestRate <= MinInterestRate)
            throw new BanksException("newInterestRate is incorrect so interest rate in Debit can't be changed.");

        InterestRate = newInterestRate;
    }

    public void AccumulateInterestRate()
    {
        _interestRateStorage += Money * InterestRatePerMonth;
        Console.WriteLine($"AccumulateInterestRate {Money} {InterestRatePerMonth}");

        if (OwnTime.GetInstance().MonthPassed(CreationTime))
        {
            PayInterestRate();
        }
    }

    public void PayInterestRate()
    {
        Money += _interestRateStorage;
        _interestRateStorage = MinInterestRate;
    }

    public void ChangeConditions()
    {
        Console.WriteLine("ChangeConditions");
        if (ClientAccount.Bank.Conditions is null)
            throw new BanksException("Null conditions while chnging them in CreditAccount.");

        ChangeInterestRate(ClientAccount.Bank.Conditions.DebitInterestRate);
    }
}