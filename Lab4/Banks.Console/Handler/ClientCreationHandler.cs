using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class ClientCreationHandler : AbstractHandler
{
    private const int MinNumber = 0;
    private const int MinId = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Create client"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        int bankId = GetBankId();
        Bank bank = CentralBank.GetInstance().GetBankById(bankId);
        string name = GetName();
        string surname = GetSurname();
        IBuilder builder = new ClientBuilder(bank, name, surname);

        if (WhantSetAddress())
            builder.SetAddress(GetAddress());

        if (WhantSetPassport())
        {
            builder.SetPassport(GetPassport(bankId));
        }

        ClientAccount client = builder.CreateAndGetClient();
        bank.AddClientAccount(client);
    }

    private string GetAddress()
    {
        System.Console.WriteLine("Set Client address:");
        string? str = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(str))
        {
            System.Console.WriteLine("Set Client address:");
            str = System.Console.ReadLine();
        }

        return str;
    }

    private int GetPassport(int bankId)
    {
        System.Console.WriteLine("Set Client Passport Id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set Client Passport Id:");
            str = System.Console.ReadLine();
        }

        Bank bank = CentralBank.GetInstance().GetBankById(bankId);
        if (number < MinId || bank.ClientPassportExists(number))
            number = GetPassport(bankId);

        return number;
    }

    private bool WhantSetAddress()
    {
        System.Console.WriteLine("Do you want to set address?");
        string? str = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(str))
        {
            System.Console.WriteLine("Do you want to set address?\n");
            str = System.Console.ReadLine();
        }

        if (str.Equals("no"))
            return false;

        if (str.Equals("yes"))
        {
            return true;
        }

        return WhantSetAddress();
    }

    private bool WhantSetPassport()
    {
        System.Console.WriteLine("Do you want to set passport?");
        string? str = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(str))
        {
            System.Console.WriteLine("Do you want to set passport?\n");
            str = System.Console.ReadLine();
        }

        if (str.Equals("no"))
            return false;

        if (str.Equals("yes"))
        {
            return true;
        }

        return WhantSetPassport();
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

    private string GetName()
    {
        System.Console.WriteLine("Set Client name:");
        string? name = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            System.Console.WriteLine("Set Client name:");
            name = System.Console.ReadLine();
        }

        return name;
    }

    private string GetSurname()
    {
        System.Console.WriteLine("Set Client surename:");
        string? name = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            System.Console.WriteLine("Set Client surename:");
            name = System.Console.ReadLine();
        }

        return name;
    }
}