using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class ReceiptInvoiceSalesRecallVM
    {
        public List<string> Recall_Code { get; set; }
        public List<decimal> RecallQuantity { get; set; }
        public List<decimal> RecallTotalCost { get; set; }
    }
}