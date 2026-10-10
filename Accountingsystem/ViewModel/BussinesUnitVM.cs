using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class BussinesUnitVM
    {
        public int BU_Id { get; set; }
 
        public string BU_Code { get; set; }

        public string BU_Name { get; set; }

 
        public string BU_Description { get; set; }
   
        public string BU_Type { get; set; }

        public int CompanyComp_Id { get; set; }
        public SelectList compList { get; set; }
    }
}