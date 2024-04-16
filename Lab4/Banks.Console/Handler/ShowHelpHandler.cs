namespace Banks.Console.Handler;

public class ShowHelpHandler : AbstractHandler
{
    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Help"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        System.Console.WriteLine("List of commands available:");
        System.Console.WriteLine("-Help");
        System.Console.WriteLine("-Create bank");
        System.Console.WriteLine("-Create client");
        System.Console.WriteLine("-Create account");
        System.Console.WriteLine("-Set address");
        System.Console.WriteLine("-Set passport");
        System.Console.WriteLine("-Change debit interest rate");
        System.Console.WriteLine("-Change deposit interest rate conditions");
        System.Console.WriteLine("-Change credit commission");
        System.Console.WriteLine("-Change credit limit");
        System.Console.WriteLine("-Change deposit interest rate conditions");
        System.Console.WriteLine("-Show clients");
        System.Console.WriteLine("-Show banks");
        System.Console.WriteLine("-Show accounts");
        System.Console.WriteLine("-Subscribe to notifications");
        System.Console.WriteLine("-Speed up time");
        System.Console.WriteLine("-Add");
        System.Console.WriteLine("-Withdraw");
        System.Console.WriteLine("-Transfer");
        System.Console.WriteLine("-Rollback");
    }
}