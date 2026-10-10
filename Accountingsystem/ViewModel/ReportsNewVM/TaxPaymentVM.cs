using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TaxPaymentVM
    {
       
        public int BankAccount_Id { get; set; }
        public string GLDate { get; set; }
        public string Date { get; set; }
        public SelectList BankList { get; set; }
    }
}