using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class PorjectReciptAndPaymentVM
    {
        public string ProjectCOde { get; set; }
        public string ProjectName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerCOde { get; set; }
        public decimal ProjectRecipt { get; set; }
        public decimal ProjectPayment { get; set; }
        public decimal BalanacePayandRecipt { get; set; }
    }
}