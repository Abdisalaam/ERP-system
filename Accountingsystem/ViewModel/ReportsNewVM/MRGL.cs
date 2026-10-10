using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class MRGL
    {
        public ChartOfAccounting Account { get; set; }
        public String FromDate { get; set; }
        public String Todate { get; set; }
        public Company company { get; set; }
        public List<MRGLDetail> details{get;set;}
        public int accountId { get; set; }
        public SelectList AccountList { get; set; } 
        public decimal AmountBefor { get; set; }
        public decimal AmountPeriod { get; set; }
        public bool  penddingDocument { get; set; }

    }
}