using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    public Student(Group<Student> group, string name, uint id)
    {
        if (group is null || string.IsNullOrWhiteSpace(name))
        {
            throw new StudentCanNotBeCreatedExeption("Group or string name is incorrect, so student can't be created.");
        }

        Group = group;
        Name = name;
        Id = id;
        Group.AddStudent(this);
    }

    public Group<Student> Group { get; private set; }
    public string Name { get; }
    public uint Id { get; }

    public void ChangeStudentsGroup(Group<Student> newGroup)
    {
        if (newGroup == null)
        {
            throw new NullGroupExeption("The group is null so student can't change group.");
        }

        newGroup.AddStudent(this);
        Group.RemoveStudent(this);
        Group = newGroup;
    }
}