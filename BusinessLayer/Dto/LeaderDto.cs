using BusinessLayer.Constants;

namespace BusinessLayer.Dto;

public record LeaderDto(Guid Id,
    string Name,
    int AccessLevel,
    string Login,
    string Password,
    string Role);