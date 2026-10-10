using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class EmployeeSalaryMounthVM
    {
        public String Date { get; set; }
        public String EmpNAme { get; set; }
        public String EmpCode { get; set; }
        public decimal Total_Sallary { get; set; }
        public decimal Reduce_Sallary { get; set; }
        public decimal Increase_Sallary { get; set; }
        public List<String> NameOfSalaryCommponent { get; set; }
        public List<decimal> CompnetOfSalary { get; set; }




    }
}