using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class AssetsReportVM
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<FixedAssetOpeningBalanceVM> AssetDetails { get; set; }
        public List<FixedAssetTypeForReportVM> AssetType { get; set; }
        public Company company { get; set; }
        

    }
}