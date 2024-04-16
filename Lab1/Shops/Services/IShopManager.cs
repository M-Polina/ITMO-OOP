using Shops.Entitis;
using Shops.Models;

namespace Shops.Services;

public interface IShopManager
{
    Shop CreateShop(Address address, string name);

    public Person CreatePerson(string name, int moneyBefore);

    public void BuyProducts(Shop shop, Person person, List<Order> ordersList);

    public IReadOnlyCollection<ShopProduct> GetShopProductsList(Shop shop);

    Product RegisterProduct(string productName);

    Shop? FindCheapestShop(Order order);

    Shop? FindCheapestShopBigOrder(List<Order> orderList);
}