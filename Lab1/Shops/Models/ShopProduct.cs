using Isu.Exceptions;
using Shops.Entitis;

namespace Shops.Models;

public class ShopProduct
{
    private const int _minPrice = 0;
    private const int _minAmount = 0;

    public ShopProduct(Product product, int price, int amount)
    {
        if (product is null)
        {
            throw new ShopProductCanNotBeCreatedException(
                "Product is null so we can make the set procuct-price-amount");
        }

        if (price < _minPrice)
        {
            throw new ShopProductCanNotBeCreatedException("Product can't have negative price");
        }

        if (amount < _minAmount)
        {
            throw new ShopProductCanNotBeCreatedException(
                "Product can't be contained in a negative number of instances");
        }

        Product = product;
        Price = price;
        Amount = amount;
    }

    public Product Product { get; }
    public int Price { get; private set; }
    public int Amount { get; private set; }

    public void ChangePrice(int newPrice)
    {
        if (newPrice < _minPrice)
        {
            throw new InvalidNewPriceException("Price can't be negative");
        }

        Price = newPrice;
    }

    public void ChangeAmount(int difAmount)
    {
        if (Amount + difAmount < _minAmount)
        {
            throw new InvalidAmountException("Amount can't be negative");
        }

        Amount += difAmount;
    }
}