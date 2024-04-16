namespace DataAccessLayer.Models.Implementations;

public class PhoneMessage: AbstractMessage
{
    public PhoneMessage(Guid id, AbstractMessenger messenger, string message): base()
    {
        Id = id;
        StringMessage = message;
        Messenger = messenger;
    }

    public PhoneMessage() : base() { }
    public override string StringMessage { get; set; }
}