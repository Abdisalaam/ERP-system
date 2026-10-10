using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesReportVM
    {
        public int Counter { get; set; }
        public Company company { get; set; }
        public string paymentmethodName { get; set; }
        public decimal quantity { get; set; }
        public decimal PayedAmount { get; set; }
        public string branchName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime invoiceDeadLine { get; set; }
        public string InvoiceCode { get; set; }
        public decimal invoiceTotal { get; set; }
        public decimal RecallInvoicetotal { get; set; }
        public decimal invoicetotalAfterTax { get; set; }
        public decimal TotalOFInvoicesamount { get; set; }
        public decimal TotalOFInvoicesRecall { get; set; }
        public decimal TotalOFInvoicesBefor { get; set; }
        public decimal TotalCost { get; set; }

        public decimal totaltax { get; set; }
        public decimal totalafterfixed { get; set; }

        public string CustomerNameForSalse { get; set; }
        public string CustomerCodeForSalse { get; set; }
        public string InvoiceDateForSalse { get; set; }
        public string InvoiceCodeForSalse { get; set; }
        public decimal invoiceTotalForSalse { get; set; }
        public decimal RecallInvoicetotalForSalse { get; set; }
        public decimal invoicetotalAfterTaxForSalse { get; set; }
        public decimal TotalOFInvoicesamountForSalse { get; set; }
        public decimal TotalOFInvoicesRecallForSalse { get; set; }
        public decimal TotalOFInvoicesBeforForSalse { get; set; }
        public decimal TotalCostForSalse { get; set; }

        public decimal totaltaxForSalse { get; set; }
        public decimal totalafterfixedForSalse { get; set; }
        public List<DetailInvoiceVM> detail { get; set; }
        public int Store { get; set; }
        public int Branch { get; set; }
        public int employee { get; set; }
        public Boolean Remove { get; set; }
        public decimal TotalClassFiyAmount { get; set; }
        public decimal TotalClassFiyCost { get; set; }

    }
}