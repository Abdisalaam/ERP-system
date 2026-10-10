using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class InventorytransferHdDetailsVM
    {
       
        public int TransferDetail_Id { get; set; }

        public decimal Product_Qty { get; set; }

  
        public int InventoryTransfer_Id { get; set; }
  
        public decimal Detail_TotalCoct { get; set; }


        public int Product_ID { get; set; }

        public Nullable<int> Batch_ID { get; set; }   
       
         public SelectList MyBatchsList { get; set; }       
       
        public SelectList MyProductsList { get; set; }

        public int storeId { get; set; }

    }
}