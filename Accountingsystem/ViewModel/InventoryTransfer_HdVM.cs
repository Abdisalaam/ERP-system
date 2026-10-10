using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class InventoryTransfer_HdVM
    {
        public int TransferHd_Id { get; set; }
   
        //[DataType(DataType.Date)]
        public string Date { get; set; }

  
        public string InvTransfere_Notes { get; set; }
      
        public decimal Total_Cost { get; set; }
        public TransactionState Tranfer_State { get; set; }


        public int SelectedCurrencyId { get; set; }

        public SelectList MyCurrencyList { get; set; }

       
        public int SelectedCompanyId { get; set; }

        public SelectList MyCompanyList { get; set; }


        // public int SelectStore_Id { get; set; }

        // public int SelectStore_Code { get; set; }


       
        public int SelectedBusiness_UnitId { get; set; }

        public SelectList MyBusiness_UnitIdList { get; set; }



       
        public int SelectedSourceStore_Id { get; set; }

        public SelectList MySoursStoreList { get; set; }


     
        public int SelectedDestStore_Id { get; set; }

        public SelectList MyDestStoreList { get; set; }



      
        public int SelectedProductId { get; set; }
        public SelectList MyProductsList { get; set; }
        public string Inv_Code { get; set; }


        public List<InventoryTransfer_DetailVM> myDetailsVM { get; set; }

    }
}