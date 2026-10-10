using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class Maintenance_Invoice_DetailVM
    {
        public int Main_Detail_Id { get; set; }

        public string Subscription_Name { get; set; }

        public decimal Subscription_Cost { get; set; }
        public int Maintenance_Invoice_MasterMain_Invoice_Id { get; set; }
    }
}