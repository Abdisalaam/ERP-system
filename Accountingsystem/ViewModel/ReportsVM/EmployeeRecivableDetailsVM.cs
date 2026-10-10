using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class EmployeeRecivableDetailsVM
    {
        public string GLDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}