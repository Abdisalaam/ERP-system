using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TaxReportDetailVM
    {
        public string Tax_Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal TaxBallance { get; set; }
        public decimal PurchaseBallance { get; set; }
        public decimal SaleseBallance { get; set; }
        public bool isPurchase { get; set; }
        public int  Tax_Id { get; set; }
    }
}