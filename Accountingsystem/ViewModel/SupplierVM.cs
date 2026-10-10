using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class SupplierVM
    {

        public int Supp_Id { get; set; }


       
        public string Supp_Name { get; set; }

   
        public string Supp_Address { get; set; }


 
        public string Supp_Phone { get; set; }


   
        public string Supp_Mail { get; set; }

        public Nullable<int> SelectedCountryId { get; set; }
        public SelectList MyCountryList { get; set; }

        public Nullable<int> SelectedProvinceId { get; set; }
        public SelectList MyProvinceList { get; set; }

  
        public Nullable<int> SelectedCityId { get; set; }
        public SelectList MyCitiesList { get; set; }

        [DataType(DataType.PostalCode)]
     

        public string Supp_Fax { get; set; }


       
        
        public string Supp_Code { get; set; }

        public int SelectedChartOfAccountId { get; set; }
    
   
        public string Supp_Mobile { get; set; }

   
        public decimal Purchase_Price { get; set; }

    
        public int Payment_Policy_Id { get; set; }
        public SelectList MyPaymentPolicy { get; set; }
      
        public string Supp_Website { get; set; }

        public int Branch_Id { get; set; }
        public String Tax_Number { get; set; }
        public SelectList BranchList { get; set; }
        public SelectList MyChartOfAccountList { get; set; }
      
        public HttpFileCollectionBase SuppFiles { get; set; }
        public IEnumerable<HttpPostedFileBase> SuppFiles2 { get; set; }
        public decimal Supplier_Balance { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }
   
        public string City { get; set; }
    }
}