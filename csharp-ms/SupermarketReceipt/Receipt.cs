using System;
using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Receipt
    {
        public readonly List<DiscountStatement> _discounts = new List<DiscountStatement>();
        public readonly List<ReceiptItem> _items = new List<ReceiptItem>();

        public double GetTotalPrice()
        {
            var total = 0.0;
            foreach (var item in _items) total += item.TotalPrice;
            foreach (var discount in _discounts) total += discount.DiscountAmount;
            return Math.Round(total,3);
        }

        public void AddProduct(Product product, double quantity, double price, double totalPrice)
        {
            _items.Add(new ReceiptItem(product, quantity, price, totalPrice));
        }

        public List<ReceiptItem> GetItems()
        {
            return new List<ReceiptItem>(_items);
        }

        public void AddDiscountStatement(DiscountStatement discount)
        {
            _discounts.Add(discount);
        }

        public List<DiscountStatement> GetDiscounts()
        {
            return _discounts;
        }
    }

    public class ReceiptItem
    {
        public ReceiptItem(Product product, double quantity, double price, double totalPrice)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }

        public Product Product { get; }
        public double Price { get; }
        public double TotalPrice { get; }
        public double Quantity { get; }
    }
}