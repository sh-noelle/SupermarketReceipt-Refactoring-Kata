using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt.OfferTypes
{
    public abstract class OfferHandlerBase
    {
        //public abstract Discount getDiscount(int sizeOfGrouping, double discountRate, double sellingPrice);
        public OfferHandlerBase next;
        public void SetNext(OfferHandlerBase next)
        {
            this.next = next;
        }
        public abstract void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, int quantity, SupermarketCatalog catalog);
    }
}
