using Banks.Banks;
using Banks.Client;
using Banks.MessageHandlers;

namespace Banks.Console.Handler;

public class SubscribeHandler : AbstractHandler
{
    private const int MinId = 0;

    public ConsoleMessageHandler Handler { get; } = new ConsoleMessageHandler();

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Subscribe"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        System.Console.WriteLine("Types of objects, you can subsctribe to: \n Debit interest rate\nDeposit conditions");
        System.Console.WriteLine("Credit commission\nCredit limit\nSuspicious client limit");
        Bank bank = GetBankId();

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

        bank.Subscribe(client, Handler, GetObservableType());
    }

    private ObservablesNames GetObservableType()
    {
        System.Console.WriteLine("Set condition to subscribe:");
        string? str = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(str))
        {
            System.Console.WriteLine("Set condition to subscribe:");
            str = System.Console.ReadLine();
        }

        if (str.Equals("Credit commission"))
            return ObservablesNames.CreditCommission;
        if (str.Equals("Credit limit"))
            return ObservablesNames.CreditLimit;
        if (str.Equals("Deposit conditions"))
            return ObservablesNames.DepositConditions;
        if (str.Equals("Debit interest rate"))
            return ObservablesNames.DebitInterestRate;
        if (str.Equals("Suspicious client limit"))
            return ObservablesNames.SuspiciousClientLimit;

        return GetObservableType();
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