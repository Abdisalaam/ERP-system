using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class EmployeeShiftVM
    {
       public ShiftCoding shiftCodeing { get; set; }
        public List<EmployeeShiftCheckVM> EmployeeList { get; set; }
    }
}