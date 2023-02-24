using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        public Dictionary<Product, Offer> _offers = new Dictionary<Product, Offer>();

        public Teller(SupermarketCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddSpecialOffer(SpecialOfferCategories offerType, Product product, int sizeOfGrouping, double discountRate, double sellingPrice)
        {
            _offers[product] = new Offer(offerType, product, sizeOfGrouping, discountRate, sellingPrice);
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