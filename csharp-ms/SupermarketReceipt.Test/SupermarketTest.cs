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
        public void GetOneFree_oneProductInCart_ReturnsGetOneFreeValue()
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
        public void TwentyPercentDiscount_oneProductInCart_ReturnsTwentyPercentDiscountValue() 
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
        public void TenPercentDiscount_oneProductInCart_ReturnsTenPercentDiscountValue()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var pricePerItem = 2.49;
            var quantity = 1;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(quantity * pricePerItem * expectedDiscount, 3);
            
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
        public void TenPercentDiscount_severalProductsInCart_ReturnsTenPercentDiscountValue()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var noodle = new Product("noodle", ProductUnit.Each);
            var UnitPriceOfRice = 2.49;
            var UnitPriceOfNoodle = 2.80;
            var quantityOfRice = 1;
            var quantityOfNoodle = 3;
            var expectedDiscount = 0.90;
            var expectedTotalPrice =Math.Round(
                (quantityOfRice * UnitPriceOfRice
                + quantityOfNoodle * UnitPriceOfNoodle)
                * expectedDiscount, 3);

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
        public void FiveForAmount_oneProductInCart_ReturnsFiveForAmountValue() 
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
        public void FiveForAmount_severalProductsInCart_ReturnsFiveForAmountValue()
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
        public void TwoForAmount_oneProductInCart_ReturnsTwoForAmountValue() 
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
        public void TwoForAmount_severalProductsInCart_ReturnsTwoForAmountValue()
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
        public void NoDiscount_oneProductInCart_ReturnNoDiscountValue()
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
        public void NoDiscount_severalProductsInCart_ReturnNoDiscountValue() 
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

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        public void MixedOffer_AllSameItems_TenPercentANDTwoForAmount_AllReturnsTwoForAmountValue(int testedQuantity)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var unitPrice = 0.69;
            var quantity = testedQuantity;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(0.99 * (quantity / 2) + (quantity % 2) * unitPrice, 3); 
            // the calculation bases on the sequence of if/else statement in HandleOffers method 
            // TwoForAmount -> TenPercentDiscount


            _catalog.AddProduct(orange, unitPrice);
            _cart.AddItemQuantity(orange, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, orange, 0.99);
            
            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(quantity,receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        public void MixedOffer_AllSameItems_TenPercentANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var unitPrice = 0.69;
            var quantity = testedQuantity;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(1.65 * (quantity / 5) + (quantity % 5) * unitPrice, 3);
            // the calculation bases on the sequence of if/else statement in HandleOffers method 
            // FiveForAmount -> TenPercentDiscount


            _catalog.AddProduct(orange, unitPrice);
            _cart.AddItemQuantity(orange, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, orange, 1.65);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(quantity, receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        public void MixedOffer_AllSameItems_TwoForAmountANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var unitPrice = 0.69;
            var quantity = testedQuantity;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(1.65 * (quantity / 5) + (quantity % 5) * unitPrice,3);
            // the calculation bases on the sequence of if/else statement in HandleOffers method 
            // FiveForAmount -> TwoForAmount


            _catalog.AddProduct(orange, unitPrice);
            _cart.AddItemQuantity(orange, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, orange, 0.99);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, orange, 1.65);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(quantity, receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        public void MixedOffer_AllSameItems_TenPercentDiscountANDTwoForAmountANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var unitPrice = 0.69;
            var quantity = testedQuantity;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(1.65 * (quantity / 5) + (quantity % 5) * unitPrice, 3);
            // the calculation bases on the sequence of if/else statement in HandleOffers method 
            // FiveForAmount -> TwoForAmount/ TenPercentDiscount


            _catalog.AddProduct(orange, unitPrice);
            _cart.AddItemQuantity(orange, quantity);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, orange, 0.99);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, orange, 1.65);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"quantity: {quantity}; receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(quantity, receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(5, 6)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        [InlineData(8, 9)]
        [InlineData(9, 10)]
        [InlineData(10, 11)]
        [InlineData(12, 1)]
        public void MixedOffer_HalfTenPercent_HalfTenPercentANDTwoForAmount_HalfReturnsTenDisCount_HalfReturnsTwoForAmountValue(int testedQ1, int testedQ2)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = testedQ1;
            var quantityOfLemon = testedQ2;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange * expectedDiscount 
                                    + 1.05 * (quantityOfLemon / 2) + (quantityOfLemon % 2) * unitPriceOfLemon,3);

            // automatically round up to 3 decimal places
            // no blocking on cases like [1, 1] or [12,1]

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, lemon, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, lemon, 1.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(5, 6)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        [InlineData(8, 9)]
        [InlineData(9, 10)]
        [InlineData(10, 11)]
        [InlineData(12, 1)]
        public void MixedOffer_HalfTenPercent_HalfTenPercentANDFiveForAmount__HalfReturnsTenDisCount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = testedQ1;
            var quantityOfLemon = testedQ2;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange * expectedDiscount
                                    + 3.05 * (quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);

            // automatically round up to 3 decimal places
            // no blocking on cases like [1, 1] or [12,1]

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, lemon, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, lemon, 3.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(5, 6)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        [InlineData(8, 9)]
        [InlineData(9, 10)]
        [InlineData(10, 11)]
        [InlineData(12, 1)]
        public void MixedOffer_HalfFiveForAmount_HalfTenPercentANDTwoForAmount__HalfReturnsFiveForAmount_HalfReturnsTwoForAmountValue(int testedQ1, int testedQ2)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = testedQ1;
            var quantityOfLemon = testedQ2;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(4.0 * (quantityOfOrange / 5) + (quantityOfOrange % 5) * unitPriceOfOrange
                                    + 1.05 * (quantityOfLemon / 2) + (quantityOfLemon % 2) * unitPriceOfLemon, 3);

            // automatically round up to 3 decimal places
            // no blocking on cases like [1, 5], [1, 1] or [12,1]

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, orange, 4.0);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, lemon, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, lemon, 1.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(5, 6)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        [InlineData(8, 9)]
        [InlineData(9, 10)]
        [InlineData(10, 11)]
        [InlineData(12, 1)]
        public void MixedOffer_HalfTwoForAmount_HalfTenPercentANDFiveForAmount__HalfReturnsTwoForAmount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = testedQ1;
            var quantityOfLemon = testedQ2;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(1.02 * (quantityOfOrange / 2) + (quantityOfOrange % 2) * unitPriceOfOrange
                                    + 3.05 * (quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);
            // automatically round up to 3 decimal places
            // no blocking on cases like [1, 5], [1, 1] or [12,1]

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, orange, 1.02);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, lemon, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, lemon, 3.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 9)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(5, 6)]
        [InlineData(6, 7)]
        [InlineData(7, 8)]
        [InlineData(8, 9)]
        [InlineData(9, 10)]
        [InlineData(10, 11)]
        [InlineData(12, 1)]
        public void MixedOffer_HalfTenPercentANDTwoForAmount_HalfTenPercentANDFiveForAmount__HalfReturnsTwoForAmount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
        {
            //ten percent +TwoForAmount; itemB: ten percent +FiveForAmount
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var lemon = new Product("lemon", ProductUnit.Each);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = testedQ1;
            var quantityOfLemon = testedQ2;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(1.02 * (quantityOfOrange / 2) + (quantityOfOrange % 2) * unitPriceOfOrange
                                    + 3.05 * (quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);
            // automatically round up to 3 decimal places
            // no blocking on cases like [1, 5], [1, 1] or [12,1]

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, orange, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, orange, 1.02);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, lemon, 10.0);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, lemon, 3.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
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
// same products - several discounts - does it support?

// 5-10 same items
//// a) ten percent + TwoForAmount  [set]
//// b) ten percent + FiveForAmount  [set]
//// c) TwoForAmount + FiveForAmount  [set]
//// d) ten percent + TwoForAmount + FiveForAmount  [set]
///
// (5-10) same itemAs + (5-10) same itemBs (where x%2 or x%5 equals to 0)
//// a) itemA:  ten percent ; itemB: ten percent + TwoForAmount   [set]
//// b) itemA:  ten percent ; itemB: ten percent + FiveForAmount   [set]
//// c) itemA:  FiveForAmount ; itemB: ten percent + TwoForAmount   [set]
//// d) itemA:  TwoForAmount ; itemB: ten percent + FiveForAmount   [set]
/////e) itemA:  ten percent + TwoForAmount ; itemB: ten percent + FiveForAmount   [set]
///


// tenpercentdiscount - leave two comments
// observe test name , what discount, expect on which item, does it check the ten percent discount outcome?
// what is your thought towards the original test design? reliable? can it check the target outcome?