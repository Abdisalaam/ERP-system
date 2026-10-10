using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class AllPurchaseInvoicesWithTaxVM
    {
        public string startDate { get; set; }
        public string EndDate { get; set; }
        public List<PurInvoiceMaster> PurchaseMasterWithTax { get; set; }
        public List<PurInvoiceMaster> PurchaseMasterWithOut { get; set; }
        public Company company { get; set; }
        public decimal totalBeforeTax { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalAfterTax { get; set; }
    }
}