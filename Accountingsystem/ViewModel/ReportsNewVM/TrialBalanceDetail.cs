using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TrialBalanceDetail
    {
        public ChartOfAccounting Account { set; get; }
        public decimal AmountBefor { set; get; }
        public decimal CeditMov { set; get; }
        public decimal DepitMov { set; get; }
        public decimal AmountOfPeriod { set; get; }
        public decimal FinalAmount { set; get; }
        public Accounts ChildAccountd { get; set; }
        public FixedAsset Asset { get; set; }
        public List<TrialBalanceDetail> ChildAccount { get; set; }

        public string LevelExpresion { get; set; }
        public bool IshasAmount { get; set; }
        public TrialBalanceDetail(){

            ChildAccount = new List<TrialBalanceDetail>();

    }

    }
}