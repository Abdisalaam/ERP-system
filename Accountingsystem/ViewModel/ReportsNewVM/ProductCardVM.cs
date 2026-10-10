using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProductCardVM
    {
        public List<ProductQuantityVM> ProductList { get; set; }
        public string FromTo { get; set; }
        public string To { get; set; }
        public int Product_Id { get; set; }
        public Product product { get; set; }
        public SelectList ProducSelectionList { get; set; }
        public int Store_Id { get; set; }
        public Store Store { get; set; }
        public SelectList StorList{ get; set; }

        public decimal TotalStartQuantity { get; set; }
        public decimal TotalStartProCost { get; set; }
        public decimal TotalStartCost { get; set; }


        public decimal TotalImportQuantity { get; set; }
        public decimal TotalImportProCost { get; set; }
        public decimal TotalImportCost { get; set; }


        public decimal TotalExportQuantity { get; set; }
        public decimal TotalExportProCost { get; set; }
        public decimal TotalExportCost { get; set; }
        public decimal TotalAllQuantity { get; set; }
        public decimal TotalAllProCost { get; set; }
        public decimal TotalAllCost { get; set; }
        public Company company { get; set; }


    }
}