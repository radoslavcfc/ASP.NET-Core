using Agency.Data.Models;
using Agency.Services;
using Agency.Models.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Agency.Web.Controllers
{
    public class WorkersController : Controller
    {
        private readonly ILogger<NamesController> _logger;
        private readonly IWorkersService _workersService;
        private readonly UserManager<AgencyUser> _userManager;

        public WorkersController(ILogger<NamesController> logger,
                        IWorkersService workersService, UserManager<AgencyUser> userManager)
        {
            this._logger = logger;
            this._workersService = workersService;
            this._userManager = userManager;
        }

        public IActionResult Complete()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Complete(CompleteWorkerDataModel bindingModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(bindingModel);
            }

            var currentUserId = this._userManager.GetUserId(this.User);
           
            this._workersService.UpdateAsync(currentUserId, bindingModel);

            return this.Redirect("/Workers/ContactData");
        }
    }
}
