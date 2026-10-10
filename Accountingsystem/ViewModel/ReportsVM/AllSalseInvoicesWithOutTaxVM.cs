using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class AllSalseInvoicesWithOutTaxVM
    {
        public string startDate { get; set; }
        public string EndDate { get; set; }
  
        public List<InvoiceMaster> SalesInvoiceMasterWithOut { get; set; } 
        public List<SalesReceiptMaster> SalesReciptWithOut { get; set; }
        public decimal totalBeforeTaxSalesReceipt { get; set; }
        public decimal totalTaxSalesReceipt { get; set; }
        public decimal totalAfterTaxSalesReceipt { get; set; }

        public decimal totalBeforeTaxSales { get; set; }
        public decimal totalTaxSales { get; set; }
        public decimal totalAfterTaxSales { get; set; }

        public decimal totalBeforeTax { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalAfterTax { get; set; }
        public Company company { get; set; }

        [Required(ErrorMessage = "من فضلك اختر الفرع")]
        public int branchId { get; set; }
        [Required(ErrorMessage = "من فضلك اختر الفرع")]
        public SelectList branchList { get; set; }
    }
}