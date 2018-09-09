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

namespace MidWare.Controllers
{
    // [Route("[controller]/[action]")]
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
            Bid param = new Bid();
            param.BidDate = DateTime.Now;
            //TODO: Add user information in BidBy
            param.BidBy = HttpContext.Session.GetString("accountId");
            param.BidValue = model.BidValue;
            param.Duration = model.Duration;
            param.Comment = model.Comment;
            //param.ProjectId = (object)TempData["projectId"];
            var result = _repoProjectFeed.UpdateProject(param, TempData["projectId"].ToString());
            ViewData["Message"] = "Data successfully updated.";
            return View("/Views/Home/ProjectFeedList.cshtml");
        }

        // GET: Bid/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Bid/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Bid/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bid/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}