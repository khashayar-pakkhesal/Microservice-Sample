using ShoppingAggregator.Contracts;
using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _httpClient.GetAsync("Api/v1/Catalog");
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _httpClient.GetAsync($"Api/v1/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string categoryName)
        {
            var response = await _httpClient.GetAsync($"Api/v1/Catalog/GetProductsByCategory/{categoryName}");
            return await response.ReadContentAs<List<CatalogModel>>();
        }
    }
}
