using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Entities;

public class GroupExtension : Group<FacultyStudent>
{
    public GroupExtension(GroupName name)
        : base(name)
    {
        Timetable = new Timetable<GroupExtension>();
    }

    public Timetable<GroupExtension> Timetable { get; private set; }

    public IReadOnlyCollection<Lesson<GroupExtension>> LessonsList => Timetable.LessonsList;

    public void AddLesson(Lesson<GroupExtension> groupLesson)
    {
        if (groupLesson is null)
        {
            throw new WrongGroepLessonException("Lesson is null co it can't be added to group");
        }

        if (!groupLesson.Group.Equals(this))
        {
            throw new WrongGroepLessonException("Lesson is not for this group co it can't be added to group");
        }

        Timetable.AddLesson(groupLesson);
    }
}