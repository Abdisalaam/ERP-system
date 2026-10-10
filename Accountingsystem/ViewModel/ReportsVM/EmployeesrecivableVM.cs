using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class EmployeesrecivableVM
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public SelectList  EmployeesList{ get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public Company compnay { get; set; }
        public List<EmployeeRecivableDetailsVM> RecivableDetails { get; set; }
    }
}