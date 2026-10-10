using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class CustomerInvoiceSubscVM
    {

        public int Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_County { get; set; }
        public string Customer_Code { get; set; }
        public string Serial { get; set; }
        public decimal pay_Amount { get; set; }
        public decimal Invoice_Amount { get; set; }
    }
}