using Catalog.Web.Data;
using Catalog.Web.Models.TransferModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository repository;
        public ProductsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await repository.GetCategoriesJson();
            return Ok(result);
        }

        [HttpGet("productcolorgroups/{id}")]
        public async Task<IActionResult> GetProductColorGroups(int id)
        {
            var result = await repository.GetProductColorGroups(id);
            return Ok(result);
        }

        [HttpGet("productcolors/{id}")]
        public async Task<IActionResult> GetBulkProductMaterials(int id)
        {
            var result = await repository.GetProductBulkWeights(id);
            return Ok(result);
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateProduct(ProductRateParam data)
        {
            var result = await repository.RateProduct(data);
            return Ok(result);
        }

        [HttpPost("addToBasket")]
        public async Task<IActionResult> AddToBasket(AddToBasketParam data)
        {
            /*
             * بسته بودن فروشگاه
             * بررسی فروش مثبت برای محصولات کلافی
             * بررسی اعتبار شماره همراه و رمز عبور و دریافت اطلاعات کاربر
             * بررسی غیر فعال بودن حساب کاربری
             * بررسی اطلاعات شهر و استان
             * بررسی وجود نماینده در آن شهر درصورتی که کاربر تخفیف ندارد و ارسال لیست نمایندگان
             * بررسی کد ملی
             * اضافه کردن محصول به سبد خرید
             */
            var result = await repository.AddToBasket(data);
            return Ok(new { result });
        }
    }
}