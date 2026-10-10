using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class POSDetailPaymentVM
    {
        public int PaymentMethod_Id { get; set; }
       public SelectList PaymentList { get; set; }
        public int POS_ID { get; set; }
    }
}