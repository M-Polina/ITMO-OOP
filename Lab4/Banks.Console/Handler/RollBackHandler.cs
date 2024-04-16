using System.Transactions;
using Banks.Banks;

namespace Banks.Console.Handler;

public class RollBackHandler : AbstractHandler
{
    private const int MinNumber = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("RollBack"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        System.Console.WriteLine("Rollback of last transaction in bank will be created.");
        Bank bank = GetBankId();
        Guid transactionId = bank.LastTransactionId;
        bank.RollbackTrabsaction(transactionId);
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
        if (centralBank.FindBankById(number) is null && centralBank.BanksList.Count != MinNumber)
            number = GetBankId().Id;

        return centralBank.GetBankById(number);
    }
}