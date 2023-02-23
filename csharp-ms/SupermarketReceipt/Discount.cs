namespace SupermarketReceipt
{
    public class DiscountStatement
    {
        public DiscountStatement(Product product, string description, double discountAmount)
        {
            Product = product;
            Description = description;
            DiscountAmount = discountAmount;
        }

        public string Description { get; }
        public double DiscountAmount { get; }
        public Product Product { get; }
    }
}