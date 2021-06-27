using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Diploma.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_Diploma.Controllers
{
    public class AdminController : Controller
    {
        /*private readonly RoleManager<IdentityRole> _roleManager;*/

        // GET: Admin
        public ActionResult Index(ICollection<UserInfo> userInfos)
        {
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var users = userManager.Users.ToList();
            var userWithRoles = userManager.Users.Include(u => u.Roles).ToList()
                .Select(u => new UserInfo { Email = u.Email, 
                    Roles = userManager.GetRoles(u.Id), 
                    UserId = u.Id.ToString(), 
                    Username = u.UserName})
                .ToList();
            userInfos = userWithRoles;
            return View(userInfos);
        }

        [HttpGet]
        public ActionResult EditUserView(string userId, string userName)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roles = roleManager.Roles.OrderBy(u => u.Name).ToList();

            return View(new UserEditModel { user = userManager.FindByNameAsync(userName).Result, 
                Roles = userManager.GetRoles(userId),
                allRoles = roles });
        }

        public ActionResult SaveUser(UserEditModel model)
        {
            var context = new ApplicationDbContext();
            var user = context.Users.Single(u => u.Id == model.user.Id);
            user.UserName = model.user.UserName;
            user.Email = model.user.Email;
            user.PhoneNumber = model.user.PhoneNumber;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var roles = userManager.GetRoles(model.user.Id);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            userManager.AddToRoles(user.Id, "Master");
            context.SaveChanges();
            var rolesList = roleManager.Roles.OrderBy(u => u.Name).ToList();
            var userStore = new UserStore<ApplicationUser>(context);
            var users = userManager.Users.ToList();
            var userWithRoles = userManager.Users.Include(u => u.Roles).ToList()
                .Select(u => new UserInfo
                {
                    Email = u.Email,
                    Roles = userManager.GetRoles(u.Id),
                    UserId = u.Id.ToString(),
                    Username = u.UserName
                })
                .ToList();
            var userInfos = userWithRoles;
            return View("Index", userInfos);
        }

        public ActionResult ServiceCreation()
        {
            var context = new ApplicationDbContext();
            var servicesTypes = context.ServiceType.Select(u => u.Type).ToList();
            Service service = new Service();
            ServiceType serviceType = new ServiceType();
            return View(new AdminResouresViewModel() { Service = service, ServiceType = serviceType, Types = servicesTypes });

        }

        [HttpPost]
        public ActionResult ServiceCreation(AdminResouresViewModel model)
        {

            var context = new ApplicationDbContext();
            ServiceType serviceType = new ServiceType();
            var temp = context.ServiceType.ToList();
            bool check = false;
            foreach (var item in temp)
            {
                
                if (item.Type == model.ServiceType.Type)
                {
                    check = true;
                    
                }

            }
            if (check == false)
            {
                serviceType.Type = model.ServiceType.Type;
                serviceType.ServiceTypeId = Guid.NewGuid().ToString();
                context.ServiceType.Add(serviceType);
            }
           

            Service service = new Service();
            service.ServiceId = Guid.NewGuid().ToString();
            service.Description = model.Service.Description;
            service.ServiceTypeId = serviceType.ServiceTypeId;
            service.MoneyForService = model.Service.MoneyForService;
            context.Service.Add(service);
            context.SaveChanges();
            var servicesTypes = context.ServiceType.Select(u => u.Type).ToList();
            var resourse = new AdminResouresViewModel() { Service = service, ServiceType = serviceType, Types = servicesTypes };
            if (ModelState.IsValid)
            {
                ModelState.Clear();
            }
            return View(resourse);
        }

        public ActionResult AddUser()
        {
            ViewBag.Message = "";
            return View("AddUser");
        }

        public ActionResult Register(RegisterViewModel model)
        {
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, UserStatus = true, DateIn = DateTime.Now, DateOut = DateTime.Now };
            var result = userManager.Create(user, model.Password);
            userManager.AddToRoleAsync(user.Id, "User");
            context.SaveChanges();
            ViewBag.Message = "Пользователь добавлен";
            return View("AddUser");
        }
    }
}
