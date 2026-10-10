using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class WarehousesReportVM
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ChartOfAccounting> Stores { get; set; }
        public Company company { get; set; }
    }
}