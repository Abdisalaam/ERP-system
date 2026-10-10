using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class TaxesVM
    {
        public int Tax_ID { get; set; }
      
        
        public string Tax_Name { get; set; }
    
      
        public string Tax_Type { get; set; }
    
        
       // [DataType(DataType.Date)]
        public string Tax_AddedDate { get; set; }
      
     
        
        public Decimal Tax_Percentage { get; set; }
    
        public string Tax_Code { get; set; }
  
      
        public int SelectedChartOfAccountId { get; set; }
     
      
        public SelectList MyChartOfAccountList { get; set; }
     
        public bool IsProductTax { get; set; }
   
       
    }
}