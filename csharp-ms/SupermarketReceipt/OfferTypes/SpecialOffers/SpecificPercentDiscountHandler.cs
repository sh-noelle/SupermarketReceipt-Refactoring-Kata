using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SupermarketReceipt.OfferTypes.SpecialOffers
{
    public class SpecificPercentDiscountHandler : OfferHandlerBase
    {
        public override void HandleOffer(Receipt receipt, Dictionary<Product, Offer> specialOffers, Product product, int quantity, SupermarketCatalog catalog)
        {
            var offer = specialOffers[product];
            if (offer.OfferType == SpecialOfferCategories.SpecificPercentDiscount)
            {
                var unitPrice = catalog.GetUnitPrice(product);
                var discount = unitPrice * quantity * offer.DiscountRate;

                var discountStatement = new DiscountStatement(
                    product,
                    $"{offer.DiscountRate.ToString("P1", CultureInfo.InvariantCulture)} off",
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
