using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class BarCodePrint
    {
        public int Product_Id { get; set; }
        public string barcode { get; set; }
        public Product product { get; set; }        
        public bool name  { get; set; }
        public bool PriceWithTax { get; set; }
        public bool Tax { get; set; }
        public bool DateOfExpire { get; set; }
        public bool Price { get; set; }
        public bool Notes { get; set; }
    }
}