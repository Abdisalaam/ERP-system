using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class PurchaseReportVM
    {
        public int Counter { get; set; }
        public Company company { get; set; }
        public string supplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierTaxNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string invoiceDeadLine { get; set; }
        public string  InvoiceCode { get; set; }
        public decimal invoiceTotal { get; set; }
        public decimal RecallInvoicetotal { get; set; }
        public decimal invoicetotalAfterTax { get; set; }
        public decimal TotalOFInvoicesamount { get; set; }
        public decimal TotalOFInvoicesRecall { get; set; }
        public decimal TotalOFInvoicesBefor { get; set; }

        public decimal totaltax { get; set; }
        public decimal totalafterfixed { get; set; }




    }
}