using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.NoSql.Data.DomainRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MidWare.Models;

namespace MidWare.Controllers
{
    [Route("[controller]")]
    //[Authorize]
    public class CarrierController : Controller
    {
        private readonly IProjectFeedRepository _repo;
        private readonly IAccountRepository _accRepo;

        public CarrierController()
        {
            _repo = new ProjectFeedRepository();
            _accRepo = new AccountRepository();
        }
        
        // GET: Carrier
        public ActionResult Index()
        {
            var loggedUser = LoggedUser();
            ViewData["accountId"] = loggedUser.Id;
            var model = new List<ProjectFeedModel>();
            var list = _repo.GetProjectFeedCreatedBy(loggedUser.Id);

            if (list != null)
            {
                foreach (var item in list)
                {
                    model.Add(new ProjectFeedModel(item));
                }
                ViewData["Message"] = "No Feeds Available.";

            }
            return View(@"/Views/Home/ProjectFeedList.cshtml", model);
                        
        }

        [HttpGet]
        [Route("assign/{id}/{accId}")]
        public ActionResult AssignProject(string id, string accId)
        {
           var result =  _repo.AssignProject(id, accId);

            return RedirectToAction("Index");
        }

        private CurrentUser LoggedUser()
        {

            var claims = HttpContext.User.Claims;
            var obj = new CurrentUser();
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                    obj.Id = claim.Value;
                if (claim.Type == ClaimTypes.Name)
                    obj.Name = claim.Value;
                if (claim.Type == ClaimTypes.Role)
                    obj.AccountType = Convert.ToInt32(claim.Value);
                if (claim.Type == ClaimTypes.Email)
                    obj.Email = claim.Value;

            }
            return obj;
        }
    }
}