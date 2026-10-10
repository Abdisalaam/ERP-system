using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class OffGlMasterVM
    {
      
        public int Owner { get; set; }
    
        public SelectList OwnerList { get; set; }
        public String GlCode { get; set; }
     
        public String Startdate{get; set;}
      
        public String Enddate { get; set; }
    }
}