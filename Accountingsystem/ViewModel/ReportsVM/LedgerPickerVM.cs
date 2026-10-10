using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class LedgerPickerVM
    {
        public Nullable<int> Master_ID { get; set; }
        public SelectList MasterAccountList { get; set; }
        [Required(ErrorMessage = "ادخل الحساب ")]
        public int SubAccount_ID { get; set; }
        public SelectList SubAccountList { get; set; }
     
        public String FromDate { get; set; }
        public String Todate { get; set; }
       
    }
}