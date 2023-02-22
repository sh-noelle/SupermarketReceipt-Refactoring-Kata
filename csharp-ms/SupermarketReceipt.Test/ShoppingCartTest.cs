using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using SupermarketReceipt;
using Xunit;
using Xunit.Abstractions;

namespace SupermarketReceipt.Test
{
    public class ShoppingCartTest
    {
        private readonly ITestOutputHelper _output;
        private ShoppingCart _shoppingCart;
        private List<ProductQuantity> _mockItems;
        private Dictionary<Product, double> _mockProductQuantities;
        
        public ShoppingCartTest(ITestOutputHelper output)
        {
            _output = output;
            _shoppingCart = new ShoppingCart();
            _mockItems = new List<ProductQuantity> 
            {
            new ProductQuantity(new Product("apple",ProductUnit.Kilo), 1.4),
            new ProductQuantity(new Product("watermelon",ProductUnit.Each), 2.0),
            new ProductQuantity(new Product("banana",ProductUnit.Kilo), 1.5),
            };
            _mockProductQuantities = new Dictionary<Product, double>();
            _mockProductQuantities.Add(new Product("apple", ProductUnit.Kilo), 1.4);
            _mockProductQuantities.Add(new Product("watermelon", ProductUnit.Each), 2.0);
            _mockProductQuantities.Add(new Product("banana", ProductUnit.Kilo), 1.5);
        }

        public static IEnumerable<object[]> AddItemQuantity_Condition1() 
        {
            yield return new object[] { new Product("apple", ProductUnit.Kilo), 3.0, 1.4 };
            yield return new object[] { new Product("watermelon", ProductUnit.Each), 5.0, 2.0 };
            yield return new object[] { new Product("banana", ProductUnit.Kilo), 5.0, 1.5 };
        }

        public static IEnumerable<object[]> AddItemQuantity_Condition2() 
        {
            yield return new object[] { new Product("kiwi", ProductUnit.Each), 3.0 };
            yield return new object[] { new Product("strawberry", ProductUnit.Kilo), 6.0 };
            yield return new object[] { new Product("blueberry", ProductUnit.Kilo), 5.0 };
            yield return new object[] { new Product("candy", ProductUnit.Kilo), 1.0 };
            yield return new object[] { new Product("tofu", ProductUnit.Each), 4.0 };
        }

        [Fact]
        public void ShoppingCart_GetItems_ReturnExactItemCount()
        {
            //arrange
            _shoppingCart._items = _mockItems;
            //action
            List<ProductQuantity> getItemResult = _shoppingCart.GetItems();
            _output.WriteLine($"getItemResult's count: {getItemResult.Count}");
            //assert
            Assert.Equal(3, getItemResult.Count);
            
        }

        [Fact]
        public void ShoppingCart_AddItem() 
        {
            //arrange
            _shoppingCart._items = _mockItems;
            var product = new Product("kiwi", ProductUnit.Each);
            //action
            _shoppingCart.AddItem(product);
            _output.WriteLine($"_shoppingCart.GetItems().Count: {_shoppingCart.GetItems().Count}");
            //assert
            Assert.Equal(4, _shoppingCart.GetItems().Count);
        }

        [Theory]
        [MemberData(nameof(AddItemQuantity_Condition1))]
        public void ShoppingCart_AddItemQuantity_Condition1_ProductQuantitiesContainProductKey(Product product, double addUpQuantity, double originalQuantity) 
        {
            //arrange
            _shoppingCart._productQuantities = _mockProductQuantities;
            //action

            _shoppingCart.AddItemQuantity(product, addUpQuantity);
            var sumOfQuantity = originalQuantity + addUpQuantity;
            _output.WriteLine($"_shoppingCart._productQuantities[product]: {_shoppingCart._productQuantities[product]}");
            //assert
            Assert.Equal(sumOfQuantity, _shoppingCart._productQuantities[product]);
        }

        [Theory]
        [MemberData(nameof(AddItemQuantity_Condition2))]
        public void ShoppingCart_AddItemQuantity_Condition2_ProductQuantitiesDoesContainProductKey(Product product, double quantity) 
        {
            //arrange
            _shoppingCart._items = _mockItems;
            _shoppingCart._productQuantities = _mockProductQuantities;
            //action
            _shoppingCart.AddItemQuantity(product, quantity);
            _output.WriteLine($"_shoppingCart._productQuantities[product]: {_shoppingCart._productQuantities[product]}");
            //assert
            Assert.Equal(quantity, _shoppingCart._productQuantities[product]);
        }
    }
}