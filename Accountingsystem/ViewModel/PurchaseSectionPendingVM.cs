using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class PurchaseSectionPendingVM
    {
        public string Serial { get; set; }
        public int MasterID { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
  
    }

}