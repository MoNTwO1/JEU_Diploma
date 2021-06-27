using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Diploma.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var amountRequests = context.Requests.Where(s => s.Status == "Заявка ждет подтверждения").ToList();
            ManagerViewModel managerViewModel = new ManagerViewModel() { requests = amountRequests };
            return View(managerViewModel);
        }

        // GET: Manager/Details/5
        public ActionResult Details(string requestId)
        {
            return View();
        }

        public ActionResult Admit(string requestId)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var currentRequest = context.Requests.Where(i => i.RequestId == requestId).FirstOrDefault();
            currentRequest.Status = "Заявка подтверждена диспетчером";
            context.SaveChanges();
            var amountRequests = context.Requests.Where(s => s.Status == "Заявка ждет подтверждения").ToList();
            ViewBag.Message = "Заявка успешно подтвержддена";
            ManagerViewModel managerViewModel = new ManagerViewModel() { requests = amountRequests };
            return View("Index", managerViewModel);
        }

        public ActionResult Admited()
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var amountRequests = context.Requests.Where(s => s.Status == "Заявка подтверждена диспетчером").ToList();
            ManagerViewModel managerViewModel = new ManagerViewModel() { requests = amountRequests };
            return View(managerViewModel);
        }
    }
}
