using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class LedgerListVM
    {
        public List<LedgerFromDetailVM> LedgDetails { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AccountName{ get; set; }
        public decimal totalbeforDate { get; set; }
        public string BalanceNameBefor { get; set; }
        public string BalanceNameAfter { get; set; }
        public decimal totalinDate { get; set; }
        public Company company { get; set; }

    }
}