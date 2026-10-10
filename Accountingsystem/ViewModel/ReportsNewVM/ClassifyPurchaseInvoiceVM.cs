using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ClassifyPurchaseInvoiceVM
    {
        public string from { get; set; }
        public string to { get; set; }
        public Company company { get; set; }
        public List<PurchaseReportVM> List { get; set; }
        public SelectList branchList { get; set; }
        public Nullable<int> branchid { get; set;}
        public SelectList storeList { get; set; }
        public Nullable<int> storeID { get; set; }
        public SelectList ProductList { get; set; }
        public Nullable<int> ProductID { get; set; }
        public SelectList ProductClassList { get; set; }
        public Nullable<int> productClassify { get; set; }
    }
}