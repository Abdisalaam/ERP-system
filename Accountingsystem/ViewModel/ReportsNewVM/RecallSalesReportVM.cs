using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class RecallSalesReportVM
    {

        public int Counter { get; set; }
        public Company company { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public DateTime RecallDate { get; set; }
        public string RecallDeadLine { get; set; }
        public string InvoiceCode { get; set; }
        public decimal Recalltotal { get; set; }
        public decimal RecalltotalTax { get; set; }
        public decimal RecalltotalafterTax { get; set; }
        public decimal TotalOFRecallamount { get; set; }
        public decimal TotalOFRecalltax { get; set; }
        public decimal TotalOFrecalltotalaftertax { get; set; }
        public string Invoice_Serial { get; set; }
        public string Emp_Name { get; set; }
    }
}