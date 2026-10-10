using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class EmpLoyeeLoanVM
    {
       public List<Accounts> Accounts { get; set; }
        public Company company { get; set; }
    }
}