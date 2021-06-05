using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Diploma.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult RolesAboutView()
        {
            ViewBag.Message = "Your contact page.";

            return View("RolesAboutView");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminPage()
        {
            ViewBag.Message = "Admin page";

            return View("Admin");
        }

        [Authorize(Roles = "Master, Admin")]
        public ActionResult MasterPage()
        {
            ViewBag.Message = "Master page";

            return View("Master");
        }

        [Authorize(Roles = "User, Admin")]
        public ActionResult UserPage()
        {
            ViewBag.Message = "User page";
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var allrequests = context.Requests.ToList();
            var id = User.Identity.GetUserId().ToString();
            /*var userId = Membership.GetUser().ProviderUserKey.ToString();*/
            var userRequests = allrequests.Where(r => r.UserId.ToString() == id);

            return View("User", userRequests);
        }

    }
}