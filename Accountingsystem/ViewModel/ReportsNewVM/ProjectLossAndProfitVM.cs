using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectLossAndProfitVM
    {
        public string From_Date { get; set; }
        public string To_Date { get; set; }

        public int SubLedger_Id { get; set; }
        public SelectList SubList { get; set; }

        public Company company { get; set; }
        public SubLedger subledger { get; set; }
      public decimal Revenue { get; set; }

        public decimal Expences { get; set; }
        public decimal ProfitOrLoss { get; set; }
        public string Custmer_Name { get; set; }
        public string Custmer_Code { get; set; }
        public string Project_Code { get; set; }



    }
}