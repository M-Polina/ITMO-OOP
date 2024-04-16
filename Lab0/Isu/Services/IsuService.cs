using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private uint _id = 0;
    private List<Group<Student>> _groupsList;
    private List<Student> _studentsList;

    public IsuService()
    {
        _groupsList = new List<Group<Student>>();
        _studentsList = new List<Student>();
    }

    public IReadOnlyCollection<Group<Student>> GroupsList => _groupsList;
    public IReadOnlyCollection<Student> StudentsList => _studentsList;

    public Group<Student> AddGroup(GroupName name)
    {
        bool groupExists = GroupExists(name);

        if (groupExists)
        {
            throw new GroupAlreadyExistsExeption("This group already exists, so it can't be created.");
        }

        var newGroup = new Group<Student>(name);
        _groupsList.Add(newGroup);
        return newGroup;
    }

    public bool GroupExists(GroupName name)
    {
        return _groupsList.Any(group => group.Name.Equals(name));
    }

    public Student AddStudent(Group<Student> group, string name)
    {
        Group<Student>? foundgroup = _groupsList.Find(thisGroup => thisGroup.Equals(group));

        if (foundgroup == null)
        {
            throw new InvalidGroupException("This group doesn't exist.", group);
        }

        var newStudent = new Student(group, name, _id);
        _studentsList.Add(newStudent);
        _id += 1;
        return newStudent;
    }

    public Student GetStudent(int id)
    {
        Student? foundStudent = _studentsList.SingleOrDefault(student => student.Id == id);
        if (foundStudent == null)
        {
            throw new StudentNotFoundExeption($"There is no student with id = {id}");
        }

        return foundStudent;
    }

    public Student? FindStudent(int id)
    {
        return _studentsList.Find(student => student.Id == id);
    }

    public IReadOnlyCollection<Student> FindStudents(GroupName groupName)
    {
        if (groupName is null)
        {
            throw new NullGroupNameException("Group name is null, so we can't find students in that group");
        }

        return _studentsList.FindAll(student => student.Group.Name == groupName);
    }

    public IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber)
    {
        if (courseNumber is null)
        {
            throw new NullCourseNumberException("course Number  is null, so we can't find students in that group");
        }

        return _studentsList.FindAll(student => student.Group.Name.CourseNumber == courseNumber.Name);
    }

    public Group<Student>? FindGroup(GroupName groupName)
    {
        return _groupsList.Find(group => group.Name == groupName);
    }

    public IReadOnlyCollection<Group<Student>> FindGroups(CourseNumber courseNumber)
    {
        if (courseNumber is null)
        {
            throw new NullCourseNumberException("course Number  is null, so we can't find group");
        }

        return _groupsList.FindAll(group => group.Name.CourseNumber == courseNumber.Name);
    }

    public void ChangeStudentGroup(Student student, Group<Student> newGroup)
    {
        if (student is null)
        {
            throw new StudentIsNullExeption("student is null so he can't change the group");
        }

        student.ChangeStudentsGroup(newGroup);
    }
}