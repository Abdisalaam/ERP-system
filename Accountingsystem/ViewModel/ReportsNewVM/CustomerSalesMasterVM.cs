using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class CustomerSalesMasterVM
    {
        public Customer customer { get; set; }
        public List<InvoiceDetailOfCustomerVM> customerDetail { get; set; }
        public decimal cutomerBalance { get; set; }
    }
}