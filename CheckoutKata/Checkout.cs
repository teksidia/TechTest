﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutKata
{
    interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }

    public class Checkout : ICheckout
    {
        private readonly List<InventoryItem> _inventory;
        private readonly List<Offer> _offers;
        private readonly List<string> _basket;

        public Checkout(List<InventoryItem> inventory, List<Offer> offers)
        {
            _basket = new List<string>();
            _inventory = inventory;
            _offers = offers;
        }

        public void Scan(string item)
        {
            _basket.Add(item);
        }

        public int GetTotalPrice()
        {
            var total = 0;
            foreach (var sku in _basket)
            {
                total += CalculateDistinctPrice(sku, 1);
            }
            return total;
        }

        public int CalculateDistinctPrice(string sku, int itemCount)
        {
            var normalPrice = _inventory.First(i => i.Sku == sku).Price;
            var matchingOffer = _offers.FirstOrDefault(o => o.Sku == sku) ?? new Offer(sku, 1, normalPrice);
            var offerApplyCount = itemCount / matchingOffer.AmountNeeded;
            var nonDiscountedCount = itemCount % matchingOffer.AmountNeeded;
            return (offerApplyCount * matchingOffer.Price) + (nonDiscountedCount * normalPrice);
        }
    }
}
