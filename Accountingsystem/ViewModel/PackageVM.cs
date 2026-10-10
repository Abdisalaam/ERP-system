using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class PackageVM
    {
        public int Id { get; set; }
    
        public int Number_Branch { get; set; }

   

        public int Number_POS { get; set; }
     
        public int Number_User { get; set; }

        public bool ProductCompound { get; set; }
        public bool Subscribtion { get; set; }

        public bool Asset { get; set; }
   
        public string Package_Name { get; set; }
      
        public int Package_duration { get; set; }

        public bool Project { get; set; }
  
       
        public bool OffGL { get; set; }
     

        public bool Expences { get; set; }
      
        public int Number_store { get; set; }
     
        public int Number_subLedgger { get; set; }
 
        public bool HR { get; set; }

      
        public int Number_Employee { get; set; }
        
         
        public bool ExtraTaxes { get; set; }
        public bool Accounting { get; set; }
        public bool Suppliers { get; set; }
        public bool ReportsAndPending { get; set; }
        public bool ChartOfAccount { get; set; }
        public bool Maintenance { get; set; }
        public bool Purchase { get; set; }
        public bool Casher { get; set; }
        public bool Sales { get; set; }
        public bool Commission { get; set; }
        public bool Attendingandleaving { get; set; }
        public int NumberOfInvoice { get; set; }
        public string StartOfInvoice { get; set; }
        public string StartOfInReverse { get; set; }
        public int NumberOfReverse { get; set; }
        public bool PrintSpecial { get; set; }
        public bool Sewing { get; set; }
        public string SewingArab { get; set; }
        public string SewingEng { get; set; }
    }
}