using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class CustomerStatisticVM
    {
        public Customer customer { get; set; }
        public Supplier supplier { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal Datebalance { get; set; }
        public decimal Date30Befor { get; set; }
        public decimal Date30After { get; set; }
        public decimal Date60After { get; set; }
        public decimal Date90After { get; set; }
        public decimal Date120After { get; set; }


    }
}