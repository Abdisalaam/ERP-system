using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class RecalPurchInvoiceMasterVM
    {

        public string Batch_Number { get; set; }
     
        public string Recall_MPurch_Serial { get; set; }
     
        public int Master_Id { get; set; }
   
   
       // [DataType(DataType.Date)]
        public string Register_Date { get; set; }
       

       // [DataType(DataType.Date)]
        public string Due_Date { get; set; }
   
        public decimal Inv_TotalAmount { get; set; }
      
        public decimal Inv_TotalTax { get; set; }
     
        public decimal Inv_TotalCost { get; set; }
   
        public decimal Inv_TotalDiscount { get; set; }
  
        public int SelectedCurrencyId { get; set; }
        public SelectList CurrenyList { get; set; }

        public int SelectedProductId { get; set; }
        public SelectList MyProductsList { get; set; }
    
        public int SelectedDocumentTypeId { get; set; }
        public SelectList MyDocumentTypeList { get; set; }
   
        public Nullable<TransactionState> PI_State { get; set; }
  
        public Nullable<int> selectedStoreStore_Id { get; set; }
        public SelectList mystoreList { get; set; }
   
        public int SupplierSuppId { get; set; }
        public SelectList SupplierList { get; set; }

        public int CompanyComp_Id { get; set; }
        public SelectList CompanyList { get; set; }
     
        public int selectedPurInvoiceMasterMaster_Id { get; set; }

        public SelectList purchInvList { get; set; }
      
        public Nullable<decimal> Master_Discount { get; set; }
        public int store_Quantity { get; set; }
        public List<RecalPurchInvoiceDetailsVM> myDetailsVM { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public Nullable<int> SelectedPaymentMethodId { get; set; }
        public SelectList PaymentMethodList { get; set; }
    }
}