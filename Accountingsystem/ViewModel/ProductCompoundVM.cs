using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProductCompoundVM
    {
        public int ProductId { get; set; }
        public SelectList ProductList { get; set; }
        public decimal quantity { get; set; }
        public int Main_Product_Id { get; set; }

    }
}