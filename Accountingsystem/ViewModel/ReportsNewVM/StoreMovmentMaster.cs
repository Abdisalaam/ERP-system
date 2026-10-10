using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class StoreMovmentMaster
    {
        public string endDate { get; set; }
        public int storeId { get; set; }
        public Store store { get; set; }
        public SelectList storeList { get; set; }
        public List<StoreMovmentDateil> MovmentDateilList { get; set; }
        public Company company { get; set; }

    }
}