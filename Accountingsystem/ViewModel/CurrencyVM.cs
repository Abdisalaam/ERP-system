using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class CurrencyVM
    {

        public int Currency_Id { get; set; }

        
        public string Currency_Type { get; set; }

    
        public string Currency_Country { get; set; }

        public string Currency_ISO { get; set; }

   
        public string Currency_Name { get; set; }
 
        public string Currency_Iso2 { get; set; }


        public string Break_Coin { get; set; }
   
        public string Break_Coin2 { get; set; }
 
        public int Comp_Currency_Id { get; set; }
        public SelectList MyCurrencyList { get; set; }
        public Decimal Current_Price { get; set; }
    }
}