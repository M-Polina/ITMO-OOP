using Banks.Accounts;
using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class TransferHandler : AbstractHandler
{
    private const decimal MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Transfer"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        System.Console.WriteLine("Set information about account to transfer from:");
        Bank bankFrom = GetBankId();
        IAccount accountFrom = bankFrom.GetAccountById(GetAccountId(bankFrom));

        System.Console.WriteLine("Set information about account to transfer to:");
        Bank bankTo = GetBankId();
        IAccount accountTo = bankTo.GetAccountById(GetAccountId(bankTo));

        decimal amount = GetAmountOfMoney();
        bankFrom.TransferMoney(accountFrom, accountTo, amount);
    }

    private decimal GetAmountOfMoney()
    {
        System.Console.WriteLine("Set  amount of money to transfer:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set  amount of money to transfer:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetAmountOfMoney();

        return number;
    }

    private int GetAccountId(Bank bank)
    {
        System.Console.WriteLine("Set account id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set account id:");
            str = System.Console.ReadLine();
        }

        return number;
    }

    private Bank GetBankId()
    {
        System.Console.WriteLine("Set bank id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set bank id:");
            str = System.Console.ReadLine();
        }

        CentralBank centralBank = CentralBank.GetInstance();

        return centralBank.GetBankById(number);
    }
}