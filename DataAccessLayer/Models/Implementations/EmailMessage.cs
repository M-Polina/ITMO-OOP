using DataAccessLayer.Enums;

namespace DataAccessLayer.Models.Implementations;

public class EmailMessage : AbstractMessage
{
    public EmailMessage(Guid id, AbstractMessenger messenger, string theme, string message) : base()
    {
        Id = id;
        Theme = theme;
        StringMessage = message;
        Messenger = messenger;
    }

    public EmailMessage() : base() { }
    public string Theme { get; set; }
    public override string StringMessage { get; set; }
}