using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PosPaymentMethodVM
    {
        public int MasterPOSInvoiceId { get; set; }
        public decimal Amount { get; set; }
        public SelectList PaymentList { get; set; }
        public Nullable <int> PaymentMethod_Id { get; set; }
    }
}