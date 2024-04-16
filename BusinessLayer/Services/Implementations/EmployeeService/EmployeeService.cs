using BusinessLayer.Dto;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapping;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implementations.EmployeeService;

public class EmployeeService : IEmployeeService
{
    private const int MinNumber = 0;
    private readonly DatabaseContext _context;

    public EmployeeService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<AbstractEmployeeDto> FindAccount(string login, string password)
    {
        ArgumentNullException.ThrowIfNull(login, nameof(login));
        ArgumentNullException.ThrowIfNull(password, nameof(password));


        AbstractEmployee employee = await _context.Employees.SingleOrDefaultAsync(x => x.Login == login && x.Password == password);

        return employee.AsDto();
    }
    
    public async Task<LeaderDto> CreateLeaderAccount(string name, string login, string password,
        CancellationToken cancellationToken)
    {
        if (name is null)
            throw new BusinessLayerException("null Name while creating leader.");

        if (login is null)
            throw new BusinessLayerException("Null login while creating Leader.");

        if (password is null)
            throw new BusinessLayerException("Null login while creating Leader.");

        bool unique = false;
        
        var foundEmployee = await _context.Employees.SingleOrDefaultAsync(em => em.Login.Equals(login));
        if (foundEmployee is null)
            unique = true;

        if (!unique)
            throw new BusinessLayerException("This login already exist. Leader can't be created.");

        Leader leader = new Leader(Guid.NewGuid(), name, login, password);
        _context.Employees.Add(leader);
        await _context.SaveChangesAsync(cancellationToken);


        return leader.AsDto();
    }

    public async Task<OrdinaryEmployeeDto> CreateOrdinaryEmployee(Guid leaderId, string name, string login, string password,
        int accessLevel,
        CancellationToken cancellationToken)
    {
        
        var leaderFound = await _context.Employees.SingleOrDefaultAsync(em => em.Id.Equals(leaderId));

        if (leaderFound is null)
            throw new BusinessLayerException("Not signed in while adding worker.");

        if (leaderFound is not Leader)
            throw new BusinessLayerException("Only Leader can create employees.");
        
        Leader leader = leaderFound as Leader;

        if (name is null)
            throw new BusinessLayerException("null Name while creating leader.");

        if (login is null)
            throw new BusinessLayerException("Null login while creating Leader.");

        if (password is null)
            throw new BusinessLayerException("Null login while creating Leader.");

        if (accessLevel <= MinNumber)
            throw new BusinessLayerException("accessLevel can't be < 0.");

        bool unique = false;
        var foundEmployee = await _context.Employees.SingleOrDefaultAsync(em => em.Login.Equals(login));
        if (foundEmployee is null)
            unique = true;

        if (!unique)
            throw new BusinessLayerException("This login already exist. Employee can't be created.");
        
        OrdinaryEmployee newEmployee =
            new OrdinaryEmployee(Guid.NewGuid(), name, accessLevel, login, password, leader);

        leader.Employees.Add(newEmployee);
        
        await _context.Employees.AddAsync(newEmployee);
        await _context.SaveChangesAsync(cancellationToken);

        return newEmployee.AsDto();
    }
}