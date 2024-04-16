using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class OgnpFlow
{
    public const int MaxAmountOfStudents = 20;

    private List<FacultyStudent> _studentList;

    public OgnpFlow(string name, OgnpCourse course)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name can't be null or wite space for Course flow.");
        }

        if (course is null)
        {
            throw new NullOgnpCourceException("Ognp course is null.");
        }

        if (course.FlowList.Any(flow => flow.Name.Equals(name)))
        {
            throw new OgnpFlowAlreadyExistsException("Ognp flow already exists, so it can't be added.");
        }

        Name = name;
        Course = course;
        _studentList = new List<FacultyStudent>();
        Timetable = new Timetable<OgnpFlow>();
    }

    public string Name { get; }
    public Timetable<OgnpFlow> Timetable { get; private set; }
    public OgnpCourse Course { get; private set; }

    public IReadOnlyCollection<FacultyStudent> StudentList => _studentList;
    public IReadOnlyCollection<Lesson<OgnpFlow>> LessonsList => Timetable.LessonsList;
    public bool IsFull() => _studentList.Count == MaxAmountOfStudents;
    public FacultyStudent? FindStudent(FacultyStudent student) => _studentList.Find(s => student.Id == s.Id);

    public void AddTimetable(Timetable<OgnpFlow> timetable)
    {
        if (timetable is null)
        {
            throw new NullTimetableException("Timetable is null, so ognp flow can't be created.");
        }

        if (timetable.LessonsList.Count == 0)
        {
            throw new EmptyTimetableException("Timetable has no lessons, so ognp flow can't be created.");
        }

        if (timetable.LessonsList.ElementAt(0).Equals(this))
        {
            throw new EmptyTimetableException("Timetable has no lessons, so ognp flow can't be created.");
        }

        Timetable = timetable;
    }

    public void AddStudent(FacultyStudent student)
    {
        if (student == null)
        {
            throw new StudentIsNullExeption("Student is null, so it can be added to ognp.");
        }

        if (_studentList.Any(s => string.Equals(s.Name, student.Name)))
        {
            throw new WrorngOgnpException("Student already exists in group, so it can't be added");
        }

        if (IsFull())
            throw new WrorngOgnpException("This Ognp already is full, so this student can't be added");

        if (student.Group.Timetable.ContainsLessonsFromOgnp(Timetable.LessonsList))
        {
            throw new WrorngOgnpException("Timetables intersect, so student can't register for this ognp.");
        }

        _studentList.Add(student);
    }

    public void RemoveStudent(FacultyStudent student)
    {
        if (student == null)
        {
            throw new StudentIsNullExeption("Student is null, so it can be deleted from ognp.");
        }

        if (!_studentList.Any(s => string.Equals(s.Name, student.Name)))
        {
            throw new WrorngOgnpException("Student is not in group, so it can't be deleted");
        }

        _studentList.Remove(student);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return string.Equals(Name, ((OgnpFlow)obj).Name);
    }

    public override int GetHashCode() => Name.GetHashCode();
}