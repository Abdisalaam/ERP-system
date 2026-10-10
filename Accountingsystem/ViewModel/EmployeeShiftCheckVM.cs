using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class EmployeeShiftCheckVM
    {
       public Employee Emp { get; set; }
       public bool IsActive { get; set; }
        public int EmployeeId { get; set; }
    }
}