using Agency.Data.Models;
using Agency.Services;
using Agency.Web.Models.Names;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Agency.Web.Controllers
{
    public class NamesController : Controller
    {
        private readonly INamesService _namesService;
        private readonly ILogger<NamesController> _logger;
        private readonly IWorkersService _workersService;
        private readonly UserManager<AgencyUser> _userManager;

        public NamesController(INamesService namesService, ILogger<NamesController> logger,
                        IWorkersService workersService, UserManager<AgencyUser> userManager)
        {
            this._namesService = namesService;
            this._logger = logger;
            this._workersService = workersService;
            this._userManager = userManager;
        }

        public  IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateNamesModel createNamesModel)
        {
            if (!this.ModelState.IsValid)
            {
                _logger.LogWarning($"State of binding model CreateNames - INVALID!");
                return this.Redirect("/Names/Create");
            }
            var userId = this._userManager.GetUserId(this.User);
            var workerId = this._workersService.GetWorkerId(userId);

            var namesOfUser = new Names
            {
                FirstName = createNamesModel.FirstName,
                MiddleName = createNamesModel.MiddleName,
                LastName = createNamesModel.LastName,
                WorkerId = workerId
                
            };

            this._namesService.AddAsync(namesOfUser);
            return this.View();
        }
    }
}
