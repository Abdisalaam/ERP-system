using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmployeeRoleEditVM
    {
       public  int roleID { get; set; }
        public string roleIDMain { get; set; }
        public SelectList selectrole { get; set; }
        public string EMployeeID { get; set; }
        public string EmployName { get; set; }

    }
}