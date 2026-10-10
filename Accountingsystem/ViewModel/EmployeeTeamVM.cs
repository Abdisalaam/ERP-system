using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmployeeTeamVM
    {
        public int Team_ID { get; set; }
    
        public string Team_Name { get; set; }

        public string Team_Leader_Name { get; set; }
      
        public int Team_Leader_ID { get; set; }
        public SelectList EmPloyeeList { get; set; }
        public List<string> EmployeesMember { get; set; }
    }
}