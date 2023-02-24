using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SupermarketReceipt.OfferTypes.SpecialOfferCategories
{
    public class SpecificPercentDiscountHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, double quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOffers.TenPercentDiscount || offer.OfferType == SpecialOffers.TwentyPercentDiscount)
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
                    nextHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
                }

            }
            else if(nextHandler != null)
            {
                nextHandler.HandleOffer(receipt, specialOffers, product, quantity, catalog);
            }
        }
    }
}
