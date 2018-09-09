using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MidWare.Models;
using Domain.NoSql.Data.DomainRepository;
using Domain.NoSql.Data.DomainEntites;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Microsoft.eShopWeb.Web.Controllers
{
    [Route("[controller]")]
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repo;
        public AccountController()
        {
            _repo = new AccountRepository();
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public IActionResult Get(string id)
        {
            var accountType = HttpContext.Session.GetString("accountType");
            if (string.IsNullOrEmpty(id))
            {
                if (Convert.ToInt32(accountType) == 1) // carrier
                    return RedirectToAction("Index", "Carrier");
                else // others
                    return RedirectToAction("GetProjectFeeds", "Home");
            }

            var account = _repo.GetUserById(id);

            var model = new AccountModel(account);

            return View(@"/Views/Account/AccountDetail.cshtml", model);
        }

        // GET: /Account/SignIn 
        [HttpGet]
        [Route("SignIn")]
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!String.IsNullOrEmpty(returnUrl) && returnUrl.IndexOf("checkout", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ViewData["ReturnUrl"] = "/Home/Index";
            }



            return View();
        }

        // POST: /Account/SignIn
        [HttpPost]
        [Route("SignIn")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var result = _repo.GetUser(model.Email, model.Password);
            if (result != null)
            {
                // set session value
                HttpContext.Session.SetString("accountId", Convert.ToString(result.Id));
                HttpContext.Session.SetString("accountEmail", Convert.ToString(result.Email));
                HttpContext.Session.SetString("accountType", Convert.ToString(result.Type));
                HttpContext.Session.SetString("accountName", Convert.ToString(result.Name));

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, result.Name));
                identity.AddClaim(new Claim(ClaimTypes.Email, result.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, result.Type.ToString()));
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });


                
                if (result.Type == 1) // carrier
                    return RedirectToAction("Index", "Carrier");
                else // others
                    return RedirectToAction("GetProjectFeeds", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var vm = new RegisterViewModel
            {
                AccountTypes = new List<AccountTypeModel>
                    {
                        new AccountTypeModel {Id = 1, AccountType = "Carrier"},
                        new AccountTypeModel {Id = 2, AccountType = "Mitigation"},
                        new AccountTypeModel {Id = 3, AccountType = "Litigation"},
                        new AccountTypeModel {Id = 4, AccountType = "Adjuster"},
                        new AccountTypeModel {Id = 5, AccountType = "Restoration"},
                    }
            };

            return View(vm);
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = _repo.GetUserByEmail(model.Email);
                if (user == null)
                {
                    var account = new Account
                    {
                        Name = model.Name,
                        Email = model.Email,
                        ContactNumber = model.ContactNumber,
                        Password = model.Password,
                        Type = model.Type

                    };
                    var result = _repo.AddAccount(account);
                    if (result)
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToLocal("/Home/Index");
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Route("SignOut")]
        [AllowAnonymous]
        public IActionResult SignOut(string returnUrl = null)
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(returnUrl);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
