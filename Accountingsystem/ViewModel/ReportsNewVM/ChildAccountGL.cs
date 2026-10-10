using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ChildAccountGL
    {
        public Accounts Account { get; set; }
        public FixedAsset fixedAsset { get; set; }

        public SelectList fixedAssetList { get; set; }
        public int fixedAsset_Id { get; set; }
        public String FromDate { get; set; }
        public String Todate { get; set; }
        public Company company { get; set; }
        public List<ChildAccountGLDetail> details { get; set; }
        public CustomerStatisticVM statistic { get; set; }
        public int accountId { get; set; }
        public SelectList AccountList { get; set; }
        public decimal AmountBefor { get; set; }

        public decimal Creditbefor { get; set; }
        public decimal Depitbefor { get; set; }
        public bool penddingDoc { get; set; }
    }
}