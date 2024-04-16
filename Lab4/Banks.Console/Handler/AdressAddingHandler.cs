using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class AdressAddingHandler : AbstractHandler
{
    private const int MinNumber = 0;
    private const int MinId = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Set address"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
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

        bank.AddClientAdress(client, GetAddress());
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
}