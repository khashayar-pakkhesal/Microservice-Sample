namespace ShoppingAggregator.Models
{
    public class BasketModel
    {
        public string Username { get; set; }

        public List<ShoppingCartItemModel> Items { get; set; } = new();
    }
}
