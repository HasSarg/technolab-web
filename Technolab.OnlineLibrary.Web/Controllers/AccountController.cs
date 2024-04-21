using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technolab.OnlineLibrary.Web.Models;

namespace Technolab.OnlineLibrary.Web.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.Users)]
    public class AccountController : Controller
    {
        public AccountController(ILibraryDbContextFactory contextFactory)
        {
            this.ContextFactory = contextFactory;
        }

        public ActionResult Index() 
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string newPassword, string confirmPassword)
        {
            using var context = ContextFactory.Create();

            if (newPassword == confirmPassword && newPassword.Length >= 8)
            {
                var user = context.Users.Single(x => x.Username == User.GetUsername());
                user.Password = newPassword;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        private ILibraryDbContextFactory ContextFactory { get; }
    }
}
