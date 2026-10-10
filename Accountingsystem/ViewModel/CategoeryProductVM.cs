using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class CategoeryProductVM
    {
        public int PatchID { get; set; }
        public int ProductID { get; set; }
        public int TaxID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPrecentage { get; set; }
        public decimal ProductTaxAmount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public byte[] ProductImage { get; set; }
    }
}