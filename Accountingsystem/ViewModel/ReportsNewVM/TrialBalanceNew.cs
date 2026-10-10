using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TrialBalanceNew
    {
        public string from { get; set; }
        public string to { get; set; }
         public bool OFFGL { get; set; }
        public bool FatherAccount { get; set; }
        public bool ChildAccount { get; set; }
        public decimal AmountBefor { set; get; }
        public decimal CeditMov { set; get; }
        public decimal DepitMov { set; get; }
        public decimal AmountOfPeriod { set; get; }
        public decimal FinalAmount { set; get; }

        public List<TrialBalanceDetail> Detail { get; set; }
        public TrialBalanceNew()
        {

            Detail = new List<TrialBalanceDetail>();

        }

    }
}