using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class LessonTime
{
    private const int _minLessonNumber = 1;
    private const int _maxLessonNumber = 8;

    private int _lessonNumber;
    private Time _startTime;
    private Time _finishTime;

    public LessonTime(int lessonNumber)
    {
        if (lessonNumber < _minLessonNumber || lessonNumber > _maxLessonNumber)
        {
            throw new IncorrectLessonDateException("Wrong lesson number.");
        }

        _lessonNumber = lessonNumber;

        switch (lessonNumber)
        {
            case 1:
                _startTime = new Time(8, 20);
                _finishTime = new Time(9, 50);
                break;
            case 2:
                _startTime = new Time(10, 00);
                _finishTime = new Time(11, 30);
                break;
            case 3:
                _startTime = new Time(11, 40);
                _finishTime = new Time(13, 10);
                break;
            case 4:
                _startTime = new Time(13, 30);
                _finishTime = new Time(15, 00);
                break;
            case 5:
                _startTime = new Time(15, 20);
                _finishTime = new Time(16, 50);
                break;
            case 6:
                _startTime = new Time(17, 00);
                _finishTime = new Time(18, 30);
                break;
            case 7:
                _startTime = new Time(19, 00);
                _finishTime = new Time(21, 30);
                break;
            default:
                _startTime = new Time(21, 00);
                _finishTime = new Time(22, 30);
                break;
        }
    }

    public int LessonNumber => _lessonNumber;
    public Time StartTime => _startTime;
    public Time FinishTime => _finishTime;

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return _lessonNumber == ((LessonTime)obj).LessonNumber;
    }

    public override int GetHashCode() => _lessonNumber.GetHashCode();
}