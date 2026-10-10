using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class OrderTaxVM
    {
        public int TaxID { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }

    }
}