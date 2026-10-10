using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MeasureVM
    {

        public int MUnit_ID { get; set; }

      
     
        public string MUnit_Code { get; set; }


        public string MUnit_Descrip { get; set; }
    }
}