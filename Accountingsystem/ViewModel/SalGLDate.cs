using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SalGLDate
    {
      
        public string date { get; set; }
        public List<Employee> EmployeeList { get; set; }
    }
}