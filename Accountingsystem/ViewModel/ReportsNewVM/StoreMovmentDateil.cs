using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class StoreMovmentDateil
    {
        public Product product { get; set; }
        public decimal income { get; set; }
        public decimal outgoing { get; set; }
        public decimal balance { get; set; }

    }
}