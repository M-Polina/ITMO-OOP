namespace BusinessLayer.Dto;

public record OrdinaryEmployeeDto(Guid Id,
    string Name,
    int AccessLevel,
    string Login,
    string Password,
    LeaderDto Leader,
    string Role);
    