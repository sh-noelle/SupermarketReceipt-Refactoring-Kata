using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt.SpecialOfferTypes
{
    public abstract class OfferHandler
    {
        //public abstract Discount getDiscount(int sizeOfGrouping, double discountRate, double sellingPrice);
        public OfferHandler next;
        public void SetNext(OfferHandler next)
        {
            this.next = next;
        }
        public abstract void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, int quantity, SupermarketCatalog catalog);
    }
}
