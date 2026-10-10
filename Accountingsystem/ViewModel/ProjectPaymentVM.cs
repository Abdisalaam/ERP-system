using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProjectPaymentVM
    {
        public int? ProjectPaymentId { get; set; }
      
        public int Expense_Id { get; set; }
     
        public SelectList MyExpenseList { get; set; }
        public string Expense_Name { get; set; }
       
        public decimal Pay_Amount { get; set; }

    
        public string ProjectPay_Code { get; set; }
        public TransactionState ProjectPay_State { get; set; }

  
        public int Project_Id { get; set; }
     
        public SelectList MyProjectList { get; set; }
        
        public int? Revenue_Id { get; set; }
 
        public SelectList MyRevenueList { get; set; }
        public string Revenue_Name { get; set; }
     
       
        public string Date { get; set; }
        public decimal Total_Tax { get; set; }
        public Nullable<int> Selected_Tax_Id { get; set; }
        public SelectList taxList { get; set; }
       
        public SelectList SupplierList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public Nullable<int> SupplierID { get; set; }
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Revenue_Id ==null&& SupplierID==null )
        //    {
        //        yield return new ValidationResult("برجاء اختر الصندوق او المورد", new List<string> { "Description" });
        //    }
        //}
       
        public string Description { get; set; }
        public Nullable<int> ProjectSub_Id { get; set; }
        public Nullable<int> ProjectSubSub_Id { get; set; }
        public Nullable<int> ProjectClause_Id { get; set; }
        public SelectList ProjectSubList { get; set; }
        public SelectList ProjectSubSubList { get; set; }
        public SelectList ProjectClauseList { get; set; }
    }
}