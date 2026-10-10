using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class OfferPriceReportVM
    {
        public int Counter { get; set; }
        public Company company { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public DateTime OfferPriceDate { get; set; }
        public string OfferPriceDeadLine { get; set; }
        public string OfferPriceCode { get; set; }
        public decimal OfferPriceTotal { get; set; }

   
    }
}