using Banks.Banks;

namespace Banks.Console.Handler;

public class SpeedUpTimeHandler : AbstractHandler
{
    private const int MinNum = 0;

    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Speed up time"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        CentralBank centralBank = CentralBank.GetInstance();
        centralBank.SpeedUpTime(GetDays());
    }

    private int GetDays()
    {
        System.Console.WriteLine("Set amount of days to skip:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set amount of days to skip:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNum)
            number = GetDays();

        return number;
    }
}