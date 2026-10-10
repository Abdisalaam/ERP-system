using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class CustomerInfoVM
    {
        public Company company { get; set; }
        public string Balance { get; set; }
        public string Email { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public Customer customer { get; set; }
    }
}