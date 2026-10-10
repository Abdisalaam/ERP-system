using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class CustomerBalanceDetailsVM
    {
        public string Customer_Name { get; set; }
        public string Date { get; set; }
        public List<InvoiceMaster> InvoiceList { get; set; }
        public List<ReverseSalesInvoiceMaster> ReversInvoiceList { get; set; }
        public List<SalesReceiptMaster> SalesReceiptList { get; set; }
        public List<ReverseSalesReceiptMaster> ReversSalesReceiptList { get; set; }
        public List<Receipt> ReceiptsList { get; set; }
        public List<SpentRecipt> SpentReceiptList { get; set; }
        public List<CatchReceipt> CatchReceiptList { get; set; }
        public List<GLDetailVM> Customer_Account { get; set; }
        public List<Subscription> Subscriptionslist { get; set; }
        public Decimal Total_Balance { get; set; }
        public Decimal Open_Balance { get; set; }
    }
}