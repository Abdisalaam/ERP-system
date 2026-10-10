using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MasterPOSInvoiceVM
    {
        public int MasterPOSInvoiceId { get; set; }
        public List<string> Taxes { get; set; }
        public SelectList TaxList { get; set; }
        public string Serial_Number { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public String Invoice_Date { get; set; }
  
        public int MaintenanceOrder_Id { get; set; }
       
        public decimal Master_Discount { get; set; }
        public Nullable<TransactionState> Invoice_State { get; set; }
        public Nullable <int> Customer_Id { get; set; }

        public int PointOfSalePOS_Id { get; set; }
        public SelectList PointSaleList { get; set; }
        public SelectList CustomerList { get; set; }
        public SelectList EmpList { get; set; }
        public SelectList RevenueList { get; set; }
     
        public int RevenueAccount { get; set; }
        public int Emp_Id { get; set; }
  
        public decimal AllowDiscount { get; set; }
        public int sewingId { get; set; }

    }
}