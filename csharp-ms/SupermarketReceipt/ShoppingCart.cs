using System;
using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        public List<ProductQuantity> _items = new List<ProductQuantity>();
        public Dictionary<Product, double> _productQuantities = new Dictionary<Product, double>();
        public List<SpecialOfferModel> _specialOfferList = new List<SpecialOfferModel> 
        {
             new SpecialOfferModel{ OfferType = SpecialOfferType.TenPercentDiscount, GroupingNumber = 1, Labelling = "10% off Discount" },
             new SpecialOfferModel{ OfferType = SpecialOfferType.TwentyPercentDiscount, GroupingNumber = 1, Labelling = "20% off Discount"},
             new SpecialOfferModel{ OfferType = SpecialOfferType.GetOneFree, GroupingNumber = 2, Labelling = "Buy one get one free"},
             new SpecialOfferModel{ OfferType = SpecialOfferType.TwoForAmount, GroupingNumber = 2, Labelling = "Buy two for"},
             new SpecialOfferModel{ OfferType = SpecialOfferType.FiveForAmount, GroupingNumber = 5, Labelling = "Buy five for"},
        };

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
            foreach (var product in _productQuantities.Keys)
            {
                var quantity = (int) _productQuantities[product];
                var unitPrice = catalog.GetUnitPrice(product);
                Discount discount = null;
                if (!specialOffers.ContainsKey(product)) 
                {
                    //throw new ArgumentException("Special offers do not cover the product.");
                    //var total = quantity * unitPrice;
                    discount = null;
                }
                //foreach (KeyValuePair<SpecialOfferType, int> specialOfferItem in _specialOfferDictionary) 
                //{
                //    var groupingNum = specialOfferItem.Value;
                    
                //    if (quantity >= groupingNum)
                //    {
                //        //throw new ArgumentException("Quantity should be equal to or bigger than groupingNum");
                //    }
                //}
                
                if (specialOffers.ContainsKey(product))
                {
                    var offer = specialOffers[product];
                    
                    
                    var groupingNum = 1;
                    if (offer.OfferType == SpecialOfferType.ThreeForTwo)
                    {
                        groupingNum = 3;
                    }
                    else if (offer.OfferType == SpecialOfferType.TwoForAmount)
                    {
                        groupingNum = 2;

                        {
                            var total = offer.Argument * (quantity / groupingNum) + quantity % 2 * unitPrice;
                            var discountN = unitPrice * quantity - total;
                            discount = new Discount(product, "2 for " + offer.Argument, -discountN);
                        }
                    }

                    if (offer.OfferType == SpecialOfferType.FiveForAmount) groupingNum = 5;
                    var numberOfXs = quantity / groupingNum;
                    if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantity > 2)
                    {
                        var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantity % 3 * unitPrice);
                        discount = new Discount(product, "3 for 2", -discountAmount);
                    }

                    if (offer.OfferType == SpecialOfferType.TenPercentDiscount) 
                    {
                        discount = new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
                    }
                        
                    if (offer.OfferType == SpecialOfferType.FiveForAmount && quantity >= 5)
                    {
                        var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantity % 5 * unitPrice);
                        discount = new Discount(product, groupingNum + " for " + offer.Argument, -discountTotal);
                    }

                    if (discount != null)
                        receipt.AddDiscount(discount);
                }
            }
        }
    }
}