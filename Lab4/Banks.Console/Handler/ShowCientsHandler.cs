using Banks.Accounts;
using Banks.Banks;
using Banks.Client;

namespace Banks.Console.Handler;

public class ShowCientsHandler : AbstractHandler
{
    private const int MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Show clients"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        Bank bank = GetBankId();

        foreach (var acc in bank.ClientAccountsList)
        {
            string address = string.Empty;
            if (acc.Address is not null)
                address = acc.Address.Value;
            string id = string.Empty;
            if (acc.Passport is not null)
                id = string.Format($"{0}", acc.Passport.Id);
            System.Console.WriteLine($"{acc.FullName.Name} {acc.FullName.Surname} : {acc.Id} : {address} : {id}");
        }
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