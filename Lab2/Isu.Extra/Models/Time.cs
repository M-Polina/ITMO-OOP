namespace Isu.Extra.Models;

public class Time
{
    private const uint _maxHour = 23;
    private const uint _maxMinute = 59;

    public Time(uint hour, uint minute)
    {
        if (hour > _maxHour)
        {
            throw new AggregateException("Hour can't be > 23");
        }

        if (minute > _maxMinute)
        {
            throw new AggregateException("Minute can't be > 59");
        }

        Minute = minute;
        Hour = hour;
    }

    public uint Hour { get; }
    public uint Minute { get; }
}