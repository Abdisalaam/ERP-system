using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class TaxSalesandPurchase
    {
        public string from { get; set; }
        public string to { get; set; }
        public Tax tax { get; set; }
        public Company company { get; set; }
        public int  tax_Id { get; set; }
        public List<TaxSalesandPurchaseDetails> details { get; set; }

        public string ActionName { get; set; }

        public decimal BallanceBeforeDate { get; set; }


    }
}