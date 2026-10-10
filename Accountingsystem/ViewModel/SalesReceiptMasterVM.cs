using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class SalesReceiptMasterVM
    {
        public int Master_Id { get; set; }
     
        public string Serial_Number { get; set; }
        public string Notes { get; set; }
        public List<string> Taxes { get; set; }
        public SelectList TaxList { get; set; }
        public int  isCopy { get; set; }
        public  int? order_Id { get; set; }


        public string Description { get; set; }
     
        public string Invoice_Date { get; set; }

        [Required]
        [Range(0.00, 100)]
        public decimal Master_Discount { get; set; }
        [Required]
        public int Invoice_Currency { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }
        public TransactionState Invoice_State { get; set; }
        public Nullable<decimal> Exchange_Rate { get; set; }
       
        public int Customer_Id { get; set; }
        [Required]
        public int Store_ID { get; set; }
        [Required]
        public int Company_Id { get; set; }
        [Required]
        public int PaymentMethod_Id { get; set; }
        [Required]
        public int Account_Id { get; set; }
    
        public Nullable<int> Branch_Id { get; set; }
      
        public string Reference { get; set; }
       
       
        public string ApprovalCode { get; set; }

        public decimal Fixed_Discount { get; set; }

        public SelectList MyCompanyList { get; set; }
        public SelectList MyCustomerList { get; set; }
        public SelectList MyStoresList { get; set; }
        public SelectList PaymentMethodList { get; set; }
        public SelectList AccountList { get; set; }
        public SelectList BranchList { get; set; }
        public SelectList MyCurrencyList { get; set; }
        public Nullable<int> SubLedger_ID { get; set; }
        public SelectList MySubList { get; set; }
        public Nullable<int> BussinesUnit_ID { get; set; }
        public SelectList MyBuList { get; set; }
        public SelectList OrderList { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public SelectList MyDepartmentList { get; set; }
        public List<SalesReceiptDetailsVM> myDetailsVM { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [Required]
        public int Emp_Id { get; set; }
        [Required]
        public SelectList EmpList { get; set; }

        public List<CustomerVM> cusVM { get; set; }
    }
}