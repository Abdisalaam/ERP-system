using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class FixedAssetTypeVM
    {
        public int FAsset_TypeIDs { get; set; }
 
        public string FAsset_TypeNames { get; set; }

        public string FixedAssetTyp_codes { get; set; }
    }
}