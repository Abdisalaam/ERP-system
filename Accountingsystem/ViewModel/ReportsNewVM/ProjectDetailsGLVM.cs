using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectDetailsGLVM
    {
        public decimal Balance_Debit { get; set; }
        public decimal Balance_Credit { get; set; }
        public decimal balance { get; set; }
        public DateTime GlDate { get; set; }
        public string GlType { get; set; }
        public string GlCode { get; set; }
        public string GlDescrib { get; set; }
      

    }
}