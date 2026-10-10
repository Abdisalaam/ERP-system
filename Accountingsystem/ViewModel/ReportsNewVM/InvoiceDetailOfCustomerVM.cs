using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class InvoiceDetailOfCustomerVM
    {
        public Product Product { get; set; }
       
        public DateTime Invoice_date { get; set; }
        public string transtype { get; set; }
        public string Invoice_Serial { get; set; }
        public string Description { get; set; }
        public decimal quantity { get; set; }
        public decimal Price_unit { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal totalCost { get; set; }


    }
}