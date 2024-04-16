using Isu.Exceptions;

namespace Shops.Entitis;

public class Product
{
    public Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ProductCanNotBeCreatedException("Name is incorrect, so person can't be created.");
        }

        Name = name;
    }

    public string Name { get; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return Name == ((Product)obj).Name;
    }

    public override int GetHashCode() => Name.GetHashCode();
}