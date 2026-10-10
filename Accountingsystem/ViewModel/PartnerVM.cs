using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PartnerVM
    {
        public int Part_Id { get; set; }

      
        public string Part_Name { get; set; }

   
      
        [DataType(DataType.PhoneNumber)]
        public string Part_Phone { get; set; }

      
     
        [DataType(DataType.EmailAddress)]
        public string Part_Email { get; set; }

  
      
        //[DataType(DataType.Date)]
        public string Part_Birthday { get; set; }


    
        //[DataType(DataType.Date)]
        public string Part_AddedDate { get; set; }

     
      
        //[DataType(DataType.Date)]
        public string Part_RemovedDate { get; set; }

     

        public string Part_Nationality { get; set; }

        public int CompanyComp_Id { get; set; }
  
        public string Company_Name { get; set; }

       
        public string Partner_Code { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }


        public int SelectedChartOfAccountId { get; set; }
        public SelectList MyChartOfAccountList { get; set; }



    }
}