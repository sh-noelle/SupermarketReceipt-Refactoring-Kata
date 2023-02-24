using SupermarketReceipt.OfferTypes;
using System;
using System.Collections.Generic;
using SupermarketReceipt.OfferTypes.SpecialOfferHandling;

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
            var amountForSpecificPriceHandler = new AmountForSpecificPriceHandler();
            var buyItemsGetItemsFreeHandler = new BuyItemsGetItemsFreeHandler();
            var specificPercentDiscountHandler = new SpecificPercentDiscountHandler();
            var noDiscountHandler = new NoDiscountHandler();

            amountForSpecificPriceHandler.SetNext(buyItemsGetItemsFreeHandler);
            buyItemsGetItemsFreeHandler.SetNext(specificPercentDiscountHandler);
            specificPercentDiscountHandler.SetNext(noDiscountHandler);

        foreach (var product in _productQuantities.Keys)
        {
            var quantity = (int)_productQuantities[product];
            amountForSpecificPriceHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
        }
    }
}
}