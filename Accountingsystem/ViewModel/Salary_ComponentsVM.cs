using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class Salary_ComponentsVM
    {
        public int Salary_Components_Id { get; set; }
        public int Account_Id { get; set; }
        public string Account_Name { get; set; }
        public Decimal Account_Amount { get; set; }
        public int Employee_Id { get; set; }
        public SelectList AccountList { get; set; }
    }
}