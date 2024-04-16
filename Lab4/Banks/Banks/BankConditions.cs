using Banks.Exceptions;

namespace Banks.Banks;

public class BankConditions
{
    private DebitInterestRate _debitInterestRate;
    private CreditLimit _creditLimit;
    private CreditCommission _commission;
    private SuspiciousClientMoneyLimit _suspiciousClientMoneyLimit;
    private DepositConditions _depositConditions;

    public BankConditions(decimal debitInterestRate, decimal commission, decimal creditLimit, DepositInterestRateConditions depositCoditions, decimal suspiciousClientMoneyLimit)
    {
        _debitInterestRate = new DebitInterestRate(debitInterestRate);
        _commission = new CreditCommission(commission);
        _creditLimit = new CreditLimit(creditLimit);
        _depositConditions = new DepositConditions(depositCoditions);
        _suspiciousClientMoneyLimit = new SuspiciousClientMoneyLimit(suspiciousClientMoneyLimit);
    }

    public decimal DebitInterestRate => _debitInterestRate.InterestRate;
    public DebitInterestRate DebitInterestRateClass => _debitInterestRate;
    public decimal Comission => _commission.Commission;
    public CreditCommission ComissionClass => _commission;
    public decimal CreditLimit => _creditLimit.Limit;
    public CreditLimit CreditLimitClass => _creditLimit;
    public decimal SuspiciousClientMoneyLimit => _suspiciousClientMoneyLimit.MoneyLimit;
    public SuspiciousClientMoneyLimit SuspiciousClientMoneyLimitClass => _suspiciousClientMoneyLimit;

    public DepositInterestRateConditions DepositInterestRateConditions => _depositConditions.DepositInterestRateConditions;
    public DepositConditions DepositConditionsClass => _depositConditions;

    public void ChangeDebitInterestRate(decimal newRate)
    {
        _debitInterestRate.ChangeDebitInterestRate(newRate);
    }

    public void ChangeCreditComission(decimal commission)
    {
        _commission.ChangeCreditComission(commission);
    }

    public void ChangeCreditLimit(decimal creditLimit)
    {
        _creditLimit.ChangeCreditLimit(creditLimit);
    }

    public void ChangeSuspiciousClientMoneyLimit(decimal newMoneyLimit)
    {
        _suspiciousClientMoneyLimit.ChangeSuspiciousClientMoneyLimit(newMoneyLimit);
    }

    public void ChangeDepositConditions(DepositInterestRateConditions coditions)
    {
        _depositConditions.ChangeDepositConditions(coditions);
    }
}