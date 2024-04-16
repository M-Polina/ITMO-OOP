using DataAccessLayer.Enums;

namespace DataAccessLayer.Models.Implementations;

public abstract class AbstractMessage
{
    public AbstractMessage()
    {
        Status = MessageStatus.NewMessage;
    }
    public Guid Id { get; set; } 
    public virtual MessageStatus Status { get; set; }
    public virtual AbstractMessenger Messenger { get; set; }
    public abstract string StringMessage { get; set; }

}