using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class RegularDetailsVM
    {
        public int Details_Id { get; set; }
      
        public int Subledger_Id { get; set; }
        public SelectList sublist { get; set;}
   
        public decimal Total_Amount { get; set; }
        public int InvoiceMaster_Id { get; set; }
     
        public decimal Tax { get; set; }
   
        public int ChartOfAccount_Id { get; set; }
        public SelectList ChartList { get; set; }
    }
}