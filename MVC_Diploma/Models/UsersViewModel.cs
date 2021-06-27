using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class MarkInfo
    {
        public string MasterUserName { get; set; }
        public string Description { get; set; }
        public string RequestId { get; set; }
        public string MasterId { get; set; }
        [Range(0.00, 5.00)]
        public decimal Value { get; set; }
    }
}