using Banks.Banks;
using Banks.Exceptions;

namespace Banks.Accounts;

public class DepositInterestRate
{
    private const int MonthsInYear = 12;
    private const decimal MinMoneyLimit = 0;
    private const decimal MinInterestRate = 0;
    private const int MinElementsInListNumber = 0;

    private DepositInterestRateConditions _conditions;

    public DepositInterestRate(DepositInterestRateConditions conditions, decimal amountOfMoney)
    {
        if (conditions is null)
            throw new BanksException("ConditionsList is null while creating DepositAccount.");
        if (amountOfMoney < MinMoneyLimit)
            throw new BanksException("amountOfMoney to deposit is incorrect.");

        _conditions = conditions;
        Value = CountInterestRate(conditions, amountOfMoney);
    }

    public DepositInterestRateConditions Conditions => _conditions;
    public decimal Value { get; private set; }
    public decimal ValuePerMonth => Value / MonthsInYear;

    public decimal ChangeInterestRate(DepositInterestRateConditions conditions, decimal amountOfMoney)
    {
        if (conditions is null)
            throw new BanksException("ConditionsList is null while creating DepositAccount.");
        if (amountOfMoney < MinMoneyLimit)
            throw new BanksException("amountOfMoney to deposit is incorrect.");

        _conditions = conditions;
        Value = CountInterestRate(conditions, amountOfMoney);

        return Value;
    }

    private decimal CountInterestRate(DepositInterestRateConditions conditionsList, decimal amountOfMoney)
    {
        for (int i = conditionsList.InterestRatesList.Count - 1; i >= MinElementsInListNumber; i--)
        {
            if (amountOfMoney >= conditionsList.MoneyLimitsList.ElementAt(i))
            {
                return conditionsList.InterestRatesList.ElementAt(i);
            }
        }

        throw new BanksException("Bad conditions while counting Deposit Interest Rate.");
    }
}