using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SubLedgerMovList
    {
        public DateTime GLdate { get; set; }
        public String Description { get; set; }
        public String GL_Type { get; set; }
        public string serialCode { get; set; }
        public int Count { get; set; }
        public decimal Credit { get; set; }
        public decimal Depite { get; set; }
        public decimal Amount { get; set; }
        public Boolean IsRevenue { get; set; }
        public string Account_Name { get; set; }
        public ChartOfAccounting Account{ get; set; }
        public string Child_Name { get; set; }


    }
}