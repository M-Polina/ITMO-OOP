using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class BankCreationHandler : AbstractHandler
{
    private const decimal MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Create bank"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        CentralBank centralBank = CentralBank.GetInstance();

        string name = GetName();
        decimal debitRate = GetDebitRate();
        decimal depositRate = GetDepositRate();
        decimal commission = GetCommission();
        decimal creditLim = GetCreditLimits();
        decimal suspiciousClientLimit = GetSuspiciousClientLimit();

        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(depositRate);
        Bank bank1 = centralBank.CreateBank(name, debitRate, commission, creditLim, deposintConditions, suspiciousClientLimit);
    }

    private decimal GetSuspiciousClientLimit()
    {
        System.Console.WriteLine("Set Suspicious Client Limit:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set Suspicious Client Limit:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetSuspiciousClientLimit();

        return number;
    }

    private decimal GetCreditLimits()
    {
        System.Console.WriteLine("Set Credit Limit:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set Credit Limit:");
            str = System.Console.ReadLine();
        }

        if (number > MinNumber)
            number = GetCreditLimits();

        return number;
    }

    private decimal GetDepositRate()
    {
        System.Console.WriteLine("Set Deposit Rate:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set Deposit Rate:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetDepositRate();

        return number;
    }

    private decimal GetDebitRate()
    {
        System.Console.WriteLine("Set Debit Rate:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set Debit Rate:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetDebitRate();

        return number;
    }

    private string GetName()
    {
        System.Console.WriteLine("Set Bank name:");
        string? name = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            System.Console.WriteLine("Set Bank name:");
            name = System.Console.ReadLine();
        }

        return name;
    }

    private decimal GetCommission()
    {
        System.Console.WriteLine("Set commission:");
        decimal number;
        string? com = System.Console.ReadLine();
        while (!decimal.TryParse(com, out number))
        {
            System.Console.WriteLine("Set commission:");
            com = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetCommission();

        return number;
    }
}