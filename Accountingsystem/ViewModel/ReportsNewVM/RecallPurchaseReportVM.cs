using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class RecallPurchaseReportVM
    {
        public int Counter { get; set; }
        public Company company { get; set; }
        public string supplierName { get; set; }
        public string SupplierCode { get; set; }
        public DateTime RecallDate { get; set; }
        public string RecallDeadLine { get; set; }
    
        public string RecallPuchaseCode { get; set; }
        public string PurchaseCode { get; set; }
        public decimal Recalltotal { get; set; }
        public decimal RecalltotalTax { get; set; }
        public decimal RecalltotalafterTax { get; set; }
        public decimal TotalOFRecallamount { get; set; }
        public decimal TotalOFRecalltax { get; set; }
        public decimal TotalOFrecalltotalaftertax { get; set; }
    }
}