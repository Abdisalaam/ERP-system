using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;
namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SupplierBalanceDetailsVM
    {
        public string Supplier_Name { get; set; }

        public List<PurInvoiceMaster> PusMaster { get; set; }

        public List<RecalPurchInvoiceMaster> RecallMaster { get; set; }

        public List<PurInvoiceMaster> PurAsset { get; set; }

        public List<GLDetailVM> GLList { get; set; }


        public List<Payment> PayList { get; set; }

        public List<SpentRecipt> SpentPaymentList { get; set; }
        public List<CatchReceipt> CatchPaymentList { get; set; }

        public Decimal Total_Balance { get; set; }
        public Decimal Open_Balance { get; set; }
        public Company company { get; set; }
    }
}