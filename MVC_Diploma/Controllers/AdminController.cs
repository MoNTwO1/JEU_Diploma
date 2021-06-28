using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC_Diploma.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public ActionResult Search(string query)
        {
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
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
            return View("Index", userWithRoles.Where(n => n.Email == query));
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Export()
        {
            
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Worker statistic");

                worksheet.Cell("A1").Value = "Номер заявки";
                worksheet.Cell("B1").Value = "Мастер";
                worksheet.Cell("C1").Value = "Тип заявки";
                worksheet.Cell("D1").Value = "Описание";
                worksheet.Cell("E1").Value = "Стоимость заявки";
                worksheet.Row(1).Style.Font.Bold = true;

                var context = new ApplicationDbContext();
                var requests = context.Requests.Where(u => u.Status == "Заявка выполнена").ToList();
                List<ExportInfo> exportInfos = new List<ExportInfo>();
                foreach (var request in requests)
                {
                    var requestId = request.RequestId;
                    var master = context.Users.FirstOrDefault(u => u.Id == request.ManagerId).UserName;
                    var service = context.Service.FirstOrDefault(u => u.ServiceId == request.ServiceId);
                    var serviceType = context.ServiceType.FirstOrDefault(u => u.ServiceTypeId == service.ServiceTypeId).Type;
                    var desc = request.Description;
                    var price = service.MoneyForService;
                    exportInfos.Add(new ExportInfo
                    {
                        RequestId = requestId,
                        Master = master,
                        TypeOfRequest = serviceType,
                        Description = desc,
                        Price = price,
                    });
         
                }

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < exportInfos.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = exportInfos[i].RequestId;
                    worksheet.Cell(i + 2, 2).Value = string.Join(", ", exportInfos[i].Master);
                    worksheet.Cell(i + 2, 3).Value = exportInfos[i].TypeOfRequest;
                    worksheet.Cell(i + 2, 4).Value = exportInfos[i].Description;
                    worksheet.Cell(i + 2, 5).Value = exportInfos[i].Price;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"requests_Report_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }

        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase fileExcel)
        {
            if (ModelState.IsValid)
            {

                var errors = 0;
                using (XLWorkbook workBook = new XLWorkbook(fileExcel.InputStream, XLEventTracking.Disabled))
                {
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {

                        foreach (IXLColumn column in worksheet.ColumnsUsed().Skip(1))
                        {

                            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                            {
                                try
                                {
                                    RegisterViewModel viewModel = new RegisterViewModel();
                                    viewModel.Email = row.Cell(1).Value.ToString();
                                    viewModel.UserName = row.Cell(2).Value.ToString();
                                    viewModel.Password = row.Cell(3).Value.ToString();
                                    viewModel.ConfirmPassword = row.Cell(3).Value.ToString();
                                    RedirectToAction("ToRegister","Admin", new { model = viewModel });

                                }
                                catch (Exception e)
                                {
                                    //logging
                                    errors++;
                                }
                            }

                        }
                    }
                }
            }
            return RedirectToAction("AdminPage", "Home" );
        }

        [HttpPost]
        public ActionResult ToRegister(RegisterViewModel model)
        {
            return RedirectToAction("Register", "Account", new { model = model });
        }
    }
}
