using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class classifiyVM
    {
        public string customerCode { get; set; }
        public string customerName { get; set; }
        public string CustomerTaxNumber{ get; set; }

        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Quantity { get; set; }
        public string BranchName { get; set; }
        public string paymentMethodName { get; set; }
        public decimal totalAmount { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalCost { get; set; }
        public decimal totalReverse { get; set; }
        public decimal totalReverseWithoutTax { get; set; }
        public decimal totalReverseTax { get; set; }
        public decimal totalReverseCost { get; set; }
        public decimal totalReverseQTY { get; set; }
        public decimal totalReverseCheck { get; set; }
        public TransactionState Status { get; set; }
        //public List<ReversePOSInvoice> ReversePOSInvoice = new List<ReversePOSInvoice> () ;
        //public List<ReverseSalesInvoiceMaster> ReverseSalesInvoiceMaster = new List<ReverseSalesInvoiceMaster>();
        //public List<ReverseSalesReceiptMaster> ReverseSalesReceiptMaster = new List<ReverseSalesReceiptMaster>();

        public IEnumerable<StoreCard> storeCard { get; set; }
    }
}