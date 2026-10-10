using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SalesReceiptDetailsTempVM
    {
        public List<SalesReceiptDetailsTemp> invoiceTemp { get; set; }
        public int invoice_Id { get; set; }
        public decimal AllowDiscount { get; set; }
        public decimal TotalNewTaxs { get; set; }
    }
}