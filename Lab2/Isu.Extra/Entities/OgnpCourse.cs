using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class OgnpCourse
{
    private const char MinFaculty = 'A';
    private const char MaxFaculty = 'Z';

    private List<OgnpFlow> _flowList;

    public OgnpCourse(string name, char faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new NullGroupNameException("Ognp name can'be null.");
        }

        if (faculty < MinFaculty || faculty > MaxFaculty)
        {
            throw new WrongFacultyException("Faculty is wrong, course can't be created.");
        }

        Name = name;
        Faculty = faculty;
        _flowList = new List<OgnpFlow>();
    }

    public string Name { get; }
    public char Faculty { get; }
    public IReadOnlyCollection<OgnpFlow> FlowList => _flowList;

    public void AddOgnpFlow(OgnpFlow flow)
    {
        if (flow is null)
        {
            throw new NullOgnpFlowException("OgnpFlow is null so it can't be added to OgnpCouse.");
        }

        if (_flowList.Contains(flow))
        {
            throw new OgnpFlowAlreadyExistsException("Ognp already exists in course.");
        }

        _flowList.Add(flow);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return string.Equals(Name, ((OgnpCourse)obj).Name);
    }

    public override int GetHashCode() => Name.GetHashCode();
}