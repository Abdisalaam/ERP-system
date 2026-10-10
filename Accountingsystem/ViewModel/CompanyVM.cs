using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Accountingsystem.ViewModel
{
    public class CompanyVM
    {
        
        public int Comp_Id { get; set; }
        public IEnumerable<HttpPostedFileBase> ImageFooter { get; set; }
        public IEnumerable<HttpPostedFileBase> ImageHeader { get; set; }
     
        public IEnumerable<HttpPostedFileBase> ImageWaterMark { get; set; }

        public string Comp_Name { get; set; }
        public string CRN { get; set; }


        public string Comp_Code { get; set; }
        public Nullable<int> SelectedCountryId { get; set; }


       
        public SelectList MyCountryList { get; set; }
        public Nullable<int> SelectedProvinceId { get; set; }

     
      
        public SelectList MyProvinceList { get; set; }
        public Nullable<int> SelectedCityId { get; set; }

      
   
        public SelectList MyCitiesList { get; set; }

  

        public string Comp_Address { get; set; }

   
       

        public string Comp_phone { get; set; }

        
     
      
        public string Comp_Email { get; set; }    
     
        public string Comp_PortalCode { get; set; }

     
     
        public string Comp_Fax { get; set; }
        
       
        public string Comp_Field { get; set; }


        public Nullable<int> SelectedDepTypeId { get; set; }

     
   
        public SelectList MyDepTypeList { get; set; }

       
     
     
        public Nullable<int> Comp_Currency_Id { get; set; }
        public SelectList MyCurrencyList { get; set; }

        
      
        public string Tax_Number { get; set; }

 
        public string StartFinancialDate { get; set; }
    
        public string EndFinancialDate { get; set; }
        public Nullable<int> Number { get; set; }
        public CurrencyVM currencyPartial { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public IEnumerable<HttpPostedFileBase> Photo { get; set; }
    
        public string Country { get; set; }
 
        public string Province { get; set; }
     
        public string City { get; set; }

    }
}