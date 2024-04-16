using Banks.Banks;

namespace Banks.Console.Handler;

public class ChangeDepositInterestRateHandler : AbstractHandler
{
    private const decimal MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Change deposit interest rate conditions"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        Bank bank = GetBankId();

        DepositInterestRateConditions conditions = new DepositInterestRateConditions(GetDepositRate());
        bank.ChangeDepositConditions(conditions);
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

    private Bank GetBankId()
    {
        System.Console.WriteLine("Set bank Id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set bank Id:");
            str = System.Console.ReadLine();
        }

        CentralBank centralBank = CentralBank.GetInstance();

        return centralBank.GetBankById(number);
    }
}