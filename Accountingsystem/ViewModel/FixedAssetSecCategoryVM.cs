using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class FixedAssetSecCategoryVM
    {
        public int FAsset_CatIDs { get; set; }
       
        public string FAssetCat_Names { get; set; }

        public int SelectedFixedAssetTypeIds { get; set; }
  
        public SelectList FixedAssetTypeLists { get; set; }
   
        public string FixedAssCat_codes { get; set; }
    }
}