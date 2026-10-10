using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class UnpaidInvoiceMasterVM
    {
        public Customer customer { get; set; }
        public List<UnpaidInvoiceVM> details { get; set; } 
    }
}