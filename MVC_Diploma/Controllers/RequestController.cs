using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            var date = DateTime.UtcNow;
            var service = context.Service.Single(up => up.ServiceId == serviceId);

            Requests Request = new Requests()
            {
                RequestId = requestId,
                UserId = userId,
                ServiceId = serviceId,
                Description = service.Description,
                Date = date
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
            try
            {
                foreach (var user in users)
                {
                    foreach (var rep in reputation)
                    {
                        if (user.ReputationId != null)
                        {
                            if (user.ReputationId.ToString() == rep.ReputationId)
                            {
                                sorted.Add(new RequestsMastersInfoViewModel { masters = user, value = rep.Value, requestId = requestId, serviceId = serviceId });
                            }
                        }
                        
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            
            MastersInfoViewModel masters = new MastersInfoViewModel(sorted.OrderBy(u => u.value).ToList());
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
            admitInfo.RequestId = requestId;

            return View(admitInfo);
        }
        public ActionResult Greets(string requestId)
        {
            var payment = new Payment();
            payment.RequestId = requestId;
            return View("Greets", payment);
        }

        public ActionResult Details(string requestId)
        {
            var context = new ApplicationDbContext();
            var requestDetailViewModel = new RequestDetailView();
            requestDetailViewModel.Request = context.Requests.Single(up => up.RequestId == requestId);
            requestDetailViewModel.Service = context.Service.SingleOrDefault(s => s.ServiceId == requestDetailViewModel.Request.ServiceId);
            requestDetailViewModel.ServiceType = context.ServiceType.SingleOrDefault(s => s.ServiceTypeId == requestDetailViewModel.Service.ServiceTypeId);
            requestDetailViewModel.Master = context.Users.SingleOrDefault(u => u.Id == requestDetailViewModel.Request.ManagerId);

            return View("Request", requestDetailViewModel);
        }

        public ActionResult Payment(string requestId)
        {
            var context = new ApplicationDbContext();
            var request = context.Requests.FirstOrDefault(u => u.RequestId == requestId);
            var service = context.Service.FirstOrDefault(s => s.ServiceId == request.ServiceId);
            Order order = new Order();
            order.Id = requestId;
            order.Date = DateTime.UtcNow;
            order.Sum = service.MoneyForService;

            if (order != null)
            {
                OrderModel orderModel = new OrderModel { OrderId = request.RequestId, Sum = service.MoneyForService };
                return View(orderModel);
            }
            /*return HttpNotFound();*/
            return View();

        }

        [HttpGet]
        public string Paid()
        {
            return "<p>заказ оплачен</p>";
        }
        [HttpPost]
        public void Paid(string notification_type, string operation_id, string label, string datetime,
        decimal amount, decimal withdraw_amount, string sender, string sha1_hash, string currency, bool codepro)
        {
            string key = "xxxxxxxxxxxxxxxx"; // секретный код
                                             // проверяем хэш
            var context = new ApplicationDbContext();
            string paramString = String.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}",
                notification_type, operation_id, amount, currency, datetime, sender,
                codepro.ToString().ToLower(), key, label);
            string paramStringHash1 = GetHash(paramString);
            // создаем класс для сравнения строк
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            // если хэши идентичны, добавляем данные о заказе в бд
            if (0 == comparer.Compare(paramStringHash1, sha1_hash))
            {
                Order order = context.Orders.FirstOrDefault(o => o.Id == label);
                order.Operation_Id = operation_id;
                order.Date = DateTime.Now;
                order.Amount = amount;
                order.WithdrawAmount = withdraw_amount;
                order.Sender = sender;
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public string GetHash(string val)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(val));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public ActionResult ConfirmedRequest()
        {
            return View("ConfirmedRequest");
        }
    }

}
