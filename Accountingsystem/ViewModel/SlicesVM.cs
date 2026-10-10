using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SlicesVM
    {
        public int Slice_Id { get; set; }
   

        public string Slice_Name { get; set; }
       
     
        public string Slice_Code { get; set; }
    }
}