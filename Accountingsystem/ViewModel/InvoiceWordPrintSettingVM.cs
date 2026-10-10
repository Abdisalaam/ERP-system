using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class InvoiceWordPrintSettingVM
    {
        public int Id { get; set; }
        public string TotalExcludingVat { get; set; }
        public string Discount { get; set; }
        public string TotalTaxableAmount { get; set; }
        public string ToatlAmountDue { get; set; }
        public string ToatalAmountPaid { get; set; }
        public string ToatalRamainingAmount { get; set; }
        public string TotalExcludingVat1 { get; set; }
        public string Discount1 { get; set; }
        public string TotalTaxableAmount1 { get; set; }
        public string ToatlAmountDue1 { get; set; }
        public string ToatalAmountPaid1 { get; set; }
        public string ToatalRamainingAmount1 { get; set; }
        public string TotalVat { get; set; }
        public string TotalVat1 { get; set; }
        public bool Time { get; set; }
        public Nullable<bool> DiscountInDetail { get; set; }
        public bool BaptismNumber { get; set; }
    }
}