using ShoppingAggregator.Models;

namespace ShoppingAggregator.Contracts
{
    public interface ICatalogService
    {

        Task<IEnumerable<CatalogModel>> GetCatalog();
        Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string categoryName);
        Task<CatalogModel> GetCatalog(string id);
    }
}
