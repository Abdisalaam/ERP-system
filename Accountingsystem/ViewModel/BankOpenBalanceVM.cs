using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class BankOpenBalanceVM
    {


      
     
        public Decimal Balance_Amount { get; set; }


    
        public string Balance_Code { get; set; }

        public int SelectedBankId { get; set; }

  
        public SelectList MyBankList { get; set; }
    }
}