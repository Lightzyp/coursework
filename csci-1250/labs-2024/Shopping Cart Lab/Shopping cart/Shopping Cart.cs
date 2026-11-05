using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shopping_cart.Program;

namespace Shopping_cart
{
    internal class Shopping_Cart
    {
        public class ShoppingCart
        {
            private List<Item> items;

            public ShoppingCart()
            {
                items = new List<Item>();
            }

            public void AddToCart(Item item)
            {
                items.Add(item);
            }

            public void ClearShoppingCart()
            {
                items.Clear();
            }

            public void RemoveFromCart(int index)
            {
                if (index >= 0 && index < items.Count)
                    items.RemoveAt(index);
            }

            public IEnumerable<Item> GetItems()
            {
                return items;
            }

            // Other methods for calculating subtotals, taxes, etc.
            // ...

            public override string ToString()
            {
                return $"Shopping Cart ({items.Count} items)";
            }
        }
    }
}
