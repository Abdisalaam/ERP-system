using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SubledgerVM
    {
        public string From_Date { get; set; }
        public string To_Date { get; set; }

        public int SubLedger_Id { get; set; }
        public SelectList SubList { get; set; }

        public Company company { get; set; }
        public SubLedger subledger { get; set; }
        public List<SubLedgerMovList>DetailList {get;set;}
        
    }
}