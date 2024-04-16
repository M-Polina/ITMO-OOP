using Isu.Exceptions;
using Shops.Entitis;
using Shops.Models;

namespace Shops.Services;

public class ShopManager : IShopManager
{
    private uint _shopId = 0;
    private uint _personId = 0;
    private List<Product> _productsList;
    private List<Person> _personsList;
    private List<Shop> _shopsList;

    public ShopManager()
    {
        _productsList = new List<Product>();
        _personsList = new List<Person>();
        _shopsList = new List<Shop>();
    }

    public IReadOnlyCollection<Product> ProductsList => _productsList;
    public IReadOnlyCollection<Shop> ShopsList => _shopsList;
    public IReadOnlyCollection<Person> PersonsList => _personsList;

    public Person? FindPersonById(uint id) => _personsList.SingleOrDefault(p => p.Id == id);
    public Shop? FindShopById(uint id) => _shopsList.SingleOrDefault(s => s.Id == id);

    public Shop CreateShop(Address address, string name)
    {
        var isNotUnique = ShopAlreadyHasAdress(address);

        if (isNotUnique)
        {
            throw new ShopCanNotBeCreatedException(
                "The shop with this address already exists, so new shop can't be created.");
        }

        var newShop = new Shop(_shopId, address, name);
        _shopsList.Add(newShop);
        _shopId += 1;
        return newShop;
    }

    public Person CreatePerson(string name, int moneyBefore)
    {
        var newPerson = new Person(name, _personId, moneyBefore);
        _personsList.Add(newPerson);
        _personId += 1;
        return newPerson;
    }

    public Product RegisterProduct(string productName)
    {
        Product? product = FindProductByName(productName);

        if (product is not null)
        {
            throw new ProductCanNotBeRegisteredException("This product already exists.");
        }

        var newProduct = new Product(productName);
        _productsList.Add(newProduct);

        return newProduct;
    }

    public IReadOnlyCollection<ShopProduct> GetShopProductsList(Shop shop)
    {
        Shop? foundShop = _shopsList.SingleOrDefault(s => s.Equals(shop));

        if (foundShop is null)
        {
            throw new CanNotBuyProductException("Shop is null so we can't find products.");
        }

        return shop.ShopProductsList;
    }

    public void BuyProducts(Shop shop, Person person, List<Order> ordersList)
    {
        Shop? foundShop = _shopsList.SingleOrDefault(s => s.Equals(shop));

        if (foundShop is null)
        {
            throw new CanNotBuyProductException("Shop is null so person can't buy products.");
        }

        Person? foundPerson = FindPersonById(person.Id);

        if (foundPerson is null)
        {
            throw new CanNotBuyProductException("Person is null so he can't buy products.");
        }

        shop.Buy(person, ordersList);
    }

    public Shop? FindCheapestShop(Order order)
    {
        Product product = order.Product;

        Shop? bestShop = _shopsList.FindAll(shop => shop.HasProduct(product))
            .FindAll(shop => shop.GetAmountOfProduct(product) >= order.Amount)
            .MinBy(shop => shop.GetPriceOfProduct(product));

        return bestShop;
    }

    public Shop? FindCheapestShopBigOrder(List<Order> orderList)
    {
        Shop? bestShop = _shopsList.FindAll(shop => shop.HasOrders(orderList))
            .MinBy(shop => shop.GetPriceOfOreder(orderList));

        return bestShop;
    }

    private Product? FindProductByName(string name)
    {
        return _productsList.SingleOrDefault(p => p.Name.Equals(name));
    }

    private Product? FindProduct(Product product)
    {
        return _productsList.SingleOrDefault(p => p.Equals(product));
    }

    private bool ShopAlreadyHasAdress(Address address) => _shopsList.Any(shop => shop.Address.Equals(address));
}