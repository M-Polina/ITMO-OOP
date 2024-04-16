using BusinessLayer.Dto;
using DataAccessLayer.Models;

namespace BusinessLayer.Mapping;

public static class AbstractEmployeeMapping
{
    public static AbstractEmployeeDto AsDto(this AbstractEmployee employee)
        => new AbstractEmployeeDto(employee.Id, employee.Name, employee.AccessLevel, employee.Login, employee.Password);
}