using System.Data;

namespace Banks.Time;

// This class has to be a singletone, so we need to use static here.
public class OwnTime
{
    private const int MinDaysNum = 0;
    private const int DaysInMonth = 30;

    private static OwnTime? _instance;

    private OwnTime()
    {
        TimeNow = DateTime.Now;
    }

    public DateTime TimeNow { get; private set; }

    public static OwnTime GetInstance()
    {
        if (_instance is null)
            _instance = new OwnTime();

        return _instance;
    }

    public void SpeedUp()
    {
        TimeNow = TimeNow.AddDays(1);
    }

    public bool MonthPassed(DateTime time)
    {
       return (((TimeNow - time).Days % DaysInMonth) == MinDaysNum) && (TimeNow > time);
    }
}