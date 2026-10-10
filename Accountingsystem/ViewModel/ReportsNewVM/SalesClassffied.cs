using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesClassffied
    {
        public string startDate { get; set; }
        public string EndDate { get; set; }
        public Company company { get; set; }
        public bool InvoiceMaster { get; set; }
        public bool SalesReciptMaster { get; set; }
        public bool POSMaster { get; set; }
        public bool OrderMaster { get; set; }
        public bool Pendding { get; set; }
        public List<InvoiceMaster> InvoiceMasterList { get; set; }
        public List<SalesReceiptMaster> SalesReciptMasterList { get; set; }
        public List<MasterPOSInvoice> POSMasterList { get; set; }
        public List<Order> OrderMasterList { get; set; }
        public List<classifiyVM> classvmList{ get; set; }
        public int Branch_Id { get; set; }

        public SelectList BranchList { get; set; }
        public int Store_Id { get; set; }
        public SelectList StoreList { get; set; }

        public int emp_Id { get; set; }
        public SelectList EmpList { get; set; }
        public int CustomerId { get; set; }
        public SelectList CustomerList { get; set; }
    }
}