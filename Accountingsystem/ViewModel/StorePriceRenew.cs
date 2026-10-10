using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class StorePriceRenew
    {
        public decimal percentage { get; set; }
        public List<StoreSalesPricesProduct> ListProduct { get; set; }

        public int  StoreId { get; set; }
        public List<Category> CatList { get; set; }

    }
}