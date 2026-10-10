using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class StoreSalesPricesProduct
    {
        public int count { get; set; }
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
        public int batchId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public ProductBatches batch { get; set; }
        public SelectList CatList { get; set; }
        public decimal CostPrice { get; set; }

        public decimal SalesPriceCommon { get; set; }
        public decimal taxPercentage { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal SalesPriceWithTax  { get; set; }



    }
}