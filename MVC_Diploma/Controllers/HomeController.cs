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

        [Authorize(Roles = "Admin, User, Master, Manager")]
        public ActionResult AdminPage()
        {
            ViewBag.Message = "Admin page";

            return View("Admin");
        }

        [Authorize(Roles = "Admin, User, Master, Manager")]
        public ActionResult MasterPage()
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var id = User.Identity.GetUserId().ToString();
            var amountRequests = context.Requests.Where(s => s.Status == "Заявка подтверждена диспетчером").ToList();
            var onlyMasterRequests = amountRequests.Where(i => i.ManagerId == id).ToList();
            ManagerViewModel managerViewModel = new ManagerViewModel() { requests = onlyMasterRequests };
            return View("Master", managerViewModel);
        }

        [Authorize(Roles = "Admin, User, Master, Manager")]
        public ActionResult ManagerPage()
        {
            ViewBag.Message = "Master page";

            return View("Master");
        }

        [Authorize(Roles = "Admin, User, Master, Manager")]
        public ActionResult UserPage()
        {
            ViewBag.Message = "User page";
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var allrequests = context.Requests.ToList();
            var id = User.Identity.GetUserId().ToString();
            var userRequests = allrequests.Where(r => r.UserId.ToString() == id);

            return View("User", userRequests);
        }
        public ActionResult End(string requestId)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var currentRequest = context.Requests.Where(i => i.RequestId == requestId).FirstOrDefault();
            currentRequest.Status = "Заявка выполнена";
            context.SaveChanges();
            var id = User.Identity.GetUserId().ToString();
            var amountRequests = context.Requests.Where(s => s.Status == "Заявка подтверждена диспетчером").ToList();
            var onlyMasterRequests = amountRequests.Where(i => i.ManagerId == id).ToList();
            ManagerViewModel managerViewModel = new ManagerViewModel() { requests = onlyMasterRequests };
            return View("Master", managerViewModel);
        }

    }
}