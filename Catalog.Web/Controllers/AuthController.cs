using Catalog.Web.Data;
using Catalog.Web.Models.TransferModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository repo;
        public AuthController(IRepository repository)
        {
            repo = repository;
        }

        public async Task<IActionResult> Login(LoginParam data)
        {
            var user = await repo.Login(data);
            if (user == null)
                return Unauthorized();
            return Ok(user);
        }
    }
}