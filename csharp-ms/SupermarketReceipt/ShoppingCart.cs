using SupermarketReceipt.OfferTypes.SpecialOffers;
using SupermarketReceipt.OfferTypes;
using System;
using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        public List<ProductQuantity> _items = new List<ProductQuantity>();
        public Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();

        public ShoppingCart() 
        {

        }
        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
        }

        public void AddItem(Product product)
        {
            AddItemQuantity(product, 1.0);
        }


        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
            if (_productQuantities.ContainsKey(product))
            {
                var newAmount = _productQuantities[product] + quantity;
                _productQuantities[product] = newAmount;
            }
            else
            {
                _productQuantities.Add(product, quantity);
            }
        }


    public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> specialOffers, SupermarketCatalog catalog)
    {
            var fiveItemsForSaleHandler = new AmountForSpecificPriceHandler();
            var twoItemsForSaleHandler = new AmountForSpecificPriceHandler();
            var buyOneGetOneHandler = new BuyItemsGetItemsFreeHandler();
            var tenPercentDiscountHandler = new SpecificPercentDiscountHandler();
            var twentyPercentDiscountHandler = new SpecificPercentDiscountHandler();
            var noDiscountHandler = new NoDiscountHandler();

            fiveItemsForSaleHandler.SetNext(twoItemsForSaleHandler);
            twoItemsForSaleHandler.SetNext(buyOneGetOneHandler);
            buyOneGetOneHandler.SetNext(twentyPercentDiscountHandler);
            twentyPercentDiscountHandler.SetNext(tenPercentDiscountHandler);
            tenPercentDiscountHandler.SetNext(noDiscountHandler);

        foreach (var product in _productQuantities.Keys)
        {
            var quantity = (int)_productQuantities[product];
            fiveItemsForSaleHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
        }
    }
}
}