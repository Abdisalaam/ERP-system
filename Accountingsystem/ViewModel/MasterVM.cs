using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MasterVM
    {
        public int isCopy { get; set; }
        
        public int Master_Id { get; set; }
    

        public SelectList TaxList { get; set; }

        public string Serial_Number { get; set; }
        public List<string> Taxes { get; set; }

        public string OfferCode { get; set; }
        public string Patch_Number { get; set; }
        public string Description { get; set; }

       
        [Required]        
     
        public string Invoice_Date { get; set; }
        [Required]
        public string Due_Date { get; set; }
  
        public string GL_Date { get; set; }
        [Required]
        
        public decimal Master_Discount { get; set; }
        public decimal Fixed_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        [Required]
        public int Customer_Id { get; set; }
        public decimal Total_Amount { get; set; }
        [Required]
        public int Company_Id { get; set; }
     
        public decimal Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }
        public Document_Type Document_Type { get; set; }
        public TransactionState Invoice_State { get; set; }
        [Required]
        public int SelectedCompany_Id { get; set; }
        public SelectList MyCompanyList { get; set; }
        [Required]
        public int SelectedCustomerId { get; set; }
        public SelectList MyCustomerList { get; set; }
        [Required]
        public int SelectedCurrencyId { get; set; }
        public SelectList MyCurrencyList { get; set; }
        public int SelectedBussinesUnitId { get; set; }
        public SelectList MyBussinesUnitList { get; set; }

        [Required]
        public int SelectedStoreId { get; set; }
        public SelectList MyStoresList { get; set; }
        public SelectList AccountList { get; set; }
        public SelectList BranchList { get; set; }
        [Required]
        public SelectList EmpList{ get; set; } 
        public List<DetailsVM> myDetailsVM { get; set; }

        public Nullable<int> Tax_ID { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
       
        public string Reference { get; set; }
        [Required]
        public int RevenueAccount_Id { get; set; }
        [Required]
        public int Payment_Policy { get; set; }

        [Required]
        public int Emp_Id { get; set; }
        public Nullable<int> Branch_Id { get; set; }
        public SelectList customerList { get; set; }
        public Nullable<int> order_Id { get; set; }
        public SelectList OrderList { get; set; }
        public Nullable<int> Department_ID { get; set; }
        public SelectList DepList { get; set; }
        public Nullable<int> BussinesUnit_ID { get; set; }
        public SelectList BUList { get; set; }
        public Nullable<int> SubLedger_ID { get; set; }
        public SelectList SubList { get; set; }
        public string Notes { get; set; }

    }
}