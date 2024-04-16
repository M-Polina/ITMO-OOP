using Banks.Exceptions;

namespace Banks.Client;

public class Address
{
    public Address(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BanksException("Address is incorrect, so Address can't be created.");
        }

        Value = address;
    }

    public string Value { get; private set; }
}