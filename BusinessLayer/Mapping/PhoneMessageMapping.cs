using BusinessLayer.Dto;
using DataAccessLayer.Models.Implementations;

namespace BusinessLayer.Mapping;

public static class PhoneMessageMapping
{
    public static PhoneMessageDto AsDto(this PhoneMessage phoneMessage)
        => new PhoneMessageDto(phoneMessage.Id, phoneMessage.Status.ToString(), phoneMessage.StringMessage);
}