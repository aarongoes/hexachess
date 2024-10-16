using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Hexachess.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;

namespace Hexachess.Controllers
{
    [Authorize, ExcludeFromCodeCoverage]
    public class UserController : Controller
    {
        private readonly IUserLogic userLogic;

        public UserController(Factory.Factory factory)
        {
            userLogic = factory.GetUserLogic(Engine.MySql);
        }

        [AllowAnonymous]
        public IActionResult LoginRegister()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (!ModelState.IsValid) return View(model);

            // Then try to find the user; if not exists we will not try to login
            var user = userLogic.Read(new KeyValuePair<string, object>("Name",model.Username));
            if (user == null)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View("LoginRegister", model);
            }

            // INFO: be smart: use whichever standard implementation the framework provides. Most of the time it is better then what we can come up with)
            var hasher = new PasswordHasher<User>();
            if (hasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View("LoginRegister", model);
            }
          
            // INFO: Claims are use to specify properties of an Identity (a user)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name)
            };
 
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsIdentity.AddClaim(new Claim("Token", user.Token.ToString()));
            var authProperties = new AuthenticationProperties();
 
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            
            var hasher = new PasswordHasher<User>();

            if (userLogic.Exists(new KeyValuePair<string, object>("Name", model.Username)))
            {
                ModelState.AddModelError("", "The user name provided has been taken.");
                return View("LoginRegister", model);
            }
            
            var user = userLogic.Create(model.Username, hasher.HashPassword(userLogic.CreateEmtpy(), model.Password));
            userLogic.Add(user);
            
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            
            return RedirectToAction("LoginRegister");
        }

        // GET: /<controller>/
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("LoginRegister");
        }
    }
}