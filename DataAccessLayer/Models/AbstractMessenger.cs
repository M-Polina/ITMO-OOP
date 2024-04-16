using DataAccessLayer.Models.Implementations;

namespace DataAccessLayer.Models;

public abstract class AbstractMessenger
{
    public AbstractMessenger()
    {
        Messages = new List<AbstractMessage>();
    }
    public Guid Id { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<AbstractMessage> Messages { get; set; }
}