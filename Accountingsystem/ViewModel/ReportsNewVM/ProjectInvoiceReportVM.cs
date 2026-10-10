using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectInvoiceReportVM
    {
        public Company company { get; set; }
        public string customerName { get; set; }
        public string CustomerCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceCode { get; set; }
        public decimal InvoiceAMount { get; set; }
        public decimal TaxAMount { get; set; }
        public decimal totalafterTax { get; set; }

        public int empId { get; set; }
    }
}