using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ReverseSalesReceiptMasterVM
    {
        public int Master_Id { get; set; }
    
        public string Serial_Number { get; set; }

        public string Description { get; set; }

    

        public String Invoice_Date { get; set; }

        public decimal Master_Discount { get; set; }

        public string Invoice_Currency { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }
        public TransactionState Invoice_State { get; set; }
        public Nullable<decimal> Exchange_Rate { get; set; }
       
        public Nullable<int> Customer_Id { get; set; }
      
      
        public Nullable<int> PaymentMethod_Id { get; set; }
        public int Account_Id { get; set; }
      
        public int Branch_Id { get; set; }
        public string Reference { get; set; }
        public string ApprovalCode { get; set; }
      
        public int Store_ID { get; set; }
    
        public int Company_Id { get; set; }
        public decimal Total_Payment_Method_Cost { get; set; }
        public SelectList MyCompanyList { get; set; }
        public SelectList MyCustomerList { get; set; }
        public SelectList MyCurrencyList { get; set; }
        public SelectList MyBussinesUnitList { get; set; }
        public SelectList MyChartOfAccountsList { get; set; }
        public SelectList MyProductsList { get; set; }
        public SelectList mystoreList { get; set; }
        public SelectList purchInvList { get; set; }
  
        public int salesRecipt_Id { get; set; }
        public SelectList PaymentMethodList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}