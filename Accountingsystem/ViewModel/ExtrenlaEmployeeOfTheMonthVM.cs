using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class ExtrenlaEmployeeOfTheMonthVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Models.ExtrnalSalesEmployee> ExtrnalSalesEmployees { get; set; }

    }
}