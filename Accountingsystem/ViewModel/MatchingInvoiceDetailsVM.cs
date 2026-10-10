using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class MatchingInvoiceDetailsVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string Description { get; set; }
        public System.DateTime Invoice_Date { get; set; }
        public System.DateTime Due_Date { get; set; }
        public System.DateTime GL_Date { get; set; }
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        public int Customer_Id { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }     
        public int Company_Id { get; set; }
        public int Store_ID { get; set; }
        public int Account_ID { get; set; }
        public decimal TotalAmountAfMat { get; set; }
        public List<ReverseSalesInvoiceMaster> ReverseSales { get; set; }


    }
}