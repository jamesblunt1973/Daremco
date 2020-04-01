using Catalog.Web.Models.Entities;
using Catalog.Web.Models.Enums;
using Catalog.Web.Models.ViewModels;
using Catalog.Web.Models.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Web.Data
{
    public interface IRepository
    {
        Task<FilterResult> GetProducts(FilterData data);
        Task<string> GetCategoriesJson();
        Task<IEnumerable<Category>> GetCategories();
        Task<PrimaryData> GetPrimaryData();
        Task<Product> GetProduct(int id);
        Task<IEnumerable<ProductColorGroup>> GetProductColorGroups(int id);
		Task<IEnumerable<SitemapProduct>> GetSiteMapProducts();
		Task<ProductRateResult> RateProduct(ProductRateParam data);
        Task<IEnumerable<MiscCategory>> GetMiscCategories();
        Task<IEnumerable<MiscProduct>> GetMiscProducts(FilterData data);
        Task<Post> GetPost(Posts id);
        Task<IEnumerable<ExtraProduct>> GetExtraProducts(FilterData data);
        Task<ExtraProduct> GetExtraProduct(int id);
        Task<IEnumerable<Agent>> GetAgents(FilterData data);
        Task<IEnumerable<State>> GetStates();
        Task<IEnumerable<City>> GetCities(int stateId);
        Task<string> AddToBasket(AddToBasketParam data);
        Task<IEnumerable<BulkTypeWeight>> GetProductBulkWeights(int id);
        Task<User> Login(LoginParam data);
    }
}
