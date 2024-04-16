namespace PresentationLayer.Models;

public record CreateMobilePhoneModel(Guid AccountId, int PhoneNumber) : AbstractCreatorMessengerModel;