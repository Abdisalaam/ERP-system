using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class TransfareProjectStoreVM
    {
        public List<TransfaretoProjectStroeDetailVM> Details { get; set; }
        public Company company { get; set; }

        public int masterID { get; set; }
        public string TransCOde { get; set; }
        public string Description { get; set; }
        public string ProjectID { get; set; }
        public string AccountID { get; set; }
        public decimal totalAmount { get; set; }
        public string transDate { get; set; }
        public string storeName { get; set; }
        public TransactionState Status { get; set; }
    }
}