using Isu.Exceptions;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class FacultyStudent
{
    public const int MaxAmountOfOgnp = 2;

    private List<OgnpFlow> _ognpList;

    public FacultyStudent(GroupExtension group, string name, uint id)
    {
        if (group is null || string.IsNullOrWhiteSpace(name))
        {
            throw new StudentCanNotBeCreatedExeption("Group or string name is incorrect, so student can't be created.");
        }

        Group = group;
        Name = name;
        Id = id;
        _ognpList = new List<OgnpFlow>();
    }

    public List<OgnpFlow> OgnpList => _ognpList;
    public GroupExtension Group { get; }
    public string Name { get; }
    public uint Id { get; }

    public bool HasOgnpFlow(OgnpFlow flow) => _ognpList.Any(f => f.Equals(flow));

    public bool HasNoOgnp()
    {
        return _ognpList.Count == 0;
    }

    public void AddOgnp(OgnpFlow newOgnp)
    {
        if (newOgnp is null)
            throw new WrorngOgnpException("Ognp is null, so student can't register for it.");

        if (newOgnp.Course.Faculty == Group.Name.Faculty)
            throw new WrorngOgnpException("Ognp is in your faculty, so student can't chose it.");

        if (_ognpList.Count() >= MaxAmountOfOgnp)
            throw new WrorngOgnpException("This student already has two ognp, so he can't register for new one.");

        if (Group.Timetable.ContainsLessonsFromOgnp(newOgnp.Timetable.LessonsList))
            throw new LessonsIntersectException("Timetables intersect, so student can't register for this ognp.");

        _ognpList.Add(newOgnp);
    }

    public void DeleteOgnp(OgnpFlow ognpToDelete)
    {
        if (ognpToDelete is null)
            throw new WrorngOgnpException("Ognp is null, so it can't be deleted from student");

        if (!_ognpList.Contains(ognpToDelete))
            throw new WrorngOgnpException("This student doesn't have this ognp, so it can't be deleted.");

        _ognpList.Remove(ognpToDelete);
    }
}