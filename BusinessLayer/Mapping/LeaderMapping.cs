using BusinessLayer.Constants;
using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class LeaderMapping
{
    public static LeaderDto AsDto(this Leader leader)
        => new LeaderDto(leader.Id, leader.Name, leader.AccessLevel, leader.Login, leader.Password,AccountRole.Leader.ToString("G"));
}