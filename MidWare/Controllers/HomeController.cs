using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MidWare.Models;
using Domain.NoSql.Data.DomainRepository;
using Domain.NoSql.Data.DomainEntites;
using Microsoft.AspNetCore.Http;

namespace MidWare.Controllers
{
    // [Route("[controller]")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IProjectFeedRepository _repo;
        private readonly IAccountRepository _accRepo;

        public HomeController()
        {
            _repo = new ProjectFeedRepository();
            _accRepo = new AccountRepository();
        }

        public IActionResult Index()
        {
            return RedirectToAction("SignIn", "Account");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult GetProjectFeeds()
        {
            var model = new List<ProjectFeedModel>();
            var list = _repo.GetProjectFeeds();

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
        [Route("MyFeed")]
        public ActionResult MyFeed()
        {
            var accountType = HttpContext.Session.GetString("accountType");
            var model = new List<ProjectFeedModel>();
            var list = _repo.GetProjectFeedByType(Convert.ToInt32(accountType));

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

        public IActionResult CreateProjectFeed()
        {
            var vm = new 
            {
                AccountTypes = new List<ProjectTypeModel>
                    {
                        new ProjectTypeModel {Id = 1, ProjectType = "Mitigation"},
                        new ProjectTypeModel {Id = 2, ProjectType = "Litigation"},
                        new ProjectTypeModel {Id = 3, ProjectType = "Adjuster"},
                        new ProjectTypeModel {Id = 4, ProjectType = "Restoration"},
                    }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProjectFeed(CreateProjectFeedModel model)
        {
            ViewData["Message"] = "Create Feed page.";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accountId = HttpContext.Session.GetString("accountId");
            var accountEmail = HttpContext.Session.GetString("accountEmail");

            if (accountId == null)
            {
                return RedirectToAction("/Account/SignIn");
            }

            var projectDto = new ProjectFeed()
            {
                Type = model.Type,
                Title = model.Title,
                SkillLevel = model.SkillLevel,
                Budget = model.Budget,
                Name = model.Name,
                Address = model.Address,
                Details = model.Details,

                CreatedById = accountId,
                CreatedByEmail = accountEmail

            };

            var result = _repo.AddProject(projectDto);
            if (result)
            {
                return RedirectToAction("GetProjectFeeds");
            }
            return View(@"/Views/Home/CreateProjectFeed.cshtml", model);
        }


        public IActionResult ProjectFeedDeatil(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("GetProjectFeeds");

            var model = new ProjectFeedDetailModel();

            var projectFeed = _repo.GetProjectById(id);

            if (projectFeed != null)
            {
                model.ProjectFeed = new ProjectFeedModel(projectFeed);

                var account = _accRepo.GetUserById(projectFeed.CreatedById);
                //var feedback = _accrepo.GetUserById(projectFeed.CreatedById);

                model.Account = new AccountModel(account);
            }
                      

            return View(@"/Views/Home/ProjectFeedDetail.cshtml", model);
        }
        //GET: Home/AwardedProjects/5
        public IActionResult AwardedProjects(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("GetProjectFeeds");
            var model = new List<ProjectFeedModel>();
            var list = _repo.GetAwardedProjectByAccountId(id);

            if (list != null)
            {
                foreach (var item in list)
                {
                    model.Add(new ProjectFeedModel(item));
                }
                ViewData["Message"] = "No Feeds Available.";
            }
            return View(@"/Views/Home/MarkProjectDone.cshtml", model);
        }

        //GET: Home/DoneProject/5
        public IActionResult DoneProject(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("GetProjectFeeds");
            var success = _repo.UpdateProjecActiveStatus(id, false);

            ViewData["Message"] = "Job marked as comeplete.";
            return View(@"/Views/Home/ProjectFeedList.cshtml");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
