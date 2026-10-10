using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class ProjectSubledgerVM
    {
        public int SubledgerID { get; set; }
        public string SubledgerName { get; set; }

        public int number { get; set; }

        public bool abc { get; set; }
        public bool incremental { get; set; }
        public int projecttempId { get; set; }
    }
}