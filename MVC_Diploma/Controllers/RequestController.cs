using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Diploma.Models;

namespace MVC_Diploma.Controllers
{
    public class RequestController : Controller
    {
        /*ApplicationDbContext _context;*/
        [Authorize(Roles = "User, Admin")]
        public ActionResult Index()
        {
            var context = new ApplicationDbContext();
            var allServices = context.Service.ToList();
            var allServiceTypes = context.ServiceType.ToList();
            RequestsViewModel requestsViewModel = new RequestsViewModel() { Services = allServices,
                serviceTypes = allServiceTypes};

            return View(requestsViewModel);
        }

        public ActionResult Pick(string serviceId)
        {
            
            var context = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var reputation = context.Reputation.ToList().OrderBy(r => r.Value).ToList();
            //выбор заявки и занесение услуги в бд
            var userId = User.Identity.GetUserId();
            var requestId = Guid.NewGuid().ToString();
            var service = context.Service.Single(up => up.ServiceId == serviceId);

            Requests Request = new Requests()
            {
                RequestId = requestId,
                UserId = userId,
                ServiceId = serviceId,
                Description = service.Description
            };
            //Регистрация в бд
            context.Requests.Add(Request);
            
            
            context.SaveChanges();
            //мастеры
            /*var roleId = context.Users.Where(x => x.Roles.Select(n => n.RoleId))*/
            var users = context.Users.Where(x => x.Roles
            .Select(y => y.RoleId)
            .Contains("97ee5280-6dc6-4f71-a150-4cc3d546de03"))
                .ToList();
            List<RequestsMastersInfoViewModel> sorted = new List<RequestsMastersInfoViewModel>();
            foreach (var user in users)
            {
                foreach (var rep in reputation)
                {
                    if (user.ReputationId.ToString() == rep.ReputationId)
                    {
                        sorted.Add(new RequestsMastersInfoViewModel { masters = user, value = rep.Value, requestId = requestId, serviceId = serviceId });
                    }
                }
                
                
            }
            MastersInfoViewModel masters = new MastersInfoViewModel(sorted);
            return View("PickMaster", sorted);
        }

        public ActionResult Admit(string masterId, string requestId)
        {
            var context = new ApplicationDbContext();
            var request = context.Requests.Single(up => up.RequestId == requestId);
            request.ManagerId = masterId;
            request.Status = "Заявка ждет подтверждения";
            context.SaveChanges();

            var master = context.Users.Single(p => p.Id == masterId);
            var service = context.Service.Single(s => s.ServiceId == request.ServiceId);
            var serviceType = context.ServiceType.Single(t => t.ServiceTypeId == service.ServiceTypeId);

            AdmitInfoViewModel admitInfo = new AdmitInfoViewModel();
            admitInfo.MasterName = master.UserName;
            admitInfo.Description = request.Description;
            admitInfo.ServiceDescription = service.MoneyForService.ToString();
            admitInfo.TypeOfService = serviceType.Type;
            admitInfo.Price = service.MoneyForService.ToString();

            return View(admitInfo);
        }
        public ActionResult Greets()
        {
            return View("Greets");
        }
    }

}
