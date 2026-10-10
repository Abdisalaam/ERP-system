using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesOfProductVM
    {
        public string startDate { get; set; }
        public string EndDate { get; set; }
        public Company company { get; set; }
        public bool InvoiceMaster { get; set; }
        public bool SalesReciptMaster { get; set; }
        public bool POSMaster { get; set; }
        public bool OrderMaster { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public List<SalesOfProductDetailVM> DetailList { get; set; }
        public decimal TotalOfSale { get; set; }
        public decimal TotalOfNetProfit { get; set; }

        public int Branch_Id { get; set; }

        public SelectList BranchList { get; set; }
        public int Store_Id { get; set; }
        public SelectList StoreList { get; set; }

        public int emp_Id { get; set; }
        public SelectList EmpList { get; set; }



    }
}