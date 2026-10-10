using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class DetailInvoiceVM
    {
        public int PrdouctId { get; set; }
        public int CategoryID { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
    }
}