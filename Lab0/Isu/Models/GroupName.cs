using System;
using System.Data;
using System.Text.RegularExpressions;
using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    private const int _groupNameLength = 5;

    public GroupName(string stringName)
    {
        if (stringName.Length != _groupNameLength)
        {
            throw new InvalidGroupNameException(this, stringName);
        }

        if (!NameIsCorrect(stringName))
        {
            throw new InvalidGroupNameException(this, stringName);
        }

        StringName = stringName;
        Faculty = stringName[0];
        EducationLevel = Convert.ToInt32(stringName[1] - '0');
        CourseNumber = Convert.ToInt32(stringName[2] - '0');
        GroupNumber = int.Parse(stringName.Substring(3));
    }

    public string StringName { get; }

    public char Faculty { get; }

    public int EducationLevel { get; }

    public int CourseNumber { get; }

    public int GroupNumber { get; }

    public bool Equals(GroupName other)
    {
        return other != null && StringName == other.StringName;
    }

    private bool NameIsCorrect(string stringName)
    {
        Match match = Regex.Match(stringName, @"([A-Z])(3)([1-4])(\d){2}");
        return match.Success;
    }
}