using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class TaxSalesandPurchaseDetails
    {

        public string Code { get; set; }
        public DateTime GLDate { get; set; }
        public decimal  salesPurchValue { get; set; }
        public decimal Percentage { get; set; }

        public decimal ValueOfTax { get; set; }

        public string CustomerName { get; set; }
        public string CustomerTaxNum { get; set; }

        public string DocumentType { get; set; }
        public decimal QuantityOfSales { get; set; }
        public int GLMaster_Id { get; set; }
    }
}