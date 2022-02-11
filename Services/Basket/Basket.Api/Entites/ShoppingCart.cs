using System.Collections.Generic;

namespace Basket.Api.Entites
{
    public class ShoppingCart
    {

        public ShoppingCart()
        {

        }

        public ShoppingCart(string username)
        {
            Username = username;
        }

        public string Username { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;

                foreach (var item in Items)
                {
                    total += item.Price;
                }

                return total;
            }
        }
    }
}
