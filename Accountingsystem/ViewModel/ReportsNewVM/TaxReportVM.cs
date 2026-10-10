using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TaxReportVM
    {
        [Required(ErrorMessage = "ادخل التاريخ")]
        public string Date { get; set; }
        public List<TaxReportDetailVM> taxReportDetail { get; set; }
        public decimal TotalPurchaseTax { get; set; }
        public decimal TotalPurchase { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueTax { get; set; }
        public decimal DifferenceBetweenRevenueAndTax { get; set; }
        public Company company { get; set; }

    }
}