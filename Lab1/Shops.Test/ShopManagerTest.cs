using Isu.Exceptions;
using Shops.Entitis;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopManagerTest
{
    private ShopManager _shopManager;

    public ShopManagerTest()
    {
        _shopManager = new ShopManager();
    }

    [Fact]
    public void DeliverProductsToShop_ProductIsInShop()
    {
        int moneyBefore = 1000;
        var person = _shopManager.CreatePerson("Andrew", moneyBefore);
        var shop = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var product1 = _shopManager.RegisterProduct("Apple");
        var product2 = _shopManager.RegisterProduct("Pear");

        int productPrice = 40;
        int productCount = 100;
        var shopProduct = new ShopProduct(product1, productPrice, productCount);
        var shopProduct2 = new ShopProduct(product2, productPrice, productCount);
        shop.AddProduct(shopProduct);
        shop.AddProduct(shopProduct2);

        Assert.True(shop.HasProduct(product1));
        Assert.True(shop.HasProduct(product2));
    }

    [Fact]
    public void ChangePriceOfProductInShop()
    {
        int moneyBefore = 1000;
        var person = _shopManager.CreatePerson("Andrew", moneyBefore);

        var shop = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var product = _shopManager.RegisterProduct("Apple");

        int productPrice = 40;
        int productCount = 100;
        var shopProduct = new ShopProduct(product, productPrice, productCount);
        shop.AddProduct(shopProduct);

        int newProductPrice = 20;
        shop.ChangeProductPrice(product, newProductPrice);

        Assert.Equal(newProductPrice, shop.GetPriceOfProduct(product));
    }

    [Fact]
    public void Shop_FindShopWithBestPrice()
    {
        var shop1 = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var shop2 = _shopManager.CreateShop(new Address("Lensovieta 30"), "Shop 2");
        var shop3 = _shopManager.CreateShop(new Address("Lensovieta 31"), "Shop 3");

        var product1 = _shopManager.RegisterProduct("Apple");
        var product2 = _shopManager.RegisterProduct("Pear");

        var shopProduct1 = new ShopProduct(product1, 40, 100);
        var shopProduct2 = new ShopProduct(product2, 50, 150);
        var shopProduct3 = new ShopProduct(product1, 45, 200);
        var shopProduct4 = new ShopProduct(product2, 60, 100);

        shop1.AddProducts(new List<ShopProduct> { shopProduct4, shopProduct1 });
        shop2.AddProducts(new List<ShopProduct> { shopProduct2, shopProduct3 });
        shop3.AddProducts(new List<ShopProduct> { shopProduct2, shopProduct4 });

        Assert.Equal(shop1, _shopManager.FindCheapestShop(new Order(product1, 5)));
        Assert.Equal(shop2, _shopManager.FindCheapestShop(new Order(product2, 4)));
    }

    [Fact]
    public void Shop_FindShopWithBestPriceForOrderList()
    {
        var shop1 = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var shop2 = _shopManager.CreateShop(new Address("Lensovieta 30"), "Shop 2");
        var shop3 = _shopManager.CreateShop(new Address("Lensovieta 31"), "Shop 3");

        var product1 = _shopManager.RegisterProduct("Apple");
        var product2 = _shopManager.RegisterProduct("Pear");

        var shopProduct1 = new ShopProduct(product1, 40, 100);
        var shopProduct2 = new ShopProduct(product2, 50, 150);
        var shopProduct3 = new ShopProduct(product1, 45, 200);
        var shopProduct4 = new ShopProduct(product1, 60, 100);

        shop1.AddProduct(shopProduct1);
        shop1.AddProduct(shopProduct4);
        shop2.AddProduct(shopProduct2);
        shop2.AddProduct(shopProduct3);
        shop3.AddProducts(new List<ShopProduct> { shopProduct2, shopProduct4 });

        var orderList1 = new List<Order>() { new Order(product1, 2), new Order(product2, 2) };
        var orderList2 = new List<Order>() { new Order(product1, 2), new Order(product2, 10000) };

        Assert.Equal(shop2, _shopManager.FindCheapestShopBigOrder(orderList1));
        Assert.Null(_shopManager.FindCheapestShopBigOrder(orderList2));
    }

    [Fact]
    public void BuyProducts_ProductInShopAndPersonCanBuyIt()
    {
        int moneyBefore = 1000;
        var person = _shopManager.CreatePerson("Andrew", moneyBefore);
        var shop = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var product1 = _shopManager.RegisterProduct("Apple");
        var product2 = _shopManager.RegisterProduct("Pear");

        int productPrice = 40;
        int productCount = 100;
        var shopProduct = new ShopProduct(product1, productPrice, productCount);
        var shopProduct2 = new ShopProduct(product2, productPrice, productCount);
        shop.AddProduct(shopProduct);
        shop.AddProduct(shopProduct2);

        int productToBuyCount = 10;
        var order1 = new Order(product1, productToBuyCount);
        var order2 = new Order(product2, productToBuyCount);
        List<Order> orderList = new List<Order>() { order1, order2 };

        shop.Buy(person, orderList);

        Assert.Equal(moneyBefore - ((productPrice * productToBuyCount) * 2), person.GetAmountOfMoney());
        Assert.Equal(productCount - productToBuyCount, shop.GetAmountOfProduct(product1));
        Assert.Equal(productCount - productToBuyCount, shop.GetAmountOfProduct(product2));
    }

    [Fact]
    public void BuyProducts_ProductInShopButNotEnoughToBuy()
    {
        int moneyBefore = 1000;
        var person = _shopManager.CreatePerson("Andrew", moneyBefore);
        var shop = _shopManager.CreateShop(new Address("Lensovieta 29"), "Shop 1");
        var product1 = _shopManager.RegisterProduct("Apple");

        int productPrice = 40;
        int productCount = 100;
        var shopProduct = new ShopProduct(product1, productPrice, productCount);
        shop.AddProduct(shopProduct);

        int productToBuyCount = 1000;
        var order1 = new Order(product1, productToBuyCount);
        List<Order> orderList = new List<Order>() { order1 };

        Assert.Throws<CanNotBuyProductException>(() => _shopManager.BuyProducts(shop, person, orderList));
    }
}