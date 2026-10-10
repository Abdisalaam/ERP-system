using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProductQuantityListVM
    {
      public List<ProductQuantityVM> ProductList { get; set; }
        public string FromTo { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
        public SelectList ProducSelectionList { get; set; }
        public decimal TotalStartQuantity { get; set; }
        public decimal TotalImportQuantity { get; set; }

        public decimal TotalExportQuantity { get; set; }
        public decimal TotalAll { get; set; }
        public Company company { get; set; }
    }
}