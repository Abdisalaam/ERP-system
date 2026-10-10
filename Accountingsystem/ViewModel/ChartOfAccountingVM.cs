using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class ChartOfAccountingVM
    {
        public int Account_Id { get; set; }
 
        public string Account_Name { get; set; }
   
        public string Account_Code { get; set; }
        public decimal Account_Balance { get; set; }
    
        public int Account_Level { get; set; }
        public string Level_Expression { get; set; }

        public Nullable<int> ChartOfAccountingAccount_Id { get; set; }


        public decimal Total { get; set; }
        public SelectList chartList { get; set; }
        public Nullable<int> Account_Curenncy { get; set; }
        public SelectList CurrencyList { get; set; }
    }
}