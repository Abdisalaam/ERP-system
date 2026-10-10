using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProjectInvoiceMasterVM
    {
        public int PInvoiceMaster_ID { get; set; }
      
       
        public string Serial_Number { get; set; }
    
        public int Project_Id { get; set; }
        public SelectList ProjectList { get; set; }
      
        public string Project_Code { get; set; }
      
        public string Project_Name { get; set; }
    
        public string Project_Description { get; set; }
    
        public string Invoice_Description { get; set; }
      
        public string Project_Location { get; set; }
    
        public string DeadLine { get; set; }
       
        public string StartTime { get; set; }
       
        public decimal Estimate_Amount { get; set; }
     
        public int CustomerCustomer_Id { get; set; }
    
        public string Customer_Name { get; set; }

     
       
        public int Payment_Method { get; set; }
      
        public string Payment_Method_Name { get; set; }
    
        public Nullable<int> Revenu_Id { get; set; }
       
        public  string invoicedate { get; set; }
        public Nullable<int> MasterTempMaster_Id { get; set; }
      
        public decimal MasterDiscount { get; set; }
     
        public Nullable<decimal> FixedDiscount { get; set; }
        public decimal allTotals { get; set; }
        public TransactionState Project_State { get; set; }
     
        public Nullable<int> Emp_Id { get; set; }
    
        public SelectList EmpList { get; set; }
    
    }
}