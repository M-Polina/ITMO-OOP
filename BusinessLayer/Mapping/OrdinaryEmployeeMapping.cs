using BusinessLayer.Constants;
using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class OrdinaryEmployeeMapping
{
    public static OrdinaryEmployeeDto AsDto(this OrdinaryEmployee employee)
        => new OrdinaryEmployeeDto(employee.Id, employee.Name, employee.AccessLevel, employee.Login, employee.Password, employee.Leader.AsDto(),
            AccountRole.OrdinaryEmployee.ToString("G"));
}