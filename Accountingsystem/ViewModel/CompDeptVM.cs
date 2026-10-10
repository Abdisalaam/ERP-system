using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class CompDeptVM
    {
        public int Id { get; set; }

   
        public string Company_Name { get; set; }


        public string CompDept_Code { get; set; }

   
        public string CompDept_Name { get; set; }

  
       

        public string CompDept_Phone { get; set; }

  
      
        public string CompDept_Description { get; set; }
    }
}