using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class MRGLDetail
    {
        public Accounts Account { get; set; }
        public string AccountName { get; set; }
        public DateTime date { get; set; }
        public string DocumentNumber { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public ChartOfAccounting ChartAccount { get; set; }
    }
}