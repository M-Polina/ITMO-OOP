using BusinessLayer.Dto;

namespace BusinessLayer.Services;

public interface IEmployeeService
{
    Task<LeaderDto> CreateLeaderAccount(string Name, string Login, string Password, CancellationToken cancellationToken);

    Task<OrdinaryEmployeeDto> CreateOrdinaryEmployee(Guid leaderId, string Name, string Login, string Password, int accessLevel,
        CancellationToken cancellationToken);
    
    Task<AbstractEmployeeDto> FindAccount(string name, string password);
    // Task<bool> LogIn(string login, string password, CancellationToken cancellationToken);
    // Task<bool> LogOut(CancellationToken cancellationToken);
}