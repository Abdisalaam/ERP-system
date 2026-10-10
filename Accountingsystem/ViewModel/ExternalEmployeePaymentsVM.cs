using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ExternalEmployeePaymentsVM
    {
        public int Id { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int SelectedExternalSalesEmployeeID { get; set; }
        public SelectList ExternalSalesEmployeeList { get; set; }
        public string Comment { get; set; }
        public int ExternalSalesEmployeeID { get; set; }
        public ExtrnalSalesEmployee ExtrnalSalesEmployee { get; set; }
    }
}