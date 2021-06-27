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
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (IsAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
        }
        public Boolean IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public ActionResult Mark (string requestId)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var allrequests = context.Requests.ToList();
            
            var currentRequests = allrequests.Where(r => r.RequestId == requestId).FirstOrDefault();
            var master = context.Users.Where(i => i.Id == currentRequests.ManagerId).FirstOrDefault();
            MarkInfo markInfo = new MarkInfo()
            {
                Description = currentRequests.Description,
                MasterId = currentRequests.ManagerId,
                MasterUserName = master.UserName,
                RequestId = requestId
            };
            return View(markInfo);
        }

        public ActionResult CalculateRep(MarkInfo markInfo)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var master = context.Users.Where(u => u.Id == markInfo.MasterId).FirstOrDefault();
            var reputation = context.Reputation.Where(r => r.ReputationId == master.ReputationId).FirstOrDefault();
            var requestMark = context.Requests.Where(i => i.RequestId == markInfo.RequestId).FirstOrDefault();
            requestMark.UserMark = true;
            if (reputation.Value == 0)
            {
                reputation.Value = markInfo.Value;
                reputation.NumberOfVotes++;
                context.SaveChanges();
            }
            else if (reputation.Value != 0)
            {
                var temp = reputation.Value * reputation.NumberOfVotes;
                reputation.NumberOfVotes++;
                var currentValue = (temp + markInfo.Value) / (reputation.NumberOfVotes);
                reputation.Value = currentValue;
                context.SaveChanges();
            }
            return RedirectToAction("UserPage", "Home");

        }
    }
}