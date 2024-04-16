namespace PresentationLayer.Models;

public record CreateEmailModel(Guid AccountId, string Name) : AbstractCreatorMessengerModel;