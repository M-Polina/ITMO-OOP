using Banks.Banks;

namespace Banks.Console.Handler;

public class ChangeDebitInterestRateHandler : AbstractHandler
{
    private const decimal MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Change debit interest rate"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        int bankId = GetBankId();
        Bank bank = CentralBank.GetInstance().GetBankById(bankId);

        bank.ChangeDebitInterestRate(GetDebitRate());
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

    private int GetBankId()
    {
        System.Console.WriteLine("Set bank Id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set bank Id:");
            str = System.Console.ReadLine();
        }

        return number;
    }
}