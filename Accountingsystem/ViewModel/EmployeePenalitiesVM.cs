using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class EmployeePenalitiesVM
    {
        public Employee employee { get; set; }
        public int employee_Id { get; set; }
  
        public string DateOfMounth { get; set; }
        public List<RewardAndPenaltiesVM>  PenaltiesList  { get; set; }

    }
}