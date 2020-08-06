using Agency.Services;
using Agency.Web.Models.Names;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Agency.Web.Controllers
{
    public class NamesController : Controller
    {
        private readonly INamesService namesService;

        public NamesController(INamesService namesService)
        {
            this.namesService = namesService;
        }

        public async Task<IActionResult> Create()
        {
            var createNamesModel = new CreateNamesModel();
            return this.View();
        }
    }
}
