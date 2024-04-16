using Banks.Exceptions;

namespace Banks.Client;

public class Passport
{
    private const int MinId = 0;

    public Passport(int id)
    {
        if (id < MinId)
            throw new BanksException("Id is incorrect, so Passport can't be created.");

        Id = id;
    }

    public int Id { get; private set; }
}