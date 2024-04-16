using Isu.Exceptions;
using Shops.Entitis;

namespace Shops.Models;

public class Order
{
    private const int _minAmount = 0;

    public Order(Product product, int amount)
    {
        if (product is null)
        {
            throw new OrderCanNotBeCreatedException("Product is null so we can make the sorder");
        }

        if (amount < _minAmount)
        {
            throw new OrderCanNotBeCreatedException("Product can't be contained in a negative number of instances");
        }

        Product = product;
        Amount = amount;
    }

    public Product Product { get; }
    public int Amount { get; }
}