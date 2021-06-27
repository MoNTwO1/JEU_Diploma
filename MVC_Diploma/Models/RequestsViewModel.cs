﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Diploma.Models
{
    public class RequestsViewModel
    {
        public List<Service> Services { get; set; }
        public List<ServiceType> serviceTypes { get; set; }
        public Requests request { get; set; }
    }

    public class RequestsMastersInfoViewModel
    {
        public decimal value { get; set; }
        public ApplicationUser masters { get; set; }
        public string requestId { get; set; }
        public string serviceId { get; set; }
    }

    public class MastersInfoViewModel
    {
        private List<RequestsMastersInfoViewModel> sorted;

        public MastersInfoViewModel(List<RequestsMastersInfoViewModel> sorted)
        {
            this.sorted = sorted;
        }

        public List<RequestsMastersInfoViewModel> MastersInfoViewModels { get; set; }
    }

    public class AdmitInfoViewModel
    {
        public string MasterName { get; set; }
        public string Description { get; set; }
        public string ServiceDescription { get; set; }
        public string TypeOfService { get; set; }
        public string Price { get; set; }

    }
}

    