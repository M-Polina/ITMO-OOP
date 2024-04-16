using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Lesson<TGroup>
    where TGroup : class
{
    private const int _minDay = 1;
    private const int _maxDay = 7;

    public Lesson(int day, int lessonNumber, string teacherName, string name, TGroup group)
    {
        if (day < _minDay || day > _maxDay)
        {
            throw new IncorrectLessonDateException("Wrong day in a week number.");
        }

        if (group is null)
        {
            throw new NullCourseFlowException("Flow is null so CourseFlowLesson can't be created.");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name can't be null");
        }

        Day = day;
        TeacherName = teacherName;
        Group = group;
        Name = name;
        LessonTime = new LessonTime(lessonNumber);
    }

    public int Day { get; }
    public string Name { get; }
    public string TeacherName { get; }
    public LessonTime LessonTime { get; private set; }
    public TGroup Group { get; private set; }

    public bool SameTimeAndPlace<TOtherGroup>(Lesson<TOtherGroup> lesson)
        where TOtherGroup : class
    {
        return LessonTime.Equals(lesson.LessonTime) && Day == lesson.Day;
    }
}