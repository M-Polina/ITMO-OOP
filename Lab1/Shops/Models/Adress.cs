using Isu.Exceptions;

namespace Shops.Models;

public class Address
{
    public Address(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new AddressCanNotBeCreatedException("Address is incorrect.");
        }

        Value = address;
    }

    public string Value { get; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return Value == ((Address)obj).Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}