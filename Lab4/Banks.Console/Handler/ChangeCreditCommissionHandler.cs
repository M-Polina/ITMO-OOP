using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class ChangeCreditCommissionHandler : AbstractHandler
{
    private const decimal MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Change credit commission"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        Bank bank = GetBankId();

        bank.ChangeCreditComission(GetCreditCommission());
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

    private decimal GetCreditCommission()
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
            number = GetCreditCommission();

        return number;
    }
}