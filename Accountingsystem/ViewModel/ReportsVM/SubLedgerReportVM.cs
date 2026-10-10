using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubLedgerReportVM
    {
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public Decimal TotalDepit { get; set; }
        public Decimal TotalCredit { get; set; }
        public List<GLDetailVM> Account_Sudledger { get; set; }
        public Decimal Difference { get; set; }
        public int SubLedger_Id { get; set; }
        public SelectList SubList { get; set; }
        public String Su_Code { get; set; }
        public String SubName { get; set; }
        public Company company { get; set; }

    }
}