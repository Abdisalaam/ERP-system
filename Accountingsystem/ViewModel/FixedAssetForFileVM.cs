using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class FixedAssetForFileVM
    {
        public String  FAsset_Descriptions { get; set; }
        public int SelectedFixedAsset_TypeId { get; set; }
        public double FAsset_PurchasePrices { get; set; }
        public double Complex { get; set; }
        public double BookVlaue { get; set; }

        public DateTime FAsset_WorkingDates { get; set; }
        public DateTime OpenBalanceWorkDate { get; set; }
        public int FAsset_AgeMonths { get; set; }
        public double JunckValue { get; set; }
        
    }
}