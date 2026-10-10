using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class OrderDetailsVM
    {
        public int Id { get; set; }
        public int PatchId { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public byte[] CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyTaxNymber { get; set; }

        public string CustomerTaxNumber { get; set; }

        public string CustomerPhone { get; set; }
        public decimal ProductTaxAmount { get; set; }
        public string ProductTaxName { get; set; }
        public string ExtraTaxName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm  dd-MM-yyyy }")]
        public DateTime ReversdeOrderDate { get; set; }
        public decimal ExtraTaxAmount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal SubTotalProduct { get; set; }
        public decimal SubTotalOrder { get; set; }
        public decimal SubTotalProductBeforeTax { get; set; }
        public decimal Total { get; set; }
        public decimal TotalProductTax { get; set; }
        public decimal AmountAfterDiscount { get; set; }
        public int OrderId { get; set; }
        public int? QRNumber { get; set; }
        public string AdditionalDetail { get; set; }


    }
}