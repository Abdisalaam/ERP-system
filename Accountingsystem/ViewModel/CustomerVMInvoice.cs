using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class CustomerVMInvoice
    {
        public int customer_Id { get; set; }
        public string Date { get; set; }
        public Decimal Amount { get; set; }
        public string Customer_Name { get; set; }
    }
}