using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService service;

    public IsuServiceTest()
    {
        service = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        GroupName groupName1 = new GroupName("M3201");
        Group<Student> group1 = service.AddGroup(groupName1);
        Student student1 = service.AddStudent(group1, "Den");
        Assert.True(student1.Group == group1);
        Assert.Contains(student1, group1.StudentList);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        GroupName groupName1 = new GroupName("M3201");
        Group<Student> group1 = service.AddGroup(groupName1);
        for (int i = 0; i < group1.MaxAmountOfStudents; i++)
        {
            service.AddStudent(group1, "Den");
        }

        Assert.Throws<GroupIsFullExeption>(() => service.AddStudent(group1, "Mike"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("№3201")));
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("A4201")));
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("A3501")));
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("A32-1")));
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("A320p1")));
        Assert.Throws<InvalidGroupNameException>(() => service.AddGroup(new GroupName("A3p01")));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        GroupName groupName1 = new GroupName("M3201");
        Group<Student> group1 = service.AddGroup(groupName1);
        GroupName groupName2 = new GroupName("M3202");
        Group<Student> group2 = service.AddGroup(groupName2);
        Student student1 = service.AddStudent(group1, "Den");
        service.ChangeStudentGroup(student1, group2);
        Assert.False(student1.Group == group1);
        Assert.True(student1.Group == group2);
        Assert.DoesNotContain(student1, group1.StudentList);
        Assert.Contains(student1, group2.StudentList);
    }
}