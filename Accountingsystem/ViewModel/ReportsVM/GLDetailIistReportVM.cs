using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class GLDetailIistReportVM
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string childName { get; set; }
        public string Debit { get; set; }
        public string credit { get; set; }
        public string CurrencyName { get; set; }
        public string subledgerCode { get; set; }
        public string subledgerName { get; set; }
        public int detailcounter { get; set; }

        public string decribtion { get; set; }
    }
}