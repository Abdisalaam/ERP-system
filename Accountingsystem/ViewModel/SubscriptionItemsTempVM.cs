using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SubscriptionItemsTempVM
    {
        public int Id { get; set; }
        public int SubTempID { get; set; }
        public string Item_Name { get; set; }
        public Nullable<int> Item_Quantity { get; set; }
        public Nullable<decimal> Item_price { get; set; }
    }
}