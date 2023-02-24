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
        private Dictionary<Product, Offer> _mockOffers;
        private SpecialOffers _buyOneGetOneFree = SpecialOffers.BuyItemsGetItemsFree;
        private SpecialOffers _tenPercentDiscount = SpecialOffers.SpecificPercentDiscount;
        private SpecialOffers _twentyPercentDiscount = SpecialOffers.SpecificPercentDiscount;
        private SpecialOffers _twoItemsForSale = SpecialOffers.ItemsForSales;
        private SpecialOffers _fiveItemsForSale = SpecialOffers.ItemsForSales;
        private SpecialOffers _noDiscount = SpecialOffers.NoDiscount;
        public SupermarketTest(ITestOutputHelper output) 
        {
            _output = output;
            _catalog = new FakeCatalog();
            _cart = new ShoppingCart();
            _teller = new Teller(_catalog);
            _mockOffers = new Dictionary<Product, Offer>();
        }

        // Originally enum SpecialOfferType do not have GetOneFree & TwentyPercentDiscount.
        // So it is expected test cases related to GetOneFree & TwentyPercentDiscount will fail.
        [Fact]
        public void GetOneFree_oneProductInCart_ReturnsGetOneFreeValue()
        {
            // ARRANGE
            var toothBrush = new Product("toothbrush", ProductUnit.Each);
            var unitPrice = 0.99;
            var quantity = 2;
            var expectedTotalPrice = unitPrice * 1;

            _catalog.AddProduct(toothBrush, unitPrice);
            _cart.AddItemQuantity(toothBrush, quantity);
            _teller.AddSpecialOffer(_buyOneGetOneFree, toothBrush, 2, 0, 0.99);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];



            // ASSERT
            Assert.Equal(toothBrush, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TwentyPercentDiscount_oneKindProductInCart_ReturnsTwentyPercentDiscountValue() 
        {
            // ARRANGE
            var apples = new Product("apples", ProductUnit.Kilo);
            var unitPrice = 1.99;
            var quantity = 1;
            var expectedDiscount = 0.80;
            var expectedTotalPrice = Math.Round(quantity * unitPrice * expectedDiscount, 3);

            _catalog.AddProduct(apples, unitPrice);
            _cart.AddItemQuantity(apples, quantity);
            _teller.AddSpecialOffer(_twentyPercentDiscount, apples, 1, 0.20, 0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(apples, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);

        }

        [Fact]
        public void TenPercentDiscount_oneKindProductInCart_ReturnsTenPercentDiscountValue()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var unitPrice = 2.49;
            var quantity = 1;
            var expectedDiscount = 0.90;
            var expectedTotalPrice = Math.Round(quantity * unitPrice * expectedDiscount, 3);
            
            _catalog.AddProduct(rice, unitPrice);
            _cart.AddItemQuantity(rice, quantity);
            _teller.AddSpecialOffer(_tenPercentDiscount, rice, 1, 0.10, 0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(rice, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TenPercentDiscount_severalKindsProductsInCart_ReturnsTenPercentDiscountValue()
        {
            // ARRANGE
            var rice = new Product("rice", ProductUnit.Each);
            var noodle = new Product("noodle", ProductUnit.Each);
            var unitPriceOfRice = 2.49;
            var unitPriceOfNoodle = 2.80;
            var quantityOfRice = 1;
            var quantityOfNoodle = 3;
            var expectedDiscount = 0.90;
            var expectedTotalPrice =Math.Round(
                (quantityOfRice * unitPriceOfRice
                + quantityOfNoodle * unitPriceOfNoodle)
                * expectedDiscount, 3);

            _catalog.AddProduct(rice, unitPriceOfRice);
            _catalog.AddProduct(noodle, unitPriceOfNoodle);
            _cart.AddItemQuantity(rice, quantityOfRice);
            _cart.AddItemQuantity(noodle, quantityOfNoodle);
            _teller.AddSpecialOffer(_tenPercentDiscount, rice, 1, 0.10, 0);
            _teller.AddSpecialOffer(_tenPercentDiscount, noodle, 1, 0.10, 0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(rice, receiptItem.Product);
            Assert.Equal(unitPriceOfRice, receiptItem.Price);
            Assert.Equal(unitPriceOfNoodle, receiptItem2.Price);
            Assert.Equal(quantityOfRice, receiptItem.Quantity);
            Assert.Equal(quantityOfNoodle, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Fact]
        public void FiveForAmount_oneKindProductInCart_ReturnsFiveForAmountValue() 
        {
            // ARRANGE
            var toothPaste = new Product("toothpaste", ProductUnit.Each);
            var unitPrice = 1.79;
            var quantity = 5;
            var sellingPrice = 3.50;
            var expectedTotalPrice = sellingPrice * 1;

            _catalog.AddProduct(toothPaste, unitPrice);
            _cart.AddItemQuantity(toothPaste, quantity);
            _teller.AddSpecialOffer(_fiveItemsForSale, toothPaste, 5, 0, 3.50);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(toothPaste, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void FiveForAmount_severalKindsProductsInCart_ReturnsFiveForAmountValue()
        {
            // ARRANGE
            var toothPaste = new Product("toothpaste", ProductUnit.Each);
            var cupcake = new Product("cupcake", ProductUnit.Each);
            var unitPriceOfToothPaste = 1.79;
            var unitPriceOfCupcake = 0.55;
            var quantityOfToothPaste = 5;
            var quantityOfCupcake = 5;
            var expectedTotalPrice = 7.49 + 3.0;

            _catalog.AddProduct(toothPaste, unitPriceOfToothPaste);
            _catalog.AddProduct(cupcake, unitPriceOfCupcake);

            _cart.AddItemQuantity(toothPaste, quantityOfToothPaste);
            _cart.AddItemQuantity(cupcake, quantityOfToothPaste);

            _teller.AddSpecialOffer(_fiveItemsForSale, toothPaste, 5, 0, 7.49);
            _teller.AddSpecialOffer(_fiveItemsForSale, cupcake, 5, 0, 3.0);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(toothPaste, receiptItem.Product);
            Assert.Equal(unitPriceOfToothPaste, receiptItem.Price);
            Assert.Equal(unitPriceOfCupcake, receiptItem2.Price);
            Assert.Equal(quantityOfToothPaste, receiptItem.Quantity);
            Assert.Equal(quantityOfCupcake, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }


        [Fact]
        public void TwoForAmount_oneKindProductInCart_ReturnsTwoForAmountValue() 
        {
            // ARRANGE
            var cherryTomatoes = new Product("cherry tomatoes", ProductUnit.Each);
            var unitPrice = 0.69;
            var quantity = 2;
            var expectedTotalPrice = 0.99;

            _catalog.AddProduct(cherryTomatoes, unitPrice);
            _cart.AddItemQuantity(cherryTomatoes, quantity);
            _teller.AddSpecialOffer(_twoItemsForSale, cherryTomatoes, 2, 0, 0.99);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(cherryTomatoes, receiptItem.Product);
            Assert.Equal(unitPrice, receiptItem.Price);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
            Assert.Equal(quantity, receiptItem.Quantity);
        }

        [Fact]
        public void TwoForAmount_severalKindsProductsInCart_ReturnsTwoForAmountValue()
        {
            // ARRANGE
            var cherryTomatoes = new Product("cherry tomatoes", ProductUnit.Each);
            var cherryBlossoms = new Product(",cherry blossoms", ProductUnit.Each);

            var unitPriceOfCherryTomatoes = 0.69;
            var unitPriceOfCherryBlossoms = 0.77;
            var quantityOfCherryTomatoes = 2;
            var quantityOfCherryBlossoms = 2;

            var expectedTotalPrice = 0.99 + 1.05;

            _catalog.AddProduct(cherryTomatoes, unitPriceOfCherryTomatoes);
            _catalog.AddProduct(cherryBlossoms, unitPriceOfCherryBlossoms);
            _cart.AddItemQuantity(cherryTomatoes, quantityOfCherryTomatoes);
            _cart.AddItemQuantity(cherryBlossoms, quantityOfCherryBlossoms);

            _teller.AddSpecialOffer(_twoItemsForSale, cherryTomatoes, 2, 0, 0.99);
            _teller.AddSpecialOffer(_twoItemsForSale, cherryBlossoms, 2, 0, 1.05);

            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];
            var receiptItem2 = receipt.GetItems()[1];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(cherryTomatoes, receiptItem.Product);
            Assert.Equal(unitPriceOfCherryTomatoes, receiptItem.Price);
            Assert.Equal(unitPriceOfCherryBlossoms, receiptItem2.Price);
            Assert.Equal(quantityOfCherryTomatoes , receiptItem.Quantity);
            Assert.Equal(quantityOfCherryBlossoms, receiptItem2.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(99)]
        [InlineData(100)]
        public void NoDiscount_oneKindProductInCart_ReturnNoDiscountValue(double quantity1)
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Each);
            var unitPriceOfOrange = 0.69;
            var quantityOfOrange = quantity1;
            var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange, 3);

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _teller.AddSpecialOffer(_noDiscount, orange, 1, 0, 0);
            // ACT
            var receipt = _teller.ChecksOutArticlesFrom(_cart);
            var receiptItem = receipt.GetItems()[0];

            _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

            // ASSERT
            Assert.Equal(orange, receiptItem.Product);
            Assert.Equal(unitPriceOfOrange, receiptItem.Price);
            Assert.Equal(quantityOfOrange, receiptItem.Quantity);
            Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        [InlineData(3,3)]
        [InlineData(4,4)]
        [InlineData(5, 5)]
        [InlineData(10, 10)]
        [InlineData(11, 11)]
        [InlineData(99, 99)]
        [InlineData(100, 100)]
        [InlineData(1.5, 1.5)]
        [InlineData(2.5, 3.5)]
        public void NoDiscount_severalKindsProductsInCart_ReturnNoDiscountValue(double quantity1, double quantity2) 
        {
            // ARRANGE
            var orange = new Product("orange", ProductUnit.Kilo);
            var lemon = new Product("lemon", ProductUnit.Kilo);

            var unitPriceOfOrange = 0.69;
            var unitPriceOfLemon = 0.77;
            var quantityOfOrange = quantity1;
            var quantityOfLemon = quantity2;
            var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange + unitPriceOfLemon * quantityOfLemon, 3);

            _catalog.AddProduct(orange, unitPriceOfOrange);
            _catalog.AddProduct(lemon, unitPriceOfLemon);
            _cart.AddItemQuantity(orange, quantityOfOrange);
            _cart.AddItemQuantity(lemon, quantityOfLemon);
            _teller.AddSpecialOffer(_noDiscount, orange, 1, 0, 0);
            _teller.AddSpecialOffer(_noDiscount, lemon, 1, 0, 0);

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

    //    [Theory]
    //    [InlineData(5)]
    //    [InlineData(6)]
    //    [InlineData(7)]
    //    [InlineData(8)]
    //    [InlineData(9)]
    //    [InlineData(10)]
    //    [InlineData(11)]
    //    public void MixedOffer_AllSameItems_TenPercentANDTwoForAmount_AllReturnsTwoForAmountValue(int testedQuantity)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var unitPrice = 0.69;
    //        var quantity = testedQuantity;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(0.99 * Math.Floor((double)quantity / 2) + (quantity % 2) * unitPrice, 3); 
    //        // the calculation bases on the sequence of if/else statement in HandleOffers method 
    //        // TwoForAmount -> TenPercentDiscount


    //        _catalog.AddProduct(orange, unitPrice);
    //        _cart.AddItemQuantity(orange, quantity);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 0.99);
            
    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPrice, receiptItem.Price);
    //        Assert.Equal(quantity,receiptItem.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(5)]
    //    [InlineData(6)]
    //    [InlineData(7)]
    //    [InlineData(8)]
    //    [InlineData(9)]
    //    [InlineData(10)]
    //    [InlineData(11)]
    //    public void MixedOffer_AllSameItems_TenPercentANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var unitPrice = 0.69;
    //        var quantity = testedQuantity;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(1.65 * Math.Floor((double)quantity / 5) + (quantity % 5) * unitPrice, 3);
    //        // the calculation bases on the sequence of if/else statement in HandleOffers method 
    //        // FiveForAmount -> TenPercentDiscount


    //        _catalog.AddProduct(orange, unitPrice);
    //        _cart.AddItemQuantity(orange, quantity);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, orange, 5, 0, 1.65);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPrice, receiptItem.Price);
    //        Assert.Equal(quantity, receiptItem.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(5)]
    //    [InlineData(6)]
    //    [InlineData(7)]
    //    [InlineData(8)]
    //    [InlineData(9)]
    //    [InlineData(10)]
    //    [InlineData(11)]
    //    public void MixedOffer_AllSameItems_TwoForAmountANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var unitPrice = 0.69;
    //        var quantity = testedQuantity;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(1.65 * Math.Floor((double)quantity / 5) + (quantity % 5) * unitPrice,3);
    //        // the calculation bases on the sequence of if/else statement in HandleOffers method 
    //        // FiveForAmount -> TwoForAmount


    //        _catalog.AddProduct(orange, unitPrice);
    //        _cart.AddItemQuantity(orange, quantity);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 0.99);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, orange, 5, 0, 1.65);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPrice, receiptItem.Price);
    //        Assert.Equal(quantity, receiptItem.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(5)]
    //    [InlineData(6)]
    //    [InlineData(7)]
    //    [InlineData(8)]
    //    [InlineData(9)]
    //    [InlineData(10)]
    //    [InlineData(11)]
    //    public void MixedOffer_AllSameItems_TenPercentDiscountANDTwoForAmountANDFiveForAmount_AllReturnsFiveForAmountValue(int testedQuantity)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var unitPrice = 0.69;
    //        var quantity = testedQuantity;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(1.65 * Math.Floor((double)quantity / 5) + (quantity % 5) * unitPrice, 3);
    //        // the calculation bases on the sequence of if/else statement in HandleOffers method 
    //        // FiveForAmount -> TwoForAmount/ TenPercentDiscount


    //        _catalog.AddProduct(orange, unitPrice);
    //        _cart.AddItemQuantity(orange, quantity);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 0.99);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, orange, 5, 0, 1.65);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];

    //        _output.WriteLine($"quantity: {quantity}; receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPrice, receiptItem.Price);
    //        Assert.Equal(quantity, receiptItem.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(1, 12)]
    //    [InlineData(5, 5)]
    //    [InlineData(6, 6)]
    //    [InlineData(7, 7)]
    //    [InlineData(8, 8)]
    //    [InlineData(9, 9)]
    //    [InlineData(10, 10)]
    //    [InlineData(11, 11)]
    //    [InlineData(5, 6)]
    //    [InlineData(6, 7)]
    //    [InlineData(7, 8)]
    //    [InlineData(8, 9)]
    //    [InlineData(9, 10)]
    //    [InlineData(10, 11)]
    //    [InlineData(12, 1)]
    //    public void MixedOffer_HalfTenPercent_HalfTenPercentANDTwoForAmount_HalfReturnsTenDisCount_HalfReturnsTwoForAmountValue(int testedQ1, int testedQ2)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = testedQ1;
    //        var quantityOfLemon = testedQ2;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange * expectedDiscount 
    //                                + 1.05 * Math.Floor((double)quantityOfLemon / 2) + (quantityOfLemon % 2) * unitPriceOfLemon,3);

    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twoItemsForSale, lemon, 2, 0, 1.05);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];
    //        var receiptItem2 = receipt.GetItems()[1];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPriceOfOrange, receiptItem.Price);
    //        Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
    //        Assert.Equal(quantityOfOrange, receiptItem.Quantity);
    //        Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(1, 12)]
    //    [InlineData(5, 5)]
    //    [InlineData(6, 6)]
    //    [InlineData(7, 7)]
    //    [InlineData(8, 8)]
    //    [InlineData(9, 9)]
    //    [InlineData(10, 10)]
    //    [InlineData(11, 11)]
    //    [InlineData(5, 6)]
    //    [InlineData(6, 7)]
    //    [InlineData(7, 8)]
    //    [InlineData(8, 9)]
    //    [InlineData(9, 10)]
    //    [InlineData(10, 11)]
    //    [InlineData(12, 1)]
    //    public void MixedOffer_HalfTenPercent_HalfTenPercentANDFiveForAmount__HalfReturnsTenDisCount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = testedQ1;
    //        var quantityOfLemon = testedQ2;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(unitPriceOfOrange * quantityOfOrange * expectedDiscount
    //                                + 3.05 * Math.Floor((double)quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);

    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, lemon, 5, 0, 3.05);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];
    //        var receiptItem2 = receipt.GetItems()[1];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPriceOfOrange, receiptItem.Price);
    //        Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
    //        Assert.Equal(quantityOfOrange, receiptItem.Quantity);
    //        Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(1, 12)]
    //    [InlineData(5, 5)]
    //    [InlineData(6, 6)]
    //    [InlineData(7, 7)]
    //    [InlineData(8, 8)]
    //    [InlineData(9, 9)]
    //    [InlineData(10, 10)]
    //    [InlineData(11, 11)]
    //    [InlineData(5, 6)]
    //    [InlineData(6, 7)]
    //    [InlineData(7, 8)]
    //    [InlineData(8, 9)]
    //    [InlineData(9, 10)]
    //    [InlineData(10, 11)]
    //    [InlineData(12, 1)]
    //    public void MixedOffer_HalfFiveForAmount_HalfTenPercentANDTwoForAmount__HalfReturnsFiveForAmount_HalfReturnsTwoForAmountValue(int testedQ1, int testedQ2)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = testedQ1;
    //        var quantityOfLemon = testedQ2;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(4.0 * Math.Floor((double)quantityOfOrange / 5) + (quantityOfOrange % 5) * unitPriceOfOrange
    //                                + 1.05 * Math.Floor((double)quantityOfLemon / 2) + (quantityOfLemon % 2) * unitPriceOfLemon, 3);

    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 5], [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, orange, 5, 0, 4.0);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twoItemsForSale, lemon, 2, 0, 1.05);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];
    //        var receiptItem2 = receipt.GetItems()[1];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPriceOfOrange, receiptItem.Price);
    //        Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
    //        Assert.Equal(quantityOfOrange, receiptItem.Quantity);
    //        Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(1, 12)]
    //    [InlineData(5, 5)]
    //    [InlineData(6, 6)]
    //    [InlineData(7, 7)]
    //    [InlineData(8, 8)]
    //    [InlineData(9, 9)]
    //    [InlineData(10, 10)]
    //    [InlineData(11, 11)]
    //    [InlineData(5, 6)]
    //    [InlineData(6, 7)]
    //    [InlineData(7, 8)]
    //    [InlineData(8, 9)]
    //    [InlineData(9, 10)]
    //    [InlineData(10, 11)]
    //    [InlineData(12, 1)]
    //    public void MixedOffer_HalfTwoForAmount_HalfTenPercentANDFiveForAmount__HalfReturnsTwoForAmount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = testedQ1;
    //        var quantityOfLemon = testedQ2;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(1.02 * Math.Floor((double)quantityOfOrange / 2) + (quantityOfOrange % 2) * unitPriceOfOrange
    //                                + 3.05 * Math.Floor((double)quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);
    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 5], [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 1.02);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, lemon, 5, 0, 3.05);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];
    //        var receiptItem2 = receipt.GetItems()[1];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice}");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPriceOfOrange, receiptItem.Price);
    //        Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
    //        Assert.Equal(quantityOfOrange, receiptItem.Quantity);
    //        Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Theory]
    //    [InlineData(1, 12)]
    //    [InlineData(5, 5)]
    //    [InlineData(6, 6)]
    //    [InlineData(7, 7)]
    //    [InlineData(8, 8)]
    //    [InlineData(9, 9)]
    //    [InlineData(10, 10)]
    //    [InlineData(11, 11)]
    //    [InlineData(5, 6)]
    //    [InlineData(6, 7)]
    //    [InlineData(7, 8)]
    //    [InlineData(8, 9)]
    //    [InlineData(9, 10)]
    //    [InlineData(10, 11)]
    //    [InlineData(12, 1)]
    //    public void MixedOffer_HalfTenPercentANDTwoForAmount_HalfTenPercentANDFiveForAmount__HalfReturnsTwoForAmount_HalfReturnsFiveForAmountValue(int testedQ1, int testedQ2)
    //    {
    //        //ten percent +TwoForAmount; itemB: ten percent +FiveForAmount
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = testedQ1;
    //        var quantityOfLemon = testedQ2;
    //        var expectedDiscount = 0.90;
    //        var expectedTotalPrice = Math.Round(1.02 * Math.Floor((double)quantityOfOrange / 2) + (quantityOfOrange % 2) * unitPriceOfOrange
    //                                + 3.05 * Math.Floor((double)quantityOfLemon / 5) + (quantityOfLemon % 5) * unitPriceOfLemon, 3);
    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 5], [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, orange, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 1.02);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, lemon, 5, 0, 3.05);

    //        // ACT
    //        var receipt = _teller.ChecksOutArticlesFrom(_cart);
    //        var receiptItem = receipt.GetItems()[0];
    //        var receiptItem2 = receipt.GetItems()[1];

    //        _output.WriteLine($"receiptItem.TotalPrice: {receipt.GetTotalPrice()}; expectedTotalPrice: {expectedTotalPrice} ");

    //        // ASSERT
    //        Assert.Equal(orange, receiptItem.Product);
    //        Assert.Equal(unitPriceOfOrange, receiptItem.Price);
    //        Assert.Equal(unitPriceOfLemon, receiptItem2.Price);
    //        Assert.Equal(quantityOfOrange, receiptItem.Quantity);
    //        Assert.Equal(quantityOfLemon, receiptItem2.Quantity);
    //        Assert.Equal(expectedTotalPrice, receipt.GetTotalPrice());
    //    }

    //    [Fact]
    //    public void SpecialOfferStoredDataOutput_TwoItems_EachItemWithOneOffer_ReturnsOutputInXXXOrder()
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);
    //        var avocado = new Product("avocado", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var unitPriceOfAvocado = 1.15;
            
    //        var quantityOfOrange = 2;
    //        var quantityOfLemon = 2;
    //        var quantityOfAvocado = 2;

    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 5], [1, 1] or [12,1]

    //        _teller._offers = _mockOffers;
    //        // ACT
    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _catalog.AddProduct(avocado, unitPriceOfAvocado);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _cart.AddItemQuantity(avocado, quantityOfAvocado);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 1.02);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_twentyPercentDiscount, avocado, 1, 0.20, 0);

    //        var expectedOrangeOffer = new Offer(_twoItemsForSale, orange, 2, 0, 1.02);
    //        var expectedLemonOffer = new Offer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        var expectedAvocadoOffer = new Offer(_twentyPercentDiscount, avocado, 1, 0.20, 0);

    //        _output.WriteLine($"dictionary 1st key: {_teller._offers.First().Key.Name}; 1st value: " +
    //            $"discountRate - {_teller._offers.First().Value.DiscountRate}; " +
    //            $"offerType - {_teller._offers.First().Value.OfferType}; " + 
    //            $"sellingPrice - {_teller._offers.First().Value.SellingPrice}; " +
    //            $"sizeOfGrouping - {_teller._offers.First().Value.SizeOfGrouping}");

    //        _output.WriteLine($"dictionary 2nd key: {_teller._offers.Keys.ToList()[1].Name}; 2nd value: " +
    //            $"discountRate - {_teller._offers.Values.ToList()[1].DiscountRate}; " +
    //            $"offerType - {_teller._offers.Values.ToList()[1].OfferType}; " +
    //            $"sellingPrice - {_teller._offers.Values.ToList()[1].SellingPrice}; " +
    //            $"sizeOfGrouping - {_teller._offers.Values.ToList()[1].SizeOfGrouping}");

    //        _output.WriteLine($"dictionary 3rd key: {_teller._offers.Keys.ToList()[2].Name}; 3rd value: " +
    //            $"discountRate - {_teller._offers.Values.ToList()[2].DiscountRate}; " +
    //            $"offerType - {_teller._offers.Values.ToList()[2].OfferType}; " +
    //            $"sellingPrice - {_teller._offers.Values.ToList()[2].SellingPrice}; " +
    //            $"sizeOfGrouping - {_teller._offers.Values.ToList()[2].SizeOfGrouping}");

    //        // ASSERT
    //        // Dictionary<Product, Offer>
    //        Assert.Equal(orange, _teller._offers.First().Key);
    //        Assert.Equal(expectedOrangeOffer.DiscountRate, _teller._offers.First().Value.DiscountRate);
    //        Assert.Equal(expectedOrangeOffer.OfferType, _teller._offers.First().Value.OfferType);
    //        Assert.Equal(expectedOrangeOffer.SellingPrice, _teller._offers.First().Value.SellingPrice);
    //        Assert.Equal(expectedOrangeOffer.SizeOfGrouping, _teller._offers.First().Value.SizeOfGrouping);

    //        Assert.Equal(lemon, _teller._offers.Keys.ToList()[1]);
    //        Assert.Equal(expectedLemonOffer.DiscountRate, _teller._offers.Values.ToList()[1].DiscountRate);
    //        Assert.Equal(expectedLemonOffer.OfferType, _teller._offers.Values.ToList()[1].OfferType);
    //        Assert.Equal(expectedLemonOffer.SellingPrice, _teller._offers.Values.ToList()[1].SellingPrice);
    //        Assert.Equal(expectedLemonOffer.SizeOfGrouping, _teller._offers.Values.ToList()[1].SizeOfGrouping);

    //        Assert.Equal(avocado, _teller._offers.Keys.ToList()[2]);
    //        Assert.Equal(expectedAvocadoOffer.DiscountRate, _teller._offers.Values.ToList()[2].DiscountRate);
    //        Assert.Equal(expectedAvocadoOffer.OfferType, _teller._offers.Values.ToList()[2].OfferType);
    //        Assert.Equal(expectedAvocadoOffer.SellingPrice, _teller._offers.Values.ToList()[2].SellingPrice);
    //        Assert.Equal(expectedAvocadoOffer.SizeOfGrouping, _teller._offers.Values.ToList()[2].SizeOfGrouping);

    //    }

    //    [Fact]
    //    public void SpecialOfferStoredDataOutput_TwoItems_OneItemWithMultipleOffers_ReturnsOutputInXXXOrder()
    //    {
    //        // ARRANGE
    //        var orange = new Product("orange", ProductUnit.Each);
    //        var lemon = new Product("lemon", ProductUnit.Each);

    //        var unitPriceOfOrange = 0.69;
    //        var unitPriceOfLemon = 0.77;
    //        var quantityOfOrange = 2;
    //        var quantityOfLemon = 2;
    //        // automatically round up to 3 decimal places
    //        // no blocking on cases like [1, 5], [1, 1] or [12,1]

    //        _catalog.AddProduct(orange, unitPriceOfOrange);
    //        _catalog.AddProduct(lemon, unitPriceOfLemon);
    //        _cart.AddItemQuantity(orange, quantityOfOrange);
    //        _cart.AddItemQuantity(lemon, quantityOfLemon);
    //        _teller.AddSpecialOffer(_twoItemsForSale, orange, 2, 0, 1.02);
    //        _teller.AddSpecialOffer(_tenPercentDiscount, lemon, 1, 0.10, 0);
    //        _teller.AddSpecialOffer(_fiveItemsForSale, lemon, 5, 0, 3.05);

    //        // ACT

    //        var expectedOrangeOffer = new Offer(_twoItemsForSale, orange, 2, 0, 1.02);

    //        _output.WriteLine($"dictionary's count: {_teller._offers.Count}");

    //        _output.WriteLine($"dictionary 1st key: {_teller._offers.First().Key.Name}; 1st value: " +
    //            $"discountRate - {_teller._offers.First().Value.DiscountRate}; " +
    //            $"offerType - {_teller._offers.First().Value.OfferType}; " +
    //            $"sellingPrice - {_teller._offers.First().Value.SellingPrice}; " +
    //            $"sizeOfGrouping - {_teller._offers.First().Value.SizeOfGrouping}");

    //        _output.WriteLine($"dictionary 2nd key: {_teller._offers.Keys.ToList()[1].Name}; 2nd value: " +
    //            $"discountRate - {_teller._offers.Values.ToList()[1].DiscountRate}; " +
    //            $"offerType - {_teller._offers.Values.ToList()[1].OfferType}; " +
    //            $"sellingPrice - {_teller._offers.Values.ToList()[1].SellingPrice}; " +
    //            $"sizeOfGrouping - {_teller._offers.Values.ToList()[1].SizeOfGrouping}");


    //        // ASSERT
    //        Assert.Equal(2, _teller._offers.Count);
    //        Assert.Equal(orange, _teller._offers.First().Key);
    //        Assert.Equal(expectedOrangeOffer.DiscountRate, _teller._offers.First().Value.DiscountRate);
    //        Assert.Equal(expectedOrangeOffer.OfferType, _teller._offers.First().Value.OfferType);
    //        Assert.Equal(expectedOrangeOffer.SellingPrice, _teller._offers.First().Value.SellingPrice);
    //        Assert.Equal(expectedOrangeOffer.SizeOfGrouping, _teller._offers.First().Value.SizeOfGrouping);
    //    }

    }
}


//Assert.Equal(4.975, receipt.GetTotalPrice());
//Assert.Equal(new List<Discount>(), receipt.GetDiscounts());
//Assert.Single(receipt.GetItems());


// xunit coverage
// 1. same products - several discounts - does it support?
// cannot support multiple discounts

// 2. test cases
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
///
// tenpercentdiscount - leave two comments
// observe test name , what discount, expect on which item, does it check the ten percent discount outcome?
// what is your thought towards the original test design? reliable? can it check the target outcome?
