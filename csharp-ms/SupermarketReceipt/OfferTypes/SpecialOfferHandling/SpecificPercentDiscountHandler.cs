using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SupermarketReceipt.OfferTypes.SpecialOfferHandling
{
    public class SpecificPercentDiscountHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, List<SpecialOfferItem> specialOfferList, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOfferCategories.SpecificPercentDiscount)
            {
                var unitPrice = catalog.GetUnitPrice(product);
                var discount = unitPrice * quantity * offer.DiscountRate;

                var discountStatement = new DiscountStatement(
                    product,
                    $"{offer.DiscountRate.ToString("P1", CultureInfo.InvariantCulture)} off",
                    discount
                    );

                receipt.AddDiscountStatement(discountStatement);
                if (nextHandler != null) 
                {
                    nextHandler.HandleOffer(receipt, specialOfferList, specialOffers, product, quantity, catalog);
                }

            }
            else if(nextHandler != null)
            {
                nextHandler.HandleOffer(receipt, specialOfferList, specialOffers, product, quantity, catalog);
            }
        }
    }
}
