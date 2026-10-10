using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class CostDistributionVM
    {

     
        public string Cost_Distr_Code { get; set; }
   
        public string Invoice_Serial { get; set; } 

      
        public SelectList MyInvoicesList { get; set; }

    
        public int Subledger_ID { get; set; }
     
      
        public SelectList MySubledgersList { get; set; }


    }
}