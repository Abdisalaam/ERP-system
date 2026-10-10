using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintInvoiceMasterDetailsVM
    {

        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Model { get; set; }
        public string Product_Description { get; set; }

        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal amountBeforeTax { get; set; }
        public decimal Total_Tax { get; set; }
        public decimal Total_Amount_Detail { get; set; }
        public decimal discount { get; set; }
        public string productcode { get; set; }
        public String MeasureUnit { get; set; }
        public decimal TaxRate { get; set; }
        public Tax tax { get; set; }
        public decimal Detail_Discount { get; set; }
        public decimal Detail_Discount_Amount { get; set; }



    }
}