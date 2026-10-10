using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class Employee_SalesVM
    {
        public int Id { get; set; }
        public Nullable<int> Employee_ID { get; set; }
        public Nullable<decimal> Net_Sales { get; set; }
        public Nullable<decimal> Collected { get; set; }
        public Nullable<decimal> Target_Sales { get; set; }
        public Nullable<decimal> Target_Collected { get; set; }
    }
}