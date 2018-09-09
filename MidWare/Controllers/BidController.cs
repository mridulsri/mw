using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MidWare.Models;
using Domain.NoSql.Data.DomainRepository;
using Domain.NoSql.Data.DomainEntites;
using System.Security.Claims;

namespace MidWare.Controllers
{
    // [Route("[controller]/[action]")]
    [Authorize]
    public class BidController : Controller
    {
        private readonly IBidRepository _repoBid;
        private readonly IProjectFeedRepository _repoProjectFeed;
        public BidController()
        {
            _repoBid = new BidRepository();
            _repoProjectFeed = new ProjectFeedRepository();
        }
        // GET: Bid
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Account");
            }
            //return View();
            return View("/Views/Home/BidProposal.cshtml");
        }

        // GET: Bid/Details/5
        public ActionResult Details(string id)
        {
            var model = new ProjectFeedDetailModel();
            TempData["projectId"] = id;

            var projectFeed = _repoProjectFeed.GetProjectById(id);
            

            if (projectFeed != null)
            {
                model.ProjectFeed = new ProjectFeedModel(projectFeed);
                //var account = _repoProjectFeed.GetUserById(projectFeed.CreatedById);
                //var feedback = _accrepo.GetUserById(projectFeed.CreatedById);

                // model.Account = new AccountModel(account);
            }
            return View("/Views/Home/BidProposal.cshtml");
        }


        // POST: Bid/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult BidProposal(BidViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var loggedUser = LoggedUser();
            Bid param = new Bid();
            param.BidDate = DateTime.Now;
            //TODO: Add user information in BidBy
            param.BidBy = loggedUser.Id;
            param.BidValue = model.BidValue;
            param.Duration = model.Duration;
            param.Comment = model.Comment;
            //param.ProjectId = (object)TempData["projectId"];
            var result = _repoProjectFeed.UpdateProject(param, TempData["projectId"].ToString());
            ViewData["Message"] = "Data successfully updated.";

            return RedirectToAction("GetProjectFeeds", "Home");
        }


        private CurrentUser LoggedUser()
        {
            if (HttpContext == null)
                return null;
            var claims = HttpContext.User.Claims;
            var obj = new CurrentUser();
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    obj.Id = claim.Value;
                    LoggedInUser.Id = obj.Id;
                }
                if (claim.Type == ClaimTypes.Name)
                {
                    obj.Name = claim.Value;
                    LoggedInUser.Name = obj.Name;
                }
                if (claim.Type == ClaimTypes.Role)
                {
                    obj.AccountType = Convert.ToInt32(claim.Value);
                    LoggedInUser.AccountType = obj.AccountType;
                }
                if (claim.Type == ClaimTypes.Email)
                {
                    obj.Email = claim.Value;
                    LoggedInUser.Email = obj.Email;
                }
            }
            return obj;
        }
    }
}