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
            SellingPrice = sellingPrice;
            _product = product;
        }

        public SpecialOfferItem OfferType { get; }
        public double SellingPrice { get; }
        public Product Product { get; }
    }

}