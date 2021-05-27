using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Ubiety.Dns.Core;

namespace MVC_Diploma.Controllers
{
    public class RequestController : Controller
    {
        // GET: Request
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            Request requestBase = new Request();
            return View();
        }

        public ActionResult NewRequest()
        {
            return View();
        }
    }
}
