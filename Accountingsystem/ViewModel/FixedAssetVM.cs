using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Accountingsystem.ViewModel
{
    public class FixedAssetVM
    {

        public int Fasset_IDs { get; set; }

        public string FAsset_BuyDates { get; set; }

        public string FAsset_SellDates { get; set; }

      //  [DataType(DataType.Date)]
        public string FAsset_WorkingDates { get; set; }

        public Nullable<double> FAsset_PurchasePrices { get; set; }
     
        public int FAsset_AgeMonths { get; set; }
     
        public string FAsset_Descriptions { get; set; }
        public Nullable<bool> FAsset_Disposes { get; set; }
     
        public string FAsset_Codes { get; set; }

 
        public string SelectedFixedAssetSecCategoryId { get; set; }

        public SelectList MyCategoryList { get; set; }
        public string FAsset_Types { get; set; }
      
        public string SelectedFixedAsset_TypeId { get; set; }

        public SelectList MyFAssetType { get; set; }
        public string FAssetCat_names { get; set; }

   
        public double JunckValue { get; set; }

   
        public Nullable<int> FAsset_BUs { get; set; }
        public SelectList myBUnit { get; set; }
  
        public Nullable<int> selectedFAsset_Employees_ID { get; set; }
        public SelectList myEmployee { get; set; }

     
        public Nullable<int> selectedCompany_Id { get; set; }
        public SelectList myCompaney { get; set; }
     
        public string Currency { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
 
        public Nullable<int> SubLedger_ID { get; set; }
        public SelectList SubledgerList { get; set; }
 
        public Nullable<int> DepartmentID { get; set; }
        public SelectList DepListList { get; set; }

        public Nullable<int> BranchID { get; set; }
        public SelectList BranshList { get; set; }


    }
}