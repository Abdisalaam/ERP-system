using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class UnpaidInvoiceVM
    {
        public string serialNumber { get; set; }
        public string GLCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal invoiceValue { get; set; }
        public decimal invoicePaid { get; set; }
        public decimal Remaining { get; set; }

    }
}