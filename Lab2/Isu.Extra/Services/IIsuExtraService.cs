using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
     bool GroupExistsByName(string name);

     GroupExtension AddGroup(string name);

     FacultyStudent AddFacultyStudent(GroupExtension group, string name);

     bool OgnpCourseExists(string name);

     OgnpCourse AddOgnpCourse(string name, char faculty);

     OgnpFlow AddOgnpFlow(string name, OgnpCourse course);

     void AddTimetableToOgnpFlow(OgnpFlow flow, Timetable<OgnpFlow> timetable);

     void AddStudentToOgnpFlow(FacultyStudent student, OgnpFlow flow);

     void RemoveStudentFromOgnpFlow(FacultyStudent student, OgnpFlow flow);

     IReadOnlyCollection<OgnpFlow> GetOgnpFlowsFromCourse(OgnpCourse course);

     IReadOnlyCollection<FacultyStudent> GetStudentsFromOgnpFlow(OgnpFlow flow);

     IReadOnlyCollection<FacultyStudent> GtStudentsWithoutOgnpFromGroup(GroupExtension group);

     void AddLessonForGroup(GroupExtension group, Lesson<GroupExtension> lesson);
}