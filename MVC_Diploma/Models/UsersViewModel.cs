using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Diploma.Models
{
    public class UsersViewModel
    {
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string Username  { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class UserEditModel
    {
        public ApplicationUser user { get; set; }
        public IList<string> Roles { get; set; }
        public List<IdentityRole> allRoles { get; set; }

    }
}