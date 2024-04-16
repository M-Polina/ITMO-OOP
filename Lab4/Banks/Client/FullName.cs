using Banks.Exceptions;

namespace Banks.Client;

public class FullName
{
    public FullName(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BanksException("Name is incorrect, so FullName can't be created.");
        }

        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new BanksException("Surname is incorrect, so FullName can't be created.");
        }

        Surname = surname;
        Name = name;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
}