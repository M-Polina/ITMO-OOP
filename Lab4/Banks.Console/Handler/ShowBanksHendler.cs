using Banks.Banks;

namespace Banks.Console.Handler;

public class ShowBanksHendler : AbstractHandler
{
    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Show banks"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        CentralBank centralBank = CentralBank.GetInstance();

        foreach (var acc in centralBank.BanksList)
        {
            System.Console.WriteLine($"{acc.Name} : {acc.Id}");
        }
    }
}