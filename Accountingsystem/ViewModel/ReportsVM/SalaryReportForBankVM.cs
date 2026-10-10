using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SalaryReportForBankVM
    {
        public Employee emp { get; set; }
        public DateTime SalaryDate { get; set; }
        public decimal TotalSalary { get; set; }

        public string Notes { get; set; }

        public decimal basicSalary { get; set; }

        public decimal ResidentSalary { get; set; }


        public decimal increaseSalary { get; set; }

        public decimal decraseSalary { get; set; }


    }
}