using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class SalaryListaVM
    {

        public  List<EmployeeSallaryVM> ListSallary { get; set; }

        public List<EmployeeSallaryVM> LoanOfEmployee { get; set; }

        public Company company { get; set; }
    }
}