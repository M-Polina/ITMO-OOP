using DataAccessLayer.Models;

namespace BusinessLayer.Dto;

public record ReportDto(
    Guid ReportId,
    List<MessengerStatistic> MessengerStatistic,
    List<EmployeeStatistic>  EmployeeStatistic,
    int AllMessagesStatistic,
    string CreationTime);