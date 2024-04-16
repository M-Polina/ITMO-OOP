using Isu.Exceptions;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private List<OgnpCourse> _ognpCoursesList;
    private List<FacultyStudent> _studentsList;
    private List<GroupExtension> _groupsList;
    private uint _facultyStudentId = 0;

    public IsuExtraService()
    {
        _ognpCoursesList = new List<OgnpCourse>();
        _studentsList = new List<FacultyStudent>();
        _groupsList = new List<GroupExtension>();
    }

    public IReadOnlyCollection<OgnpCourse> OgnpCoursesList => _ognpCoursesList;
    public IReadOnlyCollection<FacultyStudent> StudentsList => _studentsList;
    public IReadOnlyCollection<GroupExtension> GroupsList => _groupsList;

    public bool GroupExistsByName(string name)
    {
        return _groupsList.Any(group => group.Name.StringName.Equals(name));
    }

    public GroupExtension AddGroup(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new WrongNameException("String name of group can't be null or empty.");
        }

        bool groupExists = GroupExistsByName(name);

        if (groupExists)
        {
            throw new GroupAlreadyExistsExeption("This group already exists, so it can't be created.");
        }

        GroupName groupName = new GroupName(name);

        var newGroup = new GroupExtension(groupName);
        _groupsList.Add(newGroup);
        return newGroup;
    }

    public FacultyStudent AddFacultyStudent(GroupExtension group, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new WrongNameException("String name of student can't be null or empty.");
        }

        if (!_groupsList.Any(thisGroup => thisGroup.Equals(group)))
        {
            throw new GroupNotFoundException("This group doesn't exist, so student can't be created.");
        }

        var newStudent = new FacultyStudent(group, name, _facultyStudentId);
        group.AddStudent(newStudent);
        _studentsList.Add(newStudent);
        _facultyStudentId += 1;
        return newStudent;
    }

    public bool OgnpCourseExists(string name)
    {
        return _ognpCoursesList.Any(course => course.Name.Equals(name));
    }

    public OgnpCourse AddOgnpCourse(string name, char faculty)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new WrongNameException("String name of Ognp course can't be null or empty.");
        }

        if (OgnpCourseExists(name))
        {
            throw new OgnpCourseAlreadyExistsExeption("This ognp course already exists, so it can't be created.");
        }

        var newOgnpCourse = new OgnpCourse(name, faculty);
        _ognpCoursesList.Add(newOgnpCourse);
        return newOgnpCourse;
    }

    public OgnpFlow AddOgnpFlow(string name, OgnpCourse course)
    {
        var newOgnpFlow = new OgnpFlow(name, course);
        course.AddOgnpFlow(newOgnpFlow);
        return newOgnpFlow;
    }

    public void AddTimetableToOgnpFlow(OgnpFlow flow, Timetable<OgnpFlow> timetable)
    {
        if (timetable is null)
        {
            throw new NullTimetableException("Timetable is null so it can't be added.");
        }

        if (flow is null)
        {
            throw new NullOgnpFlowException("Flow is null so it can't have lessons.");
        }

        flow.AddTimetable(timetable);
    }

    public void AddStudentToOgnpFlow(FacultyStudent student, OgnpFlow flow)
    {
        if (student is null)
        {
            throw new StudentIsNullExeption("Student is null, so he can't be added to ognp");
        }

        if (flow is null)
        {
            throw new NullOgnpFlowException("Flow is null so student can't be added to this ognp.");
        }

        if (flow.IsFull())
        {
            throw new WrorngOgnpException("This Ognp already is full, so this student can't be added");
        }

        student.AddOgnp(flow);
        flow.AddStudent(student);
    }

    public void RemoveStudentFromOgnpFlow(FacultyStudent student, OgnpFlow flow)
    {
        if (student is null)
        {
            throw new StudentIsNullExeption("Student is null, so he can't be removed from ognp");
        }

        if (flow is null)
        {
            throw new NullOgnpFlowException("Flow is null so student can't be removed from this ognp.");
        }

        flow.RemoveStudent(student);
        student.DeleteOgnp(flow);
    }

    public IReadOnlyCollection<OgnpFlow> GetOgnpFlowsFromCourse(OgnpCourse course)
    {
        if (course is null)
        {
            throw new NullOgnpCourceException("Ognp course is null, so we can find flows from it.");
        }

        return course.FlowList;
    }

    public IReadOnlyCollection<FacultyStudent> GetStudentsFromOgnpFlow(OgnpFlow flow)
    {
        if (flow is null)
        {
            throw new NullOgnpCourceException("Ognp course is null, so we can find flows from it.");
        }

        return flow.StudentList;
    }

    public IReadOnlyCollection<FacultyStudent> GtStudentsWithoutOgnpFromGroup(GroupExtension group)
    {
        if (group is null)
        {
            throw new NullOgnpCourceException("Ognp course is null, so we can find flows from it.");
        }

        IReadOnlyCollection<FacultyStudent> students = group.StudentList.Where(student => student.HasNoOgnp()).ToList();

        return students;
    }

    public void AddLessonForGroup(GroupExtension group, Lesson<GroupExtension> lesson)
    {
        if (group is null)
        {
            throw new NullGroupExeption("Group is null so we can't add lesson");
        }

        group.AddLesson(lesson);
    }
}