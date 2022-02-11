namespace ShoppingAggregator.Models
{
    public class ShoppingModel
    {
        public string Username { get; set; }
        public BasketModel BasketWithProducts { get; set; }
        public IEnumerable<OrderModel> Orders { get; set; }
    }
}
