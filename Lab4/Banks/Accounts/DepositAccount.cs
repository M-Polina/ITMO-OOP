using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;
using Banks.Time;
using Banks.Transactions;

namespace Banks.Accounts;

public class DepositAccount : IAccount
{
    private const decimal MinCommission = 0;
    private const int CoefToCountCommission = 1;
    private const int CoefToNotCountCommission = 0;
    private const int MinId = 0;
    private const int MinMonths = 0;
    private const decimal MinMoneyLimit = 0;

    private decimal _initialAmountOfMoney = MinMoneyLimit;
    private decimal _interestRateStorage = MinMoneyLimit;
    private DepositInterestRate _depositInterestRate;

    public DepositAccount(int id, ClientAccount client, decimal amountOfMoney, int months)
    {
        if (id < MinId)
            throw new BanksException("Id is <0 while creating CreditAccount");
        if (client is null)
            throw new BanksException("Client is null while creating DepositAccount.");
        if (amountOfMoney < MinMoneyLimit)
            throw new BanksException("amountOfMoney to deposit is incorrect.");
        if (months <= MinMonths)
            throw new BanksException("Amount of months to create deposit is incorrect.");

        Id = id;
        _initialAmountOfMoney = amountOfMoney;
        Money = amountOfMoney;
        Period = months;
        ClientAccount = client;
        ExpirationDate = CreationTime.AddMonths(months);
        CreationTime = OwnTime.GetInstance().TimeNow;
        _depositInterestRate = new DepositInterestRate(client.Bank.Conditions.DepositInterestRateConditions, amountOfMoney);
    }

    public decimal InitialAmountOfMoney => _initialAmountOfMoney;
    public decimal InterestRate => _depositInterestRate.Value;
    public decimal InterestRatePerMonth => _depositInterestRate.ValuePerMonth;
    public DepositInterestRateConditions InterestRateConditions => _depositInterestRate.Conditions;
    public int Id { get; }
    public ClientAccount ClientAccount { get; }
    public DateTime CreationTime { get; }
    public int Period { get; }
    public decimal Money { get; private set; }
    public DateTime ExpirationDate { get; }

    public bool CanWithdrawMoney(decimal amount)
    {
        var time = OwnTime.GetInstance().TimeNow;

        if (ExpirationDate > time)
            return false;
        if (amount < MinMoneyLimit || (Money - amount) < MinMoneyLimit)
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

        return new TransactionConditions(amount, 0);
    }

    public TransactionConditions WithdrawMoney(decimal amount)
    {
        if (!CanWithdrawMoney(amount))
            throw new BanksException("Money can't be withdrawed from DepositAccount.");

        Money -= amount;

        return new TransactionConditions(amount, MinCommission);
    }

    public TransactionConditions RollBack(TransactionConditions conditions, RollBackType rollBackType)
    {
        if (conditions is null)
            throw new BanksException("Transaction can't be rollbacked in Deposit Account, null conditions.");

        int coefficient = rollBackType == RollBackType.WithdrawRollback ? CoefToCountCommission : CoefToNotCountCommission;

        Money = Money + (((int)rollBackType) * conditions.AmountOfMoney) + (conditions.Commission * coefficient);
        return conditions;
    }

    public void ChangeInterestRate(DepositInterestRateConditions conditionsList)
    {
        if (conditionsList is null)
            throw new BanksException("InterestRateConditions is null so interestRate in Deposit can't be changed.");

        _depositInterestRate.ChangeInterestRate(conditionsList, _initialAmountOfMoney);
    }

    public void AccumulateInterestRate()
    {
        var time = OwnTime.GetInstance().TimeNow;
        if (ExpirationDate <= time)
            _interestRateStorage += Money * InterestRatePerMonth;

        if (OwnTime.GetInstance().MonthPassed(CreationTime))
        {
            PayInterestRate();
        }
    }

    public void ChangeConditions()
    {
        if (ClientAccount.Bank.Conditions is null)
            throw new BanksException("Null conditions while chnging them in CreditAccount.");

        ChangeInterestRate(ClientAccount.Bank.Conditions.DepositInterestRateConditions);
    }

    private void PayInterestRate()
    {
        var time = OwnTime.GetInstance().TimeNow;

        if (ExpirationDate <= time)
        {
            Money += _interestRateStorage;
            _interestRateStorage = MinMoneyLimit;
        }
    }
}