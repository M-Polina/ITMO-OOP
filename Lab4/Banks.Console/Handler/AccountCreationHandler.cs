using Banks.Accounts;
using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class AccountCreationHandler : AbstractHandler
{
    private const decimal MinNumber = 0;
    private const int MinId = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Create account"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        CentralBank centralBank = CentralBank.GetInstance();

        System.Console.WriteLine("Available Banks:");
        foreach (var acc in centralBank.BanksList)
        {
            System.Console.WriteLine($"{acc.Name} : {acc.Id}");
        }

        int bankId = GetBankId();
        Bank bank = CentralBank.GetInstance().GetBankById(bankId);

        System.Console.WriteLine("Chose clients from: ");
        for (int i = MinId; i < bank.ClientAccountsList.Count; i++)
        {
            IReadOnlyCollection<ClientAccount> acc = bank.ClientAccountsList;
            System.Console.WriteLine($"{i}) {acc.ElementAt(i).FullName.Name}  {acc.ElementAt(i).FullName.Surname} {acc.ElementAt(i).Passport}");
        }

        int clientInd = GetClientNum();
        if (clientInd < MinId || clientInd >= bank.ClientAccountsList.Count)
        {
            System.Console.WriteLine("Wrong client number.");
            return;
        }

        ClientAccount client = bank.ClientAccountsList.ElementAt(clientInd);

        System.Console.WriteLine("Available accounts: Debit, Deposit, Credit");
        AccountTypes accountType = GetAccountType();

        switch (accountType)
        {
            case AccountTypes.DebitAccount:
                DebitAccount acc = bank.CreateDebitAccount(client);
                System.Console.WriteLine(bank.AccountsList.Contains(acc));
                System.Console.WriteLine("DebitAccount created");
                break;
            case AccountTypes.DepositAccount:
                bank.CreateDepositAccount(client, GetAmountOfMoney(), GetMonthDuration());
                System.Console.WriteLine("DepositAccount created");
                break;
            default:
                bank.CreateCreditAccount(client);
                System.Console.WriteLine("CreditAccount created");
                break;
        }
    }

    private decimal GetAmountOfMoney()
    {
        System.Console.WriteLine("Set initial amount of money to create Deposit account:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set initial amount of money to create Deposit account:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetAmountOfMoney();

        return number;
    }

    private int GetMonthDuration()
    {
        System.Console.WriteLine("Set duration of Deposit account in months:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set duration of Deposit account in months:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetMonthDuration();

        return number;
    }

    private int GetClientNum()
    {
        System.Console.WriteLine("Set client number:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set client number:");
            str = System.Console.ReadLine();
        }

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

    private AccountTypes GetAccountType()
    {
        System.Console.WriteLine("Set Account type:");
        string? str = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(str))
        {
            System.Console.WriteLine("Set Account type:");
            str = System.Console.ReadLine();
        }

        if (str.Equals("Debit"))
            return AccountTypes.DebitAccount;
        if (str.Equals("Deposit"))
            return AccountTypes.DepositAccount;
        if (str.Equals("Credit"))
            return AccountTypes.CredicAccount;
        return GetAccountType();
    }
}