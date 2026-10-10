using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class AssetDepDaysVM
    {
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int NumberofDays { get; set; }
        public decimal AmountPerDay { get; set; }
        public decimal Total { get; set; }
        public string StartDate { get; set; }
    }
}