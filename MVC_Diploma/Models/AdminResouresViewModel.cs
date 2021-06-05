using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Diploma.Models
{
    public class AdminResouresViewModel
    {
        public Service Service { get; set; }
        public ServiceType ServiceType { get; set; }
        public List<string> Types { get; set; }
    }
}