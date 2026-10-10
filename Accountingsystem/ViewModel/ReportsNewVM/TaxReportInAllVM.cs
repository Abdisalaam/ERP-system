using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TaxReportInAllVM
    {
        public string taxName { get; set; }
        public string CHildAccountName { get; set; }
        public string CHildAccountCode { get; set; }
        public string TransactionCode { get; set; }
        public string dateofTransaction { get; set; }
        public decimal S_Amount { get; set; }
        public decimal R_Amount{ get; set; }
        public decimal Total { get; set; }

        public DateTime Date { get; set; }

        public string DocumentType { get; set; }

        public Tax tax { get; set; }

        public Company company { get; set; }
      

    }
}