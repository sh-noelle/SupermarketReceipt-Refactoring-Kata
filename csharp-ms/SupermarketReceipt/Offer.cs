namespace SupermarketReceipt
{
    public enum SpecialOfferCategories
    {
        BuyItemsGetItemsFree,
        SpecificPercentDiscount,
        ItemsForSales,
        NoDiscount
    }

    public class Offer
    {
        private Product _product;

        public Offer(SpecialOfferItem offerType, Product product, double sellingPrice)
        {
            OfferType = offerType;
            _product = product;
            SellingPrice = sellingPrice;
        }

        public SpecialOfferItem OfferType { get; }
        public Product Product { get; }
        public double SellingPrice { get; }
    }

}