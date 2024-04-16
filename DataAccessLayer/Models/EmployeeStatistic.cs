namespace DataAccessLayer.Models;

public  record EmployeeStatistic
{
    public EmployeeStatistic(Guid id, Guid employeeId, int number)
    {
        EmployeeId = employeeId;
        Number = number;
        Id = id;
    }
    public EmployeeStatistic() { }
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }
    public int Number { get; set; }
}
