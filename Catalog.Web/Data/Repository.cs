using Catalog.Web.Helpers;
using Catalog.Web.Models.Entities;
using Catalog.Web.Models.Enums;
using Catalog.Web.Models.ViewModels;
using Catalog.Web.Models.TransferModels;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Data
{
    public class Repository : IRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;
        private readonly ILogger<Repository> logger;

        public Repository(IConfiguration configuration, IMemoryCache cache, ILogger<Repository> logger)
        {
            this.configuration = configuration;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<FilterResult> GetProducts(FilterData data)
        {
            var result = new FilterResult();
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetProducts";
            var command = new CommandDefinition(commandText, data.GetDictionary(), commandType: CommandType.StoredProcedure);

            IEnumerable<ProductSummury> queryResults;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                queryResults = await connection.QueryAsync<ProductSummury>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            if (queryResults.Any())
            {
                var firstRow = queryResults.First();
                result.CategoryName = firstRow.CategoryName;
                result.GalleryName = firstRow.GalleryName;
                result.TotalCount = firstRow.TotalCount;
                result.Products = queryResults;
            }

            return result;
        }

        public async Task<string> GetCategoriesJson()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetCategories";
            var command = new CommandDefinition(commandText, new { Json = true }, commandType: CommandType.StoredProcedure);

            string json;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                json = await connection.QueryFirstAsync<string>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                json = "";
            }

            return json;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetCategories";
            var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

            IEnumerable<Category> categories;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                var dic = new Dictionary<int, Category>();
                var result = await connection.QueryAsync<Category, Gallery, Category>(command, (category, gallery) =>
                {
                    if (!dic.TryGetValue(category.Id, out Category cat))
                    {
                        cat = category;
                        cat.Galleries = new List<Gallery>();
                        dic.Add(cat.Id, cat);
                    }
                    cat.Galleries.Add(gallery);
                    return cat;
                });
                categories = result.Distinct();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return categories;
        }

        public async Task<PrimaryData> GetPrimaryData()
        {
            return await cache.GetOrCreateAsync("primary_data", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.Today.AddDays(1);
                return await GetPrimaryDataFromDb();
            });
        }

        private async Task<PrimaryData> GetPrimaryDataFromDb()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetPrimaryData";
            var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

            var primaryData = new PrimaryData();
            try
            {
                IEnumerable<DkbRaj> rajs;
                IEnumerable<TieLength> tieLengths;
                IEnumerable<Layout> layouts;
                IEnumerable<TieType> tieTypes;
                IEnumerable<SendMethod> sendMethods;
                IEnumerable<Setting> settings;
                IEnumerable<Message> messages;
                IEnumerable<PlanType> planTypes;
                IEnumerable<Material> materials;
                IEnumerable<Models.Entities.Attribute> attributes;
                IEnumerable<ExtraCategory> extraCategories;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryMultipleAsync(command);

                    var dic = new Dictionary<int, DkbRaj>();
                    rajs = result.Read<DkbRaj, MaterialRaj, DkbRaj>((r, mr) => {
                        if (!dic.TryGetValue(r.Raj, out DkbRaj dkbRaj))
                        {
                            dkbRaj = r;
                            dkbRaj.MaterialRajs = new List<MaterialRaj>();
                            dic.Add(dkbRaj.Raj, dkbRaj);
                        }
                        dkbRaj.MaterialRajs.Add(mr);
                        return dkbRaj;
                    }).Distinct();
                    tieLengths = await result.ReadAsync<TieLength>();
                    layouts = await result.ReadAsync<Layout>();
                    tieTypes = await result.ReadAsync<TieType>();
                    sendMethods = await result.ReadAsync<SendMethod>();
                    settings = await result.ReadAsync<Setting>();
                    messages = await result.ReadAsync<Message>();
                    planTypes = await result.ReadAsync<PlanType>();
                    materials = await result.ReadAsync<Material>();
                    attributes = await result.ReadAsync<Models.Entities.Attribute>();
                    extraCategories = await result.ReadAsync<ExtraCategory>();
                }

                primaryData.Rajs = rajs.ToDictionary(a => a.Raj);
                primaryData.TieLengths = tieLengths.ToDictionary(a => a.Id);
                primaryData.Layouts = layouts.ToDictionary(a => a.Id, b => b.Name);
                primaryData.TieTypes = tieTypes.ToDictionary(a => a.Id, b => b.Name);
                primaryData.SendMethods = sendMethods.ToDictionary(a => a.Id);
                primaryData.Settings = settings.ToDictionary(a => a.Name, b => b.Value);
                primaryData.Messages = messages.ToDictionary(a => a.Title, b => b.Context);
                primaryData.PlanTypes = planTypes.ToDictionary(a => a.Id, b => b.Name);
                primaryData.Materials = materials.ToDictionary(a => a.Id);
                primaryData.Attributes = attributes.ToDictionary(a => a.Id);
                primaryData.ExtraCategories = extraCategories.ToDictionary(a => a.Id);
                primaryData.Sizes = new List<Size>() {
                    new Size(){ Id = 1, Title = "زیر یک متر مربع - ذرع و چهارک", Limits = "0,1" },
                    new Size(){ Id = 2, Title = "یک تا دومتر مربع - ذرع و نیم", Limits = "1,2" },
                    new Size(){ Id = 3, Title = "دو تا چهار متر مربع - قالیچه", Limits = "2,4" },
                    new Size(){ Id = 4, Title = "چهار تا هفت متر مربع", Limits = "4,7" },
                    new Size(){ Id = 5, Title = "هفت تا ده متر مربع", Limits = "7,10" },
                    new Size(){ Id = 6, Title = "بیشتر از ده متر مربع", Limits = "10," }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return primaryData;
        }

        public async Task<Product> GetProduct(int id)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetProduct";
            var command = new CommandDefinition(commandText, new { Id = id }, commandType: CommandType.StoredProcedure);

            Product product;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                var result = await connection.QueryMultipleAsync(command);
                product = await result.ReadFirstAsync<Product>();
                product.ProductTypes = await result.ReadAsync<ProductType>();
                product.BulkTypes = await result.ReadAsync<BulkType>();
                product.Plans = await result.ReadAsync<Plan>();
                product.ProductColors = await result.ReadAsync<ProductColor>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                product = null;
            }

            return product;
        }

        public async Task<IEnumerable<ProductColorGroup>> GetProductColorGroups(int id)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetProductColorGroups";
            var command = new CommandDefinition(commandText, new { TypeId = id }, commandType: CommandType.StoredProcedure);

            IEnumerable<ProductColorGroup> p;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                p = await connection.QueryAsync<ProductColorGroup>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                p = null;
            }

            return p;
        }

        public async Task<ProductRateResult> RateProduct(ProductRateParam data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "RateProduct";
            var command = new CommandDefinition(commandText, data, commandType: CommandType.StoredProcedure);

            ProductRateResult p;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                p = await connection.QueryFirstAsync<ProductRateResult>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                p = null;
            }

            return p;
        }

        public async Task<IEnumerable<MiscCategory>> GetMiscCategories()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetMiscCategories";
            var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

            IEnumerable<MiscCategory> categories;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                var dic = new Dictionary<int, MiscCategory>();
                var result = await connection.QueryAsync<MiscCategory, MiscGallery, MiscCategory>(command, (category, gallery) =>
                {
                    if (!dic.TryGetValue(category.Id, out MiscCategory cat))
                    {
                        cat = category;
                        cat.Galleries = new List<MiscGallery>();
                        dic.Add(cat.Id, cat);
                    }
                    cat.Galleries.Add(gallery);
                    return cat;
                });
                categories = result.Distinct();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return categories;
        }

        public async Task<IEnumerable<MiscProduct>> GetMiscProducts(FilterData data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetMiscProducts";

            var command = new CommandDefinition(commandText, data.GetDictionary(), commandType: CommandType.StoredProcedure);

            IEnumerable<MiscProduct> result;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                result = await connection.QueryAsync<MiscProduct>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return result;
        }

        public async Task<IEnumerable<ExtraProduct>> GetExtraProducts(FilterData data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetExtraProducts";

            var command = new CommandDefinition(commandText, data.GetDictionary(), commandType: CommandType.StoredProcedure);

            IEnumerable<ExtraProduct> result;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                result = await connection.QueryAsync<ExtraProduct>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return result;
        }

        public async Task<ExtraProduct> GetExtraProduct(int id)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetExtraProduct";
            var command = new CommandDefinition(commandText, new { Id = id }, commandType: CommandType.StoredProcedure);

            ExtraProduct product;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                product = await connection.QueryFirstAsync<ExtraProduct>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                product = null;
            }

            return product;
        }

        public async Task<IEnumerable<Agent>> GetAgents(FilterData data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetAgents";

            var command = new CommandDefinition(commandText, data.GetDictionary(), commandType: CommandType.StoredProcedure);

            IEnumerable<Agent> result;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                result = await connection.QueryAsync<Agent>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return result;
        }

        public async Task<IEnumerable<State>> GetStates()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetStates";
            var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

            IEnumerable<State> states;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                states = await connection.QueryAsync<State>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return states;
        }

        public async Task<IEnumerable<City>> GetCities(int stateId)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetCities";
            var command = new CommandDefinition(commandText, new { StateId = stateId }, commandType: CommandType.StoredProcedure);

            IEnumerable<City> cities;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                cities = await connection.QueryAsync<City>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return cities;
        }

        public async Task<string> AddToBasket(AddToBasketParam data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "AddToBasket";
            var command = new CommandDefinition(commandText, data, commandType: CommandType.StoredProcedure);

            string result = "";
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                result = await connection.QueryFirstAsync<string>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                result = ex.Message;
            }

            return result;
        }

        public async Task<Post> GetPost(Posts id)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "SELECT * FROM Posts WHERE Id = @Id";
            var command = new CommandDefinition(commandText, new { id }, commandType: CommandType.Text);

            Post post = null;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                post = await connection.QueryFirstAsync<Post>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
            }

            return post;
        }

        public async Task<IEnumerable<BulkTypeWeight>> GetProductBulkWeights(int id)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetBulkWeights";
            var command = new CommandDefinition(commandText, new { id }, commandType: CommandType.StoredProcedure);

            IEnumerable<BulkTypeWeight> weights;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                weights = await connection.QueryAsync<BulkTypeWeight>(command);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetExceptionMessage());
                return null;
            }

            return weights;
        }

        public async Task<User> Login(LoginParam data)
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "CheckUser";
            var command = new CommandDefinition(commandText, data, commandType: CommandType.StoredProcedure);

            User user;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                user = await connection.QuerySingleOrDefaultAsync<User>(command);
            }
            catch (Exception ex)
            {
                var error = ex.GetExceptionMessage();
                logger.LogError(error);
                throw new Exception(error);
            }

            return user;
        }

        public async Task<IEnumerable<SitemapProduct>> GetSiteMapProducts()
        {
            string connectionString = configuration.GetConnectionString("Default");

            var commandText = "GetSitemapProducts";
            var command = new CommandDefinition(commandText, commandType: CommandType.StoredProcedure);

            IEnumerable<SitemapProduct> products;
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                products = await connection.QueryAsync<SitemapProduct>(command);
            }
            catch (Exception ex)
            {
                var error = ex.GetExceptionMessage();
                logger.LogError(error);
                throw new Exception(error);
            }

            return products;
        }
    }
}
