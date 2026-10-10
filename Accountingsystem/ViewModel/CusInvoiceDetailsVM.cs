using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class CusInvoiceDetailsVM
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public System.DateTime Invoice_Date { get; set; }

        public decimal Total_Amount { get; set; }

        public decimal TotalAmountAfMat { get; set; }
    }
}