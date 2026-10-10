using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class EmployeeList
    {
      public  List<Employee> EmpList { get; set; }
        public Company company { get; set; }
    }
}