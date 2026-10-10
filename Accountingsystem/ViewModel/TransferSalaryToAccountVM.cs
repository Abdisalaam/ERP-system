using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class TransferSalaryToAccountVM
    {
      

        public int Account { get; set; }
    
        public Decimal Total_Sallary { get; set; }
        public SelectList AccountList { get; set; }
    }
}