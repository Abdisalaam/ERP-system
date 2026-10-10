using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectDateVM
    {
        public string from { get; set; }
        public string to { get; set; }
        public int employeeId { get; set; }
        public Employee employee { get; set; }
        public List<ProjectBalancesVM> ListProjectBalance { get; set; }
        public List<ProjectInvoiceReportVM> List { get; set; }
        public Company company { get; set; }
        public SelectList employeeList { get; set; }
    }
}