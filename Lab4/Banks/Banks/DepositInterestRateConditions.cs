using Banks.Exceptions;

namespace Banks.Banks;

public class DepositInterestRateConditions
{
    private const decimal MinMoneyLimit = 0;
    private const decimal MinInterestRate = 0;
    private const decimal _neededDifferenceBetwenelementsInLists = 1;

    private List<decimal> _moneyLimitsList;
    private List<decimal> _interestRatesList;

    public DepositInterestRateConditions(decimal interestRate)
    {
        if (!InterestRateIsCorrect(interestRate))
        {
            throw new BanksException("Interest rate is incorrect while creating DepositInterestRateConditions.");
        }

        _moneyLimitsList = new List<decimal>() { MinMoneyLimit };
        _interestRatesList = new List<decimal>() { interestRate };
    }

    public DepositInterestRateConditions(List<decimal> limits, List<decimal> interestRates)
    {
        if (!MoneyLimitsAreCorrect(limits))
        {
            throw new BanksException("limits are incorrect while creating DepositInterestRateConditions.");
        }

        if (!InterestRatesAreCorrect(interestRates))
        {
            throw new BanksException("interestRates are incorrect while creating DepositInterestRateConditions.");
        }

        if ((interestRates.Count - limits.Count) != _neededDifferenceBetwenelementsInLists)
            throw new BanksException("interestRates and limits are incorrect while creating DepositInterestRateConditions.");

        _moneyLimitsList = new List<decimal>() { MinMoneyLimit };
        _moneyLimitsList.AddRange(limits);
        _interestRatesList = interestRates;
    }

    public IReadOnlyCollection<decimal> MoneyLimitsList => _moneyLimitsList;
    public IReadOnlyCollection<decimal> InterestRatesList => _interestRatesList;

    private bool InterestRateIsCorrect(decimal interestRate)
    {
        if (interestRate <= MinInterestRate)
            return false;
        return true;
    }

    private bool MoneyLimitIsCorrect(decimal moneyLimit)
    {
        if (moneyLimit <= MinMoneyLimit)
            return false;
        return true;
    }

    private bool InterestRatesAreCorrect(List<decimal> interestRatesList)
    {
        if (interestRatesList is null)
            return false;

        var allCorrect = interestRatesList.All(interest => InterestRateIsCorrect(interest));

        return allCorrect;
    }

    private bool MoneyLimitsAreCorrect(List<decimal> moneyLimitsList)
    {
        if (moneyLimitsList is null)
            return false;

        var allCorrect = moneyLimitsList.All(limit => MoneyLimitIsCorrect(limit));

        return allCorrect;
    }
}