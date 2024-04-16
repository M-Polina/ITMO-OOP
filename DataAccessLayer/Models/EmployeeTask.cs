using DataAccessLayer.Enums;

namespace DataAccessLayer.Models;

public class EmployeeTask
{
    public EmployeeTask(Guid id, Guid employeeId, Guid messageId, Guid messengerId, DateTime processingTime, AbstractEmployee employee)
    {
        Id = id;
        MessageId = messageId;
        MessengerId = messengerId;
        EmployeeId = employeeId;
        ProcessingTime = processingTime;
        Employee = employee;
    }

    public EmployeeTask() { Status = EmployeeTaskStatus.NewTask; }

    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid MessengerId { get; set; }
    public DateTime ProcessingTime { get; set; }
    public virtual EmployeeTaskStatus Status { get; set; }
    public virtual AbstractEmployee Employee { get; set; }
}