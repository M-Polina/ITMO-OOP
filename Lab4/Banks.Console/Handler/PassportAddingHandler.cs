using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class PassportAddingHandler : AbstractHandler
{
    private const int MinId = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Set passport"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        int bankId = GetBankId();
        Bank bank = CentralBank.GetInstance().GetBankById(bankId);

        System.Console.WriteLine("Shose clients from: ");
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

        bank.AddClientPassport(client, GetPassport(bankId));
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
}