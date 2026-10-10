using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Accountingsystem.ViewModel
{
    public class BranchVM
    {
        public int Branch_Id { get; set; }
        public IEnumerable<HttpPostedFileBase> Photo { get; set; }

        public string Branch_Name { get; set; }


        public string Company_Name { get; set; }
        public string Branch_Country { get; set; }
        public string Branch_Province { get; set; }
        public string Branch_City { get; set; }

        public int Company_ID { get; set; }

        public Nullable<int> SelectedCountryId { get; set; }

     
        public SelectList MyCountryList { get; set; }

        public Nullable<int> SelectedProvinceId { get; set; }

       
     
        public SelectList MyProvinceList { get; set; }

        public Nullable<int> SelectedCityId { get; set; }

   
     
        public SelectList MyCitiesList { get; set; }

   
      
        public string Branch_Address { get; set; }


 
     
      
        public string Branch_Phone { get; set; }


  
  
        public string Branch_Email { get; set; }


  
      
        public string Branch_Fax { get; set; }


       
        public string Branch_Code { get; set; }
        public IEnumerable<HttpPostedFileBase> ImageFooter { get; set; }
        public IEnumerable<HttpPostedFileBase> ImageHeader { get; set; }
        public IEnumerable<HttpPostedFileBase> ImageWaterMark { get; set; }

    }
}