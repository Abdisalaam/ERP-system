using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesDatesVM
    {
        public string from { get; set; }
        public string to { get; set; }
        public int empId { get; set; }
        public Boolean OfferTransfer { get; set; }
        public Boolean OfferNotTransfer { get; set; }
        public List<SalesReportVM> List { get; set; } 
        public List<RecallSalesReportVM> ReversList { get; set; }
        public  List<OfferPriceReportVM> OfferList { get; set; }
    }
}