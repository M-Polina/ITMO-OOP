using Isu.Entities;
using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    private const int _firstCourse = 1;
    private const int _fourthCourse = 4;
    public CourseNumber(int number)
    {
        if (number < _firstCourse || number > _fourthCourse)
        {
            throw new WrongNumberOfCourseExeption($"Wrong Course Number: {number}");
        }

        Name = number;
    }

    public int Name { get; }
}