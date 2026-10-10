using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class CostDistributionDetailPrintVM
    {
        public Product product { get; set; } 
        public decimal quantity { get; set; }
        public decimal Costbefor { get; set; }
        public decimal CostOneBefor { get; set; }
        public decimal CostOneAfter { get; set; }
        public decimal IncreasingCost { get; set; }
        public decimal CostAfter { get; set; }
        public decimal Percenatge { get; set; }
    }
}