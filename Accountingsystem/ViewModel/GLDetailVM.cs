using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class GLDetailVM
    {
        public int GLDetail_Id { get; set; }



        


        public decimal GLDetail_Debit { get; set; }
      

        public decimal GLDetail_Credit { get; set; }
      
        public int Account_Id { get; set; }
      
        public int Bu_Id { get; set; }
        public decimal difference { get; set; }
     
        public int SubLedger_Id { get; set; }
   
        public int Asset_Id { get; set; }
    
        public string Description_Detail { get; set; }
        public int GLMasterGLMaster_Id { get; set; }
        public int Account_Child_ID { get; set; }
        public string Account_Name { get; set; }
        public string Account_Code { get; set; }
        public string Subledger_Code { get; set; }
        public string Subledger_Name { get; set; }
      
        public string Account_Child_Name { get; set; }

    }
}