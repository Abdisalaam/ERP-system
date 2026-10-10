using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class BankVM
    {

        public int Bank_ID { get; set; }
        public int? Emp_Id{ get; set; }
        public SelectList EmpList { get; set; }


        public string Bank_Name { get; set; }
  
       
        public string Bank_Address { get; set; }

        public string Bank_Code { get; set; }

        public int SelectedChartOfAccountId { get; set; }

  
        public SelectList MyChartOfAccountList { get; set; }
      
        public string BankNumber { get; set; }
  
        public string IbanNumber { get; set; }

        public decimal balance { get; set; }
   
        public Nullable<int> CurrencyID { get; set; }
     
        public SelectList CurrencyList { get; set; }
 
        public string Currency { get; set; }

    }
}