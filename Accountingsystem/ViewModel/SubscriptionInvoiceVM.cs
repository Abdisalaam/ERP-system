using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SubscriptionInvoiceVM
    {
        public int Inv_ID { get; set; }
      
        public string Serial_Number { get; set; }
        public string Subscrip_Code { get; set; }
        public string Subscrip_Description { get; set; }
        public string DeadLine { get; set; }
        public string StartTime { get; set; }
        public int SubscribtionId { get; set; }

        public string InvoiceDate { get; set; }
        public decimal Estimate_Amount { get; set; }
        public Nullable<int> Customer_Id { get; set; }
    
        public string Customer_Name { get; set; }

        public int Payment_Method { get; set; }
      
        public decimal Pay_Amount { get; set; }
        public SelectList CustomerList { get; set; }
      
        public SelectList paymentMethodList { get; set; }
    

        public int Sub_Type_ID { get; set; }
        public int Account_ID { get; set; }
        public string checkNumber { get; set; }
        public SelectList AccountList { get; set; }





        public Nullable<int> Subscrip_BU { get; set; }
        public SelectList BUList { get; set; }
        public Nullable<int> Subscrip_Pranch { get; set; }
        public SelectList BranshList { get; set; }

        public Nullable<int> Subledger_ID { get; set; }
        public SelectList SubledgerList { get; set; }

        public Nullable<int> Department_Id { get; set; }
        public SelectList DepartmentList { get; set; }
        public string Note { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}