using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProjectSubSubledgerVM
    {
      
        public int subsubledgerID { get; set; }
        public string subsubledgerName { get; set; }
        public int projectSubledgerId { get; set; }
        public int projectTempId { get; set; }
        public SelectList projectSubledgerList { get; set; }
    }
}