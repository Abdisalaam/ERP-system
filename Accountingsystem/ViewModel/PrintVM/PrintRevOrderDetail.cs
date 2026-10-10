using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintRevOrderDetail
    {
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public string Product_Code { get; set; }
        public string Product_Model { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }

        public decimal Total_Tax { get; set; }
        public decimal Total_Amount_Detail { get; set; }


        public decimal amountBeforeTax { get; set; }
        public decimal discount { get; set; }
        public String MeasureUnit { get; set; }
        public decimal TaxRate { get; set; }
        public Tax tax { get; set; }

    }
}