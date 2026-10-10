using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PurMasterVM
    {
        public int  isCopy { get; set; }
        public int Master_Id { get; set; }
       
     
        public string Serial_Number { get; set; }
     
        public string Purch_Order_Code { get; set; }


   
        public string Description { get; set; }

        //  [DataType(DataType.Date)]
   
       
        public string Invoice_Date { get; set; }

        //  [DataType(DataType.Date)]
    
        public string Due_Date { get; set; }
     
        //  [DataType(DataType.Date)]
   
        public string GL_Date { get; set; }


        // [Required(ErrorMessage = "يجب ادخالي الخصم")]
    
        public decimal Master_Discount { get; set; }

        public string Invoice_Currency { get; set; }
     
        public int Customer_Id { get; set; }

   
        public decimal Total_Amount { get; set; }

    
        public int Company_Id { get; set; }

       
        public decimal Total_Tax { get; set; }

       
        public decimal Total_Cost { get; set; }

       
        public Document_Type Document_Type { get; set; } //Enum

     
        public TransactionState Invoice_State { get; set; } //Enum

  
        public bool Tax_Type { get; set; }

        
        public int SelectedCompany_Id { get; set; }
        public SelectList MyCompanyList { get; set; }

      
        public Nullable<int> SelectedSupplierId { get; set; }
       
     
        public SelectList MySupplierList { get; set; }

       
        public int SelectedCurrencyId { get; set; }
        public SelectList MyCurrencyList { get; set; }


        public Nullable<int> SelectedBussinesUnitId { get; set; }
        public SelectList MyBussinesUnitList { get; set; }
   
   
        public int SelectedProductId { get; set; }
        public SelectList MyProductsList { get; set; }
  
        public int SelectedAssetId { get; set; }
        public SelectList MyAssetList { get; set; }
    
      
        public int SelectedStoreId { get; set; }
      
        public SelectList MyStoresList { get; set; }


   
        public int SelectedAccount_Id { get; set; }
        public SelectList AccountsList { get; set; }

        public Nullable<int> SelectedBU_Id { get; set; }
        public SelectList BU_List { get; set; }

        public SelectList SubledgerList { get; set; }
        public int SelectedSubledger_Id { get; set; }

  
        public decimal Exchange_Rate { get; set; }

     
        public string ItemDesc { get; set; }
        public List<PurchaseDetailsVM> myDetailsVM { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public Nullable<int> SelectedEmp_Id { get; set; }
        public SelectList MyEmpList { get; set; }
    
        public Nullable<int> SelectedBranch_Id { get; set; }
        public SelectList MyBranchList { get; set; }
        public string Reference { get; set; }
        public int order_Id { get; set; }


        public int Payment_Policy { get; set; }
        public string Supp_Inv_Serial { get; set; }

      
        public decimal Fixed_Discount { get; set; }

 
        public Nullable<int> PaymentMethod_Id { get; set; }

        public SelectList PaymentMethodList { get; set; }

    }
}