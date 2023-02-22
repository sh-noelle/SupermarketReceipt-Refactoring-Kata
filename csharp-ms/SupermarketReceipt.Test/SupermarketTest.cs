using SupermarketReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SupermarketReceipt.Test
{
    public class SupermarketTest
    {
        private readonly ITestOutputHelper _output;
        private SupermarketCatalog _catalog;
        private ShoppingCart _cart;
        private Teller _teller;
        public SupermarketTest(ITestOutputHelper output) 
        {
            _output = output;
            _catalog = new FakeCatalog();
            _cart = new ShoppingCart();
            _teller = new Teller(_catalog);
        }

        // Originally enum SpecialOfferType do not have GetOneFree & TwentyPercentDiscount.
        // So it is expected test cases related to GetOneFree & TwentyPercentDiscount will fail.
        [Fact]
        public void GetOneFree_oneProductInCart()
        {
            // ARRANGE
            var toothBrush = new Product("toothbrush", ProductUnit.Each);
            var pricePerItem = 0.99;
            var quantity = 2;
            var expectedTotalPrice = pricePerItem * 1;

            _catalog.AddProduct(toothBrush, pricePerItem);
            _cart.AddItemQuantity(toothBrush, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.GetOneFree, toothBrush, 10.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];



            // ASSERT
            Assert.Equal(toothBrush, receiptItem.Product);
            Assert.Equal(pricePerItem, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receiptItem.TotalPrice);
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TwentyPercentDiscount_oneProductInCart() 
        {
            // ARRANGE
            var apples = new Product("apples", ProductUnit.Kilo);
            var pricePerItem = 1.99;
            var quantity = 2.5;
            var expectedDiscount = 0.80;
            var expectedTotalPrice = quantity * pricePerItem * expectedDiscount;

            _catalog.AddProduct(apples, pricePerItem);
            _cart.AddItemQuantity(apples, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TwentyPercentDiscount, apples, 20.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            // ASSERT
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(pricePerItem, receiptItem.Price);
            Assert.Equal(expectedTotalPrice , receiptItem.TotalPrice);
            Assert.Equal(quantity, receiptItem.Quantity);

        }

        [Fact]
        public void TenPercentDiscount_oneProductInCart()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var pricePerItem = 2.49;
            var quantity = 1;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = quantity * pricePerItem * expectedDiscount;
            
            _catalog.AddProduct(rice, pricePerItem);
            _cart.AddItemQuantity(rice, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, rice, 10.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(rice, receiptItem.Product);
            Assert.Equal(pricePerItem, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TenPercentDiscount_severalProductsInCart()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var noodle = new Product("noodle", ProductUnit.Each);
            var UnitPriceOfRice = 2.49;
            var UnitPriceOfNoodle = 2.80;
            var quantityOfRice = 1;
            var quantityOfNoodle = 3;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = 
                ( quantityOfRice * UnitPriceOfRice 
                + quantityOfNoodle * UnitPriceOfNoodle) 
                * expectedDiscount;

            _catalog.AddProduct(rice, UnitPriceOfRice);
            _catalog.AddProduct(noodle, UnitPriceOfNoodle);
            _cart.AddItemQuantity(rice, quantityOfRice);
            _cart.AddItemQuantity(noodle, quantityOfNoodle);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, rice, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, noodle, 10.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(rice, receiptItem.Product);
            Assert.Equal(UnitPriceOfRice, receiptItem.Price);
            Assert.Equal(UnitPriceOfNoodle, receiptItem2.Price);
            Assert.Equal(quantityOfRice, receiptItem.Quantity);
            Assert.Equal(quantityOfNoodle, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Fact]
        public void FiveForAmount_oneProductInCart() 
        {
            // ARRANGE
            var toothPaste = new Product("toothpaste", ProductUnit.Each);
            var pricePerItem = 1.79;
            var quantity = 5;
            var expectedTotalPrice = 7.49;

            _catalog.AddProduct(toothPaste, pricePerItem);
            _cart.AddItemQuantity(toothPaste, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, toothPaste, 7.49);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(toothPaste, receiptItem.Product);
            Assert.Equal(pricePerItem, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void FiveForAmount_severalProductsInCart()
        {
            // ARRANGE
            var toothPaste = new Product("toothpaste", ProductUnit.Each);
            var cupcake = new Product("cupcake", ProductUnit.Each);
            var UnitPriceOfToothPaste = 1.79;
            var UnitPriceOfCupcake = 0.55;
            var quantityOfToothPaste = 5;
            var quantityOfCupcake = 5;
            var expectedTotalPrice = 7.49 + 3.0;

            _catalog.AddProduct(toothPaste, UnitPriceOfToothPaste);
            _catalog.AddProduct(cupcake, UnitPriceOfCupcake);

            _cart.AddItemQuantity(toothPaste, quantityOfToothPaste);
            _cart.AddItemQuantity(cupcake, quantityOfToothPaste);

            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, toothPaste, 7.49);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, cupcake, 3.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(toothPaste, receiptItem.Product);
            Assert.Equal(UnitPriceOfToothPaste, receiptItem.Price);
            Assert.Equal(UnitPriceOfCupcake, receiptItem2.Price);
            Assert.Equal(quantityOfToothPaste, receiptItem.Quantity);
            Assert.Equal(quantityOfCupcake, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }


        [Fact]
        public void TwoForAmount_oneProductInCart() 
        {
            // ARRANGE
            var cherryTomatoes = new Product("cherry tomatoes", ProductUnit.Each);
            var pricePerItem = 0.69;
            var quantity = 2;
            var expectedTotalPrice = 0.99;

            _catalog.AddProduct(cherryTomatoes, pricePerItem);
            _cart.AddItemQuantity(cherryTomatoes, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, cherryTomatoes, 0.99);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(cherryTomatoes, receiptItem.Product);
            Assert.Equal(pricePerItem, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TwoForAmount_severalProductsInCart()
        {
            // ARRANGE
            var cherryTomatoes = new Product("cherry tomatoes", ProductUnit.Each);
            var cherryBlossoms = new Product(",cherry blossoms", ProductUnit.Each);

            var UnitPriceOfCherryTomatoes = 0.69;
            var UnitPriceOfCherryBlossoms = 0.77;
            var quantityOfCherryTomatoes = 2;
            var quantityOfCherryBlossoms = 2;

            var expectedTotalPrice = 0.99 + 1.05;

            _catalog.AddProduct(cherryTomatoes, UnitPriceOfCherryTomatoes);
            _catalog.AddProduct(cherryBlossoms, UnitPriceOfCherryBlossoms);
            _cart.AddItemQuantity(cherryTomatoes, quantityOfCherryTomatoes);
            _cart.AddItemQuantity(cherryBlossoms, quantityOfCherryBlossoms);

            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, cherryTomatoes, 0.99);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, cherryBlossoms, 1.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(cherryTomatoes, receiptItem.Product);
            Assert.Equal(UnitPriceOfCherryTomatoes, receiptItem.Price);
            Assert.Equal(UnitPriceOfCherryBlossoms, receiptItem2.Price);
            Assert.Equal(quantityOfCherryTomatoes , receiptItem.Quantity);
            Assert.Equal(quantityOfCherryBlossoms, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Fact]
        public void NoDiscount_oneProductInCart()
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var UnitPriceOfOrange = 0.69;
            var quantityOfOrange = 2;
            var expectedTotalPrice = UnitPriceOfOrange * quantityOfOrange;

            _catalog.AddProduct(orange, UnitPriceOfOrange);
            _cart.AddItemQuantity(orange, quantityOfOrange);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(UnitPriceOfOrange, receiptItem.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Fact]
        public void NoDiscount_severalProductsInCart() 
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var UnitPriceOfOrange = 0.69;
            var UnitPriceOfLemon = 0.77;
            var quantityOfOrange = 2;
            var quantityOfLemon = 2;
            var expectedTotalPrice = UnitPriceOfOrange * quantityOfOrange + UnitPriceOfLemon * quantityOfLemon;

            _catalog.AddProduct(orange, UnitPriceOfOrange);
            _catalog.AddProduct(lemon, UnitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);


            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(UnitPriceOfOrange, receiptItem.Price);
            Assert.Equal(UnitPriceOfLemon, receiptItem2.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }
    }
}


//Assert.Equal(4.975, receipt.GetTotalPrice());
//Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
//Assert.Single(receipt.GetItems());


// xunit coverage
// 3 worked cases - combine 2 sets of products for specific discounts
// same products - several discounts - does it support?

// tenpercentdiscount - leave two comments
// observe test name , what discount, expect on which item, does it check the ten percent discount outcome?
// what is your thought towards the original test design? reliable? can it check the target outcome?