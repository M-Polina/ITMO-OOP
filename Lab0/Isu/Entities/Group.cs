using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group<TStudent>
    where TStudent : class
{
    private const uint _maxAmountOfStudents = 21;
    private const uint _minAmountOfStudents = 0;
    private List<TStudent> _studentsList;

    public Group(GroupName name)
    {
        if (name == null)
        {
            throw new NullGroupNameException("Group can't be created because GroupName is null.");
        }

        Name = name;
        _studentsList = new List<TStudent>();
    }

    public IReadOnlyCollection<TStudent> StudentList => _studentsList;

    public uint MaxAmountOfStudents => _maxAmountOfStudents;

    public GroupName Name { get; }

    public void AddStudent(TStudent student)
    {
        if (student is null)
        {
            throw new StudentIsNullExeption("student is null so he can't change the group");
        }

        if (_studentsList.Count() >= MaxAmountOfStudents)
        {
            throw new GroupIsFullExeption("Student can't be added to group, because it is Full");
        }

        if (_studentsList.Contains(student))
        {
            throw new StudentIsFoundException("Student can't be added to group, because he is there.");
        }

        _studentsList.Add(student);
    }

    public void RemoveStudent(TStudent student)
    {
        if (student is null)
        {
            throw new StudentIsNullExeption("student is null so he can't change the group");
        }

        if (!_studentsList.Contains(student))
        {
            throw new StudentNotFoundExeption("Student can't be removed from the group, because he is not there.");
        }

        _studentsList.Remove(student);
    }

    public bool Equals(Group<TStudent> other)
    {
        return other != null && Name.Equals(other.Name);
    }
}