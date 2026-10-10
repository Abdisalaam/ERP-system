using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintProjectInvoiceDetailsVM
    {
        public int Detail_ID { get; set; }
        public string Product_Name { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal totalamount { get; set; }
        public Tax tax { get; set; }

        public decimal Price { get; set; }
        public decimal TotalBeforTax { get; set; }
        public decimal DetailDiscount { get; set; }
    }
}