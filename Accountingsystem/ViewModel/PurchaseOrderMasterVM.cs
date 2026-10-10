using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PurchaseOrderMasterVM
    {
        public int isCopy { get; set; }
        public int Master_Id { get; set; }
        public string orderDate { get; set; }

        public string Serial_Number { get; set; }

     
        public string Description { get; set; }
     
        public decimal Master_Discount { get; set; }
     
        public string Invoice_Currency { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }
        public Document_Type Document_Type { get; set; }
      
        public int Supplier_Id { get; set; }

        public int Store_Id { get; set; }
      
        public int Company_Id { get; set; }


        public Nullable<decimal> ExchangeRate { get; set; }

        public Nullable<int> Subledger_Id { get; set; }

        public Nullable<int> BU_Id { get; set; }
    
        public Nullable<int> Account_Id { get; set; }
       
        public Nullable<int> Branch_Id { get; set; }
 
        public Nullable<int> Emp_Id { get; set; }
    
        public Nullable<int> Payment_Policy { get; set; }

        public string Reference { get; set; }
        public SelectList MyStoresList { get; set; }
        public SelectList AccountsList { get; set; }
        public SelectList BU_List { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList MyEmpList { get; set; }
        public SelectList MyBranchList { get; set; }
        public SelectList MyCompanyList { get; set; }
        public SelectList MyBussinesUnitList { get; set; }
        public SelectList MyCurrencyList { get; set; }
        public SelectList MySupplierList { get; set; }
        public List<PurchaseOrderDetailVM> myDetailsVM { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}