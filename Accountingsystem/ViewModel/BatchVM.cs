using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class BatchVM
    {
        public int ProductId { get; set; }
        public Store store { get; set; }
        public int batchId { get; set; }

        public decimal salesPrice { get; set; }
        public bool isActive { get; set; }

    }
}