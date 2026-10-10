using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintRecalPurchInvoiceDetailsVM
    {
        public string Product_Model { get; set; }
        public string Product_Describtion { get; set; }
        public string Product_Code { get; set; }
        public int Details_Id { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal amountBeforeTax { get; set; }
        public decimal discount { get; set; }
        public String MeasureUnit { get; set; }
        public decimal TaxRate { get; set; }
        public Tax tax { get; set; }
        public decimal Total_Tax { get; set; }
        public String product_Name { get; set; }
    }
}