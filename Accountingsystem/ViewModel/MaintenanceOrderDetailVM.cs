using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class MaintenanceOrderDetailVM
    {
        public int Product_Id { get; set; }
        public SelectList ProductList { get; set; }
        public int Master_Id { get; set; }
        public int CustomerID { get; set; }
        public int Discount { get; set; }

    }
}