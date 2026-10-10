using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PurInvoiceMasterAccetVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string  Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public string Invoice_Currency { get; set; }
        public string suuplier_Name { get; set; }
        public decimal Total_Amount { get; set; }
        public List<PurInvoiceMasterAccetDetailsVM> MyAsset { get; set; }

    }
}