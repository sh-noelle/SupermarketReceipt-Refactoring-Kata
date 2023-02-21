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
        private List<ProductQuantity> _items;
        private Dictionary<Product, double> _mockProductQuantities = new Dictionary<Product, double>();
        public ShoppingCartTest(ITestOutputHelper output)
        {
            _output = output;
            _shoppingCart = new ShoppingCart();
            _items = new List<ProductQuantity> 
            {
            new ProductQuantity(new Product("apple",ProductUnit.Kilo), 1.4),
            new ProductQuantity(new Product("watermelon",ProductUnit.Each), 2.0),
            new ProductQuantity(new Product("banana",ProductUnit.Kilo), 1.5),
            };
        }

        [Fact]
        public void ShoppingCart_GetItems_ReturnExactItemCount()
        {
            //arrange
            _shoppingCart._items = _items;
            //action
            List<ProductQuantity> getItemResult = _shoppingCart.GetItems();
            //assert
            Assert.Equal(3, getItemResult.Count);
            _output.WriteLine($"getItemResult's count: {getItemResult.Count}");
        }

        [Fact]
        public void ShoppingCart_AddItem() 
        { 
        }

    }
}