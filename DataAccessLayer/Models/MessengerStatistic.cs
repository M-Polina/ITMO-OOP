namespace DataAccessLayer.Models;

public record MessengerStatistic
{
    public MessengerStatistic(Guid id, Guid messengerId, int number)
    {
        MessengerId = messengerId;
        Number = number;
        Id = id;
    }
    public MessengerStatistic() { }

    public Guid Id { get; set; }

    public Guid MessengerId { get; set; }
    public int Number { get; set; }
}
