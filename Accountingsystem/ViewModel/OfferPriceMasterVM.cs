using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class OfferPriceMasterVM
    {
        public int isCopy { get; set; }
        public SelectList TaxList { get; set; }
        public List<string> Taxes { get; set; }
        public int OfferMas_Id { get; set; }
       
 
        public string Serial_Number { get; set; }
        public string Description { get; set; }
        [Required]
        public String Offer_Date { get; set; }
        [Required]
        public int SelectedCurrency_Id { get; set; }
        public SelectList CurrencyList { get; set; }
        [Required]
        public int SelectedCustomer_Id{ get; set; }
        [Required]
        public SelectList CustomerList { get; set; }

        [Required]
        public int SelectedCompany_Id { get; set; }     

        public SelectList CompanyList { get; set; }
        public int Tax_ID { get; set; }

        public List<InvoiceMasterDetailsVM> InvoiceMasterdetailOffer { get; set; }
      
        public String OfferDeadLine { get; set; }
        [Required]
      
        public decimal Master_Discount { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Total_Tax { get; set; }
        [Required]
        public int SelectedStore_ID { get; set; }
        [Required]
        public SelectList StoreList { get; set; }
        public Nullable<decimal> Exchange_Rate { get; set; }
        public string Reference { get; set; }
        public Nullable<int> Payment_Policy { get; set; }
        public int RevenueAccount_Id { get; set; }
        [Required]
        public int Emp_Id { get; set; }
        public SelectList EmpList { get; set; }
        public Nullable<int> Branch_Id { get; set; }
        public SelectList BranchList { get; set; }
        public SelectList AccountList { get; set; }
        public CustomerVM Cust { get; set; }
        public Nullable<int> DepartSubledger_Id { get; set; }
        public Nullable<int> BussinesSubledger_Id { get; set; }
        public Nullable<int> Subledger_Id { get; set; }
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }
        public string Notes { get; set; }
        public decimal Fixed_Discount { get; set; }

    }
}