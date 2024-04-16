using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTest
{
    private IsuExtraService _service;

    public IsuExtraServiceTest()
    {
        _service = new IsuExtraService();
    }

    [Fact]
    public void AddLessonsForGroup_GroupHasLesons()
    {
        GroupExtension group1 = _service.AddGroup("A3211");

        var lesson1 = new Lesson<GroupExtension>(1, 2, "Vendy", "Programming", group1);
        var lesson2 = new Lesson<GroupExtension>(2, 3, "Piter", "Math", group1);

        _service.AddLessonForGroup(group1, lesson1);
        _service.AddLessonForGroup(group1, lesson2);

        Assert.Equal(1, group1.Timetable.LessonsList.ElementAt(0).Day);
        Assert.Equal(2, group1.Timetable.LessonsList.ElementAt(0).LessonTime.LessonNumber);
        Assert.Equal("Vendy", group1.Timetable.LessonsList.ElementAt(0).TeacherName);
        Assert.Equal("Programming", group1.Timetable.LessonsList.ElementAt(0).Name);
    }

    [Fact]
    public void AddOgnpFlowToCourse_CourseHasFlowAndFlowIsInCourse()
    {
        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson1 = new Lesson<OgnpFlow>(1, 2, "Vendy", "Programming", flow1);
        timetable.AddLesson(lesson1);

        _service.AddTimetableToOgnpFlow(flow1, timetable);

        var courseFlows = _service.GetOgnpFlowsFromCourse(course);

        Assert.Equal(flow1, course.FlowList.ElementAt(0));
        Assert.Equal(course, flow1.Course);
        Assert.Equal(flow1, courseFlows.ElementAt(0));
    }

    [Fact]
    public void AddLessonsForOgnpFlow_FlowHasLesons()
    {
        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson1 = new Lesson<OgnpFlow>(1, 2, "Vendy", "Programming", flow1);
        timetable.AddLesson(lesson1);

        _service.AddTimetableToOgnpFlow(flow1, timetable);

        Assert.Equal(1, flow1.Timetable.LessonsList.ElementAt(0).Day);
        Assert.Equal(2, flow1.Timetable.LessonsList.ElementAt(0).LessonTime.LessonNumber);
        Assert.Equal("Vendy", flow1.Timetable.LessonsList.ElementAt(0).TeacherName);
        Assert.Equal("Programming", flow1.Timetable.LessonsList.ElementAt(0).Name);
    }

    [Fact]
    public void RegisterStudentForOgnpFlow_FlowHasStudentStudentHasFlow()
    {
        GroupExtension group1 = _service.AddGroup("A3211");

        var lesson1 = new Lesson<GroupExtension>(1, 2, "Vendy", "Programming", group1);
        var lesson2 = new Lesson<GroupExtension>(2, 3, "Piter", "Math", group1);

        _service.AddLessonForGroup(group1, lesson1);
        _service.AddLessonForGroup(group1, lesson2);

        FacultyStudent student = _service.AddFacultyStudent(group1, "Den");

        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson0 = new Lesson<OgnpFlow>(3, 2, "Milly", "Lection", flow1);
        timetable.AddLesson(lesson0);

        _service.AddStudentToOgnpFlow(student, flow1);

        Assert.True(student.Id == flow1.StudentList.ElementAt(0).Id);
        Assert.Equal(flow1, student.OgnpList.ElementAt(0));
    }

    [Fact]
    public void RegisterStudentForOgnpFlow_LessonstIntersect()
    {
        GroupExtension group1 = _service.AddGroup("A3211");

        var lesson1 = new Lesson<GroupExtension>(1, 2, "Vendy", "Programming", group1);
        var lesson2 = new Lesson<GroupExtension>(2, 3, "Piter", "Math", group1);

        _service.AddLessonForGroup(group1, lesson1);
        _service.AddLessonForGroup(group1, lesson2);

        FacultyStudent student = _service.AddFacultyStudent(group1, "Den");

        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson0 = new Lesson<OgnpFlow>(1, 2, "Milly", "Lection", flow1);
        timetable.AddLesson(lesson0);
        flow1.AddTimetable(timetable);

        Assert.Throws<LessonsIntersectException>(() => _service.AddStudentToOgnpFlow(student, flow1));
    }

    [Fact]
    public void RemoveStudentFromOgnpFlow_FlowHasNoStudentStudentHasNoOgnpFlow()
    {
        GroupExtension group1 = _service.AddGroup("A3211");

        var lesson1 = new Lesson<GroupExtension>(1, 2, "Vendy", "Programming", group1);
        var lesson2 = new Lesson<GroupExtension>(2, 3, "Piter", "Math", group1);

        _service.AddLessonForGroup(group1, lesson1);
        _service.AddLessonForGroup(group1, lesson2);

        FacultyStudent student = _service.AddFacultyStudent(group1, "Den");

        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson0 = new Lesson<OgnpFlow>(3, 2, "Milly", "Lection", flow1);
        timetable.AddLesson(lesson0);

        _service.AddStudentToOgnpFlow(student, flow1);
        _service.RemoveStudentFromOgnpFlow(student, flow1);

        Assert.Null(flow1.FindStudent(student));
        Assert.False(student.HasOgnpFlow(flow1));
    }

    [Fact]
    public void GetStudensWithoutOgnpFlow()
    {
        GroupExtension group1 = _service.AddGroup("A3211");

        var lesson1 = new Lesson<GroupExtension>(1, 2, "Vendy", "Programming", group1);
        var lesson2 = new Lesson<GroupExtension>(2, 3, "Piter", "Math", group1);

        _service.AddLessonForGroup(group1, lesson1);
        _service.AddLessonForGroup(group1, lesson2);

        FacultyStudent student1 = _service.AddFacultyStudent(group1, "Den");
        FacultyStudent student2 = _service.AddFacultyStudent(group1, "Lens");

        OgnpCourse course = _service.AddOgnpCourse("Cyber security", 'S');
        OgnpFlow flow1 = _service.AddOgnpFlow("Cyb3.1", course);

        var timetable = new Timetable<OgnpFlow>();
        var lesson0 = new Lesson<OgnpFlow>(3, 2, "Milly", "Lection", flow1);
        timetable.AddLesson(lesson0);

        _service.AddStudentToOgnpFlow(student1, flow1);

        Assert.True(student2.Id == _service.GtStudentsWithoutOgnpFromGroup(group1).ElementAt(0).Id);
    }
}