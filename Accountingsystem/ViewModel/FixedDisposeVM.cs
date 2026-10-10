using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class FixedDisposeVM
    {
    
        public int Fasset_IDs { get; set; }

        public string FAsset_SellDates { get; set; }
        public string WhyDispose { get; set; }
 
        public string FAsset_Descriptions { get; set; }
  
        public string FAsset_Codes { get; set; }
     
        public int SelectedAccountID { get; set; }
 
        public SelectList MyAccountList { get; set; }


        public  int Company_Id { get; set; }
 
        public string Company_name { get; set; }

      
        public int Currency_Id { get; set; }
 
        public string currency_Name { get; set; }
 
       
        public Nullable<int> Bu_Id { get; set; }
 
        public string Account_Name { get; set; }
    
        public Nullable<decimal> sellingPrice { get; set; }

        public Nullable<int> SelectedBankAccountID { get; set; }

        public SelectList MyAccountBankList { get; set; }



    }
}