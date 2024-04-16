using BusinessLayer.Dto;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class MessengerMapping
{
    public static MessengerDto AsDto(this AbstractMessenger messenger)
        => new MessengerDto(messenger.Id, messenger.Account.AsDto(), messenger.GetType().ToString());
}