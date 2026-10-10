using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class GlMasterAndDetailListVM
    {
       
        public string DateNowforReport { get; set; }
        public string CompanyName { get; set; }
        public string GlCode { get; set; }
        public string GLType { get; set; }
        public string GlDate { get; set; }
        public string GLDescr { get; set; }
        public decimal DebitTotal { get; set; }
        public decimal CreditTotal { get; set; }
        public List<GLDetailIistReportVM> GLDetailList { get; set; }
    }
}