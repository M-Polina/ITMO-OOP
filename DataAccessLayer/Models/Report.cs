using DataAccessLayer.Enums;
using DataAccessLayer.Models.Implementations;

namespace DataAccessLayer.Models;

public class Report
{
    public Report(
        Guid id,
        int allMessagesStatistic,
        List<MessengerStatistic> messengerStatistic,
        List<EmployeeStatistic> employeeStatistic,
        DateTime time)
    {
        Id = id;
        CreationDate = time;
        AllMessagesStatistic = allMessagesStatistic;
        MessengerStatistic = messengerStatistic;
        EmployeeStatistic = employeeStatistic;
        Tasks = new List<EmployeeTask>();
    }

    public Report() { }
    public int AllMessagesStatistic { get; set; }
    public Guid Id { get; set; }
    public virtual DateTime CreationDate { get; set; }
    public virtual ICollection<EmployeeTask> Tasks { get; set; }
    public virtual List<MessengerStatistic> MessengerStatistic { get; set; }
    public virtual List<EmployeeStatistic> EmployeeStatistic { get; set; }
}