using Isu.Extra.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Timetable<TGroup>
    where TGroup : class
{
    private const int _minLessonsNumber = 0;

    private List<Lesson<TGroup>> _lessonsList;

    public Timetable()
    {
        _lessonsList = new List<Lesson<TGroup>>();
    }

    public IReadOnlyCollection<Lesson<TGroup>> LessonsList => _lessonsList;

    public void AddLesson(Lesson<TGroup> givenLesson)
    {
        if (givenLesson is null)
        {
            throw new LessonIsNullException("Lesson is null so it can't be added to group.");
        }

        if (Contains(givenLesson))
        {
            throw new LessonAlreadyExistsException("Lesson already exists in group, so it can't be added");
        }

        if (_lessonsList.Count > _minLessonsNumber)
        {
            if (!givenLesson.Group.Equals(_lessonsList[0].Group))
            {
                throw new WrongLessonException(
                    "This lesson has not the same group that others in timetable, so it can't be added.");
            }
        }

        _lessonsList.Add(givenLesson);
    }

    public bool Contains<TOtherGroup>(Lesson<TOtherGroup> givenLesson)
        where TOtherGroup : class
    {
        return _lessonsList.Any(lesson => lesson.SameTimeAndPlace<TOtherGroup>(givenLesson));
    }

    public bool ContainsOgnpLesson(Lesson<OgnpFlow> givenLesson)
    {
        return _lessonsList.Any(lesson => lesson.SameTimeAndPlace<OgnpFlow>(givenLesson));
    }

    public bool ContainsLessonsFromOgnp(IReadOnlyCollection<Lesson<OgnpFlow>> lessonsList)
    {
        if (lessonsList is null)
            throw new NullLessonsListException("Lessons list is null.");

        return lessonsList.Any(lesson => ContainsOgnpLesson(lesson));
    }
}