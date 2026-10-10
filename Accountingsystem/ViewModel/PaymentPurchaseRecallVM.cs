using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class PaymentPurchaseRecallVM
    {
        public List<string> Recall_Code { get; set; }
        public List<decimal> RecallQuantity { get; set; }
        public List<decimal> RecallTotalCost { get; set; }
    }
}