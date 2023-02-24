using System;
using System.Collections.Generic;
using System.Text;

namespace SupermarketReceipt.OfferTypes
{
    public abstract class OfferHandlerBase
    {
        //public abstract Discount getDiscount(int sizeOfGrouping, double discountRate, double sellingPrice);
        public OfferHandlerBase nextHandler;
        public void SetNext(OfferHandlerBase nextHandler)
        {
            this.nextHandler = nextHandler;
        }
        public abstract void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog);
    }
}
