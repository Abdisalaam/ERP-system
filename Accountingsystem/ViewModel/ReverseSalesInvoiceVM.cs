using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class ReverseSalesInvoiceVM
    {
        public int Master_Id { get; set; }
        
        public string Serial_Number { get; set; }

        
        public string Description { get; set; }


      
       
        public string Invoice_Date { get; set; }

       
       
        public string Due_Date { get; set; }
     
        public string GL_Date { get; set; }

        
        public decimal Master_Discount { get; set; }


       
        public string Invoice_Currency { get; set; }


       
        public decimal Total_Amount { get; set; }

    
        public decimal Total_Tax { get; set; }
      
        public decimal Total_Cost { get; set; }
     
        public Document_Type Document_Type { get; set; }

  
        public TransactionState Invoice_State { get; set; }
  

        public int Store_ID { get; set; }
  
      

        public string SubLedger { get; set; }
    

        public int SelectedCompany_Id { get; set; }
        public SelectList MyCompanyList { get; set; }
      
        public int SelectedCustomerId { get; set; }

        public SelectList MyCustomerList { get; set; }

       
        public int SelectedCurrencyId { get; set; }
        public SelectList MyCurrencyList { get; set; }

     
        public int SelectedBussinesUnitId { get; set; }
        public SelectList MyBussinesUnitList { get; set; }


        public List<ReveInvoiceDetailsVM> myDetailsVM { get; set; }
     

        public int SelectedChartOfAccountsId { get; set; }
        public SelectList MyChartOfAccountsList { get; set; }
     
        public int SelectedProductId { get; set; }
        public SelectList MyProductsList { get; set; }

       
        public int selectedStoreStore_Id { get; set; }
        public SelectList mystoreList { get; set; }


     
        public int selectedrevInvoiceMasterMaster_Id { get; set; }
        public SelectList purchInvList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}