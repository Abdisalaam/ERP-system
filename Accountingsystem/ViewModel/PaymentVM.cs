using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Accountingsystem.Models;


namespace Accountingsystem.ViewModel
{
    public class PaymentVM
    {
        public int Pay_ID { get; set; }


        
        public int Pay_Currency_ID { get; set; }
        public SelectList myPayCurrency { get; set; }

        public int PayComp_ID { get; set; }
        public SelectList MyCompanyList { get; set; }

      
        public Decimal Pay_Amount { get; set; }

  
        public Nullable<decimal> Exchange_Rate { get; set; }

     
        public string Pay_Number { get; set; }

    
        public int PayBank_Account { get; set; }


 
        public string Rec_Description { get; set; }

        
       
        //[DataType(DataType.Date)]
        public string PayGl_Date { get; set; }

   
        //[DataType(DataType.Date)]
        public string Pay_Date { get; set; }

        public TransactionState Pay_State { get; set; }
      

        public int Pay_BU { get; set; }

        public SelectList myPayBu { get; set; }

  
        public Decimal TotalAmountAfMat { get; set; }

  
        public int Supplier_Id { get; set; }
       
  
        public string Pay_TotalCheck { get; set; }
        public string RecCheck_Number { get; set; }

        public SelectList mySupplierId { get; set; }

    
        public int SelectedChartOfAccountId { get; set; }

        public SelectList MyChartOfAccountList { get; set; }
        public int SelectedPurchaseInvoiceID { get; set; }
     
        public SelectList PurchaseInvoiceList { get; set; }

        public int SelectedPurInvoiceMaster_Id { get; set; }
  
        public SelectList MyPurInvoiceMaster { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public SelectList EmpList { get; set; }
        public int Emp_Id { get; set; }

    }
}