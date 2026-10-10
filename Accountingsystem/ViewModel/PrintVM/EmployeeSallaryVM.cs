using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class EmployeeSallaryVM
    {
        public String CompnyName { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
        public String Employee_Name { get; set; }
        public String Employee_Code { get; set; }
        public decimal SallaryAmount { get; set; }
        public String Account { get; set; }
        public decimal LoanOfEmp { get; set; }
        public string documentSerial { get; set; }


    }
}