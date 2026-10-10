using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MasterPOSInvoiceDetailTempVM
    {
        public List<DetailPOSInvoiceTemp> invoiceTemp{ get; set; }
        public int invoice_Id { get; set; }
        public decimal AllowDiscount { get; set; }
        public decimal TotalNewTax { get; set; }
        public SelectList TaxList { get; set; }

    }
}