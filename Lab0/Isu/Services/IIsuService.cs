using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group<Student> AddGroup(GroupName name);
    Student AddStudent(Group<Student> group, string name);

    Student GetStudent(int id);
    Student? FindStudent(int id);
    IReadOnlyCollection<Student> FindStudents(GroupName groupName);
    IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber);

    Group<Student>? FindGroup(GroupName groupName);
    IReadOnlyCollection<Group<Student>> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group<Student> newGroup);
}
