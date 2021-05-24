using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MVC_Diploma.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Diploma.Startup))]
namespace MVC_Diploma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }
        // In this method we will create default User roles and Admin user for login    
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser
                {
                    UserName = "shanu",
                    Email = "syedshanumcain@gmail.com"
                };

                string userPWD = "A@Z200711";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole
                {
                    Name = "Manager"
                };
                roleManager.Create(role);

            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
                roleManager.Create(role);

            }
            // creating Creating Laywer role     
            if (!roleManager.RoleExists("Laywer"))
            {
                var role = new IdentityRole
                {
                    Name = "Laywer"
                };
                roleManager.Create(role);

            }
            // creating Creating Programer role     
            if (!roleManager.RoleExists("Programer"))
            {
                var role = new IdentityRole
                {
                    Name = "Programer"
                };
                roleManager.Create(role);

            }
            // creating Creating Inspector role     
            if (!roleManager.RoleExists("Inspector"))
            {
                var role = new IdentityRole
                {
                    Name = "Inspector"
                };
                roleManager.Create(role);

            }
            // creating Creating HeadMaster role     
            if (!roleManager.RoleExists("HeadMaster"))
            {
                var role = new IdentityRole
                {
                    Name = "HeadMaster"
                };
                roleManager.Create(role);

            }
            // creating Creating Counter role     
            if (!roleManager.RoleExists("Counter"))
            {
                var role = new IdentityRole
                {
                    Name = "Counter"
                };
                roleManager.Create(role);

            }


        }
    }
}
