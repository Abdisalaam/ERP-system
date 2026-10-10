using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SupppVMLEdger
    {
        public int Supp_Id { get; set; }
        public string Supp_Name { get; set; }
        public string Supp_Country { get; set; }
        public string Supp_Code { get; set; }

        public string SubLed_Date { get; set; }
        public decimal SubLedger_Balance { get; set; }
    }
}