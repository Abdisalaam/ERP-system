using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class StoreQuantityVM
    {
        public List<ProductBatches> ProductBatch { get; set; }

        public decimal Total { get; set; }
        public string Store_Name { get; set; }
        public Company Company { get; set; }



    }
}