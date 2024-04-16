using Isu.Exceptions;
using Shops.Models;

namespace Shops.Entitis;

public class Shop
{
    private const int _initialAmountOfMoney = 0;

    private List<ShopProduct> _shopProductsList;
    private Wallet _wallet;

    public Shop(uint id, Address address, string name)
    {
        if (address is null)
        {
            throw new ShopCanNotBeCreatedException("Adress is incorrect (null), so shop can't be created.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ShopCanNotBeCreatedException("Name is incorrect, so shop can't be created.");
        }

        _shopProductsList = new List<ShopProduct>();

        Id = id;
        Address = address;
        Name = name;
        _wallet = new Wallet(_initialAmountOfMoney);
    }

    public uint Id { get; }
    public Address Address { get; }
    public string Name { get; }

    public IReadOnlyCollection<ShopProduct> ShopProductsList => _shopProductsList;

    public ShopProduct? FindShopProduct(Product product)
    {
        return _shopProductsList.SingleOrDefault(shopProduct => shopProduct.Product.Equals(product));
    }

    public bool HasProduct(Product product)
    {
        return _shopProductsList.Any(shopProduct => shopProduct.Product.Equals(product));
    }

    public bool HasOrders(List<Order> orderList)
    {
        bool hasOrder = true;
        Product product;
        ShopProduct? foundProduct;

        foreach (var order in orderList)
        {
            product = order.Product;
            foundProduct = _shopProductsList.SingleOrDefault(shopProduct => shopProduct.Product.Equals(product));

            if (foundProduct is null)
            {
                hasOrder = false;
                break;
            }
            else
            {
                if (foundProduct.Amount < order.Amount)
                {
                    hasOrder = false;
                    break;
                }
            }
        }

        return hasOrder;
    }

    public int GetAmountOfProduct(Product product)
    {
        ShopProduct? currentShopProduct = FindShopProduct(product);

        if (currentShopProduct is null)
            throw new ArgumentException("Product can't be null when finding it's ammount.");

        return currentShopProduct.Amount;
    }

    public int GetPriceOfProduct(Product product)
    {
        ShopProduct? currentShopProduct = FindShopProduct(product);

        if (currentShopProduct is null)
            throw new ArgumentException("Product can't be null when finding it's ammount.");

        return currentShopProduct.Price;
    }

    public int GetPriceOfOreder(List<Order> orderList)
    {
        int orederPrice = 0;

        Product product;
        ShopProduct? foundProduct;

        foreach (var order in orderList)
        {
            product = order.Product;
            foundProduct = _shopProductsList.SingleOrDefault(shopProduct => shopProduct.Product.Equals(product));

            if (foundProduct is null)
                throw new ArgumentException("Product can't be null when finding orders price.");

            orederPrice = foundProduct.Price * order.Amount;
        }

        return orederPrice;
    }

    public void ChangeProductPrice(Product product, int newPrice)
    {
        ShopProduct? currentShopProduct = FindShopProduct(product);

        if (currentShopProduct is null)
        {
            throw new ProductPriceCanNotBeChangedException("This shop has no products like this");
        }

        currentShopProduct.ChangePrice(newPrice);
    }

    public void AddProduct(ShopProduct supply)
    {
        ShopProduct? currentShopProduct = FindShopProduct(supply.Product);

        if (currentShopProduct is null)
        {
            _shopProductsList.Add(supply);
        }
        else
        {
            currentShopProduct.ChangeAmount(supply.Amount);
        }
    }

    public void AddProducts(List<ShopProduct> addedProductsList)
    {
        foreach (var supply in addedProductsList)
        {
            AddProduct(supply);
        }
    }

    public void Buy(Person person, List<Order> ordersList)
    {
        if (person is null)
        {
            throw new CanNotBuyProductException("Person is null so he can't buy products.");
        }

        List<Order> orderList = ordersList.GroupBy(order => order.Product)
            .Select(group => new Order(group.Key, group.Sum(order => order.Amount)))
            .ToList();

        int moneyToWriteOff = 0;

        foreach (Order order in ordersList)
        {
            ShopProduct? foundShopProduct = FindShopProduct(order.Product);

            if (foundShopProduct is null)
            {
                throw new CanNotBuyProductException($"There is no product {order.Product.Name} in the shop.");
            }

            if (order.Amount > foundShopProduct.Amount)
            {
                throw new CanNotBuyProductException($"There is not enough products {order.Product.Name} in the shop.");
            }

            moneyToWriteOff += foundShopProduct.Price * order.Amount;
        }

        person.WriteOffMoney(moneyToWriteOff);
        _wallet.DipositMoney(moneyToWriteOff);

        foreach (Order order in ordersList)
        {
            ShopProduct? foundShopProduct = FindShopProduct(order.Product);

            if (foundShopProduct is null)
            {
                throw new CanNotBuyProductException($"There is no product {order.Product.Name} in the shop.");
            }

            foundShopProduct.ChangeAmount(-order.Amount);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        return Id == ((Shop)obj).Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}