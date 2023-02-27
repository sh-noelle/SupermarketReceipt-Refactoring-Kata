using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        public List<SpecialOfferItem> _specialOfferList= new List<SpecialOfferItem>();
        public Dictionary<Product, Offer> _offers = new Dictionary<Product, Offer>();

        public Teller(SupermarketCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddSpecialOffer(string offerType, Product product, double sellingPrice)
        {
            var specialOffer = _specialOfferList.FirstOrDefault(offer => offer.SpecialOffer == offerType);
            _offers[product] = new Offer(specialOffer, product, sellingPrice);
        }

        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            foreach (var productQuantity in productQuantities)
            {
                var product = productQuantity.Product;
                var quantity = productQuantity.Quantity;
                var unitPrice = _catalog.GetUnitPrice(product);
                var price = quantity * unitPrice;
                receipt.AddProduct(product, quantity, unitPrice, price);
            }

            theCart.HandleOffers(receipt, _offers, _catalog);
           
            return receipt;
        }
    }
}