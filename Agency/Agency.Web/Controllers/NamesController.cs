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

        public  IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateNamesModel createNamesModel)
        {
            return this.View();
        }
    }
}
