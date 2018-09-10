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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MidWare.Controllers
{
    // [Route("[controller]")]
    [Authorize]
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Account");
            }
            var loggedUser = LoggedUser();
            ViewData["accId"] = loggedUser.Id;
            ViewData["accType"] = loggedUser.AccountType;
            if (string.IsNullOrEmpty(loggedUser.Id))
            {
                return RedirectToAction("SignIn", "Account");

            }
            if (loggedUser.AccountType == 1)
            {
                return RedirectToAction("Index", "Carrier");
            }
            else
            {
                return RedirectToAction("GetProjectFeeds");
            }

        }

        public IActionResult GetProjectFeeds()
        {
            var loggedUser = LoggedUser();
            ViewData["accId"] = loggedUser.Id;
            ViewData["accType"] = loggedUser.AccountType;
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
            var loggedUser = LoggedUser();
            ViewData["accId"] = loggedUser.Id;
            ViewData["accType"] = loggedUser.AccountType;
            var model = new List<ProjectFeedModel>();
            List<ProjectFeed> list = null;
            if (loggedUser.AccountType == 1)
            {
                list = _repo.GetProjectFeedCreatedBy(loggedUser.Id);
            }
            else
            {
                list = _repo.GetAwardedProjectByAccountId(loggedUser.Id);
            }


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
            var vm = new CreateProjectFeedModel()
            {
                ProjectTypes = new List<ProjectTypeModel>
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

            model.ProjectTypes = new List<ProjectTypeModel>
                    {
                        new ProjectTypeModel {Id = 1, ProjectType = "Mitigation"},
                        new ProjectTypeModel {Id = 2, ProjectType = "Litigation"},
                        new ProjectTypeModel {Id = 3, ProjectType = "Adjuster"},
                        new ProjectTypeModel {Id = 4, ProjectType = "Restoration"},
                    };

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var loggedUser = LoggedUser();
            if (loggedUser.Id == null)
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

                CreatedById = loggedUser.Id,
                CreatedByEmail = loggedUser.Email

            };

            var result = _repo.AddProject(projectDto);
            if (result)
            {
                return RedirectToAction("Index", "Carrier");
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

        [HttpGet]
        public IActionResult SubmitReport(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("GetProjectFeeds");

            var model = new SubmitReportModel();

            var projectFeed = _repo.GetProjectById(id);

            if (projectFeed != null)
                model.ProjectFeed = new ProjectFeedModel(projectFeed);

            var list = _repo.GetProjectFeeds();

            if (list != null)
            {
                foreach (var item in list)
                {
                    model.ProjectFeedList.Add(new ProjectFeedModel(item));
                }
            }

            model.AddInvoice = new AddInvoice()
            {
                ProjectFeedId = id
            };
            model.AddReport = new AddReport()
            {
                ProjectFeedId = id
            };

            if (projectFeed != null)
                model.ProjectFeed = new ProjectFeedModel(projectFeed);

            return View(@"/Views/Home/SubmitReport.cshtml", model);
        }

        [HttpPost]
        public IActionResult SubmitReport(AddReport model)
        {
            if(ModelState.IsValid)
            {
                _repo.AddReport(model.ProjectFeedId, model.Report, "");
                return RedirectToAction("MyFeed");
            }

            return RedirectToAction("SubmitReport", model.ProjectFeedId);
        }

        [HttpPost]
        public IActionResult SubmitInvoice(AddInvoice model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddInvoice(model.ProjectFeedId, model.Invoice, "");
                return RedirectToAction("MyFeed");
            }

            return RedirectToAction("SubmitReport", model.ProjectFeedId);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
