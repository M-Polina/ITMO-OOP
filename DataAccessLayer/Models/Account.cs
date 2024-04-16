namespace DataAccessLayer.Models.Implementations;

public class Account
{
    public Account(Guid id, int minAccessLevel)
    {
        Id = id;
        MinAccessLevel = minAccessLevel;
        Messengers = new List<AbstractMessenger>();
    }

    public Account() { }
    public Guid Id { get; set; }
    public int MinAccessLevel { get; set; }
    public virtual ICollection<AbstractMessenger> Messengers { get; set; }
}