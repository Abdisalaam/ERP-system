using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class Dailypayment
    {
        public int payment_Id { get; set; }
        public string paymentName { get; set; }

        public decimal paymentValue { get; set; }
 

    }
}