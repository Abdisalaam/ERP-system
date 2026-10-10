using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class DashboadDiagramVM
    {
        public decimal revenu { get; set; }
        public decimal Expences { get; set; }
        public decimal Purchase { get; set; }
        public decimal CostOfSales { get; set; }

        public decimal ProfitAndLoss { get; set; }

        public decimal Supplier { get; set; }
        public decimal Customer { get; set; }
        public decimal Treasury { get; set; }
        public decimal Banks { get; set; }
        public int date { get; set; }
    }
}