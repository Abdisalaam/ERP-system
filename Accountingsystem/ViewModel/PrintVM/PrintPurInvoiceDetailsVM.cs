using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintPurInvoiceDetailsVM
    {
        public int Details_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Description { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Details_total_Cost { get; set; }
        public string ProductName { get; set; }
        public string productCode { get; set; }

        public decimal amountBeforeTax { get; set; }
        public decimal discount { get; set; }
        public String MeasureUnit { get; set; }
        public decimal TaxRate { get; set; }
        public Tax tax { get; set; }

    }
}