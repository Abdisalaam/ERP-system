using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class PrintChartOfAccountVM
    {
        public List<ChartOfAccounting> chart { set; get; }
        public Company company { set; get; }
    }
}