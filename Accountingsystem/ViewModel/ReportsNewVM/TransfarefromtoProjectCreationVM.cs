using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class TransfarefromtoProjectCreationVM
    {
        public int ProjectID { get; set; }
        public SelectList ProjectList { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        
    }
}