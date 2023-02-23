using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SupermarketReceipt.OfferTypes
{
    public class NoDiscountHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, int quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOfferCategories.NoDiscount) 
            {
                var discount = 0;
                var discountStatement = new DiscountStatement(
                        product,
                        "Marked price",
                        discount * -1
                        );
                receipt.AddDiscountStatement(discountStatement);
            }
            else
            {
                next.HandleOffer(receipt, specialOffers, product, quantity, catalog);
            }
        }
    }
}
