using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmployeeSalaryVM


    {
        public int id { set; get; }

        public int BankAccount { set; get;  }
        public SelectList AccountList { set; get; }

        public String Employee_Name { get; set; }
  
        public String Employee_Code { get; set; }
        public String Description { get; set; }
        public String Date { get; set; }

        public decimal Trim_Pay { get; set; }
       
    }
}