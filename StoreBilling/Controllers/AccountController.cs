using System.Web.Mvc;
using System.Web.Security;

namespace StoreBilling.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")] // Prevents browser caching
        public ActionResult Login()
        {
            // If already logged in, go to dashboard
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        // [ValidateAntiForgeryToken] <--- REMOVED to prevent cache mismatch errors
        public ActionResult Login(string username, string password, bool rememberMe = false)
        {
            // If already logged in (e.g., from a different tab), sign out first
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            if (username == "admin" && password == "admin123")
            {
                FormsAuthentication.SetAuthCookie(username, rememberMe);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}